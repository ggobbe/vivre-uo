using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class EvasionScroll : SpellScroll
	{
		[Constructable]
		public EvasionScroll() : this( 1 )
		{
		}

		[Constructable]
		public EvasionScroll( int amount ) : base( 402, 0x46B2, amount )
		{
            Name = "Evasion Scroll";
		}

        public EvasionScroll(Serial serial)
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