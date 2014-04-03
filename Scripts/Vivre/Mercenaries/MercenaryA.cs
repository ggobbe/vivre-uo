using System;
using Server.Items;

namespace Server.Mobiles
{
    public class MercenaryA : BaseMercenary
    {
        public static int Price = 50;

        [Constructable]
        public MercenaryA()
            : base()
        {
            Name = "Compagnon";

            // stats
            SetStr(20, 40);
            SetDex(20, 40);
            SetInt(20, 40);

            // equip
            AddItem(new Shirt(Utility.RandomDyedHue()));
            AddItem(new LongPants(Utility.RandomDyedHue()));
            AddItem(new Dagger());
            AddItem(new Shoes(1527));

            // skills
            SetSkill(SkillName.Fencing, 40);
        }

        public MercenaryA(Serial serial)
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
