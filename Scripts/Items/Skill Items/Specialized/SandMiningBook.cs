//Myron - Traduction.
using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public class SandMiningBook : Item
	{
		public override string DefaultName
		{
			get { return "Les plus belles plages de sable fin"; }
		}

		[Constructable]
		public SandMiningBook() : base( 0xFF4 )
		{
			Weight = 1.0;
		}

		public SandMiningBook( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			PlayerMobile pm = from as PlayerMobile;
			//Myron - Suppression de condition pour permettre de les mettre dans une biblio.
			/*if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else*/
			if ( pm == null || from.Skills[SkillName.Mining].Base < 100.0 )
			{
				pm.SendMessage( "Seul un grand maitre mineur comprendrait cet ouvrage." );
			}
			else if ( pm.SandMining )
			{
				pm.SendMessage( "Vous avez déjà lu cet ouvrage." );
			}
			else
			{
				pm.SandMining = true;
				pm.SendMessage( "Vous avez appris à récolter du sable fin sur les plages." );
				//Delete();
			}
		}
	}
}