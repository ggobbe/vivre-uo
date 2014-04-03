using System;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Items
{
    public abstract class BaseScales : Item, ICommodity
    {
        public override int LabelNumber { get { return 1053139; } } // dragon scales

        private CraftResource m_Resource;

        private bool m_Harvested;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Harvested
        {
            get { return m_Harvested; }
            set { m_Harvested = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource Resource
        {
            get { return m_Resource; }
            set { m_Resource = value; InvalidateProperties(); }
        }

        public override double DefaultWeight
        {
            get { return 0.1; }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (m_Harvested)
            {
                from.SendMessage("Vous ne décelez rien sous ces écailles");
                return;
            }

            from.SendMessage("Vous dénotez certains gravats sur les écailles");
            from.SendMessage("Peut-être qu'un liquide ennemi pourrait les faire partir?");
            from.BeginTarget(2, false, TargetFlags.None, new TargetCallback(OnTarget));
        }

        public void OnTarget(Mobile from, object obj)
        {
            if (!(obj is AlchemyVial))
            {
                from.SendMessage("Cela ne servira à rien");
                return;
            }

            AlchemyVial targ = (AlchemyVial)obj;

            if (targ.AlchemyLiquidType != LiquidType.OgreBlood && targ.AlchemyLiquidType != LiquidType.OrcBlood && targ.AlchemyLiquidType != LiquidType.TrollBlood)
            {
                from.SendMessage("Vous versez le sang, mais rien ne se passe");
                targ.AlchemyLiquidType = LiquidType.None;
                return;
            }

            if (!from.CheckTargetSkill(SkillName.Alchemy, targ, 50, 95))
            {
                from.SendMessage("Vous versez le sang, mais échouez à faire tomber les gravats");
                targ.AlchemyLiquidType = LiquidType.None;
                return;
            }

            int rarete = 0;

            switch (Resource)
            {
                case CraftResource.BlackScales: rarete = 2; break;
                case CraftResource.WhiteScales: rarete = 3; break;
                case CraftResource.GreenScales: rarete = 4; break;
                case CraftResource.BlueScales: rarete = 4; break;
                default: rarete = 6; break;
            }
            if (this.Amount >= rarete)
            {
                from.SendMessage("Vous recueillez des gravats");
                this.m_Harvested = true;
                DragonOre ore = new DragonOre();
                ore.ItemID = 0x19B8;
                ore.Amount = (int)Math.Floor(this.Amount / (double)rarete);
                from.AddToBackpack(ore);
                targ.AlchemyLiquidType = LiquidType.None;
            }
            else
                from.SendMessage("Il n'y en a malheureusement pas assez pour constituer des gravats convenables");
        }



        int ICommodity.DescriptionNumber { get { return LabelNumber; } }
        bool ICommodity.IsDeedable { get { return true; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write((bool)m_Harvested);
            writer.Write((int)m_Resource);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        m_Harvested = reader.ReadBool();
                        goto case 0;
                    }
                case 0:
                    {
                        m_Resource = (CraftResource)reader.ReadInt();
                        break;
                    }
            }
        }

        public override bool StackWith(Mobile from, Item dropped, bool playSound)
        {
            if(!(dropped is BaseScales))
                return false;
            if (((BaseScales)dropped).Harvested != Harvested)
                return false; 
            return base.StackWith(from, dropped, playSound);
        }

        public BaseScales(CraftResource resource)
            : this(resource, 1)
        {
        }

        public BaseScales(CraftResource resource, int amount)
            : base(0x26B4)
        {
            Stackable = true;
            Amount = amount;
            Hue = CraftResources.GetHue(resource);
            m_Resource = resource;
        }


        public BaseScales(CraftResource resource, int amount, bool harvested)
            : base(0x26B4)
        {
            Stackable = true;
            Amount = amount;
            Hue = CraftResources.GetHue(resource);
            m_Resource = resource;
        }
        public BaseScales(Serial serial)
            : base(serial)
        {
        }
    }

    public class RedScales : BaseScales
    {
        [Constructable]
        public RedScales()
            : this(1)
        {
        }

        [Constructable]
        public RedScales(int amount)
            : base(CraftResource.RedScales, amount)
        {
        }

        public RedScales(Serial serial)
            : base(serial)
        {
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

    public class YellowScales : BaseScales
    {
        [Constructable]
        public YellowScales()
            : this(1)
        {
        }

        [Constructable]
        public YellowScales(int amount)
            : base(CraftResource.YellowScales, amount)
        {
        }

        public YellowScales(Serial serial)
            : base(serial)
        {
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

    public class BlackScales : BaseScales
    {
        [Constructable]
        public BlackScales()
            : this(1)
        {
        }

        [Constructable]
        public BlackScales(int amount)
            : base(CraftResource.BlackScales, amount)
        {
        }

        public BlackScales(Serial serial)
            : base(serial)
        {
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

    public class GreenScales : BaseScales
    {
        [Constructable]
        public GreenScales()
            : this(1)
        {
        }

        [Constructable]
        public GreenScales(int amount)
            : base(CraftResource.GreenScales, amount)
        {
        }

        public GreenScales(Serial serial)
            : base(serial)
        {
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

    public class WhiteScales : BaseScales
    {
        [Constructable]
        public WhiteScales()
            : this(1)
        {
        }

        [Constructable]
        public WhiteScales(int amount)
            : base(CraftResource.WhiteScales, amount)
        {
        }

        public WhiteScales(Serial serial)
            : base(serial)
        {
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

    public class BlueScales : BaseScales
    {
        public override int LabelNumber { get { return 1053140; } } // sea serpent scales

        [Constructable]
        public BlueScales()
            : this(1)
        {
        }

        [Constructable]
        public BlueScales(int amount)
            : base(CraftResource.BlueScales, amount)
        {
        }

        public BlueScales(Serial serial)
            : base(serial)
        {
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