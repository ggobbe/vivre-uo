using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("Corps de Celestial")]
    public class Celestial : BaseCreature
    {
        [Constructable]
        public Celestial()
            : base(AIType.AI_Melee, FightMode.Aggressor, 5, 2, 0.2, 0.4)
        {
            Name = "Celestial, le forgeron divin";
            Body = 0x7B;
            Hue = 0x47E;
            YellowHealthbar = true;

            SetStr(609, 843);
            SetDex(191, 243);
            SetInt(351, 458);

            SetHits(1458, 1627);

            SetDamage(13, 19);
        
            SetDamageType(ResistanceType.Energy, 100);

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 45, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 120);

            SetSkill(SkillName.Anatomy, 51.1, 74.2);
            SetSkill(SkillName.EvalInt, 90.3, 99.8);
            SetSkill(SkillName.Magery, 99.1, 120.0);
            SetSkill(SkillName.Meditation, 90.1, 99.6);
            SetSkill(SkillName.MagicResist, 90.6, 99.5);
            SetSkill(SkillName.Tactics, 90.1, 99.5);
            SetSkill(SkillName.Wrestling, 97.7, 100.0);
        }

        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override bool BardImmune { get { return true;} }
        public override bool BleedImmune{ get { return true;} }
        public override bool AutoDispel{ get { return true;} }
        private bool DayDmg;

        public override WeaponAbility GetWeaponAbility()
        {
                return WeaponAbility.ParalyzingBlow;
         }

        public override void OnCombatantChange()
        {
            int hours = 0;
            int minutes = 0;

            Clock.GetTime(Map, Location.X, Location.Y, out hours, out minutes);

            if (hours < 18 && hours > 7)
            {
                DayDmg = true;
            }
            else
            {
                DayDmg = false;
            }
            base.OnCombatantChange();
        }

        public override void CheckReflect(Mobile caster, ref bool reflect)
        {
            if(caster.FindItemOnLayer(Layer.Neck) is BaseJewel)
            {
                BaseJewel neck = (BaseJewel) caster.FindItemOnLayer(Layer.Neck);
                if (neck.Resource != CraftResource.MGlowing)
                        reflect = false;
            }
            else if(Utility.RandomDouble()<0.5)
            {
                reflect = true; // Always reflect is not protected by Glowing
                caster.SendMessage("Un porte bonheur de lumière, voilà ce qu'il manque");
            }

            return;
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            if (DayDmg && Utility.RandomDouble()<.20)
             {
                 Say("Je me nourris de votre lumière!");
                 Str += 1;
             }
            else if (defender.LightLevel < -8)
            {
                Say("Quelle noirceur! *cri de douleur*");
                AOS.Damage(this, defender, Utility.Random(10,15), true, 0, 0, 0, 0, 0, 0, 100, false, false, false);
            }
            base.OnGaveMeleeAttack(defender);
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            int bonusdmg = (Math.Max((int)attacker.Skills[SkillName.Chivalry].Value,(int)attacker.Skills[SkillName.Blacksmith].Value)/20);
            
            AOS.Damage(this, attacker, bonusdmg, true, 0, 0, 0, 0, 0, 0, 100, false, false, false);

            if(attacker is BaseCreature)
            {
                Say("Meurt, toi qui n'est pas digne de me reconnaitre");
                attacker.Kill();
            }

            if (attacker.FindItemOnLayer(Layer.OneHanded) is BaseWeapon)
            {
                BaseWeapon wpn = (BaseWeapon)attacker.FindItemOnLayer(Layer.OneHanded);
                if (wpn.Resource < CraftResource.MGlowing && Utility.RandomDouble()<.10)
                {
                    Emote("Votre arme perce sa lumière, vous offrant un second souffle");
                    attacker.Heal(Utility.Random(1,15));
                }
            }

            base.OnGotMeleeAttack(attacker);
        }


        public override void OnDeath(Container c)
        {
            SmithHammer hammer = new SmithHammer();
            hammer.Resource = CraftResource.MGlowing;
            hammer.UsesRemaining = Utility.Random(100, 200);
            hammer.Name = "Marteau de Celestial";
            c.DropItem(hammer);

            c.DropItem(new BlueDiamond(Utility.Random(16,25)));


            base.OnDeath(c);
            
        }


        public override int GetAttackSound() { return Utility.Random(0x2F5, 2); }
        public override int GetDeathSound() { return 0x2F7; }
        public override int GetAngerSound() { return 0x2F8; }
        public override int GetHurtSound() { return 0x2F9; }
        public override int GetIdleSound() { return 0x2FA; }

        public Celestial(Serial serial)
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