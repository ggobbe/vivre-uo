using System;

namespace Server.Items
{
    public class BullFrogLard : Item
    {

        [Constructable]
        public BullFrogLard()
            : base(0x3183)
        {
            Name = "Gras de crapaud";
            Hue = 50;
            Movable = true; 
            Stackable = true;
        }

        public BullFrogLard(Serial serial)
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