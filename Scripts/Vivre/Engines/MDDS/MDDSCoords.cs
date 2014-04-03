using System;
using System.Collections.Generic;
using System.Text;

namespace Server.MDDS
{
    class MDDSCoords
    {
        private static List<RoomCoords> m_Rooms = new List<RoomCoords>();

        public static List<RoomCoords> Rooms { get { return m_Rooms; } }

        public static void Initialize()
        {
            m_Rooms.Add(new RoomCoords(new Point3D(5139, 2019, 0), GetExits(1)));   // salle 1
            m_Rooms.Add(new RoomCoords(new Point3D(5207, 1994, 0), GetExits(2)));   // salle 2
            m_Rooms.Add(new RoomCoords(new Point3D(5239, 2039, 0), GetExits(3)));   // salle 3
            m_Rooms.Add(new RoomCoords(new Point3D(5275, 2043, 0), GetExits(4)));   // salle 4  - BOSS?
            m_Rooms.Add(new RoomCoords(new Point3D(5271, 1935, 0), GetExits(5)));   // salle 5
            m_Rooms.Add(new RoomCoords(new Point3D(5187, 1942, 0), GetExits(6)));   // salle 6
            m_Rooms.Add(new RoomCoords(new Point3D(5139, 1915, 0), GetExits(7)));   // salle 7
            m_Rooms.Add(new RoomCoords(new Point3D(5167, 1831, 0), GetExits(8)));   // salle 8
            m_Rooms.Add(new RoomCoords(new Point3D(5222, 1863, 0), GetExits(9)));   // salle 9
            m_Rooms.Add(new RoomCoords(new Point3D(5308, 1859, 0), GetExits(10)));  // salle 10
        }

        private static List<Point3D> GetExits(int roomNumber)
        {
            List<Point3D> exits = new List<Point3D>();
            switch (roomNumber)
            {
                case 1:
                    exits.Add(new Point3D(5132, 1946, 0));
                    exits.Add(new Point3D(5156, 1955, 0));
                    exits.Add(new Point3D(5136, 1979, 0));
                    break;
                case 2:
                    exits.Add(new Point3D(5226, 1944, 0));
                    exits.Add(new Point3D(5261, 1977, 0));
                    exits.Add(new Point3D(5219, 1964, 0));
                    break;
                case 3:
                    exits.Add(new Point3D(5195, 2011, 0));
                    exits.Add(new Point3D(5188, 2025, 0));
                    exits.Add(new Point3D(5206, 2031, 0));
                    break;
                case 4:
                    exits.Add(new Point3D(5322, 1961, 0));
                    exits.Add(new Point3D(5359, 1996, 0));
                    exits.Add(new Point3D(5356, 2019, 0));
                    break;
                case 5:
                    exits.Add(new Point3D(5286, 1878, 0));
                    exits.Add(new Point3D(5331, 1899, 0));
                    exits.Add(new Point3D(5329, 1919, 0));
                    break;
                case 6:
                    exits.Add(new Point3D(5219, 1889, 0));
                    exits.Add(new Point3D(5194, 1877, 0));
                    exits.Add(new Point3D(5193, 1900, 0));
                    break;
                case 7:
                    exits.Add(new Point3D(5157, 1889, 0));
                    exits.Add(new Point3D(5169, 1874, 0));
                    exits.Add(new Point3D(5134, 1870, 0));
                    break;
                case 8:
                    exits.Add(new Point3D(5149, 1845, 0));
                    exits.Add(new Point3D(5140, 1824, 0));
                    exits.Add(new Point3D(5153, 1835, 0));
                    break;
                case 9:
                    exits.Add(new Point3D(5214, 1833, 0));
                    exits.Add(new Point3D(5266, 1849, 0));
                    exits.Add(new Point3D(5249, 1837, 0));
                    break;
                case 10:
                    exits.Add(new Point3D(5340, 1822, 0));
                    exits.Add(new Point3D(5316, 1816, 0));
                    exits.Add(new Point3D(5336, 1809, 0));
                    break;
            }
            return exits;
        }
    }

    class RoomCoords
    {
        private Point3D m_Entry;
        private List<Point3D> m_Exits;

        public Point3D Entry { get { return m_Entry; } }
        public List<Point3D> Exits { get { return m_Exits; } }

        public RoomCoords(Point3D entry, List<Point3D> exits)
        {
            m_Entry = entry;
            m_Exits = exits;
        }
    }
}