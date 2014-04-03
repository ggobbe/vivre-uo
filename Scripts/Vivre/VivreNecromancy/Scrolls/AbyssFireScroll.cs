/***************************************************************************
 *                          AbyssFireScroll.cs
 *                          ---------------------
 *   begin                : August 26, 2010
 *   copyright            : (C) Scriptiz
 *   email                : maeliguul@hotmail.com
 *   version              : 2010-09-24
 *
 ***************************************************************************/
using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class AbyssFireScroll : SpellScroll
    {
        [Constructable]
        public AbyssFireScroll()
            : this(1)
        {
        }

        [Constructable]
        public AbyssFireScroll(int amount)
            : base(704, 0x2260, amount)
        {
            Name = "Parchemin feu des abysses";
        }

        public AbyssFireScroll(Serial serial)
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