    using System;

    namespace Server.Items
    {

        public class RobeACapuche : BaseOuterTorso
        {
            [Constructable]
            public RobeACapuche()
                : this(0)
            {
            }

            [Constructable]
            public RobeACapuche(int hue)
                : base(0x2684, hue)
            {
                Name = "Robe à capuche";
                Weight = 1.0;
            }

            public RobeACapuche(Serial serial)
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

                if (ItemID == 0x204E)
                    ItemID = 0x2684;
            }
        }
    }