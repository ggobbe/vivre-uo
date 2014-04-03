using System;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	[FlipableAttribute( 3780, 3781 )]
	public class couteau : Item
	{
		[Constructable]
		public couteau() : base( 3780 )
		{
			Name = "Couteau de jardinier";
			Weight = 1.0;
		}

		public couteau( Serial serial ) : base( serial )
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

		public override void OnDoubleClick( Mobile from )
		{
			PlayerMobile pm = from as PlayerMobile;

			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else 
			{
          		from.CloseGump( typeof( UpRootGump ) );
          		//from.SendGump( new UpRootGump( from, this ) );
			}
		}
	}
}