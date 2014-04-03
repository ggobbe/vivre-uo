using System;
using System.Collections;
using Server;
using Server.Targeting;

namespace Knives.TownHouses
{
	public class ContractSetupGump : GumpPlusLight
	{
		public enum Page { Blocks, Floors, Sign, LocSec, Length, Price }
		public enum TargetType { SignLoc, MinZ, MaxZ, BlockOne, BlockTwo }

		private RentalContract c_Contract;
		private Page c_Page;

		public ContractSetupGump( Mobile m, RentalContract contract ) : base( m, 50, 50 )
		{
			m.CloseGump( typeof( ContractSetupGump ) );

			c_Contract = contract;
		}

		protected override void BuildGump()
		{
            int width = 300;
            int y = 0;

			switch( c_Page )
			{
				case Page.Blocks: BlocksPage(width, ref y); break;
                case Page.Floors: FloorsPage(width, ref y); break;
                case Page.Sign: SignPage(width, ref y); break;
                case Page.LocSec: LocSecPage(width, ref y); break;
                case Page.Length: LengthPage(width, ref y); break;
                case Page.Price: PricePage(width, ref y); break;
			}
        
            AddBackgroundZero(0, 0, width, y+40, 0x13BE);
        }

		private void BlocksPage(int width, ref int y)
		{
			if ( c_Contract == null )
				return;

			c_Contract.ShowAreaPreview( Owner );

			AddHtml( 0, y+=10, width, "<CENTER>Creer la zone");
            AddImage(width / 2 - 100, y + 2, 0x39);
            AddImage(width / 2 + 70, y + 2, 0x3B);

			y+=25;

			if ( !General.HasOtherContract( c_Contract.ParentHouse, c_Contract ) )
			{
				AddHtml( 60, y, 90, "Maison entiere");
                AddButton(30, y, c_Contract.EntireHouse ? 0xD3 : 0xD2, "Maison entiere", new GumpCallback(EntireHouse));
			}

			if ( !c_Contract.EntireHouse )
			{
				AddHtml( 170, y, 70, "Ajouter zone");
                AddButton(240, y, 0x15E1, 0x15E5, "Ajouter zone", new GumpCallback(AddBlock));

				AddHtml( 170, y+=20, 70, "Clear All");
				AddButton( 240, y, 0x15E1, 0x15E5, "Clear All", new GumpCallback( ClearBlocks ) );
			}

            string helptext = String.Format("Bienvenue sur le menu de configuration contrat de location! Pour commencer, vous devez" +
            " d'abord créer la zone que vous souhaitez vendre. Comme on le voit ci-dessus, il ya deux façons de le faire:" +
            "louer toute la maison, ou de parties de celle-ci. Lorsque vous créez la zone, un aperçu simple va vous montrer exactement" +
            "quelle zone vous avez sélectionné . Vous pouvez faire toutes sortes de formes bizarres en utilisant de multiples zones!");

			AddHtml( 10, y+=35, width-20, 170, helptext, false, false );

            y += 170;

			if ( c_Contract.EntireHouse || c_Contract.Blocks.Count != 0 )
			{
				AddHtml( width-60, y+=20, 60, "Next");
				AddButton( width-30, y, 0x15E1, 0x15E5, "Next", new GumpStateCallback( ChangePage ), (int)c_Page  + ( c_Contract.EntireHouse ? 4 : 1 ) );
			}
		}

		private void FloorsPage(int width, ref int y)
		{
			AddHtml( 0, y+=10, width, "<CENTER>Sols");
            AddImage(width / 2 - 100, y + 2, 0x39);
            AddImage(width / 2 + 70, y + 2, 0x3B);

			AddHtml( 40, y+=25, 80, "Etage de Base");
            AddButton(110, y, 0x15E1, 0x15E5, "Etage de Base", new GumpCallback(MinZSelect));

			AddHtml( 160, y, 80, "Top Floor");
			AddButton( 230, y, 0x15E1, 0x15E5, "Etage superieur", new GumpCallback( MaxZSelect ) );

			AddHtml( 100, y+=25, 100, String.Format( "{0} total etages{1}", c_Contract.Floors > 10 ? "1" : "" + c_Contract.Floors, c_Contract.Floors == 1 || c_Contract.Floors > 10 ? "" : "s" ));

			string helptext = String.Format( "Maintenantque vous aurez besoin de cibler les étagesque vous souhaitez louer" +
                "Si vous voulez seulement un étage, vous pouvez ne pas cible l'étage supérieur Toutes choses entre la base" +
                "et le dernier étage sera inclue dans la location, avec consequences sur le prix" );

			AddHtml( 10, y+=35, width-20, 120, helptext, false, false );

            y += 120;

			AddHtml( 30, y+=20, 80, "Previous");
			AddButton( 10, y, 0x15E3, 0x15E7, "Previous", new GumpStateCallback( ChangePage ), (int)c_Page-1 );

			if ( c_Contract.MinZ != short.MinValue )
			{
				AddHtml( width-60, y, 60, "Next");
				AddButton( width-30, y, 0x15E1, 0x15E5, "Next", new GumpStateCallback( ChangePage ), (int)c_Page+1 );
			}
		}

		private void SignPage(int width, ref int y)
		{
			if ( c_Contract == null )
				return;

			c_Contract.ShowSignPreview();

			AddHtml( 0, y+=10, width, "<CENTER>Their Sign Location");
            AddImage(width / 2 - 100, y + 2, 0x39);
            AddImage(width / 2 + 70, y + 2, 0x3B);

			AddHtml( 100, y+=25, 80, "Def. Emplacement");
			AddButton( 180, y, 0x15E1, 0x15E5, "Sign Loc", new GumpCallback( SignLocSelect ) );

			string helptext = String.Format( "Avec ce panneau, le locataire aura tous les pouvoirs d'un propriétaire" +
"sur toute sa surface. Si il exercent ce pouvoir de démolir le logement locatif, il rompu le" +
"contrat et ne recevra pas son dépôt de garantie. Il peut aussi vous banir de sa maison de location!" );

			AddHtml( 10, y+=35, width-20, 110, helptext, false, false );

            y += 110;

			AddHtml( 30, y+=20, 80, "Previous");
			AddButton( 10, y, 0x15E3, 0x15E7, "Previous", new GumpStateCallback( ChangePage ), (int)c_Page-1 );

			if ( c_Contract.SignLoc != Point3D.Zero )
			{
				AddHtml( width-60, y, 60, "Next");
				AddButton( width-30, y, 0x15E1, 0x15E5, "Next", new GumpStateCallback( ChangePage ), (int)c_Page+1 );
			}
		}

		private void LocSecPage(int width, ref int y)
		{
			AddHtml( 0, y+=10, width, "<CENTER>Lockdowns and Secures");
            AddImage(width / 2 - 100, y + 2, 0x39);
            AddImage(width / 2 + 70, y + 2, 0x3B);

            AddHtml(0, y += 25, width, "<CENTER>Suggest Secures");
            AddButton(width / 2 - 70, y + 3, 0x2716, "Suggest LocSec", new GumpCallback(SuggestLocSec));
            AddButton(width / 2 + 60, y + 3, 0x2716, "Suggest LocSec", new GumpCallback(SuggestLocSec));

            AddHtml(30, y += 25, width / 2 - 20, "<DIV ALIGN=RIGHT>Secures (Max: " + (General.RemainingSecures(c_Contract.ParentHouse) + c_Contract.Secures) + ")");
			AddTextField( width/2+50, y, 50, 20, 0x480, 0xBBC, "Secures", c_Contract.Secures.ToString() );
            AddButton(width / 2 + 25, y + 3, 0x2716, "Secures", new GumpCallback(Secures));

            AddHtml(30, y += 20, width / 2 - 20, "<DIV ALIGN=RIGHT>Lockdowns (Max: " + (General.RemainingLocks(c_Contract.ParentHouse) + c_Contract.Locks) + ")");
			AddTextField( width/2+50, y, 50, 20, 0x480, 0xBBC, "Lockdowns", c_Contract.Locks.ToString() );
            AddButton(width / 2 + 25, y + 3, 0x2716, "Lockdowns", new GumpCallback(Lockdowns));

			string helptext = String.Format( "Sans donner d'espace, ce ne serait pas vraiment une maison, ici, vous assignez les lockdowns" +
"et secures de votre propre maison. Utilisez le bouton suggérer pour une idée de combien vous devriez alouer. Soyez très prudent lorsque vous" +
"louez votre bien: si vous utilisez trop de stockage vous commencerez à utiliser du stockage que vous avez réservé a vos clients" +
"Vous recevrez un avertissement de 48 heures lorsque cela se produit, après quoi le contrat disparaîtra!" );

			AddHtml( 10, y+=35, width-20, 180, helptext, false, false );

            y += 180;

			AddHtml( 30, y+=20, 80, "Previous");
			AddButton( 10, y, 0x15E3, 0x15E7, "Previous", new GumpStateCallback( ChangePage ), (int)c_Page-1 );

			if ( c_Contract.Locks != 0 && c_Contract.Secures != 0 )
			{
				AddHtml( width-60, y, 60, "Next");
				AddButton( width-30, y, 0x15E1, 0x15E5, "Next", new GumpStateCallback( ChangePage ), (int)c_Page+1 );
			}
		}

		private void LengthPage(int width, ref int y)
		{
            AddHtml(0, y += 10, width, "<CENTER>Périodes");
            AddImage(width / 2 - 100, y + 2, 0x39);
            AddImage(width / 2 + 70, y + 2, 0x3B);

			AddHtml( 120, y+=25, 50, c_Contract.PriceType);
			AddButton( 170, y+8, 0x985, "LengthUp", new GumpCallback( LengthUp ) );
			AddButton( 170, y-2, 0x983, "LengthDown", new GumpCallback( LengthDown ) );

            string helptext = String.Format("Tous les {0}, la banque vous transfère automatiquement le coût de location" +
"En utilisant les flèches, vous pouvez definir d'autres périodes, mieux adaptées à vos besoins", c_Contract.PriceTypeShort.ToLower());

			AddHtml( 10, y+=35, width-20, 100, helptext, false, false );

            y += 100;

			AddHtml( 30, y+=20, 80, "Previous");
			AddButton( 10, y, 0x15E3, 0x15E7, "Previous", new GumpStateCallback( ChangePage ), (int)c_Page - ( c_Contract.EntireHouse ? 4 : 1 ) );

			AddHtml( width-60, y, 60, "Next");
			AddButton( width-30, y, 0x15E1, 0x15E5, "Next", new GumpStateCallback( ChangePage ), (int)c_Page+1 );
		}

		private void PricePage(int width, ref int y)
		{
            AddHtml(0, y += 10, width, "<CENTER>Loyer par périodes");
            AddImage(width / 2 - 100, y + 2, 0x39);
            AddImage(width / 2 + 70, y + 2, 0x3B);

            AddHtml(0, y += 25, width, "<CENTER>Gratuit");
            AddButton(width / 2 - 80, y, c_Contract.Free ? 0xD3 : 0xD2, "Gratuit", new GumpCallback(Free));
            AddButton(width / 2 + 60, y, c_Contract.Free ? 0xD3 : 0xD2, "Gratuit", new GumpCallback(Free));

			if ( !c_Contract.Free )
			{
				AddHtml( 0, y+=25, width/2-20, "<DIV ALIGN=RIGHT>Par " + c_Contract.PriceTypeShort);
				AddTextField( width/2+20, y, 70, 20, 0x480, 0xBBC, "Prix", c_Contract.Price.ToString() );
                AddButton(width / 2 - 5, y + 3, 0x2716, "Prix", new GumpCallback(Price));

                AddHtml(0, y += 20, width, "<CENTER>Suggérer");
                AddButton(width / 2 - 70, y + 3, 0x2716, "Suggérer", new GumpCallback(SuggestPrice));
                AddButton(width / 2 + 60, y + 3, 0x2716, "Suggérer", new GumpCallback(SuggestPrice));
            }

            string helptext = String.Format("  Maintenant, vous pouvez finaliser le contrat en incluant votre prix par {0}" +
"Une fois que vous avez finalisé, la seule façon vous pouvez le modifier est de l'annuler et de commencer un nouveau contrat! En" +
"utilisant le bouton suggérer, un prix sera automatiquement choisi sur les bases suivantes: <BR>", c_Contract.PriceTypeShort);

			helptext += String.Format( "<CENTER>Volume: {0}<BR>", c_Contract.CalcVolume() );
			helptext += String.Format( "Cout par unite: {0} Or</CENTER>", General.SuggestionFactor );
            helptext += "<br>Avec l'option ci-dessous, vous pouvez offrir cet espace entre trop.";

			AddHtml( 10, y+=35, width-20, 150, helptext, false, true );

            y += 150;

			AddHtml( 30, y+=20, 80, "Previous");
			AddButton( 10, y, 0x15E3, 0x15E7, "Previous", new GumpStateCallback( ChangePage ), (int)c_Page-1 );

			if ( c_Contract.Price != 0 )
			{
				AddHtml( width-70, y, 60, "Finaliser");
                AddButton(width - 30, y, 0x15E1, 0x15E5, "Finaliser", new GumpCallback(FinalizeSetup));
			}
		}

		protected override void OnClose()
		{
			c_Contract.ClearPreview();
		}

		private void SuggestPrice()
		{
			if ( c_Contract == null )
				return;

			c_Contract.Price = c_Contract.CalcVolume()*General.SuggestionFactor;

			if ( c_Contract.RentByTime == TimeSpan.FromDays( 1 ) )
				c_Contract.Price /= 60;
			if ( c_Contract.RentByTime == TimeSpan.FromDays( 7 ) )
				c_Contract.Price = (int)((double)c_Contract.Price/8.57);
			if ( c_Contract.RentByTime == TimeSpan.FromDays( 30 ) )
				c_Contract.Price /= 2;

			NewGump();
		}

		private void SuggestLocSec()
		{
			int price = c_Contract.CalcVolume()*General.SuggestionFactor;
			c_Contract.Secures = price/75;
			c_Contract.Locks = c_Contract.Secures/2;

			c_Contract.FixLocSec();

			NewGump();
		}

        private void Price()
        {
            c_Contract.Price = GetTextFieldInt("Prix");
            Owner.SendMessage("Prix choisi!");
            NewGump();
        }

        private void Secures()
        {
            c_Contract.Secures = GetTextFieldInt("Secures");
            Owner.SendMessage("Secures choisi!");
            NewGump();
        }

        private void Lockdowns()
        {
            c_Contract.Locks = GetTextFieldInt("Lockdowns");
            Owner.SendMessage("Lockdowns choisi!");
            NewGump();
        }

        private void ChangePage(object obj)
		{
			if ( c_Contract == null || !(obj is int) )
				return;

			c_Contract.ClearPreview();

			c_Page = (Page)(int)obj;

			NewGump();
		}

		private void EntireHouse()
		{
			if ( c_Contract == null || c_Contract.ParentHouse == null )
				return;

			c_Contract.EntireHouse = !c_Contract.EntireHouse;

			c_Contract.ClearPreview();

			if ( c_Contract.EntireHouse )
			{
                ArrayList list = new ArrayList();

                bool once = false;
                foreach (Rectangle3D rect in RUOVersion.RegionArea(c_Contract.ParentHouse.Region))
                {
                    list.Add(new Rectangle2D(new Point2D(rect.Start.X, rect.Start.Y), new Point2D(rect.End.X, rect.End.Y)));

                    if (once)
                        continue;

                    if (rect.Start.Z >= rect.End.Z)
                    {
                        c_Contract.MinZ = rect.End.Z;
                        c_Contract.MaxZ = rect.Start.Z;
                    }
                    else
                    {
                        c_Contract.MinZ = rect.Start.Z;
                        c_Contract.MaxZ = rect.End.Z;
                    }

                    once = true;
                }

				c_Contract.Blocks = list;
			}
			else
			{
				c_Contract.Blocks.Clear();
				c_Contract.MinZ = short.MinValue;
				c_Contract.MaxZ = short.MinValue;
			}

			NewGump();
		}

		private void SignLocSelect()
		{
			Owner.Target = new InternalTarget( this, c_Contract, TargetType.SignLoc );
		}

		private void MinZSelect()
		{
            Owner.SendMessage("Cibler l'étage de base de votre région de location.");
			Owner.Target = new InternalTarget( this, c_Contract, TargetType.MinZ );
		}


		private void MaxZSelect()
		{
            Owner.SendMessage("Cibler l'étage supérieur de votre région de location.");
			Owner.Target = new InternalTarget( this, c_Contract, TargetType.MaxZ );
		}

		private void LengthUp()
		{
			if ( c_Contract == null )
				return;

			c_Contract.NextPriceType();

			if ( c_Contract.RentByTime == TimeSpan.FromDays( 0 ) )
				c_Contract.RentByTime = TimeSpan.FromDays( 1 );

			NewGump();
		}

		private void LengthDown()
		{
			if ( c_Contract == null )
				return;

			c_Contract.PrevPriceType();

			if ( c_Contract.RentByTime == TimeSpan.FromDays( 0 ) )
				c_Contract.RentByTime = TimeSpan.FromDays( 30 );

			NewGump();
		}

		private void Free()
		{
			c_Contract.Free = !c_Contract.Free;

			NewGump();
		}

		private void AddBlock()
		{
            Owner.SendMessage("Cibler le coin nord-ouest.");
			Owner.Target = new InternalTarget( this, c_Contract, TargetType.BlockOne );
		}

		private void ClearBlocks()
		{
			if ( c_Contract == null )
				return;

			c_Contract.Blocks.Clear();

			c_Contract.ClearPreview();

			NewGump();
		}

		private void FinalizeSetup()
		{
			if ( c_Contract == null )
				return;

			if ( c_Contract.Price == 0 )
			{
                Owner.SendMessage("Vous ne pouvez pas louer la zone pour 0 or!");
				NewGump();
				return;
			}

			c_Contract.Completed = true;
			c_Contract.BanLoc = c_Contract.ParentHouse.Region.GoLocation;

			if ( c_Contract.EntireHouse )
			{
				Point3D point = c_Contract.ParentHouse.Sign.Location;
				c_Contract.SignLoc = new Point3D( point.X, point.Y, point.Z-5 );
				c_Contract.Secures = Core.AOS ? c_Contract.ParentHouse.GetAosMaxSecures() : c_Contract.ParentHouse.MaxSecures;
				c_Contract.Locks = Core.AOS ? c_Contract.ParentHouse.GetAosMaxLockdowns() : c_Contract.ParentHouse.MaxLockDowns;
			}

            Owner.SendMessage("Vous avez finalisé ce contrat de location. Maintenant trouvez quelqu'un pour le signer!");
		}

		private class InternalTarget : Target
		{
			private ContractSetupGump c_Gump;
			private RentalContract c_Contract;
			private TargetType c_Type;
			private Point3D c_BoundOne;

			public InternalTarget( ContractSetupGump gump, RentalContract contract, TargetType type ) : this( gump, contract, type, Point3D.Zero ){}

			public InternalTarget( ContractSetupGump gump, RentalContract contract, TargetType type, Point3D point ) : base( 20, true, TargetFlags.None )
			{
				c_Gump = gump;
				c_Contract = contract;
				c_Type = type;
				c_BoundOne = point;
			}

			protected override void OnTarget( Mobile m, object o )
			{
				IPoint3D point = (IPoint3D)o;

				if ( c_Contract == null || c_Contract.ParentHouse == null )
					return;

				if ( !c_Contract.ParentHouse.Region.Contains( new Point3D( point.X, point.Y, point.Z ) ) )
				{
                    m.SendMessage("Vous devez cibler dans la maison");
					m.Target = new InternalTarget( c_Gump, c_Contract, c_Type, c_BoundOne );
					return;
				}

				switch( c_Type )
				{
					case TargetType.SignLoc:
						c_Contract.SignLoc = new Point3D( point.X, point.Y, point.Z );
						c_Contract.ShowSignPreview();
						c_Gump.NewGump();
						break;

					case TargetType.MinZ:
                        if (!c_Contract.ParentHouse.Region.Contains(new Point3D(point.X, point.Y, point.Z)))
							m.SendMessage( "Ce n'est pas dans votre maison" );
						else if ( c_Contract.HasContractedArea( point.Z ) )
                            m.SendMessage("Cette zone est déjà prise par un autre contrat de location");
						else
						{
							c_Contract.MinZ = point.Z;

							if ( c_Contract.MaxZ < c_Contract.MinZ+19 )
								c_Contract.MaxZ = point.Z+19;
						}

                        c_Contract.ShowFloorsPreview(m);
						c_Gump.NewGump();
						break;

					case TargetType.MaxZ:
						if ( !c_Contract.ParentHouse.Region.Contains(new Point3D(point.X, point.Y, point.Z)) )
                            m.SendMessage("Ce n'est pas dans votre maison");
						else if ( c_Contract.HasContractedArea( point.Z ) )
							m.SendMessage( "Cette zone est déjà prise par un autre contrat de location." );
						else
						{
							c_Contract.MaxZ = point.Z+19;

							if ( c_Contract.MinZ > c_Contract.MaxZ )
								c_Contract.MinZ = point.Z;
						}

                        c_Contract.ShowFloorsPreview(m);
                        c_Gump.NewGump();
						break;

					case TargetType.BlockOne:
                        m.SendMessage("Maintenant, ciblez le coin sud-est");
						m.Target = new InternalTarget( c_Gump, c_Contract, TargetType.BlockTwo, new Point3D( point.X, point.Y, point.Z ) );
						break;

					case TargetType.BlockTwo:
						Rectangle2D rect = TownHouseSetupGump.FixRect( new Rectangle2D( c_BoundOne, new Point3D( point.X+1, point.Y+1, point.Z ) ) );

						if ( c_Contract.HasContractedArea( rect, point.Z ) )
                            m.SendMessage("Cette zone est déjà prise par un autre contrat de location");
						else
						{
							c_Contract.Blocks.Add( rect );
							c_Contract.ShowAreaPreview( m );
						}

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