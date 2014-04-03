using System;
using Server;
using Server.Misc;

namespace Server.Commands
{
    public class DelayRestart
    {
        private static Timer m_Timer = null;

        public static void Initialize()
        {
            CommandSystem.Register("dRestart", AccessLevel.Administrator, new CommandEventHandler(Core_OnCommand));
        }

        [Usage("DRestart <seconds>")]
        [Description("Redémarre le serveur dans <seconds> secondes")]
        public static void Core_OnCommand(CommandEventArgs e)
        {
            if (e.Arguments.Length == 0 || e.Arguments.Length > 1)
            {
                e.Mobile.SendMessage("Usage : DRestart <seconds>");
                return;
            }

            if (e.Arguments[0].ToLower() == "stop")
            {
                m_Timer.Stop();
                World.Broadcast(0x35, false, "Restart annulé.");
                m_Timer = null;
                Logging.RestartLog(String.Format("{0} a annulé le restart planifié", CommandLogging.Format(e.Mobile)));
                return;
            }

            if (m_Timer != null)
            {
                e.Mobile.SendMessage("Un restart est déjà programmé.");
                return;
            }

            int seconds = 0;
            try
            {
                seconds = Int32.Parse(e.Arguments[0]);
            }
            catch
            {
                e.Mobile.SendMessage("<seconds> must be an integer !");
                return;
            }

            Logging.RestartLog(String.Format("{0} a lancé restart planifié dans {1} secondes", CommandLogging.Format(e.Mobile), seconds));

            m_Timer = new InternalTimer(seconds);
            m_Timer.Start();
        }

        private class InternalTimer : Timer
        {
            DateTime m_RestartTime;
            bool forceBroadcast;

            public InternalTimer(int seconds)
                : base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
            {
                m_RestartTime = DateTime.Now + TimeSpan.FromSeconds(seconds);
                forceBroadcast = true;
            }

            protected override void OnTick()
            {
                if (DateTime.Now >= m_RestartTime)
                {
                    World.Broadcast(0x35, false, "Redémarrage du serveur...");
                    Logging.RestartLog("Redémarrage planifié");
                    Stop();
                    m_Timer = null;
                    AutoSave.Save();
                    Core.Kill(true);
                    return;
                }

                int seconds = (int)(m_RestartTime - DateTime.Now).TotalSeconds;

                string time = "";
                bool broadcast = false;

                if (seconds >= (60 * 60))
                {
                    int hours = seconds / (60 * 60);
                    int minutes = seconds / 60;
                    time = hours + " heure" + (hours > 1 ? "s" : "");
                    time += " et " + (minutes % 60) + " minute" + ((minutes % 60) > 1 ? "s" : "");
                    Priority = TimerPriority.OneMinute;

                    if (hours <= 2 && minutes % 30 == 0) broadcast = true;
                    if (minutes % 60 == 0) broadcast = true;
                }
                else if (seconds >= 60)
                {
                    int minutes = seconds / 60;
                    time = minutes + " minute" + (minutes > 1 ? "s" : "");
                    Priority = TimerPriority.OneMinute;

                    if (minutes == 1) Priority = TimerPriority.FiveSeconds;

                    if (minutes % 10 == 0) broadcast = true;
                    if (minutes < 10 && minutes >= 5 && minutes % 2 == 0) broadcast = true;
                    if (minutes < 5) broadcast = true;

                    if (Priority == TimerPriority.FiveSeconds && seconds > 61) broadcast = false;
                }
                else
                {
                    time = seconds + " seconde" + (seconds > 1 ? "s" : "");
                    Priority = TimerPriority.OneSecond;

                    if (seconds % 10 == 0) broadcast = true;
                    if (seconds <= 10 && seconds % 2 == 0) broadcast = true;
                    if (seconds <= 5) broadcast = true;
                }

                if(broadcast && !forceBroadcast) World.Broadcast(0x35, false, "Le serveur va redémarrer dans " + time);
                else if (forceBroadcast)
                {
                    World.Broadcast(0x35, false, "Un redémarrage du serveur a été planifié dans " + time);
                    forceBroadcast = false;
                }
            }
        }
    }
}
