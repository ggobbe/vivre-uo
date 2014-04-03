using System;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public class IronBeetleBody : Item
    {
        private double m_SummonScalar;
        private int m_Durability;

        [CommandProperty(AccessLevel.GameMaster)]
        public double SummonScalar
        {
            get { return m_SummonScalar; }
            set { m_SummonScalar = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Durability
        {
            get { return m_Durability; }
            set { m_Durability = value; InvalidateProperties(); }
        }

        [Constructable]
        public IronBeetleBody()
            : base(0x210F)
        {
            Weight = 150;
            Stackable = false;
            Name = "Corps d'Iron Beetle";
            Durability = 100;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (SummonScalar == 0)
            {
                base.OnDoubleClick(from);
            }
            else
            {
                if (!from.CheckSkill(SkillName.Mysticism, 0, 35))
                {
                    from.SendMessage("Le mécanisme, sous vos mains non initiées en magie, s'abime");
                    Durability--;
                }
                else
                {
                    from.SendMessage("La bestiole prend vie");

                    IronBeetle beetle = new IronBeetle();
                    beetle.SummonScalar = this.SummonScalar;
                    beetle.Controlled = true;
                    beetle.ControlMaster = from;
                    beetle.Crafted = true;
                    beetle.MoveToWorld(from.Location, from.Map);
                    this.Delete();
                }
                if (Durability == 0)
                {
                    from.SendMessage("Vous détruisez la carapace");
                    Delete();
                }
            }
            base.OnDoubleClick(from);
        }

        public IronBeetleBody(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);

            writer.Write((double)m_SummonScalar);
            writer.Write((int)m_Durability);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_SummonScalar = reader.ReadDouble();
            m_Durability = reader.ReadInt();
        }
    }
}

