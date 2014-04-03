using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class NobleSacrificeScroll : SpellScroll
	{
		[Constructable]
		public NobleSacrificeScroll() : this( 1 )
		{
		}

		[Constructable]
		public NobleSacrificeScroll( int amount ) : base( 207, 0x227C, amount )
		{
            Name = "Noble Sacrifice Scroll";
		}

        public NobleSacrificeScroll(Serial serial)
            : base(serial)
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