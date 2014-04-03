using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("Azoth")]
    public class SleepinessCold : BaseCreature, NecroBoss
    {
        [Constructable]
        public SleepinessCold()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Azoth, le froid glacial";
            Body = 0x105;

            SetStr(509, 538);
            SetDex(50);
            SetInt(1513, 1578);

            SetHits(750, 1000);

            SetDamage(25, 31);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Cold, 20);
            SetDamageType(ResistanceType.Poison, 20);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 75, 76);
            SetResistance(ResistanceType.Fire, 60, 65);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 76, 80);
            SetResistance(ResistanceType.Energy, 75, 78);

            SetSkill(SkillName.Wrestling, 100.2, 101.4);
            SetSkill(SkillName.Tactics, 105.5, 102.1);
            SetSkill(SkillName.MagicResist, 150);
            SetSkill(SkillName.Magery, 150.0);
            SetSkill(SkillName.EvalInt, 150.0);
            SetSkill(SkillName.Meditation, 120.0);

            Fame = 8000;
            Karma = -8000;

            VirtualArmor = 70;

            PackItem(new GnarledStaff());
            PackNecroReg(15, 25);

            PackItem(new WitherScroll());

            int nScrolls = Utility.Random(4);
            for (int i = 0; i < nScrolls; i++)
            {
                if (Utility.RandomBool())
                    PackItem(new WitherScroll());
            }
        }

        public override bool CanRummageCorpses { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }

        public override bool AutoDispel { get { return true; } }
        public override int TreasureMapLevel { get { return 5; } }

        public SleepinessCold(Serial serial)
            : base(serial)
        {
        }

        public override int GetIdleSound()
        {
            return 0x1BF;
        }

        public override int GetAttackSound()
        {
            return 0x1C0;
        }

        public override int GetHurtSound()
        {
            return 0x1C1;
        }

        public override int GetDeathSound()
        {
            return 0x1C2;
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