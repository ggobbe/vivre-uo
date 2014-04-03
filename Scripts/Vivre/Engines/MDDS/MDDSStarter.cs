using System;
using System.Collections.Generic;
using System.Text;

using Server;
using Server.Items;

namespace Server.MDDS
{
    class MDDSStarter : NoxCrystal
    {
        private List<MDDSInstance> m_Instances = new List<MDDSInstance>();
        private List<Map> m_Maps = null;

        [Constructable]
        public MDDSStarter() : base()
        {
            Movable = false;
            LootType = LootType.Regular;
        }

        public MDDSStarter(Serial serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (m_Maps == null) m_Maps = FillMaps();

            if (m_Instances.Count >= m_Maps.Count || !from.InRange(this.Location, 2))
            {
                base.OnDoubleClick(from);
                return;
            }

            MDDSInstance newInstance = new MDDSInstance(this, FindFreeMap(), from.Location, from.Map);
            ClearGates(newInstance.Map);
            newInstance.Rooms = GenRooms(newInstance);
            newInstance.Followers.Add(from);

            from.SendMessage("Bienvenue dans le MDDS de la map {0}...", newInstance.Map.Name);
            from.MoveToWorld(newInstance.Rooms[0].Entry, newInstance.Map);

            m_Instances.Add(newInstance);
        }

        private List<Map> FillMaps()
        {
            List<Map> maps = new List<Map>();
            maps.Add(Map.Felucca2);
            maps.Add(Map.Felucca3);
            maps.Add(Map.Felucca4);
            maps.Add(Map.Felucca5);
            maps.Add(Map.Felucca6);
            return maps;
        }

        public void CleanInstances()
        {
            for (int i = 0; i < m_Instances.Count; i++)
            {
                if (m_Instances[i] != null && m_Instances[i].Followers.Count == 0)
                {
                    for (int j = 0; j < m_Instances[i].Rooms.Count; j++)
                    {
                        for (int k = 0; k < m_Instances[i].Rooms[j].ExitGates.Count; k++)
                        {
                            m_Instances[i].Rooms[j].ExitGates[k].Delete();
                        }
                    }
                    m_Instances.Remove(m_Instances[i]);
                }
            }
        }

        private Map FindFreeMap()
        {
            foreach (Map m in m_Maps)
            {
                bool isExisting = false;
                foreach (MDDSInstance mi in m_Instances)
                {
                    if (mi.Map == m)
                    {
                        isExisting = true;
                        break;
                    }
                }

                if (!isExisting) return m;
            }
            return null;
        }

        private List<MDDSRoom> GenRooms(MDDSInstance instance)
        {
            List<MDDSRoom> rooms = new List<MDDSRoom>();

            List<RoomCoords> tmp = new List<RoomCoords>(MDDSCoords.Rooms);
            while (tmp.Count > 0)
            {
                int next = Utility.RandomMinMax(0, tmp.Count - 1);
                rooms.Add(new MDDSRoom(instance, tmp[next].Entry, tmp[next].Exits));
                tmp.RemoveAt(next);
            }
            return rooms;
        }

        public static void ClearGates(Map map)
        {
            List<Item> toDel = new List<Item>();
            foreach (Item i in World.Items.Values)
            {
                if (i is MDDSGate && i.Map == map) toDel.Add(i);
            }
            for (int i = 0; i < toDel.Count; i++)
                toDel[i].Delete();
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
