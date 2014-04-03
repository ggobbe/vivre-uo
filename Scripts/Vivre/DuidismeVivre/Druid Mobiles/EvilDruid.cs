//created by henry_r
//12/19/07
//[RunUO 2.0 RC1]
//See .txt file for info
//
using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.DruidSystem;

namespace Server.DruidSystem.Mobiles
{
	[CorpseName( "a evil druid corpse" )]
	public class EvilDruid : BaseCreature
	{
		private static string[] m_Names = new string[]
		{
			"Amergin",
			"Lochru",
			"Senias",
			"Taliesin",
			"Urais",
			"Marduk",
			"Zamolxis"
		};

		private static string[] m_Titles = new string[]
		{
			"Protector of the Druids",
			"Guardian of the Druids",
			"Seer of the Druids",
			"The Mysticical Druid",
			"Master of the Druids"
		};	
			
		[Constructable]
		public EvilDruid() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = m_Names[Utility.Random( m_Names.Length )];
			Title = m_Titles[Utility.Random( m_Titles.Length )];
			Body = 0x190;
			Hue = Utility.RandomSkinHue();

			SetStr( 90, 100 );
			SetDex( 50, 75 );
			SetInt( 150, 250  );
			SetHits( 900, 1100 );
			SetDamage( 12, 18 );
			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 50, 70 );
			SetResistance( ResistanceType.Fire, 50, 70 );
			SetResistance( ResistanceType.Cold, 50, 70 );
			SetResistance( ResistanceType.Poison, 50, 70 );
			SetResistance( ResistanceType.Energy, 50, 70 );

			SetSkill( SkillName.Herding, 95.0, 120.0 );
			SetSkill( SkillName.AnimalLore, 95.0, 120.0 );
			SetSkill( SkillName.Meditation, 95.0, 100.0 );
			SetSkill( SkillName.MagicResist, 100.0, 120.0 );
			SetSkill( SkillName.Tactics, 95.0, 120.0 );
			SetSkill( SkillName.Wrestling, 95.0, 120.0 );

			Fame = 2000;
			Karma = -10000;

			VirtualArmor = 70;

                                                     m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 2, 5 ) );

			int hue = Utility.RandomBlueHue();
			AddItem( new Robe( hue ) );
			AddItem( new Sandals() );
			AddItem( new WizardsHat( hue ) );

			PackGold( 400, 600 );

                                                                    switch (Utility.Random( 16 ) )
                                            {

                                                         case 0: PackItem( new BlendWithForestScroll() ); break;
                                                         case 1: PackItem( new GraspingRootsScroll() ); break;
                                                         case 2: PackItem( new MushroomCircleScroll() ); break;
                                                         case 3: PackItem( new PackOfBeastScroll() ); break;
                                                         case 4: PackItem( new SpringOfLifeScroll() ); break;
                                                         case 5: PackItem( new VolcanicEruptionScroll() ); break;
                                                         case 6: PackItem( new EnchantedGroveScroll() ); break;
                                                         case 7: PackItem( new HollowReedScroll() ); break;
                                                         case 8: PackItem( new MushroomGatewayScroll() ); break;
                                                         case 9: PackItem( new RestorativeSoilScroll() ); break;
                                                         case 10: PackItem( new SwarmOfInsectsScroll() ); break;
                                                         case 11: PackItem( new FireflyScroll() ); break;
                                                         case 12: PackItem( new LureStoneScroll() ); break;
                                                         case 13: PackItem( new NaturesPassageScroll() ); break;
                                                         case 14: PackItem( new ShieldOfEarthScroll() ); break;
                                                         case 15: PackItem( new TreefellowScroll() ); break;
                                                         

                                                         }

                                                                        switch (Utility.Random( 3 ) )
                                             {
			
                                                          case 0: PackItem( new PetrifiedWood ( Utility.Random( 10 ) + 3) ); break;
                                                          case 1: PackItem( new Pumice ( Utility.Random( 10 ) + 3 ) ); break;
                                                          case 2: PackItem( new SpringWater ( Utility.Random( 10 ) + 3 ) ); break; 

                                                          }                                                                                                 
			
                                                      if ( 0.003 > Utility.RandomDouble() ) 
		                   PackItem( new DruidCloak() );
		}

		public override bool CanRummageCorpses{ get{ return false; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool AlwaysMurderer{ get{ return true; } }

                                   public override bool InitialInnocent{ get{ return true; } }
		

		public override int GetHurtSound()
		{
			return 0x156;
		}

		public override int GetDeathSound()
		{
			return 0x15C;
		}

		private DateTime m_NextAbilityTime;

		public override void OnThink()
		{
			if ( DateTime.Now >= m_NextAbilityTime )
			{
				Mobile combatant = this.Combatant;

				if ( combatant != null && combatant.Map == this.Map && combatant.InRange( this, 12 ) && IsEnemy( combatant ) && !UnderEffect( combatant ) )
				{
					m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 20, 30 ) );

					this.Say( true, "I call a swarm of insects to sting your flesh!" );

					m_Table[combatant] = Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), TimeSpan.FromSeconds( 7.0 ), new TimerStateCallback( DoEffect ), new object[]{ combatant, 0 } );
				}
			}

			base.OnThink();
		}

		private static Hashtable m_Table = new Hashtable();

		public static bool UnderEffect( Mobile m )
		{
			return m_Table.Contains( m );
		}

		public static void StopEffect( Mobile m, bool message )
		{
			Timer t = (Timer)m_Table[m];

			if ( t != null )
			{
				if ( message )
					m.PublicOverheadMessage( Network.MessageType.Emote, m.SpeechHue, true, "* The open flame begins to scatter the swarm of insects *" );

				t.Stop();
				m_Table.Remove( m );
			}
		}

		public void DoEffect( object state )
		{
			object[] states = (object[])state;

			Mobile m = (Mobile)states[0];
			int count = (int)states[1];

			if ( !m.Alive )
			{
				StopEffect( m, false );
			}
			else
			{
				Torch torch = m.FindItemOnLayer( Layer.TwoHanded ) as Torch;

				if ( torch != null && torch.Burning )
				{
					StopEffect( m, true );
				}
				else
				{
					if ( (count % 4) == 0 )
					{
						m.LocalOverheadMessage( Network.MessageType.Emote, m.SpeechHue, true, "* The swarm of insects bites and stings your flesh! *" );
						m.NonlocalOverheadMessage( Network.MessageType.Emote, m.SpeechHue, true, String.Format( "* {0} is stung by a swarm of insects *", m.Name ) );
					}

					m.FixedParticles( 0x91C, 10, 180, 9539, EffectLayer.Waist );
					m.PlaySound( 0x00E );
					m.PlaySound( 0x1BC );

					AOS.Damage( m, this, Utility.RandomMinMax( 30, 40 ) - (Core.AOS ? 0 : 10), 100, 0, 0, 0, 0 );

					states[1] = count + 1;
					
					if ( !m.Alive || Utility.Random( 20 ) >18 )
						StopEffect( m, false );
				}
			}
		}




                                   public override void OnDeath( Container c )

                                             {

                                                      if ( 0.02 > Utility.RandomDouble() ) 
                                                      {
                                                                       
                                            {
		                   c.DropItem( new DruidicSpellbook() );
                                                     
                                             }
                      
                                     }
                                                                                                               
			base.OnDeath( c );
                                   }
						

		public EvilDruid( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}