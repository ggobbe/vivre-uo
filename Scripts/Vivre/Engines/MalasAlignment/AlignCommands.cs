using System;
using Server;
using Server.Misc;

namespace Server.Commands
{
    public class AlignCommands
    {
        public static void Initialize()
        {
            CommandSystem.Register("AlignStats", AccessLevel.GameMaster, new CommandEventHandler(Align_OnCommand));
            CommandSystem.Register("BBMABOB", AccessLevel.Administrator, new CommandEventHandler(BBMABOB_OnCommand));
            // Bob Bring Me A Bottle Of Beer <3
            CommandSystem.Register("AlignReset", AccessLevel.Administrator, new CommandEventHandler(AlignReset_OnCommand));
            
        }

        private static void Align_OnCommand(CommandEventArgs e)
        {
            if (Alignments.Instance.MobilesKills.Keys.Count > 0)
            {
                e.Mobile.SendMessage("Mobiles kills : ");
                foreach (Alignment a in Alignments.Instance.MobilesKills.Keys)
                {
                    e.Mobile.SendMessage(a.ToString() + " : " + Alignments.Instance.MobilesKills[a]);
                }
            }

            if (Alignments.Instance.PlayersKills.Keys.Count > 0)
            {
                e.Mobile.SendMessage("Players kills : ");
                foreach (Alignment a in Alignments.Instance.PlayersKills.Keys)
                {
                    e.Mobile.SendMessage(a.ToString() + " : " + Alignments.Instance.PlayersKills[a]);
                }
            }
        }

        private static void BBMABOB_OnCommand(CommandEventArgs e)
        {
            Alignments.Instance.MoveToWorld(e.Mobile.Location, e.Mobile.Map);
        }

        private static void AlignReset_OnCommand(CommandEventArgs e)
        {
            Alignments.Reset();
        }
    }
}
