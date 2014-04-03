using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class Blackmoor : BaseReagent, ICommodity
    {
        string Description
        {
            get
            {
                return String.Format("{0} blackmoor", Amount);
            }
        }

        int ICommodity.DescriptionNumber { get { return LabelNumber; } }
        bool ICommodity.IsDeedable { get { return true; } }

        [Constructable]
        public Blackmoor()
            : this(1)
        {
        }

        [Constructable]
        public Blackmoor(int amount)
            : base(0xF79, amount)
        {
        }

        public Blackmoor(Serial serial)
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