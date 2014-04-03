using System;

namespace Server.Items
{
    public class BoatStatic : Static
    {
        public BoatStatic(int itemID)
            : base(itemID)
        {
        }

        public BoatStatic(Serial serial)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }
    }
}
