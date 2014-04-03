using System;
using System.Collections;
using Server;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Factions;
using Server.Spells;
using Server.Spells.Spellweaving;

namespace Server.SkillHandlers
{
	public class AnimalTaming
	{
		private static Hashtable m_BeingTamed = new Hashtable();

		public static void Initialize()
		{
			SkillInfo.Table[(int)SkillName.AnimalTaming].Callback = new SkillUseCallback( OnUse );
		}

		private static bool m_DisableMessage;

		public static bool DisableMessage
		{
			get{ return m_DisableMessage; }
			set{ m_DisableMessage = value; }
		}

		public static TimeSpan OnUse( Mobile m )
		{
			m.RevealingAction();

			m.Target = new InternalTarget();
			m.RevealingAction();

			if ( !m_DisableMessage )
				m.SendMessage( "Quel animal voulez-vous apprivoiser?" ); // Tame which animal?

			return TimeSpan.FromHours( 6.0 );
		}

		public static bool CheckMastery( Mobile tamer, BaseCreature creature )
		{
			BaseCreature familiar = (BaseCreature)Spells.Necromancy.SummonFamiliarSpell.Table[tamer];

			if ( familiar != null && !familiar.Deleted && familiar is DarkWolfFamiliar )
			{
				if ( creature is DireWolf || creature is GreyWolf || creature is TimberWolf || creature is WhiteWolf || creature is BakeKitsune )
					return true;
			}

			return false;
		}

		public static bool MustBeSubdued( BaseCreature bc )
		{
			if (bc.Owners.Count > 0) { return false; } //Checks to see if the animal has been tamed before
            return bc.SubdueBeforeTame && (bc.Hits > (bc.HitsMax / 10));
        }	

		public static void ScaleStats( BaseCreature bc, double scalar )
		{
			if ( bc.RawStr > 0 )
				bc.RawStr = (int)Math.Max( 1, bc.RawStr * scalar );

			if ( bc.RawDex > 0 )
				bc.RawDex = (int)Math.Max( 1, bc.RawDex * scalar );

			if ( bc.RawInt > 0 )
				bc.RawInt = (int)Math.Max( 1, bc.RawInt * scalar );

			if ( bc.HitsMaxSeed > 0 )
			{
				bc.HitsMaxSeed = (int)Math.Max( 1, bc.HitsMaxSeed * scalar );
				bc.Hits = bc.Hits;
				}

			if ( bc.StamMaxSeed > 0 )
			{
				bc.StamMaxSeed = (int)Math.Max( 1, bc.StamMaxSeed * scalar );
				bc.Stam = bc.Stam;
			}
		}

		public static void ScaleSkills( BaseCreature bc, double scalar )
		{
			ScaleSkills( bc, scalar, scalar );
		}

		public static void ScaleSkills( BaseCreature bc, double scalar, double capScalar )
		{
			for ( int i = 0; i < bc.Skills.Length; ++i )
			{
				bc.Skills[i].Base *= scalar;

				bc.Skills[i].Cap = Math.Max( 100.0, bc.Skills[i].Cap * capScalar );

				if ( bc.Skills[i].Base > bc.Skills[i].Cap )
				{
					bc.Skills[i].Cap = bc.Skills[i].Base;
				}
			}
		}

		private class InternalTarget : Target
		{
			private bool m_SetSkillTime = true;

            public InternalTarget() : base(Core.AOS ? 3 : 2, false, TargetFlags.None)
            {
            }

			protected override void OnTargetFinish( Mobile from )
			{
				if ( m_SetSkillTime )
					from.NextSkillTime = DateTime.Now;
			}

            public virtual void ResetPacify(object obj)
            {
                if (obj is BaseCreature)
                {
                    ((BaseCreature)obj).BardPacified = true;
                }
            }

			protected override void OnTarget( Mobile from, object targeted )
			{
				from.RevealingAction();

				if ( targeted is Mobile )
				{
					if ( targeted is BaseCreature )
					{
						BaseCreature creature = (BaseCreature)targeted;

						if ( !creature.Tamable )
						{
							creature.PrivateOverheadMessage( MessageType.Regular, 0x3B2, false, "Cette créature ne peut être apprivoisée", from.NetState ); // That creature cannot be tamed.
						}
						else if ( creature.Controlled )
						{
							creature.PrivateOverheadMessage( MessageType.Regular, 0x3B2, false, "Cet animal a déjà un maître", from.NetState ); // That animal looks tame already.
						}
						else if ( from.Female && !creature.AllowFemaleTamer )
						{
							creature.PrivateOverheadMessage( MessageType.Regular, 0x3B2, false, "Seul un homme peut approcher cette créature", from.NetState ); // That creature can only be tamed by males.
						}
						else if ( !from.Female && !creature.AllowMaleTamer )
						{
							creature.PrivateOverheadMessage( MessageType.Regular, 0x3B2, false, "Seule une femme peut approcher cette créature", from.NetState ); // That creature can only be tamed by females.
						}
						else if ( creature is CuSidhe && from.Race != Race.Elf )
						{
							creature.PrivateOverheadMessage( MessageType.Regular, 0x3B2, false, "Vous ne pouvez apprivoiser cela!", from.NetState ); // You can't tame that!
						}
						else if ( from.Followers + creature.ControlSlots > from.FollowersMax )
						{
							from.SendMessage( "Vous avez trop d'animaux domestiques pour en apprivoiser un nouveau" ); // You have too many followers to tame that creature.
						}
						else if ( creature.Owners.Count >= BaseCreature.MaxOwners && !creature.Owners.Contains( from ) )
						{
                            creature.PrivateOverheadMessage(MessageType.Regular, 0x3B2, false, "Cet animal a eu suffisamment de maître dans le passé et souhaite qu'on le laisse tranquille", from.NetState); // This animal has had too many owners and is too upset for you to tame.
						}
						else if ( MustBeSubdued( creature ) )
						{
							creature.PrivateOverheadMessage( MessageType.Regular, 0x3B2, false, "Vous devez assujetir cette créature avant de l'apprivoiser", from.NetState ); // You must subdue this creature before you can tame it!
						}
						else if ( CheckMastery( from, creature ) || from.Skills[SkillName.AnimalTaming].Value >= creature.MinTameSkill )
						{
							FactionWarHorse warHorse = creature as FactionWarHorse;

							if ( warHorse != null )
							{
								Faction faction = Faction.Find( from );

								if ( faction == null || faction != warHorse.Faction )
								{
									creature.PrivateOverheadMessage( MessageType.Regular, 0x3B2, false, "Vous ne pouvez apprivoiser cette créature", from.NetState ); // You cannot tame this creature.
									return;
								}
							}

							if ( m_BeingTamed.Contains( targeted ) )
							{
								creature.PrivateOverheadMessage( MessageType.Regular, 0x3B2, false, "Quelqu'un tente déjà de l'apprivoiser", from.NetState ); // Someone else is already taming this.
							}
							else if ( creature.CanAngerOnTame && 0.95 >= Utility.RandomDouble() )
							{
								creature.PrivateOverheadMessage( MessageType.Regular, 0x3B2, false, "Vous irritez la créature!", from.NetState ); // You seem to anger the beast!
								creature.PlaySound( creature.GetAngerSound() );
								creature.Direction = creature.GetDirectionTo( from );
                                if (creature.BardPacified && Utility.RandomDouble() > .24)
                                {
                                    Timer.DelayCall(TimeSpan.FromSeconds(2.0), new TimerStateCallback(ResetPacify), creature);
                                }
                                else
                                {
                                    creature.BardEndTime = DateTime.Now;
                                }

                                creature.BardPacified = false;

                                if (creature.AIObject != null)
                                    creature.AIObject.DoMove(creature.Direction);


								if ( from is PlayerMobile && !(( (PlayerMobile)from ).HonorActive || TransformationSpellHelper.UnderTransformation( from, typeof( EtherealVoyageSpell ))))
									creature.Combatant = from;
							}
							else
							{
								m_BeingTamed[targeted] = from;

								from.LocalOverheadMessage( MessageType.Emote, 0x59, false, "Vous tentez d'apprivoiser la créature" ); // You start to tame the creature.
								from.NonlocalOverheadMessage( MessageType.Emote, 0x59, false, "*tente d'apprivoiser une créature*" ); // *begins taming a creature.*

								new InternalTimer( from, creature, Utility.Random( 3, 2 ) ).Start();

								m_SetSkillTime = false;
							}
						}
						else
						{
							creature.PrivateOverheadMessage( MessageType.Regular, 0x3B2, false, "Vous n'avez aucune chance d'apprivoiser cette créature", from.NetState ); // You have no chance of taming this creature.
						}
					}
					else
					{
						((Mobile)targeted).PrivateOverheadMessage( MessageType.Regular, 0x3B2, false, "Cela ne peut être apprivoisé", from.NetState ); // That being cannot be tamed.
					}
				}
				else
				{
					from.SendMessage( "Vous ne pouvez apprivoiser cela!" ); // You can't tame that!
				}
			}

			private class InternalTimer : Timer
			{
				private Mobile m_Tamer;
				private BaseCreature m_Creature;
				private int m_MaxCount;
				private int m_Count;
				private bool m_Paralyzed;
				private DateTime m_StartTime;

				public InternalTimer( Mobile tamer, BaseCreature creature, int count ) : base( TimeSpan.FromSeconds( 3.0 ), TimeSpan.FromSeconds( 3.0 ), count )
				{
					m_Tamer = tamer;
					m_Creature = creature;
					m_MaxCount = count;
					m_Paralyzed = creature.Paralyzed;
					m_StartTime = DateTime.Now;
					Priority = TimerPriority.TwoFiftyMS;
				}

				protected override void OnTick()
				{
					m_Count++;

					DamageEntry de = m_Creature.FindMostRecentDamageEntry( false );
					bool alreadyOwned = m_Creature.Owners.Contains( m_Tamer );

                    if (!m_Tamer.InRange(m_Creature, Core.AOS ? 7 : 6))
					{
						m_BeingTamed.Remove( m_Creature );
						m_Tamer.NextSkillTime = DateTime.Now;
						m_Creature.PrivateOverheadMessage( MessageType.Regular, 0x3B2, false, "Vous vous êtes trop éloigné pour continuer de l'apprivoiser", m_Tamer.NetState ); // You are too far away to continue taming.
						Stop();
					}
					else if ( !m_Tamer.CheckAlive() )
					{
						m_BeingTamed.Remove( m_Creature );
						m_Tamer.NextSkillTime = DateTime.Now;
						m_Creature.PrivateOverheadMessage( MessageType.Regular, 0x3B2, false, "Vous êtes morts et ne pouvez donc continuer de l'apprivoiser", m_Tamer.NetState ); // You are dead, and cannot continue taming.
						Stop();
					}
					else if ( !m_Tamer.CanSee( m_Creature ) || !m_Tamer.InLOS( m_Creature ) || !CanPath() )
					{
						m_BeingTamed.Remove( m_Creature );
						m_Tamer.NextSkillTime = DateTime.Now;
                        m_Tamer.SendMessage("Vous n'arrivez pas à suivre l'animal et devez donc cesser de l'apprivoiser"); // You do not have a clear path to the animal you are taming, and must cease your attempt.
						Stop();
					}
					else if ( !m_Creature.Tamable )
					{
						m_BeingTamed.Remove( m_Creature );
						m_Tamer.NextSkillTime = DateTime.Now;
						m_Creature.PrivateOverheadMessage( MessageType.Regular, 0x3B2, false, "Cette créature ne peut être apprivoisée", m_Tamer.NetState ); // That creature cannot be tamed.
						Stop();
					}
					else if ( m_Creature.Controlled )
					{
						m_BeingTamed.Remove( m_Creature );
						m_Tamer.NextSkillTime = DateTime.Now;
						m_Creature.PrivateOverheadMessage( MessageType.Regular, 0x3B2, false, "Cet animal a déjà un maître", m_Tamer.NetState ); // That animal looks tame already.
						Stop();
					}
					else if ( m_Creature.Owners.Count >= BaseCreature.MaxOwners && !m_Creature.Owners.Contains( m_Tamer ) )
					{
						m_BeingTamed.Remove( m_Creature );
						m_Tamer.NextSkillTime = DateTime.Now;
						m_Creature.PrivateOverheadMessage( MessageType.Regular, 0x3B2, false, "Cet animal a eu suffisamment de maître dans le passé et souhaite qu'on le laisse tranquille", m_Tamer.NetState ); // This animal has had too many owners and is too upset for you to tame.
						Stop();
					}
					else if ( MustBeSubdued( m_Creature ) )
					{
						m_BeingTamed.Remove( m_Creature );
						m_Tamer.NextSkillTime = DateTime.Now;
						m_Creature.PrivateOverheadMessage( MessageType.Regular, 0x3B2, false, "Vous devez assujetir cette créature avant de l'apprivoiser", m_Tamer.NetState ); // You must subdue this creature before you can tame it!
						Stop();
					}
					else if ( de != null && de.LastDamage > m_StartTime )
					{
						m_BeingTamed.Remove( m_Creature );
						m_Tamer.NextSkillTime = DateTime.Now;
						m_Creature.PrivateOverheadMessage( MessageType.Regular, 0x3B2, false, "Cet animal est trop irrité pour être apprivoisé", m_Tamer.NetState ); // The animal is too angry to continue taming.
						Stop();
					}
					else if ( m_Count < m_MaxCount )
					{
						m_Tamer.RevealingAction();

						switch ( Utility.Random( 3 ) )
						{
							case 0: m_Tamer.PublicOverheadMessage( MessageType.Regular, 0x3B2, Utility.Random( 502790, 4 ) ); break;
							case 1: m_Tamer.PublicOverheadMessage( MessageType.Regular, 0x3B2, Utility.Random( 1005608, 6 ) ); break;
							case 2: m_Tamer.PublicOverheadMessage( MessageType.Regular, 0x3B2, Utility.Random( 1010593, 4 ) ); break;
						}

						if ( !alreadyOwned ) // Passively check animal lore for gain
							m_Tamer.CheckTargetSkill( SkillName.AnimalLore, m_Creature, 0.0, 120.0 );

						if ( m_Creature.Paralyzed )
							m_Paralyzed = true;
					}
					else
					{
						m_Tamer.RevealingAction();
						m_Tamer.NextSkillTime = DateTime.Now;
						m_BeingTamed.Remove( m_Creature );

						if ( m_Creature.Paralyzed )
							m_Paralyzed = true;

						if ( !alreadyOwned ) // Passively check animal lore for gain
							m_Tamer.CheckTargetSkill( SkillName.AnimalLore, m_Creature, 0.0, 120.0 );

						double minSkill = m_Creature.MinTameSkill + (m_Creature.Owners.Count * 6.0);

						if ( minSkill > -24.9 && CheckMastery( m_Tamer, m_Creature ) )
							minSkill = -24.9; // 50% at 0.0?

						minSkill += 24.9;

						if ( CheckMastery( m_Tamer, m_Creature ) || alreadyOwned || m_Tamer.CheckTargetSkill( SkillName.AnimalTaming, m_Creature, minSkill - 25.0, minSkill + 25.0 ) )
						{
							if ( m_Creature.Owners.Count == 0 ) // First tame
							{
								if ( m_Creature is GreaterDragon )
								{
									ScaleSkills( m_Creature, 0.72, 0.90 ); // 72% of original skills trainable to 90%
									m_Creature.Skills[SkillName.Magery].Base = m_Creature.Skills[SkillName.Magery].Cap; // Greater dragons have a 90% cap reduction and 90% skill reduction on magery
								}
								else if ( m_Paralyzed )
									ScaleSkills( m_Creature, 0.86 ); // 86% of original skills if they were paralyzed during the taming
								else
									ScaleSkills( m_Creature, 0.90 ); // 90% of original skills

								if ( m_Creature.StatLossAfterTame )
									ScaleStats( m_Creature, 0.50 );
							}

							if ( alreadyOwned )
							{
								m_Tamer.SendMessage( "L'animal vient instinctivement près de vous" ); // That wasn't even challenging.
							}
							else
							{
								m_Creature.PrivateOverheadMessage( MessageType.Regular, 0x3B2, false, "Il semble vous accepter comme maître", m_Tamer.NetState ); // It seems to accept you as master.
								m_Creature.Owners.Add( m_Tamer );
							}

							m_Creature.SetControlMaster( m_Tamer );
							m_Creature.IsBonded = false;
						}
						else
						{
							m_Creature.PrivateOverheadMessage( MessageType.Regular, 0x3B2, false, "L'animal refuse de vous laisser approcher", m_Tamer.NetState ); // You fail to tame the creature.
						}
					}
				}

				private bool CanPath()
				{
					IPoint3D p = m_Tamer as IPoint3D;

					if ( p == null )
						return false;

					if( m_Creature.InRange( new Point3D( p ), 1 ) )
						return true;

					MovementPath path = new MovementPath( m_Creature, new Point3D( p ) );

					return path.Success;
				}
			}
		}
	}
}