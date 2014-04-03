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
    public class BoxGump : Gump 
    { 
        private TownBox m_Box;
        private TownStone m_Town;
    	public int valeurdon;

        public BoxGump( PlayerMobile from,TownStone town, TownBox box ) : base( 20, 30 ) 
        { 
            m_Box = box; 
        	m_Town = town;
            bool isMaire = town.isMaire(from); 
			bool isConseiller = town.isConseiller(from) || isMaire;
        	bool isAmbassadeur = town.isAmbassadeur(from) || isConseiller;
        	bool isCapitaine = town.isCapitaine(from);
        	bool isGarde = town.isGarde(from) || isCapitaine;
        	bool isCitoyen = town.isCitoyen(from) || isAmbassadeur || isGarde;
        	bool isHLL = town.isHLL(from);
			bool IsGM = isGM(from);
        	string payeurs = "";
			//int don = 0;
	    
            AddPage( 0 ); 

            AddBackground( 0, 0, 420, 430, 5054 ); 
            AddBackground( 10, 10, 400,410, 3000 ); 

			AddLabel( 12, 12, 0,"Insp Gadget v1.0");
			AddLabel( 32, 32, 0,"Freeze v1.1");

            AddImage( 130, 0, 100 ); 
            AddLabel( 130 + ((143 - (town.Nom.Length * 8)) / 2), 40, 0, m_Town.Nom );

            AddLabel( 55, 103, 0, "Infos" ); // INFO 
            AddButton( 20, 103, 4005, 4007, 0, GumpButtonType.Page, 1 ); 
			if(isCitoyen || IsGM)
			{
				AddLabel( 170, 103, 0,"Taxes" ); // Feuille de paiement
            	AddButton( 135, 103, 4005, 4007, 0, GumpButtonType.Page, 2 ); 
			}
            if(isConseiller || isCapitaine || IsGM )
            {
            	AddLabel( 295, 103, 0,"Gestion" ); // Gestion
            	AddButton( 260, 103, 4005, 4007, 0, GumpButtonType.Page, 3 );
            }

            AddLabel( 345, 390, 0,"Quitter" );  // Quitter 
            AddButton( 310, 390, 0xFB4, 0x0FB6, 0, GumpButtonType.Reply, 0 ); 

			
			AddLabel( 20, 390, 0,"Faire un don de :       Po à la ville!");
			AddButton( 270, 390, 4005, 4007, 10, GumpButtonType.Reply, 0 ); 
			AddTextEntry(130, 390, 50, 20, 0x384, 5, "0");

			




			//INFOS
			AddPage( 1 );
        	AddLabel( 20, 130, 0, "Trésorie de la ville de " + m_Town.Nom);
        	AddLabel(20, 200, 0, "Les taxes sont de " + m_Box.Taxe + " pieces d'or par semaine");
  			if(m_Box.Retard_1.Contains(from))
        		AddLabel(20, 250, 0x7E, "Vous avez du retard dans le paiement des taxes");
  			if(m_Box.Retard_2.Contains(from))
        		AddLabel(20, 270, 0x25, "Si vous ne payez pas vous serez expulsé");
        	
        	AddLabel(20, 300, 0, String.Format("Prochain paiement le : {0}",m_Box.EndDate));
  
  			//TAXES
  			AddPage( 2 );
        	if(m_Box.Payeurs.Contains(from))
  				AddLabel(20, 130, 0, "Vous avez payer vos taxes");
        	else
        	{
        		AddLabel(45, 130, 0, String.Format("Verser {0} pieces d'or", m_Box.Taxe));
        		AddButton( 20, 130, 2714, 2715, 20, GumpButtonType.Reply, 0); 
        	}
        	
        	AddLabel(20, 170, 0, "Budget de la ville :");
        	AddLabel(30, 190, 0, String.Format("Caisses de la ville {0}", m_Box.CaisseVille) ); 
        	AddLabel(30, 210, 0, String.Format("Caisses de la Garde {0}", m_Box.CaisseGarde) ); 
        	
        	//GESTION
        	AddPage( 3 );
        	if(isConseiller || IsGM)
        	{	
        		AddLabel(20, 130, 0, "Montant de la Taxe :        pieces");
        		AddTextEntry(160, 130, 50, 20, 0x384, 1, m_Box.Taxe.ToString());
        		AddButton(270, 130, 2714, 2715, 30, GumpButtonType.Reply, 0);
        	}
        	else
        		AddLabel(20, 130, 0, String.Format("Montant de la Taxe : {0}",m_Box.Taxe));
        	if(isMaire || IsGM)
        	{	
        		AddLabel(20, 150, 0, "Retirer        Po de la Caisse Ville");
        		AddTextEntry(70, 150, 50, 20, 0x384, 2, m_Box.CaisseVille.ToString());
        		AddButton(270, 150, 2714, 2715, 31, GumpButtonType.Reply, 0);
        	}
        	if(isCapitaine || isMaire || IsGM)
        	{	
        		AddLabel(20, 170, 0, "Retirer        Po de la caisse Garde");
        		AddTextEntry(70, 170, 50, 20, 0x384, 3, m_Box.CaisseGarde.ToString());
        		AddButton(270, 170, 2714, 2715, 32, GumpButtonType.Reply, 0);
        	}
			
			if( isMaire || IsGM)
        	{	
        		AddLabel(20, 190, 0, "Basculer        Po vers caisses : Ville      Garde");
        		AddTextEntry(73, 190, 50, 20, 0x384, 4,"0" );
        		AddButton(270, 190, 2714, 2715, 34, GumpButtonType.Reply, 0);
				AddButton(350, 190, 2714, 2715, 35, GumpButtonType.Reply, 0);
        	}

			if (isConseiller || IsGM)
			{
				AddLabel(20, 220, 0, "Creer un contrat Vendeur pour 2000 Po");
				AddButton(270, 220, 2714, 2715, 33, GumpButtonType.Reply, 0);
			}
        	AddLabel(20, 240, 0, "Liste des payeurs :");
        	foreach(PlayerMobile mobile in m_Box.Payeurs)
        		payeurs = payeurs + mobile.Name + ", ";
        	AddHtml(20, 260, 380, 120, payeurs, true, true);
        	
        }
        
        public bool isGM (PlayerMobile from)
        {
            return from.AccessLevel >= AccessLevel.GameMaster;
        }

        public override void OnResponse( NetState sender, RelayInfo info ) 
        { 
        	PlayerMobile from = sender.Mobile as PlayerMobile; 

            switch ( info.ButtonID ) 
            {
				case 10 : //Don a la ville
				{	
					try
					{
					 valeurdon = Int32.Parse(info.GetTextEntry(5).Text);
					}
					catch
					{
						from.SendMessage("Entrer une valeur numerique!");
					}
					m_Box.Donnation(from,valeurdon);
					break;
				}



            	case 20 : //paiement de la taxe
            	{
            		m_Box.Paiement(from,true);
            		break;
            	}
            	case 30 : //montant de la taxe
            	{
            		try
            		{
            			m_Box.Taxe = Int32.Parse(info.GetTextEntry(1).Text);
            		}
            		catch
            		{
            			from.SendMessage("Entrer une valeur numerique!");
            		}
            		break;
            	}
            	
				
				case 31 : // Retirer de la caisse ville
				{
					try
					{
						if(Int32.Parse(info.GetTextEntry(2).Text) <= m_Box.CaisseVille)
						{
							from.Backpack.DropItem(new BankCheck(Int32.Parse(info.GetTextEntry(2).Text)));
							m_Box.CaisseVille -= Int32.Parse(info.GetTextEntry(2).Text);
						}
						else
							from.SendMessage("Il n'y a pas assez d'or dans les Caisses");
					}
					catch
					{
						from.SendMessage("Entrer une valeur numerique!");
					}
					break;
				}
            	case 32 : //Retirer des Caisses de la garde
            	{
            		try
            		{
            			if(Int32.Parse(info.GetTextEntry(3).Text) <= m_Box.CaisseGarde)
            			{
                            from.Backpack.DropItem(new BankCheck(Int32.Parse(info.GetTextEntry(3).Text)));
            				m_Box.CaisseGarde -= Int32.Parse(info.GetTextEntry(3).Text);
            			}
            			else
            				from.SendMessage("Il n'y a pas assez d'or dans les Caisses");
							Console.WriteLine(m_Box.CaisseGarde);
            		}
            		catch
            		{
            			from.SendMessage("Entrer une valeur numerique!");
            		}
            		break;
            	}
            	
            	case 33 : //vendeurs
            	{
					try
					{
						if(2000 <= m_Box.CaisseVille)
						{
							from.Backpack.DropItem(new ContractOfEmployment());
							m_Box.CaisseVille -= 2000;
						}
						else
							from.SendMessage("Il n'y a pas assez d'or dans les Caisses");
					}
					catch
					{
					}
					break;
            	}

				case 34 : //Basculler VERS Caisses de la ville
            	{
            		try
            		{
            			if(Int32.Parse(info.GetTextEntry(4).Text) <= m_Box.CaisseGarde)
            			{
            				m_Box.CaisseGarde -= Int32.Parse(info.GetTextEntry(4).Text);
							m_Box.CaisseVille += Int32.Parse(info.GetTextEntry(4).Text);
            			}
            			else
            				from.SendMessage("Il n'y a pas assez d'or dans les Caisses");
            		}
            		catch
            		{
            			from.SendMessage("Entrer une valeur numerique!");
            		}
            		break;
            	}

				case 35 : //Basculer VERS caise de la Garde
				{
					try
					{
						if(Int32.Parse(info.GetTextEntry(4).Text) <= m_Box.CaisseVille)
						{
							m_Box.CaisseGarde += Int32.Parse(info.GetTextEntry(4).Text);
							m_Box.CaisseVille -= Int32.Parse(info.GetTextEntry(4).Text);
						}
						else
							from.SendMessage("Il n'y a pas assez d'or dans les Caisses");
					}
					catch
					{
						from.SendMessage("Entrer une valeur numerique!");
					}
					break;
				}
            }
        }
    }
} 
