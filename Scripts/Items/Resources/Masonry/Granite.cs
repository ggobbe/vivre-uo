using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public abstract class BaseGranite : Item
    {
        private CraftResource m_Resource;

        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource Resource
        {
            get { return m_Resource; }
            set { m_Resource = value; InvalidateProperties(); }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write((int)m_Resource);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                case 0:
                    {
                        m_Resource = (CraftResource)reader.ReadInt();
                        break;
                    }
            }

            if (version < 1)
                Stackable = Core.ML;
        }

        public override double DefaultWeight
        {
            get { return Core.ML ? 1.0 : 10.0; } // Pub 57
        }

        public BaseGranite(CraftResource resource)
            : base(0x1779)
        {
            Hue = CraftResources.GetHue(resource);
            Stackable = Core.ML;

            m_Resource = resource;
        }

        public BaseGranite(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber { get { return 1044607; } } // high quality granite

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (!CraftResources.IsStandard(m_Resource))
            {
                int num = CraftResources.GetLocalizationNumber(m_Resource);

                if (num > 0)
                    list.Add(num);
                else
                    list.Add(CraftResources.GetName(m_Resource));
            }
        }
    }

    public class Granite : BaseGranite
    {
        [Constructable]
        public Granite()
            : base(CraftResource.MIron)
        {
        }

        public Granite(Serial serial)
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

    public class BronzeGranite : BaseGranite
    {
        [Constructable]
        public BronzeGranite()
            : base(CraftResource.MBronze)
        {
        }

        public BronzeGranite(Serial serial)
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


    public class GoldGranite : BaseGranite
    {
        [Constructable]
        public GoldGranite()
            : base(CraftResource.MGold)
        {
        }

        public GoldGranite(Serial serial)
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


    public class CopperGranite : BaseGranite
    {
        [Constructable]
        public CopperGranite()
            : base(CraftResource.MCopper)
        {
        }

        public CopperGranite(Serial serial)
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


    public class OldcopperGranite : BaseGranite
    {
        [Constructable]
        public OldcopperGranite()
            : base(CraftResource.MOldcopper)
        {
        }

        public OldcopperGranite(Serial serial)
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


    public class DullcopperGranite : BaseGranite
    {
        [Constructable]
        public DullcopperGranite()
            : base(CraftResource.MDullcopper)
        {
        }

        public DullcopperGranite(Serial serial)
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



    public class SilverGranite : BaseGranite
    {
        [Constructable]
        public SilverGranite()
            : base(CraftResource.MSilver)
        {
        }

        public SilverGranite(Serial serial)
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


    public class ShadowGranite : BaseGranite
    {
        [Constructable]
        public ShadowGranite()
            : base(CraftResource.MShadow)
        {
        }

        public ShadowGranite(Serial serial)
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


    public class BloodrockGranite : BaseGranite
    {
        [Constructable]
        public BloodrockGranite()
            : base(CraftResource.MBloodrock)
        {
        }

        public BloodrockGranite(Serial serial)
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


    public class BlackrockGranite : BaseGranite
    {
        [Constructable]
        public BlackrockGranite()
            : base(CraftResource.MBlackrock)
        {
        }

        public BlackrockGranite(Serial serial)
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


    public class MytherilGranite : BaseGranite
    {
        [Constructable]
        public MytherilGranite()
            : base(CraftResource.MMytheril)
        {
        }

        public MytherilGranite(Serial serial)
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


    public class RoseGranite : BaseGranite
    {
        [Constructable]
        public RoseGranite()
            : base(CraftResource.MRose)
        {
        }

        public RoseGranite(Serial serial)
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


    public class VeriteGranite : BaseGranite
    {
        [Constructable]
        public VeriteGranite()
            : base(CraftResource.MVerite)
        {
        }

        public VeriteGranite(Serial serial)
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


    public class AgapiteGranite : BaseGranite
    {
        [Constructable]
        public AgapiteGranite()
            : base(CraftResource.MAgapite)
        {
        }

        public AgapiteGranite(Serial serial)
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


    public class RustyGranite : BaseGranite
    {
        [Constructable]
        public RustyGranite()
            : base(CraftResource.MRusty)
        {
        }

        public RustyGranite(Serial serial)
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


    public class ValoriteGranite : BaseGranite
    {
        [Constructable]
        public ValoriteGranite()
            : base(CraftResource.MValorite)
        {
        }

        public ValoriteGranite(Serial serial)
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


    public class DragonGranite : BaseGranite
    {
        [Constructable]
        public DragonGranite()
            : base(CraftResource.MDragon)
        {
        }

        public DragonGranite(Serial serial)
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



    public class TitanGranite : BaseGranite
    {
        [Constructable]
        public TitanGranite()
            : base(CraftResource.MTitan)
        {
        }

        public TitanGranite(Serial serial)
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


    public class CrystalineGranite : BaseGranite
    {
        [Constructable]
        public CrystalineGranite()
            : base(CraftResource.MCrystaline)
        {
        }

        public CrystalineGranite(Serial serial)
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


    public class KryniteGranite : BaseGranite
    {
        [Constructable]
        public KryniteGranite()
            : base(CraftResource.MKrynite)
        {
        }

        public KryniteGranite(Serial serial)
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


    public class VulcanGranite : BaseGranite
    {
        [Constructable]
        public VulcanGranite()
            : base(CraftResource.MVulcan)
        {
        }

        public VulcanGranite(Serial serial)
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


    public class BloodcrestGranite : BaseGranite
    {
        [Constructable]
        public BloodcrestGranite()
            : base(CraftResource.MBloodcrest)
        {
        }

        public BloodcrestGranite(Serial serial)
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


    public class ElvinGranite : BaseGranite
    {
        [Constructable]
        public ElvinGranite()
            : base(CraftResource.MElvin)
        {
        }

        public ElvinGranite(Serial serial)
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


    public class AcidGranite : BaseGranite
    {
        [Constructable]
        public AcidGranite()
            : base(CraftResource.MAcid)
        {
        }

        public AcidGranite(Serial serial)
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


    public class AquaGranite : BaseGranite
    {
        [Constructable]
        public AquaGranite()
            : base(CraftResource.MAqua)
        {
        }

        public AquaGranite(Serial serial)
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



    public class EldarGranite : BaseGranite
    {
        [Constructable]
        public EldarGranite()
            : base(CraftResource.MEldar)
        {
        }

        public EldarGranite(Serial serial)
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


    public class GlowingGranite : BaseGranite
    {
        [Constructable]
        public GlowingGranite()
            : base(CraftResource.MGlowing)
        {
        }

        public GlowingGranite(Serial serial)
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


    public class GorganGranite : BaseGranite
    {
        [Constructable]
        public GorganGranite()
            : base(CraftResource.MGorgan)
        {
        }

        public GorganGranite(Serial serial)
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


    public class SandrockGranite : BaseGranite
    {
        [Constructable]
        public SandrockGranite()
            : base(CraftResource.MSandrock)
        {
        }

        public SandrockGranite(Serial serial)
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


    public class SteelGranite : BaseGranite
    {
        [Constructable]
        public SteelGranite()
            : base(CraftResource.MSteel)
        {
        }

        public SteelGranite(Serial serial)
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