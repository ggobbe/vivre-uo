using System;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.ServerSeasons;

namespace Server.Commands
{
    public class SeasonCommand
    {

        public static void Initialize()
        {
            CommandSystem.Register("saison", AccessLevel.Player, new CommandEventHandler(Saison_OnCommand));
        }

        [Usage("saison")]
        [Description("Informe le joueur de la saison actuelle")]
        public static void Saison_OnCommand(CommandEventArgs e)
        {
            string season = "la désolation";
            switch (e.Mobile.Map.Season)
            {
                case (int)Season.Spring:
                    season = "le printemps";
                    break;
                case (int)Season.Summer:
                    season = "l'été";
                    break;
                case (int)Season.Autumn:
                    season = "l'automne";
                    break;
                case (int)Season.Winter:
                    season = "l'hiver";
                    break;
            }

            e.Mobile.SendMessage(String.Format("C'est {0}", season));
        }
    }
}