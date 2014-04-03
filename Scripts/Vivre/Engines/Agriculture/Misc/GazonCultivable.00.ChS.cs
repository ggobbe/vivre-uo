using System;

namespace Server.Items
{
    public class GazonCultivable : BaseFloor
    {
        public static int[] GazonTiles = new int[]
		{
			0x177D, 0x177E,
            0x177F, 0x1780
		};

        [Constructable]
        public GazonCultivable()
            : base(0x177D, 1)
        {
            int i = Utility.Random(0, 3);
            this.ItemID = GazonTiles[i];
            this.Name = "Gazon Labourable";
        }

        public GazonCultivable(Serial serial)
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