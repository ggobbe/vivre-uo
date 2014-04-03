using System;
using System.Collections.Generic;
using System.Text;
using Server.Commands;

namespace Server.MDDS
{
    class MDDSCommands
    {
        public static void Initialize()
        {
            CommandSystem.Register("DeleteMDDS", AccessLevel.Administrator, new CommandEventHandler(DeleteMDDS_OnCommand));
            CommandSystem.Register("WhereIsMDDS", AccessLevel.GameMaster, new CommandEventHandler(WhereIsMDDS_OnCommand));
        }

        [Usage("DeleteMDDS")]
        [Description("Supprime le MDDSStarter")]
        public static void DeleteMDDS_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            List<Item> toDel = new List<Item>();
            foreach (Item i in World.Items.Values)
            {
                if (i is MDDSStarter) toDel.Add(i);
            }

            for (int i = 0; i < toDel.Count; i++)
            {
                from.SendMessage("MDDSStarter has been deleted on {0} at [{1},{2},{3}]", toDel[i].Map, toDel[i].X, toDel[i].Y, toDel[i].Z);
                toDel[i].Delete();
            }
        }

        [Usage("WhereIsMDDS")]
        [Description("Indique la position du MDDS")]
        public static void WhereIsMDDS_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            foreach (Item i in World.Items.Values)
            {
                if (i is MDDSStarter)
                    from.SendMessage("MDDSStarter on {0} at [{1},{2},{3}]", i.Map, i.X, i.Y, i.Z);
            }
        } 
    }
}
