using System;
using Server;

namespace Server.Misc
{
	public class Broadcasts
	{
		public static void Initialize()
		{
			EventSink.Crashed += new CrashedEventHandler( EventSink_Crashed );
			EventSink.Shutdown += new ShutdownEventHandler( EventSink_Shutdown );
		}

		public static void EventSink_Crashed( CrashedEventArgs e )
		{
			try
			{
				World.Broadcast( 0x35, false, "Le serveur s'est brusquement coupé." );
			}
			catch
			{
			}
		}

		public static void EventSink_Shutdown( ShutdownEventArgs e )
		{
			try
			{
				World.Broadcast( 0x35, false, "Le serveur s'est éteint." );
			}
			catch
			{
			}
		}
	}
}