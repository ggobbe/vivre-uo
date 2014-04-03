/***************************************************************************
 *                               WebCommands.cs
 *                            -------------------
 *   begin                : August 14, 2010
 *   copyright            : (C) Scriptiz
 *   email                : maeliguul@hotmail.com
 *   version              : 2011-08-14
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
    public class WebCommands
    {
        // Les urls nécessaires :)
        public static string SiteUrl = "http://www.vivre-uo.fr";
        public static string ForumUrl = "http://www.vivre-uo.fr/forum/";
        public static string VoteUrl = "http://www.rpg-paradize.com/?page=vote&vote=23892";

        public static void Initialize()
        {
            CommandSystem.Register("site", AccessLevel.Player, new CommandEventHandler(Site_OnCommand));
            CommandSystem.Register("forum", AccessLevel.Player, new CommandEventHandler(Forum_OnCommand));
            CommandSystem.Register("vote", AccessLevel.Player, new CommandEventHandler(Vote_OnCommand));
        }

        [Usage("site")]
        [Description("Ouvre une page web vers le site de Vivre.")]
        public static void Site_OnCommand(CommandEventArgs e)
        {
            e.Mobile.LaunchBrowser(SiteUrl);
        }

        [Usage("forum")]
        [Description("Ouvre une page web vers le forum de Vivre.")]
        public static void Forum_OnCommand(CommandEventArgs e)
        {
            e.Mobile.LaunchBrowser(ForumUrl);
        }

        [Usage("vote")]
        [Description("Ouvre une page web vers la page vous permettant de voter pour Vivre.")]
        public static void Vote_OnCommand(CommandEventArgs e)
        {
            e.Mobile.LaunchBrowser(VoteUrl);
        }
    }
}