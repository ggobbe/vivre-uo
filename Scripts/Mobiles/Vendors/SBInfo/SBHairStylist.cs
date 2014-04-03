using System; 
using System.Collections.Generic; 
using Server.Items; 

namespace Server.Mobiles 
{ 
	public class SBHairStylist : SBInfo 
	{ 
		private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo(); 
		private IShopSellInfo m_SellInfo = new InternalSellInfo(); 

		public SBHairStylist() 
		{ 
		} 

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } } 
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } } 

		public class InternalBuyInfo : List<GenericBuyInfo> 
		{ 
			public InternalBuyInfo() 
			{ 
                // Scriptiz : utiliser le CustomHairStylist à la place !
                /*
				Add( new GenericBuyInfo( "special beard dye", typeof( SpecialBeardDye ), 500, 20, 0xE26, 0 ) ); 
				Add( new GenericBuyInfo( "special hair dye", typeof( SpecialHairDye ), 500, 20, 0xE26, 0 ) ); 
				Add( new GenericBuyInfo( "1041060", typeof( HairDye ), 60, 20, 0xEFF, 0 ) );
                Add(new GenericBuyInfo("Une coupe de cheveux", typeof(HairRestylingDeed), 120, 20, 0xEFF, 0));
                Add(new GenericBuyInfo("Un changement de barbe", typeof(BeardRestylingDeed), 120, 20, 0xEFF, 0)); 
                */
			
            } 
		} 

		public class InternalSellInfo : GenericSellInfo 
		{ 
			public InternalSellInfo() 
			{ 
                /*
				Add( typeof( HairDye ), 30 ); 
				Add( typeof( SpecialBeardDye ), 250 ); 
				Add( typeof( SpecialHairDye ), 2500 );
                Add(typeof(HairRestylingDeed), 60);
                Add(typeof(BeardRestylingDeed), 60);
                */
			} 
		} 
	} 
}