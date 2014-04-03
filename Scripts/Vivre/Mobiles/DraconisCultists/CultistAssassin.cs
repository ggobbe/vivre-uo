using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
	[CorpseName( "Assassin du Culte" )]
	public class CultistAssassin : BaseCreature
	{
		[Constructable]
		public CultistAssassin() : base( AIType.AI_Assassin, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Assassin du Culte";
			Body = 0x190;		

			SetStr( 85, 120 );
			SetDex( 100, 120 );
			SetInt( 151, 200 );

			SetHits( 200 );
			SetStam( 150 );
			SetMana( 120 );

			SetDamage( 8, 10 );

			SetDamageType( ResistanceType.Physical, 70 );			
			SetDamageType( ResistanceType.Poison, 30 );

			SetResistance( ResistanceType.Physical, 30, 40 );
			SetResistance( ResistanceType.Fire, 60, 80 );
			SetResistance( ResistanceType.Cold, 40, 40 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 50, 60 );

			SetSkill( SkillName.Poisoning, 120.0 );
			SetSkill( SkillName.MagicResist, 100, 120 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Fencing, 90.1, 110.0 );
            SetSkill( SkillName.Anatomy, 100.0 );
            SetSkill(SkillName.Hiding,100);
            SetSkill(SkillName.Stealth, 120.0);
            SetSkill(SkillName.Focus, 120.0);


            ActiveSpeed = 0.12;
            PassiveSpeed = 0.250;
            

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 40;


            HairItemID = 0;
            AddItem(new StuddedChest());
            AddItem(new StuddedArms());
            AddItem(new StuddedGloves());
            AddItem(new StuddedGorget());
            AddItem(new StuddedLegs());
            AddItem(new Boots());            
           
            Dagger weapon = new Dagger(); 

            weapon.Movable = false;          
            weapon.Quality = WeaponQuality.Exceptional;
            weapon.Attributes.WeaponDamage = 40;
            weapon.WeaponAttributes.HitLeechHits = 30;
            AddItem(weapon);
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
		}
		
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override Poison HitPoison{ get{ return Poison.Deadly; } }
		public override int TreasureMapLevel{ get{ return 5; } }


        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (attacker is BaseCreature && ((BaseCreature)attacker).Summoned) //pas subtil, mais la bestiole peut tanker...un peu :)
            {
                Hidden = true;
                UseSkill(SkillName.Stealth);
                Combatant = ((BaseCreature)attacker).SummonMaster;
                
                attacker.RawStr = 10;
                attacker.RawInt = 1;
                attacker.RawDex = 10;
                attacker.Mana = 5;
                attacker.Stam = 0;

            }
           
        }
        public override void OnGaveMeleeAttack(Mobile defender)
        {

            base.OnGaveMeleeAttack(defender);

            if (defender is BaseCreature && ((BaseCreature)defender).Summoned)
            {
                Hidden = true;
                UseSkill(SkillName.Stealth);
                Combatant = ((BaseCreature)defender).SummonMaster;

                defender.RawStr = 10;
                defender.RawInt = 1;
                defender.RawDex = 10;
                defender.Mana = 5;
                defender.Stam = 0;
            }
        }

        public override void OnDamagedBySpell(Mobile from)
        {
            base.OnDamagedBySpell(from);         
            this.Hidden = true;                      
            this.UseSkill(SkillName.Stealth);
            return;
        }

		public CultistAssassin( Serial serial ) : base( serial )
		{
		}		

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}