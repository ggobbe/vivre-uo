using System;
using Server;

namespace Server.Commands
{
    public class CoreCmd
    {
        public static void Initialize()
        {
            CommandSystem.Register("Core", AccessLevel.GameMaster, new CommandEventHandler(Core_OnCommand));
        }

        private static void Core_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Core.AOS = " + Core.AOS);
            e.Mobile.SendMessage("Core.ML = " + Core.ML);
            e.Mobile.SendMessage("Core.SE = " + Core.SE);
        }
    }
}
