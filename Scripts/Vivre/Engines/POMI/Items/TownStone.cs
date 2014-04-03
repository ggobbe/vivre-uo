using System;
using Server.Network;
using System.Collections;
using Server.Mobiles;
using Server.Targeting;

namespace Server.IPOMI
{
	public class TownStone : Item
	{
		private POMI m_Pomi;
		private ArrayList m_Citoyens;
		private ArrayList m_Candidats;
		private ArrayList m_HLL;
		private ArrayList m_gardes_pnj;
		private ArrayList m_Allies;
		private ArrayList m_Paix;
		private ArrayList m_Guerre;
		private ArrayList m_Neutre;
		private PlayerMobile m_Maire;
		private PlayerMobile m_Conseiller;
		private PlayerMobile m_Ambassadeur;
		private PlayerMobile m_Capitaine;
		private ArrayList m_Gardes;
		private PomiCloak m_MaireCloak;
		private PomiCloak m_ConseillerCloak;
		private PomiCloak m_AmbassadeurCloak;
		private PomiCloak m_CapitaineCloak;
		private CapitaineBook m_CapitaineBook;
		private ArrayList m_GardeCloak;
		private string m_Nom;
		private string m_Charte0;
		private string m_Charte1;
		private string m_Charte2;
		private string m_Charte3;
		private string m_Charte4;
		private string m_Charte5;
		private string m_Charte6;
		private string m_Charte7;
		private ArrayList m_Votants;
		private ArrayList m_Elections;
		private ArrayList m_Resultats;
		private DateTime m_EndDate;
		private ElectionTimer m_ElecTimer;
		private TimeSpan m_ElecDelay;
		private int m_MaxDistance;
		private TownBox m_Box;
		private VilleRaciale m_VilleRace;
		//private bool m_AdminInUse;
		
		public TownStone(POMI Pomi) : base( 0xED4 )
		{
			m_Pomi = Pomi;
			m_Citoyens = new ArrayList();
			m_Candidats = new ArrayList();
			m_HLL = new ArrayList();
			m_Allies = new ArrayList();
			m_Paix = new ArrayList();
			m_Guerre = new ArrayList();
			m_Neutre = new ArrayList();
			m_Maire = null;
			m_Conseiller = null;
			m_Ambassadeur = null;
			m_Capitaine = null;
			m_Gardes = new ArrayList();
			m_gardes_pnj = new ArrayList();
			m_MaireCloak = null;
			m_ConseillerCloak = null;
			m_AmbassadeurCloak = null;
			m_CapitaineCloak = null;
			m_CapitaineBook = null;
			m_GardeCloak = new ArrayList();
			Name = "Pierre de ville";
			m_Nom = "Sans nom";
			m_Votants = new ArrayList();
			m_Elections = new ArrayList();
			m_Resultats = new ArrayList();
			m_ElecDelay = TimeSpan.FromDays( 14.0 );
			m_EndDate = DateTime.Now + m_ElecDelay;
			m_ElecTimer = new ElectionTimer(this, m_ElecDelay);
			m_ElecTimer.Start();
			m_MaxDistance = 100;
			//m_AdminInUse = false;			
		}

		public TownStone( Serial serial ) : base( serial )
		{
		}
		

		public enum VilleRaciale
		{
			Aucune,
			Humain,
			Drow,
			ElfeGlace,
			ElfeSylvain,
			HautElfe,
			HautEtSylvain,
			Hobbit,
			Nain
		}

		public bool isMaire(PlayerMobile from)
		{
			if(from == m_Maire) return true;
			return false;
		}

		public bool isConseiller(PlayerMobile from)
		{
			if(from == m_Conseiller) return true;
			return false;
		}

		public bool isAmbassadeur(PlayerMobile from)
		{
			if(from == m_Ambassadeur) return true;
			return false;
		}

		public bool isCapitaine(PlayerMobile from)
		{
			if(from == m_Capitaine) return true;
			return false;
		}

		public bool isGarde(PlayerMobile from)
		{
			if(m_Gardes.Contains(from)) return true;
			return false;
		}

		public bool isCitoyen(PlayerMobile from)
		{
			if(m_Citoyens.Contains(from)) return true;
			return false;
		}
		
		public bool isHLL(PlayerMobile from)
		{
			if(m_HLL.Contains(from)) return true;
			return false;
		}
		
		public POMI Pomi
		{
			get{return m_Pomi;}
		}

		public string Nom
		{
			get {return m_Nom;}
			set {m_Nom = value;}	
		}
		
		public PlayerMobile Maire
		{
			get {return m_Maire;}
			set {m_Maire = value;}	
		}
		
		public PlayerMobile Conseiller
		{
			get {return m_Conseiller;}
			set {m_Conseiller = value;}	
		}
		
		public PlayerMobile Ambassadeur
		{
			get {return m_Ambassadeur;}
			set {m_Ambassadeur = value;}	
		}
		
		public PlayerMobile Capitaine
		{
			get {return m_Capitaine;}
			set {m_Capitaine = value;}	
		}
		
		public ArrayList Citoyens
		{
			get {return m_Citoyens;}
			set {m_Citoyens = value;}	
		}
		
		public ArrayList Candidats
		{
			get {return m_Candidats;}
			set {m_Candidats = value;}	
		}
		
		public ArrayList Gardes
		{
			get {return m_Gardes;}
			set {m_Gardes = value;}	
		}
		
		public ArrayList GardesPNJ
		{
			get {return m_gardes_pnj;}
			set {m_gardes_pnj = value;}
		}

		public ArrayList HLL
		{
			get {return m_HLL;}
			set {m_HLL = value;}	
		}
		
		public ArrayList Allies
		{
			get {return m_Allies;}
			set {m_Allies = value;}	
		}
		
		public ArrayList Paix
		{
			get {return m_Paix;}
			set {m_Paix = value;}	
		}
		
		public ArrayList Guerre
		{
			get {return m_Guerre;}
			set {m_Guerre = value;}	
		}
		
		public ArrayList Neutre
		{
			get {return m_Neutre;}
			set {m_Neutre = value;}	
		}
		
		public PomiCloak MaireCloak
		{
			get {return m_MaireCloak;}
			set {m_MaireCloak = value;}
		}
		
		public PomiCloak ConseillerCloak
		{
			get {return m_ConseillerCloak;}
			set {m_ConseillerCloak = value;}
		}

		public PomiCloak AmbassadeurCloak
		{
			get {return m_AmbassadeurCloak;}
			set {m_AmbassadeurCloak = value;}
		}

		public PomiCloak CapitaineCloak
		{
			get {return m_CapitaineCloak;}
			set {m_CapitaineCloak = value;}
		}

		public CapitaineBook CptBook
		{
			get {return m_CapitaineBook;}
			set {m_CapitaineBook = value;}
		}

		public ArrayList GardeCloak
		{
			get {return m_GardeCloak;}
			set {m_GardeCloak = value;}
		}

		public string Charte0
		{
			get {return m_Charte0;}
			set {m_Charte0 = value;}
		}
		
		public string Charte1
		{
			get {return m_Charte1;}
			set {m_Charte1 = value;}
		}
		public string Charte2
		{
			get {return m_Charte2;}
			set {m_Charte2 = value;}
		}
		public string Charte3
		{
			get {return m_Charte3;}
			set {m_Charte3 = value;}
		}
		public string Charte4
		{
			get {return m_Charte4;}
			set {m_Charte4 = value;}
		}
		public string Charte5
		{
			get {return m_Charte5;}
			set {m_Charte5 = value;}
		}
		public string Charte6
		{
			get {return m_Charte6;}
			set {m_Charte6 = value;}
		}
		public string Charte7
		{
			get {return m_Charte7;}
			set {m_Charte7 = value;}
		}
		
		public ArrayList Votants
		{
			get {return m_Votants;}
			set {m_Votants = value;}	
		}

		public ArrayList Elections
		{
			get {return m_Elections;}
			set {m_Elections = value;}	
		}

		public ArrayList Resultats
		{
			get {return m_Resultats;}
			set {m_Resultats = value;}	
		}

		public DateTime EndDate
		{
			get {return m_EndDate;}
			set {m_EndDate = value;}
		}
		
		public ElectionTimer ElecTimer
		{
			get {return m_ElecTimer;}
			set {m_ElecTimer = value;}
		}
		
		[CommandProperty( AccessLevel.GameMaster )] 
		public TimeSpan ElecDelay
		{
			get {return m_ElecDelay;}
			set {m_ElecDelay = value;}
		}

		[CommandProperty( AccessLevel.GameMaster )] 
		public VilleRaciale VilleRace
		{
			get {return m_VilleRace;}
			set {m_VilleRace = value;}
		}
		
		public TownBox Box
		{
			get {return m_Box;}
			set {m_Box = value;}
		}

		[CommandProperty( AccessLevel.GameMaster )] 
		public int MaxDistance
		{
			get {return m_MaxDistance;}
			set {m_MaxDistance = value;}
		}

		/*public bool AdminInUse
		{
			get {return m_AdminInUse;}
			set {m_AdminInUse = value;}	
		}*/

        // Scriptiz : possibilité de changer la hue avec un .set hue xxx tout en impactant les changements sur tout le pomi
        [CommandProperty(AccessLevel.GameMaster)]
        public override int Hue
        {
            get { return base.Hue; }
            set
            {
                base.Hue = value;
                if (this.MaireCloak != null)
                    this.MaireCloak.Hue = value;
                if (this.ConseillerCloak != null)
                    this.ConseillerCloak.Hue = value;
                if (this.AmbassadeurCloak != null)
                    this.AmbassadeurCloak.Hue = value;
                if (this.CapitaineCloak != null)
                    this.CapitaineCloak.Hue = value;
                if (this.CptBook != null)
                    this.CptBook.Hue = value;
                foreach (PomiCloak cloak in this.GardeCloak)
                    cloak.Hue = value;

                PomiCloak pnjcloak;
                //Halberd arme;
                foreach (GuardSpawner guard in this.GardesPNJ)
                {
                    try
                    {
                        //arme = guard.SpawnedGuard.FindItemOnLayer(Layer.TwoHanded) as Halberd;
                        pnjcloak = guard.SpawnedGuard.FindItemOnLayer(Layer.Cloak) as PomiCloak;
                        if (pnjcloak != null)
                            pnjcloak.Hue = value;
                        //if (arme != null)
                            //arme.Hue = hue;
                    }
                    catch { }

                }

                //this.Hue = value;
                this.Box.Hue = value;
            }
        }
		
		public void Initialisation()
		{
			m_Citoyens.Clear();
			m_Candidats.Clear();
			m_HLL.Clear();
			m_Allies.Clear();
			m_Paix.Clear();
			m_Guerre.Clear();
			m_Neutre.Clear();
			if(m_Maire != null)
				m_Maire.Title = null;
			m_Maire = null;
			m_Nom = "Sans nom";
			Name = "Pierre de ville";
			Hue = 0;
			NouveauConseil();
			m_ElecTimer.Stop();
			m_ElecTimer = new ElectionTimer(this, m_ElecDelay);
			m_ElecTimer.Start();
			m_VilleRace = VilleRaciale.Aucune;
		}
		
		public void NouveauConseil()
		{
			if(m_MaireCloak != null)
				m_MaireCloak.Delete();
			m_MaireCloak = null;

			if(m_Conseiller != null)
				m_Conseiller.Title = null;
			m_Conseiller = null;
			if(m_ConseillerCloak != null)
				m_ConseillerCloak.Delete();
			m_ConseillerCloak = null;

			if(m_Ambassadeur != null)
				m_Ambassadeur.Title = null;
			m_Ambassadeur = null;
			if(m_AmbassadeurCloak != null)
				m_AmbassadeurCloak.Delete();
			m_AmbassadeurCloak = null;

			if(m_Capitaine != null)
				m_Capitaine.Title = null;
			m_Capitaine = null;
			if(m_CapitaineCloak != null)
				m_CapitaineCloak.Delete();
			m_CapitaineCloak = null;
			if(m_CapitaineBook != null)
				m_CapitaineBook.Delete();
			m_CapitaineBook = null;


			foreach(PlayerMobile GardePlayer in m_Gardes)
				GardePlayer.Title = null;
			m_Gardes.Clear();
			foreach(GuardSpawner guard in m_gardes_pnj)
				guard.Delete();
			m_gardes_pnj.Clear();
			foreach(PomiCloak cloak in m_GardeCloak)
				cloak.Delete();
			m_GardeCloak.Clear();
			m_Votants.Clear();
			m_Elections.Clear();
			m_Resultats.Clear();
		}
		
		public bool InOtherTown(Mobile mobile)
		{
			bool exist = false;
			foreach(TownStone town in m_Pomi.Villes)
			{
				if(town != this)
					if(town.Citoyens.Contains(mobile) || town.Candidats.Contains(mobile))  exist = true;
			}
			return exist;
		}

		public override void OnDelete()
		{
			Initialisation();
			foreach(TownStone ville in m_Pomi.Villes)
			{
				if(ville.Allies.Contains(this))
					ville.Allies.Remove(this);
				if(ville.Paix.Contains(this))
					ville.Paix.Remove(this);
				if(ville.Guerre.Contains(this))
					ville.Guerre.Remove(this);
				if(ville.Neutre.Contains(this))
					ville.Neutre.Remove(this);
			}
			m_Pomi.Villes.Remove(this);
			try 
			{
				m_Box.Delete();
			}
			catch{}
		} 

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			//version 1  Freeze 6 nov 2004

			writer.Write( (byte) m_VilleRace );

			// version 0

			
			writer.WriteMobileList( m_Citoyens, true );
			writer.WriteMobileList( m_Candidats, true );
			writer.WriteMobileList( m_HLL, true );
			writer.WriteMobileList( m_Gardes, true );
			writer.WriteItemList( m_gardes_pnj, true );
			writer.Write( (Mobile)m_Maire );
			writer.Write( (Mobile)m_Conseiller );
			writer.Write( (Mobile)m_Ambassadeur );
			writer.Write( (Mobile)m_Capitaine );
			writer.Write( (POMI)m_Pomi);
			writer.Write( (PomiCloak)m_MaireCloak);
			writer.Write( (PomiCloak)m_ConseillerCloak);
			writer.Write( (PomiCloak)m_AmbassadeurCloak);
			writer.Write( (PomiCloak)m_CapitaineCloak);
			writer.Write( (CapitaineBook)m_CapitaineBook);
			writer.WriteItemList( m_GardeCloak, true );
			writer.Write((string)m_Nom);
			writer.Write((string)m_Charte0);
			writer.Write((string)m_Charte1);
			writer.Write((string)m_Charte2);
			writer.Write((string)m_Charte3);
			writer.Write((string)m_Charte4);
			writer.Write((string)m_Charte5);
			writer.Write((string)m_Charte6);
			writer.Write((string)m_Charte7);
			writer.WriteMobileList( m_Votants, true );
			writer.WriteMobileList( m_Elections, true );
			writer.WriteMobileList( m_Resultats, true );
			writer.Write( m_EndDate );
			writer.Write( m_ElecDelay );
			writer.Write( m_MaxDistance );
			writer.WriteItemList( m_Allies, true );
			writer.WriteItemList( m_Paix, true );
			writer.WriteItemList( m_Guerre, true );
			writer.WriteItemList( m_Neutre, true );
			writer.Write( (TownBox)m_Box);
			
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version ) 
			{ 
				case 1:
				{
					m_VilleRace = (VilleRaciale)reader.ReadByte();
					
					goto case 0;
				}

				case 0:
				{

					m_Citoyens = reader.ReadMobileList();
					m_Candidats = reader.ReadMobileList();
					m_HLL = reader.ReadMobileList();
					m_Gardes = reader.ReadMobileList();
					m_gardes_pnj = reader.ReadItemList();
					m_Maire = (PlayerMobile)reader.ReadMobile();
					m_Conseiller = (PlayerMobile)reader.ReadMobile();
					m_Ambassadeur = (PlayerMobile)reader.ReadMobile();
					m_Capitaine = (PlayerMobile)reader.ReadMobile();
					m_Pomi = (POMI)reader.ReadItem();	
					m_MaireCloak = (PomiCloak)reader.ReadItem();
					m_ConseillerCloak = (PomiCloak)reader.ReadItem();
					m_AmbassadeurCloak = (PomiCloak)reader.ReadItem();
					m_CapitaineCloak = (PomiCloak)reader.ReadItem();
					m_CapitaineBook = (CapitaineBook)reader.ReadItem();
					m_GardeCloak = reader.ReadItemList();
					m_Nom = reader.ReadString();
					m_Charte0 = reader.ReadString();
					m_Charte1 = reader.ReadString();
					m_Charte2 = reader.ReadString();
					m_Charte3 = reader.ReadString();
					m_Charte4 = reader.ReadString();
					m_Charte5 = reader.ReadString();
					m_Charte6 = reader.ReadString();
					m_Charte7 = reader.ReadString();
					m_Votants = reader.ReadMobileList();
					m_Elections = reader.ReadMobileList();
					m_Resultats = reader.ReadMobileList();
					m_EndDate = reader.ReadDateTime();
					m_ElecTimer = new ElectionTimer(this, m_EndDate - DateTime.Now);
					m_ElecTimer.Start();
					m_ElecDelay = reader.ReadTimeSpan();
					m_MaxDistance = reader.ReadInt();
					m_Allies = reader.ReadItemList();
					m_Paix = reader.ReadItemList();
					m_Guerre = reader.ReadItemList();
					m_Neutre = reader.ReadItemList();
					m_Box = (TownBox)reader.ReadItem();
					break;
				}
			}

	}

		public override void OnDoubleClick( Mobile from )
		{
			//if(!m_AdminInUse)
			//{
				from.SendGump(new TownGump((PlayerMobile)from,this));
			//	m_AdminInUse = true;
			//	}	
			//else
			//	from.SendMessage("Pierre de Ville en cours d'utilisation");
		}

		public override void OnSingleClick(Mobile from)
		{
			base.OnSingleClick(from);
			LabelTo(from,"[ {0} ]", m_Nom);
		}
	}
	
	public class GestionTarget : Target
    	{
        TownStone m_Town;
    	int m_ButtonID;
        
        public GestionTarget(TownStone town, int ButtonID) : base( -1, true, TargetFlags.None )
		{
			m_Town = town;
			m_ButtonID = ButtonID;
		}
		protected override void OnTarget( Mobile mobile, object targeted )
		{
			if(targeted is PlayerMobile)
			{
				PlayerMobile target = targeted as PlayerMobile;
				if(m_Town.Citoyens.Contains(target))
					switch(m_ButtonID)
					{
						case 302: //Nomer le Maire
						{ 
							if(m_Town.MaireCloak != null)
								m_Town.MaireCloak.Delete();
							m_Town.MaireCloak = new PomiCloak(m_Town, "Bourgmestre");
							target.Backpack.DropItem(m_Town.MaireCloak);
							m_Town.Maire = target; //initalisation avec le Maire target
							target.SendMessage("Vous devenez le Bourgmestre de " + m_Town.Nom);
							break;
						}
						case 303: //Nomer le Conseiller
						{
							if(m_Town.Maire != target && m_Town.Ambassadeur != target && m_Town.Capitaine != target)
							{
								if(m_Town.ConseillerCloak != null)
									m_Town.ConseillerCloak.Delete();
								m_Town.ConseillerCloak = new PomiCloak(m_Town, "Conseiller");
								target.Backpack.DropItem(m_Town.ConseillerCloak);
								m_Town.Conseiller = target;
								target.SendMessage("Vous devenez le Conseiller de " + m_Town.Nom);
							}
							else
								mobile.SendMessage("Cette personne a deja un Titre");
							break;
						}
						case 304: //Nomer l'Ambassadeur
						{
							if(m_Town.Maire != target && m_Town.Conseiller != target && m_Town.Capitaine != target)
							{
								if(m_Town.AmbassadeurCloak != null)
									m_Town.AmbassadeurCloak.Delete();
								m_Town.AmbassadeurCloak = new PomiCloak(m_Town, "Ambassadeur");
								target.Backpack.DropItem(m_Town.AmbassadeurCloak);
								m_Town.Ambassadeur = target;
								target.SendMessage("Vous devenez l'Ambassadeur de " + m_Town.Nom);
							}
							else
								mobile.SendMessage("Cette personne a deja un Titre");
							break;
						}
						case 305: //Nomer le Capitaine
						{
							if(m_Town.Maire != target && m_Town.Ambassadeur != target && m_Town.Conseiller != target)
							{
								if(m_Town.CapitaineCloak != null)
									m_Town.CapitaineCloak.Delete();
								m_Town.CapitaineCloak = new PomiCloak(m_Town, "Capitaine");
								if(m_Town.CptBook != null)
									m_Town.CptBook.Delete();
								m_Town.CptBook = new CapitaineBook(m_Town);
								target.Backpack.DropItem(m_Town.CapitaineCloak);
								target.Backpack.DropItem(m_Town.CptBook);
								m_Town.Capitaine = target;
								target.SendMessage("Vous devenez le Capitaine de la Garde de " + m_Town.Nom);
							}
							else
								mobile.SendMessage("Cette personne a deja un Titre");
							break;
						}
					}
				else
					mobile.SendMessage("Cette personne n'est pas citoyenne");
			}
    	}
	}
	
	public class GardeTarget : Target
    	{
    		TownStone m_Town;
        
        	public GardeTarget(TownStone town) : base( -1, true, TargetFlags.None )
		{
			m_Town = town;
		}
		protected override void OnTarget( Mobile mobile, object targeted )
		{
			if(targeted is PlayerMobile)
			{
				PlayerMobile target = targeted as PlayerMobile;
				if(m_Town.Citoyens.Contains(target))
				{
					if(!m_Town.Gardes.Contains(target) &&
					   (m_Town.Maire != target) && 
				 	   (m_Town.Ambassadeur != target) && 
					   (m_Town.Conseiller != target) &&
					   (m_Town.Capitaine != target))
					{
						m_Town.Gardes.Add(target);
						PomiCloak cloak = new PomiCloak(m_Town, "Garde");
						m_Town.GardeCloak.Add(cloak);
						target.Backpack.DropItem(cloak);
					}
					else
						mobile.SendMessage("Cette personne a deja un titre");
				}
				else
					mobile.SendMessage("Cette personne n'est pas citoyenne");
			}
    		}
    	}

	public class PNJTarget : Target
	{
		TownStone m_Town;
		public PNJTarget(TownStone town) : base( -1, true, TargetFlags.None )
		{
			m_Town = town;
		}

		protected override void OnTarget( Mobile mobile, object targeted )
		{
			IPoint3D target = targeted as IPoint3D;
			if(target != null)
			{
				if( Math.Sqrt( (m_Town.X - target.X)*(m_Town.X - target.X) + (m_Town.Y - target.Y)*(m_Town.Y - target.Y) ) < m_Town.MaxDistance)
				{
					GuardSpawner guard = new GuardSpawner(new Point3D(target.X, target.Y,target.Z), m_Town);
					m_Town.GardesPNJ.Add(guard);
				}
				else
					mobile.SendMessage("C'est trop loin de la pierre de ville");
			}
		}
	}

	public class ElectionTimer : Timer
	{
		private TownStone m_Town;
		ArrayList LitigeMaire = new ArrayList();
		PlayerMobile NouveauMaire = null;
		int nbvoies = 0;
			
		public ElectionTimer(TownStone town, TimeSpan delay) : base( delay )
		{
			//Priority = TimerPriority.OneSecond;
			m_Town = town;
		}
		
		protected override void OnTick()
		{
			int result = 0;
			foreach(PlayerMobile mobile in m_Town.Elections)
			{
				result = CompteVoies(mobile);
				if(result > nbvoies)
				{
					nbvoies = result;
					NouveauMaire = mobile;
					LitigeMaire.Clear();
				}
				else if(result == nbvoies && result != 0)
				{
					if(!LitigeMaire.Contains(NouveauMaire))
						LitigeMaire.Add(NouveauMaire);
					LitigeMaire.Add(mobile);
				}
			}
			if(LitigeMaire.Count>0)
				NouveauMaire = (PlayerMobile)LitigeMaire[Utility.Random(LitigeMaire.Count)];
			if(NouveauMaire != null && NouveauMaire != m_Town.Maire)
			{
				m_Town.NouveauConseil();
				m_Town.Maire = NouveauMaire;
				m_Town.MaireCloak = new PomiCloak(m_Town, "Bourgmestre");
				m_Town.Maire.Backpack.DropItem(m_Town.MaireCloak);
			}
			m_Town.Votants.Clear();
			m_Town.Elections.Clear();
			m_Town.Resultats.Clear();
		 	m_Town.EndDate = DateTime.Now + m_Town.ElecDelay;
			//m_Town.ElecTimer = new ElectionTimer(m_Town, m_Town.ElecDelay);
			m_Town.ElecTimer.Start();
		}

		private int CompteVoies(PlayerMobile from)
		{
			int result = 0;
			foreach(PlayerMobile mobile in m_Town.Resultats)
			{
				if(mobile == from)
					result++;
			}
			return result;
		}
	}
}
