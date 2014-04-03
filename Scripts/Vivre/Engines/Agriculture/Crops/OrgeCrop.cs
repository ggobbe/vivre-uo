using System; 
using System.Collections;
using Server.Network; 
using Server.Mobiles; 
using Server.Items; 
using Server.Gumps;

namespace Server.Items.Crops 
{ 
	public class OrgeSeed : BaseCrop 
	{ 
		// return true to allow planting on Dirt Item (ItemID 0x32C9)
		// See CropHelper.cs for other overriddable types
		public override bool CanGrowGarden{ get{ return true; } }
		
		[Constructable]
		public OrgeSeed() : this( 1 )
		{
		}

		[Constructable]
		public OrgeSeed( int amount ) : base( 0xF27 )
		{
			Stackable = true; 
			Weight = .5; 
			Hue = 0x5E2; 

			Movable = true; 
			
			Amount = amount;
			Name = "Semence d'orge"; 
		}

		public override void OnDoubleClick( Mobile from ) 
		{ 
			if ( from.Mounted && !CropHelper.CanWorkMounted )
			{
				from.SendMessage( "Vous ne pouvez planter sur ce type de terrain" ); 
				return; 
			}

			Point3D m_pnt = from.Location;
			Map m_map = from.Map;

			if ( !IsChildOf( from.Backpack ) ) 
			{ 
				from.SendLocalizedMessage( 1042010 ); //You must have the object in your backpack to use it. 
				return; 
			} 

			else if ( !CropHelper.CheckCanGrow( this, m_map, m_pnt.X, m_pnt.Y ) )
			{
				from.SendMessage( "Cette graine ne germera pas ici" ); 
				return; 
			}
			
			//check for BaseCrop on this tile
			ArrayList cropshere = CropHelper.CheckCrop( m_pnt, m_map, 0 );
			if ( cropshere.Count > 0 )
			{
				from.SendMessage( "Il y a deja une graine de plante ici." ); 
				return;
			}

			//check for over planting prohibt if 4 maybe 3 neighboring crops
			ArrayList cropsnear = CropHelper.CheckCrop( m_pnt, m_map, 1 );
			if ( ( cropsnear.Count > 3 ) || (( cropsnear.Count == 3 ) && Utility.RandomBool() ) )
			{
				from.SendMessage( "Il y a trop de pouce ici." ); 
				return;
			}

			if ( this.BumpZ ) ++m_pnt.Z;

			if ( !from.Mounted )
				from.Animate( 32, 5, 1, true, false, 0 ); // Bow

			from.SendMessage("Vous plantez la graine."); 
			this.Consume(); 
			Item item = new OrgeSeedling( from ); 
			item.Location = m_pnt; 
			item.Map = m_map; 
		
		} 

		public OrgeSeed( Serial serial ) : base( serial ) 
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


	public class OrgeSeedling : BaseCrop 
	{ 
		private static Mobile m_sower;
		public Timer thisTimer;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Sower{ get{ return m_sower; } set{ m_sower = value; } }
		
		[Constructable] 
		public OrgeSeedling( Mobile sower ) : base( Utility.RandomList ( 0xDAE, 0xDAF ) ) 
		{ 
			Movable = false; 
			Name = "Orge pouce"; 
			m_sower = sower;
			this.Hue = 0x28E;
			init( this );
		} 

		public static void init( OrgeSeedling plant )
		{

			plant.thisTimer = new CropHelper.GrowTimer( plant, typeof(OrgeCrop), plant.Sower ); 
			plant.thisTimer.Start(); 
		}

		public override void OnDoubleClick( Mobile from ) 
		{ 
			if ( from.Mounted && !CropHelper.CanWorkMounted )
			{
				from.SendMessage( "Vous ne pouvez cultiver ici" ); 
				return; 
			}

			if ( ( Utility.RandomDouble() <= .25 ) && !( m_sower.AccessLevel > AccessLevel.Counselor ) ) 
			{ //25% Chance
				from.SendMessage( "Vous arrachez la pouce" ); 
				thisTimer.Stop();
				this.Delete();
			}
			else from.SendMessage( "La pouce est trop jeune pour etre arrache" ); 
		}

		public OrgeSeedling( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 0 ); 
			writer.Write( m_sower );
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 
			m_sower = reader.ReadMobile();

			init( this );
		} 
	} 

	public class OrgeCrop : BaseCrop 
	{ 
		private const int max = 10;
		private int fullGraphic;
		private int pickedGraphic;
		private DateTime lastpicked;

		private Mobile m_sower;
		private int m_yield;

		public Timer regrowTimer;

		private DateTime m_lastvisit;

		[CommandProperty( AccessLevel.GameMaster )] 
		public DateTime LastSowerVisit{ get{ return m_lastvisit; } }

		[CommandProperty( AccessLevel.GameMaster )] 
		public bool Growing{ get{ return regrowTimer.Running; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Sower{ get{ return m_sower; } set{ m_sower = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int Yield{ get{ return m_yield; } set{ m_yield = value; } }

		public int Capacity{ get{ return max; } }
		public int FullGraphic{ get{ return fullGraphic; } set{ fullGraphic = value; } }
		public int PickGraphic{ get{ return pickedGraphic; } set{ pickedGraphic = value; } }
		public DateTime LastPick{ get{ return lastpicked; } set{ lastpicked = value; } }
		
		[Constructable] 
		public OrgeCrop( Mobile sower ) : base( Utility.RandomList( 0xC55, 0xC56 ) ) 
		{ 
			Movable = false; 
			Name = "Orge Plante"; 

			m_sower = sower;
			m_lastvisit = DateTime.Now;

			init( this, false );
		}

		public static void init ( OrgeCrop plant, bool full )
		{
			plant.PickGraphic = Utility.RandomList( 0xC55, 0xC56, 0xC57, 0xC59 );
			plant.FullGraphic = Utility.RandomList( 0xC58, 0xC5A, 0xC5B );
			plant.Hue = 0x28E;
			
			plant.LastPick = DateTime.Now;
			plant.regrowTimer = new CropTimer( plant );

			if ( full )
			{
				plant.Yield = plant.Capacity;
				((Item)plant).ItemID = plant.FullGraphic;
			}
			else
			{
				plant.Yield = 0;
				((Item)plant).ItemID = plant.PickGraphic;
				plant.regrowTimer.Start();
			}
		}
		
		public void UpRoot( Mobile from )
		{
			from.SendMessage( "The crop withers away." ); 
			if ( regrowTimer.Running )
				regrowTimer.Stop();

			this.Delete();
		}

		public override void OnDoubleClick( Mobile from ) 
		{ 
			if ( m_sower == null || m_sower.Deleted ) 
				m_sower = from;

			if ( from.Mounted && !CropHelper.CanWorkMounted )
			{
				from.SendMessage( "Vous ne pouvez cultiver ici" ); 
				return; 
			}

			if ( DateTime.Now > lastpicked.AddSeconds(3) ) // 3 seconds between picking
			{
				lastpicked = DateTime.Now;
			
				int lumberValue = (int)from.Skills[SkillName.Lumberjacking].Value / 5;
				if ( lumberValue == 0 )
				{
					from.SendMessage( "Vous ne savez pas comment cultiver cela" ); 
					return;
				}

				if ( from.InRange( this.GetWorldLocation(), 2 ) ) 
				{ 
					if ( m_yield < 1 )
					{
						from.SendMessage( "Il n y a rien ici a cultiver" ); 

						if ( PlayerCanDestroy && !( m_sower.AccessLevel > AccessLevel.Counselor ) )
						{  
							UpRootGump g = new UpRootGump( from, this );
							from.SendGump( g );
						}
					}
					else //check skill and sower
					{
						from.Direction = from.GetDirectionTo( this );

						from.Animate( from.Mounted ? 29:32, 5, 1, true, false, 0 ); 

						if ( from == m_sower ) 
						{
							lumberValue *= 2;
							m_lastvisit = DateTime.Now;
						}

						if ( lumberValue > m_yield ) 
							lumberValue = m_yield + 1;

						int pick = Utility.Random( lumberValue );
						if ( pick == 0 )
						{
							from.SendMessage( "vous n obtenez pas de nouvelle pouce" ); 
							return;
						}
					
						m_yield -= pick;
						from.SendMessage( "Vous Cultivez {0} crop{1}!", pick, ( pick == 1 ? "" : "s" ) ); 

						//PublicOverheadMessage( MessageType.Regular, 0x3BD, false, string.Format( "{0}", m_yield )); // use for debuging
						((Item)this).ItemID = pickedGraphic;

						// ********************************
						// *** Orge does not yet exist ***
						// ********************************
						// Orge crop = new Orge( pick ); 
						OrgeGerbe crop = new OrgeGerbe( pick ); 
						from.AddToBackpack( crop );

						if ( SowerPickTime != TimeSpan.Zero && m_lastvisit + SowerPickTime < DateTime.Now && !( m_sower.AccessLevel > AccessLevel.Counselor ) )
						{
							this.UpRoot( from );
							return;
						}

						if ( !regrowTimer.Running )
						{
							regrowTimer.Start();
						}
					}
				} 
				else 
				{ 
					from.SendMessage( "Vous etes trop loin pour cultiver" ); 
				} 
			}
		} 

		private class CropTimer : Timer
		{
			private OrgeCrop i_plant;

			public CropTimer( OrgeCrop plant ) : base( TimeSpan.FromSeconds( 600 ), TimeSpan.FromSeconds( 15 ) )
			{
				Priority = TimerPriority.OneSecond;
				i_plant = plant;
			}

			protected override void OnTick()
			{
				if ( ( i_plant != null ) && ( !i_plant.Deleted ) )
				{
					int current = i_plant.Yield;

					if ( ++current >= i_plant.Capacity )
					{
						current = i_plant.Capacity;
						((Item)i_plant).ItemID = i_plant.FullGraphic;
						Stop();
					}
					else if ( current <= 0 )
						current = 1;

					i_plant.Yield = current;
					//i_plant.PublicOverheadMessage( MessageType.Regular, 0x22, false, string.Format( "{0}", current )); // use for debuging
				}
				else Stop();
			}
		}

		public OrgeCrop( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 1 ); 
			writer.Write( m_lastvisit );
			writer.Write( m_sower );
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 
			switch ( version )
			{
				case 1:
				{
					m_lastvisit = reader.ReadDateTime();
					goto case 0;
				}
				case 0:
				{
					m_sower = reader.ReadMobile();
					break;
				}
			}

			if ( version == 0 ) 
				m_lastvisit = DateTime.Now;

			init( this, true );
		} 
	} 
} 
