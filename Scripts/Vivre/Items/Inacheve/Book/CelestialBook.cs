using System;
using Server;

namespace Server.Items
{
public class CelestialBook : BaseBook
	{
		public static readonly BookContent Content = new BookContent
			(
				"Notes d'une vie", "Celestial",
				new BookPageInfo
				(
					"This volume, and",
					"others in the series,",
					"are sponsored by",
					"donations from Lord",
					"Blackthorn, ever a",
					"supporter of",
					"understanding the",
					"other sentient races"
				),
				new BookPageInfo
				(
					"of Britannia.",
					"-",
					"",
					"  The Orcish tongue",
					"may fall unpleasingly",
					"'pon the ear, yet it",
					"has within it a",
					"complex grammar oft"
				),
				new BookPageInfo
				(
					"misunderstood by",
					"those who merely",
					"hear the few broken",
					"words of English our",
					"orcish brothers",
					"manage without",
					"education.",
					"  These are the basic"
				),
				new BookPageInfo
				(
					"rules of orcish:",
					"  Orcish has five",
					"tenses: present, past,",
					"future imperfect,",
					"present interjectional,",
					"and prehensile.",
					"  Examples: gugroflu,",
					"gugrofloog, gugrobo,"
				),
				new BookPageInfo
				(
					"gugroglu!, gugrogug.",
					"  All transitive verbs",
					"in the prehensile",
					"tense end in \"ug.\"",
					"  Examples:",
					"urgleighug,",
					"biggugdaghgug,",
					"curdakalmug."
				),
				new BookPageInfo
				(
					"  All present",
					"interjectional",
					"conjugations start",
					"with the letter G",
					"unless the contain the",
					"third declensive",
					"accent of the letter U.",
					"  Examples:"
				),
				new BookPageInfo
				(
					"ghothudunglug, but not",
					"azhbuugub.",
					"  The past tense can",
					"only refer to events",
					"since the last meal,",
					"but the prehensile",
					"tense can refer to",
					"any event within"
				),
				new BookPageInfo
				(
					"reach.",
					"  The present tense",
					"is conjugated like the",
					"future imperfect",
					"tense, when the",
					"interrogative mode is",
					"used by pitching the",
					"sound a quarter-tone"
				),
				new BookPageInfo
				(
					"higher.",
					"Orcish hath no",
					"concept of person, as",
					"in first person, third",
					"person, I, we, etc.",
					"  Orcish grammar",
					"relies upon the three",
					"cardinal rules of"
				),
				new BookPageInfo
				(
					"accretion, prefixing,",
					"and agglutination, in",
					"addition to pitch. In",
					"the former, phonemes",
					"combine into larger",
					"words which may",
					"contain full phrasal",
					"significance. In the"
				),
				new BookPageInfo
				(
					"second, prefixing",
					"specific phonetic",
					"sounds changes the",
					"subject of the",
					"sentence into object,",
					"interrogative,",
					"addressed individual,",
					"or dinner."
				),
				new BookPageInfo
				(
					"  Agglutination occurs",
					"whenever four of the",
					"same letter are",
					"present in a word, in",
					"which case, any two",
					"of them may be",
					"removed or slurred.",
					"  Pitch changes the"
				),
				new BookPageInfo
				(
					"phoneme value of",
					"individual syllables,",
					"thus completely",
					"altering what a word",
					"may mean. The",
					"classic example is",
					"\"Aktgluthugrot",
					"bigglogubuu"
				),
				new BookPageInfo
				(
					"dargilgaglug lublublub\"",
					"which can mean \"You",
					"are such a pretty",
					"girl,\" \"My mother ate",
					"your primroses,\" or",
					"\"Jellyfish nose paints",
					"alms potato,\"",
					"depending on pitch."
				),
				new BookPageInfo
				(
					"  Orcish poetry often",
					"relies upon repeating",
					"the same phrase in",
					"multiple pitches, even",
					"changing pitch",
					"midword. None of",
					"this great art is",
					"translatable."
				),
				new BookPageInfo
				(
					"  The orcish language",
					"uses the following",
					"vowels: ab, ad, ag, akt,",
					"at, augh, auh, azh, e,",
					"i, o, oo, u, uu. The",
					"vowel sound a is not",
					"recognized as a vowel",
					"and does not exist in"
				),
				new BookPageInfo
				(
					"their alphabet.",
					"The orcish alphabet is",
					"best learned using the",
					"classic rhyme",
					"repeated at 23",
					"different pitches:",
					"   Lugnog ghu blat",
					"suggaroglug,"
				),
				new BookPageInfo
				(
					"Gaghbuu dakdar ab",
					"highugbo,",
					"  Gothnogbuim ad",
					"gilgubbugbuilug",
					"Bilgeaugh thurggulg",
					"stuiggro!",
					"",
					"A translation of the"
				),
				new BookPageInfo
				(
					"first pitch:",
					"Eat food, the first",
					"letter is ab,",
					"Kill people, next letter",
					"is ad,",
					"I forget the rest",
					"But augh is in there",
					"somewhere!"
				),
				new BookPageInfo
				(
					"",
					"  What follows is a",
					"complete phonetic",
					"library of the orcish",
					"language:",
					"ab, ad, ag, akt, alm,",
					"at, augh, auh, azh,",
					"ba, ba, bag, bar, baz,"
				),
				new BookPageInfo
				(
					"bid, bilge, bo, bog, bog,",
					"brui, bu, buad, bug,",
					"bug, buil, buim, bum,",
					"buo, buor, buu, ca,",
					"car, clog, cro, cuk,",
					"cur, da, dagh, dagh,",
					"dak, dar, deak, der,",
					"dil, dit, dor, dre, dri,"
				),
				new BookPageInfo
				(
					"dru, du, dud, duf,",
					"dug, dug, duh, dun,",
					"eag, eg, egg, eichel,",
					"ek, ep, ewk, faugh,",
					"fid, flu, fog, foo,",
					"foz, fruk, fu, fub,",
					"fud, fun, fup, fur,",
					"gaa, gag, gagh, gan,"
				),
				new BookPageInfo
				(
					"gar, gh, gha, ghat,",
					"ghed, ghig, gho, ghu,",
					"gig, gil, gka, glu, glu,",
					"glug, gna, gno, gnu,",
					"gol, gom, goth, grunt,",
					"grut, gu, gub, gub,",
					"gug, gug, gugh, guk,",
					"guk,"
				)
			);

		public override BookContent DefaultContent{ get{ return Content; } }

		[Constructable]
        public CelestialBook()
            : base(0x2252, false)
		{
            Hue = 1154;
		}

        public CelestialBook(Serial serial)
            : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}
