using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Spells.Druid
{
   public class VolcanicEruptionSpell : DruidicSpell
   {
      private static SpellInfo m_Info = new SpellInfo(
            "Volcanic Eruption", "Vauk Ohm En Tia Crur",
            //SpellCircle.Eighth,
            245,
            9042,
            false,
            Reagent.SulfurousAsh,
            Reagent.Pumice
         );
         
      public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 1 ); } }
      public override SpellCircle Circle { get { return SpellCircle.Eighth; } }
      public override double RequiredSkill{ get{ return 98.0; } }
      public override int RequiredMana{ get{ return 85; } }

      public VolcanicEruptionSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
      {
      }

      public override void OnCast()
      {
         Caster.Target = new InternalTarget( this );
      }

      public void Target( IPoint3D p )
      {
         if ( !Caster.CanSee( p ) )
         {
            Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
         }
         else if ( /*SpellHelper.CheckTown( p, Caster ) &&*/ CheckSequence() )
         {
            SpellHelper.Turn( Caster, p );

            if ( p is Item )
               p = ((Item)p).GetWorldLocation();

            double damage = Utility.Random( 27, 22 );

            ArrayList targets = new ArrayList();

            IPooledEnumerable eable = Caster.Map.GetMobilesInRange( new Point3D( p ), 1 + (int)(Caster.Skills[DamageSkill].Value / 10.0) );

            foreach ( Mobile m in eable )
            {
               if ( SpellHelper.ValidIndirectTarget( Caster, m ) && Caster.CanBeHarmful( m, false ) )
                  targets.Add( m );
            }

            eable.Free();

            if ( targets.Count > 0 )
            {
               //damage /= targets.Count; // ~ divides damage between targets, kinda sux

               for ( int i = 0; i < targets.Count; ++i )
               {
                  Mobile m = (Mobile)targets[i];

                  double toDeal = damage;

                  if ( CheckResisted( m ) )
                  {
                     toDeal *= 0.7;

                     m.SendLocalizedMessage( 501783 ); // You feel yourself resisting magical energy.
                  }

                  Caster.DoHarmful( m );
                  SpellHelper.Damage( this, m, toDeal, 50, 100, 0, 0, 0 );

                  m.FixedParticles( 0x3709, 20, 10, 5044, EffectLayer.RightFoot );
                  m.PlaySound( 0x21F );
                  m.FixedParticles( 0x36BD, 10, 30, 5052, EffectLayer.Head );
                  m.PlaySound( 0x208 );

               }
            }
         }

         FinishSequence();
      }

      private class InternalTarget : Target
      {
         private VolcanicEruptionSpell m_Owner;

         public InternalTarget( VolcanicEruptionSpell owner ) : base( 12, true, TargetFlags.None )
         {
            m_Owner = owner;
         }

         protected override void OnTarget( Mobile from, object o )
         {
            IPoint3D p = o as IPoint3D;

            if ( p != null )
               m_Owner.Target( p );
         }

         protected override void OnTargetFinish( Mobile from )
         {
            m_Owner.FinishSequence();
         }
      }
   }
}

