/**
 * Pelle permettant de labourer le sol d'une maison custom pour autant qu'il soit vierge
 * Auteur : Scriptiz
 * Version : 2009-03-18
 */

using System;
using Server;
using Server.Engines.Harvest;
using Server.Network;
using Server.Targeting;
using Server.Mobiles;
using Server.Items;
using Server.Items.Crops;
using Server.Multis;
using Server.Engines.Craft;

namespace Server.Items
{
    public class PelleLaboureuseB : Item
    {
        public static int[] ploughableTiles = new int[] {0x31F4, 0x31F5, 0x31F6, 0x31F7};
        private int m_UsesRemaining;

        [CommandProperty(AccessLevel.GameMaster)]
        public int UsesRemaining
        {
            get { return m_UsesRemaining; }
            set { m_UsesRemaining = value; }
        }

        [Constructable]
        public PelleLaboureuseB()
            : this(50)
        {
        }

        [Constructable]
        public PelleLaboureuseB(int uses)
            : base(0xF39)
        {
            Weight = 5.0;
            Name = "Pelle de Fermier";
            UsesRemaining = uses;
        }

        public PelleLaboureuseB(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!this.IsChildOf(from.Backpack))
            {
                from.SendMessage("Vous devez avoir la pelle dans votre sac pour pouvoir l'utiliser.");
            }
            else
            {
                from.SendMessage("Ciblez le sol à labourer.");
                from.Target = new LabourerTarget(from, this);
            }
        }

        public void decreaseUses(Mobile from)
        {
            UsesRemaining--;
            Name = "Pelle de Fermier (" + UsesRemaining + ")";

            if (this.UsesRemaining <= 0)
            {
                from.SendMessage("La pelle se brise entre vos mains.");
                from.PlaySound(0x2A);
                this.Delete();
            }
            else
            {
                int[] sons = new int[] { 0x125, 0x126 };
                int son = Utility.Random(0, 1);
                from.PlaySound(sons[son]);
                from.Animate(11, 1, 1, true, false, 0);
            }
        }

        private class LabourerTarget : Target
        {
            Mobile m_From;
            PelleLaboureuseB m_Pelle;

            public LabourerTarget(Mobile from, PelleLaboureuseB pelle)
                : base(-1, false, TargetFlags.None)
            {
                m_From = from;
                m_Pelle = pelle;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                int groundId = 0;
                IPoint3D t = targeted as IPoint3D;
                if (t == null)
                    return;

                Point3D loc = new Point3D(t);
                if (t is StaticTarget)
                {
                    loc.Z -= TileData.ItemTable[((StaticTarget)t).ItemID & 0x3FFF].CalcHeight;

                    StaticTarget isFarmGround = (StaticTarget)t;
                    groundId = isFarmGround.ItemID;
                }
                else if (t is Item)
                {
                    Item isFarmGround = (Item)t;
                    
                    if(isFarmGround != null)
                    {
	                    if(isFarmGround is BaseCrop)
	                    {
	                    	isFarmGround.Delete();
	                    	from.SendMessage("Vous arrachez la pousse du sol.");
	                    	return;
	                    }
	                    else
	                    	groundId = isFarmGround.ItemID;
                    }
                }

                from.Direction = from.GetDirectionTo(loc.X, loc.Y);

                if (ValidatePlacement(loc))
                {
                    bool ground = false;
                    for (int i = 0; ground == false && i < ploughableTiles.Length; i++)
                    {
                        if (ploughableTiles[i] == groundId)
                            ground = true;
                    }

                    if (ground)
                    {
                        EndPlace(loc);
                        m_Pelle.decreaseUses(from);
                        from.SendMessage("Vous labourez le sol pour en faire de la terre cultivable.");
                    }
                    else
                        from.SendMessage("Ce type de sol n'est pas labourable.");
                }
            }

            public bool ValidatePlacement(Point3D loc)
            {
                if (m_From.Map == null)
                    return false;

                BaseHouse house = BaseHouse.FindHouseAt(m_From.Location, m_From.Map, 20);
                if (house == null || !house.IsOwner(m_From))
                {
                    m_From.SendMessage("Vous devez être dans votre maison pour labourer le sol.");
                    return false;
                }

                if (loc.Y > m_From.Location.Y + 1 || loc.Y < m_From.Location.Y - 1)
                {
                    m_From.SendMessage("C'est trop loin...");
                    return false;
                }

                if (loc.X > m_From.Location.X + 1 || loc.X < m_From.Location.X - 1)
                {
                    m_From.SendMessage("C'est trop loin...");
                    return false;
                }
                return true;
            }

            public void EndPlace(Point3D loc)
            {
                Static farmGround = new Static(13001);
                farmGround.Name = "Terre labourée";
                farmGround.Map = m_From.Map;
                farmGround.X = loc.X;
                farmGround.Y = loc.Y;
                farmGround.Z = loc.Z;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            // version 1
            writer.Write((int)m_UsesRemaining);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    m_UsesRemaining = reader.ReadInt();
                    break;
                case 0:
                    m_UsesRemaining = 50;
                    break;
            }
        }
    }
}