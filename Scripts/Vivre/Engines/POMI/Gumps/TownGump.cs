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
using System.IO;

namespace Server.IPOMI
{
    public class TownGump : Gump
    {
        private TownStone m_Town;

        public TownGump(PlayerMobile from, TownStone town)
            : base(20, 30)
        {
            m_Town = town;
            bool isMaire = town.isMaire(from);
            bool isConseiller = town.isConseiller(from) || isMaire;
            bool isAmbassadeur = town.isAmbassadeur(from) || isConseiller;
            bool isCapitaine = town.isCapitaine(from) || isConseiller;
            bool isGarde = town.isGarde(from) || isCapitaine;
            bool isCitoyen = town.isCitoyen(from) || isAmbassadeur || isGarde;
            bool isHLL = town.isHLL(from);
            bool IsGM = isGM(from);

            int i;
            int x;
            int page;
            int nb_pages;

            AddPage(0);

            AddBackground(0, 0, 420, 430, 5054);
            AddBackground(10, 10, 400, 410, 3000);

            AddImage(130, 0, 100);
            AddLabel(130 + ((143 - (town.Nom.Length * 8)) / 2), 40, 0, town.Nom);

            AddLabel(55, 103, 0, "Infos"); // INFO 
            AddButton(20, 103, 4005, 4007, 0, GumpButtonType.Page, 1);
            if (isCitoyen || IsGM)
            {
                AddLabel(170, 103, 0, "Citoyens"); // Citoyens
                AddButton(135, 103, 4005, 4007, 0, GumpButtonType.Page, 20);
            }
            if (isCapitaine || isAmbassadeur || IsGM)
            {
                AddLabel(295, 103, 0, "Gestion"); // Gestion
                AddButton(260, 103, 4005, 4007, 0, GumpButtonType.Page, 3);
            }

            AddLabel(335, 390, 0, "Quitter");  // Quitter 
            AddButton(300, 390, 0xFB4, 0x0FB6, 0, GumpButtonType.Reply, 0);

            // Info page 
            AddPage(1);
            // Scriptiz : on affiche les rawname au lieu de name pour éviter les modifs de la cagoule
            if (town.Maire != null)
                AddLabel(20, 130, 0, "Bourgmestre de la ville : " + town.Maire.RawName); // Maire de la ville
            else
                AddLabel(20, 130, 0, "Bourgmestre de la ville : [ Poste vacant ]");

            if (town.Conseiller != null)
                AddLabel(20, 150, 0, "Conseiller du Bourgmestre : " + town.Conseiller.RawName); // Conseiller
            else
                AddLabel(20, 150, 0, "Conseiller du Bourgmestre : [ Poste vacant ]");
            if (town.Ambassadeur != null)
                AddLabel(20, 170, 0, "Ambassadeur : " + town.Ambassadeur.RawName); // Ambassadeur
            else
                AddLabel(20, 170, 0, "Ambassadeur : [ Poste vacant ]");
            if (town.Capitaine != null)
                AddLabel(20, 190, 0, "Capitaine de la Garde : " + town.Capitaine.RawName); // Capitaine
            else
                AddLabel(20, 190, 0, "Capitaine de la Garde : [ Poste vacant ]");

            AddHtml(20, 220, 380, 140, town.Charte0 + "<br>" +
                    town.Charte1 + "<br>" +
                    town.Charte2 + "<br>" +
                    town.Charte3 + "<br>" +
                    town.Charte4 + "<br>" +
                    town.Charte5 + "<br>" +
                    town.Charte6 + "<br>" +
                    town.Charte7, true, true);

            AddButton(90, 367, 2714, 2715, 0, GumpButtonType.Page, 50);
            AddLabel(115, 367, 0, "Hors la Loi");
            AddButton(230, 367, 2714, 2715, 104, GumpButtonType.Reply, 0);
            AddLabel(255, 367, 0, "Diplomatie");


            if (m_Town.Candidats.Contains(from))
            {
                AddButton(30, 390, 0xFB1, 0xFB3, 102, GumpButtonType.Reply, 0);
                AddLabel(65, 390, 0, "Retirer sa candidature");
            }
            else if (m_Town.Citoyens.Contains(from))
            {
                AddButton(30, 390, 0xFA2, 0xFA4, 103, GumpButtonType.Reply, 0);
                AddLabel(65, 390, 0, "Demenager");
            }
            else if (m_Town.Citoyens.Count < 499)
            {
                AddButton(30, 390, 0xFBD, 0xFBF, 101, GumpButtonType.Reply, 0);
                AddLabel(65, 390, 0, "Devenir Citoyen");
            }


            // Citoyens
            AddPage(20);

            AddLabel(17, 125, 0x25, "Bourgmestre");
            AddLabel(92, 125, 0x4D, "Conseiller");
            AddLabel(149, 125, 0x7E, "Ambassadeur");
            AddLabel(226, 125, 0x5A, "Capitaine");
            AddLabel(284, 125, 0x5B, "Garde");
            AddButton(319, 50, 0xFA8, 0xFAA, 0, GumpButtonType.Page, 8);
            AddLabel(350, 50, 0, "Elections");
            i = 0;
            x = 0;
            page = 0;
            nb_pages = m_Town.Citoyens.Count / 33;
            foreach (PlayerMobile mobile in m_Town.Citoyens)
            {
                int couleur = 0;
                if (m_Town.isMaire(mobile)) couleur = 0x25;
                if (m_Town.isConseiller(mobile)) couleur = 0x4D;
                if (m_Town.isAmbassadeur(mobile)) couleur = 0x7E;
                if (m_Town.isCapitaine(mobile)) couleur = 0x5A;
                if (m_Town.isGarde(mobile)) couleur = 0x5B;
                if (x == 33)
                {
                    if (page > 0 && page < nb_pages)
                    {
                        AddButton(60, 390, 0xFAE, 0x0FB0, 0, GumpButtonType.Page, 20 + page - 1); //previous
                        AddButton(160, 390, 0xFA5, 0x0FA7, 0, GumpButtonType.Page, 20 + page + 1); //next
                    }
                    if (page == 0 && nb_pages > 0)
                        AddButton(160, 390, 0xFA5, 0x0FA7, 0, GumpButtonType.Page, 20 + page + 1); //next
                    x = 0;
                    page++;
                    AddPage(20 + page);
                    if (page == nb_pages && nb_pages > 0)
                        AddButton(60, 390, 0xFAE, 0x0FB0, 0, GumpButtonType.Page, 20 + page - 1); //previous
                }
                AddLabel(20 + (((i / 11) % 3) * 125), 145 + ((i % 11) * 20), couleur, mobile.RawName);    // Scriptiz : vrai nom
                i++;
                x++;
            }

            // Gestion de la ville
            AddPage(3);

            AddLabel(15, 130, 0, "Ville de");
            if (IsGM)
            {
                AddTextEntry(70, 130, 100, 50, 0x384, 1, town.Nom);
                AddButton(170, 130, 2714, 2715, 301, GumpButtonType.Reply, 0); // Renomer la ville
                AddLabel(240, 130, 0, "Reinitialisation");
                AddButton(380, 130, 2714, 2715, 311, GumpButtonType.Reply, 0); // Reinit de la ville
                AddLabel(240, 150, 0, "Nomer le Bourgmestre");
                AddButton(380, 150, 2714, 2715, 302, GumpButtonType.Reply, 0); // Nomer le Maire
                AddLabel(240, 170, 0, "Forcer Elections");
                AddButton(380, 170, 2714, 2715, 312, GumpButtonType.Reply, 0); // Forcer les Elections

            }
            else
                AddLabel(70, 130, 0, town.Nom);
            if (m_Town.Maire == from && !IsGM)
            {
                AddLabel(250, 130, 0, "Refaire une cape");
                AddButton(350, 130, 2714, 2715, 310, GumpButtonType.Reply, 0);
            }
            if (isConseiller || IsGM)
            {
                AddLabel(45, 160, 0, "Liste des Citoyens"); //Liste des citoyens
                AddButton(20, 160, 2714, 2715, 0, GumpButtonType.Page, 60);
                AddLabel(45, 180, 0, "Liste des Candidats"); // Liste des Candidats
                AddButton(20, 180, 2714, 2715, 0, GumpButtonType.Page, 40);
                AddLabel(20, 240, 0, "Renvoyer");
                AddLabel(125, 240, 0, "Nomer l'Ambassadeur"); // Nommer l'Ambassadeur
                AddButton(100, 240, 2714, 2715, 304, GumpButtonType.Reply, 0);
                AddButton(75, 240, 0xA94, 0xA95, 307, GumpButtonType.Reply, 0);
                AddLabel(20, 260, 0, "Renvoyer");
                AddLabel(125, 260, 0, "Nomer le Capitaine de la Garde"); // Nommer le capitaine
                AddButton(100, 260, 2714, 2715, 305, GumpButtonType.Reply, 0);
                AddButton(75, 260, 0xA94, 0xA95, 308, GumpButtonType.Reply, 0);
                AddLabel(45, 280, 0, "Couleur de la Ville"); //Couleurs de la ville
                AddButton(20, 280, 2714, 2715, 309, GumpButtonType.Reply, 0);
                AddLabel(45, 300, 0, "Charte de la ville");
                AddButton(20, 300, 2714, 2715, 312, GumpButtonType.Page, 7);
                AddLabel(45, 320, 0, "Diplomatie");
                AddButton(20, 320, 2714, 2715, 312, GumpButtonType.Page, 90);
            }
            if (isMaire || IsGM)
            {
                AddLabel(20, 220, 0, "Renvoyer");
                AddLabel(125, 220, 0, "Nomer le Conseiller"); // Nommer le conseiller
                AddButton(100, 220, 2714, 2715, 303, GumpButtonType.Reply, 0);
                AddButton(75, 220, 0xA94, 0xA95, 306, GumpButtonType.Reply, 0);
            }

            if (isCapitaine || IsGM)
            {
                AddLabel(45, 200, 0, "Gestion des Hors la Loi"); //Gestion des HLL
                AddButton(20, 200, 2714, 2715, 0, GumpButtonType.Page, 50);
            }


            // Affiche les candidatures
            i = 0;
            x = 0;
            page = 0;
            nb_pages = m_Town.Candidats.Count / 36;
            AddPage(40);
            foreach (PlayerMobile mobile in m_Town.Candidats)
            {
                if (x == 36)
                {
                    if (page > 0 && page < nb_pages)
                    {
                        AddButton(60, 390, 0xFAE, 0x0FB0, 0, GumpButtonType.Page, 40 + page - 1); //previous
                        AddButton(160, 390, 0xFA5, 0x0FA7, 0, GumpButtonType.Page, 40 + page + 1); //next
                    }
                    if (page == 0 && nb_pages > 0)
                        AddButton(160, 390, 0xFA5, 0x0FA7, 0, GumpButtonType.Page, 40 + page + 1); //next
                    x = 0;
                    page++;
                    AddPage(40 + page);
                    if (page == nb_pages && nb_pages > 0)
                        AddButton(60, 390, 0xFAE, 0x0FB0, 0, GumpButtonType.Page, 40 + page - 1); //previous
                }
                AddLabel(60 + ((x / 12) * 125), 137 + ((x % 12) * 20), 0, mobile.RawName);  // Scriptiz : vrai nom
                AddButton(40 + ((x / 12) * 125), 137 + ((x % 12) * 20), 2714, 2715, 4000 + i, GumpButtonType.Reply, 0);
                AddButton(15 + ((x / 12) * 125), 137 + ((x % 12) * 20), 0xA94, 0xA95, 4500 + i, GumpButtonType.Reply, 0);
                i++;
                x++;
            }



            //Gestion des HLL
            AddPage(50);
            if (isCapitaine || IsGM)
            {
                AddLabel(15, 125, 0, "Ajouter                 aux Hors la loi de la ville");
                AddTextEntry(70, 125, 100, 50, 0x384, 2, "Entrer un nom");
                AddButton(170, 126, 2714, 2715, 551, GumpButtonType.Reply, 0);
            }
            else
                AddLabel(15, 125, 0, "Liste des Hors la loi de la ville");

            i = 0;
            x = 0;
            page = 0;
            nb_pages = m_Town.HLL.Count / 33;
            foreach (PlayerMobile mobile in m_Town.HLL)
            {
                if (x == 33)
                {
                    if (page > 0 && page < nb_pages)
                    {
                        AddButton(60, 390, 0xFAE, 0x0FB0, 0, GumpButtonType.Page, 50 + page - 1); //previous
                        AddButton(160, 390, 0xFA5, 0x0FA7, 0, GumpButtonType.Page, 50 + page + 1); //next
                    }
                    if (page == 0 && nb_pages > 0)
                        AddButton(160, 390, 0xFA5, 0x0FA7, 0, GumpButtonType.Page, 50 + page + 1); //next
                    x = 0;
                    page++;
                    AddPage(50 + page);
                    if (page == nb_pages && nb_pages > 0)
                        AddButton(60, 390, 0xFAE, 0x0FB0, 0, GumpButtonType.Page, 50 + page - 1); //previous
                }
                AddLabel(40 + ((x / 11) * 125), 145 + ((x % 11) * 20), 0, mobile.RawName);  // Scriptiz : on affiche le vrai nom !
                if (isCapitaine || IsGM) AddButton(15 + ((x / 11) * 125), 146 + ((x % 11) * 20), 0xA94, 0xA95, 5000 + i, GumpButtonType.Reply, 0);
                i++;
                x++;
            }

            //Liste des citoyens
            AddPage(60);

            i = 0;
            x = 0;
            page = 0;
            nb_pages = m_Town.Citoyens.Count / 36;

            foreach (PlayerMobile mobile in m_Town.Citoyens)
            {
                if (x == 36)
                {
                    if (page > 0 && page < nb_pages)
                    {
                        AddButton(60, 390, 0xFAE, 0x0FB0, 0, GumpButtonType.Page, 60 + page - 1); //previous
                        AddButton(160, 390, 0xFA5, 0x0FA7, 0, GumpButtonType.Page, 60 + page + 1); //next
                    }
                    if (page == 0 && nb_pages > 0)
                        AddButton(160, 390, 0xFA5, 0x0FA7, 0, GumpButtonType.Page, 60 + page + 1); //next
                    x = 0;
                    page++;
                    AddPage(60 + page);
                    if (page == nb_pages && nb_pages > 0)
                        AddButton(60, 390, 0xFAE, 0x0FB0, 0, GumpButtonType.Page, 60 + page - 1); //previous	
                }
                AddLabel(40 + ((x / 12) * 125), 137 + ((x % 12) * 20), 0, mobile.RawName);  // Scriptiz : vrai nom
                AddButton(15 + ((x / 12) * 125), 137 + ((x % 12) * 20), 0xA94, 0xA95, 6000 + i, GumpButtonType.Reply, 0);
                i++;
                x++;
            }

            //Charte de la ville
            AddPage(7);
            AddBackground(18, 138, 384, 24, 3000);
            AddTextEntry(20, 140, 380, 20, 0, 3, town.Charte0);
            AddBackground(18, 168, 384, 24, 3000);
            AddTextEntry(20, 170, 380, 20, 0, 4, town.Charte1);
            AddBackground(18, 198, 384, 24, 3000);
            AddTextEntry(20, 200, 380, 20, 0, 5, town.Charte2);
            AddBackground(18, 228, 384, 24, 3000);
            AddTextEntry(20, 230, 380, 20, 0, 6, town.Charte3);
            AddBackground(18, 258, 384, 24, 3000);
            AddTextEntry(20, 260, 380, 20, 0, 7, town.Charte4);
            AddBackground(18, 288, 384, 24, 3000);
            AddTextEntry(20, 290, 380, 20, 0, 8, town.Charte5);
            AddBackground(18, 318, 384, 24, 3000);
            AddTextEntry(20, 320, 380, 20, 0, 9, town.Charte6);
            AddBackground(18, 348, 384, 24, 3000);
            AddTextEntry(20, 350, 380, 20, 0, 10, town.Charte7);
            AddButton(50, 390, 4005, 4007, 701, GumpButtonType.Reply, 0);

            //Elections
            AddPage(8);
            AddLabel(15, 125, 0, String.Format("Fin des elections le {0} ", m_Town.EndDate));
            i = 0;

            if (!m_Town.Votants.Contains(from))
            {
                if (!m_Town.Elections.Contains(from) && m_Town.Elections.Count < 10)
                {
                    AddLabel(75, 390, 0, "Candidature");
                    AddButton(40, 390, 0xFBD, 0xFBF, 81, GumpButtonType.Reply, 0); //Candidature Maire
                }
            }
            else
            {
                AddLabel(60, 390, 0, "Vous etes loyal a " + ((PlayerMobile)m_Town.Resultats[m_Town.Votants.IndexOf(from)]).RawName);    // Scriptiz : vrai nom
            }
            foreach (PlayerMobile mobile in m_Town.Elections)
            {
                AddLabel(65, 150 + i * 20, 0, mobile.RawName);  // Scriptiz : vrai nom
                if (!m_Town.Votants.Contains(from))
                    AddButton(40, 150 + i * 20, 2714, 2715, 800 + i, GumpButtonType.Reply, 0);
                if (IsGM)
                    AddButton(15, 150 + i * 20, 0xA94, 0xA95, 850 + i, GumpButtonType.Reply, 0);
                i++;
            }

            //Diplomatie
            AddPage(90);

            AddLabel(20, 125, 0x25, "Guerre");
            AddLabel(63, 125, 0x4D, "Neutre");
            AddLabel(130, 125, 0x7E, "Alliance");
            AddLabel(210, 125, 0x5A, "Paix");

            i = 0;
            x = 0;
            page = 0;
            nb_pages = m_Town.Pomi.Villes.Count / 33;
            foreach (TownStone ville in m_Town.Pomi.Villes)
            {
                int couleur = 0;
                if (x == 33)
                {
                    if (page > 0 && page < nb_pages)
                    {
                        AddButton(60, 390, 0xFAE, 0x0FB0, 0, GumpButtonType.Page, 90 + page - 1); //previous
                        AddButton(160, 390, 0xFA5, 0x0FA7, 0, GumpButtonType.Page, 90 + page + 1); //next
                    }
                    if (page == 0 && nb_pages > 0)
                        AddButton(160, 390, 0xFA5, 0x0FA7, 0, GumpButtonType.Page, 90 + page + 1); //next
                    x = 0;
                    page++;
                    AddPage(90 + page);
                    if (page == nb_pages && nb_pages > 0)
                        AddButton(60, 390, 0xFAE, 0x0FB0, 0, GumpButtonType.Page, 90 + page - 1); //previous
                }
                if (m_Town.Guerre.Contains(ville)) couleur = 0x25;
                if (m_Town.Neutre.Contains(ville)) couleur = 0x4D;
                if (m_Town.Allies.Contains(ville)) couleur = 0x7E;
                if (m_Town.Paix.Contains(ville)) couleur = 0x5A;
                if (ville != m_Town)
                {
                    AddLabel(40 + ((x / 11) * 125), 145 + ((x % 11) * 20), couleur, ville.Nom);
                    AddButton(15 + ((x / 11) * 125), 146 + ((x % 11) * 20), 0xA94, 0xA95, 9000 + i, GumpButtonType.Reply, 0);
                    x++;
                }
                i++;

            }

        }

        public bool isGM(PlayerMobile from)
        {
            //            if (from.AccessLevel == AccessLevel.GameMaster|| from.AccessLevel == AccessLevel.Counselor||from.AccessLevel == AccessLevel.Administrator) 
            if (from.AccessLevel >= AccessLevel.GameMaster)
                return true;
            else
                return false;
        }

        public bool TestRace(Mobile from, TownStone m_Town)
        {
            string raceville = m_Town.VilleRace.ToString();
            string racejoueur = ((PlayerMobile)from).Race.Name;

            if (raceville == "Aucune")
                return true;
            else if (from.AccessLevel >= AccessLevel.GameMaster)
                return true;
            else if (raceville == "Humain" && ((racejoueur == "HumainHyel") || (racejoueur == "HumainOelm") || (racejoueur == "Voyageur")))
                return true;
            else if (raceville == "Drow" && racejoueur == "Drow")
                return true;
            else if (raceville == "ElfeGlace" && racejoueur == "ElfeGlace")
                return true;
            else if (raceville == "ElfeSylvain" && racejoueur == "ElfeSylvain")
                return true;
            else if (raceville == "HautElfe" && racejoueur == "HautElfe")
                return true;
            else if (raceville == "HautEtSylvain" && ((racejoueur == "HautElfe") || (racejoueur == "ElfeSylvain")))
                return true;
            else if (raceville == "Hobbit" && racejoueur == "Hobbit")
                return true;
            else if (raceville == "Nain" && ((racejoueur == "NainMontagne") || (racejoueur == "NainColline")))
                return true;
            else
                return false;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            try
            {
                if (!(sender.Mobile is PlayerMobile))
                {
                    World.Broadcast(0x35, false, "Quelqu'un chipote avec le POMI, merci de donner les détails à Scriptiz !");
                    return;
                }

                PlayerMobile from = sender.Mobile as PlayerMobile;

                switch (info.ButtonID)
                {
                    case 101: //Devenir Citoyen
                        {
                            if (!m_Town.InOtherTown(from))
                            {
                                if (TestRace(from, m_Town))
                                {
                                    m_Town.Candidats.Add(from);
                                    from.SendMessage("Vous etes candidat a la citoyenneté de " + m_Town.Nom);
                                }
                                else
                                {
                                    from.SendMessage(38, "Cette ville ne correspond pas à votre race et ne vous acceptera pas !");
                                    break;
                                }
                            }
                            else
                                from.SendMessage("Vous etes deja candidat ou citoyen d'une ville");
                            break;
                        }
                    case 102: //retirer sa candidature
                        {
                            if (m_Town.Candidats.Contains(from))
                                m_Town.Candidats.Remove(from);
                            break;
                        }
                    case 103: //Demenager
                        {
                            if (!m_Town.isMaire(from) &&
                               !m_Town.isConseiller(from) &&
                               !m_Town.isAmbassadeur(from) &&
                               !m_Town.isCapitaine(from) &&
                               !m_Town.Gardes.Contains(from) &&
                               !m_Town.Elections.Contains(from) &&
                               m_Town.Citoyens.Contains(from))
                            {
                                m_Town.Citoyens.Remove(from);
                            }
                            else
                                from.SendMessage("Vos engagements vous empechent de quitter la ville");
                            break;
                        }
                    case 104: //Diplomatie
                        {
                            from.SendGump(new DiplomatieGump(from, m_Town));
                            break;
                        }
                    case 301: // Renomer la ville 
                        {
                            m_Town.Nom = info.GetTextEntry(1).Text;
                            m_Town.Name = "Pierre de " + m_Town.Nom;
                            break;
                        }
                    case 302: //Nomer le Maire
                    case 303: //Nomer le conseiller
                    case 304: //Nomer l'Ambassadeur
                    case 305: //Nomer le Capitaine
                        {
                            from.Target = new GestionTarget(m_Town, info.ButtonID);
                            break;
                        }
                    case 306: //Renvoyer le conseiller
                        {
                            m_Town.Conseiller = null;
                            if (m_Town.ConseillerCloak != null)
                                m_Town.ConseillerCloak.Delete();
                            break;
                        }
                    case 307: //Renvoyer l'Ambassadeur
                        {
                            m_Town.Ambassadeur = null;
                            if (m_Town.AmbassadeurCloak != null)
                                m_Town.AmbassadeurCloak.Delete();
                            break;
                        }
                    case 308: //Renvoyer le Capitaine
                        {
                            m_Town.Capitaine = null;
                            if (m_Town.CapitaineCloak != null)
                                m_Town.CapitaineCloak.Delete();
                            if (m_Town.CptBook != null)
                                m_Town.CptBook.Delete();
                            break;
                        }
                    case 309: //changer les couleurs
                        {
                            from.SendHuePicker(new TownHuePicker(7974, m_Town));
                            break;
                        }
                    case 310: //nouvelle cape
                        {

                            if (m_Town.MaireCloak != null)
                                m_Town.MaireCloak.Delete();
                            m_Town.MaireCloak = new PomiCloak(m_Town, "Bourgmestre");
                            from.Backpack.DropItem(m_Town.MaireCloak);
                            break;


                        }
                    case 311: //Reinit de la ville
                        {
                            m_Town.Initialisation();
                            break;
                        }
                    case 312:
                        {
                            m_Town.ElecTimer.Stop();
                            //m_Town.ElecTimer = new ElectionTimer(m_Town, TimeSpan.FromSeconds( 5.0 ));
                            m_Town.ElecTimer.Start();
                            break;
                        }
                    //Ajout d'un HLL
                    case 551:
                        {
                            bool addok = false;
                            if (info.GetTextEntry(2) == null) return;   // Scriptiz : crash
                            foreach (Mobile mobile in World.Mobiles.Values)
                            {
                                // Scriptiz : correction d'un sale crash
                                if (mobile == null || !(mobile is PlayerMobile)) continue;

                                // Scriptiz : on traite le vrai nom ! (éviter les abus cagoules)
                                if (mobile.RawName.ToLower().Trim() == info.GetTextEntry(2).Text.ToLower().Trim())
                                {
                                    if (!m_Town.HLL.Contains((PlayerMobile)mobile))
                                    {
                                        m_Town.HLL.Add((PlayerMobile)mobile);
                                        addok = true;
                                    }
                                }
                            }
                            if (addok)
                                from.SendMessage(info.GetTextEntry(2).Text + " a ete ajout aux Hors la Loi");
                            else
                                from.SendMessage("Cette personne n'existe pas ou est deja Hors la Loi");
                            from.SendGump(new TownGump(from, m_Town));
                            break;
                        }
                    //Modif de la Charte
                    case 701:
                        {
                            m_Town.Charte0 = info.GetTextEntry(3).Text;
                            m_Town.Charte1 = info.GetTextEntry(4).Text;
                            m_Town.Charte2 = info.GetTextEntry(5).Text;
                            m_Town.Charte3 = info.GetTextEntry(6).Text;
                            m_Town.Charte4 = info.GetTextEntry(7).Text;
                            m_Town.Charte5 = info.GetTextEntry(8).Text;
                            m_Town.Charte6 = info.GetTextEntry(9).Text;
                            m_Town.Charte7 = info.GetTextEntry(10).Text;
                            from.SendGump(new TownGump(from, m_Town));
                            break;
                        }
                    //Candidat a la Mairie
                    case 81:
                        {
                            m_Town.Elections.Add(from);
                            from.SendGump(new TownGump(from, m_Town));
                            break;
                        }
                    default:
                        {
                            try
                            {
                                //Acceptation / refus des candidats
                                if (info.ButtonID >= 4000 && info.ButtonID < 5000)
                                {
                                    if (info.ButtonID < 4500)
                                    {
                                        m_Town.Citoyens.Add(m_Town.Candidats[info.ButtonID - 4000]);
                                        ((PlayerMobile)m_Town.Candidats[info.ButtonID - 4000]).SendMessage("Votre candidature à été acceptée");
                                        m_Town.Candidats.RemoveAt(info.ButtonID - 4000);
                                    }
                                    if (info.ButtonID > 4500)
                                    {
                                        ((PlayerMobile)m_Town.Candidats[info.ButtonID - 4500]).SendMessage("Votre candidature à été refusée");
                                        m_Town.Candidats.RemoveAt(info.ButtonID - 4500);
                                    }
                                }
                                //Suppression d'un HLL
                                if (info.ButtonID >= 5000 && info.ButtonID < 5500)
                                {
                                    m_Town.HLL.RemoveAt(info.ButtonID - 5000);
                                    from.SendGump(new TownGump(from, m_Town));
                                }
                                //Suppression d'un Citoyen
                                if (info.ButtonID >= 6000 && info.ButtonID < 6500)
                                {
                                    if (!m_Town.isMaire((PlayerMobile)m_Town.Citoyens[info.ButtonID - 6000]) &&
                                       !m_Town.isConseiller((PlayerMobile)m_Town.Citoyens[info.ButtonID - 6000]) &&
                                       !m_Town.isAmbassadeur((PlayerMobile)m_Town.Citoyens[info.ButtonID - 6000]) &&
                                       !m_Town.isCapitaine((PlayerMobile)m_Town.Citoyens[info.ButtonID - 6000]) &&
                                       !m_Town.Elections.Contains((PlayerMobile)m_Town.Citoyens[info.ButtonID - 6000]))
                                        m_Town.Citoyens.RemoveAt(info.ButtonID - 6000);
                                    else
                                        from.SendMessage("C'est un membre du conseil ou un candidat!");
                                }
                                //Elections
                                if (info.ButtonID >= 800 && info.ButtonID < 850)
                                {
                                    m_Town.Votants.Add(from);
                                    m_Town.Resultats.Add((PlayerMobile)m_Town.Elections[info.ButtonID - 800]);
                                }
                                if (info.ButtonID >= 850 && info.ButtonID < 900)
                                {

                                    foreach (PlayerMobile mobile in m_Town.Resultats)
                                    {
                                        if (((PlayerMobile)m_Town.Elections[info.ButtonID - 850]) == mobile)
                                        {
                                            m_Town.Votants.RemoveAt(m_Town.Resultats.IndexOf(mobile));
                                        }
                                    }
                                    while (m_Town.Resultats.Contains(m_Town.Elections[info.ButtonID - 850]))
                                    {
                                        m_Town.Resultats.Remove(m_Town.Elections[info.ButtonID - 850]);
                                    }
                                    m_Town.Elections.Remove(m_Town.Elections[info.ButtonID - 850]);
                                }

                                //diplomatie
                                if (info.ButtonID >= 9000 && info.ButtonID < 10000)
                                {
                                    from.SendGump(new ChangeStatusGump(m_Town, (TownStone)(m_Town.Pomi.Villes[info.ButtonID - 9000])));
                                    break;
                                }
                                if (info.ButtonID != 0) from.SendGump(new TownGump(from, m_Town));
                            }
                            catch
                            {
                                from.SendMessage("Quelqu'un a deja fait cette operation");
                            }
                            break;
                        }
                }
                //m_Town.AdminInUse = false;
            }
            catch
            {
                // Scriptiz : debug du POMI !
                using (StreamWriter sw = new StreamWriter("pomi_debug.log", true))
                {
                    string name = sender.Mobile != null ? sender.Mobile.RawName : "null";
                    string username = sender.Account != null ? sender.Account.Username : "null";
                    sw.WriteLine(String.Format("{0} : {1} ({2}) a envoyé {3}", DateTime.Now, name, username, info.ButtonID));
                    foreach (TextRelay tr in info.TextEntries)
                    {
                        sw.WriteLine(String.Format("\t{0} : {1}", tr.EntryID, tr.Text));
                    }
                }
            }
        }
    }

    public class TownHuePicker : HuePicker
    {
        private TownStone m_town;

        public TownHuePicker(int ItemID, TownStone town)
            : base(ItemID)
        {
            m_town = town;
        }
        public override void OnResponse(int hue)
        {
            //if (m_town.MaireCloak != null)
            //    m_town.MaireCloak.Hue = hue;
            //if (m_town.ConseillerCloak != null)
            //    m_town.ConseillerCloak.Hue = hue;
            //if (m_town.AmbassadeurCloak != null)
            //    m_town.AmbassadeurCloak.Hue = hue;
            //if (m_town.CapitaineCloak != null)
            //    m_town.CapitaineCloak.Hue = hue;
            //if (m_town.CptBook != null)
            //    m_town.CptBook.Hue = hue;
            //foreach (PomiCloak cloak in m_town.GardeCloak)
            //    cloak.Hue = hue;
            //PomiCloak pnjcloak;
            //Halberd arme;
            //foreach (GuardSpawner guard in m_town.GardesPNJ)
            //{
            //    try
            //    {
            //        arme = guard.SpawnedGuard.FindItemOnLayer(Layer.TwoHanded) as Halberd;
            //        pnjcloak = guard.SpawnedGuard.FindItemOnLayer(Layer.Cloak) as PomiCloak;
            //        if (pnjcloak != null)
            //            pnjcloak.Hue = hue;
            //        if (arme != null)
            //            arme.Hue = hue;
            //    }
            //    catch { }

            //}

            m_town.Hue = hue;
            //m_town.Box.Hue = hue;
        }
    }
}