using System;
using System.Collections.Generic;
using System.Text;
using Server.Items;

namespace Server.MDDS
{
    class MDDSRoom
    {
        private MDDSInstance m_Instance;
        private Point3D m_Entry;
        private List<Point3D> m_Exits;
        private List<Mobile> m_Monsters;
        private List<MDDSGate> m_ExitGates;

        public MDDSInstance Instance { get { return m_Instance; } }
        public Point3D Entry { get { return m_Entry; } }
        public List<MDDSGate> ExitGates { get { return m_ExitGates; } }

        public MDDSRoom(MDDSInstance instance, Point3D entry, List<Point3D> exits)
        {
            m_Instance = instance;
            m_Entry = entry;
            m_Exits = exits;
            m_Monsters = new List<Mobile>();
            m_ExitGates = new List<MDDSGate>();
            GenExits();
        }

        public void GenExits()
        {
            foreach (Point3D p in m_Exits)
            {
                MDDSGate g = new MDDSGate(this);
                g.MoveToWorld(p, m_Instance.Map);
                m_ExitGates.Add(g);
            }
        }

        public void DelGatesExcept(MDDSGate gate)
        {
            for (int i = 0; i < m_ExitGates.Count; i++)
                if (m_ExitGates[i] != gate) m_ExitGates[i].Delete();
        }
    }
}
