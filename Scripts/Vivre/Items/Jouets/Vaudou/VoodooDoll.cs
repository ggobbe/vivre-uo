using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public class BaseVoodooDoll  : Item
	{
        private int m_Punishment;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Charges
        {
            get { return m_Punishment; }
            set { m_Punishment = value; InvalidateProperties(); }
        }


        private Mobile m_Possessed;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Possessed
        {
            get
            {
                return m_Possessed;
            }
            set
            {
                m_Possessed = value;
            }
        }

        public BaseVoodooDoll(int itemID) : base(itemID)
		{
            Movable = true;
            Stackable = false;
            Name = "Une poupée";
            m_Possessed = null;
            m_Punishment = 0;
		}

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 2) || !from.InLOS(this))
            {
                from.SendLocalizedMessage(501816);
                return;
            }

            if(m_Possessed != null)
            {
                from.SendMessage("Cette poupée ressemble à quelqu'un... ne la laissez pas entre de mauvaises mains...");
                return;
            }
            from.SendMessage("Cette poupée serait plus terrible avec des cheveux!");
            from.BeginTarget(2, false, TargetFlags.None, new TargetCallback(OnTarget));
        }

        public void OnTarget(Mobile from, object obj)
        {
            if(!(obj is HairStrand))
            {
                from.SendMessage("Ce ne sont pas des cheveux!");
                return;
            }

            HairStrand hair = (HairStrand)obj;

            if(hair.HairOwner == null)
            {
                from.SendMessage("Ces cheveux doivent être des faux!");
                return;
            }

            if(hair.HairOwner.Skills[SkillName.MagicResist].Value > from.Skills[SkillName.SpiritSpeak].Value)
            {
                from.SendMessage("La magie rémanente dans les cheveux est trop forte pour vous.");
                return;
            }

            if(hair.HairOwner.Female && ItemID != 0x2107)
            {
                from.SendMessage("Des cheveux de femme sur un corps d'homme, quelle idée!");
                return;
            }
            if (!hair.HairOwner.Female && ItemID != 0x2106)
            {
                from.SendMessage("Des cheveux d'homme sur un corps de femme, quelle idée!");
                return;
            }

            from.SendMessage("Vous appliquez des cheveux à la poupée");
            m_Possessed = hair.HairOwner;
            m_Punishment = (int) from.Skills[SkillName.SpiritSpeak].Value / 5;
            Name += " ressemblant à " + m_Possessed.Name;
            hair.Delete();
        }

        public BaseVoodooDoll(Serial serial)
            : base(serial)
		{
		}

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
            writer.Write((Mobile)m_Possessed);
            writer.Write(m_Punishment);

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_Possessed = reader.ReadMobile();
            m_Punishment = reader.ReadInt();
        }
    }


    public class VoodooDollFemale : BaseVoodooDoll
    {

        [Constructable]
        public VoodooDollFemale()  : base(0x2107)
        {
        }

        public VoodooDollFemale(Serial serial)
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

    public class VoodooDollMale : BaseVoodooDoll
    {

        [Constructable]
        public VoodooDollMale()
            : base(0x2106)
        {
        }

        public VoodooDollMale(Serial serial)
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