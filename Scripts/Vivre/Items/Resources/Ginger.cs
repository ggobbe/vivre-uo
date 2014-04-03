using System;

namespace Server.Items
{
    [FlipableAttribute(0x2BE3, 0x2BE4)]
    public class GingerRoot : Item
    {

        [Constructable]
        public GingerRoot()
            : base(0x2BE3)
        {
            Movable = true;
            Stackable = false ;
            Name = "Gingembre";
        }

        public GingerRoot(Serial serial)
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