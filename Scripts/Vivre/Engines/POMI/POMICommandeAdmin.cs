using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Gumps;
using Server.IPOMI;
using Server.Targeting;
using System.Collections;

namespace Server.Commands
{

	/// <summary>
	/// /////////////////////////////////////////  EFFACEMENT VILLE CONTENU DANS POMI V1.0  /////////////   M. FREEZE  V1.0
	/// </summary>
	public class EffaceVillePOMI
	{
		public static void Initialize()
		{
			CommandSystem.Register( "EffaceVillePOMI", AccessLevel.Administrator, new CommandEventHandler( EffaceVillePOMI_OnCommand ) );
		}

		[Usage( "EffaceVillePOMI <index de la ville>" )]
		[Description( "Efface une ville contenu dans la pierre POMI par son Index, verifiez avec .Adminpomi" )]
		public static void EffaceVillePOMI_OnCommand( CommandEventArgs e )
		{

			string index = e.ArgString.Trim();
			
			if ( (index.Length > 0) && (index.Length < 2) )
			{	
				Mobile from = e.Mobile;
				from.SendMessage("Visez une PIERRE POMI");
				from.Target = new EffaceVillePOMITarget(index);
			}
			else
				e.Mobile.SendMessage( "Usage: EffaceVillePOMI  <index>" );
		}
	}

	public class EffaceVillePOMITarget : Target
	{
		private string m_index;
		private string text;
		public EffaceVillePOMITarget(string index) : base( -1, false, TargetFlags.None )
		{
			m_index = index;
		}

		protected override void OnTarget( Mobile mobile, object targeted )
		{
			PlayerMobile from = (PlayerMobile) mobile;
			if ( targeted is POMI )
			{	
					POMI cible = (POMI)targeted;
					int test = Utility.ToInt32(m_index);

					Console.WriteLine("test = " + test +"      index = "+m_index);
					if ( (test >=0) && ( test < (cible.Villes.Count)))
					{
						//((POMI)targeted).Villes.Remove(test);
						cible.Villes.RemoveAt(test);

					}
			}
			else
				from.SendMessage("CECI N'EST PAS UNE PIERRE POMI !");
			
		}
	}

/// <summary>
/// /////////////////////////////////////////    AFFICHAGE DES VILLES CONTENU DANS POMI    ////////     M.FREEZE  V1.0
/// </summary>

   public class AdminPOMI
   {
      public static void Initialize()
      {
         CommandSystem.Register( "AdminPOMI", AccessLevel.Administrator, new CommandEventHandler( AdminPOMI_OnCommand ) );
      }

      [Usage( "AdminPOMI" )]
      [Description( "Affiche les villes contenues dans POMI" )]
      public static void AdminPOMI_OnCommand( CommandEventArgs e )
      {
         Mobile from = e.Mobile;
		 from.SendMessage("Visez une PIERRE POMI");
		 from.Target = new AdminPOMITarget();
         

      }
   }

	public class AdminPOMITarget : Target
	{

		public AdminPOMITarget( ) : base( -1, false, TargetFlags.None )
		{
		}

		protected override void OnTarget( Mobile mobile, object targeted )
		{
			PlayerMobile from = (PlayerMobile) mobile;
			if ( targeted is POMI )
			{					
				from.SendGump( new gumpAdminPOMI ( from , (POMI)targeted  ) );
			}
			else
				from.SendMessage("CECI N'EST PAS UNE PIERRE POMI !");
			
		}
	}
}

namespace Server.Gumps
{
	public class gumpAdminPOMI : Gump
	{

		public gumpAdminPOMI(Mobile from, POMI cible) : base(0,0)
		{
			Closable = true;
			Dragable = true;
			int i = 50;
			int j = 0;
	//		POMI Pomicible = (POMI)cible;
			AddPage(0);

			AddBackground( 0, 0, 295, 400, 5054);
			AddBackground( 15, 15, 265, 370, 3500);
            AddLabel( 100, 30, 0, string.Format( "VILLES POMI"));
			if (cible.Villes.Count <= 0)
			{
				from.SendMessage("Il n'y a pas de ville sur cette pierre POMI! [Count = " + cible.Villes.Count +" ]");
				
			}
			else
			{
				foreach(TownStone ville in cible.Villes)
				{
					AddLabel( 30 ,(10+i), 0, (j +"- " +ville.Name) );
					i=i+15;
					j++;
				}
			}
		}

		public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
		{

		}
	}
}

