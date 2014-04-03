using System;
using System.Collections.Generic;
using Server;
using Server.Multis;
using Server.Network;

namespace Server.Items
{
    [DynamicFliping]
    [Flipable(0x4025, 0x4026)]
    public class SalaryChest : BaseContainer
    {
        private bool m_Active;
        private int m_Salary;
        private Mobile m_Employer;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Active
        {
            get
            {
                return m_Active;
            }
            set
            {
                m_Active = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Salary
        {
            get
            {
                return m_Salary;
            }
            set
            {
                m_Salary = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Employer
        {
            get
            {
                return m_Employer;
            }
            set
            {
                m_Employer = value;
            }
        }


        [Constructable]
        public SalaryChest()
            : base(0x4025)
        {
            Name = "Coffre à salaire";
        }

        public SalaryChest(Serial serial)  : base(serial)
        {
        }

        public override bool  TryDropItem(Mobile from, Item dropped, bool sendFullMessage)
        {
            from.SendMessage("Vous ne pouvez rien déposer ici");
 	        return false;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write((bool)m_Active);
            writer.Write((int)m_Salary);
            writer.Write((Mobile)m_Employer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_Active = reader.ReadBool();
            m_Salary = reader.ReadInt();
            m_Employer = reader.ReadMobile();
        }
    }
}
