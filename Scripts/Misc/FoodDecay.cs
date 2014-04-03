using System;
using Server.Network;
using Server;
using Server.Mobiles;
using Server.Regions;

namespace Server.Misc
{
	public class FoodDecayTimer : Timer
	{
		public static void Initialize()
		{
			new FoodDecayTimer().Start();
		}

        // Scriptiz : le temps de decay passe de 5 à 20 minutes
		public FoodDecayTimer() : base( TimeSpan.FromMinutes( 20 ), TimeSpan.FromMinutes( 20 ) )
		{
			Priority = TimerPriority.OneMinute;
		}

		protected override void OnTick()
		{
			FoodDecay();			
		}

		public static void FoodDecay()
		{
			foreach ( NetState state in NetState.Instances )
			{
                if (state.Mobile == null) continue; // Scriptiz : sert à rien de traiter les null

                // Scriptiz : les Young et les prisonniers ne subissent pas la faim et la soif
                if (state.Mobile is PlayerMobile && (((PlayerMobile)state.Mobile).Young || state.Mobile.Region.IsPartOf(typeof(Jail)))) continue;

                HungerDecay(state.Mobile);
                ThirstDecay(state.Mobile);

                // Scriptiz : Mise à jour du gump d'alimentation
                if (state.Mobile != null) Alimentation.UpdateGump(state.Mobile);
			}
		}

		public static void HungerDecay( Mobile m )
		{
			if ( m != null && m.Hunger >= 1 )
				m.Hunger -= 1;

            // Scriptiz : Ajout du système de faim
            if (m != null) Alimentation.CheckHunger(m);
		}

		public static void ThirstDecay( Mobile m )
		{
			if ( m != null && m.Thirst >= 1 )
				m.Thirst -= 1;

            // Scriptiz : Ajout du système de soif
            if (m != null) Alimentation.CheckThirst(m);
		}
	}
}