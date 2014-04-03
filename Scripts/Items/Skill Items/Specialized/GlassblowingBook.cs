//Myron - Traduction.
using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public class GlassblowingBook : Item
	{
		public override string DefaultName
		{
			get { return "Guide illustré du souffleur de verre"; }
		}

		[Constructable]
		public GlassblowingBook() : base( 0xFF4 )
		{
			Weight = 1.0;
		}

		public GlassblowingBook( Serial serial ) : base( serial )
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
			else */
			if ( pm == null || from.Skills[SkillName.Alchemy].Base < 100.0 )
			{
				pm.SendMessage( "Seul un grand maitre alchimistre comprendrait cet ouvrage." );
			}
			else if ( pm.Glassblowing )
			{
				pm.SendMessage( "Vous connaissez déjà le contenu de l'ouvrage." );
			}
			else
			{
				pm.Glassblowing = true;
				pm.SendMessage( "Vous pouvez désormais souffler le verre avec du sable fin." );
				//Delete();
			}
		}
	}
}