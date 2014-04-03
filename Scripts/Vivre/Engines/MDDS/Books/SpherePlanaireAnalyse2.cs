using System;
using Server;

namespace Server.Items
{
	public class SpherePlanaireAnalyse2 : RedBook
	{
		public static readonly BookContent Content = new BookContent
			(
				"Sphère planaire t.2", "Moonglow",
				
				new BookPageInfo
				(
" Nos premières expeditions",
"nous ont permis de reunir",
"les informations suivantes:",
"",
"Poser la main sur ell",
"nous absorbe a l'interieur",
"et nous projette dans un ",
"monde souterrain. D'après"

				),
				new BookPageInfo
				(
"nos outils,il s'agit d'un",
"demi-plan intermédiaire",
"d'origine artificielle. Qui",
"qu'en soit le créateur,il",
"maitrise ou maitrisait",
"l'archimagie.",
"",
" Ce monde souterrain est"
				),
				new BookPageInfo
				(
"fait de grottes,mais sans ",
"chemin physique entre",
"elles. La circulation se",
" fait exclusivement par",
" le biais de portails ",
"magiques,qui",
"n'apparaissent qu'après",
"resolution d'une epreuve."
				),
				new BookPageInfo
				(
"",
" Ces épreuves sont la",
"majorité du temps",
"sommaires : des",
"confrontations avec des ",
"créatures diverses,ne",
"vivant pas ici.",
"Elles semblent invoquées,"
				),
				new BookPageInfo
				(
"ou bien téléportées.",
"Nous pourrions résumer",
"l'endroit à des arènes.",
 "",
" A noter toutefois ",
"qu'exceptionnellement,nous",
"fûmes confrontés à des",
"épreuves differentes,tels"
				),
				new BookPageInfo
				(
"des énigmes ou des séries",
"de pièges.",
"",
" Pendant nos explorations,",
"nous avons souvent eu le ",
"sentiment d'être observés,",
"et à de brèves occasions ",
"avons même entendus des" 
				),
				new BookPageInfo
				(
"voix étranges. Peut-être",
"les fantômes de ceux ",
"n'ayant eu la chance d'en",
" sortir vivants.",
"",
" Afin de faciliter notre",
"étude, nous avons imposé",
" à la sphère planaire de"
				),
				new BookPageInfo
				(
"nouvelles règles,espérant",
"que cela ne provoque pas",
"d'effets négatifs. La",
"principale est un",
"sortilège pour nous",
"reconduire à l'extérieur",
"si nous devions subir un",
" coup fatal."
				),
				new BookPageInfo
				(
" Occlo comprends une",
"forte population de",
"jeunes aventuriers :",
"nous pourrions en engager",
"certains pour en explorer",
"régulièrement les recoins",
"et les secrets,afin ",
"que nous puissions réunir"

				),
				new BookPageInfo
				(
" de nouvelles hypothèses",
" et repérer les plus gros",
"dangers auxquels nos",
"expéditions pourraient",
" être confrontés.",
"",
" De même,n'hesitons pas",
"à envoyer ces"

				),
				
				new BookPageInfo
				(
"mercenaires à la recherche",
"d'informations à son sujet.",
"Je suis certain que la",
"Sphere Planaire se trouve",
"sur Tenelia depuis",
"longtemps, et qu'il existe",
"des methodes connues pour",
"en exploiter la pleine"
				),
				new BookPageInfo
				(

"puissance.Reste à les",
"découvrir."
				)

			);

		public override BookContent DefaultContent{ get{ return Content; } }

		[Constructable]
		public SpherePlanaireAnalyse2() : base( false )
		{
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			list.Add( "Sphere planaire T.2" );
		}

		public override void OnSingleClick( Mobile from )
		{
			LabelTo( from, "etude par le Conseil de Moonglow" );
		}

		public SpherePlanaireAnalyse2( Serial serial ) : base( serial )
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