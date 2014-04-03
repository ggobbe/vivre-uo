using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class ShadowjumpScroll : SpellScroll
	{
		[Constructable]
		public ShadowjumpScroll() : this( 1 )
		{
		}

		[Constructable]
        public ShadowjumpScroll(int amount)
            : base(506, 0x46AE, amount)
		{
            Name = "Shadowjump Scroll";
		}

        public ShadowjumpScroll(Serial serial)
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