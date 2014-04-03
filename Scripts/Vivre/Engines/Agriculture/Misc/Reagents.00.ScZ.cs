using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class Katyl : BaseReagent
    {
        [Constructable]
        public Katyl()
            : this(1)
        {
            Name = "Katyl";
            Hue = 0xb66;
        }

        [Constructable]
        public Katyl(int amount)
            : base(0xF7F, amount)
        {
            Hue = 0xb66;
            Name = "Katyl";
        }

        public Katyl(Serial serial)
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


    public class Onax : BaseReagent
{
        [Constructable]
        public Onax()
            : this(1)
        {
            Hue = 0x27;
            Name = "Onax";
        }

        [Constructable]
        public Onax(int amount)
            : base(0xF8F, amount)
        {
            Hue = 0x27;
            Name = "Onax";
        }

        public Onax(Serial serial)
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