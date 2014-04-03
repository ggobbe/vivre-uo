using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class CleanseByFireScroll : SpellScroll
	{
		[Constructable]
		public CleanseByFireScroll() : this( 1 )
		{
		}

		[Constructable]
		public CleanseByFireScroll( int amount ) : base( 200, 0x227C, amount )
		{
            Name = "Cleanse By Fire Scroll";
		}

        public CleanseByFireScroll(Serial serial)
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