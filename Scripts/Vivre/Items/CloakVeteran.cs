using System;

namespace Server.Items
{
	[Flipable]
	public class VeteranCloak : BaseCloak
	{
		
		[Constructable]
		public VeteranCloak() : this(0)
		{
		}

		[Constructable]
		public VeteranCloak( int hue ) : base( 0x1515, hue )
		{
            Name = "Cape des survivants";
			Weight = 5.0;
            Hue = 577;
            LootType = LootType.Blessed;
		}

        public VeteranCloak (Serial serial)
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