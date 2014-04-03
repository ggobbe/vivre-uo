using System;
using Server;
using Server.Commands;
using Server.Misc;
using Server.Network;

namespace Server.Commands
{ 
   public class RestartSoon
   { 
      public static void Initialize()
      { 
         CommandSystem.Register( "RestartSoon", AccessLevel.GameMaster, new CommandEventHandler( RestartSoon_OnCommand ) ); 
      } 

      [Usage( "RestartSoon" )] 
      [Description( "Restart dès qu'il n'y a plus personne" )] 
      public static void RestartSoon_OnCommand( CommandEventArgs e ) 
      { 
      	if(timer!=null)
      	{
      		e.Mobile.SendMessage("Le restart est déjà programmé.");
      	}
      	else 
      	{
      		timer = new RestartSoonTimer(e);
      		timer.Start();
      		e.Mobile.SendMessage("Restart programmé.");
      	}
      }
      
      private static RestartSoonTimer timer;
      
      private class RestartSoonTimer: Timer
      {
      	private CommandEventArgs m_e;
      	
      	public RestartSoonTimer(CommandEventArgs e): base( TimeSpan.FromMinutes( 1 ),TimeSpan.FromMinutes( 1 ) )
      	{
      		m_e = e;
      	}
      	
      	protected override void OnTick()
      	{
      		if(NetState.Instances.Count==0)
      		{
                World.Broadcast(0x35, false, "Redémarrage du serveur car plus personne en ligne !");
      			AutoRestart.Restart_OnCommand( m_e );
      		}
      	}
      }
   }
}
         
