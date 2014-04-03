﻿using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class EyeOfNewt : BaseReagent, ICommodity
    {
        string Description
        {
            get
            {
                return String.Format("{0} eye of newt", Amount);
            }
        }

        int ICommodity.DescriptionNumber { get { return LabelNumber; } }
        bool ICommodity.IsDeedable { get { return true; } }

        [Constructable]
        public EyeOfNewt()
            : this(1)
        {
        }

        [Constructable]
        public EyeOfNewt(int amount)
            : base(0x0F87, amount)
        {
        }

        public EyeOfNewt(Serial serial)
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