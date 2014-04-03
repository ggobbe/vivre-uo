using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "Corps d'Essence Yeth" )]
	public class EssenceYeth : BaseCreature
	{
		[Constructable]
		public EssenceYeth() : base( AIType.AI_Melee, FightMode.Closest, 25, 1, 0.2, 0.4 )
		{
			Name = "Essence Yeth";
			Body = 51;
			BaseSoundID = 456;
			Hue = Utility.RandomSlimeHue();

			SetStr( 700, 800 );
			SetDex( 180, 220 );
			SetInt( 1, 2 );

			SetHits( 15, 19 );

			SetDamage( 1, 5 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 5, 10 );
			SetResistance( ResistanceType.Poison, 100, 100 );

			SetSkill( SkillName.Poisoning, 30.1, 40.0 );
			SetSkill( SkillName.MagicResist, 50.1, 60.0 );
			SetSkill( SkillName.Tactics, 20.0, 35.0 );
			SetSkill( SkillName.Wrestling, 25.0, 40.0 );

			Fame = 300;
			Karma = -300;

			VirtualArmor = 8;

			Tamable = false;
		}

        public override bool OnBeforeDeath()
        {
            Say("L'Essence Yeth se separe en deux!");
            EssenceYeth spawn = new EssenceYeth();
            Map map = this.Map;
            Point3D loc = this.Location;
            spawn.MoveToWorld(loc, map);
            return base.OnBeforeDeath();
        }
		public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor );
			AddLoot( LootPack.Gems );
		}

		public override Poison PoisonImmune{ get{ return Poison.Lesser; } }
		public override Poison HitPoison{ get{ return Poison.Lesser; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Fish | FoodType.FruitsAndVegies | FoodType.GrainsAndHay | FoodType.Eggs; } }

		public EssenceYeth( Serial serial ) : base( serial )
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
