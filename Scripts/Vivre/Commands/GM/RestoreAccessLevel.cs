using System;
using Server;

namespace Server.Commands
{
    public class RestoreAccessLevel
    {
        public static void Initialize()
        {
            CommandSystem.Register("ra", AccessLevel.Player, new CommandEventHandler(RA_OnCommand));
            CommandSystem.Register("pl", AccessLevel.Counselor, new CommandEventHandler(PL_OnCommand));
        }

        private static void RA_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile.Account != null && e.Mobile.Account.AccessLevel > AccessLevel.Player)
            {
                e.Mobile.AccessLevel = e.Mobile.Account.AccessLevel;
                e.Mobile.SendMessage("Vous avez récupéré vos accès.");
            }
            else
            {
                e.Mobile.SendMessage("This command is deprecated.");
            }
        }

        private static void PL_OnCommand(CommandEventArgs e)
        {
            e.Mobile.AccessLevel = AccessLevel.Player;
            e.Mobile.SendMessage("Vous êtes désormais un joueur.");
        }
    }
}
