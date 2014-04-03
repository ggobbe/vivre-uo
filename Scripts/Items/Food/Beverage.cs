using System;
using System.Collections;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Engines.Plants;
using Server.Engines.Quests;
using Server.Engines.Quests.Hag;
using Server.Engines.Quests.Matriarch;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public enum BeverageType
    {
        Ale,
        Cider,
        Liquor,
        Milk,
        Wine,
        Water,

        // Scriptiz : Ajout des breuvages de vivre à la fin de l'énumération !!
        BiereCommune,
        BiereAmbre,
        BiereMiel,
        BiereBrune,
        BiereEpice,
        BiereSorciere,
        MoinetteYew,
        Kwak,

        // Scriptiz : ajout de quelques breuvages pour Crystal
        LaitChevre,
        LaitBrebis,
        JusPomme,
        JusPeche,
        JusRaisin
    }

    public interface IHasQuantity
    {
        int Quantity { get; set; }
    }

    public interface IWaterSource : IHasQuantity
    {
    }

    // TODO: Flipable attributes
    [TypeAlias("Server.Items.BottleBiereCommune", "Server.Items.BottleBiereAmbre", "Server.Items.BottleBiereMiel",
        "Server.Items.BottleBiereBrune", "Server.Items.BottleBiereEpice", "Server.Items.BottleMoinetteYew", "Server.Items.BottleKwak",
        "Server.Items.BottleBiereSorciere", "Server.Items.BottleAle", "Server.Items.BottleLiquor", "Server.Items.BottleWine")]
    public class BeverageBottle : BaseBeverage
    {
        public override int BaseLabelNumber { get { return 1042959; } } // a bottle of Ale
        public override int MaxQuantity { get { return 5; } }
        public override bool Fillable { get { return false; } }

        public override int ComputeItemID()
        {
            if (!IsEmpty)
            {
                switch (Content)
                {
                    case BeverageType.Ale: return 0x99F;
                    case BeverageType.Cider: return 0x99F;
                    case BeverageType.Liquor: return 0x99B;
                    case BeverageType.Milk: return 0x99B;
                    case BeverageType.Wine: return 0x9C7;
                    case BeverageType.Water: return 0x99B;

                    // Boissons de vivre
                    case BeverageType.BiereCommune: return 0x99F;
                    case BeverageType.BiereAmbre: return 0x99F;
                    case BeverageType.BiereMiel: return 0x99F;
                    case BeverageType.BiereBrune: return 0x99F;
                    case BeverageType.BiereEpice: return 0x99F;
                    case BeverageType.MoinetteYew: return 0x99F;
                    case BeverageType.Kwak: return 0x99F;
                    case BeverageType.BiereSorciere: return 0x99F;

                    // Scriptiz : ajout de quelques trucs pour Crystal
                    case BeverageType.LaitChevre: return 0x99B;
                    case BeverageType.LaitBrebis: return 0x99B;
                    case BeverageType.JusPomme: return 0x99F;
                    case BeverageType.JusPeche: return 0x99F;
                    case BeverageType.JusRaisin: return 0x9C7;
                }
            }

            return 0;
        }

        [Constructable]
        public BeverageBottle(BeverageType type)
            : base(type)
        {
            Weight = 1.0;
        }

        public BeverageBottle(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        if (CheckType("BottleAle")) Content = BeverageType.Ale;
                        else if (CheckType("BottleLiquor")) Content = BeverageType.Liquor;
                        else if (CheckType("BottleWine")) Content = BeverageType.Wine;

                        // Scriptiz : traitement des breuvages de Vivre
                        else if (CheckType("BottleBiereCommune")) Content = BeverageType.BiereCommune;
                        else if (CheckType("BottleBiereAmbre")) Content = BeverageType.BiereAmbre;
                        else if (CheckType("BottleBiereMiel")) Content = BeverageType.BiereMiel;
                        else if (CheckType("BottleBiereBrune")) Content = BeverageType.BiereBrune;
                        else if (CheckType("BottleBiereEpice")) Content = BeverageType.BiereEpice;
                        else if (CheckType("BottleMoinetteYew")) Content = BeverageType.MoinetteYew;
                        else if (CheckType("BottleKwak")) Content = BeverageType.Kwak;
                        else if (CheckType("BottleBiereSorciere")) Content = BeverageType.BiereSorciere;
                        else if (CheckType("BottleLiquor")) Content = BeverageType.Liquor;
                        else if (CheckType("BottleWine")) Content = BeverageType.Wine;

                        // Scriptiz : Ajouts pour Crystal
                        else if (CheckType("BottleLaitChevre")) Content = BeverageType.LaitChevre;
                        else if (CheckType("BottleLaitBrebis")) Content = BeverageType.LaitBrebis;
                        else if (CheckType("BottleJusPomme")) Content = BeverageType.JusPomme;
                        else if (CheckType("BottleJusPeche")) Content = BeverageType.JusPeche;
                        else if (CheckType("BottleJusRaisin")) Content = BeverageType.JusRaisin;
                        else throw new Exception(World.LoadingType);

                        Quantity = MaxQuantity;

                        break;
                    }
            }
        }
    }

    public class Jug : BaseBeverage
    {
        public override int BaseLabelNumber { get { return 1042965; } } // a jug of Ale
        public override int MaxQuantity { get { return 10; } }
        public override bool Fillable { get { return false; } }

        public override int ComputeItemID()
        {
            if (!IsEmpty)
                return 0x9C8;

            return 0;
        }

        [Constructable]
        public Jug(BeverageType type)
            : base(type)
        {
            Weight = 1.0;
        }

        public Jug(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class CeramicMug : BaseBeverage
    {
        public override int BaseLabelNumber { get { return 1042982; } } // a ceramic mug of Ale
        public override int MaxQuantity { get { return 1; } }

        public override int ComputeItemID()
        {
            if (ItemID >= 0x995 && ItemID <= 0x999)
                return ItemID;
            else if (ItemID == 0x9CA)
                return ItemID;

            return 0x995;
        }

        [Constructable]
        public CeramicMug()
        {
            Weight = 1.0;
        }

        [Constructable]
        public CeramicMug(BeverageType type)
            : base(type)
        {
            Weight = 1.0;
        }

        public CeramicMug(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class PewterMug : BaseBeverage
    {
        public override int BaseLabelNumber { get { return 1042994; } } // a pewter mug with Ale
        public override int MaxQuantity { get { return 1; } }

        public override int ComputeItemID()
        {
            if (ItemID >= 0xFFF && ItemID <= 0x1002)
                return ItemID;

            return 0xFFF;
        }

        [Constructable]
        public PewterMug()
        {
            Weight = 1.0;
        }

        [Constructable]
        public PewterMug(BeverageType type)
            : base(type)
        {
            Weight = 1.0;
        }

        public PewterMug(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class Goblet : BaseBeverage
    {
        public override int BaseLabelNumber { get { return 1043000; } } // a goblet of Ale
        public override int MaxQuantity { get { return 1; } }

        public override int ComputeItemID()
        {
            if (ItemID == 0x99A || ItemID == 0x9B3 || ItemID == 0x9BF || ItemID == 0x9CB)
                return ItemID;

            return 0x99A;
        }

        [Constructable]
        public Goblet()
        {
            Weight = 1.0;
        }

        [Constructable]
        public Goblet(BeverageType type)
            : base(type)
        {
            Weight = 1.0;
        }

        public Goblet(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [TypeAlias("Server.Items.MugBiereCommune", "Server.Items.MugBiereAmbre", "Server.Items.MugBiereMiel", "Server.Items.MugBiereBrune",
        "Server.Items.MugBiereEpice", "Server.Items.MugMoinetteYew", "Server.Items.MugKwak", "Server.Items.MugBiereSorciere", "Server.Items.MugAle",
        "Server.Items.GlassCider", "Server.Items.GlassLiquor", "Server.Items.GlassMilk", "Server.Items.GlassWine", "Server.Items.GlassWater")]
    public class GlassMug : BaseBeverage
    {
        public override int EmptyLabelNumber { get { return 1022456; } } // mug
        public override int BaseLabelNumber { get { return 1042976; } } // a mug of Ale
        public override int MaxQuantity { get { return 5; } }

        public override int ComputeItemID()
        {
            if (IsEmpty)
                return (ItemID >= 0x1F81 && ItemID <= 0x1F84 ? ItemID : 0x1F81);

            switch (Content)
            {
                case BeverageType.Ale: return (ItemID == 0x9EF ? 0x9EF : 0x9EE);
                case BeverageType.Cider: return (ItemID >= 0x1F7D && ItemID <= 0x1F80 ? ItemID : 0x1F7D);
                case BeverageType.Liquor: return (ItemID >= 0x1F85 && ItemID <= 0x1F88 ? ItemID : 0x1F85);
                case BeverageType.Milk: return (ItemID >= 0x1F89 && ItemID <= 0x1F8C ? ItemID : 0x1F89);
                case BeverageType.Wine: return (ItemID >= 0x1F8D && ItemID <= 0x1F90 ? ItemID : 0x1F8D);
                case BeverageType.Water: return (ItemID >= 0x1F91 && ItemID <= 0x1F94 ? ItemID : 0x1F91);

                // Scriptiz : breuvages pour Vivre
                case BeverageType.BiereCommune: return (ItemID == 0x9EF ? 0x9EF : 0x9EE);
                case BeverageType.BiereAmbre: return (ItemID == 0x9EF ? 0x9EF : 0x9EE);
                case BeverageType.BiereMiel: return (ItemID == 0x9EF ? 0x9EF : 0x9EE);
                case BeverageType.BiereBrune: return (ItemID == 0x9EF ? 0x9EF : 0x9EE);
                case BeverageType.BiereEpice: return (ItemID == 0x9EF ? 0x9EF : 0x9EE);
                case BeverageType.MoinetteYew: return (ItemID == 0x9EF ? 0x9EF : 0x9EE);
                case BeverageType.Kwak: return (ItemID == 0x9EF ? 0x9EF : 0x9EE);
                case BeverageType.BiereSorciere: return (ItemID == 0x9EF ? 0x9EF : 0x9EE);

                // Scriptiz : ajout des breuvages pour Crystal
                case BeverageType.LaitBrebis:
                case BeverageType.LaitChevre: return (ItemID >= 0x1F89 && ItemID <= 0x1F8C ? ItemID : 0x1F89);
                case BeverageType.JusPeche:
                case BeverageType.JusPomme: return (ItemID >= 0x1F7D && ItemID <= 0x1F80 ? ItemID : 0x1F7D);
                case BeverageType.JusRaisin: return (ItemID >= 0x1F8D && ItemID <= 0x1F90 ? ItemID : 0x1F8D);
            }

            return 0;
        }

        [Constructable]
        public GlassMug()
        {
            Weight = 1.0;
        }

        [Constructable]
        public GlassMug(BeverageType type)
            : base(type)
        {
            Weight = 1.0;
        }

        public GlassMug(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        if (CheckType("MugAle")) Content = BeverageType.Ale;
                        else if (CheckType("GlassCider")) Content = BeverageType.Cider;
                        else if (CheckType("GlassLiquor")) Content = BeverageType.Liquor;
                        else if (CheckType("GlassMilk")) Content = BeverageType.Milk;
                        else if (CheckType("GlassWine")) Content = BeverageType.Wine;
                        else if (CheckType("GlassWater")) Content = BeverageType.Water;

                        // Scriptiz : Breuvages de Vivre
                        else if (CheckType("MugBiereCommune")) Content = BeverageType.BiereCommune;
                        else if (CheckType("MugBiereAmbre")) Content = BeverageType.BiereAmbre;
                        else if (CheckType("MugBiereMiel")) Content = BeverageType.BiereMiel;
                        else if (CheckType("MugBiereBrune")) Content = BeverageType.BiereBrune;
                        else if (CheckType("MugBiereEpice")) Content = BeverageType.BiereEpice;
                        else if (CheckType("MugMoinetteYew")) Content = BeverageType.MoinetteYew;
                        else if (CheckType("MugKwak")) Content = BeverageType.Kwak;
                        else if (CheckType("MugBiereSorciere")) Content = BeverageType.BiereSorciere;

                        // Scriptiz : Ajouts pour Crystal
                        else if (CheckType("MugLaitChevre")) Content = BeverageType.LaitChevre;
                        else if (CheckType("MugLaitBrebis")) Content = BeverageType.LaitBrebis;
                        else if (CheckType("MugJusPomme")) Content = BeverageType.JusPomme;
                        else if (CheckType("MugJusPeche")) Content = BeverageType.JusPeche;
                        else if (CheckType("MugJusRaisin")) Content = BeverageType.JusRaisin;
                        else throw new Exception(World.LoadingType);

                        Quantity = MaxQuantity;

                        break;
                    }
            }
        }
    }

    [TypeAlias("Server.Items.PitcherBiereCommune", "Server.Items.PitcherBiereAmbre", "Server.Items.PitcherBiereMiel", "Server.Items.PitcherBiereBrune", "Server.Items.PitcherBiereEpice",
                "Server.Items.PitcherMoinetteYew", "Server.Items.PitcherKwak", "Server.Items.PitcherBiereSorciere", "Server.Items.PitcherAle",
                "Server.Items.PitcherCider", "Server.Items.PitcherLiquor", "Server.Items.PitcherMilk", "Server.Items.PitcherWine",
                "Server.Items.PitcherWater", "Server.Items.GlassPitcher")]
    public class Pitcher : BaseBeverage
    {
        public override int BaseLabelNumber { get { return 1048128; } } // a Pitcher of Ale
        public override int MaxQuantity { get { return 40; } }    // Scriptiz : les pichets passent à 40 doses (cfr uo.com)

        public override int ComputeItemID()
        {
            if (IsEmpty)
            {
                if (ItemID == 0x9A7 || ItemID == 0xFF7)
                    return ItemID;

                return 0xFF6;
            }

            switch (Content)
            {
                // Scriptiz : ajout des bières de Vivre
                case BeverageType.BiereAmbre:
                case BeverageType.BiereBrune:
                case BeverageType.BiereCommune:
                case BeverageType.BiereEpice:
                case BeverageType.BiereMiel:
                case BeverageType.BiereSorciere:
                case BeverageType.MoinetteYew:
                case BeverageType.Kwak:
                case BeverageType.Ale:
                    {
                        if (ItemID == 0x1F96)
                            return ItemID;

                        return 0x1F95;
                    }
                // Scriptiz : ajout de boissons
                case BeverageType.JusPeche:
                case BeverageType.JusPomme:
                case BeverageType.Cider:
                    {
                        if (ItemID == 0x1F98)
                            return ItemID;

                        return 0x1F97;
                    }
                case BeverageType.Liquor:
                    {
                        if (ItemID == 0x1F9A)
                            return ItemID;

                        return 0x1F99;
                    }
                // Scriptiz : ajout de breuvages
                case BeverageType.LaitChevre:
                case BeverageType.LaitBrebis:
                case BeverageType.Milk:
                    {
                        if (ItemID == 0x9AD)
                            return ItemID;

                        return 0x9F0;
                    }
                // Scriptiz : ajout de breuvages
                case BeverageType.JusRaisin:
                case BeverageType.Wine:
                    {
                        if (ItemID == 0x1F9C)
                            return ItemID;

                        return 0x1F9B;
                    }
                case BeverageType.Water:
                    {
                        if (ItemID == 0xFF8 || ItemID == 0xFF9 || ItemID == 0x1F9E)
                            return ItemID;

                        return 0x1F9D;
                    }
            }

            return 0;
        }

        [Constructable]
        public Pitcher()
        {
            Weight = 2.0;
        }

        [Constructable]
        public Pitcher(BeverageType type)
            : base(type)
        {
            Weight = 2.0;
        }

        public Pitcher(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            if (CheckType("PitcherWater") || CheckType("GlassPitcher"))
                base.InternalDeserialize(reader, false);
            else
                base.InternalDeserialize(reader, true);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        if (CheckType("PitcherAle")) Content = BeverageType.Ale;
                        else if (CheckType("PitcherCider")) Content = BeverageType.Cider;
                        else if (CheckType("PitcherLiquor")) Content = BeverageType.Liquor;
                        else if (CheckType("PitcherMilk")) Content = BeverageType.Milk;
                        else if (CheckType("PitcherWine")) Content = BeverageType.Wine;
                        else if (CheckType("PitcherWater")) Content = BeverageType.Water;
                        else if (CheckType("GlassPitcher")) Content = BeverageType.Water;

                        // Scriptiz : breuvages de Vivre
                        else if (CheckType("PitcherBiereCommune")) Content = BeverageType.BiereCommune;
                        else if (CheckType("PitcherBiereAmbre")) Content = BeverageType.BiereAmbre;
                        else if (CheckType("PitcherBiereMiel")) Content = BeverageType.BiereMiel;
                        else if (CheckType("PitcherBiereBrune")) Content = BeverageType.BiereBrune;
                        else if (CheckType("PitcherBiereEpice")) Content = BeverageType.BiereEpice;
                        else if (CheckType("PitcherMoinetteYew")) Content = BeverageType.MoinetteYew;
                        else if (CheckType("PitcherKwak")) Content = BeverageType.Kwak;
                        else if (CheckType("PitcherBiereSorciere")) Content = BeverageType.BiereSorciere;

                        // Scriptiz : Ajouts pour Crystal
                        else if (CheckType("PitcherLaitChevre")) Content = BeverageType.LaitChevre;
                        else if (CheckType("PitcherLaitBrebis")) Content = BeverageType.LaitBrebis;
                        else if (CheckType("PitcherJusPomme")) Content = BeverageType.JusPomme;
                        else if (CheckType("PitcherJusPeche")) Content = BeverageType.JusPeche;
                        else if (CheckType("PitcherJusRaisin")) Content = BeverageType.JusRaisin;
                        else throw new Exception(World.LoadingType);

                        Quantity = MaxQuantity;

                        break;
                    }
            }
        }
    }

    public abstract class BaseBeverage : Item, IHasQuantity
    {
        private BeverageType m_Content;
        private int m_Quantity;
        private Mobile m_Poisoner;
        private Poison m_Poison;

        public override int LabelNumber
        {
            get
            {
                if ((int)this.m_Content > (int)BeverageType.Water) return BaseLabelNumber; // Scriptiz : basenumber si boisson custom !

                int num = BaseLabelNumber;

                if (IsEmpty || num == 0)
                    return EmptyLabelNumber;

                return BaseLabelNumber + (int)m_Content;
            }
        }

        // Scriptiz : Création du nom pour les breuvages spéciaux de vivre
        public override string DefaultName
        {
            get
            {
                if (this == null) return null;

                if ((int)this.m_Content <= (int)BeverageType.Water) return null;

                if (this.m_Quantity == 0) return null;

                string theName = null;  // construisons le nom final

                if (this.GetType() == typeof(Pitcher))
                    theName = "Pichet de ";
                else if (this.GetType() == typeof(BeverageBottle))
                    theName = "Bouteille de ";
                else if (this.GetType() == typeof(Jug))
                    theName = "Pot de ";

                switch (this.m_Content)
                {
                    case BeverageType.BiereCommune:
                        theName += "Bière Commune";
                        break;
                    case BeverageType.BiereAmbre:
                        theName += "Bière Ambrée";
                        break;
                    case BeverageType.BiereMiel:
                        theName += "Bière au Miel";
                        break;
                    case BeverageType.BiereBrune:
                        theName += "Bière Brune";
                        break;
                    case BeverageType.BiereEpice:
                        theName += "Bière aux Epices";
                        break;
                    case BeverageType.BiereSorciere:
                        theName += "Bière de Sorcière";
                        break;
                    case BeverageType.MoinetteYew:
                        theName += "Moinette de Yew";
                        break;
                    case BeverageType.Kwak:
                        theName += "Bière Kwak";
                        break;
                    // Scriptiz : ajout de quelques trucs pour Crystal
                    case BeverageType.LaitChevre:
                        theName += "Lait de Chêvre";
                        break;
                    case BeverageType.LaitBrebis:
                        theName += "Lait de Brebis";
                        break;
                    case BeverageType.JusPomme:
                        theName += "Jus de Pommes";
                        break;
                    case BeverageType.JusPeche:
                        theName += "Jus de Pêches";
                        break;
                    case BeverageType.JusRaisin:
                        theName += "Jus de Raisins";
                        break;
                }

                return theName;
            }
        }

        public virtual bool ShowQuantity { get { return (MaxQuantity > 1); } }
        public virtual bool Fillable { get { return true; } }
        public virtual bool Pourable { get { return true; } }

        public virtual int EmptyLabelNumber { get { return base.LabelNumber; } }
        public virtual int BaseLabelNumber { get { return 0; } }

        public abstract int MaxQuantity { get; }
        public abstract int ComputeItemID();

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsEmpty
        {
            get { return (m_Quantity <= 0); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ContainsAlchohol
        {
            get { return (!IsEmpty && m_Content != BeverageType.Milk && m_Content != BeverageType.Water); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsFull
        {
            get { return (m_Quantity >= MaxQuantity); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Poison Poison
        {
            get { return m_Poison; }
            set { m_Poison = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Poisoner
        {
            get { return m_Poisoner; }
            set { m_Poisoner = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public BeverageType Content
        {
            get { return m_Content; }
            set
            {
                m_Content = value;

                InvalidateProperties();

                int itemID = ComputeItemID();

                if (itemID > 0)
                    ItemID = itemID;
                else
                    Delete();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Quantity
        {
            get { return m_Quantity; }
            set
            {
                if (value < 0)
                    value = 0;
                else if (value > MaxQuantity)
                    value = MaxQuantity;

                m_Quantity = value;

                InvalidateProperties();

                int itemID = ComputeItemID();

                if (itemID > 0)
                    ItemID = itemID;
                else
                    Delete();
            }
        }

        public virtual int GetQuantityDescription()
        {
            int perc = (m_Quantity * 100) / MaxQuantity;

            if (perc <= 0)
                return 1042975; // It's empty.
            else if (perc <= 33)
                return 1042974; // It's nearly empty.
            else if (perc <= 66)
                return 1042973; // It's half full.
            else
                return 1042972; // It's full.
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (ShowQuantity)
                list.Add(GetQuantityDescription());
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);

            if (ShowQuantity)
                LabelTo(from, GetQuantityDescription());
        }

        // Scriptiz : ajout du menu contextuel pour vider un breuvage
        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from.Alive)
                list.Add(new ContextMenus.BeverageEntry(from, this));
        }

        public virtual bool ValidateUse(Mobile from, bool message)
        {
            if (Deleted)
                return false;

            if (!Movable && !Fillable)
            {
                Multis.BaseHouse house = Multis.BaseHouse.FindHouseAt(this);

                if (house == null || !house.IsLockedDown(this))
                {
                    if (message)
                        from.SendLocalizedMessage(502946, "", 0x59); // That belongs to someone else.

                    return false;
                }
            }

            if (from.Map != Map || !from.InRange(GetWorldLocation(), 2) || !from.InLOS(this))
            {
                if (message)
                    from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.

                return false;
            }

            return true;
        }

        public virtual void Fill_OnTarget(Mobile from, object targ)
        {
            if (!IsEmpty || !Fillable || !ValidateUse(from, false))
                return;

            if (targ is BaseBeverage)
            {
                BaseBeverage bev = (BaseBeverage)targ;

                if (bev.IsEmpty || !bev.ValidateUse(from, true))
                    return;

                this.Content = bev.Content;
                this.Poison = bev.Poison;
                this.Poisoner = bev.Poisoner;

                if (bev.Quantity > this.MaxQuantity)
                {
                    this.Quantity = this.MaxQuantity;
                    bev.Quantity -= this.MaxQuantity;
                }
                else
                {
                    this.Quantity += bev.Quantity;
                    bev.Quantity = 0;
                }
                from.PlaySound(0x2D6);
            }
            // Scriptiz : Permet de remplir son pichet sur des WaterThrough et sur des water barrel statiques
            else if (targ is Static)
            {
                Static s = (Static)targ;
                if (from.Map != s.Map || !from.InRange(s.GetWorldLocation(), 2) || !from.InLOS(s))
                {
                    from.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "Vous ne pouvez atteindre cela"); // I can't reach that.
                    return;
                }
                bool filled = BaseBeverage.FillFromStaticWaterSource(from, this, s.ItemID);
                if (filled) from.SendMessage("Vous remplissez votre contenant d'eau"); // You fill the container with water.
            }
            // Scriptiz : Permet de remplir sur des tiles d'eau
            else if (targ is StaticTarget)
            {
                StaticTarget st = (StaticTarget)targ;
                if (!from.InRange(new Point2D(st.Location.X, st.Location.Y), 2) || !from.InLOS(st))
                {
                    from.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "Vous ne pouvez atteindre cela"); // I can't reach that.
                    return;
                }
                bool filled = BaseBeverage.FillFromStaticWaterSource(from, this, st.ItemID);
                if (filled) from.SendMessage("Vous remplissez votre contenant d'eau"); // You fill the container with water.
            }
            else if (targ is BaseWaterContainer && !(targ is Bucket))
            {
                BaseWaterContainer bwc = targ as BaseWaterContainer;

                if (Quantity == 0 || (Content == BeverageType.Water && !IsFull))
                {
                    int iNeed = Math.Min((MaxQuantity - Quantity), bwc.Quantity);

                    if (iNeed > 0 && !bwc.IsEmpty && !IsFull)
                    {
                        bwc.Quantity -= iNeed;
                        Quantity += iNeed;
                        Content = BeverageType.Water;

                        from.PlaySound(0x4E);
                    }
                }
            }
            else if (targ is Item)
            {
                Item item = (Item)targ;
                IWaterSource src;

                src = (item as IWaterSource);

                if (src == null && item is AddonComponent)
                    src = (((AddonComponent)item).Addon as IWaterSource);

                if (src == null || src.Quantity <= 0)
                    return;

                if (from.Map != item.Map || !from.InRange(item.GetWorldLocation(), 2) || !from.InLOS(item))
                {
                    from.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "Vous ne pouvez atteindre cela"); // I can't reach that.
                    return;
                }

                this.Content = BeverageType.Water;
                this.Poison = null;
                this.Poisoner = null;

                if (src.Quantity > this.MaxQuantity)
                {
                    this.Quantity = this.MaxQuantity;
                    src.Quantity -= this.MaxQuantity;
                }
                else
                {
                    this.Quantity += src.Quantity;
                    src.Quantity = 0;
                }
                from.PlaySound(0x2D6);
                from.SendMessage("Vous remplissez votre contenant d'eau"); // You fill the container with water.
            }
            else if (targ is Cow)
            {
                Cow cow = (Cow)targ;

                if (cow.TryMilk(from))
                {
                    Content = BeverageType.Milk;
                    Quantity = MaxQuantity;
                    from.SendMessage("Vous remplissez votre contenant de lait"); // You fill the container with milk.
                    from.PlaySound(0x2D6);
                }
            }
            else if (targ is LandTarget)
            {
                int tileID = ((LandTarget)targ).TileID;

                PlayerMobile player = from as PlayerMobile;

                if (player != null)
                {
                    QuestSystem qs = player.Quest;

                    if (qs is WitchApprenticeQuest)
                    {
                        FindIngredientObjective obj = qs.FindObjective(typeof(FindIngredientObjective)) as FindIngredientObjective;

                        if (obj != null && !obj.Completed && obj.Ingredient == Ingredient.SwampWater)
                        {
                            bool contains = false;

                            for (int i = 0; !contains && i < m_SwampTiles.Length; i += 2)
                                contains = (tileID >= m_SwampTiles[i] && tileID <= m_SwampTiles[i + 1]);

                            if (contains)
                            {
                                Delete();

                                player.SendLocalizedMessage(1055035); // You dip the container into the disgusting swamp water, collecting enough for the Hag's vile stew.
                                obj.Complete();
                                return; // Scriptiz : si c'était pour la quête on s'arrête là
                            }
                        }
                    }

                    // Scriptiz : Le joueur peut remplir à partir de LandTarget
                    LandTarget lt = (LandTarget)targ;
                    if (!from.InRange(new Point2D(lt.Location.X, lt.Location.Y), 2) || !from.InLOS(lt))
                    {
                        from.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "Vous ne pouvez atteindre cela"); // I can't reach that.
                        return;
                    }

                    int[] waterTiles = new int[] {
                            0xA8, 0xA9, 0xAA, 0xAB,
                            0x136, 0x137,
                            0x3FF0, 0x3FF1, 0x3FF2, 0x3FF3,
                        };

                    bool water = false;
                    for (int i = 0; i < waterTiles.Length; i++)
                    {
                        if (waterTiles[i] == tileID)
                        {
                            water = true;
                            break;
                        }
                    }

                    if (water)
                    {
                        this.Poison = null;
                        this.Poisoner = null;

                        int randomSeed = Utility.Random(1, 100);

                        if (randomSeed <= 20)
                        {
                            if (randomSeed <= 5)
                                this.Poison = Poison.Regular;
                            else
                                this.Poison = Poison.Lesser;
                        }

                        this.Content = BeverageType.Water;
                        this.Quantity = this.MaxQuantity;
                        from.SendMessage("Vous remplissez votre contenant d'eau"); // You fill the container with water.
                        from.PlaySound(0x2D6);
                    }
                }
            }
        }

        // Scriptiz : permet de remplir son pichet à partir de staticTarget ou de StaticItem
        private static bool FillFromStaticWaterSource(Mobile from, BaseBeverage b, int staticId)
        {
            if (!b.IsEmpty || !b.Fillable || !b.ValidateUse(from, false))
                return false;

            if (from == null) return false;
            if (b == null) return false;

            b.Poison = null;
            b.Poisoner = null;

            int randomSeed = Utility.Random(1, 100);

            // water barrels
            if (staticId == 5453 || staticId == 3707)
            {
                b.Quantity = (b.MaxQuantity < 5 ? b.MaxQuantity : 5);

                if (Utility.Random(100) <= 5)
                    b.Poison = Poison.Lesser;
            }
            // water trough
            else if (staticId >= 2881 && staticId <= 2884)
            {
                if (randomSeed <= 10)
                    b.Poison = Poison.Lesser;
            }
            // water tiles & waterfalls
            else if ((staticId >= 6038 && staticId <= 6066) || (staticId >= 13422 && staticId <= 13525) || (staticId >= 13549 && staticId <= 13608))
            {
                if (randomSeed <= 20)
                {
                    if (randomSeed <= 5)
                        b.Poison = Poison.Regular;
                    else
                        b.Poison = Poison.Lesser;
                }
            }
            // fountains
            else if ((staticId >= 5937 && staticId <= 5978) || (staticId >= 6595 && staticId <= 6636))
            {
                if (randomSeed <= 20)
                {
                    if (randomSeed <= 5)
                        b.Poison = Poison.Regular;
                    else
                        b.Poison = Poison.Lesser;
                }
            }
            // swamp tiles (disactivated)
            //else if (staticId >= 12809 && staticId <= 12933)
            //{
            //    if (randomSeed <= 50)
            //        b.Poison = Poison.Regular;
            //    else if (randomSeed <= 80)
            //        b.Poison = Poison.Greater;
            //    else if (randomSeed <= 100)
            //        b.Poison = Poison.Deadly;
            //}
            else
            {
                b.Quantity = 0;
                return false;
            }

            // Fill the beverage
            b.Content = BeverageType.Water;
            if (staticId != 5453 && staticId != 3707) b.Quantity = b.MaxQuantity;

            from.PlaySound(0x2D6);
            return true;
        }

        private static int[] m_SwampTiles = new int[]
			{
				0x9C4, 0x9EB,
				0x3D65, 0x3D65,
				0x3DC0, 0x3DD9,
				0x3DDB, 0x3DDC,
				0x3DDE, 0x3EF0,
				0x3FF6, 0x3FF6,
				0x3FFC, 0x3FFE,
			};

        #region Effects of achohol
        private static Hashtable m_Table = new Hashtable();

        public static void Initialize()
        {
            EventSink.Login += new LoginEventHandler(EventSink_Login);
        }

        private static void EventSink_Login(LoginEventArgs e)
        {
            CheckHeaveTimer(e.Mobile);
        }

        public static void CheckHeaveTimer(Mobile from)
        {
            if (from.BAC > 0 && from.Map != Map.Internal && !from.Deleted)
            {
                Timer t = (Timer)m_Table[from];

                if (t == null)
                {
                    if (from.BAC > 60)
                        from.BAC = 60;

                    t = new HeaveTimer(from);
                    t.Start();

                    m_Table[from] = t;
                }
            }
            else
            {
                Timer t = (Timer)m_Table[from];

                if (t != null)
                {
                    t.Stop();
                    m_Table.Remove(from);

                    from.SendMessage("Vous vous sentez mieux"); // You feel sober.
                }
            }
        }

        // Scriptiz : calcul le FillFactor en fonction du type du breuvage
        public int ComputeFillFactor()
        {
            switch (Content)
            {
                case BeverageType.BiereAmbre:
                case BeverageType.BiereBrune:
                case BeverageType.BiereCommune:
                case BeverageType.BiereEpice:
                case BeverageType.BiereMiel:
                case BeverageType.BiereSorciere:
                case BeverageType.Kwak:
                case BeverageType.MoinetteYew:
                    return 3;
                case BeverageType.LaitBrebis:
                case BeverageType.LaitChevre:
                case BeverageType.JusPeche:
                case BeverageType.JusPomme:
                case BeverageType.JusRaisin:
                case BeverageType.Ale:
                case BeverageType.Cider:
                case BeverageType.Liquor:
                case BeverageType.Milk:
                case BeverageType.Wine:
                    return 2;
                case BeverageType.Water:
                    return 1;
                default: return 1;
            }
        }

        private class HeaveTimer : Timer
        {
            private Mobile m_Drunk;

            public HeaveTimer(Mobile drunk)
                : base(TimeSpan.FromSeconds(5.0), TimeSpan.FromSeconds(5.0))
            {
                m_Drunk = drunk;

                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Drunk.Deleted || m_Drunk.Map == Map.Internal)
                {
                    Stop();
                    m_Table.Remove(m_Drunk);
                }
                else if (m_Drunk.Alive)
                {
                    if (m_Drunk.BAC > 60)
                        m_Drunk.BAC = 60;

                    // chance to get sober
                    if (10 > Utility.Random(100))
                        --m_Drunk.BAC;

                    // lose some stats
                    m_Drunk.Stam -= 1;
                    m_Drunk.Mana -= 1;

                    if (Utility.Random(1, 4) == 1)
                    {
                        if (!m_Drunk.Mounted)
                        {
                            // turn in a random direction
                            m_Drunk.Direction = (Direction)Utility.Random(8);

                            // heave
                            m_Drunk.Animate(32, 5, 1, true, false, 0);
                        }

                        // *hic*
                        m_Drunk.PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, 500849);
                    }

                    if (m_Drunk.BAC <= 0)
                    {
                        Stop();
                        m_Table.Remove(m_Drunk);

                        m_Drunk.SendMessage("Vous vous sentez mieux"); // You feel sober.
                    }
                }
            }
        }
        #endregion

        public virtual void Pour_OnTarget(Mobile from, object targ)
        {
            if (IsEmpty || !Pourable || !ValidateUse(from, false))
                return;

            if (targ is BaseBeverage)
            {
                BaseBeverage bev = (BaseBeverage)targ;

                if (!bev.ValidateUse(from, true))
                    return;

                if (bev.IsFull && bev.Content == this.Content)
                {
                    from.SendMessage("Vous ne pouvez verser ici, c'est déjà plein"); // Couldn't pour it there.  It was already full.
                }
                else if (!bev.IsEmpty)
                {
                    from.SendMessage("Vous ne pouvez verser ici"); // Can't pour it there.
                }
                else
                {
                    bev.Content = this.Content;
                    bev.Poison = this.Poison;
                    bev.Poisoner = this.Poisoner;

                    if (this.Quantity > bev.MaxQuantity)
                    {
                        bev.Quantity = bev.MaxQuantity;
                        this.Quantity -= bev.MaxQuantity;
                    }
                    else
                    {
                        bev.Quantity += this.Quantity;
                        this.Quantity = 0;
                    }

                    from.PlaySound(0x4E);
                }
            }
            else if (from == targ)
            {
                if (from.Thirst < 20)
                    from.Thirst += ComputeFillFactor(); // Scriptiz : ajout du FillFactor (anciennement : 1)

                if (ContainsAlchohol)
                {
                    int bac = 0;

                    switch (this.Content)
                    {
                        case BeverageType.Ale: bac = 1; break;
                        case BeverageType.Wine: bac = 2; break;
                        case BeverageType.Cider: bac = 3; break;
                        case BeverageType.Liquor: bac = 4; break;

                        // Scriptiz : Breuvages alcolisés de Vivre
                        case BeverageType.BiereCommune:
                        case BeverageType.BiereAmbre:
                        case BeverageType.BiereMiel:
                        case BeverageType.BiereBrune:
                        case BeverageType.BiereEpice:
                        case BeverageType.MoinetteYew:
                        case BeverageType.BiereSorciere: bac = 1; break;
                        case BeverageType.Kwak: bac = 2; break;
                    }

                    from.BAC += bac;

                    if (from.BAC > 60)
                        from.BAC = 60;

                    CheckHeaveTimer(from);
                }

                from.PlaySound(Utility.RandomList(0x30, 0x2D6));

                if (m_Poison != null)
                    from.ApplyPoison(m_Poisoner, m_Poison);

                --Quantity;
            }
            else if (targ is BaseWaterContainer && !(targ is Bucket))
            {
                BaseWaterContainer bwc = targ as BaseWaterContainer;

                if (Content != BeverageType.Water)
                {
                    from.SendLocalizedMessage(500842); // Can't pour that in there.
                }
                else if (bwc.Items.Count != 0)
                {
                    from.SendLocalizedMessage(500841); // That has something in it.
                }
                else
                {
                    int itNeeds = Math.Min((bwc.MaxQuantity - bwc.Quantity), Quantity);

                    if (itNeeds > 0)
                    {
                        bwc.Quantity += itNeeds;
                        Quantity -= itNeeds;

                        from.PlaySound(0x4E);
                    }
                }
            }
            else if (targ is PlantItem)
            {
                ((PlantItem)targ).Pour(from, this);
            }
            else if (targ is AddonComponent &&
                (((AddonComponent)targ).Addon is WaterVatEast || ((AddonComponent)targ).Addon is WaterVatSouth) &&
                this.Content == BeverageType.Water)
            {
                PlayerMobile player = from as PlayerMobile;

                if (player != null)
                {
                    SolenMatriarchQuest qs = player.Quest as SolenMatriarchQuest;

                    if (qs != null)
                    {
                        QuestObjective obj = qs.FindObjective(typeof(GatherWaterObjective));

                        if (obj != null && !obj.Completed)
                        {
                            BaseAddon vat = ((AddonComponent)targ).Addon;

                            if (vat.X > 5784 && vat.X < 5814 && vat.Y > 1903 && vat.Y < 1934 &&
                                ((qs.RedSolen && vat.Map == Map.Trammel) || (!qs.RedSolen && vat.Map == Map.Felucca)))
                            {
                                if (obj.CurProgress + Quantity > obj.MaxProgress)
                                {
                                    int delta = obj.MaxProgress - obj.CurProgress;

                                    Quantity -= delta;
                                    obj.CurProgress = obj.MaxProgress;
                                }
                                else
                                {
                                    obj.CurProgress += Quantity;
                                    Quantity = 0;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                from.SendMessage("Vous ne pouvez verser ici"); // Can't pour it there.
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsEmpty)
            {
                if (!Fillable || !ValidateUse(from, true))
                    return;

                from.BeginTarget(-1, true, TargetFlags.None, new TargetCallback(Fill_OnTarget));
                SendLocalizedMessageTo(from, 500837); // Fill from what?
            }
            else if (Pourable && ValidateUse(from, true))
            {
                from.BeginTarget(-1, true, TargetFlags.None, new TargetCallback(Pour_OnTarget));
                from.SendMessage("Sur quoi voulez-vous l'utiliser?"); // What do you want to use this on?
            }
        }

        public static bool ConsumeTotal(Container pack, BeverageType content, int quantity)
        {
            return ConsumeTotal(pack, typeof(BaseBeverage), content, quantity);
        }

        public static bool ConsumeTotal(Container pack, Type itemType, BeverageType content, int quantity)
        {
            Item[] items = pack.FindItemsByType(itemType);

            // First pass, compute total
            int total = 0;

            for (int i = 0; i < items.Length; ++i)
            {
                BaseBeverage bev = items[i] as BaseBeverage;

                if (bev != null && bev.Content == content && !bev.IsEmpty)
                    total += bev.Quantity;
            }

            if (total >= quantity)
            {
                // We've enough, so consume it

                int need = quantity;

                for (int i = 0; i < items.Length; ++i)
                {
                    BaseBeverage bev = items[i] as BaseBeverage;

                    if (bev == null || bev.Content != content || bev.IsEmpty)
                        continue;

                    int theirQuantity = bev.Quantity;

                    if (theirQuantity < need)
                    {
                        bev.Quantity = 0;
                        need -= theirQuantity;
                    }
                    else
                    {
                        bev.Quantity -= need;
                        return true;
                    }
                }
            }

            return false;
        }

        public BaseBeverage()
        {
            ItemID = ComputeItemID();
        }

        public BaseBeverage(BeverageType type)
        {
            m_Content = type;
            m_Quantity = MaxQuantity;
            ItemID = ComputeItemID();
        }

        public BaseBeverage(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write((Mobile)m_Poisoner);

            Poison.Serialize(m_Poison, writer);
            writer.Write((int)m_Content);
            writer.Write((int)m_Quantity);
        }

        protected bool CheckType(string name)
        {
            return (World.LoadingType == String.Format("Server.Items.{0}", name));
        }

        public override void Deserialize(GenericReader reader)
        {
            InternalDeserialize(reader, true);
        }

        protected void InternalDeserialize(GenericReader reader, bool read)
        {
            base.Deserialize(reader);

            if (!read)
                return;

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        m_Poisoner = reader.ReadMobile();
                        goto case 0;
                    }
                case 0:
                    {
                        m_Poison = Poison.Deserialize(reader);
                        m_Content = (BeverageType)reader.ReadInt();
                        m_Quantity = reader.ReadInt();
                        break;
                    }
            }
        }
    }
}