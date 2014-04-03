using System;
using Server.Network;
using Server.Spells;

namespace Server.Items
{
    public class NecroSpellbook : Spellbook
    {
        public override SpellbookType SpellbookType { get { return SpellbookType.Necromancer; } }   // to change
        public override int BookOffset { get { return 700; } }
        public override int BookCount { get { return 6; } }

        [Constructable]
        public NecroSpellbook()
            : this((ulong)0)
        {
        }

        [Constructable]
        public NecroSpellbook(ulong content)
            : base(content, 0x2253)
        {
            Layer = (Core.ML ? Layer.OneHanded : Layer.Invalid);
            // Scriptiz : on ne peut pas looter les livres de magie
            Lootable = false;
        }

        public NecroSpellbook(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0 && Core.ML)
                Layer = Layer.OneHanded;
        }
    }
}