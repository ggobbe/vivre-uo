using System;
using Server;

namespace Server.Items
{
    public class SpecialForge : Item
    {
        private CraftResource m_Resource;

        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource Resource
        {
            get { return m_Resource; }
            set { m_Resource = value; Hue = CraftResources.GetHue(m_Resource); InvalidateProperties(); }
        }


        public override void AddNameProperty(ObjectPropertyList list)
        {
            string oreType;
            string complete = "Une forge achevée";

            switch (m_Resource)
            {
                case CraftResource.MShadow: oreType = "Une forge d'ombre"; break; 
                case CraftResource.MBloodrock: oreType = "Une Forge de sang"; break; 
                case CraftResource.MBlackrock: oreType = "Une forge de désespoir"; break; 
                case CraftResource.MVulcan: oreType = "Une forge volcanique"; break; 
                case CraftResource.MAcid: oreType = "Une forge toxique"; break;
                case CraftResource.MAqua: oreType = "Une forge aquatique"; break; 
                case CraftResource.MGlowing: oreType = "Une forge céleste"; break; 
                default: oreType = null; break;
            }

            if (oreType != null)
                list.Add(oreType); 
            else if (m_Resource != CraftResource.None)
                list.Add(complete);
            else
                list.Add(Name);

        }

        [Constructable]
        public SpecialForge()
            : base(0xFB1)
        {
            Stackable = false;
            Name = "Forge inachevée";
            Weight = 200;
        }

        public SpecialForge(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.WriteEncodedInt((int)m_Resource);

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_Resource = (CraftResource)reader.ReadEncodedInt();
        }
    }
}