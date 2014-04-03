using System;
using System.IO;
using System.Text;
using Server;
using Server.Network;
using Server.Guilds;
using Server.Mobiles;
using Server.Items;
using System.Globalization;

namespace Server.Misc
{
	public class StatusPage : Timer
	{
		public static bool Enabled = true;

		public static void Initialize()
		{
			if ( Enabled )
				new StatusPage().Start();
		}

		public StatusPage() : base( TimeSpan.FromSeconds( 5.0 ), TimeSpan.FromSeconds( 60.0 ) )
		{
			Priority = TimerPriority.FiveSeconds;
		}

		private static string Encode( string input )
		{
			StringBuilder sb = new StringBuilder( input );

			sb.Replace( "&", "&amp;" );
			sb.Replace( "<", "&lt;" );
			sb.Replace( ">", "&gt;" );
			sb.Replace( "\"", "&quot;" );
			sb.Replace( "'", "&apos;" );

			return sb.ToString();
		}

        public static string FormatTimeSpan(TimeSpan ts)
        {
            StringBuilder sb = new StringBuilder();
            int days = ts.Days;
            int hours = ts.Hours % 24;
            int minutes = ts.Minutes % 60;
            int seconds = ts.Seconds % 60;

            if (days > 0) sb.Append(days + " jour" + (days > 1 ? "s " : " "));
            if (hours > 0) sb.Append(hours + " heure" + (hours > 1 ? "s " : " "));
            if (minutes > 0) sb.Append(minutes + " minute" + (minutes > 1 ? "s " : " "));
            if (seconds > 0) sb.Append(seconds + " seconde" + (seconds > 1 ? "s " : " "));

            return sb.ToString();
        }

        protected override void OnTick()
        {
            if (!Directory.Exists("web"))
                Directory.CreateDirectory("web");

            using (StreamWriter op = new StreamWriter("web/index.html"))
            {
                op.WriteLine("<!DOCTYPE html>");
                op.WriteLine("<html lang=\"fr\">");
                op.WriteLine("<head><meta charset=\"utf-8\" />");
                op.WriteLine("<meta http-equiv=\"refresh\" content=\"90\" />");
                op.WriteLine("<title>Vivre Server Status</title>");
                op.WriteLine("<link rel=\"stylesheet\" href=\"style.css\" type=\"text/css\" />");
                op.WriteLine("<link href=\"favicon.ico\" rel=\"shortcut icon\" type=\"image/x-icon\" /> ");
                op.WriteLine("</head>");
                op.WriteLine("<body>");
                op.WriteLine("<h1>Vivre Server Status</h1>");

                op.WriteLine("<p id=\"header\">Serveur en ligne depuis : " +FormatTimeSpan(DateTime.Now - Clock.ServerStart) + "<br />");
                op.WriteLine("Nombre de mobiles : " + World.Mobiles.Count + "<br />");
                op.WriteLine("Nombre d'objets : " + World.Items.Count + "<br /><br />");

                // Scriptiz : ne comptons pas les GM connectés et cachés du statut
                int hidden = 0;
                foreach (NetState ns in Network.NetState.Instances)
                {
                    if (ns != null && ns.Mobile != null)
                    {
                        PlayerMobile pm = ns.Mobile as PlayerMobile;
                        if (pm != null && pm.AccessLevel > AccessLevel.Player && !pm.ShowInStatus) 
                            hidden++;
                    }
                }
                op.WriteLine("Joueurs en ligne : " + (Network.NetState.Instances.Count - hidden) + "<br /></p>");
                //op.WriteLine("Joueurs en ligne : " + Network.NetState.Instances.Count + "<br /></p>");

                op.WriteLine("<table id=\"players\">");
                op.WriteLine("<tr class=\"titleRow\">");
                op.WriteLine("<th>Nom</font></th><th>R&eacute;gion</th><th>Temps connect&eacute;</th>");
                op.WriteLine("</tr>");

                bool alter = false;
                foreach (NetState state in NetState.Instances)
                {
                    Mobile m = state.Mobile;

                    if (m != null)
                    {
                        PlayerMobile pm = m as PlayerMobile;
                        if (pm != null && !pm.ShowInStatus)
                            continue;

                        Guild g = m.Guild as Guild;

                        string cssClass = (alter ? "lightRow" : "darkRow");
                        alter = !alter;
                        op.Write("         <tr class=\"" + cssClass + "\"><td>");

                        if (g != null)
                        {
                            op.Write(Encode(m.Name));
                            op.Write(" [");

                            string title = m.GuildTitle;

                            if (title != null)
                                title = title.Trim();
                            else
                                title = "";

                            if (title.Length > 0)
                            {
                                op.Write(Encode(title));
                                op.Write(", ");
                            }

                            op.Write(Encode(g.Abbreviation));

                            op.Write(']');
                        }
                        else
                        {
                            op.Write(Encode(m.Name));
                        }

                        op.Write("</td><td>");

                        // Scriptiz : on affiche la région et non la position si c'est un joueur
                        if (m.AccessLevel == AccessLevel.Player && m.Region != null)
                            if (m.Region.Name != "") op.Write(Encode(m.Region.Name));
                            else op.Write(Encode(m.Map.Name));
                        else
                        {
                            switch (Utility.Random(7))
                            {
                                case 0: case 1: case 2:
                                    op.Write("Inconnue");
                                    break;
                                case 3: case 4: case 5:
                                    op.Write("Green Acres");
                                    break;
                                case 6:
                                    op.Write("Dans ton dos");
                                    break;
                            }
                        }

                        op.WriteLine("</td><td>");

                        // Scriptiz : affichage du temps connecté
                        if(pm != null)
                        {
                            TimeSpan sessionTime = DateTime.Now.Subtract(pm.SessionStart);

                            string time = "";
                            if (sessionTime.TotalMinutes < 60)
                            {
                                int minutes = (int)sessionTime.TotalMinutes;
                                time = minutes + (minutes > 1 ? " minutes" : " minute");
                            }
                            else
                            {
                                int hours = (int)sessionTime.TotalHours;
                                time = hours + (hours > 1 ? " heures" : " heure");
                            }

                            op.Write(time);
                        }

                        op.WriteLine("</td></tr>");
                    }
                }

                //op.WriteLine("         <tr>");
                op.WriteLine("      </table>");

                // Scriptiz : on affiche l'heure QC et FR
                DateTime quebecTime = DateTime.UtcNow.Subtract(TimeSpan.FromHours(5));
                DateTime franceTime = DateTime.UtcNow.AddHours(1);
                
                // Scriptiz : Heure d'été
                if (TimeZone.CurrentTimeZone.IsDaylightSavingTime(DateTime.Now))
                {
                    quebecTime = quebecTime.AddHours(1);
                    franceTime = franceTime.AddHours(1);
                }

                op.WriteLine("<p id=\"footer\">");
                op.Write("G&eacute;n&eacute;r&eacute; le " + quebecTime.ToShortDateString() + " &agrave; " + quebecTime.ToShortTimeString());
                op.Write(" (Qu&eacute;bec)");
                op.WriteLine("<br />");
                op.Write("G&eacute;n&eacute;r&eacute; le " + franceTime.ToShortDateString() + " &agrave; " + franceTime.ToShortTimeString());
                op.Write(" (France)");
                op.WriteLine("</p>");

                // Scriptiz : graphe indiquant les clients connectés sur le temps
                op.WriteLine("<p style=\"margin-bottom:0px;\"><br />");
                op.WriteLine("<img src=\"http://runuo.vivre-uo.fr/stats/graphs_clients_ot.png\" alt=\"clients_ot\" width=\"600\" /></p>");
                op.WriteLine("   </body>");
                op.WriteLine("</html>");
            }
        }
	}
}