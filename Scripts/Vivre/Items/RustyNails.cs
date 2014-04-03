using Server.Mobiles;
using Server.Network;
using Server.Gumps;
using Server.Targeting;
using Server.Commands;
using System;

namespace Server.Items
{
    [Flipable(0x102E, 0x102F)]
    public class RustyNails : Item
    {

        [Constructable]
        public RustyNails()
            : base(0x102E)
        {
            Name = "Clous rouillés";
            Weight = 2.0;
        }

        public RustyNails(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 2) || !from.InLOS(this))
            {
                from.SendLocalizedMessage(501816);
                return;
            }

            from.SendMessage("Dans quoi souhaitez-vous l'enfoncer?");
            from.BeginTarget(1, false, TargetFlags.Harmful, new TargetCallback(OnTarget));
        }

        public void OnTarget(Mobile from, object obj)
        {
            if (obj is PlayerMobile)
            {
                PlayerMobile pm = (PlayerMobile)obj;

                pm.Hits -= 1;
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "Vous piquez " + pm.Name);
                pm.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "Vous êtes piqué par " + from.Name);

                if (Utility.Random(20) == 7)
                {
                    pm.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "La rouille empoisonne votre sang");
                    pm.Poison = Poison.Lesser;
                }

                this.Delete();
                return;
            }

            if (obj is BaseVoodooDoll)
            {
                BaseVoodooDoll doll = (BaseVoodooDoll)obj;

                Mobile victim = doll.Possessed;
                if (victim == null)
                {
                    from.SendMessage("Aucune âme n'hante le corps de cette poupée");
                    return;
                }
                if (victim.AccessLevel > from.AccessLevel)
                {
                    from.SendMessage("Attention il/elle pourrait se retourner contre vous...");
                    return;
                }
                if (!victim.Alive)
                {
                    from.SendMessage("Faire mal à un mort ne vous servira à rien, hélas");
                    return;
                }
                if (victim.Map == Map.Internal)
                {
                    from.SendMessage("Faire mal à un jouer non connecté ne vous servira à rien, hélas");
                    return;
                }
                if (Utility.Random(10) == 9)
                {
                    from.SendMessage("Vous n'avez plus de clous rouillés");
                    this.Delete();
                }
                from.SendGump(new EffectGump(from, victim, doll));
            }
        }


        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (Name == null)
                Name = "Clous rouillés";
        }
    }


    public class EffectGump : Gump
    {
        private Mobile m_From;
        private Mobile m_Suffer;
        private BaseVoodooDoll m_Doll;
        private const int Blanco = 0xFFFFFF;
        private const int Azul = 0x8080FF;

        public void AddButtonLabeled(int x, int y, int buttonID, string text)
        {
            AddButton(x, y - 1, 4005, 4007, buttonID, GumpButtonType.Reply, 0);
            AddHtml(x + 35, y, 240, 20, Color(text, Blanco), false, false);
        }
        public int GetButtonID(int type, int index)
        {
            return 1 + (index * 15) + type;
        }
        public string Color(string text, int color)
        {
            return string.Format("<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text);
        }
        public EffectGump(Mobile from, Mobile victim, BaseVoodooDoll voodoodoll)
            : base(600, 50)
        {
            m_From = from;
            m_Suffer = victim;
            m_Doll = voodoodoll;
            Closable = true;
            Dragable = true;
            AddPage(0);
            //AddBackground( 0, 65, 130, 360, 5054);
            //AddAlphaRegion( 10, 70, 110, 350 );
            //AddImageTiled(10, 70, 110, 20, 9354);
            AddBackground(0, 65, 240, 360, 5054);
            AddAlphaRegion(10, 70, 220, 350);
            AddImageTiled(10, 70, 220, 20, 9354);
            AddLabel(13, 70, 200, "Liste des souffrances");
            AddImage(210, 0, 10410);
            AddImage(210, 305, 10412);
            AddImage(210, 150, 10411);

            AddButtonLabeled(10, 90, GetButtonID(1, 1), "Bras");
            AddButtonLabeled(10, 115, GetButtonID(1, 2), "Main");
            AddButtonLabeled(10, 140, GetButtonID(1, 3), "Cuisse");
            AddButtonLabeled(10, 165, GetButtonID(1, 4), "Pied");
            AddButtonLabeled(10, 190, GetButtonID(1, 5), "Ventre");
            AddButtonLabeled(10, 215, GetButtonID(1, 6), "Dos");
            AddButtonLabeled(10, 240, GetButtonID(1, 7), "Nez");
            AddButtonLabeled(10, 265, GetButtonID(1, 8), "Yeux");
            AddButtonLabeled(10, 290, GetButtonID(1, 9), "Bouche");
            AddButtonLabeled(10, 315, GetButtonID(1, 10), "Fesses");

        }
        public override void OnResponse(NetState sender, RelayInfo info)
        {
            int val = info.ButtonID - 1;
            if (val < 0)
                return;

            Mobile from = m_From;
            Mobile victim = m_Suffer;
            BaseVoodooDoll voodoodoll = m_Doll;
            int type = val % 15;
            int index = val / 15;

            string frommsg = "Vous piquez ";
            string victimmsg = "*Vous ressentez une douleur ";
            string publicmsg = null;
            int sound = victim.Female ? 814 : 1088;

            switch (index)
            {
                default: break;
                case 1:
                    {
                        frommsg += "son bras";
                        victimmsg += "au bras*";
                        publicmsg = "*Se prend le bras*";
                        victim.Animate(120, 5, 1, true, false, 0);

                        break;
                    }
                case 2:
                    {
                        frommsg += "sa main";
                        victimmsg += "à la main*";
                        if (from.Skills[SkillName.Anatomy].Value > Utility.Random(200))
                        {
                            Item item = victim.FindItemOnLayer(Layer.OneHanded);
                            if (item != null)
                            {
                                item.MoveToWorld(victim.Location);
                                publicmsg = "*Lâche ce qu'il tient sous la douleur*";
                                if (!victim.Mounted)
                                    victim.Animate(120, 5, 1, true, false, 0);
                            }
                            else
                                publicmsg = "*Se prend la main*";
                        }
                        else
                            publicmsg = "*Se prend le bras*";
                        break;
                    }
                case 3:
                    {
                        frommsg += "sa cuisse";
                        victimmsg += "à la cuisse*";
                        publicmsg = "*boite légèrement*";
                        break;
                    }
                case 4:
                    {
                        frommsg += "son pied";
                        victimmsg += "au pied*";
                        publicmsg = "*Sautille*";
                        if (!victim.Mounted)
                            victim.Animate(25, 5, 1, true, true, 3);
                        if (from.Skills[SkillName.Anatomy].Value > Utility.Random(150))
                        {
                            victim.CantWalk = true;
                            Timer.DelayCall(TimeSpan.FromSeconds(3), ChangeWalk, victim);
                        }
                        break;
                    }
                case 5:
                    {
                        frommsg += "son ventre";
                        victimmsg += "au ventre*";
                        if (from.Skills[SkillName.Anatomy].Value > Utility.Random(80))
                        {
                            sound = victim.Female ? 813 : 1087;

                            Point3D p = new Point3D(victim.Location);
                            switch (victim.Direction)
                            {
                                case Direction.North:
                                    p.Y--; break;
                                case Direction.South:
                                    p.Y++; break;
                                case Direction.East:
                                    p.X++; break;
                                case Direction.West:
                                    p.X--; break;
                                case Direction.Right:
                                    p.X++; p.Y--; break;
                                case Direction.Down:
                                    p.X++; p.Y++; break;
                                case Direction.Left:
                                    p.X--; p.Y++; break;
                                case Direction.Up:
                                    p.X--; p.Y--; break;
                                default:
                                    break;
                            }
                            p.Z = victim.Map.GetAverageZ(p.X, p.Y);

                            bool canFit = Server.Spells.SpellHelper.AdjustField(ref p, victim.Map, 12, false);

                            if (canFit)
                            {
                                Puke puke = new Puke();
                                puke.Map = victim.Map;
                                puke.Location = p;
                            }
                        }
                        else
                            publicmsg = "*Se tient le ventre*";
                        if (!victim.Mounted)
                            victim.Animate(32, 5, 1, true, false, 0);
                        break;
                    }
                case 6:
                    {
                        frommsg += "son dos";
                        victimmsg += "Au dos*";
                        publicmsg = "*Courbe l'échine*";
                        if (!victim.Mounted)
                            victim.Animate(32, 5, 1, true, true, 3);
                        break;
                    }
                case 7:
                    {
                        frommsg += "son nez";
                        victimmsg += "au nez*";
                        publicmsg = "*Éternue bruyamment*";
                        sound = victim.Female ? 817 : 1091;
                        if (!victim.Mounted)
                            victim.Animate(32, 5, 1, true, false, 0);
                        break;
                    }
                case 8:
                    {
                        frommsg += "ses yeux";
                        victimmsg += "aux yeux*";
                        publicmsg = "*Pleure abondamment*";
                        sound = victim.Female ? 787 : 1058;
                        break;
                    }
                case 9:
                    {
                        frommsg += "sa bouche";
                        victimmsg += "aux lèvres*";
                        publicmsg = "*Ses lèvres se closent*";
                        sound = victim.Female ? 784 : 1055;
                        if (!victim.Mounted)
                            victim.Animate(33, 5, 1, true, false, 0);
                        if (from.Skills[SkillName.Anatomy].Value > Utility.Random(150))
                        {
                            victim.Squelched = true;
                            Timer.DelayCall(TimeSpan.FromSeconds(5), ChangeSquelch, victim);
                        }
                        break;
                    }
                case 10:
                    {
                        frommsg += "ses fesses";
                        victimmsg += "aux fesses*";
                        publicmsg = "*Pète*";
                        sound = victim.Female ? 792 : 1064;
                        victim.FixedParticles(0x3735, 1, 30, 9503, EffectLayer.Waist);
                        break;
                    }
            }
            from.CloseGump(typeof(EffectGump));

            from.SendMessage(frommsg);

            from.SendMessage("Vous perdez du Karma");
            from.Karma -= Utility.Random(100);

            victim.PrivateOverheadMessage(MessageType.Regular, 0x3B2, false, victimmsg, victim.NetState);

            if (publicmsg != null)
                victim.Say(publicmsg);

            victim.PlaySound(sound);

            voodoodoll.Charges--;

            if (voodoodoll.Charges <= 0)
            {
                from.SendMessage("La poupée perd ses cheveux, redevenant inutile");
                if (voodoodoll is VoodooDollFemale)
                    from.AddToBackpack(new PoupeeFemale());
                else
                    from.AddToBackpack(new PoupeeMale());

                voodoodoll.Delete();
            }

        }
        public void ChangeWalk(Mobile from)
        {
            from.CantWalk = false;
        }

        public void ChangeSquelch(Mobile from)
        {
            from.Squelched = false;
        }
    }

    public class Puke : Item
    {
        private Timer m_Timer;

        [Constructable]
        public Puke()
            : base(Utility.RandomList(0xf3b, 0xf3c))
        {
            Name = "Du Vomi";  // a pile of puke 
            Hue = 0x557;
            Movable = false;

            m_Timer = new ItemRemovalTimer(this);
            m_Timer.Start();

        }

        public override void OnAfterDelete()
        {
            base.OnAfterDelete();

            if (m_Timer != null)
                m_Timer.Stop();
        }

        public override void OnSingleClick(Mobile from)
        {
            this.LabelTo(from, this.Name);
        }

        public Puke(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            this.Delete(); // none when the world starts 
        }
    }
}