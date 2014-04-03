//Myron - Traduction
using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public class MasonryBook : Item
	{
		public override string DefaultName
		{
			get { return "Sculpter la pierre : Un passe temps passionnant"; }
		}

		[Constructable]
		public MasonryBook() : base( 0xFBE )
		{
			Weight = 1.0;
		}

		public MasonryBook( Serial serial ) : base( serial )
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
			else */if ( pm == null || from.Skills[SkillName.Carpentry].Base < 100.0 )
			{
				pm.SendMessage( "Seul un grand maitre charpentier comprendrait cet ouvrage." );
			}
			else if ( pm.Masonry )
			{
				pm.SendMessage( "Ce livre est exceptionnellement bien illustré mais le lire une deuxième fois ne vous apportera rien de plus." );
			}
			else
			{
				pm.Masonry = true;
				pm.SendMessage( "Vous pouvez désormais sculpter la pierre." );
				//Delete();
			}
		}
	}
}