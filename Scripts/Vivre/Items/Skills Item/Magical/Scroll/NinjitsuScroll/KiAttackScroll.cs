using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class KiAttackScroll : SpellScroll
	{
		[Constructable]
		public KiAttackScroll() : this( 1 )
		{
		}

		[Constructable]
        public KiAttackScroll(int amount)
            : base(503, 0x46AF, amount)
		{
            Name = "Ki Attack Scroll";
		}

        public KiAttackScroll(Serial serial)
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