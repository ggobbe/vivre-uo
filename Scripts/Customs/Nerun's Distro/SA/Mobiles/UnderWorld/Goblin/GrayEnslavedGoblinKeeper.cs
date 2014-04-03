using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName( "a goblin corpse" )]
	public class EnslavedGoblinKeeper : BaseCreature
	{
		[Constructable]
		public EnslavedGoblinKeeper() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an enslaved goblin keeper";
			Body = 723;
			Hue = 2500;
			BaseSoundID = 0x45A;

			SetStr( 326 );
			SetDex( 79 );
			SetInt( 114 );

			SetHits( 186 );
			SetStam( 79 );
			SetMana( 114 );

			SetDamage( 5, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 45 );
			SetResistance( ResistanceType.Fire, 33 );
			SetResistance( ResistanceType.Cold, 25 );
			SetResistance( ResistanceType.Poison, 20 );
			SetResistance( ResistanceType.Energy, 10 );

			SetSkill( SkillName.MagicResist, 129.9 );
			SetSkill( SkillName.Tactics, 86.7 );
			SetSkill( SkillName.Anatomy, 86.6 );
			SetSkill( SkillName.Wrestling, 103.6 );

			Fame = 1500;
			Karma = -1500;

			VirtualArmor = 28;

			switch ( Utility.Random( 20 ) )
			{
				case 0: PackItem( new Scimitar() ); break;
				case 1: PackItem( new Katana() ); break;
				case 2: PackItem( new WarMace() ); break;
				case 3: PackItem( new WarHammer() ); break;
				case 4: PackItem( new Kryss() ); break;
				case 5: PackItem( new Pitchfork() ); break;
			}

			PackItem( new ThighBoots() );

            Ribs rib = new Ribs();
            if (Utility.RandomDouble() <= 0.15)
                rib.Poison = Poison.Regular;

			switch ( Utility.Random( 3 ) )
			{
				case 0: PackItem( rib ); break;
				case 1: PackItem( new Shaft() ); break;
				case 2: PackItem( new Candle() ); break;
			}

			if ( 0.2 > Utility.RandomDouble() )
				PackItem( new BolaBall() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 1; } }
		public override int Meat{ get{ return 1; } }

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.SavagesAndOrcs; }
		}

		public EnslavedGoblinKeeper( Serial serial ) : base( serial )
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