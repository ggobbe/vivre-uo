using System;

namespace Server.Items
{
    public class Hop : Item
    {
       

        [Constructable]
        public Hop()
            : this(1)
        {
        }

        [Constructable]
        public Hop(int amount)
            : base(0x1AA2)
        {
            Stackable = false;
            Weight = 0.1;
            Amount = amount;
            Name = "Houblon";
        }

        public Hop(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}