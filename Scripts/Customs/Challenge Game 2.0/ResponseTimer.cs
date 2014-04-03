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

namespace Server.Items
{
	public class ResponseTimers : Timer
	{
		private int m_Count = 20;
		private ChallengeStone m_Item;
		private Mobile m_ChallengerMobile;
		private PlayerMobile m;
		private const string Affraid = "{0} got scared and refused the challenge!";
		private const string Error = "There was an error while trying to form this challenge, please try again shortly!";
	
		public ResponseTimers( ChallengeStone item, Mobile challenger ) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ))
		{
			m_Item = item;
			m_ChallengerMobile = challenger;
			m = challenger as PlayerMobile;
		}
		protected override void OnTick() 
		{
			m_Count--;
			if( m_Count == 0 )
			{
				if(m_Item.ChallengeTeam.Count == 0)
				{
					m_ChallengerMobile.SendMessage( 43, "Timer has expired, try again later.");
					m.IsInChallenge = false;
				}
				foreach ( PlayerMobile opponent in m_Item.OpponentTeam )
				{
					opponent.SendMessage(43, String.Format( Error ) );
				}
				foreach( PlayerMobile challenger in m_Item.ChallengeTeam )
				{
					challenger.SendMessage(43, String.Format( Error ) );
				}
				m_Item.ClearAll();
				m_ChallengerMobile.CloseGump( typeof( FinalGump ));
				m_ChallengerMobile.CloseGump( typeof( PartnerGump ));
				Stop();
			}

			if( m_ChallengerMobile.NetState == null )
			{
				m_Item.ClearAll();
				Stop();
			}
		}
	}
}
