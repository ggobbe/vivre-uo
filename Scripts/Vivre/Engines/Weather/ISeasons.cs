//To enable RUNUO 2.0 compatibility, simply comment out the next line (#define);
//#define RUNUO_1

using System;
using System.Collections;
using System.Collections.Generic;

using Server;

namespace Server.ServerSeasons
{
	public enum Season
	{
		Spring = 0,
		Summer = 1,
		Autumn = 2,
		Winter = 3,
		Desolation = 4,
		Fall = Autumn,
	}

	public interface ISeasons
	{
		Season Season { get; set; }
		void UpdateSeason();
		void UpdateSeason(Mobile m);
	}
}