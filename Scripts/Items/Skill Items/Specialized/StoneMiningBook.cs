//Myron - Traduction
using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public class StoneMiningBook : Item
	{
		public override string DefaultName
		{
			get { return "Faire carrière dans la pierre"; }
		}

		[Constructable]
		public StoneMiningBook() : base( 0xFBE )
		{
			Weight = 1.0;
		}

		public StoneMiningBook( Serial serial ) : base( serial )
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
			else */if ( pm == null || from.Skills[SkillName.Mining].Base < 100.0 )
			{
				from.SendMessage( "Seul un grand maitre mineur comprendrait cet ouvrage." );
			}
			else if ( pm.StoneMining )
			{
				pm.SendMessage( "Vous n'avez plus rien à apprendre de cet ouvrage." );
			}
			else
			{
				pm.StoneMining = true;
				pm.SendMessage( "Vous avez appris à récolter de bonnes pierres." );
				//Delete();
			}
		}
	}
}