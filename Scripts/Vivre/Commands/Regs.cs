using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Commands;

namespace Server.Commands
{
	public class CompteurReg
	{
		public static void Initialize()
		{
			CommandSystem.Register( "Regs", AccessLevel.Player, new CommandEventHandler( Regs_OnCommand ) );
		}
		
		[Usage( "Regs" )]
		[Description( "Compteur de Reactifs" )]
		public static void Regs_OnCommand( CommandEventArgs e )
		{
			Mobile somemobile = e.Mobile;
			somemobile.SendGump( new RegsGump(somemobile) );
		}
	}

	public class RegsGump : Gump
	{
		PlayerMobile m_From;
		int nb;
		
		private static Type[] m_Types_Mage = new Type[]
		{
			typeof( BlackPearl ), typeof( Bloodmoss ),
			typeof( Garlic ), typeof( Ginseng ),
			typeof( MandrakeRoot ), typeof( Nightshade ),
			typeof( SulfurousAsh ), typeof( SpidersSilk )
		};
		private static Type[] m_Types_Necro = new Type[]
		{
			typeof( BatWing ), typeof( DaemonBlood ),
			typeof( PigIron ), typeof( NoxCrystal ),
			typeof( GraveDust )
		};
		
		private static int[] m_Img_Mage = new int[]
		{
			0xF7A, 0xF7B, 0xF84, 0xF85, 0xF86, 0xF88, 0xF8C, 0xF8D,
		};
		private static int[] m_Img_Necro = new int[]
		{
			0xF78, 0xF7D, 0xF8A, 0xF8E, 0xF8F
		};
		
		private static string[] m_Txt_Mage = new string[]
		{
			"Perle noire", "Mousse de sang",
			"Ail", "Ginseng",
			"Mandragore", "Belladone",
			"Cendres sulfureuses", "Soie d'araignée"
		};

		private static string[] m_Txt_Necro = new string[]
		{
			"Aile de chauve-souris", "Fiole de sang",
			"Fonte brute", "Ecaille de serpent",
			"Cendre volcanique"
		};
		
		public RegsGump ( Mobile from ) : base ( 40, 40 )
		{
			m_From = from as PlayerMobile;
			
			m_From.CloseGump( typeof( RegsGump ) );
			
			Container backpack = m_From.Backpack;
			
			AddPage( 0 );
			AddBackground( 0, 0, 440, 270, 5054 );
			AddBlackAlpha( 10, 10, 420, 25 );
			AddBlackAlpha( 10, 45, 200, 215 );
			AddBlackAlpha( 220, 45, 200, 215 );
			
			AddLabel( 155, 14, 0x384, "Réactifs" );
			AddLabel( 100, 50, 0x284, "Mages" );
			AddLabel( 295, 50, 0x284, "Necros" );
			
			for( int i = 0; i < m_Types_Mage.Length; i++ )
			{
				nb = backpack.GetAmount( m_Types_Mage[i] );
				
				AddItem( 15, 70 + (i * 20), m_Img_Mage[i] );
				AddLabelCropped( 55, 70 + (i * 20) , 150, 21, 0x384, m_Txt_Mage[i] + " :" );
				AddLabelCropped( 183, 70 + (i * 20) , 46, 21, 0x284, nb.ToString() );
			}
			for( int i = 0; i < m_Types_Necro.Length; i++ )
			{
				nb = backpack.GetAmount( m_Types_Necro[i] );
				
				AddItem( 225, 70 + (i * 22), m_Img_Necro[i] );
				AddLabelCropped( 265, 70 + (i * 20) , 150, 21, 0x384, m_Txt_Necro[i] + " :" );
				AddLabelCropped( 395, 70 + (i * 20) , 46, 21, 0x284, nb.ToString() );
			}
		}

		public void AddBlackAlpha( int x, int y, int width, int height )
		{
			AddImageTiled( x, y, width, height, 2624 );
			AddAlphaRegion( x, y, width, height );
		}
		
		public override void OnResponse( NetState state, RelayInfo info )
		{
			
		}
	}
}
