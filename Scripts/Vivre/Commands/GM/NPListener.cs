/***************************************************************************
 *                               NPListener.cs
 *                            -------------------
 *   begin                : May 25, 2010
 *   copyright            : (C) Scriptiz
 *   email                : maeliguul@hotmail.com
 *   version              : 2010-07-23
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
using System.Collections.Generic;
using Server;
using Server.Mobiles;

namespace Server.Misc
{
    public class NPListener
    {
        private static int distance = 15;
        private static Mobile[][] ListenedMobiles = new Mobile[15][];   // 15 personnes max !

        public static void Initialize()
        {
            EventSink.Speech += new SpeechEventHandler(SpeechToNPC);
            EventSink.Logout += new LogoutEventHandler(GMLogout);
        }

        public static bool AddListenedMobile(Mobile listener, Mobile listened)
        {
            if (listener == null || listened == null) return false;

            for (int i = 0; i < ListenedMobiles.Length; i++)
            {
                if (ListenedMobiles[i] == null)
                {
                    ListenedMobiles[i] = new Mobile[2];
                    ListenedMobiles[i][0] = listener;
                    ListenedMobiles[i][1] = listened;
                    return true;
                }
            }

            return false;
        }

        public static bool RemoveListenedMobile(Mobile listener, Mobile listened)
        {
            if (listener == null || listened == null) return false;

            bool removed = false;
            for (int i = 0; i < ListenedMobiles.Length; i++)
            {
                if (ListenedMobiles[i] != null && ListenedMobiles[i][0] == listener && ListenedMobiles[i][1] == listened)
                {
                    ListenedMobiles[i][0] = null;
                    ListenedMobiles[i][1] = null;
                    ListenedMobiles[i] = null;
                    removed = true;
                }
            }
            return removed;
        }

        private static void SpeechToNPC(SpeechEventArgs args)
        {
            if (args.Mobile == null) return;

            Mobile speaker = args.Mobile;

            foreach (Mobile m in speaker.GetMobilesInRange(distance))
            {
                int index = IndexListened(m);
                if (index >= 0)
                {
                    Mobile gm = ListenedMobiles[index][0];
                    Mobile pnj = ListenedMobiles[index][1];

                    if (gm.Map != m.Map || gm.GetDistanceToSqrt(m) >= 12)
                        gm.SendMessage(pnj.SpeechHue, "<" + pnj.Name + "> " + speaker.Name + " : " + args.Speech);
                }
            }
        }

        private static int IndexListened(Mobile listened)
        {
            for (int i = 0; i < ListenedMobiles.Length; i++)
            {
                if (ListenedMobiles[i] != null && ListenedMobiles[i][1] == listened && ListenedMobiles[i][0] != null)
                    return i;
                else
                {
                    // On tombe sur un mauvais enregistrement : On le nettoie
                    if(ListenedMobiles[i] != null && (ListenedMobiles[i][0] == null || ListenedMobiles[i][1] == null))
                        ListenedMobiles[i] = null;
                }
            }
            return -1;
        }

        // Si un GM se déconnecte on le retire de la liste lui et ses PNJ écoutés
        private static void GMLogout(LogoutEventArgs args)
        {
            if (args.Mobile == null || args.Mobile.AccessLevel == AccessLevel.Player) return;

            Mobile gm = args.Mobile;

            for (int i = 0; i < ListenedMobiles.Length; i++)
            {
                if (ListenedMobiles[i] != null && ListenedMobiles[i][0] == gm)
                    ListenedMobiles[i] = null;
            }
        }
    }
}