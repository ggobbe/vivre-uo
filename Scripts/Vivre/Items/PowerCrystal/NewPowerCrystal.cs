//Incomplet jusqu'à nouvel ordre. Suffisant pour les Irons Beetles
using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.ContextMenus;
using System.Collections.Generic;

namespace Server.Items
{
    public class MysticPowerCrystal : Item
    {
        private int m_Completion;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Completion
        {
            get
            {
                return m_Completion;
            }
            set
            {
                m_Completion = value; InvalidateProperties();
            }
        }


        [Constructable]
        public MysticPowerCrystal()
            : base(0x1F1C)
        {
            Name = "Crystal de Pouvoir";
            Movable = true;
            Stackable = false;
            Completion = 0;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add("{0} %", Completion);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if(!IsChildOf(from.Backpack))
                {
                    from.SendMessage("Le crystal doit être entre vos mains");   
                }
            else if (Completion == 100)
            {
                from.SendMessage("Le crystal ne pourrait absorber davantage de pouvoir!");
            }
            else
            {
                from.SendMessage("Quel pouvoir souhaitez-vous absorber?");
                from.BeginTarget(2, false, TargetFlags.None, new TargetCallback(AbsorbTarget));
            }

            base.OnDoubleClick(from);
        }

        public void AbsorbTarget(Mobile from, object obj)
        {
            if (!(obj is Beetle))
            {
                from.SendMessage("Ce pouvoir vous serait inutile pour la seule utilisation actuelle de ce crystal...");
                return;
            }

            Beetle targ = (Beetle)obj;

            if(!targ.Controlled || targ.ControlMaster != from || !targ.IsBonded)
            {
                from.SendMessage("La créature à qui vous capturerez l'âme doit vous être fortement liée.");
                return;
            }

            bool skill = from.CheckSkill(SkillName.Mysticism, 10, 60);

            if (!skill)
            {
                from.SendMessage("Vous échouez dans votre tentative d'absorber l'âme de cette créature");

                int bad = Utility.Random(1, 100);
                if (bad == 5)
                {
                    targ.Emote("S'effondre au sol, inerte");
                    targ.IsBonded = false;
                    targ.Kill();
                }
                else if (bad > 5 && bad <9)
                {
                    targ.Emote("S'effondre au sol, inanimée");
                    targ.Kill();
                }
                else if (bad < 4)
                {
                    from.Emote("Le Crystal explore");
                    this.Delete();
                }
                else if (bad > 10 && bad < 20)
                {
                    targ.Emote("Faiblit");
                    targ.Str--;
                    if (targ.Str == 0)
                        targ.Delete();
                }
                else if (bad > 20 && bad < 30)
                {
                    targ.Emote("Faiblit");
                    targ.Dex--;
                    if (targ.Dex == 0)
                        targ.Delete();
                }
                else if (bad > 30 && bad < 40)
                {
                    targ.Emote("Faiblit");
                    targ.Int--;
                    if (targ.Int == 0)
                        targ.Delete();
                }
                return;
            }

            from.SendMessage("Vous absorbez une partie de l'âme de la bête");
            this.Completion += Math.Max(Utility.Random(1, 2), (int)from.Skills[SkillName.Mysticism].Value / 10);

            if (Completion > 100)
                Completion = 100;

            if (!from.CheckSkill(SkillName.AnimalLore, 0, 40))
            {
                targ.Emote("Exprime une douleur intense et ne semble pas vous reconnaitre");
                targ.IsBonded = false;
                targ.Controlled = false;
                targ.ControlMaster = null;
            }
            else if (!from.CheckSkill(SkillName.AnimalLore, 0, 120))
            {
                targ.Emote("Exprime une douleur intense et vous observe avec crainte");
                targ.IsBonded = false;
            }
            else
                targ.Emote("Semble ne pas comprendre");
            
            return;
        }

        public MysticPowerCrystal(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
            writer.Write((int)m_Completion);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            int m_Completion = reader.ReadInt();
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from.Alive && (Completion > 20))
                list.Add(new ContextMenus.PowerCrystalMenu(from, this));
        }
    }
}