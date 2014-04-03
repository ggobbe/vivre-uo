using System; 
using Server.Items; 
using Server.Items.Crops;
using Server.Engines.Plants;
using System.Collections.Generic;

namespace Server.Mobiles 
{ 
	public class SBFarmHand : SBInfo 
	{ 
		private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBFarmHand() 
		{ 
		}

        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
        public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

        public class InternalBuyInfo : List<GenericBuyInfo>
		{ 
			public InternalBuyInfo() 
			{
                Add(new GenericBuyInfo("Pelle laboureuse", typeof(PelleLaboureuseB), 50, 10, 0xF39, 0));
				Add( new GenericBuyInfo( typeof( Apple ), 3, 20, 0x9D0, 0 ) );
				Add( new GenericBuyInfo( typeof( Grapes ), 3, 20, 0x9D1, 0 ) );
				Add( new GenericBuyInfo( typeof( Watermelon ), 7, 20, 0xC5C, 0 ) );
				Add( new GenericBuyInfo( typeof( YellowGourd ), 3, 20, 0xC64, 0 ) );
				Add( new GenericBuyInfo( typeof( Pumpkin ), 11, 20, 0xC6A, 0 ) );
				Add( new GenericBuyInfo( typeof( Onion ), 3, 20, 0xC6D, 0 ) );
				Add( new GenericBuyInfo( typeof( Lettuce ), 5, 20, 0xC70, 0 ) );
				Add( new GenericBuyInfo( typeof( Squash ), 3, 20, 0xC72, 0 ) );
				Add( new GenericBuyInfo( typeof( HoneydewMelon ), 7, 20, 0xC74, 0 ) );
				Add( new GenericBuyInfo( typeof( Carrot ), 3, 20, 0xC77, 0 ) );
				Add( new GenericBuyInfo( typeof( Cantaloupe ), 6, 20, 0xC79, 0 ) );
				Add( new GenericBuyInfo( typeof( Cabbage ), 5, 20, 0xC7B, 0 ) );
				//Add( new GenericBuyInfo( typeof( EarOfCorn ), 3, 20, XXXXXX, 0 ) );
				//Add( new GenericBuyInfo( typeof( Turnip ), 6, 20, XXXXXX, 0 ) );
				//Add( new GenericBuyInfo( typeof( SheafOfHay ), 2, 20, XXXXXX, 0 ) );
				Add( new GenericBuyInfo( typeof( Lemon ), 3, 20, 0x1728, 0 ) );
				Add( new GenericBuyInfo( typeof( Lime ), 3, 20, 0x172A, 0 ) );
				Add( new GenericBuyInfo( typeof( Peach ), 3, 20, 0x9D2, 0 ) );
				Add( new GenericBuyInfo( typeof( Pear ), 3, 20, 0x994, 0 ) );
				Add( new GenericBuyInfo( "Graine de Coton", typeof( CottonSeed ), 250, 20, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Graine de Lin", typeof( FlaxSeed ), 250, 20, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Germe de Blé", typeof( WheatSeed ), 150, 20, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Germe de Maïs", typeof( CornSeed ), 150, 20, 0xC82, 0 ) );
				Add( new GenericBuyInfo( "Graine de Carotte", typeof( CarrotSeed ), 50, 20, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Bulbe d'Oignon", typeof( OnionSeed ), 50, 20, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Bulbe d'Ail", typeof( GarlicSeed ), 50, 20, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Graine de Laitue", typeof( LettuceSeed ), 50, 20, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Graine de Chou", typeof( CabbageSeed ), 50, 20, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Graine mystérieuse", typeof( Seed ), 100, 5, 0xDCF, 0 ) );
				Add( new GenericBuyInfo( "Graine de Pommier", typeof( AppleSeed ), 1000, 20, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Graine de Pêcher", typeof( PeachSeed ), 1000, 20, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Graine de Poirier", typeof( PearSeed ), 1000, 20, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Graine de Cocotier", typeof( CocoSeed ), 1500, 20, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Graine de Bananier", typeof( BananaSeed ), 1500, 20, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Graine de Katyliis", typeof( KatylSeed ), 3000, 2, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Graine de Tolonax", typeof( OnaxSeed ), 3000, 2, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Graine d'Orge", typeof( OrgeSeed ), 150, 20, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Graine de Houblon", typeof( HoublonSeed ), 250, 20, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Sac de terre", typeof( Sacter ), 500, 10, 0xF27, 0x5E2 ) );
				//Add( new GenericBuyInfo( "Deed de Vigne N-S", typeof( VignesDeedNS ), 3000, 10, 0x14F0, 0 ) );
				//Add( new GenericBuyInfo( "Deed de Vigne E-O", typeof( VignesDeedEO ), 3000, 10, 0x14F0, 0 ) );
				Add( new GenericBuyInfo( "Pot de fleur", typeof( PlantBowl ), 500, 5, 0x15FD, 0 ) );
			} 
		} 

		public class InternalSellInfo : GenericSellInfo 
		{ 
			public InternalSellInfo() 
			{ 
				Add( typeof( Apple ), 1 );
				Add( typeof( Grapes ), 1 );
				Add( typeof( Watermelon ), 3 );
				Add( typeof( YellowGourd ), 1 );
				Add( typeof( Pumpkin ), 5 );
				Add( typeof( Onion ), 1 );
				Add( typeof( Lettuce ), 2 );
				Add( typeof( Squash ), 1 );
				Add( typeof( Carrot ), 1 );
				Add( typeof( HoneydewMelon ), 3 );
				Add( typeof( Cantaloupe ), 3 );
				Add( typeof( Cabbage ), 2 );
				Add( typeof( Lemon ), 1 );
				Add( typeof( Lime ), 1 );
				Add( typeof( Peach ), 1 );
				Add( typeof( Pear ), 1 );
                Add(typeof(PelleLaboureuseB), 25);
			} 
		} 
	} 
}