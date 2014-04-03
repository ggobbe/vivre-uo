using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class BloodVial : BaseReagent, ICommodity
    {
        string Description
        {
            get
            {
                return String.Format("{0} vial of blood", Amount);
            }
        }

        int ICommodity.DescriptionNumber { get { return LabelNumber; } }
        bool ICommodity.IsDeedable { get { return true; } }

        [Constructable]
        public BloodVial()
            : this(1)
        {
        }

        [Constructable]
        public BloodVial(int amount)
            : base(0xF7D, amount)
        {
        }

        public BloodVial(Serial serial)
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