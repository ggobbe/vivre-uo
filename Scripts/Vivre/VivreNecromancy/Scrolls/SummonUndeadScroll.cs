/***************************************************************************
 *                          SummonUndeadScroll.cs
 *                          ---------------------
 *   begin                : July 25, 2010
 *   copyright            : (C) Scriptiz
 *   email                : maeliguul@hotmail.com
 *   version              : 2010-07-25
 *
 ***************************************************************************/
using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class SummonUndeadScroll : SpellScroll
    {
        [Constructable]
        public SummonUndeadScroll()
            : this(1)
        {
        }

        [Constructable]
        public SummonUndeadScroll(int amount)
            : base(700, 0x2260, amount)
        {
            Name = "Parchemin d'invocation des mort";
        }

        public SummonUndeadScroll(Serial serial)
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