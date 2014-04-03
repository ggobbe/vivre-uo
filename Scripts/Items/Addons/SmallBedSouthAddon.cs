using System;
using Server;

namespace Server.Items
{
    // Scriptiz : dyable avec un furniture dye tub !
    [Furniture]
	public class SmallBedSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new SmallBedSouthDeed(); } }

        public override bool RetainDeedHue { get { return true; } }

        [Constructable]
        public SmallBedSouthAddon()
            : this(0)
        {
        }

        public SmallBedSouthAddon(int hue)
        {
            AddComponent(new AddonComponent(0xA63), 0, 0, 0);
            AddComponent(new AddonComponent(0xA5C), 0, 1, 0);
        }

		public SmallBedSouthAddon( Serial serial ) : base( serial )
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

	public class SmallBedSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new SmallBedSouthAddon(this.Hue); } }
		public override int LabelNumber{ get{ return 1044321; } } // small bed (south)

		[Constructable]
		public SmallBedSouthDeed()
		{
		}

		public SmallBedSouthDeed( Serial serial ) : base( serial )
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