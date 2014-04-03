using System;
using Server;

namespace Knives.TownHouses
{
	public class ContractConfirmGump : GumpPlusLight
	{
		private RentalContract c_Contract;

		public ContractConfirmGump( Mobile m, RentalContract rc ) : base( m, 100, 100 )
		{
			m.CloseGump( typeof( ContractConfirmGump ) );

			c_Contract = rc;
		}

		protected override void BuildGump()
		{
            int width = 300;
            int y = 0;

			if ( c_Contract.RentalClient == null )
				AddHtml( 0, y+5, width, HTML.Black + "<CENTER>Louer cette maison?");
			else
				AddHtml( 0, y+5, width, HTML.Black + "<CENTER>Contrat de location");

			string text = String.Format( "  Moi, {0}, accepte de louer ce bien appartenant a {1} pour la somme de {2} tous les {3}. "+
                                            "Les versements pour ce paiement seront effectués directement à partir de ma banque. Dans le cas où" +
                                            "Je ne peux pas payer cette taxe, la propriété revient à {1}. Je peux annuler cet accord à tout moment par" +
                                            "démolir la propriété. {1} peut également résilier ce contrat à tout moment soit en démolissant son" +
                                            "biens ou en annulant le contrat, auquel cas votre dépôt de garantie vous sera retourné.",
				c_Contract.RentalClient == null ? "_____" : c_Contract.RentalClient.Name,
				c_Contract.RentalMaster.Name,
				c_Contract.Free ? 0 : c_Contract.Price,
				c_Contract.PriceTypeShort.ToLower() );

            text += "<BR>Voici quelques infos au sujet de cette propriété:<BR>";

			text += String.Format( "<CENTER>Lockdowns: {0}<BR>", c_Contract.Locks );
			text += String.Format( "Secures: {0}<BR>", c_Contract.Secures );
			text += String.Format( "Floors: {0}<BR>", (c_Contract.MaxZ-c_Contract.MinZ < 200) ? (c_Contract.MaxZ-c_Contract.MinZ)/20+1 : 1 );
			text += String.Format( "Space: {0} cubic units", c_Contract.CalcVolume() );

			AddHtml( 40, y+=30, width-60, 200, HTML.Black + text, false, true );

            y += 200;

			if ( c_Contract.RentalClient == null )
			{
				AddHtml( 60, y+=20, 60, HTML.Black + "Preview");
				AddButton( 40, y+3, 0x837, 0x838, "Preview", new GumpCallback( Preview ) );

				bool locsec = c_Contract.ValidateLocSec();

				if ( Owner != c_Contract.RentalMaster && locsec )
				{
					AddHtml( width-100, y, 60, HTML.Black + "Accepter");
					AddButton( width-60, y+3, 0x232C, 0x232D, "Accepter", new GumpCallback( Accept ) );
				}
				else
					AddImage( width-60, y-10, 0x232C );

				if ( !locsec )
                    Owner.SendMessage((Owner == c_Contract.RentalMaster ? "Vous n'avez pas de Lockdown ou Secures disponible pour ce contrat." : "Le propriétaire de ce contrat ne peut actuellement pas louer cette propriété."));
			}
			else
			{
				if ( Owner == c_Contract.RentalMaster )
				{
					AddHtml( 60, y+=20, 100, HTML.Black + "Annuler Contrat");
					AddButton( 40, y+3, 0x837, 0x838, "Annuler Contrat", new GumpCallback( CancelContract ) );
				}
                else
				    AddImage( width-60, y+=20, 0x232C );
			}

            AddBackgroundZero( 0, 0, width, y+23, 0x24A4 );
        }

		protected override void OnClose()
		{
			c_Contract.ClearPreview();
		}

		private void Preview()
		{
            c_Contract.ShowAreaPreview(Owner);
			NewGump();
		}

		private void CancelContract()
		{
			if ( Owner == c_Contract.RentalClient )
				c_Contract.House.Delete();
			else
				c_Contract.Delete();
		}

		private void Accept()
		{
			if ( !c_Contract.ValidateLocSec() )
			{
                Owner.SendMessage("Le propriétaire de ce contrat ne peut actuellement pas louer cette propriété.");
				return;
			}

			c_Contract.Purchase( Owner );

			if ( !c_Contract.Owned )
				return;

			c_Contract.Visible = true;
			c_Contract.RentalClient = Owner;
			c_Contract.RentalClient.AddToBackpack( new RentalContractCopy( c_Contract ) );
		}
	}
}