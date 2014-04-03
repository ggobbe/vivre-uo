using System;
using Server;

namespace Server.Items
{
    // Scriptiz : dyable avec un furniture dye tub !
    [Furniture]
    public class LargeBedEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new LargeBedEastDeed(); } }

        public override bool RetainDeedHue { get { return true; } } // Scriptiz : on garde la couleur du deed

		[Constructable]
		public LargeBedEastAddon()
            : this(0)   // Scriptiz : on migre vers le constructeur avec hue 0 par d�faut
		{
		}

        // Scriptiz : constructeur pour garder la hue du deed
        public LargeBedEastAddon(int hue)
        {
            AddComponent( new AddonComponent( 0xA7D ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0xA7C ), 0, 1, 0 );
			AddComponent( new AddonComponent( 0xA79 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 0xA78 ), 1, 1, 0 );
            Hue = hue;
        }

		public LargeBedEastAddon( Serial serial ) : base( serial )
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

	public class LargeBedEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new LargeBedEastAddon(this.Hue); } }   // Scriptiz : on garde la hue
		public override int LabelNumber{ get{ return 1044324; } } // large bed (east)

		[Constructable]
		public LargeBedEastDeed()
		{
		}

		public LargeBedEastDeed( Serial serial ) : base( serial )
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