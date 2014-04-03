using System;
using System.Collections;
using Server;
using Server.Engines.Plants;
using Server.Gumps;
using Server.Items;
using Server.Multis;
using System.Collections.Generic;
using Server.ContextMenus;

/***************************************
 * SeedBox by Kelon, 18.Aug.2005
 * *************************************
 * A box to store all seeds more easily
 * ************************************/

namespace Server.Items
{
	public class SeedBox : Item, ISecurable
	{
		/// <summary>
		/// Maximum number of seeds that can be stored in a SeedBox
		/// </summary>
		private const int MaxStorage = 10000;

		private int m_Stored;
		private ArrayList m_KnownStorage;
		private ArrayList m_UnknownStorage;
		private SecureLevel m_Level;

		/// <summary>
		/// Number of stored seeds
		/// </summary>
		public int Stored
		{
			get { return m_Stored; }
			set
			{
				m_Stored = value;
				Weight = 3 + m_Stored * 0.03;
				InvalidateProperties();
			}
		}

		/// <summary>
		/// Seeds, that are showing their type
		/// </summary>
		public ArrayList KnownStorage
		{
			get { return m_KnownStorage; }
		}

		/// <summary>
		/// Seeds, that are not showing their type
		/// </summary>
		public ArrayList UnknownStorage
		{
			get { return m_UnknownStorage; }
		}

		#region ISecurable Members

		[CommandProperty( AccessLevel.GameMaster )]
		public SecureLevel Level
		{
			get{ return m_Level; }
			set{ m_Level = value; }
		}

		#endregion

		public override int LabelNumber {get{	return 3010154;	}} // Plant Seeds

		[Constructable]
		public SeedBox() : base( 0x09B1 ) // Basket graphics
		{
			Weight = 3;
			Hue = 0x084F;
			m_KnownStorage = new ArrayList();
			m_UnknownStorage = new ArrayList();
			m_Stored = 0;
		}

		public SeedBox( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			int stored = 0; // recalculate to ensure
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			writer.Write( (int)m_Level );
			writer.Write( m_UnknownStorage.Count );
			foreach( SeedInfo si in m_UnknownStorage )
			{
				writer.Write( (int)si.Type );
				writer.Write( si.Hues.Count );
				foreach( SeedHue sh in si.Hues )
				{
					writer.Write( (int)sh.Hue );
					writer.Write( (int)sh.Amount );
					stored += sh.Amount;
				}
			}

			writer.Write( m_KnownStorage.Count );
			foreach( SeedInfo si in m_KnownStorage )
			{
				writer.Write( (int)si.Type );
				writer.Write( si.Hues.Count );
				foreach( SeedHue sh in si.Hues )
				{
					writer.Write( (int)sh.Hue );
					writer.Write( (int)sh.Amount );
					stored += sh.Amount;
				}
			}
			m_Stored = stored;
		}

		public override void Deserialize( GenericReader reader )
		{
			int stored = 0;
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_Level = (SecureLevel)reader.ReadInt();
			m_UnknownStorage = new ArrayList();

			for( int i = reader.ReadInt(); i > 0 ; i-- )
			{
				SeedInfo si = new SeedInfo( (PlantType)reader.ReadInt() );
				for( int c = reader.ReadInt(); c > 0; c-- )
				{
					SeedHue sh = new SeedHue( (PlantHue)reader.ReadInt(), reader.ReadInt() );
					si.Hues.Add( sh );
					stored += sh.Amount;
				}
				m_UnknownStorage.Add( si );
			}

			m_KnownStorage = new ArrayList();
			for( int i = reader.ReadInt(); i > 0; i-- )
			{
				SeedInfo si = new SeedInfo( (PlantType)reader.ReadInt() );
				for( int c = reader.ReadInt(); c > 0; c-- )
				{
					SeedHue sh = new SeedHue( (PlantHue)reader.ReadInt(), reader.ReadInt() );
					si.Hues.Add( sh );
					stored += sh.Amount;
				}
				m_KnownStorage.Add( si );
			}
			m_Stored = stored;
		}

		public override void AddNameProperties(ObjectPropertyList list)
		{
			base.AddNameProperties (list);
			list.Add( 1060838, m_Stored.ToString() ); // ~1_val~ seed
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );
			SetSecureLevelEntry.AddTo( from, this, list );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="from"></param>
		/// <param name="dropped"></param>
		/// <returns>Since this isn't a container, it's false</returns>
		public override bool OnDragDrop(Mobile from, Item dropped)
		{
			Seed drop = dropped as Seed;
			if( drop == null ) // Must be a seed
			{
				from.SendLocalizedMessage( 1042276 ); // You cannot drop that there.
				return false;
			}
			if( m_Stored < MaxStorage )
			{
				AddSeed( drop );
				drop.Delete();
				return false;
			}
			from.SendLocalizedMessage( 1042972 ); // It's full.
			return false;
			
		}

		public override void OnDoubleClick(Mobile from)
		{
			if( from.InRange( this.GetWorldLocation(), 3 ) && from.InLOS( this.GetWorldLocation() ) )
				from.SendGump( new SeedBoxGump( from, this ) );
			else
				from.SendLocalizedMessage( 500446 ); // That is too far away.
		}

		/// <summary>
		/// Adds a seed to the storage list
		/// </summary>
		/// <param name="seed">Item</param>
		public void AddSeed( Seed seed )
		{
			// First, we start searching for similiar seeds
			if( seed.ShowType )
			{
				foreach( SeedInfo fsi in m_KnownStorage )
				{
					if( fsi.Type == seed.PlantType )
					{
						fsi.Add( seed.PlantHue, 1);
						Stored++;
						return;
					}
				}
				// Second, if no similiar seeds were found, we're adding this one.
				SeedInfo ssi = new SeedInfo( seed.PlantType ); // Damn scope bug!
				ssi.Add( seed.PlantHue, 1 );
				m_KnownStorage.Add( ssi );
				m_KnownStorage.Sort( new PlantTypeComparer() );
				Stored++;
			}
			else
			{
				foreach( SeedInfo fsi in m_UnknownStorage )
				{
					if( fsi.Type == seed.PlantType )
					{
						fsi.Add( seed.PlantHue, 1);
						Stored++;
						return;
					}
				}
				// Second, if no similiar seeds were found, we're adding this one.
				SeedInfo ssi = new SeedInfo( seed.PlantType );
				ssi.Add( seed.PlantHue, 1 );
				// No sorting with unknown seeds
				m_UnknownStorage.Add( ssi );
				Stored++;
			}
		}

		private class PlantTypeComparer : IComparer
		{
			public int Compare( object x, object y)
			{
				return Compare( (int)((SeedInfo)x).Type, (int)((SeedInfo)y).Type );
			}
			public int Compare( int x, int y)
			{
				return x - y;
			}
		}

		private class PlantHueComparer : IComparer
		{
			public int Compare( object x, object y)
			{
				return Compare( (int)((SeedHue)x).Hue, (int)((SeedHue)y).Hue );
			}
			public int Compare( int x, int y)
			{	
				x = x | 0x4000000; // ignore non-crossables
				y = y | 0x4000000;
				return x - y;
			}
		}

		/// <summary>
		/// Information about the stores seeds
		/// </summary>
		private class SeedInfo
		{
			public PlantType Type;
			public ArrayList Hues;

			public SeedInfo( PlantType type )
			{
				Type = type;
				Hues = new ArrayList();
			}

			/// <summary>
			/// Adds a seed to the list
			/// </summary>
			/// <param name="hue"></param>
			/// <param name="amount"></param>
			public void Add( PlantHue hue, int amount )
			{
				foreach( SeedHue si in Hues )
				{
					if( si.Hue == hue )
					{
						si.Amount += amount;
						return;
					}
				}
				Hues.Add( new SeedHue( hue, amount ) );
				Hues.Sort( new PlantHueComparer() );
			}
		}

		private class SeedHue
		{
			public PlantHue Hue;
			public int Amount;

			public SeedHue( PlantHue hue, int amount )
			{
				Hue = hue;
				Amount = amount;
			}
		}

		/// <summary>
		/// The gump to get the seeds
		/// </summary>
		private class SeedBoxGump : Gump
		{
			private Mobile m_Owner;
			private SeedBox m_Box;
			
			private SeedInfo seedinfo;//#01 
			private bool unknownstorage;//#01
			
			public SeedBoxGump( Mobile owner, SeedBox box ) : this( owner, box, null, true){}//#01
			
			//public SeedBoxGump( Mobile owner, SeedBox box ) : base( 0, 0 )//#01
			public SeedBoxGump( Mobile owner, SeedBox box, SeedInfo si, bool unknown ) : base( 0, 0 )
			{
				m_Owner = owner;
				m_Box = box;
				Closable = true;
				Disposable = true;
				Dragable = true;
				Resizable = false;
				
				seedinfo = si;//#01
				unknownstorage = unknown;//#01
								
				if( box == null || box.Deleted )
					return;

				int max = AddBackground(); // Custom method to calculate size
				AddBackground( 170, 40, 82, 80, 9350 ); // Image window

				AddPages( max );

			}

			/// <summary>
			/// Custom method to calculate the minimal size
			/// </summary>
			/// <returns>Possible items per row</returns>
			private int AddBackground()
			{
				int maxhues = 0;
				int maxperrow = 0;
				int typesadd = m_Box.KnownStorage.Count - 5;
				foreach( SeedInfo si in m_Box.UnknownStorage )
					maxhues += si.Hues.Count;
				foreach( SeedInfo si in m_Box.KnownStorage )
					if( si.Hues.Count > maxhues )
						maxhues = si.Hues.Count;

				maxhues = ( maxhues / 2 + maxhues % 2 ) - 3; 
				maxperrow = ( maxhues > typesadd ? maxhues : typesadd ); // subtract 3 for backgrounds size calculation
				maxperrow = maxperrow > 0 ? maxperrow : 0;
				int verticaladd = maxperrow * 20;
				AddPage( 0 );
				AddBackground( 20, 20, 450, 187 + verticaladd, 3500 ); // White paper background
				AddImageTiled( 150, 30, 20, 167 + verticaladd, 3503 ); // White paper separator
				AddImageTiled( 250, 50, 210, 17, 1803 ); // White paper horizontal bar
				AddItem( 40, 170 + verticaladd, 3224, 0 ); // Large fern deco
				return maxperrow + 3;
			}

			/// <summary>
			/// Adds the page buttons (PlantTypes).
			/// </summary>
			private void AddPages( int MaxPerRow )
			{
				int pageindex = 1;
				int itemindex = 0;
				int buttonindex = 1;
				int verticaladd = 0; // temporary
				
				//#01 début
				if(seedinfo != null && !unknownstorage)//si un type en mémoire et que ce n'est pas l'unknown
				{
					PlantTypeInfo pii = null;
					foreach( SeedInfo si in m_Box.KnownStorage )
					{
						if(si.Type == seedinfo.Type)//on met sa page en premier dans la liste
						{
							pii = PlantTypeInfo.GetInfo( si.Type );
							AddButton( 37, 55, 2103, 2104, 0, GumpButtonType.Page, pageindex ); // Type button
							AddHtmlLocalized( 50, 50, 110, 20, pii.Name, false, false ); // Plant name
							pageindex++;
						}
					}
					if( m_Box.UnknownStorage.Count > 0 )//et donc les unknown en second
					{
						AddButton( 37, 35 + pageindex * 20, 2103, 2104, 0, GumpButtonType.Page, pageindex ); // Type button
						AddHtmlLocalized( 50, 30 + pageindex * 20, 110, 20, 1060800, false, false ); // Plant name: unknown
						pageindex++;
					}
				}
				else//sinon comme d'hab
				{
					//#01 fin
					if( m_Box.UnknownStorage.Count > 0 )
					{
						AddButton( 37, 55, 2103, 2104, 0, GumpButtonType.Page, pageindex ); // Type button
						AddHtmlLocalized( 50, 50, 110, 20, 1060800, false, false ); // Plant name: unknown
						pageindex++;
					}
				}//#01 fini le else

				PlantTypeInfo pi = null;
				
				//#01 début
				if(seedinfo != null)//toujours si un type en memoire
				{
					foreach( SeedInfo si in m_Box.KnownStorage )//le reste de la liste SANS celui en mémoire puisqu'il est dejà en premier
					{
						if(si.Type != seedinfo.Type)
						{
							pi = PlantTypeInfo.GetInfo( si.Type );
							AddButton( 37, 35 + pageindex * 20, 2103, 2104, 0, GumpButtonType.Page, pageindex ); // Type button
							AddHtmlLocalized( 50, 30 + pageindex * 20, 110, 20, pi.Name, false, false ); // Plant name
							pageindex++;
						}
					}
				}
				else 
				{//#01 fin
					foreach( SeedInfo si in m_Box.KnownStorage )
					{
						pi = PlantTypeInfo.GetInfo( si.Type );
						AddButton( 37, 35 + pageindex * 20, 2103, 2104, 0, GumpButtonType.Page, pageindex ); // Type button
						AddHtmlLocalized( 50, 30 + pageindex * 20, 110, 20, pi.Name, false, false ); // Plant name
						pageindex++;
					}
				}//#01 fini le else
				
				pageindex = 1; // reset

				PlantHueInfo ph = null;
				
				//#01 début
				
				if(seedinfo != null && !unknownstorage)//si un type en memoire, sa page est la première, 
														//mais si c'est l'unknown, on fait comme d'hab (elle est la premiere)
				{
					PlantType planttype = seedinfo.Type;
					
					foreach( SeedInfo si in m_Box.KnownStorage )
					{
						if(si.Type == planttype)
						{
							pi = PlantTypeInfo.GetInfo( si.Type );
							AddPage( 1);
							AddItem( 187 + pi.OffsetX, 50 + pi.OffsetY, pi.ItemID, 0 ); // Plant picture
							AddHtmlLocalized( 264, 45, 170, 20, pi.Name, false, false ); // Type description

							itemindex = 0;
							verticaladd = 0;
							foreach( SeedHue sh in si.Hues )
							{
								ph = PlantHueInfo.GetInfo( sh.Hue );
								AddButton( 170 + verticaladd, 130 + itemindex * 20, 22407, 22406, buttonindex++, GumpButtonType.Reply, 0 ); // Take button
								AddLabel( 190 + verticaladd, 130 + itemindex * 20, 0x835, sh.Amount.ToString() );
						
								if( (int)sh.Hue < 0x8000000 )
									AddHtmlLocalized( 220 + verticaladd, 130 + itemindex * 20, 100, 20, ph.Name, false, false ); // Hue description
								else
								{
									AddLabel( 220 + verticaladd, 130 + itemindex * 20, 0, "bright" );
									AddHtmlLocalized( 260 + verticaladd, 130 + itemindex * 20, 100, 20, ph.Name, false, false ); // Hue description
								}
								itemindex++;
								if( itemindex >= MaxPerRow ) // start next row
								{
									itemindex = 0;
									verticaladd = 150;
								}
							}
							pageindex++;
						}
					}
				}
				//#01 fin
				
				if( m_Box.UnknownStorage.Count > 0 )
				{
					AddPage( pageindex );
					AddItem( 190, 60, 1, 0 ); // Plant picture
					AddHtmlLocalized( 264, 45, 170, 20, 3000575, false, false ); // Type description: UNKNOWN

					// Unknown seeds are displayed in a single category
					foreach( SeedInfo si in m_Box.UnknownStorage )
					{
						foreach( SeedHue sh in si.Hues )
						{
							ph = PlantHueInfo.GetInfo( sh.Hue );
							AddButton( 170 + verticaladd, 130 + itemindex * 20, 22407, 22406, buttonindex++, GumpButtonType.Reply, 0 ); // Take button
							AddLabel( 190 + verticaladd, 130 + itemindex * 20, 0x835, sh.Amount.ToString() );
							AddHtmlLocalized( 220 + verticaladd, 130 + itemindex * 20, 100, 20, ph.Name, false, false ); // Hue description
							itemindex++;
							if( itemindex >= MaxPerRow ) // Start next row
							{
								itemindex = 0;
								verticaladd = 150;
							}
						}
					}
					pageindex++;
				}

				foreach( SeedInfo si in m_Box.KnownStorage )
				{
					if(seedinfo == null || si.Type != seedinfo.Type)//#01
					{
					
					pi = PlantTypeInfo.GetInfo( si.Type );
					AddPage( pageindex );
					AddItem( 187 + pi.OffsetX, 50 + pi.OffsetY, pi.ItemID, 0 ); // Plant picture
					AddHtmlLocalized( 264, 45, 170, 20, pi.Name, false, false ); // Type description

					itemindex = 0;
					verticaladd = 0;
					foreach( SeedHue sh in si.Hues )
					{
						ph = PlantHueInfo.GetInfo( sh.Hue );
						AddButton( 170 + verticaladd, 130 + itemindex * 20, 22407, 22406, buttonindex++, GumpButtonType.Reply, 0 ); // Take button
						AddLabel( 190 + verticaladd, 130 + itemindex * 20, 0x835, sh.Amount.ToString() );
						
						if( (int)sh.Hue < 0x8000000 )
							AddHtmlLocalized( 220 + verticaladd, 130 + itemindex * 20, 100, 20, ph.Name, false, false ); // Hue description
						else
						{
							AddLabel( 220 + verticaladd, 130 + itemindex * 20, 0, "bright" );
							AddHtmlLocalized( 260 + verticaladd, 130 + itemindex * 20, 100, 20, ph.Name, false, false ); // Hue description
						}
						itemindex++;
						if( itemindex >= MaxPerRow ) // start next row
						{
							itemindex = 0;
							verticaladd = 150;
						}
					}
					pageindex++;
					}//#01 fin du if
				}

				// Check if something was added
				if( pageindex == 1 )
				{
					AddPage( pageindex );
					AddImage( 175, 45, 7012, 2406 );
					AddHtmlLocalized( 264, 45, 170, 20, 501038, false, false ); // Claim List is empty
				}
			}

			public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
			{
				if( m_Box == null || m_Box.Deleted || info.ButtonID == 0 )
					return;
				
				Mobile from = sender.Mobile;
				
				int curritem = 1;
				
				SeedInfo inf = null;
				
				if(seedinfo != null && !unknownstorage)
				{
					PlantType planttype = seedinfo.Type;
					
					foreach( SeedInfo si in m_Box.KnownStorage )
					{
						if(si.Type == planttype)
						{
							foreach( SeedHue sh in si.Hues )
							{
								if( info.ButtonID == curritem )
								{
									if( AddToPack( from, si.Type, sh.Hue, true ) )
									{
										sh.Amount--;
										m_Box.Stored--;
										if( sh.Amount < 1 )
											si.Hues.Remove( sh );
										if( si.Hues.Count < 1 )
											m_Box.UnknownStorage.Remove( si );
									}
									//from.SendGump( new SeedBoxGump( from, m_Box ) );//#01
									inf = si;//#01
									if(si.Hues.Count < 1 )inf = null;//#01
									from.SendGump( new SeedBoxGump( from, m_Box, inf, false) );//#01
									return;
								}
								curritem++;
							}
						}
					}
				}
				
				foreach( SeedInfo si in m_Box.UnknownStorage )
				{
					foreach( SeedHue sh in si.Hues )
					{
						if( info.ButtonID == curritem )
						{
							if( AddToPack( from, si.Type, sh.Hue, false ) )
							{
								sh.Amount--;
								m_Box.Stored--;
								if( sh.Amount < 1 )
									si.Hues.Remove( sh );
								if( si.Hues.Count < 1 )
									m_Box.UnknownStorage.Remove( si );
							}
							//from.SendGump( new SeedBoxGump( from, m_Box ) );//#01
							inf = si;//#01
							if(si.Hues.Count < 1 )inf = null;//#01
							from.SendGump( new SeedBoxGump( from, m_Box, inf, true) );//#01
							return;
						}
						curritem++;
					}
				}
				foreach( SeedInfo si in m_Box.KnownStorage )
				{
					if(seedinfo == null || si.Type != seedinfo.Type)//#01
					{
					
					foreach( SeedHue sh in si.Hues )
					{
						if( info.ButtonID == curritem )
						{
							if( AddToPack( from, si.Type, sh.Hue, true ) )
							{
								sh.Amount--;
								m_Box.Stored--;
								if( sh.Amount < 1 )
									si.Hues.Remove( sh );
								if( si.Hues.Count < 1 )
									m_Box.KnownStorage.Remove( si );
							}
							//from.SendGump( new SeedBoxGump( from, m_Box ) );//#01
							inf = si;//#01
							if(si.Hues.Count < 1 )inf = null;//#01
							from.SendGump( new SeedBoxGump( from, m_Box, inf, false) );//#01
							return;
						}
						curritem++;
					}
					}//#01 fin du if
				}
				from.SendLocalizedMessage( 500065 ); // Object not found
			}

			private bool AddToPack( Mobile from, PlantType type, PlantHue hue, bool showtype )
			{
				if( from == null || from.Deleted || from.Backpack == null || from.Backpack.Deleted )
					return false;

				if( !from.InRange( m_Box.GetWorldLocation(), 3 ) || !from.InLOS( m_Box.GetWorldLocation() ) )
				{
					from.SendLocalizedMessage( 500446 ); // That is too far away.
					return false;
				}

				Seed seed = new Seed( type, hue, showtype );
				if( !from.Backpack.TryDropItem( from, seed, true ) )
				{
					seed.MoveToWorld( from.Location, from.Map );
				}
				return true;
			}
		}
	}
}


















