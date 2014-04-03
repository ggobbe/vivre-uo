using System;
using System.Collections;
using Server;
using Server.Targeting;

namespace Knives.TownHouses
{
	public class TownHouseSetupGump : GumpPlusLight
	{
		public static Rectangle2D FixRect( Rectangle2D rect )
		{
			Point3D pointOne = Point3D.Zero;
			Point3D pointTwo = Point3D.Zero;

			if ( rect.Start.X < rect.End.X )
			{
				pointOne.X = rect.Start.X;
				pointTwo.X = rect.End.X;
			}
			else
			{
				pointOne.X = rect.End.X;
				pointTwo.X = rect.Start.X;
			}

			if ( rect.Start.Y < rect.End.Y )
			{
				pointOne.Y = rect.Start.Y;
				pointTwo.Y = rect.End.Y;
			}
			else
			{
				pointOne.Y = rect.End.Y;
				pointTwo.Y = rect.Start.Y;
			}

			return new Rectangle2D( pointOne, pointTwo );
		}

		public enum Page { Welcome, Blocks, Floors, Sign, Ban, LocSec, Items, Length, Price, Skills, Other, Other2 }
		public enum TargetType { BanLoc, SignLoc, MinZ, MaxZ, BlockOne, BlockTwo }

		private TownHouseSign c_Sign;
		private Page c_Page;
        private bool c_Quick;

		public TownHouseSetupGump( Mobile m, TownHouseSign sign ) : base( m, 50, 50 )
		{
			m.CloseGump( typeof( TownHouseSetupGump ) );

			c_Sign = sign;
		}

		protected override void BuildGump()
		{
			if ( c_Sign == null )
				return;

            int width = 300;
            int y = 0;

            if (c_Quick)
            {
                QuickPage(width, ref y);
            }
            else
            {
                switch (c_Page)
                {
                    case Page.Welcome: WelcomePage(width, ref y); break;
                    case Page.Blocks: BlocksPage(width, ref y); break;
                    case Page.Floors: FloorsPage(width, ref y); break;
                    case Page.Sign: SignPage(width, ref y); break;
                    case Page.Ban: BanPage(width, ref y); break;
                    case Page.LocSec: LocSecPage(width, ref y); break;
                    case Page.Items: ItemsPage(width, ref y); break;
                    case Page.Length: LengthPage(width, ref y); break;
                    case Page.Price: PricePage(width, ref y); break;
                    case Page.Skills: SkillsPage(width, ref y); break;
                    case Page.Other: OtherPage(width, ref y); break;
                    case Page.Other2: OtherPage2(width, ref y); break;
                }

                BuildTabs(width, ref y);
            }

            AddBackgroundZero(0, 0, width, y+=30, 0x13BE);

            if (c_Sign.PriceReady && !c_Sign.Owned)
            {
                AddBackground(width / 2 - 50, y, 100, 30, 0x13BE);
                AddHtml(width / 2 - 50 + 25, y + 5, 100, "Revendiquer Maison");
                AddButton(width / 2 - 50 + 5, y + 10, 0x837, 0x838, "Revendiquer", new GumpCallback(Claim));
            }
        }

		private void BuildTabs(int width, ref int y)
		{
			int x = 20;

            y += 30;

            AddButton(x-5, y - 3, 0x768, "Rapide", new GumpCallback(Quick));
            AddLabel(x, y - 3, c_Quick ? 0x34 : 0x47E, "Q");

            AddButton(x+=20, y, c_Page == Page.Welcome ? 0x939 : 0x93A, "Accueil", new GumpStateCallback(ChangePage), 0);
            AddButton(x += 20, y, c_Page == Page.Blocks ? 0x939 : 0x93A, "Page Blocks", new GumpStateCallback(ChangePage), 1);

			if ( c_Sign.BlocksReady )
				AddButton( x+=20, y, c_Page == Page.Floors ? 0x939 : 0x93A, "Page Etages", new GumpStateCallback( ChangePage ), 2 );

			if ( c_Sign.FloorsReady )
				AddButton( x+=20, y, c_Page == Page.Sign ? 0x939 : 0x93A, "Page Panneau", new GumpStateCallback( ChangePage ), 3 );

			if ( c_Sign.SignReady )
                AddButton(x += 20, y, c_Page == Page.Ban ? 0x939 : 0x93A, "Page Bans", new GumpStateCallback(ChangePage), 4);

			if ( c_Sign.BanReady )
                AddButton(x += 20, y, c_Page == Page.LocSec ? 0x939 : 0x93A, "Page LocSec", new GumpStateCallback(ChangePage), 5);

			if ( c_Sign.LocSecReady )
			{
                AddButton(x += 20, y, c_Page == Page.Items ? 0x939 : 0x93A, "Page Objets", new GumpStateCallback(ChangePage), 6);

				if ( !c_Sign.Owned )
                    AddButton(x += 20, y, c_Page == Page.Length ? 0x939 : 0x93A, "Page Duree", new GumpStateCallback(ChangePage), 7);
				else
					x+=20;

				AddButton( x+=20, y, c_Page == Page.Price ? 0x939 : 0x93A, "Page Prix", new GumpStateCallback( ChangePage ), 8 );
			}

			if ( c_Sign.PriceReady )
			{
                AddButton(x += 20, y, c_Page == Page.Skills ? 0x939 : 0x93A, "Page Skills", new GumpStateCallback(ChangePage), 9);
                AddButton(x += 20, y, c_Page == Page.Other ? 0x939 : 0x93A, "Autre Page", new GumpStateCallback(ChangePage), 10);
                AddButton(x += 20, y, c_Page == Page.Other2 ? 0x939 : 0x93A, "Autre Page 2", new GumpStateCallback(ChangePage), 11);
            }
		}

        private void QuickPage(int width, ref int y)
        {
            c_Sign.ClearPreview();

            AddHtml(0, y += 10, width, "<CENTER>Installation rapide");
            AddImage(width / 2 - 100, y + 2, 0x39);
            AddImage(width / 2 + 70, y + 2, 0x3B);

            AddButton(5, 5, 0x768, "Rapide", new GumpCallback(Quick));
            AddLabel(10, 5, c_Quick ? 0x34 : 0x47E, "Q");

            AddHtml(0, y += 25, width / 2 - 55, "<DIV ALIGN=RIGHT>Nom");
            AddTextField(width / 2 - 15, y, 100, 20, 0x480, 0xBBC, "Nom", c_Sign.Name);
            AddButton(width / 2 - 40, y + 3, 0x2716, "Name", new GumpCallback(Name));

            AddHtml(0, y += 25, width/2, "<CENTER>Ajouter zone");
            AddButton(width / 4 - 50, y + 3, 0x2716, "Ajouter zone", new GumpCallback(AddBlock));
            AddButton(width / 4 + 40, y + 3, 0x2716, "Ajouter zone", new GumpCallback(AddBlock));

            AddHtml(width/2, y, width/2, "<CENTER>Clear All");
            AddButton(width / 4 * 3 - 50, y + 3, 0x2716, "ClearAll", new GumpCallback(ClearAll));
            AddButton(width / 4 * 3 + 40, y + 3, 0x2716, "ClearAll", new GumpCallback(ClearAll));

            AddHtml(0, y += 25, width, "<CENTER>Etage Base: " + c_Sign.MinZ.ToString());
            AddButton(width / 2 - 80, y + 3, 0x2716, "Etage Base", new GumpCallback(MinZSelect));
            AddButton(width / 2 + 70, y + 3, 0x2716, "Etage Base", new GumpCallback(MinZSelect));

            AddHtml(0, y += 17, width, "<CENTER>Etage superieur: " + c_Sign.MaxZ.ToString());
            AddButton(width / 2 - 80, y + 3, 0x2716, "Etage superieur", new GumpCallback(MaxZSelect));
            AddButton(width / 2 + 70, y + 3, 0x2716, "Etage superieur", new GumpCallback(MaxZSelect));

            AddHtml(0, y += 25, width / 2, "<CENTER>Panneau Loc");
            AddButton(width / 4 - 50, y + 3, 0x2716, "Panneau Loc", new GumpCallback(SignLocSelect));
            AddButton(width / 4 + 40, y + 3, 0x2716, "Panneau Loc", new GumpCallback(SignLocSelect));

            AddHtml(width/2, y, width/2, "<CENTER>Ban Loc");
            AddButton(width / 4 * 3 - 50, y + 3, 0x2716, "Ban Loc", new GumpCallback(BanLocSelect));
            AddButton(width / 4 * 3 + 40, y + 3, 0x2716, "Ban Loc", new GumpCallback(BanLocSelect));

            AddHtml(0, y += 25, width, "<CENTER>Suggest Secures");
            AddButton(width / 2 - 70, y + 3, 0x2716, "Suggest LocSec", new GumpCallback(SuggestLocSec));
            AddButton(width / 2 + 60, y + 3, 0x2716, "Suggest LocSec", new GumpCallback(SuggestLocSec));

            AddHtml(0, y += 20, width / 2 - 20, "<DIV ALIGN=RIGHT>Secures");
            AddTextField(width / 2 + 20, y, 50, 20, 0x480, 0xBBC, "Secures", c_Sign.Secures.ToString());
            AddButton(width / 2 - 5, y + 3, 0x2716, "Secures", new GumpCallback(Secures));

            AddHtml(0, y += 22, width / 2 - 20, "<DIV ALIGN=RIGHT>Lockdowns");
            AddTextField(width / 2 + 20, y, 50, 20, 0x480, 0xBBC, "Lockdowns", c_Sign.Locks.ToString());
            AddButton(width / 2 - 5, y + 3, 0x2716, "Lockdowns", new GumpCallback(Lockdowns));

            AddHtml(0, y += 25, width, "<CENTER>Donner les objets a l'acheteur de la maison");
            AddButton(width / 2 - 110, y, c_Sign.KeepItems ? 0xD3 : 0xD2, "Garder objets", new GumpCallback(KeepItems));
            AddButton(width / 2 + 90, y, c_Sign.KeepItems ? 0xD3 : 0xD2, "Garder objets", new GumpCallback(KeepItems));

            if (c_Sign.KeepItems)
            {
                AddHtml(0, y += 25, width / 2 - 25, "<DIV ALIGN=RIGHT>Au Prix");
                AddTextField(width / 2 + 15, y, 70, 20, 0x480, 0xBBC, "PrixObjet", c_Sign.ItemsPrice.ToString());
                AddButton(width / 2 - 10, y + 5, 0x2716, "Prix Objet", new GumpCallback(ItemsPrice));
            }
            else
            {
                AddHtml(0, y += 25, width, "<CENTER>Ne pas supprimer les objets");
                AddButton(width / 2 - 110, y, c_Sign.LeaveItems ? 0xD3 : 0xD2, "LaisserObjets", new GumpCallback(LeaveItems));
                AddButton(width / 2 + 90, y, c_Sign.LeaveItems ? 0xD3 : 0xD2, "LaisserObjets", new GumpCallback(LeaveItems));
            }

            if (!c_Sign.Owned)
            {
                AddHtml(120, y += 25, 50, c_Sign.PriceType);
                AddButton(170, y + 8, 0x985, 0x985, "DureeUp", new GumpCallback(PriceUp));
                AddButton(170, y - 2, 0x983, 0x983, "DureeDown", new GumpCallback(PriceDown));
            }

            if (c_Sign.RentByTime != TimeSpan.Zero)
            {
                AddHtml(0, y += 25, width, "<CENTER>Location Recurente");
                AddButton(width / 2 - 80, y, c_Sign.RecurRent ? 0xD3 : 0xD2, "RecurLoc", new GumpCallback(RecurRent));
                AddButton(width / 2 + 60, y, c_Sign.RecurRent ? 0xD3 : 0xD2, "RecurLoc", new GumpCallback(RecurRent));

                if (c_Sign.RecurRent)
                {
                    AddHtml(0, y += 20, width, "<CENTER>Location-vente");
                    AddButton(width / 2 - 80, y, c_Sign.RentToOwn ? 0xD3 : 0xD2, "Location-vente", new GumpCallback(RentToOwn));
                    AddButton(width / 2 + 60, y, c_Sign.RentToOwn ? 0xD3 : 0xD2, "Location-vente", new GumpCallback(RentToOwn));
                }
            }

            AddHtml(0, y += 25, width, "<CENTER>Gratuit");
            AddButton(width / 2 - 80, y, c_Sign.Free ? 0xD3 : 0xD2, "Gratuit", new GumpCallback(Free));
            AddButton(width / 2 + 60, y, c_Sign.Free ? 0xD3 : 0xD2, "Gratuit", new GumpCallback(Free));

            if (!c_Sign.Free)
            {
                AddHtml(0, y += 25, width / 2 - 20, "<DIV ALIGN=RIGHT>" + c_Sign.PriceType + " Prix");
                AddTextField(width / 2 + 20, y, 70, 20, 0x480, 0xBBC, "Prix", c_Sign.Price.ToString());
                AddButton(width / 2 - 5, y + 5, 0x2716, "Prix", new GumpCallback(Price));

                AddHtml(0, y += 25, width, "<CENTER>Suggerer Prix");
                AddButton(width / 2 - 70, y + 3, 0x2716, "Suggerer", new GumpCallback(SuggestPrice));
                AddButton(width / 2 + 50, y + 3, 0x2716, "Suggerer", new GumpCallback(SuggestPrice));
            }
        }

		private void WelcomePage(int width, ref int y)
		{
            AddHtml(0, y += 10, width, "<CENTER>Bienvenue !");
            AddImage(width / 2 - 100, y + 2, 0x39);
            AddImage(width / 2 + 70, y + 2, 0x3B);

            string helptext = "";

            AddHtml(0, y += 25, width / 2 - 55, "<DIV ALIGN=RIGHT>Nom");
            AddTextField(width / 2 - 15, y, 100, 20, 0x480, 0xBBC, "Nom", c_Sign.Name);
            AddButton(width / 2 - 40, y + 3, 0x2716, "Nom", new GumpCallback(Name));

            if (c_Sign != null && c_Sign.Map != Map.Internal && c_Sign.RootParent == null)
            {
                AddHtml(0, y += 25, width, "<CENTER>AllerA");
                AddButton(width / 2 - 50, y + 3, 0x2716, "AllerA", new GumpCallback(Goto));
                AddButton(width / 2 + 40, y + 3, 0x2716, "AllerA", new GumpCallback(Goto));
            }

            if (c_Sign.Owned)
            {
                helptext = String.Format("Cette maison appartient à {0}, donc soyez conscient que changer quoi que ce soit"+
                 "grâce à ce menu va changer la maison elle-même! Vous pouvez ajouter plus d'espace, modifier la propriété" +
                 "les règles, presque tout! Vous ne pouvez pas, toutefois, changer le statut de location de la maison, beaucoup trop de" +
                 "possibilites de bugs. Si vous changez les restrictions et le propriétaire n'y correspond plus," +
                 "ils recevront l'avertissement de demolition  normal de 24 heures ", c_Sign.House.Owner.Name);

                AddHtml(10, y += 25, width - 20, 180, helptext, false, false);

                y += 180;
            }
            else
            {
                helptext = String.Format("Bienvenue dans le menu de configuration TownHouse! Ce menu vous guidera à travers" +
                 "chaque étape du processus d'installation. Vous pouvez configurer n'importe quelle zone d'une maison, ainsi que tout les détails de" +
                 "lockdowns, le prix, et si que vous souhaitez vendre ou louer la maison. Commençons ici par le nom de" +
                 "cette nouvelle Maison de Ville!");

                AddHtml(10, y += 25, width - 20, 130, helptext, false, false);

                y += 130;
            }

            AddHtml(width - 60, y+=15, 60, "Next");
            AddButton(width - 30, y, 0x15E1, 0x15E5, "Next", new GumpStateCallback(ChangePage), (int)c_Page + 1);
        }

		private void BlocksPage(int width, ref int y)
		{
			if ( c_Sign == null )
				return;

			c_Sign.ShowAreaPreview(Owner);

			AddHtml( 0, y+=10, width, "<CENTER>Creer la zone");
            AddImage(width / 2 - 100, y + 2, 0x39);
            AddImage(width / 2 + 70, y + 2, 0x3B);

			AddHtml( 0, y+=25, width, "<CENTER>Ajouter zone");
            AddButton(width / 2 - 50, y + 3, 0x2716, "Ajouterzone", new GumpCallback(AddBlock));
            AddButton(width / 2 + 40, y + 3, 0x2716, "Ajouterzone", new GumpCallback(AddBlock));

			AddHtml( 0, y+=20, width, "<CENTER>Clear All");
            AddButton(width / 2 - 50, y + 3, 0x2716, "ClearAll", new GumpCallback(ClearAll));
            AddButton(width / 2 + 40, y + 3, 0x2716, "ClearAll", new GumpCallback(ClearAll));

            string helptext = String.Format("  Le programme d'installation commence par définir la zone que vous souhaitez vendre ou louer " +
"Vous pouvez ajouter autant de blocs que vous le souhaitez, à chaque fois un aperçu montrera ce que" +
" vous avez sélectionné. Si vous avez envie de recommencer, il suffit de les faire disparaître! Vous devez avoir" +
"au moins un bloc défini avant de passer à l'étape suivante.");

			AddHtml( 10, y+=35, width-20, 140, helptext, false, false );

            y += 140;

            AddHtml(30, y += 15, 80, "Prec.");
            AddButton(10, y, 0x15E3, 0x15E7, "Prec.", new GumpStateCallback(ChangePage), (int)c_Page - 1);

            if (c_Sign.BlocksReady)
			{
				AddHtml( width-60, y, 60, "Next");
				AddButton( width-30, y, 0x15E1, 0x15E5, "Next", new GumpStateCallback( ChangePage ), (int)c_Page+1 );
			}
		}

		private void FloorsPage(int width, ref int y)
		{
            c_Sign.ShowFloorsPreview(Owner);

            AddHtml(0, y += 10, width, "<CENTER>Etages");
            AddImage(width / 2 - 100, y + 2, 0x39);
            AddImage(width / 2 + 70, y + 2, 0x3B);

            AddHtml(0, y += 25, width, "<CENTER>Etage de base: " + c_Sign.MinZ.ToString());
            AddButton(width / 2 - 80, y + 3, 0x2716, "Etagedebase", new GumpCallback(MinZSelect));
            AddButton(width / 2 + 70, y + 3, 0x2716, "Etagedebase", new GumpCallback(MinZSelect));

            AddHtml(0, y += 20, width, "<CENTER>Etage superieur: " + c_Sign.MaxZ.ToString());
            AddButton(width / 2 - 80, y + 3, 0x2716, "EtageSuperieur", new GumpCallback(MaxZSelect));
            AddButton(width / 2 + 70, y + 3, 0x2716, "EtageSuperieur", new GumpCallback(MaxZSelect));

			string helptext = String.Format(    "Maintenant, il vous faut cibler les étages que vous souhaitez vendre "+
            "Si vous voulez seulement un étage, vous pouvez ne pas cibler l'étage supérieur. Toutes choses entre la base" +
            "et le dernier étage sera inclue dans la maison, avec repercussions sur le prix.");

			AddHtml( 10, y+=35, width-20, 110, helptext, false, false);

            y += 110;

			AddHtml( 30, y+=15, 80, "Previous");
			AddButton( 10, y, 0x15E3, 0x15E7, "Previous", new GumpStateCallback( ChangePage ), (int)c_Page-1 );

			if ( c_Sign.FloorsReady )
			{
				AddHtml( width-60, y, 60, "Next");
				AddButton( width-30, y, 0x15E1, 0x15E5, "Next", new GumpStateCallback( ChangePage ), (int)c_Page+1 );
			}
		}

		private void SignPage(int width, ref int y)
		{
			if ( c_Sign == null )
				return;

			c_Sign.ShowSignPreview();

			AddHtml( 0, y+=10, width, "<CENTER>Emplacement Panneau");
            AddImage(width / 2 - 100, y + 2, 0x39);
            AddImage(width / 2 + 70, y + 2, 0x3B);

			AddHtml( 0, y+=25, width, "<CENTER>Definir Emplacement");
            AddButton(width / 2 - 60, y + 3, 0x2716, "EmplacementPanneau", new GumpCallback(SignLocSelect));
            AddButton(width / 2 + 50, y + 3, 0x2716, "EmplacementPanneau", new GumpCallback(SignLocSelect));

			string helptext = String.Format( "Avec ce panneau, le propriétaire aura les memes droits de possession "+
"que sur une autre maison. Si il utilisent le panneau de démolition de la maison, il retournera automatiquement" +
"a la vente ou à location. Le panneau joueurs à utiliser pour acheter la maison apparaît dans le même" +
"endroit, légèrement en dessous de la plaque de maison normale" );

			AddHtml( 10, y+=35, width-20, 130, helptext, false, false);

            y += 130;

			AddHtml( 30, y+=15, 80, "Previous");
			AddButton( 10, y, 0x15E3, 0x15E7, "Previous", new GumpStateCallback( ChangePage ), (int)c_Page-1 );

			if ( c_Sign.SignReady )
			{
				AddHtml( width-60, y, 60, "Next");
				AddButton( width-30, y, 0x15E1, 0x15E5, "Next", new GumpStateCallback( ChangePage ), (int)c_Page+1 );
			}
		}

		private void BanPage(int width, ref int y)
		{
			if ( c_Sign == null )
				return;

			c_Sign.ShowBanPreview();

			AddHtml( 0, y+=10, width, "<CENTER>Emplacement de Ban");
            AddImage(width / 2 - 100, y + 2, 0x39);
            AddImage(width / 2 + 70, y + 2, 0x3B);

			AddHtml( 0, y+=25, width, "<CENTER>Definir emplacement");
            AddButton(width / 2 - 60, y + 3, 0x2716, "EmplacementdeBan", new GumpCallback(BanLocSelect));
            AddButton(width / 2 + 50, y + 3, 0x2716, "EmplacementdeBan", new GumpCallback(BanLocSelect));

			string helptext = String.Format( "L'emplacement de Ban détermine où un joueur est envoyé quand il est éjecté ou " +
"banni d'une maison. Si vous n'avez jamais defini cela, il ira dans le coin sud-ouest a l'extérieur" +
                 "de la maison");

			AddHtml( 10, y+=35, width-20, 100, helptext, false, false );

            y += 100;

			AddHtml( 30, y+=15, 80, "Previous");
			AddButton( 10, y, 0x15E3, 0x15E7, "Previous", new GumpStateCallback( ChangePage ), (int)c_Page-1 );

			if ( c_Sign.BanReady )
			{
				AddHtml( width-60, y, 60, "Next");
				AddButton( width-30, y, 0x15E1, 0x15E5, "Next", new GumpStateCallback( ChangePage ), (int)c_Page+1 );
			}
		}

		private void LocSecPage(int width, ref int y)
		{
			AddHtml( 0, y+=10, width, "<CENTER>Lockdowns et Secures");
            AddImage(width / 2 - 100, y + 2, 0x39);
            AddImage(width / 2 + 70, y + 2, 0x3B);

            AddHtml(0, y+=25, width, "<CENTER>Suggerer");
            AddButton(width / 2 - 50, y + 3, 0x2716, "SuggererLocSec", new GumpCallback(SuggestLocSec));
            AddButton(width / 2 + 40, y + 3, 0x2716, "SuggererLocSec", new GumpCallback(SuggestLocSec));

            AddHtml(0, y += 25, width / 2 - 20, "<DIV ALIGN=RIGHT>Secures");
			AddTextField( width/2+20, y, 50, 20, 0x480, 0xBBC, "Secures", c_Sign.Secures.ToString() );
            AddButton(width / 2 - 5, y + 3, 0x2716, "Secures", new GumpCallback(Secures));

			AddHtml( 0, y+=25, width/2-20, "<DIV ALIGN=RIGHT>Lockdowns");
			AddTextField( width/2+20, y, 50, 20, 0x480, 0xBBC, "Lockdowns", c_Sign.Locks.ToString() );
            AddButton(width / 2 - 5, y + 3, 0x2716, "Lockdowns", new GumpCallback(Lockdowns));

            string helptext = String.Format("Avec cette étape vous définissez la quantité de stockage pour la maison, ou emplacement " +
"le système peut le faire pour que vous en utilisant le bouton Suggérer. En général, les joueurs obtiennent la moitié du nombre de lockdowns" +
"en secure.");

			AddHtml( 10, y+=35, width-20, 90, helptext, false, false );

            y += 90;

			AddHtml( 30, y+=15, 80, "Previous");
			AddButton( 10, y, 0x15E3, 0x15E7, "Previous", new GumpStateCallback( ChangePage ), (int)c_Page-1 );

			if ( c_Sign.LocSecReady )
			{
				AddHtml( width-60, y, 60, "Next");
				AddButton( width-30, y, 0x15E1, 0x15E5, "Next", new GumpStateCallback( ChangePage ), (int)c_Page+1 );
			}
		}

		private void ItemsPage(int width, ref int y)
		{
			AddHtml( 0, y+=10, width, "<CENTER>Objet de Decoration");
            AddImage(width / 2 - 100, y + 2, 0x39);
            AddImage(width / 2 + 70, y + 2, 0x3B);

			AddHtml( 0, y+=25, width, "<CENTER>Donner a l'acheteur les objets dans la maison");
            AddButton(width / 2 - 110, y, c_Sign.KeepItems ? 0xD3 : 0xD2, "Garderobjets", new GumpCallback(KeepItems));
            AddButton(width / 2 + 90, y, c_Sign.KeepItems ? 0xD3 : 0xD2, "Garderobjets", new GumpCallback(KeepItems));

			if ( c_Sign.KeepItems )
			{
				AddHtml( 0, y+=25, width/2-25, "<DIV ALIGN=RIGHT>aux prix de");
				AddTextField( width/2+15, y, 70, 20, 0x480, 0xBBC, "coutObjet", c_Sign.ItemsPrice.ToString());
                AddButton(width / 2 - 10, y + 5, 0x2716, "coutObjet", new GumpCallback(ItemsPrice));
			}
			else
			{
				AddHtml( 0, y+=25, width, "<CENTER>Ne pas supprimer les objets");
                AddButton(width / 2 - 110, y, c_Sign.LeaveItems ? 0xD3 : 0xD2, "LaisserObjets", new GumpCallback(LeaveItems));
                AddButton(width / 2 + 90, y, c_Sign.LeaveItems ? 0xD3 : 0xD2, "LaisserObjets", new GumpCallback(LeaveItems));
            }

            string helptext = String.Format("Par défaut, le système permet de supprimer tous les objets non-statique déjà " +
"presents dans la maison au moment de l'achat. Ces éléments sont communément appelés Objets de décoration." +
"Ils ne comprennent pas les addons de maison, comme les forges, etc. Ils comprennent les conteneurs. Vous pouvez ici" +
"permettre aux joueurs de garder ces objets, et vous pouvez également les charger de le faire!");

			AddHtml( 10, y+=35, width-20, 160, helptext, false, false );
            
            y+=160;

			AddHtml( 30, y+=15, 80, "Previous");
			AddButton( 10, y, 0x15E3, 0x15E7, "Previous", new GumpStateCallback( ChangePage ), (int)c_Page-1 );

			if ( c_Sign.ItemsReady )
			{
				AddHtml( width-60, y, 60, "Next");
				AddButton( width-30, y, 0x15E1, 0x15E5, "Next", new GumpStateCallback( ChangePage ), (int)c_Page + ( c_Sign.Owned ? 2: 1 ) );
			}
		}

		private void LengthPage(int width, ref int y)
		{
			AddHtml( 0, y+=10, width, "<CENTER>Acheter ou Louer" );
            AddImage(width / 2 - 100, y + 2, 0x39);
            AddImage(width / 2 + 70, y + 2, 0x3B);

			AddHtml( 120, y+=25, 50, c_Sign.PriceType);
			AddButton( 170, y+8, 0x985, 0x985, "DureeUp", new GumpCallback( PriceUp ) );
			AddButton( 170, y-2, 0x983, 0x983, "DureeDown", new GumpCallback( PriceDown ) );

			if ( c_Sign.RentByTime != TimeSpan.Zero )
			{
				AddHtml( 0, y+=25, width, "<CENTER>Location recurente");
                AddButton(width / 2 - 80, y, c_Sign.RecurRent ? 0xD3 : 0xD2, "RecurLoc", new GumpCallback(RecurRent));
                AddButton(width / 2 + 60, y, c_Sign.RecurRent ? 0xD3 : 0xD2, "RecurLoc", new GumpCallback(RecurRent));

				if ( c_Sign.RecurRent )
				{
                    AddHtml(0, y += 20, width, "<CENTER>Location-vente");
                    AddButton(width / 2 - 80, y, c_Sign.RentToOwn ? 0xD3 : 0xD2, "Location-vente", new GumpCallback(RentToOwn));
                    AddButton(width / 2 + 60, y, c_Sign.RentToOwn ? 0xD3 : 0xD2, "Location-vente", new GumpCallback(RentToOwn));
                }
			}

			string helptext = String.Format( "On s'approche de la fin du programme d'installation! Maintenant vous pouvez spécifier si "+
"ceci est une propriété en achat ou en location. Il suffit d'utiliser les flèches jusqu'à ce que que vous ayez le réglage que vous désirez. Pour" +
"la location, vous pouvez également procéder à l'achat non-recuit, ce qui signifie qu'après que le temps soit écoulé le joueur" +
"est a la rue! Avec récurrents, s'il a de l'argent disponible, il peut continuer à louer. Vous pouvez" +
"également faire de la Location-Vente, permettant au joueur de posséder le bien, après deux mois de paiements");

			AddHtml( 10, y+=35, width-20, 160, helptext, false, true );

            y += 160;

			AddHtml( 30, y+=15, 80, "Previous");
			AddButton( 10, y, 0x15E3, 0x15E7, "Previous", new GumpStateCallback( ChangePage ), (int)c_Page-1 );

			if ( c_Sign.LengthReady )
			{
				AddHtml( width-60, y, 60, "Next");
				AddButton( width-30, y, 0x15E1, 0x15E5, "Next", new GumpStateCallback( ChangePage ), (int)c_Page+1 );
			}
		}

		private void PricePage(int width, ref int y)
		{
			AddHtml( 0, y+=10, width, "<CENTER>Prix");
            AddImage(width / 2 - 100, y + 2, 0x39);
            AddImage(width / 2 + 70, y + 2, 0x3B);

			AddHtml( 0, y+=25, width, "<CENTER>Gratuit");
            AddButton(width / 2 - 80, y, c_Sign.Free ? 0xD3 : 0xD2, "Gratuit", new GumpCallback(Free));
            AddButton(width / 2 + 60, y, c_Sign.Free ? 0xD3 : 0xD2, "Gratuit", new GumpCallback(Free));

			if ( !c_Sign.Free )
			{
				AddHtml( 0, y+=25, width/2-20, "<DIV ALIGN=RIGHT>" + c_Sign.PriceType + " Prix");
				AddTextField( width/2+20, y, 70, 20, 0x480, 0xBBC, "Prix", c_Sign.Price.ToString());
                AddButton(width / 2 - 5, y + 5, 0x2716, "Prix", new GumpCallback(Price));

				AddHtml( 0, y+=20, width, "<CENTER>Suggest");
                AddButton(width / 2 - 50, y + 3, 0x2716, "Suggerer", new GumpCallback(SuggestPrice));
                AddButton(width / 2 + 40, y + 3, 0x2716, "Suggerer", new GumpCallback(SuggestPrice));
			}

            string helptext = String.Format("    Maintenant vous pouvez definir le prix de la maison. Rappelez-vous, s'il s'agit d'une " +
"location, le système debitera ce montant au joueur pour chaque période! Heureusement, la suggestion" +
"le prend en compte. Si que vous n'avez pas envie de chercher, laissez le système propose un prix pour vous." +
"Vous pouvez aussi offrir la maison avec l'option gratuit");

			AddHtml( 10, y+=35, width-20, 130, helptext, false, false );

            y += 130;

			AddHtml( 30, y+=15, 80, "Previous");
			AddButton( 10, y, 0x15E3, 0x15E7, "Previous", new GumpStateCallback( ChangePage ), (int)c_Page - ( c_Sign.Owned ? 2 : 1 ) );

			if ( c_Sign.PriceReady )
			{
				AddHtml( width-60, y, 60, "Next");
				AddButton( width-30, y, 0x15E1, 0x15E5, "Next", new GumpStateCallback( ChangePage ), (int)c_Page+1 );
			}
		}

		private void SkillsPage(int width, ref int y)
		{
			AddHtml( 0, y+=10, width, "<CENTER>Restictions de competencess");
            AddImage(width / 2 - 100, y + 2, 0x39);
            AddImage(width / 2 + 70, y + 2, 0x3B);

			AddHtml( 0, y+=25, width/2-20, "<DIV ALIGN=RIGHT>Competence");
            AddTextField(width / 2 + 20, y, 100, 20, 0x480, 0xBBC, "Competence", c_Sign.Skill.ToString());
            AddButton(width / 2 - 5, y + 5, 0x2716, "Competence", new GumpCallback(Skill));

            AddHtml(0, y+=25, width / 2 - 20, "<DIV ALIGN=RIGHT>Amount");
            AddTextField(width / 2 + 20, y, 50, 20, 0x480, 0xBBC, "SkillReq", c_Sign.SkillReq.ToString());
            AddButton(width / 2 - 5, y + 5, 0x2716, "Skill", new GumpCallback(Skill));

            AddHtml(0, y += 25, width/2-20, "<DIV ALIGN=RIGHT>Min Total");
            AddTextField(width / 2 + 20, y, 60, 20, 0x480, 0xBBC, "MinTotalSkill", c_Sign.MinTotalSkill.ToString());
            AddButton(width / 2 - 5, y + 5, 0x2716, "Skill", new GumpCallback(Skill));

			AddHtml( 0, y+=25, width/2-20, "<DIV ALIGN=RIGHT>Max Total");
            AddTextField(width / 2 + 20, y, 60, 20, 0x480, 0xBBC, "MaxTotalSkill", c_Sign.MaxTotalSkill.ToString());
            AddButton(width / 2 - 5, y + 5, 0x2716, "Skill", new GumpCallback(Skill));

            string helptext = String.Format(" Ces paramètres sont tous facultatifs. Si vous souhaitez restreindre qui peut être propriétaire " +
"de cette maison par les compétences, c'est ici. Vous pouvez spécifier le nom et la valeur des compétences, ou par" +
"niveau de compétence totale");

			AddHtml( 10, y+=35, width-20, 90, helptext, false, false );

            y += 90;

			AddHtml( 30, y+=15, 80, "Previous");
			AddButton( 10, y, 0x15E3, 0x15E7, "Previous", new GumpStateCallback( ChangePage ), (int)c_Page-1 );

			if ( c_Sign.PriceReady )
			{
				AddHtml( width-60, y, 60, "Next");
				AddButton( width-30, y, 0x15E1, 0x15E5, "Next", new GumpStateCallback( ChangePage ), (int)c_Page+1 );
			}
		}

        private void OtherPage(int width, ref int y)
        {
            AddHtml(0, y += 10, width, "<CENTER>Autres Options");
            AddImage(width / 2 - 100, y + 2, 0x39);
            AddImage(width / 2 + 70, y + 2, 0x3B);

            AddHtml(0, y += 25, width, "<CENTER>Young");
            AddButton(width / 2 - 80, y, c_Sign.YoungOnly ? 0xD3 : 0xD2, "Young Only", new GumpCallback(Young));
            AddButton(width / 2 + 60, y, c_Sign.YoungOnly ? 0xD3 : 0xD2, "Young Only", new GumpCallback(Young));

            if (!c_Sign.YoungOnly)
            {
                AddHtml(0, y += 25, width, "<CENTER>Innocents");
                AddButton(width / 2 - 80, y, c_Sign.Murderers == Intu.No ? 0xD3 : 0xD2, "Pas d'Assassins", new GumpStateCallback(Murderers), Intu.No);
                AddButton(width / 2 + 60, y, c_Sign.Murderers == Intu.No ? 0xD3 : 0xD2, "Pas d'Assassins", new GumpStateCallback(Murderers), Intu.No);
                AddHtml(0, y += 20, width, "<CENTER>Assassins");
                AddButton(width / 2 - 80, y, c_Sign.Murderers == Intu.Yes ? 0xD3 : 0xD2, "Assassins", new GumpStateCallback(Murderers), Intu.Yes);
                AddButton(width / 2 + 60, y, c_Sign.Murderers == Intu.Yes ? 0xD3 : 0xD2, "Assassins", new GumpStateCallback(Murderers), Intu.Yes);
                AddHtml(0, y += 20, width, "<CENTER>Tous");
                AddButton(width / 2 - 80, y, c_Sign.Murderers == Intu.Neither ? 0xD3 : 0xD2, "Ni Assassins", new GumpStateCallback(Murderers), Intu.Neither);
                AddButton(width / 2 + 60, y, c_Sign.Murderers == Intu.Neither ? 0xD3 : 0xD2, "Ni Assassins", new GumpStateCallback(Murderers), Intu.Neither);
            }

            AddHtml(0, y += 25, width, "<CENTER>Verrouiller et Demolir");
            AddButton(width / 2 - 110, y, c_Sign.Relock ? 0xD3 : 0xD2, "Reverouiller", new GumpCallback(Relock));
            AddButton(width / 2 + 90, y, c_Sign.Relock ? 0xD3 : 0xD2, "Reverouiller", new GumpCallback(Relock));

            string helptext = String.Format(" Ces options sont également facultatives. Avec l'option Young, vous pouvez restreindre "+
                 "l'achat de la maison aux Youngs uniquement. De même, vous pouvez spécifier si des assassins ou des innocents sont" +
                 "autoristes a etre propriétaires de la maison. Vous pouvez également spécifier si les portes au sein de " +
                 "la maison sont verrouillées lorsque le propriétaire démolit ses biens");

            AddHtml(10, y += 35, width - 20, 180, helptext, false, false);

            y += 180;

            AddHtml(30, y += 15, 80, "Previous");
            AddButton(10, y, 0x15E3, 0x15E7, "Previous", new GumpStateCallback(ChangePage), (int)c_Page - 1);

            AddHtml(width - 60, y, 60, "Next");
            AddButton(width - 30, y, 0x15E1, 0x15E5, "Next", new GumpStateCallback(ChangePage), (int)c_Page + 1);
        }

        private void OtherPage2(int width, ref int y)
        {
            AddHtml(0, y += 10, width, "<CENTER>Other Options 2");
            AddImage(width / 2 - 100, y + 2, 0x39);
            AddImage(width / 2 + 70, y + 2, 0x3B);

            AddHtml(0, y += 25, width, "<CENTER>Force Public");
            AddButton(width / 2 - 110, y, c_Sign.ForcePublic ? 0xD3 : 0xD2, "Public", new GumpCallback(ForcePublic));
            AddButton(width / 2 + 90, y, c_Sign.ForcePublic ? 0xD3 : 0xD2, "Public", new GumpCallback(ForcePublic));

            AddHtml(0, y += 25, width, "<CENTER>Force Private");
            AddButton(width / 2 - 110, y, c_Sign.ForcePrivate ? 0xD3 : 0xD2, "Private", new GumpCallback(ForcePrivate));
            AddButton(width / 2 + 90, y, c_Sign.ForcePrivate ? 0xD3 : 0xD2, "Private", new GumpCallback(ForcePrivate));

            AddHtml(0, y += 25, width, "<CENTER>No Trading");
            AddButton(width / 2 - 110, y, c_Sign.NoTrade ? 0xD3 : 0xD2, "NoTrade", new GumpCallback(NoTrade));
            AddButton(width / 2 + 90, y, c_Sign.NoTrade ? 0xD3 : 0xD2, "NoTrade", new GumpCallback(NoTrade));

            AddHtml(0, y += 25, width, "<CENTER>No Banning");
            AddButton(width / 2 - 110, y, c_Sign.NoBanning ? 0xD3 : 0xD2, "NoBan", new GumpCallback(NoBan));
            AddButton(width / 2 + 90, y, c_Sign.NoBanning ? 0xD3 : 0xD2, "NoBan", new GumpCallback(NoBan));

            string helptext = String.Format("Une autre page d'options en option! Parfois, des maisons ont des caractéristiques a soustraire aux joueurs " +
                 "Vous pouvez forcer les maisons en mode privé ou publique. Vous pouvez aussi éviter que la session de la maison. Enfin, vous pouvez supprimer leur capacité à bannir les joueurs");

            AddHtml(10, y += 35, width - 20, 180, helptext, false, false);

            y += 180;

            AddHtml(30, y += 15, 80, "Previous");
            AddButton(10, y, 0x15E3, 0x15E7, "Previous", new GumpStateCallback(ChangePage), (int)c_Page - 1);
        }

        private bool SkillNameExists(string text)
		{
			try
			{
				SkillName index = (SkillName)Enum.Parse( typeof( SkillName ), text, true );
				return true;
			}
			catch
			{
                Owner.SendMessage("Vous avez fourni un nom de compétence invalide.");	
				return false;
			}
		}

		private void ChangePage( object obj )
		{
			if ( c_Sign == null )
				return;

			if ( !(obj is int) )
				return;

			c_Page = (Page)(int)obj;

			c_Sign.ClearPreview();

			NewGump();
		}

        private void Name()
        {
            c_Sign.Name = GetTextField("Nom");
            Owner.SendMessage("Nom defini!");
            NewGump();
        }

        private void Goto()
        {
            Owner.Location = c_Sign.Location;
            Owner.Z += 5;
            Owner.Map = c_Sign.Map;

            NewGump();
        }

        private void Quick()
        {
            c_Quick = !c_Quick;
            NewGump();
        }

		private void BanLocSelect()
		{
			Owner.SendMessage( "Ciblez l'emplacement des bannis." );
			Owner.Target = new InternalTarget( this, c_Sign, TargetType.BanLoc );
		}

		private void SignLocSelect()
		{
            Owner.SendMessage("Ciblez de l'emplacement pour le panneau d'accueil.");
			Owner.Target = new InternalTarget( this, c_Sign, TargetType.SignLoc );
		}

		private void MinZSelect()
		{
			Owner.SendMessage( "Ciblez le plancher de base." );
			Owner.Target = new InternalTarget( this, c_Sign, TargetType.MinZ );
		}

		private void MaxZSelect()
		{
			Owner.SendMessage( "Ciblez le plancher le plus haut." );
			Owner.Target = new InternalTarget( this, c_Sign, TargetType.MaxZ );
		}

		private void Young()
		{
			c_Sign.YoungOnly = !c_Sign.YoungOnly;
			NewGump();
		}

		private void Murderers( object obj )
		{
			if ( !(obj is Intu) )
				return;

			c_Sign.Murderers = (Intu)obj;

			NewGump();
		}

        private void Relock()
        {
            c_Sign.Relock = !c_Sign.Relock;
            NewGump();
        }

        private void ForcePrivate()
        {
            c_Sign.ForcePrivate = !c_Sign.ForcePrivate;
            NewGump();
        }

        private void ForcePublic()
        {
            c_Sign.ForcePublic = !c_Sign.ForcePublic;
            NewGump();
        }

        private void NoTrade()
        {
            c_Sign.NoTrade = !c_Sign.NoTrade;
            NewGump();
        }

        private void NoBan()
        {
            c_Sign.NoBanning = !c_Sign.NoBanning;
            NewGump();
        }

        private void KeepItems()
		{
			c_Sign.KeepItems = !c_Sign.KeepItems;
			NewGump();
		}

		private void LeaveItems()
		{
			c_Sign.LeaveItems = !c_Sign.LeaveItems;
			NewGump();
		}

        private void ItemsPrice()
        {
            c_Sign.ItemsPrice = GetTextFieldInt("PrixObjet");
            Owner.SendMessage("Prix objet defini!");
            NewGump();
        }

		private void RecurRent()
		{
			c_Sign.RecurRent = !c_Sign.RecurRent;
			NewGump();
		}

		private void RentToOwn()
		{
			c_Sign.RentToOwn = !c_Sign.RentToOwn;
			NewGump();
		}

        private void Skill()
        {
            if (GetTextField("Competence") != "" && SkillNameExists(GetTextField("Competence")))
                c_Sign.Skill = GetTextField("Competence");
            else
                c_Sign.Skill = "";

            c_Sign.SkillReq = GetTextFieldInt("SkillReq");
            c_Sign.MinTotalSkill = GetTextFieldInt("MinTotalSkill");
            c_Sign.MaxTotalSkill = GetTextFieldInt("MaxTotalSkill");

            Owner.SendMessage("Infos competences definies!");

            NewGump();
        }

		private void Claim()
		{
			new TownHouseConfirmGump( Owner, c_Sign );
            OnClose();
		}

		private void SuggestLocSec()
		{
			int price = c_Sign.CalcVolume()*General.SuggestionFactor;
			c_Sign.Secures = price/75;
			c_Sign.Locks = c_Sign.Secures/2;

			NewGump();
		}

        private void Secures()
        {
            c_Sign.Secures = GetTextFieldInt("Secures");
            Owner.SendMessage("Secures set!");
            NewGump();
        }

        private void Lockdowns()
        {
            c_Sign.Locks = GetTextFieldInt("Lockdowns");
            Owner.SendMessage("Lockdowns set!");
            NewGump();
        }

        private void SuggestPrice()
		{
			c_Sign.Price = c_Sign.CalcVolume()*General.SuggestionFactor;

			if ( c_Sign.RentByTime == TimeSpan.FromDays( 1 ) )
				c_Sign.Price /= 60;
			if ( c_Sign.RentByTime == TimeSpan.FromDays( 7 ) )
				c_Sign.Price = (int)((double)c_Sign.Price/8.57);
			if ( c_Sign.RentByTime == TimeSpan.FromDays( 30 ) )
				c_Sign.Price /= 2;

			NewGump();
		}

        private void Price()
        {
            c_Sign.Price = GetTextFieldInt("Prix");
            Owner.SendMessage("Prix defini!");
            NewGump();
        }

		private void Free()
		{
			c_Sign.Free = !c_Sign.Free;
			NewGump();
		}

		private void AddBlock()
		{
			if ( c_Sign == null )
				return;

            Owner.SendMessage("Cibler le coin nord-ouest.");
			Owner.Target = new InternalTarget( this, c_Sign, TargetType.BlockOne );
		}

		private void ClearAll()
		{
			if ( c_Sign == null )
				return;

			c_Sign.Blocks.Clear();
			c_Sign.ClearPreview();
			c_Sign.UpdateBlocks();

			NewGump();
		}

		private void PriceUp()
		{
			c_Sign.NextPriceType();
			NewGump();
		}

		private void PriceDown()
		{
			c_Sign.PrevPriceType();
			NewGump();
		}

        protected override void OnClose()
        {
            c_Sign.ClearPreview();
        }


		private class InternalTarget : Target
		{
			private TownHouseSetupGump c_Gump;
			private TownHouseSign c_Sign;
			private TargetType  c_Type;
			private Point3D c_BoundOne;

			public InternalTarget( TownHouseSetupGump gump, TownHouseSign sign, TargetType type ) : this( gump, sign, type, Point3D.Zero ){}

			public InternalTarget( TownHouseSetupGump gump, TownHouseSign sign, TargetType type, Point3D point ) : base( 20, true, TargetFlags.None )
			{
				c_Gump = gump;
				c_Sign = sign;
				c_Type = type;
				c_BoundOne = point;
			}

			protected override void OnTarget( Mobile m, object o )
			{
				IPoint3D point = (IPoint3D)o;

				switch( c_Type )
				{
					case TargetType.BanLoc:
						c_Sign.BanLoc = new Point3D( point.X, point.Y, point.Z );
						c_Gump.NewGump();
						break;

					case TargetType.SignLoc:
						c_Sign.SignLoc = new Point3D( point.X, point.Y, point.Z );
                        c_Sign.MoveToWorld(c_Sign.SignLoc, c_Sign.Map);
                        c_Sign.Z -= 5;
						c_Sign.ShowSignPreview();
						c_Gump.NewGump();
						break;

					case TargetType.MinZ:
						c_Sign.MinZ = point.Z;

						if ( c_Sign.MaxZ < c_Sign.MinZ+19 )
							c_Sign.MaxZ = point.Z+19;

						if ( c_Sign.MaxZ == short.MaxValue )
							c_Sign.MaxZ = point.Z+19;

						c_Gump.NewGump();
						break;

					case TargetType.MaxZ:
						c_Sign.MaxZ = point.Z+19;

						if ( c_Sign.MinZ > c_Sign.MaxZ )
							c_Sign.MinZ = point.Z;

						c_Gump.NewGump();
						break;

					case TargetType.BlockOne:
                        m.SendMessage("Maintenant cibler le coin sud-est.");
						m.Target = new InternalTarget( c_Gump, c_Sign, TargetType.BlockTwo, new Point3D( point.X, point.Y, point.Z ) );
						break;

					case TargetType.BlockTwo:
						c_Sign.Blocks.Add( FixRect( new Rectangle2D( c_BoundOne, new Point3D( point.X+1, point.Y+1, point.Z ) ) ) );
                        c_Sign.UpdateBlocks();
                        c_Sign.ShowAreaPreview(m);
						c_Gump.NewGump();
						break;
				}
			}

			protected override void OnTargetCancel( Mobile m, TargetCancelType cancelType )
			{
				c_Gump.NewGump();
			}
		}
	}
}