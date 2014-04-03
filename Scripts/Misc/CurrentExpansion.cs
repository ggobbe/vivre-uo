using System;
using Server.Network;

namespace Server
{
	public class CurrentExpansion
	{
        private static readonly Expansion Expansion = Expansion.HS;

		public static void Configure()
		{
			Core.Expansion = Expansion;

			bool Enabled = Core.AOS;

			Mobile.InsuranceEnabled = Enabled;  // Scriptiz : on retire le ! pour activer le syst�me d'assurance
			ObjectPropertyList.Enabled = Enabled;
			Mobile.VisibleDamageType = !Enabled ? VisibleDamageType.Related : VisibleDamageType.None;
			Mobile.GuildClickMessage = !Enabled;
			Mobile.AsciiClickMessage = !Enabled;

			if ( Enabled )
			{
				AOS.DisableStatInfluences();

				if ( ObjectPropertyList.Enabled )
					PacketHandlers.SingleClickProps = true; // single click for everything is overriden to check object property list

                Mobile.ActionDelay = TimeSpan.FromSeconds(1.0); 
                Mobile.AOSStatusHandler = new AOSStatusHandler(AOS.GetStatus);
            }
		}
	}
}
