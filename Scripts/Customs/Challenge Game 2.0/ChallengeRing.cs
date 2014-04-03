/*
 	Challenge Game 2.0
	Update for RunUO 2.0 by Lokai
	7/18/2006
*/
using System; 
using Server; 
using Server.Items;
using Server.Mobiles;
using System.Collections;

namespace Server.Items
{
    public class ChallengeRing : BaseRing
    {
        private ChallengeStone m_Item;
        private ArrayList m_Players = new ArrayList();
        private int m_Kills;
        private int m_Fame;
        private int m_Karma;
        private int m_ShortMurders;

        public ChallengeRing()
            : base(0x108a)
        {
        }
        [Constructable]
        public ChallengeRing(ChallengeStone game, ArrayList players)
            : base(0x108a)
        {
            Weight = 0.1;
            Movable = false;
            m_Item = game;
            m_Players = players;
            Name = " Challenege Ring ";
            Attributes.LowerManaCost = 100;
            Attributes.LowerRegCost = 100;
        }
        public ChallengeRing(Serial serial)
            : base(serial)
        {
        }
        public int Kills
        {
            get { return m_Kills; }
        }
        public override bool OnEquip(Mobile from)
        {
            m_Kills = from.Kills;
            m_Fame = from.Fame;
            m_Karma = from.Karma;
            m_ShortMurders = from.ShortTermMurders;
            from.Kills = 10;
            return base.OnEquip(from);
        }
        public override void OnDelete()
        {
            if (this.Parent is PlayerMobile)
            {
                PlayerMobile m = (PlayerMobile)Parent;
                m.Kills = m_Kills;
                m.Fame = m_Fame;
                m.Karma = m_Karma;
                m.ShortTermMurders = m_ShortMurders;
                m.Criminal = false;
            }
            base.OnDelete();
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write((int)m_Kills);
            writer.Write((int)m_Fame);
            writer.Write((int)m_Karma);
            writer.Write((int)m_ShortMurders);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            switch (version)
            {
                case 0:
                    {
                        m_Kills = reader.ReadInt();
                        m_Fame = reader.ReadInt();
                        m_Karma = reader.ReadInt();
                        m_ShortMurders = reader.ReadInt();
                        break;
                    }
            }
        }
    }
}