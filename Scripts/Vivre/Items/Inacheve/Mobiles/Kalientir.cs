using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("Cadavre de Kalientir")]
    public class Kalientir : BaseCreature
    {
        [Constructable]
        public Kalientir()
            : base(AIType.AI_Melee, FightMode.Aggressor, 5, 2, 0.13, 0.24)
        {
            Name = "Kalientir, la brulante";
            Body = 0x313;
            Hue = 0x4E9;
            YellowHealthbar = true;

            SetStr(609, 843);
            SetDex(191, 243);
            SetInt(351, 458);

            SetHits(1558, 1627);

            SetDamage(13, 19);
        
            SetDamageType(ResistanceType.Energy, 100);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 120);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 70, 90);
            SetResistance(ResistanceType.Energy, 50,60);

            SetSkill(SkillName.Anatomy, 51.1, 74.2);
            SetSkill(SkillName.EvalInt, 90.3, 99.8);
            SetSkill(SkillName.Magery, 99.1, 120.0);
            SetSkill(SkillName.Meditation, 90.1, 99.6);
            SetSkill(SkillName.MagicResist, 110.6, 130.5);
            SetSkill(SkillName.Tactics, 90.1, 99.5);
            SetSkill(SkillName.Wrestling, 97.7, 100.0);
        }

        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override bool BardImmune { get { return true;} }
        public override bool BleedImmune{ get { return true;} }
        public override bool AutoDispel{ get { return true;} }
        

        public override WeaponAbility GetWeaponAbility()
        {
                return WeaponAbility.MortalStrike;
         }

        public override void OnCombatantChange()
        {
          
        }

        public override void CheckReflect(Mobile caster, ref bool reflect)
        {
           
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
          
        }


        public override void OnDeath(Container c)
        {
            SmithHammer hammer = new SmithHammer();
            hammer.Resource = CraftResource.MVulcan;
            hammer.UsesRemaining = Utility.Random(100, 200);
            hammer.Name = "Mandibule de Kalientir";
            c.DropItem(hammer);

            c.DropItem(new FireRuby(Utility.Random(16,25)));


            base.OnDeath(c);
            
        }


        public override int GetAttackSound() { return Utility.Random(0x2F5, 2); }
        public override int GetDeathSound() { return 0x2F7; }
        public override int GetAngerSound() { return 0x2F8; }
        public override int GetHurtSound() { return 0x2F9; }
        public override int GetIdleSound() { return 0x2FA; }

        public Kalientir(Serial serial)
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