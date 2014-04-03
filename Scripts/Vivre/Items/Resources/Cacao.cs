using System;

namespace Server.Items
{
    public class CacaoSeed : Item
    {

        [Constructable]
        public CacaoSeed()             : base(0xF19)
        {
            Movable = true;
            Stackable = true;
            Name = "Fève de cacao";
            Hue = 57;
        }

        public CacaoSeed(Serial serial)
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

    public class CacaoPowder : Item
    {

        [Constructable]
        public CacaoPowder() : base(0x573D)
        {
            Movable = true;
            Stackable = true;
            Name = "Poudre de cacao";
            Hue = 57;
        }

        public CacaoPowder(Serial serial)
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