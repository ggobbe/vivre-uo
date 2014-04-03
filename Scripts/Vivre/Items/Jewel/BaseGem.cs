using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public class BaseGem : Item
    {
        private GemType m_GemType;

        [CommandProperty(AccessLevel.Administrator)]
        public GemType Gems
        {
            get { return m_GemType; }
            set { m_GemType = value; InvalidateProperties(); }
        }

        public BaseGem(int itemID)
            : base(itemID)
        {
        }

        public BaseGem(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
			writer.Write( (int) 0 ); // version
            writer.WriteEncodedInt((int)m_GemType);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_GemType = (GemType)reader.ReadEncodedInt();
        }
    }
}