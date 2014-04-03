using System;
using System.Net;

namespace Server.Misc
{
    public class IPRestarter
    {
        private static Timer m_IPTimer;

        public static void Initialize()
        {
            m_IPTimer = new IPCheckTimer();
            m_IPTimer.Start();
        }

        private class IPCheckTimer : Timer
        {
            private string m_Adress;

            public IPCheckTimer()
                : base(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1))
            {
            	IPAddress ip = ServerList.FindPublicAddress();
            	
            	// On ne lance pas le timer si pas d'adresse IP trouvée sur internet
            	if(ip == null)
            	{
            		Console.WriteLine("Adresse IP introuvable, êtes vous connecté à internet?");
            		Console.WriteLine("IPRestarter désactivé car pas d'adresse IP.");
            		Stop();
            	} 
            	else
            	{
                	m_Adress = ServerList.FindPublicAddress().ToString();
            	}
            }

            protected override void OnTick()
            {
            	// Si pas d'IP enregistrée on arrête le timer
            	if(m_Adress == null)
            	{
            		Stop();
            		return;
            	}
            	
            	// Si on ne trouve pas l'ip, on ne reboot pas et on continue le timer.
            	IPAddress ip = ServerList.FindPublicAddress();
            	if(ip == null)
            	{
            		Console.WriteLine("Adresse IP introuvable, vérifiez votre connexion à internet.");
            		return;
            	}
            	
            	// Si adresse IP changée et personne en ligne, on reboot
                if (m_Adress != ip.ToString() && Network.NetState.Instances.Count == 0)
                {
                    World.Broadcast(0x35, false, "Changement d'adresse IP détecté, redémarrage en cours...");
                    Logging.RestartLog("Redémarrage du serveur car changement d'adresse IP détecté");
                    AutoSave.Save();
                    Core.Kill(true);
                }
            }
        }
    }
}
