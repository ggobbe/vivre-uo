using System;
using Server;

namespace Server.Items
{
    [Flipable]
    public class GuardCloak : Item
    {   
        [Constructable]
        public GuardCloak()  : base(0x1515)
        {
            Layer = Layer.Cloak;
            Weight = 5.0;
            LootType = LootType.Blessed;    // Scriptiz : on préfère éviter que ça se promène partout
        }

        // Scriptiz : cape non échangeable
        public override bool Nontransferable
        {
            get { return true; }
        }

        public GuardCloak(Serial serial) : base(serial)
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