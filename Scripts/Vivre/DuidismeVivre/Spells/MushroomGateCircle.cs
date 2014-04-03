using System;
using Server.Network;
using Server.Multis;
using Server.Items;
using Server.Targeting;
using Server.Misc;
using Server.Mobiles;
using Server.Regions;

namespace Server.Items
{
	[DispellableField]
	public class MushroomGateCircle : Moongate
	{
private int m_ItemID;
		
		public MushroomGateCircle (Point3D target, Map map, int item)
		{
	m_ItemID=item;
		//	AddComponent( new AddonComponent( 0xD10 ), 0, 0, 0 );
		//	AddComponent( new AddonComponent( 0x373A ), 0, 0, 1 );
		//		AddComponent( new AddonComponent( 0xD11 ), -1, 1, 0 );
		//		AddComponent( new AddonComponent( 0xD0C ), -0, 2, 0 );
		//		AddComponent( new AddonComponent( 0xD0D ), 1, 1, 0 );
		//		AddComponent( new AddonComponent( 0xD0E ), 2, 0, 0 );
		//		AddComponent( new AddonComponent( 0xD0F ), 1, -1, 0 );
		//		Map = map;

				if ( ShowFeluccaWarning && map == Map.Felucca )
				{
						Hue = 1175;
					ItemID=m_ItemID;
				}

	//			Dispellable = true;

				InternalTimer t = new InternalTimer( this );
				t.Start();
			
		}

		public MushroomGateCircle( Serial serial ) : base( serial )
		{
		}
	
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

		
		}

		public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );

				Delete();
			}
	private class InternalTimer : Timer
			{
				private Item m_Item;

				public InternalTimer( Item item ) : base( TimeSpan.FromSeconds( 30.0 ) )
				{
					m_Item = item;
				}

				protected override void OnTick()
				{
					m_Item.Delete();
				}
			}

	}
}
