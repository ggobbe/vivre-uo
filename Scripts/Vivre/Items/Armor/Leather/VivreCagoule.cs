//Myron - Cagoule pour les criminels
using System;
using Server.Items;

namespace Server.Items
{
    public class VivreCagoule : BaseArmor
    {
        private string m_Title;
        private CagouleTimer timer;
        public override int BasePhysicalResistance { get { return 2; } }
        public override int BaseFireResistance { get { return 3; } }
        public override int BaseColdResistance { get { return 3; } }
        public override int BasePoisonResistance { get { return 3; } }
        public override int BaseEnergyResistance { get { return 4; } }

        public override int InitMinHits { get { return 25; } }
        public override int InitMaxHits { get { return 45; } }

        public override int AosStrReq { get { return 10; } }
        public override int OldStrReq { get { return 10; } }

        public override int ArmorBase { get { return 3; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Leather; } }
        public override CraftResource DefaultResource { get { return CraftResource.RegularLeather; } }

        public override ArmorMeditationAllowance DefMedAllowance { get { return ArmorMeditationAllowance.All; } }

        [Constructable]
        public VivreCagoule()
            : base(0x278E)
        {
            Name = "Cagoule";
            Weight = 2.0;
        }

        private class CagouleTimer : Timer
        {
            private Mobile m;

            public CagouleTimer(Mobile from)
                : base(TimeSpan.FromSeconds(100), TimeSpan.FromSeconds(100))
            {
                m = from;
                Priority = TimerPriority.FiveSeconds;
            }

            protected override void OnTick()
            {
                if ((m.FindItemOnLayer(Layer.Helm) != null) && (m.FindItemOnLayer(Layer.Helm).GetType() == typeof(VivreCagoule)))
                {
                    m.Criminal = false;
                    m.Criminal = true;
                }
                else
                    this.Stop();
            }
        }

        public override bool OnEquip(Mobile from)
        {
            if (from.Female)
                from.NameMod = "Femme en cagoule";
            else
                from.NameMod = "Homme en cagoule";

            m_Title = from.Title;
            from.Title = null;
            from.DisplayGuildTitle = false;
            from.Criminal = false;
            from.Criminal = true;
            from.SendMessage("Vous portez une cagoule et êtes donc consideré comme un criminel.");
            timer = new CagouleTimer(from);
            timer.Start();
            return base.OnEquip(from);
        }

        public override void OnRemoved(object parent)
        {
            if (parent is Mobile)
            {
                Mobile from = (Mobile)parent;
                from.Title = m_Title;
                from.NameMod = null;
            }

            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }

            base.OnRemoved(parent);
        }

        public VivreCagoule(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write((string)m_Title);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Title = reader.ReadString();
        }
    }
}