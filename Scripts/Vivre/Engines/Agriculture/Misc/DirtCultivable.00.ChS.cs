using System;

namespace Server.Items
{
    public class DirtCultivable : BaseFloor
    {
        public static int[] DirtTiles = new int[]
		{
            0x31F4, 0x31F5,
            0x31F6, 0x31F7,
            0x31F8, 0x31F9,
            0x31FA, 0x31FB
		};

        [Constructable]
        public DirtCultivable()
            : base(0x177D, 1)
        {
            int i = Utility.Random(0, 5);
            this.ItemID = DirtTiles[i];
            this.Name = "Terre Labourable";
        }

        public DirtCultivable(Serial serial)
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