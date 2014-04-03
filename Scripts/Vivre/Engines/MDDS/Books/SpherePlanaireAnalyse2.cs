using System;
using Server;

namespace Server.Items
{
	public class SpherePlanaireAnalyse2 : RedBook
	{
		public static readonly BookContent Content = new BookContent
			(
				"Sph�re planaire t.2", "Moonglow",
				
				new BookPageInfo
				(
" Nos premi�res expeditions",
"nous ont permis de reunir",
"les informations suivantes:",
"",
"Poser la main sur ell",
"nous absorbe a l'interieur",
"et nous projette dans un ",
"monde souterrain. D'apr�s"

				),
				new BookPageInfo
				(
"nos outils,il s'agit d'un",
"demi-plan interm�diaire",
"d'origine artificielle. Qui",
"qu'en soit le cr�ateur,il",
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
"n'apparaissent qu'apr�s",
"resolution d'une epreuve."
				),
				new BookPageInfo
				(
"",
" Ces �preuves sont la",
"majorit� du temps",
"sommaires : des",
"confrontations avec des ",
"cr�atures diverses,ne",
"vivant pas ici.",
"Elles semblent invoqu�es,"
				),
				new BookPageInfo
				(
"ou bien t�l�port�es.",
"Nous pourrions r�sumer",
"l'endroit � des ar�nes.",
 "",
" A noter toutefois ",
"qu'exceptionnellement,nous",
"f�mes confront�s � des",
"�preuves differentes,tels"
				),
				new BookPageInfo
				(
"des �nigmes ou des s�ries",
"de pi�ges.",
"",
" Pendant nos explorations,",
"nous avons souvent eu le ",
"sentiment d'�tre observ�s,",
"et � de br�ves occasions ",
"avons m�me entendus des" 
				),
				new BookPageInfo
				(
"voix �tranges. Peut-�tre",
"les fant�mes de ceux ",
"n'ayant eu la chance d'en",
" sortir vivants.",
"",
" Afin de faciliter notre",
"�tude, nous avons impos�",
" � la sph�re planaire de"
				),
				new BookPageInfo
				(
"nouvelles r�gles,esp�rant",
"que cela ne provoque pas",
"d'effets n�gatifs. La",
"principale est un",
"sortil�ge pour nous",
"reconduire � l'ext�rieur",
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
"r�guli�rement les recoins",
"et les secrets,afin ",
"que nous puissions r�unir"

				),
				new BookPageInfo
				(
" de nouvelles hypoth�ses",
" et rep�rer les plus gros",
"dangers auxquels nos",
"exp�ditions pourraient",
" �tre confront�s.",
"",
" De m�me,n'hesitons pas",
"� envoyer ces"

				),
				
				new BookPageInfo
				(
"mercenaires � la recherche",
"d'informations � son sujet.",
"Je suis certain que la",
"Sphere Planaire se trouve",
"sur Tenelia depuis",
"longtemps, et qu'il existe",
"des methodes connues pour",
"en exploiter la pleine"
				),
				new BookPageInfo
				(

"puissance.Reste � les",
"d�couvrir."
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