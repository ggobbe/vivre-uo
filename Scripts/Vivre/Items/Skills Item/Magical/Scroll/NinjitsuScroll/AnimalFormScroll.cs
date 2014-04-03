using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class AnimalFormScroll : SpellScroll
	{
		[Constructable]
		public AnimalFormScroll() : this( 1 )
		{
		}

		[Constructable]
        public AnimalFormScroll(int amount)
            : base(502, 0x46AF, amount)
		{
            Name = "Animal Form Scroll";
		}

        public AnimalFormScroll(Serial serial)
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