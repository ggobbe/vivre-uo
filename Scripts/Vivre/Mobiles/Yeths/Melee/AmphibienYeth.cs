using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "Corps d'Amphibien Yeth" )]
	public class AmphibienYeth : BaseCreature
	{
		[Constructable]
		public AmphibienYeth() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Amphibien Yeth";
			Body = 0xCA;
            Hue = 0455;
			BaseSoundID = 660;

			SetStr( 1500, 1700 );
			SetDex( 90, 95 );
			SetInt( 1, 2 );

			SetHits( 46, 60 );
			SetStam( 46, 65 );
			SetMana( 0 );

			SetDamage( 5, 15 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 25, 35 );
			SetResistance( ResistanceType.Fire, 100, 100 );
			SetResistance( ResistanceType.Poison, 100, 100 );

            SetSkill(SkillName.Parry, 50.0, 60.0);
            SetSkill(SkillName.Poisoning, 50.0, 60.0);
			SetSkill( SkillName.MagicResist, 25.1, 40.0 );
			SetSkill( SkillName.Tactics, 35.1, 50.0 );
			SetSkill( SkillName.Wrestling, 40.1, 60.0 );

            Fame = 3000;
            Karma = -7000;

			VirtualArmor = 30;

			Tamable = false;
		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 12; } }
		public override HideType HideType{ get{ return HideType.Spined; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Fish; } }

		public AmphibienYeth(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			if ( BaseSoundID == 0x5A )
				BaseSoundID = 660;
		}
	}
}