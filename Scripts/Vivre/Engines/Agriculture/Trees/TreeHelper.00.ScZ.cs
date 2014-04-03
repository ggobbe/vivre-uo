using System;
using Server;
using System.Collections;
using Server.Network;
using Server.Gumps;
using Server.Items; 

namespace Server.Items.Crops 
{ 
	public enum TreeType
	{
		AppleTree,
		PearTree,
		PeachTree,
		CocoTree,
		BananaTree,
		KatylTree,
		OnaxTree
	}

	public class TreeHelper
	{ 
		public static bool CanPickMounted{ get{ return false; } } // If true Player can pick fruit while mounted
		public static bool TreeOrdinance{ get{ return false; } } // Criminal to Chop fruit trees in town.
		
		public static TimeSpan SaplingTime = TimeSpan.FromHours( 3 ); // Time spent as a Sapling
		public static TimeSpan StumpTime = TimeSpan.FromHours( 1 ); // Time spent as a Stump

		public class ChopAction : Timer
		{
			private Mobile m_chopper;
			private int cnt;

			public ChopAction( Mobile from ) : base( TimeSpan.FromMilliseconds( 900 ), TimeSpan.FromMilliseconds( 900 ) )
			{
				Priority = TimerPriority.TenMS;
				m_chopper = from;
				from.CantWalk = true;
				cnt = 1;
			}
			
			protected override void OnTick()
			{
				switch( cnt++ )
				{
					case 1: case 3: case 5:
					{
						m_chopper.Animate( 13, 7, 1, true, false, 0 ); // Chop
						break;
					}
					case 2: case 4:
					{
						Effects.PlaySound( m_chopper.Location, m_chopper.Map, 0x13E );
						break;
					}
					case 6:
					{
						Effects.PlaySound( m_chopper.Location, m_chopper.Map, 0x13E );
						m_chopper.CantWalk = false;
						this.Stop();
						break;
					}
				}
			}
		}

		public class TreeTimer : Timer 
		{ 
			private Item i_sapling; 
			private Type t_crop; 

			public TreeTimer( Item sapling, Type croptype, TimeSpan delay ) : base( delay ) 
			{ 
				Priority = TimerPriority.OneMinute; 

				i_sapling = sapling; 
				t_crop = croptype;
			} 

			protected override void OnTick() 
			{ 
				if (( i_sapling != null ) && ( !i_sapling.Deleted )) 
				{ 
					object[] args = { i_sapling.Location, i_sapling.Map };
					Item newitem = Activator.CreateInstance( t_crop, args ) as Item;

					i_sapling.Delete(); 
				} 
			} 
		} 

		public class GrowTimer : Timer 
		{ 
			private Item i_stump; 
			private Type t_tree; 
			private DateTime d_timerstart;
			private Item i_newtree;

			public GrowTimer( Item stump, Type treetype, TimeSpan delay ) : base( delay ) 
			{ 
				Priority = TimerPriority.OneMinute; 

				i_stump = stump; 
				t_tree = treetype;

				d_timerstart = DateTime.Now;
			}

			protected override void OnTick() 
			{ 
				Point3D pnt = i_stump.Location;
				Map map = i_stump.Map;

				if ( t_tree == typeof(PeachTree) )
					i_newtree = new PeachSapling();
					
				else if ( t_tree == typeof(PearTree) )
					i_newtree = new PearSapling();
					
				else if ( t_tree == typeof(CocoTree) )
					i_newtree = new CocoSapling();
					
				else if ( t_tree == typeof(BananaTree) )
					i_newtree = new BananaSapling();
					
				else if ( t_tree == typeof(KatylTree) )
					i_newtree = new KatylSapling();
					
				else if ( t_tree == typeof(OnaxTree) )
					i_newtree = new OnaxSapling();
					
				else 
					i_newtree = new AppleSapling();

				i_stump.Delete();
				i_newtree.MoveToWorld( pnt, map );
			} 
		} 
	}

	public class BaseTree : Item, IChopable
	{
		public BaseTree( int itemID ) : base( itemID )
		{
		}

		public BaseTree( Serial serial ) : base( serial ) 
		{ 
		} 

		public virtual void OnChop( Mobile from )
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
	
	public class TreeTrunk : Item, IChopable
	{
		private Item i_leaves;

		public Item Leaves{ get{ return i_leaves; } }

		public TreeTrunk( int itemID, Item TreeLeaves ) : base( itemID )
		{
			Movable = false;
			i_leaves = TreeLeaves;
		}

		public TreeTrunk( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void OnAfterDelete()
		{
			if (( i_leaves != null ) && ( !i_leaves.Deleted ))
				i_leaves.Delete();

			base.OnAfterDelete();
		}

		public void OnChop( Mobile from )
		{
			int testID = ((Item)i_leaves).ItemID;

			switch (testID)
			{
				case 0xD96: 
				case 0xD9A: 
				{ 
					AppleTree thistree = i_leaves as AppleTree;
					if ( thistree != null )
						thistree.Chop( from );
					break;
				}
				case 0xDAA: 
				case 0xDA6: 
				{ 
					PearTree thistree = i_leaves as PearTree;
					if ( thistree != null )
						thistree.Chop( from );
					break;
				}
				case 0xD9E: 
				case 0xDA2: 
				{ 
					PeachTree thistree = i_leaves as PeachTree;
					if ( thistree != null )
						thistree.Chop( from );
					break;
					
				}	
				case 0xCAA: 
				case 0xCA8: 
				{ 
					BananaTree thistree = i_leaves as BananaTree;
					if ( thistree != null )
						thistree.Chop( from );
					break;
					
				}	
				case 0xC96: 
				case 0xC95: 
				{ 
					CocoTree thistree = i_leaves as CocoTree;
					if ( thistree != null )
						thistree.Chop( from );
					break;

				}	
	
				case 0xC9E: 
				//case 0xC9E: 
				{ 
					OnaxTree thistree = i_leaves as OnaxTree;
					if ( thistree != null )
						thistree.Chop( from );
					break;
						
				}	
				case 0xD37: 
				case 0xD38: 
				{ 
					KatylTree thistree = i_leaves as KatylTree;
					if ( thistree != null )
						thistree.Chop( from );
					break;
						
					
					
					
				}
			}
		}
			
		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 0 ); 

			writer.Write( (Item)i_leaves ); 
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 

			Item item = reader.ReadItem();
			if ( item != null )
				i_leaves = item;
		} 
	}
	
	public class FruitTreeStump : Item
	{
		private Type t_treeType;
		private int e_tree;
		public Timer thisTimer;
		public DateTime treeTime;

		[CommandProperty( AccessLevel.GameMaster )]
		public String Sapling{ get{ return treeTime.ToString( "T" ); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public String Type
		{ get
			{
				switch( e_tree )
				{
					case (int)TreeType.AppleTree:		return "Pommier";	
					case (int)TreeType.PearTree:		return "Poirier";		
					case (int)TreeType.PeachTree:		return "Pêcher";
					case (int)TreeType.CocoTree:		return "Cocotier";
					case (int)TreeType.BananaTree:		return "Bananier";
					case (int)TreeType.KatylTree:		return "Katyliis";
					case (int)TreeType.OnaxTree:		return "TolOnax";
					
					default:	return "Error Bad Treetype";
				}
			} 
		}

		[Constructable] 
		public FruitTreeStump( Type FruitTree ) : base( 0xDAC )
		{
			Movable = false;
			Hue = 0x74E;
			Name = "Tronc d'arbre Coupé";

			t_treeType = FruitTree;

			if ( FruitTree == typeof( PearTree ) )
				e_tree = (int)TreeType.PearTree;
				
			else if ( FruitTree == typeof( PeachTree ) )
				e_tree = (int)TreeType.PeachTree;

			else if ( FruitTree == typeof( CocoTree ) )
				e_tree = (int)TreeType.CocoTree;

			else if ( FruitTree == typeof( BananaTree ) )
				e_tree = (int)TreeType.BananaTree;

			else if ( FruitTree == typeof( KatylTree ) )
				e_tree = (int)TreeType.KatylTree;

			else if ( FruitTree == typeof( OnaxTree ) )
				e_tree = (int)TreeType.OnaxTree;
				
			else 
				e_tree = (int)TreeType.AppleTree;

			init( this );
		}

		public static void init( FruitTreeStump plant )
		{
			TimeSpan delay = TreeHelper.StumpTime;
			plant.treeTime = DateTime.Now + delay;

			plant.thisTimer = new TreeHelper.GrowTimer( plant, plant.t_treeType, delay ); 
			plant.thisTimer.Start(); 
		}

		public FruitTreeStump( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 0 ); 

			writer.Write( e_tree ); 
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 

			int e_tree = reader.ReadInt();
			switch( e_tree )
			{
				case (int)TreeType.AppleTree:		t_treeType = typeof(AppleTree);	break;
				case (int)TreeType.PearTree:		t_treeType = typeof(PearTree);	break;
				case (int)TreeType.PeachTree:		t_treeType = typeof(PeachTree);	break;
				case (int)TreeType.CocoTree:		t_treeType = typeof(CocoTree);	break;
				case (int)TreeType.BananaTree:		t_treeType = typeof(BananaTree); break;
				case (int)TreeType.KatylTree:		t_treeType = typeof(KatylTree);	break;
				case (int)TreeType.OnaxTree:		t_treeType = typeof(OnaxTree);	break;
			}

			init( this );
		} 
	}
}


