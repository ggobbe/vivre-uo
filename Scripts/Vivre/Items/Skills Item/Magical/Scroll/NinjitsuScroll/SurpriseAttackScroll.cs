using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class SurpriseAttackScroll : SpellScroll
	{
		[Constructable]
		public SurpriseAttackScroll() : this( 1 )
		{
		}

		[Constructable]
        public SurpriseAttackScroll(int amount)
            : base(504, 0x46AE, amount)
		{
            Name = "Surprise Attack Scroll";
		}

        public SurpriseAttackScroll(Serial serial)
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