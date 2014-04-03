/***************************************************************************
 *                              NecroExiter.cs
 *                         -----------------------------
 *   begin                : August 10, 2011
 *   copyright            : (C) Scriptiz
 *   email                : maeliguul@hotmail.com
 *   version              : 2011-08-10
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/
using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Misc;

namespace Server.Items
{
    public class NecroExiter : Teleporter
    {
        private void EndMessageLock(object state)
        {
            ((Mobile)state).EndAction(this);
        }

        public override bool OnMoveOver(Mobile m)
        {
            int deleted = ScrollDeleter.DeleteNecroScrolls(m);
            if (deleted > 0) m.SendMessage(ScrollDeleter.Message);

            return base.OnMoveOver(m);
        }

        [Constructable]
        public NecroExiter()
        {
        }

        public NecroExiter(Serial serial)
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
