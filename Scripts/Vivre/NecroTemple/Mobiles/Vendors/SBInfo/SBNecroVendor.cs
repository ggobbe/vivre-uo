using System;
using System.Collections.Generic;
using Server.Items;
using Server.Multis;

namespace Server.Mobiles
{
    public class SBNecroVendor : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBNecroVendor()
        {
        }

        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
        public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new GenericBuyInfo(typeof(BatWing), 6, 20, 0xF78, 0));
                Add(new GenericBuyInfo(typeof(DaemonBlood), 12, 20, 0xF7D, 0));
                Add(new GenericBuyInfo(typeof(PigIron), 10, 20, 0xF8A, 0));
                Add(new GenericBuyInfo(typeof(NoxCrystal), 12, 20, 0xF8E, 0));
                Add(new GenericBuyInfo(typeof(GraveDust), 6, 20, 0xF8F, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
            }
        }
    }
}