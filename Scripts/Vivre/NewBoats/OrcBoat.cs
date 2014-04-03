using System;
using Server;
using Server.Items;
using System.Collections;
using System.Collections.Generic;

namespace Server.Multis
{
    public class OrcBoat : BaseBoat
    {
        public override int NorthID { get { return 0x18; } }
        public override int EastID { get { return 0x19; } }
        public override int SouthID { get { return 0x1A; } }
        public override int WestID { get { return 0x1B; } }

        public override int HoldDistance { get { return 5; } }
        public override int TillerManDistance { get { return -5; } }

        public override Point2D StarboardOffset { get { return new Point2D(2, -1); } }
        public override Point2D PortOffset { get { return new Point2D(-2, -1); } }

        public override Point3D MarkOffset { get { return new Point3D(0, 0, 3); } }

        public override BaseDockedBoat DockedBoat { get { return new OrcDockedBoat(this); } }

        private Direction dirSpawned = Direction.ValueMask;

        private List<BoatStatic> m_StaticParts = new List<BoatStatic>();
        public List<BoatStatic> StaticParts
        {
            get { return m_StaticParts; }
        }

        [Constructable]
        public OrcBoat() 
            : base()
        {
        }

        [Constructable]
        public OrcBoat(Direction dir)
        {
            Facing = dir;
        }

        public OrcBoat( Serial serial ) : base( serial )
		{
		}

        public override void OnDelete()
        {
            base.OnDelete();
            CleanStaticParts();
        }

        private void CleanStaticParts()
        {
            foreach (BoatStatic bs in m_StaticParts)
            {
                if (bs == null) continue;
                bs.Delete();
            }

            m_StaticParts.Clear();
        }

        public void AddMissingComponents()
        {
            if (this.Map == Map.Internal) return;
            if (dirSpawned == Facing) return;

            // On rend certains composants invisibles
            TillerMan.Visible = false;
            PPlank.Visible = false;
            SPlank.Visible = false;
            Hold.Visible = false;

            int multiID = 0;

            switch (Facing)
            {
                case Direction.North: multiID = NorthID; break;
                case Direction.East: multiID = EastID; break;
                case Direction.South: multiID = SouthID; break;
                case Direction.West: multiID = WestID; break;
            }

            if (multiID == 0) return;
            dirSpawned = this.Facing;

            CleanStaticParts();

            MultiComponentList mcl = MultiData.GetComponents(multiID);

            MultiTileEntry[] mte = mcl.List;

            for (int i = 0; i < mte.Length; i++)
            {
                if (mte[i].m_Flags == 0)
                {
                    BoatStatic s = new BoatStatic((int)mte[i].m_ItemID);
                    s.MoveToWorld(new Point3D(this.X + mte[i].m_OffsetX, this.Y + mte[i].m_OffsetY, this.Z + mte[i].m_OffsetZ), this.Map);
                    s.Movable = false;
                    m_StaticParts.Add(s);
                }
            }
        }

        public override void OnLocationChange(Point3D old)
        {
            base.OnLocationChange(old);
            AddMissingComponents();
        }

        public override void OnMapChange()
        {
            base.OnMapChange();
            DeleteOldStatics();
            AddMissingComponents();
        }

        private void DeleteOldStatics()
        {
            List<Item> items = new List<Item>();

            foreach (Item i in this.GetItemsInRange(15))
            {
                items.Add(i);
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != null && items[i] is BoatStatic) items[i].Delete();
            }
            AddMissingComponents();
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            Timer.DelayCall(TimeSpan.FromSeconds(10), DeleteOldStatics);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);            
        }
    }

    public class OrcBoatDeed : BaseBoatDeed
    {
        public override int LabelNumber { get { return 1116738; } } // large ship deed
        public override BaseBoat Boat { get { return new OrcBoat(); } }

        [Constructable]
        public OrcBoatDeed()
            : base(0x18, new Point3D(0, -1, 0))
        {
        }

        public OrcBoatDeed(Serial serial)
            : base(serial)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }
    }

    public class OrcDockedBoat : BaseDockedBoat
    {
        public override BaseBoat Boat { get { return new OrcBoat(); } }

        public OrcDockedBoat(BaseBoat boat)
            : base(0x18, new Point3D(0, -1, 0), boat)
        {
        }

        public OrcDockedBoat(Serial serial)
            : base(serial)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }
    }
}