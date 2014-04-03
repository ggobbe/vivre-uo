using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Druid
{
   public class PackOfBeastSpell : DruidicSpell
   {
      private static SpellInfo m_Info = new SpellInfo(
            "Pack Of Beast", "En Sec Ohm Ess Sepa",
            //SpellCircle.Third,
            266,
            9040,
            false,
            Reagent.SpidersSilk,
            Reagent.Bloodmoss,
            Reagent.PetrifiedWood
         );
         
      public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 1 ); } }
      public override SpellCircle Circle { get { return SpellCircle.Third; } }
      public override double RequiredSkill{ get{ return 40.0; } }
      public override int RequiredMana{ get{ return 45; } }

      public PackOfBeastSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
      {
      }

      private static Type[] m_Types = new Type[]
         {
            typeof( BrownBear ),
            typeof( TimberWolf ),
            typeof( Panther ),
            typeof( GreatHart ),
            typeof( Hind ),
            typeof( Alligator ),
            typeof( Boar ),
            typeof( GiantRat )
         };

      public override void OnCast()
      {
         if ( CheckSequence() )
         {
            try
            {

               Type beasttype = ( m_Types[Utility.Random( m_Types.Length )] );

               BaseCreature creaturea = (BaseCreature)Activator.CreateInstance( beasttype );
               BaseCreature creatureb = (BaseCreature)Activator.CreateInstance( beasttype );
               BaseCreature creaturec = (BaseCreature)Activator.CreateInstance( beasttype );
               BaseCreature creatured = (BaseCreature)Activator.CreateInstance( beasttype );


               SpellHelper.Summon( creaturea, Caster, 0x215, TimeSpan.FromSeconds( 4.0 * Caster.Skills[CastSkill].Value ), false, false );
               SpellHelper.Summon( creatureb, Caster, 0x215, TimeSpan.FromSeconds( 4.0 * Caster.Skills[CastSkill].Value ), false, false );

               Double morebeast = 0 ;

               morebeast = Utility.Random( 10 ) + ( Caster.Skills[CastSkill].Value * 0.1 );


               if ( morebeast > 11 )
               {
                  SpellHelper.Summon( creaturec, Caster, 0x215, TimeSpan.FromSeconds( 4.0 * Caster.Skills[CastSkill].Value ), false, false );
               }

               if ( morebeast > 18 )
               {
                  SpellHelper.Summon( creatured, Caster, 0x215, TimeSpan.FromSeconds( 4.0 * Caster.Skills[CastSkill].Value ), false, false );
               }


            }
            catch
            {
            }
         }

         FinishSequence();
      }

      public override TimeSpan GetCastDelay()
      {
         return TimeSpan.FromSeconds( 7.5 );
      }
   }
}
