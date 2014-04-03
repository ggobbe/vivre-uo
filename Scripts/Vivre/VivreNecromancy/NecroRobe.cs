using Server;

namespace Server.Items
{
    class NecroRobe : Robe
    {
        [Constructable]
        public NecroRobe()
            : this(1109)
        {
        }

        public NecroRobe(int hue)
            : base(hue)
        {
        }

        public NecroRobe(Serial serial)
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
