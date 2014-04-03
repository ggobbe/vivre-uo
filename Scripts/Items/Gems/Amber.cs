using System;
using Server;

namespace Server.Items
{
	public class Amber : BaseGem
	{	
		public override double DefaultWeight
		{
			get { return 0.1; }
		}

		[Constructable]
		public Amber() : this( 1 )
		{
		}

		[Constructable]
		public Amber( int amount ) : base( 0xF25 )
		{
            Gems = GemType.Amber;
			Stackable = true;
			Amount = amount;
		}

		public Amber( Serial serial ) : base( serial )
		{
		}

		

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            if (version < 1)
                Gems = GemType.Amber;
		}
	}
}