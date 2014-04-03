/***************************************************************************
 *                          MinorLifeLeechScroll.cs
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
    public class MinorLifeLeechScroll : SpellScroll
    {
        [Constructable]
        public MinorLifeLeechScroll()
            : this(1)
        {
        }

        [Constructable]
        public MinorLifeLeechScroll(int amount)
            : base(705, 0x2260, amount)
        {
            Name = "Parchemin drain de vie mineur";
        }

        public MinorLifeLeechScroll(Serial serial)
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