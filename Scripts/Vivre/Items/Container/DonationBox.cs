using System;
using System.Collections.Generic;
using Server;
using Server.Multis;
using Server.Network;

namespace Server.Items
{
    [DynamicFliping]
    [Flipable(0x9A8, 0xE80)]
    public class DonationBox : LockableContainer
    {
        [Constructable]
        public DonationBox()
            : base(0x9A8)
        {
            Name = "Boite à dons (Or seulement)";
        }

        public DonationBox(Serial serial)  : base(serial)
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
