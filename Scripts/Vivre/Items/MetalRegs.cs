using System;

namespace Server.Items
{
    public class TitanTooth : Item
    {

        [Constructable]
        public TitanTooth()
            : base(0x5747)
        {
            Name = "Dent de Titan";
            Movable = true;
            Stackable = true;
        }

        public TitanTooth(Serial serial)
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
            if (!Stackable)
                Stackable = true;
        }
    }

    public class TitanToothPowder : Item
    {

        [Constructable]
        public TitanToothPowder()
            : base(0x573D)
        {
            Name = "Poudre de Dent de Titan";
            Movable = true;
            Stackable = true;
        }

        public TitanToothPowder(Serial serial)
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
            if (!Stackable)
                Stackable = true;
        }
    }
}