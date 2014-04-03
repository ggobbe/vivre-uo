/***************************************************************************
 *                          AnimateDeadScroll.cs
 *                          ---------------------
 *   begin                : August 29, 2010
 *   copyright            : (C) Scriptiz
 *   email                : maeliguul@hotmail.com
 *   version              : 2010-07-29
 *
 ***************************************************************************/
using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class BoneArmorScroll : SpellScroll
    {
        [Constructable]
        public BoneArmorScroll()
            : this(1)
        {
            Name = "Parchemin d'armure d'os";
        }

        [Constructable]
        public BoneArmorScroll(int amount)
            : base(702, 0x2260, amount)
        {
        }

        public BoneArmorScroll(Serial serial)
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