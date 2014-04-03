using System;
using Server;

namespace Server.Items
{
    public class Amethyst : BaseGem
    {
		public override double DefaultWeight
		{
			get { return 0.1; }
		}

		[Constructable]
		public Amethyst() : this( 1 )
		{
		}

		[Constructable]
		public Amethyst( int amount ) : base( 0xF16 )
		{
			Stackable = true;
			Amount = amount;
            Gems = GemType.Amethyst;
		}

		public Amethyst( Serial serial ) : base( serial )
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
                Gems = GemType.Amethyst;
		}
	}
}