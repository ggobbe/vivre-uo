/***************************************************************************
 *                               BandCommands.cs
 *                            -------------------
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
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Commands
{
    public class Band
    {
        public static void Initialize()
        {
            CommandSystem.Register("band", AccessLevel.Player, new CommandEventHandler(Band_OnCommand));
        }

        [Usage("band")]
        [Description("Utilise un bandage pour peux que vous en possédiez.")]
        public static void Band_OnCommand(CommandEventArgs e)
        {
            Bandage m_Bandage = (Bandage)e.Mobile.Backpack.FindItemByType(typeof(Bandage));

            if (m_Bandage == null)
            {
                e.Mobile.SendMessage("Vous ne possédez pas de bandages.");
                return;
            }

            m_Bandage.OnDoubleClick(e.Mobile);
        }
    } 

    public class BandSelf
    {
        public static void Initialize()
        {
            CommandSystem.Register("bandself", AccessLevel.Player, new CommandEventHandler(BandSelf_OnCommand));
            CommandSystem.Register("bs", AccessLevel.Player, new CommandEventHandler(BandSelf_OnCommand));
        }

        public static void BandSelf_OnCommand(CommandEventArgs e)
        {
            Mobile pm = e.Mobile;
            Item band = pm.Backpack.FindItemByType(typeof(Bandage));

            if (band != null)
            {
                Bandage.BandSelfCommandCall(pm, band);
                if (band.Amount <= 5)
                    pm.SendMessage("Attention il ne vous reste que {0} bandages !", band.Amount);
            }
            else
            {
                pm.SendMessage("Vous n'avez plus de bandages !");
            }
        }
    }
}