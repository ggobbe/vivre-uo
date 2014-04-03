/***************************************************************************
 *                          AnimateDeadScroll.cs
 *                          ---------------------
 *   begin                : August 26, 2010
 *   copyright            : (C) Scriptiz
 *   email                : maeliguul@hotmail.com
 *   version              : 2010-08-29
 *
 ***************************************************************************/
using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class AnimateCorpseScroll : SpellScroll
    {
        [Constructable]
        public AnimateCorpseScroll()
            : this(1)
        {
        }

        [Constructable]
        public AnimateCorpseScroll(int amount)
            : base(701, 0x2260, amount)
        {
            Name = "Parchemin libérer la mort";
        }

        public AnimateCorpseScroll(Serial serial)
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