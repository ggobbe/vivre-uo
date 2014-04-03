using System;
using Server;

namespace Server.Items
{
	public class SpherePlanaireAnalyse1 : RedBook
	{
		public static readonly BookContent Content = new BookContent
			(
				"Sph�re planaire t.1", "Moonglow",
				
				new BookPageInfo
				(
					" C'est sous les ruines",
					"de Magincia que fut",
					"recemment d�couvert",
					"par des pillards de" ,
					"Buccanneer la Sphere",
					"planaire. Ils d�couvrirent",
					"� leurs d�pends ses",
					"pouvoirs,emport�s dans"
				),
				new BookPageInfo
				(
					"un monde inconnu dont",
					"peu revinrent vivants.",
					"",
					"L'objet nous fut vendu" ,
					"pour une importante" ,
					"somme d'argent apr�s" ,
					"une vente aux ench�res"
				),
				new BookPageInfo
				(
					"dans les souterrains de",
					"l'ile des pirates. Notre" ,
					"unique rival pour son",
					"acquisition n'a pas �t�",
					"identifi�,mais ce n'est pas",
					"la premi�re fois que des",
					"mages ind�pendants tentent",
					"de s'approprier des objets"
				),
				new BookPageInfo
				(
					"magiques,mais c'est" ,
					"exceptionnel qu'un",
					"concurrent puisse mettre",
					"autant d'or dans la balance.",
					"",
					" Nous avons d� recourir �",
					"d'autres moyens de pressions",
					"pour encourager nos amis" 
				),
				new BookPageInfo
				(
					"pillards � nous la vendre.",
					"Les pouvoirs magiques de",
					"la Sph�re planaire ont �t�",
					"largement affaiblis par nos",
					"rituels,afin que nous",
					"puissions en contr�ler les",
					"effets.",
					" Malencontreusement,cela a" 
				),
				new BookPageInfo
				(
					"d�clench� quelque", 
					"m�canisme de protection et", 
					" bloqu� une partie de ses", 
					"effets qui, pour le moment,",
					"demeurent hors d'�tudes.",
					"",
					" Autre probl�matique : notre",
					"impossibilit� � rapporter la"
				),
				new BookPageInfo
				(
					"sph�re planaire � Moonglow.",
					"Alors que nous quittons", 
					" les ruines de Magincia,une",
					"temp�te magn�tique se leva",
					" aussit�t, nous obligeant",
					"� quitter notre  cap pour",
					"ne pas finir par le fond.",
					"Nous avons d� nous arr�ter �"
				),
				new BookPageInfo
				(
					"Occlo,de la d�funte famille de",
					"Roald,et tant qu'une solution ",
					"n'aura pas �t� trouv�e la Sph�re",
					"devra y demeurer et nos �tudes",
					"accomplies sur place. "
				)

			);

		public override BookContent DefaultContent{ get{ return Content; } }

		[Constructable]
		public SpherePlanaireAnalyse1() : base( false )
		{
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			list.Add( "Sphere planaire T.1" );
		}

		public override void OnSingleClick( Mobile from )
		{
			LabelTo( from, "etude par le Conseil de Moonglow" );
		}

		public SpherePlanaireAnalyse1( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}