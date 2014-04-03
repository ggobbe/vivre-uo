using System;

namespace Server.Items
{
	public class EmptyWoodenBowl : Item
	{
		[Constructable]
		public EmptyWoodenBowl() : base( 0x15F8 )
		{
            Name = "Bol de bois";
			Weight = 1.0;
		}

		public EmptyWoodenBowl( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class EmptyPewterBowl : Item
	{
		[Constructable]
		public EmptyPewterBowl() : base( 0x15FD )
		{
            Name = "Bol d'étain";
			Weight = 1.0;
		}

		public EmptyPewterBowl( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class WoodenBowlOfCarrots : Food
	{
		[Constructable]
		public WoodenBowlOfCarrots() : base( 0x15F9 )
		{
            Name = "Bol de carottes";
			Stackable = false;
			Weight = 1.0;
			FillFactor = 2;
		}

		public override bool Eat( Mobile from )
		{
			if ( !base.Eat( from ) )
				return false;

			from.AddToBackpack( new EmptyWoodenBowl() );
			return true;
		}

		public WoodenBowlOfCarrots( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class WoodenBowlOfCorn : Food
	{
		[Constructable]
		public WoodenBowlOfCorn() : base( 0x15FA )
		{
            Name = "Bol de maïs";
			Stackable = false;
			Weight = 1.0;
			FillFactor = 2;
		}

		public override bool Eat( Mobile from )
		{
			if ( !base.Eat( from ) )
				return false;

			from.AddToBackpack( new EmptyWoodenBowl() );
			return true;
		}

		public WoodenBowlOfCorn( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class WoodenBowlOfLettuce : Food
	{
		[Constructable]
		public WoodenBowlOfLettuce() : base( 0x15FB )
		{
            Name = "Bol de salade";
			Stackable = false;
			Weight = 1.0;
			FillFactor = 2;
		}

		public override bool Eat( Mobile from )
		{
			if ( !base.Eat( from ) )
				return false;

			from.AddToBackpack( new EmptyWoodenBowl() );
			return true;
		}

		public WoodenBowlOfLettuce( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class WoodenBowlOfPeas : Food
	{
		[Constructable]
		public WoodenBowlOfPeas() : base( 0x15FC )
		{
            Name = "Bol de pois";
			Stackable = false;
			Weight = 1.0;
			FillFactor = 2;
		}

		public override bool Eat( Mobile from )
		{
			if ( !base.Eat( from ) )
				return false;

			from.AddToBackpack( new EmptyWoodenBowl() );
			return true;
		}

		public WoodenBowlOfPeas( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class PewterBowlOfCarrots : Food
	{
		[Constructable]
		public PewterBowlOfCarrots() : base( 0x15FE )
		{
            Name = "Bol de carottes";
			Stackable = false;
			Weight = 1.0;
			FillFactor = 2;
		}

		public override bool Eat( Mobile from )
		{
			if ( !base.Eat( from ) )
				return false;

			from.AddToBackpack( new EmptyPewterBowl() );
			return true;
		}

		public PewterBowlOfCarrots( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class PewterBowlOfCorn : Food
	{
		[Constructable]
		public PewterBowlOfCorn() : base( 0x15FF )
		{
            Name = "Bol de maïs";
			Stackable = false;
			Weight = 1.0;
			FillFactor = 2;
		}

		public override bool Eat( Mobile from )
		{
			if ( !base.Eat( from ) )
				return false;

			from.AddToBackpack( new EmptyPewterBowl() );
			return true;
		}

		public PewterBowlOfCorn( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class PewterBowlOfLettuce : Food
	{
		[Constructable]
		public PewterBowlOfLettuce() : base( 0x1600 )
		{
            Name = "Bol de salade";
			Stackable = false;
			Weight = 1.0;
			FillFactor = 2;
		}

		public override bool Eat( Mobile from )
		{
			if ( !base.Eat( from ) )
				return false;

			from.AddToBackpack( new EmptyPewterBowl() );
			return true;
		}

		public PewterBowlOfLettuce( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class PewterBowlOfPeas : Food
	{
		[Constructable]
		public PewterBowlOfPeas() : base( 0x1601 )
		{
            Name = "Bol de pois";
			Stackable = false;
			Weight = 1.0;
			FillFactor = 2;
		}

		public override bool Eat( Mobile from )
		{
			if ( !base.Eat( from ) )
				return false;

			from.AddToBackpack( new EmptyPewterBowl() );
			return true;
		}

		public PewterBowlOfPeas( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class PewterBowlOfPotatos : Food
	{
		[Constructable]
		public PewterBowlOfPotatos() : base( 0x1602 )
		{
            Name = "Bol de pommes de terre";
			Stackable = false;
			Weight = 1.0;
			FillFactor = 2;
		}

		public override bool Eat( Mobile from )
		{
			if ( !base.Eat( from ) )
				return false;

			from.AddToBackpack( new EmptyPewterBowl() );
			return true;
		}

		public PewterBowlOfPotatos( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	[TypeAlias( "Server.Items.EmptyLargeWoodenBowl" )]
	public class EmptyWoodenTub : Item
	{
		[Constructable]
		public EmptyWoodenTub() : base( 0x1605 )
		{
            Name = "Grand bol de bois";
			Weight = 2.0;
		}

		public EmptyWoodenTub( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	[TypeAlias( "Server.Items.EmptyLargePewterBowl" )]
	public class EmptyPewterTub : Item
	{
		[Constructable]
		public EmptyPewterTub() : base( 0x1603 )
		{
            Name = "Grand bol d'étain";
			Weight = 2.0;
		}

		public EmptyPewterTub( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class WoodenBowlOfStew : Food
	{
		[Constructable]
		public WoodenBowlOfStew() : base( 0x1604 )
		{
            Name = "Grand bol de ragoût";
			Stackable = false;
			Weight = 2.0;
			FillFactor = 2;
		}

		public override bool Eat( Mobile from )
		{
			if ( !base.Eat( from ) )
				return false;

			from.AddToBackpack( new EmptyWoodenTub() );
			return true;
		}

		public WoodenBowlOfStew( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class WoodenBowlOfTomatoSoup : Food
	{
		[Constructable]
		public WoodenBowlOfTomatoSoup() : base( 0x1606 )
		{
            Name = "Grand bol de soupe aux tomates";
			Stackable = false;
			Weight = 2.0;
			FillFactor = 2;
		}

		public override bool Eat( Mobile from )
		{
			if ( !base.Eat( from ) )
				return false;

			from.AddToBackpack( new EmptyWoodenTub() );
			return true;
		}

		public WoodenBowlOfTomatoSoup( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}