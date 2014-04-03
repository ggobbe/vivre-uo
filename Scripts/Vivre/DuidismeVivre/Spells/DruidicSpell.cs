using System;
using Server;

namespace Server.Spells.Druid
{
   public abstract class DruidicSpell : Spell
   {
      public abstract SpellCircle Circle { get; }
      public abstract double RequiredSkill{ get; }
      public abstract int RequiredMana{ get; }

      public override SkillName CastSkill{ get{ return SkillName.Spellweaving; } }
      public override SkillName DamageSkill{ get{ return SkillName.Herding; } }

      public override bool ClearHandsOnCast{ get{ return false; } }

      public DruidicSpell( Mobile caster, Item scroll, SpellInfo info ) : base( caster, scroll, info )
      {
      }

      public override void GetCastSkills( out double min, out double max )
      {
         min = RequiredSkill;
         max = RequiredSkill + 30.0;
      }

      public override int GetMana()
      {
         return RequiredMana;
      }
public virtual bool CheckResisted( Mobile target )
		{
			double n = GetResistPercent( target );

			n /= 100.0;

			if( n <= 0.0 )
				return false;

			if( n >= 1.0 )
				return true;

			int maxSkill = (1 + (int)Circle) * 10;
			maxSkill += (1 + ((int)Circle / 6)) * 25;

			if( target.Skills[SkillName.MagicResist].Value < maxSkill )
				target.CheckSkill( SkillName.MagicResist, 0.0, 120.0 );

			return (n >= Utility.RandomDouble());
		}
  public virtual double GetResistPercentForCircle( Mobile target, SpellCircle circle )
		{
			double firstPercent = target.Skills[SkillName.MagicResist].Value / 5.0;
			double secondPercent = target.Skills[SkillName.MagicResist].Value - (((Caster.Skills[CastSkill].Value - 20.0) / 5.0) + (1 + (int)circle) * 5.0);

			return (firstPercent > secondPercent ? firstPercent : secondPercent) / 2.0; // Seems should be about half of what stratics says.
		}
       public virtual double GetResistPercent( Mobile target )
		{
			return GetResistPercentForCircle( target, Circle );
		}
      /*public override TimeSpan GetCastDelay()
      {
         return TimeSpan.FromSeconds( CastDelay );
      }*/
   }
}
