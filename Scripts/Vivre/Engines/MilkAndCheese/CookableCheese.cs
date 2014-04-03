using System;
using Server.Network;
using Server.Mobiles;


namespace Server.Items
{
    public enum CheesePaste
    {
        Molle,
        Normale,
        Dure
    }

    public enum CheeseTaste
    {
        Faible,
        Leger,
        Modere,
        Prononce,
        Fort
    }

    public enum CheeseQuality
    {
        Regular,
        Exceptionnal
    }

    public class CookableCheese : Food
    {
        private Milk m_Milk;
        private CheesePaste m_Paste;
        private CheeseTaste m_Taste;
        private CheeseQuality m_Quality;
        private Mobile m_Cook;


        [CommandProperty(AccessLevel.GameMaster)]
        public Milk CheeseMilk
        {
            get { return m_Milk; }
            set { m_Milk = value; CheeseProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public CheesePaste Paste
        {
            get { return m_Paste; }
            set { m_Paste = value; CheeseProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public CheeseTaste Taste
        {
            get { return m_Taste; }
            set { m_Taste = value; CheeseProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public CheeseQuality Quality
        {
            get { return m_Quality; }
            set { m_Quality = value; CheeseProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Cook
        {
            get { return m_Cook; }
            set { m_Cook = value; CheeseProperties(); }
        }

        [Constructable]
        public CookableCheese()
            : base(1, 0x97c)
        {
            Weight = 1.0;
            Stackable = false;
            FillFactor = 2;
            Name = "Fromage";
        }

        public string BaseCheeseName()
        {
            string basename = "Fromage";
            if (Poisoner != null)
                return basename;
            if (Paste == CheesePaste.Molle && Taste == CheeseTaste.Fort)
            {
                basename = "Purin";
            }
            else if (Paste == CheesePaste.Molle && Taste == CheeseTaste.Leger)
            {
                basename = "Caprice";
            }
            else if (Paste == CheesePaste.Dure && Taste == CheeseTaste.Modere)
            {
                basename = "Dormoir";
            }
            else if (Paste == CheesePaste.Dure && Taste == CheeseTaste.Prononce)
            {
                basename = "Accroc";
            }
            else if (Paste == CheesePaste.Normale && Taste == CheeseTaste.Faible)
            {
                basename = "Lait dur";
            }
            else if (Paste == CheesePaste.Normale && Taste == CheeseTaste.Fort)
            {
                basename = "Crottin";
            }
            return basename;
        }

        public void CheeseProperties()
        {
            string nameaddon = "";

            switch (CheeseMilk)
            {
                case (Milk.Cow): nameaddon += " de vache"; break;
                case (Milk.Goat): nameaddon += " de chèvre"; break;
                case (Milk.Sheep): nameaddon += " de brebis"; break;
            }

            if (Poisoner != null)
            {
                Name = BaseCheeseName() + nameaddon;
                return;
            }

            if (Quality == CheeseQuality.Exceptionnal)
            {
                nameaddon += m_Cook != null ? (m_Cook.Female ? " de la mère " : " du père ") + m_Cook.Name : "";
                FillFactor = 4;
            }

            Name = BaseCheeseName() + nameaddon;
        }

        public override bool Eat(Mobile from)
        {

            if (FillHunger(from, FillFactor))
            {
                string tastemsg;
                int RatChances = (int)Taste;
                if (Poisoner == null)
                {
                    if (RatChances > Utility.Random(8))
                    {
                        BaseCreature rat = new Rat();
                        rat.MoveToWorld(from.Location, from.Map);
                        switch (Utility.Random(4))
                        {
                            case 2:
                                {
                                    rat.FocusMob = from;
                                    rat.AI = AIType.AI_Predator;
                                    rat.Say("*Semble jaloux de votre fromage*");
                                    break;
                                }
                            default:
                                {
                                    rat.Controlled = true;
                                    rat.ControlMaster = from;
                                    rat.ControlOrder = OrderType.Come;
                                    rat.Say("*Couinant amoureusement près de vous*");
                                    break;
                                }
                        }
                    }

                    string paste = "Vous mangez";
                    string taste = "";

                    switch (Paste)
                    {
                        case CheesePaste.Molle: paste = "Vous laissez fondre"; break;
                        case CheesePaste.Normale: paste = "Vous dégustez"; break;
                        case CheesePaste.Dure: paste = "Vous grignottez"; break;
                    }

                    switch (Taste)
                    {
                        case CheeseTaste.Faible: taste = "au goût subtil"; break;
                        case CheeseTaste.Leger: taste = "au goût délicat"; break;
                        case CheeseTaste.Modere: taste = "au goût agréable"; break;
                        case CheeseTaste.Prononce: taste = "au goût bien présent"; break;
                        case CheeseTaste.Fort: taste = "au goût persistant"; break;
                    }

                    tastemsg = String.Format("{0} ce fromage {1}", paste, taste);
                    if (Quality == CheeseQuality.Exceptionnal)
                        from.Say("*Semble subjugué{0} par le goût de ce fromage*", from.Female ? "e" : "");
                }
                else
                {
                    tastemsg = "Le goût de la moisissure vous dégoûte au plus haut point";
                    from.PlaySound(from.Female ? 813 : 1087);

                    Point3D p = new Point3D(from.Location);
                    switch (from.Direction)
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
                    p.Z = from.Map.GetAverageZ(p.X, p.Y);

                    bool canFit = Server.Spells.SpellHelper.AdjustField(ref p, from.Map, 12, false);

                    if (canFit)
                    {
                        Puke puke = new Puke();
                        puke.Map = from.Map;
                        puke.Location = p;
                    }
                    from.Say("*Recrache le fromage, l'air malade*");
                }

                from.PrivateOverheadMessage(MessageType.Regular, 0x3B2, false, tastemsg, from.NetState);


                // Play a random "eat" sound
                from.PlaySound(Utility.Random(0x3A, 3));

                if (from.Body.IsHuman && !from.Mounted)
                    from.Animate(34, 5, 1, true, false, 0);

                if (Poison != null)
                    from.ApplyPoison(Poisoner, Poison);

                Consume();

                return true;
            }

            return false;
        }

        public CookableCheese(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.WriteEncodedInt((int)m_Milk);
            writer.WriteEncodedInt((int)m_Paste);
            writer.WriteEncodedInt((int)m_Taste);
            writer.WriteEncodedInt((int)m_Quality);
            writer.Write((Mobile)m_Cook);

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Milk = (Milk)reader.ReadEncodedInt();
            m_Paste = (CheesePaste)reader.ReadEncodedInt();
            m_Taste = (CheeseTaste)reader.ReadEncodedInt();
            m_Quality = (CheeseQuality)reader.ReadEncodedInt();
            m_Cook = reader.ReadMobile();
        }
    }
}
