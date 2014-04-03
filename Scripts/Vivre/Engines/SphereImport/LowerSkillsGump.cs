/***************************************************************************
 *                              LowerSkillsGump.cs
 *                         -----------------------------
 *   begin                : May 25, 2011
 *   copyright            : (C) Scriptiz
 *   email                : maeliguul@hotmail.com
 *   version              : 2011-06-25
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
    public class LowerSkillGump : Gump
    {
        public static void Initialize()
        {
            EventSink.Login += new LoginEventHandler(EventSink_Login);
        }

        public static void EventSink_Login(LoginEventArgs e)
        {
            if (e.Mobile.SkillsTotal > e.Mobile.SkillsCap && e.Mobile.AccessLevel == AccessLevel.Player)
            {
                e.Mobile.SendGump(new LowerSkillGump(e.Mobile));
            }
        }

        public LowerSkillGump(Mobile from)
            : base(25, 50)
        {
            this.Closable = false;
            this.Dragable = false;

            AddPage(0);

            AddBackground(0, 0, 520, 440, 0x13BE);

            AddImageTiled(10, 10, 500, 20, 0xA40);
            AddImageTiled(10, 40, 500, 360, 0xA40);
            AddImageTiled(10, 410, 500, 20, 0xA40);

            AddAlphaRegion(10, 10, 500, 420);

            AddHtml(10, 12, 500, 20, "<BASEFONT COLOR=#FFFFFF>Quelle skill voulez-vous descendre pour respecter votre skills cap?", false, false);

            AddHtml(10, 412, 500, 20, "<BASEFONT COLOR=#FFFFFF>Skills total : " + (from.SkillsTotal / 10) + " / " + (from.SkillsCap / 10), false, false);

            for (int i = 0, n = 0; i < from.Skills.Length; i++)
            {
                Skill skill = from.Skills[i];

                if (skill.Base > 0.0)
                {
                    int p = n % 30;

                    if (p == 0)
                    {
                        int page = n / 30;

                        if (page > 0)
                        {
                            AddButton(260, 380, 0xFA5, 0xFA6, 0, GumpButtonType.Page, page + 1);
                            AddHtmlLocalized(305, 382, 200, 20, 1011066, 0x7FFF, false, false); // Next page
                        }

                        AddPage(page + 1);

                        if (page > 0)
                        {
                            AddButton(10, 380, 0xFAE, 0xFAF, 0, GumpButtonType.Page, page);
                            AddHtmlLocalized(55, 382, 200, 20, 1011067, 0x7FFF, false, false); // Previous page
                        }
                    }

                    int x = (p % 2 == 0) ? 10 : 260;
                    int y = (p / 2) * 20 + 40;

                    AddButton(x, y, 0xFA5, 0xFA6, i + 1, GumpButtonType.Reply, 0);
                    AddHtmlLocalized(x + 45, y + 2, 200, 20, 1044060 + i, 0x7FFF, false, false);
                    AddHtml(x + 220, y + 2, 25, 20, "<BASEFONT COLOR=#FFFFFF>" + from.Skills[i].Base, false, false);

                    n++;
                }
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            int iSkill = info.ButtonID - 1;
            if (iSkill < 0 || iSkill >= from.Skills.Length)
            {
                from.SendMessage("Petit malin va ! Il te faut descendre tes skills !");
                from.SendGump(new LowerSkillGump(from));
                return;
            }

            Skill skill = from.Skills[iSkill];
            if (skill.Base <= 0.0)
            {
                from.SendGump(new LowerSkillGump(from));
                return;
            }

            skill.Base -= 10;

            if (from.SkillsTotal > from.SkillsCap)
                from.SendGump(new LowerSkillGump(from));
            else
            {
                from.SendMessage("Vos skills sont en ordre. Bon jeu !");
                from.CloseGump(typeof(LowerSkillGump));
            }
        }
    }
}