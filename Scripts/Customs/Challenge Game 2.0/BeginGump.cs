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
    public class BeginGump : Gump
    {
        private string message = "Please choose the type of challenge you wish to commence below or use the checkbox above to enable/disable the ability for others to challenge you.";
        private PlayerMobile m_Challenger;
        private ArrayList m_Stones;
        private ChallengeGameType m_Game;

        public BeginGump(PlayerMobile challenger, ArrayList stones)
            : base(0, 0)
        {
            m_Challenger = challenger;
            m_Stones = stones;

            AddImageTiled(107, 132, 14, 14, 83);
            AddImageTiled(121, 130, 245, 14, 84);
            AddImageTiled(366, 132, 14, 14, 85);
            AddImageTiled(107, 145, 14, 140, 86);
            AddImageTiled(118, 144, 252, 143, 87);
            AddImageTiled(366, 145, 14, 140, 88);
            AddImageTiled(107, 285, 14, 14, 89);
            AddImageTiled(121, 285, 245, 11, 90);
            AddImageTiled(366, 285, 14, 14, 91);
            AddHtml(105, 145, 233, 44, "<basefont color=#FF0000><center>Accept challenges?</center></basefont>", false, false);
            AddHtml(125, 170, 237, 77, "<basefont color=#CCCC33>" + message + "</basefont>", false, false);
            AddLabel(169, 270, 43, string.Format("1vs1"));
            AddLabel(242, 270, 43, string.Format("2vs2"));

            AddButton(197, 270, 0x15A2, 0x15A3, 1, GumpButtonType.Reply, 0);
            AddButton(277, 270, 0x15A2, 0x15A3, 2, GumpButtonType.Reply, 0);

            if (m_Challenger.CanBeChallenged)
                AddButton(280, 145, 0xD3, 0xD2, 3, GumpButtonType.Reply, 0);
            else
                AddButton(280, 145, 0xD2, 0xD3, 3, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            PlayerMobile m = from as PlayerMobile;

            switch (info.ButtonID)
            {
                case 3: // Chal on/off 
                    {
                        if (((PlayerMobile)from).CanBeChallenged)
                        {
                            ((PlayerMobile)from).CanBeChallenged = false;
                            from.SendMessage(63, "You will no longer accept challenge invitations!");
                        }
                        else
                        {
                            ((PlayerMobile)from).CanBeChallenged = true;
                            from.SendMessage(63, "You may now accept challenge invitations!");
                        }
                        from.SendGump(new BeginGump((PlayerMobile)from, m_Stones));

                        return;
                    }
                case 2: // 2vs2
                    {
                        m_Game = ChallengeGameType.TwoPlayerTeam;
                        break;
                    }
                case 1: // 1vs1
                    {
                        m_Game = ChallengeGameType.OnePlayerTeam;
                        break;
                    }
                case 0:
                    {
                        from.SendMessage(43, "Cancelled!");

                        return;
                    }
            }

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
                    else if (m.GameTime < TimeSpan.FromMinutes(30.0))
                    {
                        from.SendMessage(43, "The ladder system is usable by characters who have a character age of at least 30 minutes of in-game play!");
                        return;
                    }
                    else if (m.IsInChallenge)
                    {
                        from.SendMessage(43, "You are already in the process of using the ladder system!");
                        return;
                    }
                    else if (m.Hits != m.HitsMax)
                    {
                        from.SendMessage(43, "You must be fully healed before using the ladder system!");
                        return;
                    }
                    else if (!m.CanBeChallenged)
                    {
                        from.SendMessage(43, "You currently have the challenge feature disabled, please enable it via the menu!");
                        from.SendGump(new BeginGump((PlayerMobile)from, m_Stones));
                        return;
                    }
                    else
                    {
                        challstone.ClearAll();
                        m_Challenger.IsInChallenge = true;
                        challstone.OnDoubleClick(from);
                        return;
                    }
                }
            }
            from.SendMessage(43, "There are no open ladder arenas for that type of challenge right now, please try again soon!");
            return;
        }
    }
}