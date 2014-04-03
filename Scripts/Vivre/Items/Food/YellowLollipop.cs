using System;
using Server.Network;
using Server.Mobiles;


namespace Server.Items
{
    public class YellowLollipop : Food
    {
        [Constructable]
        public YellowLollipop()
            : base(1, 0x468E)
        {
            Name = "Une sucette au citron";
            Weight = 1.0;
            FillFactor = 1;
            Stackable = false;
        }

        public YellowLollipop(Serial serial)
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
                Timer.DelayCall(TimeSpan.FromSeconds(3), ChangeCanEat);
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
            int effect = Utility.Random(30);

            if (effect <= 30 && effect >= 25)
            {
                from.PrivateOverheadMessage(MessageType.Regular, 0x3B2, false, "Vous terminez la sucette... enfin!", from.NetState);
                from.Say("Sa langue est toute jaune!");
                this.Consume();
                return;
            }

            if (effect <= 23 && effect >= 20)
            {
                from.Say("*Grimace*");
                return;
            }

            if (effect <= 18 && effect >= 15)
            {
                from.PrivateOverheadMessage(MessageType.Regular, 0x3B2, false, "*L'acidité du bonbon vous fait frémir*",from.NetState);
                return;
            }

            if (effect <= 13 && effect >= 10)
            {
                from.Say("*Fait cogner le bonbon contre ses dents*");
                return;
            }

            if (effect <= 5 && effect >= 0)
            {
                from.Say("*Slurp!*");
                return;
            }
            return;
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
        }
    }
}
