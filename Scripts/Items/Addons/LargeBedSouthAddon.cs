using System;
using Server;

namespace Server.Items
{
    // Scriptiz : dyable avec un furniture dye tub !
    [Furniture]
	public class LargeBedSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new LargeBedSouthDeed(); } }

        public override bool RetainDeedHue { get { return true; } } // Scriptiz : on garde la couleur du deed

		[Constructable]
		public LargeBedSouthAddon() 
            : this(0)
		{
		}

        public LargeBedSouthAddon(int hue)
        {
            AddComponent(new AddonComponent(0xA83), 0, 0, 0);
            AddComponent(new AddonComponent(0xA7F), 0, 1, 0);
            AddComponent(new AddonComponent(0xA82), 1, 0, 0);
            AddComponent(new AddonComponent(0xA7E), 1, 1, 0);
            Hue = hue;
        }

		public LargeBedSouthAddon( Serial serial ) : base( serial )
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

	public class LargeBedSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new LargeBedSouthAddon(this.Hue); } }
		public override int LabelNumber{ get{ return 1044323; } } // large bed (south)

		[Constructable]
		public LargeBedSouthDeed()
		{
		}

		public LargeBedSouthDeed( Serial serial ) : base( serial )
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