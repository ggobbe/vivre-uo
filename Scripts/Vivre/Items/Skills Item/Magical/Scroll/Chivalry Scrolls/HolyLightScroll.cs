using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class HolyLightScroll : SpellScroll
	{
		[Constructable]
		public HolyLightScroll() : this( 1 )
		{
		}

		[Constructable]
		public HolyLightScroll( int amount ) : base( 206, 0x227C, amount )
		{
            Name = "Holy Light Scroll";
		}

        public HolyLightScroll(Serial serial)
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