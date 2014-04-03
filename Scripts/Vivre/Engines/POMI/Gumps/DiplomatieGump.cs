using System; 
using System.Collections; 
using Server.Network; 
using Server.Prompts; 
using Server.Items; 
using Server.Gumps; 
using Server; 
using Server.Regions;
using Server.Accounting;
using Server.Mobiles;
using Server.HuePickers;

namespace Server.IPOMI
{ 
    public class DiplomatieGump : Gump 
    { 
    	private TownStone m_Town;
		
        public DiplomatieGump( PlayerMobile from, TownStone town) : base( 20, 30 ) 
        { 
            m_Town = town;
	    	
            AddPage( 0 ); 

            AddBackground( 0, 0, 420, 430, 5054 ); 
            AddBackground( 10, 10, 400, 410, 3000 ); 
 	    	AddLabel( 160, 10, 0, "Diplomatie");
            AddLabel( 130 + ((143 - (town.Nom.Length * 8)) / 2), 30, 0, town.Nom );
        	
        	AddButton( 20, 70,  4005, 4007, 0, GumpButtonType.Page, 10 );
        	AddLabel( 55, 70, 0, "Alliés");
        	AddButton( 120, 70, 4005, 4007, 0, GumpButtonType.Page, 20 );
        	AddLabel( 155, 70, 0, "Paix");
        	AddButton( 220, 70, 4005, 4007, 0, GumpButtonType.Page, 30 );
        	AddLabel( 255, 70, 0, "Guerre");
        	AddButton( 320, 70, 4005, 4007, 0, GumpButtonType.Page, 40 );
        	AddLabel( 355, 70, 0, "Neutre");
        	
        	AddLabel( 335, 390, 0,"Retour" );  // Quitter 
            AddButton( 300, 390, 0xFAE, 0x0FB0, 1, GumpButtonType.Reply, 0 ); 

        	AjoutePage(1, m_Town.Allies);
        	AjoutePage(2, m_Town.Paix);
        	AjoutePage(3, m_Town.Guerre);
        	AjoutePage(4, m_Town.Neutre);
        }
		
		private void AjoutePage(int index, ArrayList status)
		{
			int i = 0;
        	int x = 0;
        	int page = 0;
        	int nb_pages = status.Count / 36;
        	
			AddPage( index * 10 );
        	switch(index)
        		{
        			case 1 : 
        				{
        					if(status.Count == 0 )
        						AddLabel( 55, 120, 0, "Aucune Ville Alliée" );
        					else
        						AddLabel( 55, 120, 0, "Ville(s) Alliée(s)" );
        					break;
        				}
        			case 2 : 
        				{
        					if(status.Count == 0 )
        						AddLabel( 55, 120, 0, "Aucune Ville en Paix");
        					else
        						AddLabel( 55, 120, 0, "Ville(s) en Paix" );
        					break;
        				}
        			case 3 : 
        				{
        					if(status.Count == 0 )
        						AddLabel( 55, 120, 0, "Aucune Ville en Guerre");
        					else
        						AddLabel( 55, 120, 0, "Ville(s) en Guerre" );
        					break;
        				}
        			case 4 : 
        				{
        					if(status.Count <= 1 )
        						AddLabel( 55, 120, 0, "Aucune Ville Neutre");
        					else
        						AddLabel( 55, 120, 0, "Ville(s) Neutre(s)" );
        					break;
        				}
        	}
        	foreach (TownStone ville in status ) 
            { 
            	if( x == 36 )
				{
					if( page > 0 && page < nb_pages)
					{
						AddButton( 60, 390, 0xFAE, 0x0FB0, 0, GumpButtonType.Page, index * 10 + page - 1 ); //previous
						AddButton( 160, 390, 0xFA5, 0x0FA7, 0, GumpButtonType.Page, index * 10 + page + 1 ); //next
					}
					if( page == 0 && nb_pages > 0)
						AddButton( 160, 390, 0xFA5, 0x0FA7, 0, GumpButtonType.Page, index * 10 + page + 1 ); //next
					x = 0;
					page++;
					AddPage( index * 10 + page );
					if( page == nb_pages && nb_pages > 0)
					AddButton( 60, 390, 0xFAE, 0x0FB0, 0, GumpButtonType.Page, index * 10 + page - 1 ); //previous
				}
				if(ville != m_Town)
				{
					AddLabel( 60 + ((x / 12) * 125), 137 + ((x % 12) * 20), 0 , ville.Nom );
					i++;
					x++;
				}
            }
		}
		
        public override void OnResponse( NetState sender, RelayInfo info ) 
        { 
            PlayerMobile from = sender.Mobile as PlayerMobile;
        	if(info.ButtonID == 1)
        		from.SendGump(new TownGump(from, m_Town));
        }
    }
    
    public class ChangeStatusGump : Gump 
    { 
    	private TownStone m_Town;
    	private TownStone m_Conflict;
		
        public ChangeStatusGump( TownStone town, TownStone conflict) : base( 50, 30 ) 
        {
        	m_Town = town;
        	m_Conflict = conflict;
        	AddPage( 0 );
        	AddBackground( 0, 0, 110, 120, 5054 ); 
            AddBackground( 10, 10, 90, 100, 3000 ); 
 	    	AddButton(20, 20, 2714, 2715 , 1, GumpButtonType.Reply, 0);
        	AddLabel(50, 21, 0, "Alliance"); 
        	AddButton(20, 40, 2714, 2715, 2, GumpButtonType.Reply, 0);
        	AddLabel(50, 41, 0, "Paix"); 
        	AddButton(20, 60, 2714, 2715, 3, GumpButtonType.Reply, 0);
        	AddLabel(50, 61, 0, "Guerre"); 
        	AddButton(20, 80, 2714, 2715, 4, GumpButtonType.Reply, 0);
        	AddLabel(50, 81, 0, "Neutre"); 
        }
        
        public override void OnResponse( NetState sender, RelayInfo info ) 
        { 
            PlayerMobile from = sender.Mobile as PlayerMobile;
        	switch(info.ButtonID)
        	{
        		case 1 : //Alliance
        		{
        			Alliance(m_Conflict);
      				foreach(TownStone ville in m_Conflict.Guerre)
        				Guerre(ville);
        			break;
        		}
        		case 2 : //Paix
        		{
        			Paix(m_Conflict);
        			break;
        		}
        		case 3 : //Guerre
        		{
        			Guerre(m_Conflict);
        			foreach(TownStone ville in m_Conflict.Allies)
      					Guerre(ville);
        			break;
        		}
        		case 4 : //Neutre
        		{
        			Neutre(m_Conflict);
        			break;
        		}
        		default :
        		{	
        			from.SendGump(new TownGump(from, m_Town));
        			break;
        		}
        	}
        }
        
        private void Alliance(TownStone Conflict)
        {
        	if(m_Town.Neutre.Contains(Conflict))
        		m_Town.Neutre.Remove(Conflict);
        	if(m_Town.Paix.Contains(Conflict))
        		m_Town.Paix.Remove(Conflict);
        	if(m_Town.Guerre.Contains(Conflict))
        		m_Town.Guerre.Remove(Conflict);
        	if(!m_Town.Allies.Contains(Conflict))
        		m_Town.Allies.Add(Conflict);
        }
        
        private void Paix(TownStone Conflict)
        {
        	if(m_Town.Neutre.Contains(Conflict))
        		m_Town.Neutre.Remove(Conflict);
        	if(m_Town.Allies.Contains(Conflict))
        		m_Town.Allies.Remove(Conflict);
        	if(m_Town.Guerre.Contains(Conflict))
        		m_Town.Guerre.Remove(Conflict);
        	if(!m_Town.Paix.Contains(Conflict))
        		m_Town.Paix.Add(Conflict);			
        }
        
        private void Guerre(TownStone Conflict)
        {
        	if(m_Town.Neutre.Contains(Conflict))
        		m_Town.Neutre.Remove(Conflict);
        	if(m_Town.Paix.Contains(Conflict))
        		m_Town.Paix.Remove(Conflict);
        	if(m_Town.Allies.Contains(Conflict))
        		m_Town.Allies.Remove(Conflict);
        	if(!m_Town.Guerre.Contains(Conflict))
        		m_Town.Guerre.Add(Conflict);		
        }
        
        private void Neutre(TownStone Conflict)
        {
        	if(m_Town.Allies.Contains(Conflict))
        		m_Town.Allies.Remove(Conflict);
        	if(m_Town.Paix.Contains(Conflict))
        		m_Town.Paix.Remove(Conflict);
        	if(m_Town.Guerre.Contains(Conflict))
        		m_Town.Guerre.Remove(Conflict);
        	if(!m_Town.Neutre.Contains(Conflict))
        		m_Town.Neutre.Add(Conflict);
        }
    }
} 
