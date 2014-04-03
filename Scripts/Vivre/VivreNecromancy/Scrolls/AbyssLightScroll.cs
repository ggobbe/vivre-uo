/***************************************************************************
 *                          AnimateDeadScroll.cs
 *                          ---------------------
 *   begin                : August 26, 2010
 *   copyright            : (C) Scriptiz
 *   email                : maeliguul@hotmail.com
 *   version              : 2010-09-19
 *
 ***************************************************************************/
using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class AbyssLightScroll : SpellScroll
    {
        [Constructable]
        public AbyssLightScroll()
            : this(1)
        {
        }

        [Constructable]
        public AbyssLightScroll(int amount)
            : base(703, 0x2260, amount)
        {
            Name = "Parchemin lumière des abysses";
        }

        public AbyssLightScroll(Serial serial)
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