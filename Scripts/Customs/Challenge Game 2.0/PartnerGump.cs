/*
 	Challenge Game 2.0
	Update for RunUO 2.0 by Lokai
	7/18/2006
*/
using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Gumps
{
    public class PartnerGump : Gump
    {
        private PlayerMobile m_ChallengerMobile;
        private PlayerMobile m_OpponentMobile;
        private ChallengeStone m_Item;
        private int i;
        public ResponseTimers m_Timer2;
        private string message = "All protective spells will be removed before fight begins. All illegal items will be returned to your bank after the fight is finished.";
        private const string Affraid = "{0} got scared and refused the challenge!";
        private const string Error = "There was an error while trying to form this challenge, please try again shortly!";

        public PartnerGump(PlayerMobile challenger, ChallengeStone item, int counter, PlayerMobile opponent)
            : base(0, 0)
        {
            i = counter;
            m_ChallengerMobile = challenger;
            m_OpponentMobile = opponent;
            m_Item = item;
            m_Timer2 = new ResponseTimers(m_Item, m_OpponentMobile);
            m_Timer2.Start();

            Closable = false;
            Dragable = false;

            AddImageTiled(107, 132, 14, 14, 83);
            AddImageTiled(121, 130, 245, 14, 84);
            AddImageTiled(366, 132, 14, 14, 85);
            AddImageTiled(107, 145, 14, 140, 86);
            AddImageTiled(118, 144, 252, 143, 87);
            AddImageTiled(366, 145, 14, 140, 88);
            AddImageTiled(107, 285, 14, 14, 89);
            AddImageTiled(121, 285, 245, 11, 90);
            AddImageTiled(366, 285, 14, 14, 91);
            AddHtml(115, 145, 233, 44, "<basefont color=#FF0000><center>Will you join " + m_ChallengerMobile.Name + " as your partner in a challenge?</center></basefont>", false, false);
            AddHtml(125, 190, 237, 77, "<basefont color=#CCCC33>" + message + "</basefont>", false, false);
            AddButton(135, 270, 2128, 2130, 1, GumpButtonType.Reply, 0);
            AddButton(300, 270, 2119, 2121, 2, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            m_Timer2.Stop();
            switch (info.ButtonID)
            {
                case 2:
                    {
                        foreach (PlayerMobile opponent in m_Item.OpponentTeam)
                        {
                            opponent.SendMessage(43, String.Format(Affraid, from.Name));
                        }
                        foreach (PlayerMobile challenger in m_Item.ChallengeTeam)
                        {
                            challenger.SendMessage(43, String.Format(Affraid, from.Name));
                        }

                        m_Item.ClearAll();
                        break;
                    }
                case 1:
                    {
                        if (m_Item.Game == ChallengeGameType.TwoPlayerTeam)
                        {
                            if (i == 1)
                                m_ChallengerMobile.SendMessage("Choose each of your opponents.");
                            else if (i == 2)
                                m_ChallengerMobile.SendMessage("Choose the last opponent.");
                        }

                        m_ChallengerMobile.Target = new ChallengeTarget(m_ChallengerMobile, m_Item, i);
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