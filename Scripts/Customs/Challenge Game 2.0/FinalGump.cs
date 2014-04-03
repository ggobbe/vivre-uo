/*
 	Challenge Game 2.0
	Update for RunUO 2.0 by Lokai
	7/18/2006
*/
using System; 
using Server; 
using Server.Gumps; 
using Server.Network;
using Server.Items;
using Server.Mobiles;
using System.Collections;
using Server.Targeting;

namespace Server.Gumps
{
    public class FinalGump : Gump
    {
        private Mobile m_OpponentMobile;
        private Mobile m_ChallengerMobile;
        private ChallengeStone m_Item;
        private ArrayList m_Players = new ArrayList();
        public ResponseTimers m_Timer;
        private string message = "All protective spells will be removed before fight begins. All illegal items will be returned to your bank after the fight is finished.";
        private const string Affraid = "{0} got scared and refused the challenge!";
        private const string Error = "There was an error while trying to form this challenge, please try again shortly!";

        public FinalGump(PlayerMobile challenger, PlayerMobile opponent, ChallengeStone item)
            : base(0, 0)
        {
            m_OpponentMobile = opponent;
            m_ChallengerMobile = challenger;
            m_Item = item;
            m_Timer = new ResponseTimers(m_Item, m_OpponentMobile);
            m_Timer.Start();

            Closable = false;
            Dragable = false;

            Targeting.Target.Cancel(m_ChallengerMobile);
            Targeting.Target.Cancel(m_OpponentMobile);

            AddImageTiled(107, 132, 14, 14, 83);
            AddImageTiled(121, 130, 245, 14, 84);
            AddImageTiled(366, 132, 14, 14, 85);
            AddImageTiled(107, 145, 14, 140, 86);
            AddImageTiled(118, 144, 252, 143, 87);
            AddImageTiled(366, 145, 14, 140, 88);
            AddImageTiled(107, 285, 14, 14, 89);
            AddImageTiled(121, 285, 245, 11, 90);
            AddImageTiled(366, 285, 14, 14, 91);
            AddHtml(123, 154, 233, 20, "<basefont color=#FF0000><center>Will you accept " + challenger.Name + "'s challenge?</center></basefont>", false, false);
            AddHtml(125, 175, 237, 77, "<basefont color=#CCCC33>" + message + "</basefont>", false, false);
            AddButton(135, 260, 2128, 2130, 1, GumpButtonType.Reply, 0);
            AddButton(300, 260, 2119, 2121, 2, GumpButtonType.Reply, 0);

        }
        public class ResponseTimers : Timer
        {
            private int m_Count = 20;
            private ChallengeStone m_Item;
            private Mobile m_ChallengerMobile;

            public ResponseTimers(ChallengeStone item, Mobile challenger)
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
            {
                m_Item = item;
                m_ChallengerMobile = challenger;
            }
            protected override void OnTick()
            {
                m_Count--;

                if (m_Count == 0)
                {
                    foreach (PlayerMobile opponent in m_Item.OpponentTeam)
                    {
                        opponent.SendMessage(43, String.Format(Error));
                    }
                    foreach (PlayerMobile challenger in m_Item.ChallengeTeam)
                    {
                        challenger.SendMessage(43, String.Format(Error));
                    }

                    m_Item.ClearAll();
                    m_ChallengerMobile.CloseGump(typeof(FinalGump));
                    Stop();
                }

                if (m_ChallengerMobile.NetState == null)
                {
                    m_Item.ClearAll();
                    Stop();
                }
            }
        }

        public override void OnResponse(NetState state, RelayInfo info) //Function for GumpButtonType.Reply Buttons 
        {
            Mobile from = state.Mobile;
            m_Timer.Stop();
            m_Players.AddRange(m_Item.ChallengeTeam);
            m_Players.AddRange(m_Item.OpponentTeam);

            switch (info.ButtonID)
            {
                case 2:
                    {
                        foreach (PlayerMobile pm in m_Players)
                        {
                            pm.SendMessage(43, String.Format(Affraid, from.Name));
                        }

                        m_Item.ClearAll();
                        break;
                    }

                case 1:
                    {
                        m_Item.m_ChallengerExitPointDest = m_ChallengerMobile.Location;
                        m_Item.m_OpponentExitPointDest = from.Location;
                        m_Item.m_MapOrig = m_ChallengerMobile.Map;

                        foreach (PlayerMobile pm in m_Players)
                        {
                            Mobile fighters = pm as Mobile;
                            pm.SendMessage(63, "The Challenge is accepted. Let the duel begin!");
                            m_Item.makeready(fighters);
                        }

                        Point3D temp1 = m_Item.m_ChallengerPointDest;
                        Point3D temp2 = m_Item.m_OpponentPointDest;
                        Map map = m_Item.m_MapDest;

                        foreach (PlayerMobile challenger in m_Item.ChallengeTeam)
                        {
                            Point3D p = new Point3D((temp1.X + 1), temp1.Y, temp1.Z);
                            if (p == Point3D.Zero)
                                p = challenger.Location;

                            if (map == null || map == Map.Internal)
                                map = challenger.Map;

                            challenger.MoveToWorld(p, map);
                        }

                        foreach (PlayerMobile opponent in m_Item.OpponentTeam)
                        {
                            Point3D q = new Point3D((temp2.X - 1), temp2.Y, temp2.Z);
                            if (q == Point3D.Zero)
                                q = opponent.Location;

                            if (map == null || map == Map.Internal)
                                map = opponent.Map;

                            opponent.MoveToWorld(q, map);
                        }
                        m_Item.TimerStart();
                        break;
                    }

                case 0:
                    {
                        m_Item.ClearAll();
                        break;
                    }
            }
        }
    }
}