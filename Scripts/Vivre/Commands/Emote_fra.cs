//Emote v2 by CMonkey123
/*
v2 changes:
	-Shortened script
	-Added emotes (thanks to zire):
		bow, faint, punch, slap, stickouttongue, tapfoot
	-Added emote gump (thanks to zire)
*/
/* Emote v3 by GM Jubal from Ebonspire http://www.ebonspire.com
 * I Left the above comments in here for credit to properly go back to those whom originally wrote this
 * I simply made it so that the [e command would call the gump if used by itself or if the <sound> was
 * misspelled, shortened the code down from 1300+ lines down to only 635 lines including these comments.
 * Also fixed a couple of typos in the script.
 * This has been tested on both RunUO beta .36 and RunUO RC0 1.0
*/
/* Emote v4 by Lysdexic
 * Updated for RunUO 2.0 RC2
 * Puke command could be used for teleport bug... removed that ability.
 * Typos again... 
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Gumps;
using Server.Commands.Generic;

namespace Server.Commands
{
    public enum EmotePage
    {
        P1,
        P2,
        P3,
        P4,
    }
    public class Emote_fra
    {
        public static void Initialize()
        {
            CommandSystem.Register("emote", AccessLevel.Player, new CommandEventHandler(Emote_OnCommand));
            CommandSystem.Register("e", AccessLevel.Player, new CommandEventHandler(Emote_OnCommand));
        }

        [Usage("<sound>")]
        [Description("Emote avec des sons, des mots et parfois avec une animation en une commande!")]
        private static void Emote_OnCommand(CommandEventArgs e)
        {
            Mobile pm = e.Mobile;
            string em = e.ArgString.Trim();
            int SoundInt;
            switch (em)
            {
                case "dire_ah!":
                case "ah!":
                case "ah":
                    SoundInt = 1;
                    break;
                case "dire_ah!ah!":
                case "ah!ah!":
                case "ahah":
                    SoundInt = 2;
                    break;
                case "applaudir":  //applaud
                case "clapclap":
                case "clap!clap!":
                    SoundInt = 3;
                    break;
                case "se_moucher": //blownose
                case "sprot":
                case "sprot!":
                    SoundInt = 4;
                    break;
                case "saluer":  //bow
                case "salut":
                case "salut!":
                    SoundInt = 5;
                    break;
                case "s_etrangler":  //bscough  
                case "gloups":
                case "gloups!":
                    SoundInt = 6;
                    break;
                case "faire_un_rot": //burp
                case "rot":
                case "rot!":
                    SoundInt = 7;
                    break;
                case "eclaircir_sa_voix": // eclaicir la voix 
                case "humhum":
                case "humhum!":
                    SoundInt = 8;
                    break;
                case "tousser":  //cough
                case "cofcof":
                case "cof!cof!":
                    SoundInt = 9;
                    break;
                case "pleurer":  //cry : pleurer
                case "bouhou":
                case "bouhou!":
                    SoundInt = 10;
                    break;
                case "tomber":		// faint feindre mais cest mieux tomber	
                case "boum":
                case "boum!":
                    SoundInt = 11;
                    break;
                case "peter":  //fart
                case "prout":
                case "prout!":
                    SoundInt = 12;
                    break;
                case "haleter":  //gasp
                case "gasp":
                case "gasp!":
                    SoundInt = 13;
                    break;
                case "pouffer_de_rire": //giggle
                case "mouahaha":
                case "mouahaha!":
                    SoundInt = 14;
                    break;
                case "jouir":  //groan
                case "aaahm":
                case "aaahm!":
                    SoundInt = 15;
                    break;
                case "grogner":  //growl
                case "grrr":
                case "grrr!":
                    SoundInt = 16;
                    break;
                case "apostropher":  //hey
                case "eh":
                case "eh!":
                    SoundInt = 17;
                    break;
                case "avoir_le_hoquet": //hippuc
                case "hic":
                case "hic!":
                    SoundInt = 18;
                    break;
                case "s_etonner":  //huh
                case "hein":
                case "hein?":
                    SoundInt = 19;
                    break;
                case "embrasser":  //kiss
                case "smak":
                case "smak!":
                    SoundInt = 20;
                    break;
                case "rire": //laugh  : son a verifier
                case "lol":
                case "ha!ha!ha!":
                    SoundInt = 21;
                    break;
                case "nier": //no
                case "non":
                    SoundInt = 22;
                    break;
                case "ho!": //oh
                case "ho":
                case "dire_ho!":
                    SoundInt = 23;
                    break;
                case "houu!": //oooh
                case "hou":
                case "dire_hou!":
                    SoundInt = 24;
                    break;
                case "s_escuser":
                case "oups":
                case "oups!":
                    SoundInt = 25;
                    break;
                case "vomir":  //puke
                case "blurg!":
                    SoundInt = 26;
                    break;
                case "cogner":  //punch
                case "paf":
                case "paf!":
                    SoundInt = 27;
                    break;
                case "hurler":  //scream
                case "aaah":
                case "aaah!":
                    SoundInt = 28;
                    break;
                case "faire_taire": //shush
                case "chut":
                case "chut!":
                    SoundInt = 29;
                    break;
                case "soupirer":  //sigh
                case "pfff":
                case "pfff!":
                    SoundInt = 30;
                    break;
                case "gifler": //slap
                case "baf":
                case "baf!":
                    SoundInt = 31;
                    break;
                case "eternuer":  //sneeze
                case "atchoum":
                case "atchoum!":
                    SoundInt = 32;
                    break;
                case "renifler":
                case "snif":  //sniff
                case "snif!":
                    SoundInt = 33;
                    break;
                case "ronfler":  //snore
                case "zzz":
                case "zzz!":
                    SoundInt = 34;
                    break;
                case "cracher":  //spit
                case "steuh!":
                    SoundInt = 35;
                    break;
                case "tirer_la_langue": //stickouttongue
                case "meuh!":
                case "meuh":
                    SoundInt = 36;
                    break;
                case "s_impatienter": //tapfoot
                case "taptap":
                case "tap!tap!":
                    SoundInt = 37;
                    break;
                case "siffler":  //whistle
                case "fouit!":
                    SoundInt = 38;
                    break;
                case "feliciler": //woohoo
                case "youhou":
                case "youhou!":
                    SoundInt = 39;
                    break;
                case "bailler":  //yawn
                case "houam":
                case "houam!":
                    SoundInt = 40;
                    break;
                case "ouais!": //yea
                case "ouais":
                case "dire_ouais":
                    SoundInt = 41;
                    break;
                case "cri_de_guerre": // yells
                case "yaaah!":
                    SoundInt = 42;
                    break;
                default:
                    SoundInt = 0;
                    e.Mobile.SendGump(new EmoteGump(e.Mobile, EmotePage.P1));
                    break;
            }
            if (SoundInt > 0)
                new ESound(pm, SoundInt);
        }
    }
    public class EmoteGump : Gump
    {
        private Mobile m_From;
        private EmotePage m_Page;
        private const int Blanco = 0xFFFFFF;
        private const int Azul = 0x8080FF;
        public void AddPageButton(int x, int y, int buttonID, string text, EmotePage page, params EmotePage[] subpage)
        {
            bool seleccionado = (m_Page == page);
            for (int i = 0; !seleccionado && i < subpage.Length; ++i)
                seleccionado = (m_Page == subpage[i]);
            AddButton(x, y - 1, seleccionado ? 4006 : 4005, 4007, buttonID, GumpButtonType.Reply, 0);
            AddHtml(x + 35, y, 200, 20, Color(text, seleccionado ? Azul : Blanco), false, false);
        }
        public void AddButtonLabeled(int x, int y, int buttonID, string text)
        {
            AddButton(x, y - 1, 4005, 4007, buttonID, GumpButtonType.Reply, 0);
            AddHtml(x + 35, y, 240, 20, Color(text, Blanco), false, false);
        }
        public int GetButtonID(int type, int index)
        {
            return 1 + (index * 15) + type;
        }
        public string Color(string text, int color)
        {
            return String.Format("<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text);
        }
        public EmoteGump(Mobile from, EmotePage page)
            : base(600, 50)
        {
            from.CloseGump(typeof(EmoteGump));
            m_From = from;
            m_Page = page;
            Closable = true;
            Dragable = true;
            AddPage(0);
            //AddBackground( 0, 65, 130, 360, 5054);
            //AddAlphaRegion( 10, 70, 110, 350 );
            //AddImageTiled(10, 70, 110, 20, 9354);
            AddBackground(0, 65, 240, 360, 5054);
            AddAlphaRegion(10, 70, 220, 350);
            AddImageTiled(10, 70, 220, 20, 9354);
            AddLabel(13, 70, 200, "Liste des Emotes");
            AddImage(210, 0, 10410);
            AddImage(210, 305, 10412);
            AddImage(210, 150, 10411);
            switch (page)
            {
                case EmotePage.P1:
                    {
                        AddButtonLabeled(10, 90, GetButtonID(1, 1), "dire_ha! (ha!)");
                        AddButtonLabeled(10, 115, GetButtonID(1, 2), "dire_ha!ha! (ha!ha!)");
                        AddButtonLabeled(10, 140, GetButtonID(1, 3), "applaudir (clap!clap!)");
                        AddButtonLabeled(10, 165, GetButtonID(1, 4), "se_moucher (sprot!)");
                        AddButtonLabeled(10, 190, GetButtonID(1, 5), "saluer (salut!)");
                        AddButtonLabeled(10, 215, GetButtonID(1, 6), "s_etrangler (gloups!)");
                        AddButtonLabeled(10, 240, GetButtonID(1, 7), "faire_un_rot (rot!)");
                        AddButtonLabeled(10, 265, GetButtonID(1, 8), "eclaicir_sa_voix (humhum!)");
                        AddButtonLabeled(10, 290, GetButtonID(1, 9), "tousser (cof!cof!)");
                        AddButtonLabeled(10, 315, GetButtonID(1, 10), "pleurer (bouhou!)");  //cry
                        AddButtonLabeled(10, 340, GetButtonID(1, 11), "tomber (boum!)");
                        AddButtonLabeled(10, 365, GetButtonID(1, 12), "peter (prout!)");
                        AddButton(70, 380, 4502, 0504, GetButtonID(0, 2), GumpButtonType.Reply, 0);
                        break;
                    }
                case EmotePage.P2:
                    {
                        AddButtonLabeled(10, 90, GetButtonID(1, 13), "haleter (gasp!)");
                        AddButtonLabeled(10, 115, GetButtonID(1, 14), "poffer_de_rire (mouahaha!)");
                        AddButtonLabeled(10, 140, GetButtonID(1, 15), "jouir (aaahm!)");
                        AddButtonLabeled(10, 165, GetButtonID(1, 16), "grogner (grrr!)");
                        AddButtonLabeled(10, 190, GetButtonID(1, 17), "apostreopher (he!)");
                        AddButtonLabeled(10, 215, GetButtonID(1, 18), "avoir_le_hoquet (hic!)");
                        AddButtonLabeled(10, 240, GetButtonID(1, 19), "s_etonner (hein?)");
                        AddButtonLabeled(10, 265, GetButtonID(1, 20), "embrasser (smak!)");
                        AddButtonLabeled(10, 290, GetButtonID(1, 21), "rire (ha!ha!ha!)");  //Laughs
                        AddButtonLabeled(10, 315, GetButtonID(1, 22), "nier (non!)");
                        AddButtonLabeled(10, 340, GetButtonID(1, 23), "dire_oh! (oh!)");
                        AddButtonLabeled(10, 365, GetButtonID(1, 24), "dire_ouh! (Ouh!)");
                        AddButton(10, 380, 4506, 4508, GetButtonID(0, 1), GumpButtonType.Reply, 0);
                        AddButton(70, 380, 4502, 0504, GetButtonID(0, 3), GumpButtonType.Reply, 0);
                        break;
                    }
                case EmotePage.P3:
                    {
                        AddButtonLabeled(10, 90, GetButtonID(1, 25), "s_excuser (oups!)");
                        AddButtonLabeled(10, 115, GetButtonID(1, 26), "vomir (blurp!)");
                        AddButtonLabeled(10, 140, GetButtonID(1, 27), "cogner (paf!)");
                        AddButtonLabeled(10, 165, GetButtonID(1, 28), "hurler (haaa!)");
                        AddButtonLabeled(10, 190, GetButtonID(1, 29), "faire_taire (chut!)");
                        AddButtonLabeled(10, 215, GetButtonID(1, 30), "soupirer (pfff!)");
                        AddButtonLabeled(10, 240, GetButtonID(1, 31), "gifler (baf!)");
                        AddButtonLabeled(10, 265, GetButtonID(1, 32), "eternuer (atchoum!)");
                        AddButtonLabeled(10, 290, GetButtonID(1, 33), "reniffler (snif!)");
                        AddButtonLabeled(10, 315, GetButtonID(1, 34), "ronfler (zzz!)");
                        AddButtonLabeled(10, 340, GetButtonID(1, 35), "cracher (steuh!)"); //spit
                        AddButtonLabeled(10, 365, GetButtonID(1, 36), "tirer_la_langue (meuh!)");
                        AddButton(10, 380, 4506, 4508, GetButtonID(0, 2), GumpButtonType.Reply, 0);
                        AddButton(70, 380, 4502, 0504, GetButtonID(0, 4), GumpButtonType.Reply, 0);
                        break;
                    }
                case EmotePage.P4:
                    {
                        AddButtonLabeled(10, 90, GetButtonID(1, 37), "s_impatienter (tap!tap!)");
                        AddButtonLabeled(10, 115, GetButtonID(1, 38), "siffler (fouit!)");
                        AddButtonLabeled(10, 140, GetButtonID(1, 39), "feliciter (youhou!)");
                        AddButtonLabeled(10, 165, GetButtonID(1, 40), "bailler (houam!)");
                        AddButtonLabeled(10, 190, GetButtonID(1, 41), "dire_ouais (ouais!)");
                        AddButtonLabeled(10, 215, GetButtonID(1, 42), "cri_de_guerre (yaaah!)");
                        AddButton(10, 380, 4506, 4508, GetButtonID(0, 3), GumpButtonType.Reply, 0);
                        break;
                    }
            }
        }
        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            int val = info.ButtonID - 1;
            if (val < 0)
                return;

            Mobile from = m_From;
            int type = val % 15;
            int index = val / 15;

            switch (type)
            {
                case 0:
                    {
                        EmotePage page;
                        switch (index)
                        {
                            case 1: page = EmotePage.P1; break;
                            case 2: page = EmotePage.P2; break;
                            case 3: page = EmotePage.P3; break;
                            case 4: page = EmotePage.P4; break;
                            default: return;
                        }

                        from.SendGump(new EmoteGump(from, page));
                        break;
                    }
                case 1:
                    {
                        if (index > 0 && index < 13)
                        {
                            from.SendGump(new EmoteGump(from, EmotePage.P1));
                        }
                        else if (index > 12 && index < 25)
                        {
                            from.SendGump(new EmoteGump(from, EmotePage.P2));
                        }
                        else if (index > 24 && index < 37)
                        {
                            from.SendGump(new EmoteGump(from, EmotePage.P3));
                        }
                        else if (index > 36 && index < 43)
                        {
                            from.SendGump(new EmoteGump(from, EmotePage.P4));
                        }
                        new ESound(from, index);
                        break;
                    }
            }
        }
    }
    public class ItemRemovalTimer : Timer
    {
        private Item i_item;
        public ItemRemovalTimer(Item item)
            : base(TimeSpan.FromSeconds(1.0))
        {
            Priority = TimerPriority.OneSecond;
            i_item = item;
        }

        protected override void OnTick()
        {
            if ((i_item != null) && (!i_item.Deleted))
            {
                i_item.Delete();
                Stop();
            }
        }
    }

    public class Puke : Item
    {
        private Timer m_Timer;

        [Constructable]
        public Puke()
            : base(Utility.RandomList(0xf3b, 0xf3c))
        {
            Name = "Du Vomis";  // a pile of puke 
            Hue = 0x557;
            Movable = false;

            m_Timer = new ItemRemovalTimer(this);
            m_Timer.Start();

        }

        public override void OnAfterDelete()
        {
            base.OnAfterDelete();

            if (m_Timer != null)
                m_Timer.Stop();
        }

        public override void OnSingleClick(Mobile from)
        {
            this.LabelTo(from, this.Name);
        }

        public Puke(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            this.Delete(); // none when the world starts 
        }
    }

    public class ESound
    {
        public ESound(Mobile pm, int SoundMade)
        {
            switch (SoundMade)
            {
                case 1:
                    pm.PlaySound(pm.Female ? 778 : 1049);
                    pm.Say("*ha!*");
                    break;
                case 2:
                    pm.PlaySound(pm.Female ? 779 : 1050);
                    pm.Say("*ha! ha!*");
                    break;
                case 3:
                    pm.PlaySound(pm.Female ? 780 : 1051);
                    pm.Say("*applaudis*");
                    break;
                case 4:
                    pm.PlaySound(pm.Female ? 781 : 1052);
                    pm.Say("*se mouche*");
                    if (!pm.Mounted)
                        pm.Animate(34, 5, 1, true, false, 0);
                    break;
                case 5:
                    pm.Say("*salut*");
                    if (!pm.Mounted)
                        pm.Animate(32, 5, 1, true, false, 0);
                    break;
                case 6:
                    pm.PlaySound(pm.Female ? 786 : 1057);
                    pm.Say("*s'etrangle*");
                    break;
                case 7:
                    pm.PlaySound(pm.Female ? 782 : 1053);
                    pm.Say("*rot!*");
                    if (!pm.Mounted)
                        pm.Animate(33, 5, 1, true, false, 0);
                    break;
                case 8:
                    pm.PlaySound(pm.Female ? 784 : 1055);
                    pm.Say("*humhum!*");
                    if (!pm.Mounted)
                        pm.Animate(33, 5, 1, true, false, 0);
                    break;
                case 9:
                    pm.PlaySound(pm.Female ? 785 : 1056);
                    pm.Say("*tousse*");
                    if (!pm.Mounted)
                        pm.Animate(33, 5, 1, true, false, 0);
                    break;
                case 10:
                    pm.PlaySound(pm.Female ? 787 : 1058);
                    pm.Say("*pleure*");  //cries ??
                    break;
                case 11:
                    pm.PlaySound(pm.Female ? 791 : 1063);
                    pm.Say("*tombe*");
                    if (!pm.Mounted)
                        pm.Animate(22, 5, 1, true, false, 0);
                    break;
                case 12:
                    pm.PlaySound(pm.Female ? 792 : 1064);
                    pm.Say("*prout!*");
                    break;
                case 13:
                    pm.PlaySound(pm.Female ? 793 : 1065);
                    pm.Say("*gasp!*");
                    break;
                case 14:
                    pm.PlaySound(pm.Female ? 794 : 1066);
                    pm.Say("*mouahaha!*");
                    break;
                case 15:
                    pm.PlaySound(pm.Female ? 795 : 1067);
                    pm.Say("*jouis...*");
                    break;
                case 16:
                    pm.PlaySound(pm.Female ? 796 : 1068);
                    pm.Say("*grrr!*");
                    break;
                case 17:
                    pm.PlaySound(pm.Female ? 797 : 1069);
                    pm.Say("*hé!*");
                    break;
                case 18:
                    pm.PlaySound(pm.Female ? 798 : 1070);
                    pm.Say("*hic!*");
                    break;
                case 19:
                    pm.PlaySound(pm.Female ? 799 : 1071);
                    pm.Say("*hein?*");
                    break;
                case 20:
                    pm.PlaySound(pm.Female ? 800 : 1072);
                    pm.Say("*embrasse*");
                    break;
                case 21:
                    pm.PlaySound(pm.Female ? 801 : 1073);
                    pm.Say("*rire*");
                    break;
                case 22:
                    pm.PlaySound(pm.Female ? 802 : 1074);
                    pm.Say("*non!*");
                    break;
                case 23:
                    pm.PlaySound(pm.Female ? 803 : 1075);
                    pm.Say("*oh!*");
                    break;
                case 24:
                    pm.PlaySound(pm.Female ? 811 : 1085);
                    pm.Say("*hou!*");
                    break;
                case 25:
                    pm.PlaySound(pm.Female ? 812 : 1086);
                    pm.Say("*oups!*");
                    break;
                case 26:
                    pm.PlaySound(pm.Female ? 813 : 1087);
                    pm.Say("*vomis*");
                    if (!pm.Mounted)
                        pm.Animate(32, 5, 1, true, false, 0);
                    Point3D p = new Point3D(pm.Location);
                    switch (pm.Direction)
                    {
                        case Direction.North:
                            p.Y--; break;
                        case Direction.South:
                            p.Y++; break;
                        case Direction.East:
                            p.X++; break;
                        case Direction.West:
                            p.X--; break;
                        case Direction.Right:
                            p.X++; p.Y--; break;
                        case Direction.Down:
                            p.X++; p.Y++; break;
                        case Direction.Left:
                            p.X--; p.Y++; break;
                        case Direction.Up:
                            p.X--; p.Y--; break;
                        default:
                            break;
                    }
                    p.Z = pm.Map.GetAverageZ(p.X, p.Y);

                    bool canFit = Server.Spells.SpellHelper.AdjustField(ref p, pm.Map, 12, false);

                    if (canFit)
                    {
                        Puke puke = new Puke();
                        puke.Map = pm.Map;
                        puke.Location = p;
                    }
                    /*else
                        pm.SendMessage( "your puke won't fit!" ); /* Debug testing */
                    break;
                case 27:
                    pm.PlaySound(315);
                    pm.Say("*frappe*");
                    if (!pm.Mounted)
                        pm.Animate(31, 5, 1, true, false, 0);
                    break;
                case 28:
                    pm.PlaySound(pm.Female ? 814 : 1088);
                    pm.Say("*ahhhh!*");
                    break;
                case 29:
                    pm.PlaySound(pm.Female ? 815 : 1089);
                    pm.Say("*chute!*");
                    break;
                case 30:
                    pm.PlaySound(pm.Female ? 816 : 1090);
                    pm.Say("*pfff!*");
                    break;
                case 31:
                    pm.PlaySound(948);
                    pm.Say("*giffle*");
                    if (!pm.Mounted)
                        pm.Animate(11, 5, 1, true, false, 0);
                    break;
                case 32:
                    pm.PlaySound(pm.Female ? 817 : 1091);
                    pm.Say("*atchoum!*");
                    if (!pm.Mounted)
                        pm.Animate(32, 5, 1, true, false, 0);
                    break;
                case 33:
                    pm.PlaySound(pm.Female ? 818 : 1092);
                    pm.Say("*snif!*");
                    if (!pm.Mounted)
                        pm.Animate(34, 5, 1, true, false, 0);
                    break;
                case 34:
                    pm.PlaySound(pm.Female ? 819 : 1093);
                    pm.Say("*ronfle*");
                    break;
                case 35:
                    pm.PlaySound(pm.Female ? 820 : 1094);
                    pm.Say("*crache*");
                    if (!pm.Mounted)
                        pm.Animate(6, 5, 1, true, false, 0);
                    break;
                case 36:
                    pm.PlaySound(792);
                    pm.Say("*tire la langue*");
                    break;
                case 37:
                    pm.PlaySound(874);
                    pm.Say("*tape des pieds*");
                    if (!pm.Mounted)
                        pm.Animate(38, 5, 1, true, false, 0);
                    break;
                case 38:
                    pm.PlaySound(pm.Female ? 821 : 1095);
                    pm.Say("*siffle*");
                    if (!pm.Mounted)
                        pm.Animate(5, 5, 1, true, false, 0);
                    break;
                case 39:
                    pm.PlaySound(pm.Female ? 783 : 1054);
                    pm.Say("*felicite*");
                    break;
                case 40:
                    pm.PlaySound(pm.Female ? 822 : 1096);
                    pm.Say("*baille*");
                    if (!pm.Mounted)
                        pm.Animate(17, 5, 1, true, false, 0);
                    break;
                case 41:
                    pm.PlaySound(pm.Female ? 823 : 1097);
                    pm.Say("*ouais!*");
                    break;
                case 42:
                    pm.PlaySound(pm.Female ? 824 : 1098);
                    pm.Say("*crie*");
                    break;
            }
        }
    }
}