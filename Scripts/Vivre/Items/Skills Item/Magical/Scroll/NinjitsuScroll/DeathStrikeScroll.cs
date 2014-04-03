using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class DeathStrikeScroll : SpellScroll
	{
		[Constructable]
		public DeathStrikeScroll() : this( 1 )
		{
		}

		[Constructable]
        public DeathStrikeScroll(int amount)
            : base(501, 0x46AF, amount)
		{
            Name = "Death Strike Scroll";
		}

        public DeathStrikeScroll(Serial serial)
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