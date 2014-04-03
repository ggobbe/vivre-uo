using System;
using Server.Network;
using Server.Mobiles;


namespace Server.Items
{
    public class Jellybeans : Food
    {
        private int m_Charges;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Charges
        {
            get { return m_Charges; }
            set { m_Charges = value; InvalidateProperties(); }
        }

        [Constructable]
        public Jellybeans()
            : base(1, 0x468C)
        {
            Name = "Dragées surprises";
            Weight = 1.0;
            FillFactor = 1;
            Stackable = false;
            m_Charges = 10;
        }

        public Jellybeans(Serial serial)
            : base(serial)
        {
        }


        private bool CanEat = true;

        public void ChangeCanEat()
        {
            CanEat = true;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!CanEat)
            {
                from.SendMessage("Laissez-vous le temps de la savourer");
                return;
            }
            base.OnDoubleClick(from);
        }

        public override bool Eat(Mobile from)
        {

            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("Ceci doit être dans votre sac");
                return false;
            }

            // Fill the Mobile with FillFactor
            if (from is PlayerMobile)
            {
                CanEat = false;
                Timer.DelayCall(TimeSpan.FromSeconds(1), ChangeCanEat);
                // Play a random "eat" sound
                from.PlaySound(Utility.Random(0x3A, 3));

                if (from.Body.IsHuman && !from.Mounted)
                    from.Animate(34, 5, 1, true, false, 0);

                if (Poison != null)
                    from.ApplyPoison(Poisoner, Poison);

                if (from.Hunger < 20)
                    from.Hunger += Utility.Random(FillFactor);

                DoEffect(from);

                return true;
            }
            return false;
        }

        public void DoEffect(Mobile from)
        {
            int effect = Utility.Random(15);
            string flavour = "de rien";

            switch(effect)
            {
                case 0: flavour = "de pomme"; break;
                case 1: flavour = "de pêche"; break;
                case 2: flavour = "de poire"; break;
                case 3: flavour = "de melon"; break;
                case 4: flavour = "de citron"; break;
                case 5: flavour = "de lime"; break;
                case 6: flavour = "de raisin"; break;
                case 7: flavour = "de banane"; break;
                case 8: flavour = "de noix de coco"; break;
                case 9: flavour = "de dattes"; break;
                case 10: flavour = "de terre"; break;
                case 11: flavour = "de carotte"; break;
                case 12: flavour = "d'ail"; break;
                case 13: flavour = "de sang"; break;
                case 14: flavour = "de vomi"; break;
            }

            Charges--;
            from.PrivateOverheadMessage(MessageType.Regular, 0x3B2, false, "Celle-ci avait le goût " + flavour, from.NetState);

            if (Charges <= 0)
                this.Consume();
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add("{0} dragées restantes", Charges);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write(m_Charges);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Charges = reader.ReadInt();
        }
    }
}
