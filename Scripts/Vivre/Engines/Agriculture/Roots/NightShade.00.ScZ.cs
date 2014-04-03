using System;
using Server.Network;
using Server.Targeting;

namespace Server.Items.Crops
{
	public class NightshadePlant : BaseCrop 
	{ 
		private double lumberValue;
		private DateTime lastpicked;

		[Constructable] 
		public NightshadePlant() : base( Utility.RandomList( 0x18E5, 0x18E6 ) ) 
		{ 
			Movable = false;
            Name = "Solanac�e"; 
			lastpicked = DateTime.Now;
		} 

		public override void OnDoubleClick(Mobile from) 
		{ 
			if ( from == null || !from.Alive ) return;

			// lumbervalue = 100; will give 100% sucsess in picking
			lumberValue = from.Skills[SkillName.Lumberjacking].Value / 5;

			if ( DateTime.Now > lastpicked.AddSeconds(3) ) // 3 seconds between picking
			{
				lastpicked = DateTime.Now;
				if ( from.InRange( this.GetWorldLocation(), 2 ) ) 
				{ 
					if ( lumberValue > Utility.Random( 100 ) )
					{
						from.Direction = from.GetDirectionTo( this );
						from.Animate( 32, 5, 1, true, false, 0 ); // Bow

						from.SendMessage(AgriTxt.PullRoot); 
						this.Delete(); 

						from.AddToBackpack( new NightshadeUprooted() );
					}
					else from.SendMessage(AgriTxt.HardPull); 
				} 
				else 
				{ 
					from.SendMessage(AgriTxt.HardPull); 
				} 
			}
		} 

		public NightshadePlant( Serial serial ) : base( serial ) 
		{ 
			lastpicked = DateTime.Now;
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 0 ); 
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 
		} 
	} 
	
	[FlipableAttribute( 0x18E7, 0x18E8 )]
	public class NightshadeUprooted : Item, ICarvable
	{
		public void Carve( Mobile from, Item item )
		{
			int count = Utility.Random( 4 );
			if ( count == 0 ) 
			{
				from.SendMessage(AgriTxt.NoRoot); 
				this.Consume();
			}
			else
			{
				base.ScissorHelper( from, new Nightshade(), count );
				from.SendMessage( AgriTxt.YouCut + " {0} fleur{1} de solanac�e.", count, ( count == 1 ? "" : "s" ) ); 
			}

		}

		[Constructable]
		public NightshadeUprooted() : this( 1 )
		{
		}

		[Constructable]
		public NightshadeUprooted( int amount ) : base( Utility.RandomList( 0x18E7, 0x18E8 ) )
		{
			Stackable = false;
			Weight = 1.0;
			
			Movable = true; 
			Amount = amount;

			Name = AgriTxt.Root + " de solanac�e";
		}

		public NightshadeUprooted( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
