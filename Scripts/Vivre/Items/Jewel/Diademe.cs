using System;

namespace Server.Items
{
    public abstract class BaseDiademe : BaseJewel
    {
        public override int BaseGemTypeNumber { get { return 1044203; } } // star sapphire earrings

        public BaseDiademe(int itemID)
            : base(itemID, Layer.Helm)
        {
        }

        public BaseDiademe(Serial serial)
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
    [FlipableAttribute(0x2B6E, 0x3165)]
    public class Diademe : BaseDiademe
    {
        [Constructable]
        public Diademe()
            : base(0x2B6E)
        {
            Weight = 0.1;
        }

        public Diademe(Serial serial)
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
    [FlipableAttribute(0x2B70, 0x3167)]
    public class DiademeDecore : BaseDiademe
    {
        [Constructable]
        public DiademeDecore()
            : base(0x2B70)
        {
            Weight = 0.1;
        }

        public DiademeDecore(Serial serial)
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