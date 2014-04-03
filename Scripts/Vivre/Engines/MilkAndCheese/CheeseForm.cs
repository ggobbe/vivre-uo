using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Prompts;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;
using Server.ContextMenus;

namespace Server.Items
{
    public class CheeseForm : Item
    {
        private Milk m_Content;
        private DateTime m_TimeStart;
        private int m_MaxQuantity = 10;
        private int m_CheeseAmount = 5;
        private int m_Quantity;
        private double m_CookingValue;

        [CommandProperty(AccessLevel.GameMaster)]
        public Milk Content
        {
            get { return m_Content; }
            set { m_Content = value; ComputeName(); }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public DateTime TimeStart
        {
            get { return m_TimeStart; }
            set { m_TimeStart = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public int MaxQuantity
        {
            get { return m_MaxQuantity; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Quantity
        {
            get { return m_Quantity; }
            set { m_Quantity = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public double CookingValue
        {
            get { return m_CookingValue; }
            set { m_CookingValue = value; }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public int CheeseAmount
        {
            get { return m_CheeseAmount; }
        }

        [Constructable]
        public CheeseForm()
            : base(0x0E78)
        {
            Weight = 10.0;
            Name = "Moule à fromage";
            Hue = 0x481;
        }

        public CheeseForm(Serial serial)
            : base(serial)
        {
        }

        public void ComputeName()
        {
            string basename = "Moule à fromage";
            string nameaddon = "";

            switch (Content)
            {
                case (Milk.Cow): nameaddon += " de vache"; break;
                case (Milk.Goat): nameaddon += " de chèvre"; break;
                case (Milk.Sheep): nameaddon += " de brebis"; break;
            }

            Name = basename + nameaddon;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (MaxQuantity <= Quantity)
                list.Add("En fermentation depuis {0} jours", (int)(DateTime.Now - TimeStart).TotalDays);
            
            list.Add("Ce moule contient {0} litre{1} de lait", Quantity, (Quantity > 0)?"s":"");
        }       

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
            writer.WriteEncodedInt((int)m_Content);
            writer.Write((DateTime)m_TimeStart);
            writer.Write((int)m_MaxQuantity);
            writer.Write((int)m_Quantity);
            writer.Write((double)m_CookingValue);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_Content = (Milk)reader.ReadEncodedInt();
            m_TimeStart = reader.ReadDateTime();
            m_MaxQuantity = reader.ReadInt();
            m_Quantity = reader.ReadInt();
            m_CookingValue = reader.ReadDouble();
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from.Alive && (Quantity >= MaxQuantity))
                list.Add(new ContextMenus.CheeseFormMenu(from, this));
        }
    }
}