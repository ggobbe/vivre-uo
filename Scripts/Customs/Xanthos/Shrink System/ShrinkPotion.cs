using Server;
using Server.Items;
using Server.Targeting;
using Xanthos.Interfaces;

namespace Xanthos.ShrinkSystem
{
    public class ShrinkPotion : Item, IShrinkTool
    {
        private int m_Charges = 3;

        [CommandProperty(AccessLevel.GameMaster)]
        public int ShrinkCharges
        {
            get { return m_Charges; }
            set
            {
                if (0 == m_Charges || 0 == (m_Charges = value))
                    Delete();
                else
                    InvalidateProperties();
            }
        }

        #region Constructors
        public ShrinkPotion(Serial serial)
            : base(serial)
        {
        }

        [Constructable]
        public ShrinkPotion()
            : base(0xF04)
        {
            Name = "a Shrink potion";
        }
        #endregion

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add(1060658, "Charges\t{0}", m_Charges.ToString());
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!Movable)
                return;
            else if (from.InRange(this.GetWorldLocation(), 2) == false)
            {
                from.SendLocalizedMessage(500486);	//That is too far away.
                return;
            }

            Container pack = from.Backpack;

            if (!(Parent == from || (pack != null && Parent == pack))) //If not in pack.
            {
                from.SendLocalizedMessage(1042001);	//That must be in your pack to use it.
                return;
            }

            if (from.Skills[SkillName.AnimalTaming].Value < ShrinkConfig.TamingRequired)
            {
                from.SendMessage("You must have at least " + ShrinkConfig.TamingRequired + " animal taming to use a hitching post.");
                return;
            }

            from.Target = new ShrinkTarget(from, this, false);
        }

        #region Serialization
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write(m_Charges);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Charges = reader.ReadInt();
        }
        #endregion
    }
}