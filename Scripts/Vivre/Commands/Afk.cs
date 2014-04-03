using Server.Commands;
using Server.Accounting;
using Server.Network;
using System;
using System.Collections;
using Server;

namespace Server.Commands
{
    /// <summary>
    /// Summary description for AFK.
    /// </summary>
    public class AFK : Timer
    {
        // Scriptiz : Minutes before we kick the AFK players (0 = no kick)
        private int kickTime = 5;

        private static Hashtable m_AFK = new Hashtable();
        private Mobile who;
        private Point3D where;
        private DateTime when;
        public string what = "";

        public static void Initialize()
        {
            CommandSystem.Register("afk", AccessLevel.Player, new CommandEventHandler(AFK_OnCommand));
            EventSink.Logout += new LogoutEventHandler(OnLogout);
            EventSink.Speech += new SpeechEventHandler(OnSpeech);
            EventSink.PlayerDeath += new PlayerDeathEventHandler(OnDeath);
        }
        public static void OnDeath(PlayerDeathEventArgs e)
        {
            if (m_AFK.Contains(e.Mobile.Serial.Value))
            {
                AFK afk = (AFK)m_AFK[e.Mobile.Serial.Value];
                if (afk == null)
                {
                    e.Mobile.SendMessage("L'objet AFK est manquant!");
                    return;
                }
                e.Mobile.PlaySound(e.Mobile.Female ? 814 : 1088);
                afk.wakeUp();
            }
        }
        public static void OnLogout(LogoutEventArgs e)
        {
            if (m_AFK.Contains(e.Mobile.Serial.Value))
            {
                AFK afk = (AFK)m_AFK[e.Mobile.Serial.Value];
                if (afk == null)
                {
                    e.Mobile.SendMessage("L'objet AFK est manquant!");
                    return;
                }
                afk.wakeUp();
            }
        }
        public static void OnSpeech(SpeechEventArgs e)
        {
            if (m_AFK.Contains(e.Mobile.Serial.Value))
            {
                AFK afk = (AFK)m_AFK[e.Mobile.Serial.Value];
                if (afk == null)
                {
                    e.Mobile.SendMessage("L'objet AFK est manquant!");
                    return;
                }
                afk.wakeUp();
            }
        }
        public static void AFK_OnCommand(CommandEventArgs e)
        {
            if (m_AFK.Contains(e.Mobile.Serial.Value))
            {
                AFK afk = (AFK)m_AFK[e.Mobile.Serial.Value];
                if (afk == null)
                {
                    e.Mobile.SendMessage("L'objet AFK est manquant!");
                    return;
                }
                afk.wakeUp();
            }
            else
            {
                m_AFK.Add(e.Mobile.Serial.Value, new AFK(e.Mobile, e.ArgString.Trim()));
                e.Mobile.SendMessage("AFK activé.");
                e.Mobile.Emote("*est AFK*");
            }
        }
        public void wakeUp()
        {
            m_AFK.Remove(who.Serial.Value);
            who.Emote("*n'est plus AFK*");
            who.SendMessage("AFK désactivé.");
            this.Stop();
        }
        public AFK(Mobile afker, string message)
            : base(TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(15))
        {
            if ((message == null) || (message == "")) message = "est AFK";
            what = message;
            who = afker;
            when = DateTime.Now;
            where = who.Location;
            this.Start();
        }
        protected override void OnTick()
        {
            #region KickThemAll
            // Scriptiz : Let's kick all theses junkies who are stealing our bandwidth !
            if ((this.kickTime != 0) && (DateTime.Now.Subtract(when).CompareTo(TimeSpan.FromMinutes(this.kickTime)) > 0))
            {
                NetState kicked = who.NetState;

                who.SendMessage("Vous êtes absent depuis trop longtemps, le serveur vous déconnecte.");

                if (kicked != null)
                {
                    this.wakeUp();
                    kicked.Dispose();
                }
            }
            #endregion

            if (!(who.Location == where))
            {
                this.wakeUp();
                return;
            }
            who.Say("zZz");
            //TimeSpan ts = DateTime.Now.Subtract(when);
            //who.Emote("*{0} ({1}:{2}:{3})*",what,ts.Hours,ts.Minutes,ts.Seconds);
            //who.PlaySound(  who.Female ? 819 : 1093);
        }
    }
}