using System;
using Server.Items;

namespace Server.Mobiles
{
    public class MercenaryB : BaseMercenary
    {
        public static int Price = 500;

        [Constructable]
        public MercenaryB()
            : base()
        {
            Name = "Homme de main";

            // stats
            SetStr(50, 70);
            SetDex(50, 70);
            SetInt(20, 40);

            // equip
            AddItem(new ChainChest());
            AddItem(new ChainCoif());
            AddItem(new ChainLegs());
            AddItem(new Broadsword());
            int hue = Utility.RandomDyedHue();
            AddItem(new Cloak(hue));
            AddItem(new Surcoat(hue));
            AddItem(new Shoes(1527));

            // skills
            SetSkill(SkillName.Swords, 50, 70);
            SetSkill(SkillName.Parry, 30, 50);
            SetSkill(SkillName.Anatomy, 30, 50);
            SetSkill(SkillName.Tactics, 30, 50);
        }

        public MercenaryB(Serial serial)
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
