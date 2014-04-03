using System;

namespace Server.Items
{
	public class OrgeGerbe : Item
	{
		[Constructable]
		public OrgeGerbe() : this( 1 )
		{
		}

		[Constructable]
		public OrgeGerbe( int amount ) : base( 0X1EBD )
		{
			Weight = 1.0;
			Stackable = true;
			Amount = amount;
			Hue = 0x28E;
			Name = "Gerbe d'orge";
		}

		public OrgeGerbe( Serial serial ) : base( serial )
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