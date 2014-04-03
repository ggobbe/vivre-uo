using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("Corp d'un Nécromant")]
    public class NoviceNecromancer : BaseCreature
    {
        [Constructable]
        public NoviceNecromancer()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.6)
        {
            Name = "un apprenti nécromant";
            Body = 400;
            Hue = 1109;

            Item shroud = new RobeACapuche(1109);
            shroud.Movable = false;
            AddItem(shroud);

            Item staff = new GnarledStaff();
            staff.Hue = 2211;
            staff.Movable = false;
            AddItem(staff);

            SetStr(46, 70);
            SetDex(31, 50);
            SetInt(26, 60);

            SetHits(45, 60);

            SetDamage(5, 13);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Energy, 15, 25);

            SetSkill(SkillName.MagicResist, 15.1, 40.0);
            SetSkill(SkillName.Tactics, 35.1, 50.0);
            SetSkill(SkillName.Macing, 35.1, 50.0);
            SetSkill(SkillName.Necromancy, 35.1, 50.0);
            SetSkill(SkillName.Focus, 35.1, 50.0);
            SetSkill(SkillName.SpiritSpeak, 35.1, 50.0);
            SetSkill(SkillName.Meditation, 35.1, 50.0);

            Fame = 600;
            Karma = -600;

            VirtualArmor = 25;

            PackNecroReg(5, 10);
        }

        public override bool BleedImmune { get { return false; } }
        public override Poison PoisonImmune { get { return Poison.Regular; } }
        public override bool BardImmune { get { return true; } }
        public override void DisplayPaperdollTo(Mobile to) { }
        public override bool AlwaysMurderer { get { return true; } }

        public NoviceNecromancer(Serial serial)
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