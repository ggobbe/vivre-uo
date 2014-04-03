using System;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Items;

namespace Server.Spells.Druid
{
   public class GraspingRootsSpell : DruidicSpell
   {
      private static SpellInfo m_Info = new SpellInfo(
            "Grasping Roots", "En Ohm Sepa Tia Kes",
            //SpellCircle.Fifth,
            218,
            9012,
            false,
            Reagent.SpringWater,
            Reagent.Bloodmoss,
            Reagent.SpidersSilk
         );

      public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 1.5 ); } }
      public override SpellCircle Circle { get { return SpellCircle.Fifth; } }
      public override double RequiredSkill{ get{ return 40.0; } }
      public override int RequiredMana{ get{ return 40; } }


      public GraspingRootsSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
      {
      }

      public override void OnCast()
      {
         Caster.Target = new InternalTarget( this );
      }

      public void Target( Mobile m )
      {
         if ( !Caster.CanSee( m ) )
         {
            Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
         }
         else if ( CheckHSequence( m ) )
         {
            SpellHelper.Turn( Caster, m );

            double duration;

            // Algorithm: ((20% of AnimalTamin) + 7) seconds [- 50% if resisted] seems to work??
            duration = 7.0 + (Caster.Skills[DamageSkill].Value * 0.2);

            // Resist if Str + Dex / 2 is greater than CastSkill eg. AnimalLore seems to work << this formula will almost always be resisted against all but the lowest creatures
            if ( ( (Caster.Skills[CastSkill].Value ) + (Caster.Skills[DamageSkill].Value ) ) < ( ( m.Str + m.Dex ) * 0.5 ) ) // << This should work much more often
               duration *= 0.5;

            // no less than 0 seconds no more than 9 seconds
            if ( duration < 0.0 )
               duration = 0.0;
            if ( duration > 9.0 )
               duration = 9.0;

            m.PlaySound( 0x2A1 );

            m.Paralyze( TimeSpan.FromSeconds( duration ) );
            m.FixedParticles( 0x375A, 2, 10, 5027, 0x3D, 2, EffectLayer.Waist );

            {
               Point3D loc = new Point3D( m.X, m.Y, m.Z );

               Item item = new InternalItem( loc, Caster.Map, Caster );

            }


         }

         FinishSequence();
      }

      //[DispellableField]
      private class InternalItem : Item
      {
         private Timer m_Timer;
         private DateTime m_End;

         //public override bool BlocksFit{ get{ return true; } }

         public InternalItem( Point3D loc, Map map, Mobile caster ) : base( 0xC5F )
         {
            Visible = false;
            Movable = false;

            MoveToWorld( loc, map );

            if ( caster.InLOS( this ) )
               Visible = true;
            else
               Delete();

            if ( Deleted )
               return;

            m_Timer = new InternalTimer( this, TimeSpan.FromSeconds( 30.0 ) );
            m_Timer.Start();

            m_End = DateTime.Now + TimeSpan.FromSeconds( 30.0 );
         }

         public InternalItem( Serial serial ) : base( serial )
         {
         }

         public override void Serialize( GenericWriter writer )
         {
            base.Serialize( writer );

            writer.Write( (int) 1 ); // version

            writer.Write( m_End - DateTime.Now );
         }

         public override void Deserialize( GenericReader reader )
         {
            base.Deserialize( reader );

            int version = reader.ReadInt();
         }

         public override void OnAfterDelete()
         {
            base.OnAfterDelete();

            if ( m_Timer != null )
               m_Timer.Stop();
         }

         private class InternalTimer : Timer
         {
            private InternalItem m_Item;

            public InternalTimer( InternalItem item, TimeSpan duration ) : base( duration )
            {
               m_Item = item;
            }

            protected override void OnTick()
            {
               m_Item.Delete();
            }
         }
      }

      private class InternalTarget : Target
      {
         private GraspingRootsSpell m_Owner;

         public InternalTarget( GraspingRootsSpell owner ) : base( 12, true, TargetFlags.None )
         {
            m_Owner = owner;
         }

         protected override void OnTarget( Mobile from, object o )
         {
            if ( o is Mobile )
               m_Owner.Target( (Mobile)o );
         }

         protected override void OnTargetFinish( Mobile from )
         {
            m_Owner.FinishSequence();
         }
      }
   }
}
