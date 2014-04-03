using System;
using Server;

namespace Server.Items
{
	public class StarSapphire : BaseGem
	{
        GemType Gem { get { return GemType.StarSapphire; } }
		public override double DefaultWeight
		{
			get { return 0.1; }
		}

		[Constructable]
		public StarSapphire() : this( 1 )
		{
		}

		[Constructable]
		public StarSapphire( int amount ) : base( 0xF21 )
		{
			Stackable = true;
			Amount = amount;
            Gems = GemType.StarSapphire;
		}

		public StarSapphire( Serial serial ) : base( serial )
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
                Gems = GemType.StarSapphire;
		}
	}
}