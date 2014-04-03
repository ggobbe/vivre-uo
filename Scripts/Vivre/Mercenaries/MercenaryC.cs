using System;
using Server.Items;

namespace Server.Mobiles
{
    public class MercenaryC : BaseMercenary
    {
        public static int Price = 1000;

        [Constructable]
        public MercenaryC()
            : base()
        {
            Name = "Mercenaire aguerri";

            // stats
            SetStr(90, 100);
            SetDex(70, 100);
            SetInt(30, 50);

            // equip
            AddItem(new PlateArms());
            Console.WriteLine("1");
            AddItem(new PlateChest());
            Console.WriteLine("2");
            AddItem(new PlateGloves());
            Console.WriteLine("3");
            AddItem(new PlateGorget());
            Console.WriteLine("4");
            AddItem(new PlateHelm());
            Console.WriteLine("5");
            AddItem(new PlateLegs());
            Console.WriteLine("6");
            AddItem(new Broadsword());
            Console.WriteLine("7");
            AddItem(new Cloak(Utility.RandomDyedHue()));
            Console.WriteLine("8");
            Item shield = new HeaterShield(Utility.RandomDyedHue());
            AddItem(shield);
            Console.WriteLine("9");
            AddItem(new Shoes(1527));
            Console.WriteLine("0");

            // skills
            SetSkill(SkillName.Swords, 50, 70);
            SetSkill(SkillName.Parry, 30, 50);
            SetSkill(SkillName.Anatomy, 30, 50);
            SetSkill(SkillName.Tactics, 30, 50);
        }

        public MercenaryC(Serial serial)
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
