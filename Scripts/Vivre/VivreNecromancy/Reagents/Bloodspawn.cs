﻿using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class Bloodspawn : BaseReagent, ICommodity
    {
        string Description
        {
            get
            {
                return String.Format("{0} bloodspawn", Amount);
            }
        }

        int ICommodity.DescriptionNumber { get { return LabelNumber; } }
        bool ICommodity.IsDeedable { get { return true; } }

        [Constructable]
        public Bloodspawn()
            : this(1)
        {
        }

        [Constructable]
        public Bloodspawn(int amount)
            : base(0xF7C, amount)
        {
        }

        public Bloodspawn(Serial serial)
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