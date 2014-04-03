using System;
using Server;

namespace Server.Items
{
	public class SpherePlanaireAnalyse1 : RedBook
	{
		public static readonly BookContent Content = new BookContent
			(
				"Sphère planaire t.1", "Moonglow",
				
				new BookPageInfo
				(
					" C'est sous les ruines",
					"de Magincia que fut",
					"recemment découvert",
					"par des pillards de" ,
					"Buccanneer la Sphere",
					"planaire. Ils découvrirent",
					"à leurs dépends ses",
					"pouvoirs,emportés dans"
				),
				new BookPageInfo
				(
					"un monde inconnu dont",
					"peu revinrent vivants.",
					"",
					"L'objet nous fut vendu" ,
					"pour une importante" ,
					"somme d'argent après" ,
					"une vente aux enchères"
				),
				new BookPageInfo
				(
					"dans les souterrains de",
					"l'ile des pirates. Notre" ,
					"unique rival pour son",
					"acquisition n'a pas été",
					"identifié,mais ce n'est pas",
					"la première fois que des",
					"mages indépendants tentent",
					"de s'approprier des objets"
				),
				new BookPageInfo
				(
					"magiques,mais c'est" ,
					"exceptionnel qu'un",
					"concurrent puisse mettre",
					"autant d'or dans la balance.",
					"",
					" Nous avons dû recourir à",
					"d'autres moyens de pressions",
					"pour encourager nos amis" 
				),
				new BookPageInfo
				(
					"pillards à nous la vendre.",
					"Les pouvoirs magiques de",
					"la Sphère planaire ont été",
					"largement affaiblis par nos",
					"rituels,afin que nous",
					"puissions en contrôler les",
					"effets.",
					" Malencontreusement,cela a" 
				),
				new BookPageInfo
				(
					"déclenché quelque", 
					"mécanisme de protection et", 
					" bloqué une partie de ses", 
					"effets qui, pour le moment,",
					"demeurent hors d'études.",
					"",
					" Autre problématique : notre",
					"impossibilité à rapporter la"
				),
				new BookPageInfo
				(
					"sphère planaire à Moonglow.",
					"Alors que nous quittons", 
					" les ruines de Magincia,une",
					"tempête magnétique se leva",
					" aussitôt, nous obligeant",
					"à quitter notre  cap pour",
					"ne pas finir par le fond.",
					"Nous avons dû nous arrêter à"
				),
				new BookPageInfo
				(
					"Occlo,de la défunte famille de",
					"Roald,et tant qu'une solution ",
					"n'aura pas été trouvée la Sphère",
					"devra y demeurer et nos études",
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