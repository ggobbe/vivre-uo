/*
 	Challenge Game 2.0
	Update for RunUO 2.0 by Lokai
	7/18/2006
*/
using System;
using System.Collections;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using Server.Multis;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Items
{
    public class ChallengeTarget : Target
    {
        private PlayerMobile pm;
        private ChallengeStone m_Item;
        private const string ChallengeeFormat = "{0} is challenging me!";
        private const string TeamFormat = "{0} is selecting me to be a teammate!";
        private int i;

        public ChallengeTarget(PlayerMobile from, ChallengeStone item, int count)
            : base(100, false, TargetFlags.None)
        {
            pm = from;
            m_Item = item;
            i = count;
        }

        protected override void OnCantSeeTarget(Mobile from, object target)
        { m_Item.ClearAll(); }

        protected override void OnTargetCancel(Mobile from, TargetCancelType Overriden)
        { m_Item.ClearAll(); }

        protected override void OnTargetDeleted(Mobile from, object target)
        { m_Item.ClearAll(); }

        protected override void OnTargetNotAccessible(Mobile from, object target)
        { m_Item.ClearAll(); }

        protected override void OnTargetOutOfLOS(Mobile from, object target)
        { m_Item.ClearAll(); }

        protected override void OnTargetOutOfRange(Mobile from, object target)
        { m_Item.ClearAll(); }

        protected override void OnTargetUntargetable(Mobile from, object target)
        { m_Item.ClearAll(); }

        protected override void OnTarget(Mobile from, object target)
        {
            if (m_Item.ChallengeTeam.Contains(target))
            {
                from.SendMessage("You can't add someone already on your team");
                from.Target = new ChallengeTarget(pm, m_Item, i);
            }
            else if (m_Item.OpponentTeam.Contains(target))
            {
                from.SendMessage("You can't challenge someone already on the opposing team");
                from.Target = new ChallengeTarget(pm, m_Item, i);
            }
            else if (!(target is PlayerMobile))
            {
                from.SendMessage("You can't target that");
                from.Target = new ChallengeTarget(pm, m_Item, i);
            }
            else if (target is PlayerMobile)
            {
                PlayerMobile m = (PlayerMobile)target;

                if (m.Young == true)
                {
                    from.SendMessage(33, "You can not challenge someone who is young, select again!");
                    m.SendMessage(33, "The ladder system is not usable by characters who are young!");
                    from.Target = new ChallengeTarget(pm, m_Item, i);
                }
                else if (m.Frozen == true)
                {
                    from.SendMessage(33, "That player is frozen, select again!");
                    from.Target = new ChallengeTarget(pm, m_Item, i);
                }
                else if (m.Hits != m.HitsMax)
                {
                    from.SendMessage(33, "Player's health must be full in order to be challenged, select again!");
                    from.Target = new ChallengeTarget(pm, m_Item, i);
                }
                else if (m.IsInChallenge)
                {
                    from.SendMessage(33, "That player is currently being invited into a challenge, select again!");
                    from.Target = new ChallengeTarget(pm, m_Item, i);
                }
                else if (!m.CanBeChallenged)
                {
                    from.SendMessage(33, "That player is currently not accepting challenge invitations, select again!");
                    from.Target = new ChallengeTarget(pm, m_Item, i);
                }
                else
                {
                    if (m_Item.Game == ChallengeGameType.OnePlayerTeam)
                    {
                        m_Item.AddOpponentPlayer(m);
                        m.IsInChallenge = true;
                        m.PublicOverheadMessage(MessageType.Regular, 1153, true, String.Format(ChallengeeFormat, from.Name));
                        m.SendGump(new FinalGump(pm, m, m_Item));
                    }
                    else if (m_Item.Game == ChallengeGameType.TwoPlayerTeam)
                    {
                        m.IsInChallenge = true;

                        if (i < 2)
                        {
                            if (i == 0)
                            {
                                m_Item.ChallengeTeam.Add(m);
                                m.PublicOverheadMessage(MessageType.Regular, 1153, true, String.Format(TeamFormat, from.Name));
                            }
                            if (i == 1)
                            {
                                m_Item.OpponentTeam.Add(m);
                                m.PublicOverheadMessage(MessageType.Regular, 1153, true, String.Format(ChallengeeFormat, from.Name));
                            }
                            i++;
                            m.SendGump(new PartnerGump(pm, m_Item, i, m));
                        }
                        else if (i == 2)
                        {
                            m_Item.OpponentTeam.Add(m);
                            m.PublicOverheadMessage(MessageType.Regular, 1153, true, String.Format(ChallengeeFormat, from.Name));
                            m.SendGump(new FinalGump(pm, m, m_Item));
                            i = 0;
                        }
                    }
                }
            }
        }
    }
}