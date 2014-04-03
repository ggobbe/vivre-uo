using System;
using Server.Targeting;

namespace Server.Items
{
    public class HairStrand : Item
    {
        public override string DefaultName
        {
            get
            {
                return "Mèche de cheveux" + (m_HairOwner != null ? " de " + m_HairOwner.Name : "");
            }
        }


        private Mobile m_HairOwner;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile HairOwner
        {
            get
            {
                return m_HairOwner;
            }
            set
            {
                m_HairOwner = value;
            }
        }

        [Constructable]
        public HairStrand()
            : base(0x1E99)
        {
            Hue = 150;
            Weight = 1.0;
            m_HairOwner = null;
            Stackable = false;
        }

        public HairStrand(Serial serial)
            : base(serial)
        {

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
            writer.Write((Mobile)m_HairOwner);

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_HairOwner = reader.ReadMobile();
        }
    }
}