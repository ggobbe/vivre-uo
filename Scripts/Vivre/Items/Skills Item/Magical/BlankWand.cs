using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
    public class BlankWand : BaseBashing
    {
        public override WeaponAbility PrimaryAbility { get { return WeaponAbility.Dismount; } }
        public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Disarm; } }

        public override int AosStrengthReq { get { return 5; } }
        public override int AosMinDamage { get { return 9; } }
        public override int AosMaxDamage { get { return 11; } }
        public override int AosSpeed { get { return 40; } }

        public override int OldStrengthReq { get { return 0; } }
        public override int OldMinDamage { get { return 2; } }
        public override int OldMaxDamage { get { return 6; } }
        public override int OldSpeed { get { return 35; } }

        public override int InitMinHits { get { return 31; } }
        public override int InitMaxHits { get { return 110; } }

        [Constructable]
        public BlankWand() : base(Utility.RandomList( 0xDF2, 0xDF3, 0xDF4, 0xDF5 ))
        {
            Name = "Baguette de bois";
            Weight = 1.0;
        }

        public BlankWand(Serial serial)
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