using System;
using System.IO;
using Server;
using Server.Commands;

namespace Server.Misc
{
    public class AutoSave : Timer
    {
        private static TimeSpan m_Delay = TimeSpan.FromMinutes(15.0);   // Scriptiz : default is 5 minutes
        private static TimeSpan m_Warning = TimeSpan.Zero;
        //private static TimeSpan m_Warning = TimeSpan.FromSeconds( 15.0 );

        public static void Initialize()
        {
            new AutoSave().Start();
            CommandSystem.Register("SetSaves", AccessLevel.Administrator, new CommandEventHandler(SetSaves_OnCommand));
        }

        private static bool m_SavesEnabled = true;

        public static bool SavesEnabled
        {
            get { return m_SavesEnabled; }
            set { m_SavesEnabled = value; }
        }

        [Usage("SetSaves <true | false>")]
        [Description("Enables or disables automatic shard saving.")]
        public static void SetSaves_OnCommand(CommandEventArgs e)
        {
            if (e.Length == 1)
            {
                m_SavesEnabled = e.GetBoolean(0);
                e.Mobile.SendMessage("Saves have been {0}.", m_SavesEnabled ? "enabled" : "disabled");
            }
            else
            {
                e.Mobile.SendMessage("Format: SetSaves <true | false>");
            }
        }

        public AutoSave()
            : base(m_Delay - m_Warning, m_Delay)
        {
            Priority = TimerPriority.OneMinute;
        }

        protected override void OnTick()
        {
            if (!m_SavesEnabled || AutoRestart.Restarting)
                return;

            if (m_Warning == TimeSpan.Zero)
            {
                Save(true);
            }
            else
            {
                int s = (int)m_Warning.TotalSeconds;
                int m = s / 60;
                s %= 60;

                if (m > 0 && s > 0)
                    World.Broadcast(0x35, true, "Sauvegarde du monde dans {0} minute{1} et {2} seconde{3}.", m, m != 1 ? "s" : "", s, s != 1 ? "s" : "");
                else if (m > 0)
                    World.Broadcast(0x35, true, "Sauvegarde du monde dans {0} minute{1}.", m, m != 1 ? "s" : "");
                else
                    World.Broadcast(0x35, true, "Savegarde du monde dans {0} seconde{1}.", s, s != 1 ? "s" : "");

                Timer.DelayCall(m_Warning, new TimerCallback(Save));
            }
        }

        public static void Save()
        {
            AutoSave.Save(false);
        }

        public static void Save(bool permitBackgroundWrite)
        {
            if (AutoRestart.Restarting)
                return;

            World.WaitForWriteCompletion();

            try { Backup(); }
            catch (Exception e) { Console.WriteLine("WARNING: Automatic backup FAILED: {0}", e); }

            World.Save(true, permitBackgroundWrite);
        }

        private static string[] m_Backups = new string[]
			{
                // Scriptiz : more backup (12 * 15minutes = 3 hours)
                "Twelfth Backup",
                "Eleventh Backup",
                "Tenth Backup",
                "Ninth Backup",
                "Eighth Backup",
                "Seventh Backup",
                "Sixth Backup",
                "Fifth Backup",
                "Fourth Backup",

				"Third Backup",
				"Second Backup",
				"Most Recent"
			};

        private static void Backup()
        {
            // Scriptiz : two backups a day (AM & PM)
            DailyBackup();

            if (m_Backups.Length == 0)
                return;

            string root = Path.Combine(Core.BaseDirectory, "Backups/Automatic");

            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);

            string[] existing = Directory.GetDirectories(root);

            for (int i = 0; i < m_Backups.Length; ++i)
            {
                DirectoryInfo dir = Match(existing, m_Backups[i]);

                if (dir == null)
                    continue;

                if (i > 0)
                {
                    string timeStamp = FindTimeStamp(dir.Name);

                    if (timeStamp != null)
                    {
                        try { dir.MoveTo(FormatDirectory(root, m_Backups[i - 1], timeStamp)); }
                        catch { }
                    }
                }
                else
                {
                    try { dir.Delete(true); }
                    catch { }
                }
            }

            string saves = Path.Combine(Core.BaseDirectory, "Saves");

            if (Directory.Exists(saves))
                Directory.Move(saves, FormatDirectory(root, m_Backups[m_Backups.Length - 1], GetTimeStamp()));
        }


        // Scriptiz : daily backup (AM & PM)
        private static void DailyBackup()
        {
            string saves = Path.Combine(Core.BaseDirectory, "Saves");

            if (!Directory.Exists(saves))
                return;

            string dailyDir = Path.Combine(Core.BaseDirectory, "Backups/Daily");

            if (!Directory.Exists(dailyDir))
                Directory.CreateDirectory(dailyDir);

            // Creating daily backup
            DateTime now = DateTime.Now;
            string todayStr = String.Format("{0}-{1}-{2}-{3}", now.Year, now.Month, now.Day, (now.Hour < 12 ? "AM" : "PM"));
            string todayDir = Path.Combine(dailyDir, todayStr);

            if (!Directory.Exists(todayDir))
            {
                Console.WriteLine("Creating daily backup {0}", todayStr);
                Directory.CreateDirectory(todayDir);

                CopyAll(new DirectoryInfo(saves), new DirectoryInfo(todayDir));
            }

            // Deleting old backup
            DateTime del = now.Subtract(TimeSpan.FromDays(8));
            string delStr = String.Format("{0}-{1}-{2}-{3}", del.Year, del.Month, del.Day, (del.Hour < 12 ? "AM" : "PM"));
            string delDir = Path.Combine(dailyDir, delStr);

            if (Directory.Exists(delDir))
            {
                // Scriptiz : on garde les saves des lundis matin
                if (del.DayOfWeek == DayOfWeek.Monday && del.Hour < 12)
                    return;

                Console.WriteLine("Deleting old daily backup {0}", delStr);

                Directory.Delete(delDir, true);
            }
        }

        // Scriptiz : Copy Directory And Its Content To Another Directory
        private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it’s new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        private static DirectoryInfo Match(string[] paths, string match)
        {
            for (int i = 0; i < paths.Length; ++i)
            {
                DirectoryInfo info = new DirectoryInfo(paths[i]);

                if (info.Name.StartsWith(match))
                    return info;
            }

            return null;
        }

        private static string FormatDirectory(string root, string name, string timeStamp)
        {
            return Path.Combine(root, String.Format("{0} ({1})", name, timeStamp));
        }

        private static string FindTimeStamp(string input)
        {
            int start = input.IndexOf('(');

            if (start >= 0)
            {
                int end = input.IndexOf(')', ++start);

                if (end >= start)
                    return input.Substring(start, end - start);
            }

            return null;
        }

        private static string GetTimeStamp()
        {
            DateTime now = DateTime.Now;

            return String.Format("{0}-{1}-{2} {3}-{4:D2}-{5:D2}",
                    now.Day,
                    now.Month,
                    now.Year,
                    now.Hour,
                    now.Minute,
                    now.Second
                );
        }
    }
}