/********************************************************************************************
**Lait et fromage Crystal/Sna/Cooldev 2003 (laitage.cs,laitage_items.cs et fromage.cs)     **
**le script comprend 1 seau pour traire vache , brebis , chevre . 3 bouteilles de laits    **
**et 3 moule plein de fromages (je prefere les bouteilles que les pichets c'est stackable) **
**               http://invisionfree.com/forums/Hyel_dev/index.php                         **
********************************************************************************************/

using System;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;
using Server.Engines.Craft;

namespace Server.Items
{
    public enum Milk
    {
        None,
        Goat,
        Sheep,
        Cow
    }
    public enum BucketLiquid
    {
        None,
        SugarWater
    }


    public class ResourceBucket : Item
    {
        private Milk m_MilkType;
        private BucketLiquid m_ResourceType;
        private int m_MaxQuantity = 40;
        private int m_Quantity;


        [CommandProperty(AccessLevel.GameMaster)]
        public Milk MilkType
        {
            get { return m_MilkType; }
            set
            {
                if (ResourceType != BucketLiquid.None)
                    ResourceType = BucketLiquid.None;
                m_MilkType = value; ComputeProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public BucketLiquid ResourceType
        {
            get { return m_ResourceType; }
            set
            {
                if (MilkType != Milk.None)
                    MilkType = Milk.None;
                m_ResourceType = value; ComputeProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxQuantity
        {
            get { return m_MaxQuantity; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Quantity
        {
            get { return m_Quantity; }
            set { m_Quantity = Math.Min(value, MaxQuantity); InvalidateProperties(); }
        }


        [Constructable]
        public ResourceBucket()
            : base(0x0FFA)
        {
            Weight = 10.0;
            Name = "Seau";
            Hue = 1001;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add("Contient {0} litre{1} de liquide", Quantity, (Quantity > 1) ? "s" : "");
        }

        public string BucketNameAffix()
        {
            string affix = "";

            if (m_ResourceType == BucketLiquid.SugarWater)
                affix = " d'eau sucrée";

            switch (m_MilkType)
            {
                case (Milk.Cow):
                    affix = " de lait de vache"; break;
                case (Milk.Goat):
                    affix = " de lait de chèvre"; break;
                case (Milk.Sheep):
                    affix = " de lait de brebis"; break;
            }

            return affix;
        }

        public void ComputeProperties()
        {
            string prefix = "Seau";

            Name = prefix + BucketNameAffix();
        }

        public void EmptyBucket()
        {
            MilkType = Milk.None;
            ResourceType = BucketLiquid.None;
            Quantity = 0;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 2) || !from.InLOS(this))
            {
                from.SendLocalizedMessage(501816);
                return;
            }

            if (this.ResourceType == BucketLiquid.SugarWater)
            {
                if (!this.Find(from, CraftItem.m_HeatSources))
                {
                    from.SendMessage("Vous devriez faire crystaliser le tout");
                    return;
                }
                if(from.CheckSkill(SkillName.Cooking, 20,80))
                {
                    from.SendMessage("La chaleur fait figer le tout, donnant une allure différente à cette eau");
                    Sugar item = new Sugar();
                    item.Amount = this.Quantity;
                    from.AddToBackpack(item);
                    this.EmptyBucket();
                }
                else
                {
                    from.SendMessage("Vous ne parvenez pas à en faire une mixture adéquate..");
                    this.Quantity--;
                    if (Quantity == 0)
                        this.EmptyBucket();
                }
                return;
            }

            from.BeginTarget(2, false, TargetFlags.None, new TargetCallback(ExtractTarget));


        }
        public ResourceBucket(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // Version
            writer.WriteEncodedInt((int)m_MilkType);
            writer.WriteEncodedInt((int)m_ResourceType);
            writer.Write((int)m_MaxQuantity);
            writer.Write((int)m_Quantity);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_MilkType = (Milk)reader.ReadEncodedInt();
            m_ResourceType = (BucketLiquid)reader.ReadEncodedInt();
            m_MaxQuantity = reader.ReadInt();
            m_Quantity = reader.ReadInt();
        }

        public bool Find(int itemID, int[] itemIDs)
        {
            bool contains = false;

            for (int i = 0; !contains && i < itemIDs.Length; i += 2)
                contains = (itemID >= itemIDs[i] && itemID <= itemIDs[i + 1]);

            return contains;
        }

        public bool Find(Mobile from, int[] itemIDs)
        {
            Map map = from.Map;

            if (map == null)
                return false;

            IPooledEnumerable eable = map.GetItemsInRange(from.Location, 2);

            foreach (Item item in eable)
            {
                if ((item.Z + 16) > from.Z && (from.Z + 16) > item.Z && Find(item.ItemID, itemIDs))
                {
                    eable.Free();
                    return true;
                }
            }

            eable.Free();

            for (int x = -2; x <= 2; ++x)
            {
                for (int y = -2; y <= 2; ++y)
                {
                    int vx = from.X + x;
                    int vy = from.Y + y;

                    StaticTile[] tiles = map.Tiles.GetStaticTiles(vx, vy, true);

                    for (int i = 0; i < tiles.Length; ++i)
                    {
                        int z = tiles[i].Z;
                        int id = tiles[i].ID;

                        if ((z + 16) > from.Z && (from.Z + 16) > z && Find(id, itemIDs))
                            return true;
                    }
                }
            }

            return false;
        }

        public void ExtractTarget(Mobile from, object obj)
        {
            if (this.MilkType != Milk.None)
            {
                if (obj is Mobile)
                {
                    Mobile targ = (Mobile)obj;

                    if (Quantity >= MaxQuantity)
                    {
                        from.SendMessage("Ce seau est plein");
                        return;
                    }

                    if (targ.Stam < 10)
                    {
                        targ.PrivateOverheadMessage(MessageType.Regular, 0x3B2, false, "Vous ne pourrez rien enlever à cette pauvre bête!", from.NetState);
                        return;
                    }

                    if (targ is Cow && (this.MilkType == Milk.None || this.MilkType == Milk.Cow))
                    {
                        MilkType = Milk.Cow;
                        Quantity++;
                        targ.Stam -= 10;
                        from.SendMessage("Vous obtenez" + BucketNameAffix());
                        return;
                    }

                    if (targ is Brebis && (this.MilkType == Milk.None || this.MilkType == Milk.Sheep))
                    {
                        MilkType = Milk.Sheep;
                        Quantity++;
                        targ.Stam -= 10;
                        from.SendMessage("Vous obtenez" + BucketNameAffix());
                        return;
                    }

                    if (targ is Chevre && (this.MilkType == Milk.None || this.MilkType == Milk.Goat))
                    {
                        MilkType = Milk.Goat;
                        Quantity++;
                        targ.Stam -= 10;
                        from.SendMessage("Vous obtenez" + BucketNameAffix());
                        return;
                    }

                    if (targ == from)
                    {
                        from.SendMessage("Versez le contenu dans un récipient plus propice pour boire!");
                        return;
                    }
                    from.SendMessage("Cela ne servira à rien sur cette créature");
                    return;
                }
                if (obj is BaseBeverage)
                {
                    BaseBeverage targ = (BaseBeverage)obj;

                    int shared = Math.Min(this.Quantity, (targ.MaxQuantity - targ.Quantity));

                    from.SendMessage("Vous versez{0} dans le contenant", BucketNameAffix());
                    targ.Quantity += shared;
                    this.Quantity -= shared;

                    switch (MilkType)
                    {
                        case Milk.Cow: targ.Content = BeverageType.Milk; break;
                        case Milk.Goat: targ.Content = BeverageType.LaitChevre; break;
                        case Milk.Sheep: targ.Content = BeverageType.LaitBrebis; break;
                    }
                }

                if (obj is CheeseForm)
                {
                    CheeseForm targ = (CheeseForm)obj;

                    if (ResourceType != BucketLiquid.None)
                    {
                        from.SendMessage("Vous ne pouvez verser cela ici");
                        return;
                    }

                    if (targ.Quantity >= targ.MaxQuantity)
                    {
                        from.SendMessage("Ce fromage fermente déjà");
                        return;
                    }

                    if (targ.Content != Milk.None && (MilkType != targ.Content))
                    {
                        from.SendMessage("Vous ne pouvez mélanger des laits");
                        return;
                    }

                    int shared = Math.Min(Quantity, (targ.MaxQuantity - targ.Quantity));

                    targ.Content = this.MilkType;

                    from.SendMessage("Vous versez{0} dans le moule", BucketNameAffix());
                    targ.Quantity += shared;
                    this.Quantity -= shared;
                    if (targ.Quantity >= targ.MaxQuantity)
                    {
                        targ.CookingValue = from.Skills[SkillName.Cooking].Value;
                        targ.TimeStart = DateTime.Now;
                    }

                 }

            }

            else
            {
                from.SendMessage("Ce seau est vide");
                return;
            }

            if (Quantity <= 0)
                EmptyBucket();
        }
    }
}
          

