using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("corps de seigneur liche terrifiant")]
    public class NecroLichLord : BaseCreature, NecroBoss
    {
        [Constructable]
        public NecroLichLord()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Un Seigneur Liche Terrifiant";
            Body = 79;
            BaseSoundID = 412;
            Hue = 1172;

            SetStr(416, 505);
            SetDex(50);
            SetInt(566, 655);

            //SetHits(250, 303);
            SetHits(350, 403);
            SetMana(3000);

            SetDamage(15, 18);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Cold, 60);
            SetDamageType(ResistanceType.Energy, 40);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 150.5, 200.0);
            SetSkill(SkillName.Tactics, 50.1, 70.0);
            SetSkill(SkillName.Wrestling, 60.1, 80.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 50;

            int scrolls = Utility.Random(0, 10);
            switch (scrolls)
            {
                case 0:
                    goto default;
                case 1: case 2: case 3: case 4:
                    PackItem(new MindRotScroll());
                    break;
                case 5: case 6: case 7:
                    PackItem(new SummonFamiliarScroll());
                    break;
                case 8: case 9:
                    PackItem(new MindRotScroll());
                    PackItem(new SummonFamiliarScroll());
                    break;
                default:
                    Item i = new Item(8807);
                    i.Name = "Un Faux Parchemin";
                    PackItem(i);
                    break;
            }

            PackNecroReg(30, 50);
        }

        public override OppositionGroup OppositionGroup
        {
            get { return OppositionGroup.FeyAndUndead; }
        }

        public override bool CanRummageCorpses { get { return true; } }
        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override int TreasureMapLevel { get { return 4; } }
        public override bool BardImmune { get { return true; } }
        public override bool Unprovokable { get { return true; } }

        public NecroLichLord(Serial serial)
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
        }
    }
}