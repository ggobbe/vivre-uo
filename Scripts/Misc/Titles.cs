using System;
using System.Text;
using Server;
using Server.Mobiles;
using Server.Engines.CannedEvil;

namespace Server.Misc
{
	public class Titles
	{
		public const int MinFame = 0;
		public const int MaxFame = 15000;

		public static void AwardFame( Mobile m, int offset, bool message )
		{
			if ( offset > 0 )
			{
				if ( m.Fame >= MaxFame )
					return;

				offset -= m.Fame / 100;

				if ( offset < 0 )
					offset = 0;
			}
			else if ( offset < 0 )
			{
				if ( m.Fame <= MinFame )
					return;

				offset -= m.Fame / 100;

				if ( offset > 0 )
					offset = 0;
			}

			if ( (m.Fame + offset) > MaxFame )
				offset = MaxFame - m.Fame;
			else if ( (m.Fame + offset) < MinFame )
				offset = MinFame - m.Fame;

			m.Fame += offset;

			if ( message )
			{
				if ( offset > 40 )
					m.SendLocalizedMessage( 1019054 ); // You have gained a lot of fame.
				else if ( offset > 20 )
					m.SendLocalizedMessage( 1019053 ); // You have gained a good amount of fame.
				else if ( offset > 10 )
					m.SendLocalizedMessage( 1019052 ); // You have gained some fame.
				else if ( offset > 0 )
					m.SendLocalizedMessage( 1019051 ); // You have gained a little fame.
				else if ( offset < -40 )
					m.SendLocalizedMessage( 1019058 ); // You have lost a lot of fame.
				else if ( offset < -20 )
					m.SendLocalizedMessage( 1019057 ); // You have lost a good amount of fame.
				else if ( offset < -10 )
					m.SendLocalizedMessage( 1019056 ); // You have lost some fame.
				else if ( offset < 0 )
					m.SendLocalizedMessage( 1019055 ); // You have lost a little fame.
			}
		}

		public const int MinKarma = -15000;
		public const int MaxKarma =  15000;

		public static void AwardKarma( Mobile m, int offset, bool message )
		{
			if ( offset > 0 )
			{
				if ( m is PlayerMobile && ((PlayerMobile)m).KarmaLocked )
					return;

				if ( m.Karma >= MaxKarma )
					return;

				offset -= m.Karma / 100;

				if ( offset < 0 )
					offset = 0;
			}
			else if ( offset < 0 )
			{
				if ( m.Karma <= MinKarma )
					return;

				offset -= m.Karma / 100;

				if ( offset > 0 )
					offset = 0;
			}

			if ( (m.Karma + offset) > MaxKarma )
				offset = MaxKarma - m.Karma;
			else if ( (m.Karma + offset) < MinKarma )
				offset = MinKarma - m.Karma;

			bool wasPositiveKarma = ( m.Karma >= 0 );

			m.Karma += offset;

			if ( message )
			{
				if ( offset > 40 )
					m.SendLocalizedMessage( 1019062 ); // You have gained a lot of karma.
				else if ( offset > 20 )
					m.SendLocalizedMessage( 1019061 ); // You have gained a good amount of karma.
				else if ( offset > 10 )
					m.SendLocalizedMessage( 1019060 ); // You have gained some karma.
				else if ( offset > 0 )
					m.SendLocalizedMessage( 1019059 ); // You have gained a little karma.
				else if ( offset < -40 )
					m.SendLocalizedMessage( 1019066 ); // You have lost a lot of karma.
				else if ( offset < -20 )
					m.SendLocalizedMessage( 1019065 ); // You have lost a good amount of karma.
				else if ( offset < -10 )
					m.SendLocalizedMessage( 1019064 ); // You have lost some karma.
				else if ( offset < 0 )
					m.SendLocalizedMessage( 1019063 ); // You have lost a little karma.
			}

			if ( !Core.AOS && wasPositiveKarma && m.Karma < 0 && m is PlayerMobile && !((PlayerMobile)m).KarmaLocked )
			{
				((PlayerMobile)m).KarmaLocked = true;
				m.SendLocalizedMessage( 1042511, "", 0x22 ); // Karma is locked.  A mantra spoken at a shrine will unlock it again.
			}
		}

		public static string[] HarrowerTitles = new string[] { "Spite", "Opponent", "Hunter", "Venom", "Executioner", "Annihilator", "Champion", "Assailant", "Purifier", "Nullifier" };

		public static string ComputeTitle( Mobile beholder, Mobile beheld )
		{
			StringBuilder title = new StringBuilder();

			int fame = beheld.Fame;
			int karma = beheld.Karma;

			bool showSkillTitle = beheld.ShowFameTitle && ( (beholder == beheld) || (fame >= 5000) );

			/*if ( beheld.Kills >= 5 )
			{
				title.AppendFormat( beheld.Fame >= 10000 ? "The Murderer {1} {0}" : "The Murderer {0}", beheld.Name, beheld.Female ? "Lady" : "Lord" );
			}
			else*/if ( beheld.ShowFameTitle || (beholder == beheld) )
			{
				for ( int i = 0; i < m_FameEntries.Length; ++i )
				{
					FameEntry fe = m_FameEntries[i];

					if ( fame <= fe.m_Fame || i == (m_FameEntries.Length - 1) )
					{
						KarmaEntry[] karmaEntries = fe.m_Karma;

						for ( int j = 0; j < karmaEntries.Length; ++j )
						{
							KarmaEntry ke = karmaEntries[j];

							if ( karma <= ke.m_Karma || j == (karmaEntries.Length - 1) )
							{
								title.AppendFormat( beheld.Female? ke.m_FTitle : ke.m_Title, beheld.Name, beheld.Female ? "La Grande" : "Le Grand" );
								break;
							}
						}

						break;
					}
				}
			}
			else
			{
				title.Append( beheld.Name );
			}

			if( beheld is PlayerMobile && ((PlayerMobile)beheld).DisplayChampionTitle )
			{
				PlayerMobile.ChampionTitleInfo info = ((PlayerMobile)beheld).ChampionTitles;

				if( info.Harrower > 0 )
					title.AppendFormat( ": {0} of Evil", HarrowerTitles[Math.Min( HarrowerTitles.Length, info.Harrower )-1] );
				else
				{
					int highestValue = 0, highestType = 0;
					for( int i = 0; i < ChampionSpawnInfo.Table.Length; i++ )
					{
						int v = info.GetValue( i );

						if( v > highestValue )
						{
							highestValue = v;
							highestType = i;
						}
					}

					int offset = 0;
					if( highestValue > 800 )
						offset = 3;
					else if( highestValue > 300 )
						offset = (int)(highestValue/300);

					if( offset > 0 )
					{
						ChampionSpawnInfo champInfo = ChampionSpawnInfo.GetInfo( (ChampionSpawnType)highestType );
						title.AppendFormat( ": {0} of the {1}", champInfo.LevelNames[Math.Min( offset, champInfo.LevelNames.Length ) -1], champInfo.Name );
					}
				}
			}

			string customTitle = beheld.Title;

			if ( customTitle != null && (customTitle = customTitle.Trim()).Length > 0 )
			{
				title.AppendFormat( " {0}", customTitle );
			}
			else if ( showSkillTitle && beheld.Player )
			{
				string skillTitle = GetSkillTitle( beheld );

				if ( skillTitle != null ) {
					title.Append( ", " ).Append( skillTitle );
				}
			}

			return title.ToString();
		}

		public static string GetSkillTitle( Mobile mob ) {
			Skill highest = GetHighestSkill( mob );// beheld.Skills.Highest;

            if (highest != null && highest.BaseFixedPoint >= 300)
            {
                
                //string skillLevel = GetSkillLevel(highest);
                string skillLevel = (mob.Female ? FGetSkillLevel(highest) : GetSkillLevel(highest));
                string skillTitle = (mob.Female ? highest.Info.FTitle : highest.Info.Title);
                /*if ( mob.Female && skillTitle.EndsWith( "man" ) )
                    skillTitle = skillTitle.Substring( 0, skillTitle.Length - 3 ) + "woman";*/

                if(skillLevel != null && skillTitle != null)
                    return String.Concat(skillLevel, " ", skillTitle);
            }

			return null;
		}

		private static Skill GetHighestSkill( Mobile m )
		{
			Skills skills = m.Skills;

			if ( !Core.AOS )
				return skills.Highest;

			Skill highest = null;

			for ( int i = 0; i < m.Skills.Length; ++i )
			{
				Skill check = m.Skills[i];

				if ( highest == null || check.BaseFixedPoint > highest.BaseFixedPoint )
					highest = check;
				else if ( highest != null && highest.Lock != SkillLock.Up && check.Lock == SkillLock.Up && check.BaseFixedPoint == highest.BaseFixedPoint )
					highest = check;
			}

			return highest;
		}

		private static string[,] m_Levels = new string[,]
			{
				{ "Neophite",		"Neophite",		"Neophite"		},
				{ "Novice",			"Novice",		"Novice"		},
				{ "Apprenti",		"Apprenti",	"Apprenti"	},
				{ "Compagnon",		"Compagnon",	"Compagnon"	},
				{ "Expert",			"Expert",		"Expert"		},
				{ "Adepte",			"Adepte",		"Adepte"			},
				{ "Maitre",			"Maitre",		"Maitre"		},
				{ "Grand Maitre",	"Grand Maitre",	"Grand Maitre"	},
				{ "Ancien",			"Ancien",		"Ancien"		},
				{ "Legendaire",		"Legendaire",		"Legendaire"			}
			};

        private static string[,] m_FLevels = new string[,]
			{
				{ "Neophite",		"Neophite",		"Neophite"		},
				{ "Novice",			"Novice",		"Novice"		},
				{ "Apprentie",		"Apprentie",	"Apprentie"	},
				{ "Compagnon",		"Compagnon",	"Compagnon"	},
				{ "Experte",		"Experte",		"Experte"		},
				{ "Adepte",			"Adepte",		"Adepte"			},
				{ "Maitre",			"Maitre",		"Maitre"		},
				{ "Grande Maitre",	"Grande Maitre",	"Grande Maitre"	},
				{ "Ancienne",			"Ancienne",		"Ancienne"		},
				{ "Legendaire",		"Legendaire",		"Legendaire"			}
			};

		private static string GetSkillLevel( Skill skill )
		{
			return m_Levels[GetTableIndex( skill ), GetTableType( skill )];
		}

        private static string FGetSkillLevel(Skill skill)
        {
            return m_FLevels[GetTableIndex(skill), GetTableType(skill)];
        }

		private static int GetTableType( Skill skill )
		{
			switch ( skill.SkillName )
			{
				default: return 0;
				case SkillName.Bushido: return 1;
				case SkillName.Ninjitsu: return 2;
			}
		}

		private static int GetTableIndex( Skill skill )
		{
			int fp = Math.Min( skill.BaseFixedPoint, 1200 );

			return (fp - 300) / 100;
		}

		private static FameEntry[] m_FameEntries = new FameEntry[]
			{
				new FameEntry( 1249, new KarmaEntry[]
				{
					new KarmaEntry( -10000, "{0} le Proscrit", "{0} la Proscrite"  ),
					new KarmaEntry( -5000, "{0} le Meprisable", "{0} la Meprisable" ),
					new KarmaEntry( -2500, "{0} le Scelerat", "{0} la Scelerate" ),
					new KarmaEntry( -1250, "{0} le Repugnant", "{0} la Repugnante" ),
					new KarmaEntry( -625, "{0} le Rude", "{0} la Rude" ),
					new KarmaEntry( 624, "{0}", "{0}" ),
					new KarmaEntry( 1249, "{0} le Juste", "{0} la Juste" ),
					new KarmaEntry( 2499, "{0} l'Aimable", "{0} l'Aimable" ),
					new KarmaEntry( 4999, "{0} le Brave", "{0} la Brave"),
					new KarmaEntry( 9999, "{0} l'Honnete", "{0} l'Honnete" ),
					new KarmaEntry( 10000, "{0} le Fiable", "{0} la Fiable" )
				} ),
				new FameEntry( 2499, new KarmaEntry[]
				{
					new KarmaEntry( -10000, "{0} le Miserable", "{0} la Miserable" ),
					new KarmaEntry( -5000, "{0} le Lache", "{0} la Lache" ),
					new KarmaEntry( -2500, "{0} le Malicieux", "{0} la Malicieuse" ),
					new KarmaEntry( -1250, "{0} le Deshonorable", "{0} la Deshonorable" ),
					new KarmaEntry( -625, "{0} le Minable", "{0} la Minable" ),
					new KarmaEntry( 624, "{0} le Notable", "{0} la Notable" ),
					new KarmaEntry( 1249, "{0} le Droit", "{0} la Droite" ),
					new KarmaEntry( 2499, "{0} le Respectable", "{0} la Respectable" ),
					new KarmaEntry( 4999, "{0} l'Honorable", "{0} l'Honorable" ),
					new KarmaEntry( 9999, "{0} le Louable", "{0} la Louable" ),
					new KarmaEntry( 10000, "{0} l'Estimable", "{0} l'Estimable" )
				} ),
				new FameEntry( 4999, new KarmaEntry[]
				{
					new KarmaEntry( -10000, "{0} l'Infame", "{0} l'Infame" ),
					new KarmaEntry( -5000, "{0} le Cruel", "{0} la Cruelle" ),
					new KarmaEntry( -2500, "{0} le Vil", "{0} la Vile" ),
					new KarmaEntry( -1250, "{0} l'Ignoble", "{0} l'Ignoble" ),
					new KarmaEntry( -625, "{0} le Connu", "{0} la Connue" ),
					new KarmaEntry( 624, "{0} le Hautain", "{0} la Hautaine" ),
					new KarmaEntry( 1249, "{0} le Repute", "{0} la Reputee" ),
					new KarmaEntry( 2499, "{0} le Digne", "{0} la Digne" ),
					new KarmaEntry( 4999, "{0} l'Admirable", "{0} l'Admirable" ),
					new KarmaEntry( 9999, "{0} le Celebre", "{0} la Celebre" ),
					new KarmaEntry( 10000, "{0} le Grand", "{0} la Grande" )
				} ),
				new FameEntry( 9999, new KarmaEntry[]
				{
					new KarmaEntry( -10000, "{0} le Terrible", "{0} la Terrible" ),
					new KarmaEntry( -5000, "{0} le Malfaisant", "{0} la Malfaisante" ),
					new KarmaEntry( -2500, "{0} le Sordide", "{0} la Sordide" ),
					new KarmaEntry( -1250, "{0} le Tenebreux", "{0} la Tenebreuse" ),
					new KarmaEntry( -625, "{0} le Sadique", "{0} la Sadique" ),
					new KarmaEntry( 624, "{0} le Fameux", "{0} la Fameuse" ),
					new KarmaEntry( 1249, "{0} le Distingue", "{0} la Distinguee"  ),
					new KarmaEntry( 2499, "{0} l'Eminent", "{0} l'Eminente" ),
					new KarmaEntry( 4999, "{0} le Noble", "{0} la Noble" ),
					new KarmaEntry( 9999, "{0} l'Illustre", "{0} l'Illustre" ),
					new KarmaEntry( 10000, "{0} le Glorieux", "{0} la Glorieuse"  )
				} ),
				new FameEntry( 10000, new KarmaEntry[]
				{
					new KarmaEntry( -10000, "{1} {0} le Terrible", "{1} {0} la Terrible" ),
					new KarmaEntry( -5000, "{1} {0} le Malfaisant", "{1} {0} la Malfaisante" ),
					new KarmaEntry( -2500, "{1} {0} le Sombre", "{1} {0} la Sombre"),
					new KarmaEntry( -1250, "{1} {0} le Tenebreux", "{1} {0} la Tenebreuse"),
					new KarmaEntry( -625, "{1} {0} le Dechu", "{1} {0} la Dechue" ),
					new KarmaEntry( 624, "{1} {0}", "{1} {0}" ),
					new KarmaEntry( 1249, "{1} {0} le Distingue", "{1} {0} la Distinguee"  ),
					new KarmaEntry( 2499, "{1} {0} l'Eminent", "{1} {0} l'Eminente" ),
					new KarmaEntry( 4999, "{1} {0} le Noble", "{1} {0} la Noble" ),
					new KarmaEntry( 9999, "{1} {0} l'Illustre", "{1} {0} l'Illustre" ),
					new KarmaEntry( 10000, "{1} {0} le Glorieux", "{1} {0} la Glorieuse"  )
				} )
			};
	}

	public class FameEntry
	{
		public int m_Fame;
		public KarmaEntry[] m_Karma;

		public FameEntry( int fame, KarmaEntry[] karma )
		{
			m_Fame = fame;
			m_Karma = karma;
		}
	}

	public class KarmaEntry
	{
		public int m_Karma;
		public string m_Title;
        public string m_FTitle;

		public KarmaEntry( int karma, string title, string ftitle )
		{
			m_Karma = karma;
			m_Title = title;
            m_FTitle = ftitle;
		}
	}
}