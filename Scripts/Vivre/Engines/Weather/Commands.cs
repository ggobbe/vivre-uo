using System;
using System.Collections;
using System.Collections.Generic;

using Server;
using Server.Regions;
using Server.Commands;
using Server.Network;

namespace Server.ServerSeasons
{
	public class RegionCommands
	{
		public static void Initialize()
		{
			CommandSystem.Register("SetMapSeason", AccessLevel.Administrator, new CommandEventHandler(SetMapSeason_OnCommand));
			CommandSystem.Register("SetSeason", AccessLevel.GameMaster, new CommandEventHandler(SetSeason_OnCommand));
		}

		[Description("Sets the Season for the current region.")]
		private static void SetSeason_OnCommand(CommandEventArgs e)
		{
			Mobile from = e.Mobile;

			if (e.Length >= 1)
			{
				Region reg = from.Region;

				if (reg == null || !(reg is ISeasons))
				{
					from.SendMessage("You are not in a region that supports Seasons.");
					from.SendMessage("To set the Map Season, use {0}SetMapSeason", CommandSystem.Prefix);
				}
				else
				{
					try
					{
						ISeasons sreg = reg as ISeasons;

						sreg.Season = (Season)Enum.Parse(typeof(Season), (e.GetString(0).Trim()), true);
						from.SendMessage("Season has been set to {0}.", sreg.Season.ToString());
					}
					catch
					{
						from.SendMessage("Format: SetSeason < Spring | Summer | Autumn/Fall | Winter | Desolation >");
					}
				}
			}
			else
			{
				from.SendMessage("Format: SetSeason < Spring | Summer | Autumn/Fall | Winter | Desolation >");
			}
		}

		[Description("Sets the Season for the current map.")]
		private static void SetMapSeason_OnCommand(CommandEventArgs e)
		{
			Mobile from = e.Mobile;

			if (e.Length >= 1)
			{
				Map map = from.Map;

				if (map == null || map == Map.Internal)
				{
					from.SendMessage("Could not set the Season for this map.");
				}
				else
				{
					try
					{
						map.Season = (int)((Season)Enum.Parse(typeof(Season), (e.GetString(0).Trim()), true));
						from.SendMessage("Season has been set to {0}.", ((Season)map.Season).ToString());

                        foreach (NetState state in NetState.Instances)
                        {
                            Mobile m = state.Mobile;
                            if (m != null)
                            {
                                state.Send(SeasonChange.Instantiate(m.GetSeason(), true));
                                m.SendEverything();
                            }
                        }
					}
					catch
					{
						from.SendMessage("Format: SetMapSeason < Spring | Summer | Autumn/Fall | Winter | Desolation >");
					}
				}
			}
			else
			{
				from.SendMessage("Format: SetMapSeason < Spring | Summer | Autumn/Fall | Winter | Desolation >");
			}
		}
	}
}