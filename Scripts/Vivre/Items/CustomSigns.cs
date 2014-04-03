using Server;
using Server.Items;
using Server.Multis;
using Server.Network;
using System;
using Server.Prompts;

namespace Server.Items
{
    [FlipableAttribute(0xBA5, 0xBA6)]
    public class TailorSign : BaseSign
    {
        [Constructable]
        public TailorSign()
            : base(0xBA5)
        {
            Movable = true;
        }

        public TailorSign(Serial serial)
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

    [FlipableAttribute(0xBA7, 0xBA8)]
    public class TinkerSign : BaseSign
    {
        [Constructable]
        public TinkerSign()
            : base(0xBA7)
        {
            Movable = true;
        }

        public TinkerSign(Serial serial)
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

    [FlipableAttribute(0xBAF, 0xBB0)]
    public class WoodworkerSign : BaseSign
    {
        [Constructable]
        public WoodworkerSign()
            : base(0xBAF)
        {
            Movable = true;
        }

        public WoodworkerSign(Serial serial)
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

    [FlipableAttribute(0xBC7, 0xBC8)]
    public class BlacksmithSign : BaseSign
    {
        [Constructable]
        public BlacksmithSign()
            : base(0xBC7)
        {
            Movable = true;
        }

        public BlacksmithSign(Serial serial)
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

    [FlipableAttribute(0xBD1, 0xBD2)]
    public class GoldSign : BaseSign
    {
        [Constructable]
        public GoldSign()
            : base(0xBD1)
        {
            Movable = true;
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("Que souhaitez-vous graver sur ce panneau?");
            from.Prompt = new RenamePrompt(this);
        }

        public GoldSign(Serial serial)
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

    [FlipableAttribute(0xBCF, 0xBD0)]
    public class WoodenSign : BaseSign
    {
        [Constructable]
        public WoodenSign()
            : base(0xBCF)
        {
            Movable = true;
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("Que souhaitez-vous graver sur ce panneau?");
            from.Prompt = new RenamePrompt(this);
        }

        public WoodenSign(Serial serial)
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