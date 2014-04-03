using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Server;
using Server.Regions;
using Server.Network;

namespace Server.ServerSeasons
{
    public class SeasonManager : Timer
    {
        public static void Initialize()
        {
            new SeasonManager().Start();
        }

        public SeasonManager()
            : base(TimeSpan.FromSeconds(10), TimeSpan.FromHours(12))
        {
            Priority = TimerPriority.FiveSeconds;
        }

        protected override void OnTick()
        {
            UpdateSeasons();
            Priority = TimerPriority.OneMinute;
        }

        public static int GetWeekNumber(DateTime dtPassed)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(dtPassed, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }

        public static void UpdateSeasons()
        {
            // Define the actual season 
            Season actualSeason = Season.Summer;

            // Scriptiz : saisons basées sur un cycle 
            int week = GetWeekNumber(DateTime.Today);
            switch (week % 8)
            {
                case 0:
                case 1:
                    actualSeason = Season.Winter;
                    break;
                case 2:
                case 3:
                    actualSeason = Season.Spring;
                    break;
                case 4:
                case 5:
                    actualSeason = Season.Summer;
                    break;
                case 6:
                case 7:
                    actualSeason = Season.Autumn;
                    break;
                default:
                    actualSeason = Season.Summer;
                    break;
            }

            if (week > 49) actualSeason = Season.Winter;

            // Scriptiz : saisons basées sur les saisons IRL
            /*
            int month = DateTime.Today.Month;
            switch (month)
            {
                case 12:
                case 1:
                case 2:
                    actualSeason = Season.Winter;
                    break;
                case 3:
                case 4:
                case 5:
                    actualSeason = Season.Spring;
                    break;
                case 6:
                case 7:
                case 8:
                    actualSeason = Season.Summer;
                    break;
                case 9:
                case 10:
                case 11:
                    actualSeason = Season.Autumn;
                    break;
                default:
                    actualSeason = Season.Summer;
                    break;
            }
            */
			Console.WriteLine("[SeasonManager] Actual season is " + actualSeason + ".");

            int count = 0;
            foreach (Map m in Map.Maps)
            {
                if (m == null || m == Map.Internal) continue;
                
                // Si c'est la felucca d'origine on laisse Desolation !
                if (m == Map.Felucca) m.Season = (int)Season.Desolation;
                else m.Season = (int)actualSeason;
                
                count++;
            }
            Console.WriteLine("Season updated for " + count + " on " + Map.Maps.Length + " maps.");

            count = 0;
            foreach (Region r in Region.Regions)
            {
                if (r == null || !(r is ISeasons)) continue;
                ISeasons s = r as ISeasons;
                s.Season = actualSeason;
                count++;
            }
            Console.WriteLine("Season updated for " + count + " on " + Region.Regions.Count + " regions.");
        }
    }
}