using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Mobiles;

namespace Server.MDDS
{
    class MDDSInstance
    {
        private MDDSStarter m_Starter;
        private List<Mobile> m_Followers;
        private Map m_Map;
        private List<MDDSRoom> m_Rooms;
        private Point3D m_Origin;
        private Map m_OriginMap;

        public List<Mobile> Followers
        {
            get { return m_Followers; }
            set { m_Followers = value; }
        }

        public Map Map { get { return m_Map; } }

        public List<MDDSRoom> Rooms
        {
            get { return m_Rooms; }
            set { m_Rooms = value; }
        }

        public MDDSInstance(MDDSStarter starter, Map map, Point3D origin, Map originMap)
        {
            m_Starter = starter;
            m_Followers = new List<Mobile>();
            m_Map = map;
            m_Rooms = new List<MDDSRoom>();
            m_Origin = origin;
            m_OriginMap = originMap;
        }

        public Point3D GetNextRoom(MDDSRoom actualRoom, Mobile m)
        {
            int index = m_Rooms.IndexOf(actualRoom);
            if (++index < m_Rooms.Count)
            {
                m.SendMessage("Progression : {0} sur {1}.", (index + 1), m_Rooms.Count);
                return m_Rooms[index].Entry;
            }
            else
            {
                m.Map = m_OriginMap;
                m_Followers.Remove(m);
                m_Starter.CleanInstances();
                return m_Origin;
            }
        }
    }
}
