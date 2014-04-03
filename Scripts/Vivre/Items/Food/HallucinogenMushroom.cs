using System;
using Server.Mobiles;

namespace Server.Items
{
    public class HallucinogenMushroom : Food
    {
        [Constructable]
        public HallucinogenMushroom()
            : base(0x1125)
        {
            Name = "Champignon hallucinogène";
            Stackable = true;
            Weight = 1.0;
            FillFactor = 1;
            Poison = Utility.RandomBool() ? Poison.Regular : Poison.Lethal;
        }

        public override bool Eat(Mobile from)
        {
            if (CheckHunger(from))
            {
                from.PlaySound(Utility.Random(0x3A, 3));

                if (from.Body.IsHuman && !from.Mounted)
                    from.Animate(34, 5, 1, true, false, 0);

                if (from is PlayerMobile || !from.IsHallucinated)
                {
                    PlayerMobile junkie = from as PlayerMobile;
                    junkie.Hallucinating = true;
                    junkie.IncAddiction(new HallucinogenPotion());
                    Timer.DelayCall(TimeSpan.FromMinutes(5), HallucinogenPotion.StopHallucinate, junkie);
                }
                else if (Poison != null)
                    from.ApplyPoison(Poisoner, Poison);

                Consume();

                return true;
            }

            return false;
        }

        public HallucinogenMushroom(Serial serial)
            : base(serial)
        {
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