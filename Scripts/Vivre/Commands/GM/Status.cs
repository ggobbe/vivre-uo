using System;
using Server;
using Server.Mobiles;

namespace Server.Commands
{
    public class StatusCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("status", AccessLevel.Counselor, new CommandEventHandler(Status_OnCommand));
        }

        private static void Status_OnCommand(CommandEventArgs e)
        {
            PlayerMobile pm = e.Mobile as PlayerMobile;

            if (pm != null)
            {
                pm.ShowInStatus = !pm.ShowInStatus;

                if (pm.ShowInStatus)
                    pm.SendMessage("Vous êtes désormais visible dans le statut web.");
                else
                    pm.SendMessage("Vous n'êtes plus visible dans le statut web.");
            }
        }
    }
}
