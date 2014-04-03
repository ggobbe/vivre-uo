/***************************************************************************
 *                              LowerStatsGump.cs
 *                         -----------------------------
 *   begin                : July 5, 2011
 *   copyright            : (C) Scriptiz
 *   email                : maeliguul@hotmail.com
 *   version              : 2011-07-06
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
using Server;
using Server.Gumps;
using Server.Network;
using Server.Accounting;
using Server.Engines.VeteranRewards;
using Server.Multis;
using Server.Mobiles;

namespace Server.Gumps
{
    public class LowerStatsGump : Gump
    {
        public static void Initialize()
        {
            EventSink.Login += new LoginEventHandler(EventSink_Login);
        }

        public static void EventSink_Login(LoginEventArgs e)
        {
            if (e.Mobile.RawStatTotal > e.Mobile.StatCap && e.Mobile.AccessLevel == AccessLevel.Player)
            {
                e.Mobile.SendGump(new LowerStatsGump(e.Mobile));
            }
        }

        public LowerStatsGump(Mobile from)
            : base(550, 50)
        {
            this.Closable = false;
            this.Dragable = false;

            AddPage(0);

            AddBackground(0, 0, 260, 240, 0x13BE);

            AddImageTiled(10, 10, 240, 40, 0xA40);  // top region
            AddImageTiled(10, 60, 240, 140, 0xA40);
            AddImageTiled(10, 210, 240, 20, 0xA40);

            AddAlphaRegion(10, 10, 240, 220);

            AddHtml(10, 12, 500, 20, "<BASEFONT COLOR=#FFFFFF>Quelle stat voulez-vous descendre", false, false);
            AddHtml(10, 32, 500, 20, "<BASEFONT COLOR=#FFFFFF>pour respecter votre stats cap?", false, false);

            AddHtml(10, 212, 500, 20, "<BASEFONT COLOR=#FFFFFF>Stats total : " + from.RawStatTotal + " / " + from.StatCap, false, false);

            int x = 10, y = 60, i = 1;
            AddButton(x, y, 0xFA5, 0xFA6, i, GumpButtonType.Reply, 0);
            AddHtml(x + 45, y + 2, 200, 20, "<BASEFONT COLOR=#FFFFFF>Strength", false, false);
            AddHtml(x + 220, y + 2, 25, 20, "<BASEFONT COLOR=#FFFFFF>" + from.Str, false, false);

            y += 20; i++;
            AddButton(x, y, 0xFA5, 0xFA6, i, GumpButtonType.Reply, 0);
            AddHtml(x + 45, y + 2, 200, 20, "<BASEFONT COLOR=#FFFFFF>Dexterity", false, false);
            AddHtml(x + 220, y + 2, 25, 20, "<BASEFONT COLOR=#FFFFFF>" + from.Dex, false, false);

            y += 20; i++;
            AddButton(x, y, 0xFA5, 0xFA6, i, GumpButtonType.Reply, 0);
            AddHtml(x + 45, y + 2, 200, 20, "<BASEFONT COLOR=#FFFFFF>Intelligence", false, false);
            AddHtml(x + 220, y + 2, 25, 20, "<BASEFONT COLOR=#FFFFFF>" + from.Int, false, false);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            int iStat = info.ButtonID;
            if (iStat < 1 || iStat > 3)
            {
                from.SendMessage("Petit malin va ! Il te faut descendre tes skills !");
                from.SendGump(new LowerSkillGump(from));
                return;
            }

            switch (iStat)
            {
                case 1:
                    from.Str -= 5;
                    break;
                case 2 :
                    from.Dex -= 5;
                    break;
                case 3:
                    from.Int -= 5;
                    break;
            }

            if (from.RawStatTotal > from.StatCap)
                from.SendGump(new LowerStatsGump(from));
            else
            {
                from.SendMessage("Vos stats sont en ordre. Bon jeu !");
                from.CloseGump(typeof(LowerStatsGump));
            }
        }
    }
}
