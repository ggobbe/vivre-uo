using System;

namespace Server.Items
{
    public class BasketFrame : Item
    {

        [Constructable]
        public BasketFrame()
            : base(0x407B)
        {
            Movable = true;
            Stackable = false;
            Name = "Corps de panier";
        }

        public BasketFrame(Serial serial)
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