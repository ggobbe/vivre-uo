using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Items.Crops
{

    //naga
    public class OnaxSeed : BaseCrop
    {
        // return true to allow planting on Dirt Item (ItemID 0x32C9)
        // See CropHelper.cs for other overriddable types
        public override bool CanGrowGarden { get { return true; } }

        [Constructable]
        public OnaxSeed()
            : this(1)
        {
        }

        [Constructable]
        public OnaxSeed(int amount)
            : base(0xF27)
        {
            Stackable = true;
            Weight = .5;
            Hue = 0x5E2;

            Movable = true;

            Amount = amount;
            Name = AgriTxt.Seed + " de Tolonax";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Mounted && !CropHelper.CanWorkMounted)
            {
                from.SendMessage(AgriTxt.CannotWorkMounted);
                return;
            }

            Point3D m_pnt = from.Location;
            Map m_map = from.Map;

            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042010); //You must have the object in your backpack to use it. 
                return;
            }

            else if (!CropHelper.CheckCanGrow(this, m_map, m_pnt.X, m_pnt.Y))
            {
                from.SendMessage(AgriTxt.CannotGrowHere);
                return;
            }

            //check for BaseCrop on this tile
            ArrayList cropshere = CropHelper.CheckCrop(m_pnt, m_map, 0);
            if (cropshere.Count > 0)
            {
                from.SendMessage(AgriTxt.AlreadyCrop);
                return;
            }

            //check for over planting prohibt if 4 maybe 3 neighboring crops
            ArrayList cropsnear = CropHelper.CheckCrop(m_pnt, m_map, 2);//1
            if ((cropsnear.Count > 1) || ((cropsnear.Count == 1) && Utility.RandomBool()))//3
            {
                from.SendMessage(AgriTxt.TooMuchCrops);
                return;
            }

            if (this.BumpZ) ++m_pnt.Z;

            if (!from.Mounted)
                from.Animate(32, 5, 1, true, false, 0); // Bow

            from.SendMessage(AgriTxt.CropPlanted);
            this.Consume();
            Item item = new OnaxSapling();// from ); 
            item.Location = m_pnt;
            item.Map = m_map;

        }

        public OnaxSeed(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
    //naga	


    public class OnaxSapling : BaseCrop
    {
        public Timer thisTimer;
        public DateTime treeTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public String FullGrown { get { return treeTime.ToString("T"); } }

        [Constructable]
        public OnaxSapling()
            : base(Utility.RandomList(0xCA6, 0xCA6))
        {
            Movable = false;
            Name = AgriTxt.Seedling + " de Tolonax";

            init(this);
        }

        public static void init(OnaxSapling plant)
        {
            TimeSpan delay = TreeHelper.SaplingTime;
            plant.treeTime = DateTime.Now + delay;

            plant.thisTimer = new TreeHelper.TreeTimer(plant, typeof(OnaxTree), delay);
            plant.thisTimer.Start();
        }

        public OnaxSapling(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            init(this);
        }
    }


    public class OnaxTree : BaseTree
    {
        public Item i_trunk;
        private Timer chopTimer;

        private const int max = 12;
        private DateTime lastpicked;
        private int m_yield;

        public Timer regrowTimer;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Yield { get { return m_yield; } set { m_yield = value; } }

        public int Capacity { get { return max; } }
        public DateTime LastPick { get { return lastpicked; } set { lastpicked = value; } }

        [Constructable]
        public OnaxTree(Point3D pnt, Map map)
            : base(Utility.RandomList(0xC9E, 0xC9E)) // leaves
        {
            Movable = false;
            MoveToWorld(pnt, map);
            Name = "Tolonax";
            int trunkID = 0x1B1F; // ((Item)this).ItemID - 2;

            i_trunk = new TreeTrunk(trunkID, this);
            i_trunk.MoveToWorld(pnt, map);

            init(this, false);
        }

        public static void init(OnaxTree plant, bool full)
        {
            plant.LastPick = DateTime.Now;
            plant.regrowTimer = new FruitTimer(plant);

            if (full)
            {
                plant.Yield = plant.Capacity;
            }
            else
            {
                plant.Yield = 0;
                plant.regrowTimer.Start();
            }
        }

        public override void OnAfterDelete()
        {
            if ((i_trunk != null) && (!i_trunk.Deleted))
                i_trunk.Delete();

            base.OnAfterDelete();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Mounted && !TreeHelper.CanPickMounted)
            {
                from.SendMessage(AgriTxt.CannotWorkMounted);
                return;
            }

            if (DateTime.Now > lastpicked.AddSeconds(3)) // 3 seconds between picking
            {
                lastpicked = DateTime.Now;

                int lumberValue = (int)from.Skills[SkillName.Lumberjacking].Value / 5;
                if (from.Mounted)
                    ++lumberValue;

                if (lumberValue < 7)
                {
                    from.SendMessage(AgriTxt.DunnoHowTo);
                    return;
                }

                if (from.InRange(this.GetWorldLocation(), 2))
                {
                    if (m_yield < 1)
                    {
                        from.SendMessage(AgriTxt.NoCrop);
                    }
                    else //check skill
                    {
                        from.Direction = from.GetDirectionTo(this);

                        from.Animate(from.Mounted ? 26 : 17, 7, 1, true, false, 0);

                        if (lumberValue > m_yield)
                            lumberValue = m_yield + 1;

                        int pick = Utility.Random(lumberValue);
                        if (pick == 0)
                        {
                            from.SendMessage(AgriTxt.ZeroPicked);
                            return;
                        }

                        m_yield -= pick;
                        from.SendMessage(AgriTxt.YouPick + " {0} Onax{1}!", pick, (pick == 1 ? "" : "s"));

                        //PublicOverheadMessage( MessageType.Regular, 0x3BD, false, string.Format( "{0}", m_yield )); 

                        Onax crop = new Onax(pick); // naga fruit
                        from.AddToBackpack(crop);

                        if (!regrowTimer.Running)
                        {
                            regrowTimer.Start();
                        }
                    }
                }
                else
                {
                    from.SendLocalizedMessage(500446); // That is too far away. 
                }
            }
        }

        private class FruitTimer : Timer
        {
            private OnaxTree i_plant;

            public FruitTimer(OnaxTree plant)
                : base(TimeSpan.FromSeconds(900), TimeSpan.FromSeconds(30))
            {
                Priority = TimerPriority.OneSecond;
                i_plant = plant;
            }

            protected override void OnTick()
            {
                if ((i_plant != null) && (!i_plant.Deleted))
                {
                    int current = i_plant.Yield;

                    if (++current >= i_plant.Capacity)
                    {
                        current = i_plant.Capacity;
                        Stop();
                    }
                    else if (current <= 0)
                        current = 1;

                    i_plant.Yield = current;

                    //i_plant.PublicOverheadMessage( MessageType.Regular, 0x22, false, string.Format( "{0}", current )); 
                }
                else Stop();
            }
        }

        public void Chop(Mobile from)
        {
            if (from.InRange(this.GetWorldLocation(), 2))
            {
                if ((chopTimer == null) || (!chopTimer.Running))
                {
                    if ((TreeHelper.TreeOrdinance) && (from.AccessLevel == AccessLevel.Player))
                    {
                        if (from.Region is Regions.GuardedRegion)
                            from.CriminalAction(true);
                    }

                    chopTimer = new TreeHelper.ChopAction(from);

                    Point3D pnt = this.Location;
                    Map map = this.Map;

                    from.Direction = from.GetDirectionTo(this);
                    chopTimer.Start();

                    double lumberValue = from.Skills[SkillName.Lumberjacking].Value / 100;
                    if ((lumberValue > .5) && (Utility.RandomDouble() <= lumberValue))
                    {
                        Onax fruit = new Onax((int)Utility.Random(13) + m_yield);
                        from.AddToBackpack(fruit);

                        int cnt = Utility.Random((int)(lumberValue * 10) + 1);
                        Log logs = new Log(cnt); // Fruitwood Logs ??
                        from.AddToBackpack(logs);

                        FruitTreeStump i_stump = new FruitTreeStump(typeof(OnaxTree));
                        Timer poof = new StumpTimer(this, i_stump, from);
                        poof.Start();
                    }
                    else from.SendLocalizedMessage(500495); // You hack at the tree for a while, but fail to produce any useable wood.
                    //PublicOverheadMessage( MessageType.Regular, 0x3BD, false, string.Format( "{0}", lumberValue )); 
                }
            }
            else from.SendLocalizedMessage(500446);  // That is too far away.
        }

        private class StumpTimer : Timer
        {
            private OnaxTree i_tree;
            private FruitTreeStump i_stump;
            private Mobile m_chopper;

            public StumpTimer(OnaxTree Tree, FruitTreeStump Stump, Mobile from)
                : base(TimeSpan.FromMilliseconds(5500))
            {
                Priority = TimerPriority.TenMS;

                i_tree = Tree;
                i_stump = Stump;
                m_chopper = from;
            }

            protected override void OnTick()
            {
                i_stump.MoveToWorld(i_tree.Location, i_tree.Map);
                i_tree.Delete();
                m_chopper.SendMessage(AgriTxt.LogsAndFruits);
            }
        }

        public override void OnChop(Mobile from)
        {
            if (!this.Deleted)
                Chop(from);
        }

        public OnaxTree(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);

            writer.Write((Item)i_trunk);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            Item item = reader.ReadItem();
            if (item != null)
                i_trunk = item;

            init(this, true);
        }
    }
}