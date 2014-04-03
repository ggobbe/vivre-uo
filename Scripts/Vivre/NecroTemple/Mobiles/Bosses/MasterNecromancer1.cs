using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("Corp d'un Nécromant Sombre")]
    public class MasterNecromancer1 : BaseCreature, NecroBoss
    {
        [Constructable]
        public MasterNecromancer1()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Gror'tak Maitre des Squelettes";
            Body = 400;
            Hue = 1109;

            Item shroud = new RobeACapuche(1870);
            shroud.Movable = false;
            AddItem(shroud);

            Item dagger = new Dagger();
            dagger.Name = "Dague Maudite";
            dagger.Hue = 34;
            dagger.Movable = false;
            AddItem(dagger);

            SetStr(250, 400);
            SetDex(50);
            SetInt(300, 400);

            SetHits(250, 400);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 100);
            SetResistance(ResistanceType.Fire, 50);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 70, 90);

            SetSkill(SkillName.MagicResist, 20.1, 30.0);
            SetSkill(SkillName.Fencing, 80.1, 100.0);
            SetSkill(SkillName.Tactics, 80.1, 100.0);
            SetSkill(SkillName.EvalInt, 100);
            SetSkill(SkillName.Magery, 60.1, 100.0);
            SetSkill(SkillName.Necromancy, 60.1, 100.0);
            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 80;

            PackItem(new CurseWeaponScroll());
            
            if (Utility.Random(4) == 0)
                PackItem(new CurseWeaponScroll());

            if (Utility.Random(20) == 0)
                PackItem(new RobeACapuche(1870));

            PackNecroReg(10, 30);
        }

        public override bool Unprovokable { get { return true; } }
        public override bool BardImmune { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return false; } }
        public override bool AlwaysMurderer { get { return true; } }

        public override int GetIdleSound()
        {
            return 0x107;
        }

        public override int GetDeathSound()
        {
            return 0xFD;
        }

        public MasterNecromancer1(Serial serial)
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
