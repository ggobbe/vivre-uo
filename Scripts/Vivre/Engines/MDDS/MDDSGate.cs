using System;
using System.Collections.Generic;
using System.Text;
using Server.Items;

namespace Server.MDDS
{
    class MDDSGate : Moongate
    {
        private MDDSRoom m_Room;

        public MDDSGate(MDDSRoom room) : base()
        {
            m_Room = room;
            Target = new Point3D(m_Room.Entry);
            TargetMap = m_Room.Instance.Map;
            Dispellable = false;
        }

        public MDDSGate(Serial serial)
        {
        }

        public override void UseGate(Mobile m)
        {
            if (m_Room == null)
            {
                m.SendMessage("Tu reçois une baffe de Scriptiz !");
                return;
            }
            Target = new Point3D(m_Room.Instance.GetNextRoom(m_Room, m));
            if (m.Map != TargetMap) TargetMap = m.Map;
            m_Room.DelGatesExcept(this);
            base.UseGate(m);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
