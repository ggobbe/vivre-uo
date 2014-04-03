using Server.Commands;
using Server.Accounting;
using Server.Network;
using Server.Gumps;
using Server.Targeting;
using Server.Mobiles;
using Server.Items;
using System;
using System.Collections;
using Server;

namespace Server.Scripts.Commands
{
    public class ClearAddiction
    {
        public static void Initialize()
        {
            CommandSystem.Register("ClearAddiction", AccessLevel.GameMaster, new CommandEventHandler(ClearAddiction_OnCommand));
        }

        [Usage("ClearAddiction")]
        [Description("Retire les addictions")]
        private static void ClearAddiction_OnCommand(CommandEventArgs e)
        {
          e.Mobile.BeginTarget(7, false, TargetFlags.None, new TargetCallback(ClearAddiction_Callback));
        }

        public static void ClearAddiction_Callback(Mobile mJoueur, object objCible)
        {
            if (objCible is PlayerMobile)
            {
                PlayerMobile addict = objCible as PlayerMobile;
                if (addict != null && !addict.Deleted)
                {
                    addict.ClearAddiction();
                    addict.SendMessage("Vous n'êtes plus dépendant de rien... sauf de Vivre!");
                }
            }
            else
                mJoueur.SendMessage("Vous devez cibler un joueur");
        }
    }
}