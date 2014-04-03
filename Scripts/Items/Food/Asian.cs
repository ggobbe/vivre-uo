using System;

namespace Server.Items
{
	public class Wasabi : Item
	{
		[Constructable]
		public Wasabi() : base( 0x24E8 )
		{
			Weight = 1.0;
		}

		public Wasabi( Serial serial ) : base( serial )
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

	public class WasabiClumps : Food
	{
		[Constructable]
		public WasabiClumps() : base( 0x24EB )
		{
            Name = "P�te de wasabi";
			Stackable = false;
			Weight = 1.0;
			FillFactor = 2;
		}

		public WasabiClumps( Serial serial ) : base( serial )
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

	public class EmptyBentoBox : Item
	{
		[Constructable]
		public EmptyBentoBox() : base( 0x2834 )
		{
            Name = "Boite de bento vide";
			Weight = 5.0;
		}

		public EmptyBentoBox( Serial serial ) : base( serial )
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

	public class BentoBox : Food
	{
		[Constructable]
		public BentoBox() : base( 0x2836 )
		{
            Name = "Boite de bento";
			Stackable = false;
			Weight = 5.0;
			FillFactor = 2;
		}

		public override bool Eat( Mobile from )
		{
			if ( !base.Eat( from ) )
				return false;

			from.AddToBackpack( new EmptyBentoBox() );
			return true;
		}

		public BentoBox( Serial serial ) : base( serial )
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

	public class SushiRolls : Food
	{
		[Constructable]
		public SushiRolls() : base( 0x283E )
		{
            Name = "Makis";
			Stackable = false;
			Weight = 3.0;
			FillFactor = 2;
		}

		public SushiRolls( Serial serial ) : base( serial )
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

	public class SushiPlatter : Food
	{
		[Constructable]
		public SushiPlatter() : base( 0x2840 )
		{
            Name = "Plateau de sushis";
            Stackable = Core.ML;
			Weight = 3.0;
			FillFactor = 2;
		}

		public SushiPlatter( Serial serial ) : base( serial )
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

	public class GreenTeaBasket : Item
	{
		[Constructable]
		public GreenTeaBasket() : base( 0x284B )
		{
            Name = "Boite de th� vert";
			Weight = 10.0;
		}

		public GreenTeaBasket( Serial serial ) : base( serial )
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

	public class GreenTea : Food
	{
		[Constructable]
		public GreenTea() : base( 0x284C )
		{
            Name = "Th� vert";
			Stackable = false;
			Weight = 4.0;
			FillFactor = 2;
		}

		public GreenTea( Serial serial ) : base( serial )
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

	public class MisoSoup : Food
	{
		[Constructable]
		public MisoSoup() : base( 0x284D )
		{
            Name = "Soupe miso";
			Stackable = false;
			Weight = 4.0;
			FillFactor = 2;
		}

		public MisoSoup( Serial serial ) : base( serial )
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

	public class WhiteMisoSoup : Food
	{
		[Constructable]
		public WhiteMisoSoup() : base( 0x284E )
		{
            Name = "Soupe miso blanche";
			Stackable = false;
			Weight = 4.0;
			FillFactor = 2;
		}

		public WhiteMisoSoup( Serial serial ) : base( serial )
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

	public class RedMisoSoup : Food
	{
		[Constructable]
		public RedMisoSoup() : base( 0x284F )
		{
            Name = "Soupe miso rouge";
			Stackable = false;
			Weight = 4.0;
			FillFactor = 2;
		}

		public RedMisoSoup( Serial serial ) : base( serial )
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

	public class AwaseMisoSoup : Food
	{
		[Constructable]
		public AwaseMisoSoup() : base( 0x2850 )
		{
            Name = "Soupe miso awase";
			Stackable = false;
			Weight = 4.0;
			FillFactor = 2;
		}

		public AwaseMisoSoup( Serial serial ) : base( serial )
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