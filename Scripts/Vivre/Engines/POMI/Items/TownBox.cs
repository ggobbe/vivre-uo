using System;
using Server.Network;
using System.Collections;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;

namespace Server.IPOMI
{
	public class TownBox : Item
	{
		private TownStone m_Town;
		private ArrayList m_Payeurs;
		private ArrayList m_HistoPayeurs;
		private ArrayList m_Retard_1;
		private ArrayList m_Retard_2;
		private int m_Taxe;
		private int m_CaisseVille;
		private int m_CaisseGarde;
		private TaxeTimer m_TaxeTimer;
		private TimeSpan m_Delay;
		private DateTime m_EndDate;
		private int m_Don;	
		
		public int Don
		{
			get{return m_Don;}
			set{m_Don = value;}
		}
		
		public ArrayList Payeurs
		{
			get{return m_Payeurs;}
			set{m_Payeurs = value;}
		}
		public ArrayList HistoPayeurs
		{
			get{return m_HistoPayeurs;}
			set{m_HistoPayeurs = value;}
		}
		
		
		public ArrayList Retard_1
		{
			get{return m_Retard_1;}
			set{m_Retard_1 = value;}
		}
		
	
		public ArrayList Retard_2
		{
			get{return m_Retard_2;}
			set{m_Retard_2 = value;}
		}
		
		[CommandProperty( AccessLevel.Administrator )] 
		public int Taxe
		{
			get{return m_Taxe;}
			set{m_Taxe = Math.Abs(value);}
		}
		
		[CommandProperty( AccessLevel.Administrator )] 
		public int CaisseVille
		{
			get{return m_CaisseVille;}
			set{m_CaisseVille = value;}
		}
		
		[CommandProperty( AccessLevel.Administrator )] 
		public int CaisseGarde
		{
			get{return m_CaisseGarde;}
			set{m_CaisseGarde = value;}
		}
		
		[CommandProperty( AccessLevel.Administrator )] 
		public TaxeTimer TaxTimer
		{
			get{return m_TaxeTimer;}
			set{m_TaxeTimer = value;}
		}
		
		[CommandProperty( AccessLevel.GameMaster )] 
		public TimeSpan TaxeDelay
		{
			get{return m_Delay;}
			set{m_Delay = value;}
		}
		
		public DateTime EndDate
		{
			get {return m_EndDate;}
			set {m_EndDate = value;}
		}
		
		public TownBox(TownStone Town) : base( 0xE41 )
		{
			m_Town = Town;
			Name = "Trésorerie";
			Hue = Town.Hue;
			m_Town.Box = this;
			m_Payeurs = new ArrayList();
			m_HistoPayeurs = new ArrayList();
			m_Retard_1 = new ArrayList();
			m_Retard_2 = new ArrayList();
			m_Taxe = 500;
			m_CaisseVille = 0;
			m_CaisseGarde = 0;
			m_Delay = TimeSpan.FromDays( 7.0 );
			m_EndDate = DateTime.Now + m_Delay;
			m_TaxeTimer = new TaxeTimer(m_Town, m_Delay);
			m_TaxeTimer.Start();
		}

		public TownBox( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			writer.Write( (TownStone)m_Town);
			writer.WriteMobileList( m_Payeurs, true);
			writer.WriteMobileList( m_HistoPayeurs, true);
			writer.WriteMobileList( m_Retard_1, true);
			writer.WriteMobileList( m_Retard_2, true);
			writer.Write( (int)m_Taxe );
			writer.Write( (int)m_CaisseVille );
			writer.Write( (int)m_CaisseGarde );
			writer.Write( m_EndDate );
			writer.Write( m_Delay );
			
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_Town = (TownStone)reader.ReadItem();
			m_Payeurs = reader.ReadMobileList();
			m_HistoPayeurs = reader.ReadMobileList();
			m_Retard_1 = reader.ReadMobileList();
			m_Retard_2 = reader.ReadMobileList();
			m_Taxe = reader.ReadInt();
			m_CaisseVille = reader.ReadInt();
			m_CaisseGarde = reader.ReadInt();
			m_EndDate = reader.ReadDateTime();
			m_TaxeTimer = new TaxeTimer(m_Town, m_EndDate - DateTime.Now);
			m_TaxeTimer.Start();
			m_Delay = reader.ReadTimeSpan();
		}

		public override void OnSingleClick(Mobile from)
		{
			LabelTo(from, "[ " + m_Town.Nom + " ]");
			LabelTo(from, Name);
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			from.SendGump(new BoxGump((PlayerMobile)from,m_Town,this));
		}
		
		public void Donnation(PlayerMobile from,int valeur)
		{
			// Scriptiz : loggons les paiements pour être sur
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter("taxes.log", true))
                {
                    string name = (from != null ? from.Name : "null");
                    string acc = (from != null && from.Account != null ? from.Account.Username : "null");
                    sw.WriteLine(String.Format("{0} : {1} ({2}) fait un don de {3} po à {4}", DateTime.Now, name, acc, valeur, m_Town.Name));
                }
            }
            catch { }
            
			// trap the cheaters
			valeur = Math.Abs(valeur);
			
			if (valeur == 0 )
			{
				from.SendMessage("Radin!");
				return;
			}
			else if(from.BankBox.TotalGold >= valeur )
			{
				from.BankBox.ConsumeTotal(typeof(Gold), valeur);
				m_CaisseVille += valeur;
				from.SendMessage("La ville vous remercie!");
			}
			else
				from.SendMessage("Vous n'avez pas assez d'or dans votre Banque!");


		}



		public void Paiement(PlayerMobile from, bool manuel)
		{
			// Scriptiz : taxes automatiques désactivées
            if (!manuel)
                return;
            
            // Scriptiz : loggons les paiements pour être sur
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter("taxes.log", true))
                {
                    string name = (from != null ? from.Name : "null");
                    string acc = (from != null && from.Account != null ? from.Account.Username : "null");
                    sw.WriteLine(String.Format("{0} : {1} ({2}) paye {3} po à {4} [{5}]", DateTime.Now, name, acc, m_Taxe, m_Town.Name, (manuel ? "Don" : "Taxes")));
                }
            }
            catch { }

			if(!(m_Town.isMaire(from) || 
			     m_Town.isConseiller(from) ||
			     m_Town.isAmbassadeur(from) ||
			     m_Town.isCapitaine(from) ))
		    {
		   		if(from.BankBox.TotalGold >= m_Taxe )
            	{
	            	from.BankBox.ConsumeTotal(typeof(Gold), m_Taxe);
    	        	m_CaisseVille += m_Taxe / 2;
        	    	m_CaisseGarde += m_Taxe / 2;
            		if(manuel)
            		{
		           		if(m_Retard_2.Contains(from))
    		       		{
	    	        		m_Retard_2.Remove(from);
    	    	    		m_Retard_1.Add(from);
        	    		}
            			else if(m_Retard_1.Contains(from))
	            			m_Retard_1.Remove(from);
		            	else
    		        		m_Payeurs.Add(from);
            		}
            		else
    	        	{
        	    		if(!m_Retard_1.Contains(from) && !m_Retard_2.Contains(from))
            				m_Payeurs.Add(from);
            		}
            	}
	            else
    	        {
        	    	if(!manuel)
            		{
            			if(m_Retard_1.Contains(from))
            			{
		            		m_Retard_1.Remove(from);
    		        		m_Retard_2.Add(from);
        		    	}
            			else
            				m_Retard_1.Add(from);
	            	}
    	        	else
        	    		from.SendMessage("Vous n'avez pas assez d'or dans votre Banque!");
            	}
		    }
		}	
	}
	
	public class TaxeTimer : Timer
	{
		private TownStone m_Town;
			
		public TaxeTimer(TownStone town, TimeSpan delay) : base( delay )
		{
			m_Town = town;
		}
		
		protected override void OnTick()
		{
			ArrayList m_Exclus = new ArrayList();
			m_Town.Box.HistoPayeurs.Clear();
			foreach(PlayerMobile mobile in m_Town.Citoyens)
			{	
				if(m_Town.Box.Retard_2.Contains(mobile))
				{
					m_Town.Box.Retard_2.Remove(mobile);
					m_Exclus.Add(mobile);
				}
				else
					if(!m_Town.Box.Payeurs.Contains(mobile))
            			m_Town.Box.Paiement(mobile,false);
			}
			foreach(PlayerMobile mobile in m_Exclus)
			{
				foreach(PlayerMobile result in m_Town.Resultats)
				{
					if(mobile == result)
						m_Town.Votants.RemoveAt(m_Town.Resultats.IndexOf(result));
				}
				while(m_Town.Resultats.Contains(mobile))
					m_Town.Resultats.Remove(mobile);
				m_Town.Resultats.Remove(mobile);
				m_Town.Citoyens.Remove(mobile);
			}	
			foreach(PlayerMobile mobile in m_Town.Box.Payeurs)
				m_Town.Box.HistoPayeurs.Add(mobile);
			m_Town.Box.Payeurs.Clear();
			
			m_Town.Box.EndDate = DateTime.Now + m_Town.Box.TaxeDelay;
			//m_Town.Box.TaxTimer = new TaxeTimer(m_Town, m_Town.Box.TaxeDelay);
			m_Town.Box.TaxTimer.Start();
		}
	}
}
