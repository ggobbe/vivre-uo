using System;
using Server.Items;
using Server.Mobiles;
using Server;

namespace Server.Commands
{
    public class InfoCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("Karma", AccessLevel.Player, new CommandEventHandler(Karma_OnCommand));
            CommandSystem.Register("Fame", AccessLevel.Player, new CommandEventHandler(Fame_OnCommand));
        }

        private static void Karma_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (from != null)
            {
                from.SendMessage("KARMA " + from.Karma.ToString());
            }
        }

        private static void Fame_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (from != null)
            {
                from.SendMessage("FAME " + from.Fame.ToString());
            }
        }
    }
}
