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
using System.Text;
using Server.Items;
using Server.Mobiles;

namespace Server.Misc
{
    public class ScrollDeleter
    {
        public static string Message = "Vous remarquez que des parchemins nécros ont disparus dans votre sac.";

        public static int DeleteNecroScrolls(Mobile m)
        {
            if (m == null) return 0;

            // On traite les bosses pour ne pas leur retiré leurs sorts
            if (m is NecroBoss) return 0;

            Container c = m.Backpack;
            if (c == null) return 0;

            int deleted = 0, count = 0;

            List<SpellScroll> otherScrolls = new List<SpellScroll>();

            SpellScroll scroll = null;
            while ((scroll = c.FindItemByType(typeof(SpellScroll)) as SpellScroll) != null)
            {
                count++;
                if (scroll.SpellID >= 100 && scroll.SpellID <= 116)
                {
                    scroll.Delete();
                    deleted++;
                }
                else
                {
                    Container bank = m.BankBox;
                    bank.DropItem(scroll);
                    otherScrolls.Add(scroll);
                }

                if (count > 250)
                {
                    Console.WriteLine("!!! Exception Scroll Deleter !!!");
                    break;
                }
            }

            foreach (SpellScroll ss in otherScrolls)
            {
                if(ss != null)
                    m.Backpack.DropItem(ss);
            }

            return deleted;
        }
    }
}
