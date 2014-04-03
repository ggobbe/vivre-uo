using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Druid
{
	public class FireflySpell : DruidicSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Summon Firefly", "Kes En Crur",
				//SpellCircle.First,
				269,
				9020,
				false,
		 	        Reagent.SulfurousAsh,
                                Reagent.Pumice
			);

		public FireflySpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}
  
                public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 1 ); } }
		public override SpellCircle Circle { get { return SpellCircle.First; } }
                public override double RequiredSkill{ get{ return 1.0; } }
                public override int RequiredMana{ get{ return 10; } }

		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
				return false;

		

			return true;
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
					SpellHelper.Summon( new FireflyFamiliar(), Caster, 0x217, TimeSpan.FromSeconds( 4.0 * Caster.Skills[SkillName.Herding].Value ), false, false );
					}

			FinishSequence();
		}
	}
}
