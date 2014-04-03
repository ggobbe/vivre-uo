using System;

namespace Server.Items
{
	public class ElegantLowTable : Item
	{
		[Constructable]
		public ElegantLowTable() : base(0x2819)
		{
			Weight = 1.0;
		}

		public ElegantLowTable(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

		}
	}

	public class PlainLowTable : Item
	{
		[Constructable]
		public PlainLowTable() : base(0x281A)
		{
			Weight = 1.0;
		}

		public PlainLowTable(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

		}
	}

	[Flipable(0xB90,0xB7D)]
	public class LargeTable : Item
	{
		[Constructable]
		public LargeTable() : base(0xB90)
		{
			Weight = 1.0;
		}

		public LargeTable(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			if ( Weight == 4.0 )
				Weight = 1.0;
		}
	}

	[Flipable(0xB35,0xB34)]
	public class Nightstand : Item
	{
		[Constructable]
		public Nightstand() : base(0xB35)
		{
			Weight = 1.0;
		}

		public Nightstand(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			if ( Weight == 4.0 )
				Weight = 1.0;
		}
	}

	[Flipable(0xB8F,0xB7C)]
	public class YewWoodTable : Item
	{
		[Constructable]
		public YewWoodTable() : base(0xB8F)
		{
			Weight = 1.0;
		}

		public YewWoodTable(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			if ( Weight == 4.0 )
				Weight = 1.0;
		}

    }
        // Vinds : ajout des tables avec runner. existent déja sous forme static ou addon. Je préfere sous forme d'item standard pour les pjs.

        public class TableRunnerPurple : Item
        {
            [Constructable]
            public TableRunnerPurple()
                : base(0x118b)
            {
                Weight = 1.0;
            }

            public TableRunnerPurple(Serial serial)
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

        public class TableRunnerBlue : Item
        {
            [Constructable]
            public TableRunnerBlue()
                : base(0x118c)
            {
                Weight = 1.0;
            }

            public TableRunnerBlue(Serial serial)
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


        public class TableRunnerRed : Item
        {
            [Constructable]
            public TableRunnerRed()
                : base(0x118d)
            {
                Weight = 1.0;
            }

            public TableRunnerRed(Serial serial)
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

        public class TableRunnerOrange : Item
        {
            [Constructable]
            public TableRunnerOrange()
                : base(0x118e)
            {
                Weight = 1.0;
            }

            public TableRunnerOrange(Serial serial)
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

        [Flipable(0x118F, 0x1191)]
        public class TabletteRunner : Item
        {
            [Constructable]
            public TabletteRunner()
                : base(0x118f)
            {
                Name = "petite table avec traverse";
                Weight = 1.0;
            }

            public TabletteRunner(Serial serial)
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

        [Flipable(0x1192, 0x1190)]
        public class ExtTabletteRunner : Item
        {
            [Constructable]
            public ExtTabletteRunner()
                : base(0x1190)
            {
                Name = "Rallonge avec traverse";
                Weight = 1.0;
            }

            public ExtTabletteRunner(Serial serial)
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

        [Flipable(0x1667, 0x166A)]
        public class TableLongRunnerW : Item
        {
            [Constructable]
            public TableLongRunnerW()
                : base(0x1667)
            {
                Name = "Table avec traverse";
                Weight = 1.0;
            }

            public TableLongRunnerW(Serial serial)
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

        [Flipable(0x1668, 0x166b)]
        public class TableLongRunnerC : Item
        {
            [Constructable]
            public TableLongRunnerC()
                : base(0x1668)
            {
                Name = "Table avec traverse";
                Weight = 1.0;
            }

            public TableLongRunnerC(Serial serial)
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
        [Flipable(0x1669, 0x166c)]
        public class TableLongRunnerE : Item
        {
            [Constructable]
            public TableLongRunnerE()
                : base(0x1669)
            {
                Name = "Table avec traverse";
                Weight = 1.0;
            }

            public TableLongRunnerE(Serial serial)
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