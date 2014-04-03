/*
 	Challenge Game 2.0
	Update for RunUO 2.0 by Lokai
	7/18/2006
*/
using System;
using System.Collections;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;
using Server.Challenge;


namespace Server
{
    public class DuelKeywords
    {
        public static void Initialize()
        {
            EventSink.Speech += new SpeechEventHandler(EventSink_Speech);
        }

        private static void EventSink_Speech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;
            string s_PlayersSpeech = e.Speech;

            if (!(e.Mobile is PlayerMobile))
                return;

            PlayerMobile x = (PlayerMobile)(e.Mobile);

            if (s_PlayersSpeech.ToLower().IndexOf("i challenge thee") >= 0)
                StartDuel(false, x, Challenge.Challenge.WorldStones);
            if (s_PlayersSpeech.ToLower().IndexOf("as a team we challenge thee") >= 0)
                StartDuel(true, x, Challenge.Challenge.WorldStones);
        }
        public static void StartDuel(bool TwoPlayers, PlayerMobile challenger, ArrayList m_Stones)
        {
            Mobile from = (Mobile)challenger;
            PlayerMobile m = from as PlayerMobile;
            Items.ChallengeGameType m_Game;

            if (TwoPlayers)
                m_Game = ChallengeGameType.TwoPlayerTeam;
            else
                m_Game = ChallengeGameType.OnePlayerTeam;


            foreach (Item chall in m_Stones)
            {
                ChallengeStone challstone = chall as ChallengeStone;
                if (challstone.Active == true && challstone.Game == m_Game)
                {
                    if (m.Frozen == true)
                    {
                        from.SendMessage(43, "You cannot use right now because you are frozen!");
                        return;
                    }
                    else if (m.Young)
                    {
                        from.SendMessage(43, "You can not use the ladder system if your young!");
                        return;
                    }
                    else if (m.IsInChallenge)
                    {
                        from.SendMessage(43, "You are already in the process of using the ladder system!");
                        return;
                    }
                    /*     else if ( from.Map == Map.Trammel || from.Map == Map.Malas || from.Map == Map.Ilshenar )
                         {
                             from.SendMessage(1266, "You can only duel in Felucca as we are having problems dueling in other facets!" );
                         } */


                    else
                    {
                        challstone.ClearAll();
                        challenger.IsInChallenge = true;
                        challstone.OnDoubleClick(from);
                        {
                            if (m.Hits != m.HitsMax)
                            {
                                m.Hits = m.HitsMax;
                                m.Mana = 125;
                                m.Stam = 125;
                            }
                            return;
                        }
                    }
                }

            }
        }
    }
}
