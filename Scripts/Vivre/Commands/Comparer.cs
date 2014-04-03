using Server.Commands;
using Server.Accounting;
using Server.Network;
using Server.Gumps;
using Server.Targeting;
using Server.Mobiles;
using Server.Items;
using System;
using System.Collections;
using Server;

namespace Server.Scripts.Commands
{
    public class Comparer
    {
        public static void Initialize()
        {
            CommandSystem.Register("Comparer", AccessLevel.Player, new CommandEventHandler(RolePlay_OnCommand));
        }

        [Usage("Comparer")]
        [Description("Permet d'effectuer un jet RolePlay")]
        private static void RolePlay_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            if (from != null && !from.Deleted)
            {
                from.SendMessage("Sélectionnez une personne");
                from.Target = new RolePlayTarget();
            }
        }
    }

    public class RolePlayTarget : Target
    {
        public RolePlayTarget()
            : base(15, false, TargetFlags.None)
        {
        }
        protected override void OnTarget(Mobile from, object target)
        {
            if (from != null && !from.Deleted && target != null)
            {
                if (target is PlayerMobile)
                {
                    Mobile targetted = target as Mobile;
                    if (targetted == from)
                    {
                        from.SendMessage("Cela sera un jet sur vous-même...");
                    }
                    else
                    {
                        from.SendMessage("Cela sera un jet contre un joueur...");
                        targetted.SendMessage("{0} prépare une jet RolePlay avec vous...", from.Name);
                    }
                    from.SendGump(new RolePlayGump(from, (Mobile)target, 0, false, 1, false));
                }
                else if (target is Mobile)
                {
                    from.SendMessage("Cela sera un jet contre un personnage non joueur...");
                    from.SendGump(new RolePlayGump(from, (Mobile)target, 0, false, 1, false));
                }
            }
        }
    }


    public class RolePlayGump : Server.Gumps.Gump
    {

        private Mobile m_Target;
        private int sk;
        private bool m_dif;
        private bool m_playeronly;
        private int m_carac;

        public RolePlayGump(Mobile from, Mobile target, int s, bool dif, int carac, bool playeronly)
            : base(0, 0)
        {
            m_Target = target;
            sk = s;
            m_dif = dif;
            m_carac = carac;
            m_playeronly = playeronly;
            if (sk < 0) sk = 0;
            if (sk > 53) sk = 53;
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            this.AddBackground(79, 71, 472, 235, 9200);
            this.AddBackground(79, 43, 471, 27, 9200);
            this.AddImage(88, 46, 10450);
            this.AddImage(504, 48, 10450);
            this.AddPage(1);
            this.AddLabel(88, 96, 0, @"Choisissez la caractéristique");
            this.AddLabel(317, 98, 0, @"Choisissez également la compétence");
            this.AddLabel(88, 120, 0, @"correspondant le mieux à l'action:");
            this.AddLabel(317, 122, 0, @"correspondant le mieux à l'action:");
            this.AddRadio(88, 145, 210, 211, (m_carac == 1), (int)Buttons.FORCE);
            this.AddRadio(88, 176, 210, 211, (m_carac == 2), (int)Buttons.INTELLIGENCE);
            this.AddRadio(88, 205, 210, 211, (m_carac == 3), (int)Buttons.DEXTERITE);
            this.AddLabel(112, 144, 0, @"FORCE");
            this.AddLabel(112, 174, 0, @"INTELLIGENCE");
            this.AddLabel(112, 205, 0, @"DEXTERITE");
            this.AddImage(342, 178, 2440);
            if (sk > 0)
                this.AddLabel(356, 156, 0, SkillInfo.Table[sk - 1].Name);
            else
                this.AddLabel(356, 156, 0, @"");
            this.AddLabel(356, 180, 0, SkillInfo.Table[sk].Name);
            if (sk < 53)
                this.AddLabel(356, 204, 0, SkillInfo.Table[sk + 1].Name);
            else
                this.AddLabel(356, 204, 0, @"");
            this.AddButton(324, 162, 250, 251, (int)Buttons.UP, GumpButtonType.Reply, 0);
            this.AddButton(324, 195, 252, 253, (int)Buttons.DOWN, GumpButtonType.Reply, 0);
            this.AddButton(320, 275, 247, 248, (int)Buttons.OK, GumpButtonType.Reply, 0);
            this.AddButton(225, 273, 241, 242, (int)Buttons.CANCEL, GumpButtonType.Reply, 0);
            this.AddImageTiled(300, 84, 4, 154, 2701);
            this.AddAlphaRegion(344, 155, 163, 19);
            this.AddAlphaRegion(345, 204, 162, 20);
            this.AddLabel(197, 47, 0, @"             Test RolePlay");
            if (from.AccessLevel > AccessLevel.Player)
            {
                this.AddLabel(376, 240, 0, @"Difficile");
                this.AddCheck(353, 242, 210, 211, m_dif, (int)Buttons.DIFFICILE);
                this.AddLabel(176, 240, 0, @"Joueur seul");
                this.AddCheck(153, 242, 210, 211, m_playeronly, (int)Buttons.PLAYER);
            }
        }

        public enum Buttons
        {
            EXIT,
            FORCE,
            INTELLIGENCE,
            DEXTERITE,
            UP,
            DOWN,
            OK,
            CANCEL,
            DIFFICILE,
            PLAYER
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            m_dif = false;

            for (int i = 0; i < info.Switches.Length; ++i)
            {
                if (i == 0)
                    m_carac = info.Switches[i];
                if (i == 1)
                    m_dif = true;
                if (i == 2)
                    m_playeronly = true;
            }

            switch (info.ButtonID)
            {
                case (int)Buttons.UP:
                    from.SendGump(new RolePlayGump(from, m_Target, sk - 1, m_dif, m_carac, m_playeronly));
                    return;
                case (int)Buttons.DOWN:
                    from.SendGump(new RolePlayGump(from, m_Target, sk + 1, m_dif, m_carac, m_playeronly));
                    return;
                case (int)Buttons.OK:
                    if (m_Target is PlayerMobile)
                        m_Target.SendGump(new RolePlayConfirmGump(from, m_Target, sk, m_dif, m_carac, m_playeronly));
                    else
                        from.SendGump(new RolePlayConfirmGump(from, m_Target, sk, m_dif, m_carac, m_playeronly));
                    from.SendMessage("Demande de test roleplay envoyée à {0}.", m_Target.Name);
                    break;
                case (int)Buttons.CANCEL:
                    break;
                default:
                    break;
            }
        }
    }

    public class RolePlayConfirmGump : Server.Gumps.Gump
    {

        private Mobile m_From;
        private int sk;
        private bool m_dif;
        private bool m_playeronly;
        private int m_carac;
        private Mobile targ;

        public RolePlayConfirmGump(Mobile from, Mobile target, int s, bool dif, int carac, bool playeronly)
            : base(0, 0)
        {
            m_From = from;
            targ = target;
            sk = s;
            m_dif = dif;
            m_carac = carac;
            m_playeronly = playeronly;
            if (sk < 0) sk = 0;
            if (sk > 53) sk = 53;
            this.Closable = true;
            this.Disposable = false;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            this.AddBackground(86, 72, 357, 113, 9200);
            this.AddPage(1);
            this.AddLabel(219, 78, 0, @"Test RolePlay");
            if (!(target is PlayerMobile))
            {
                this.AddLabel(94, 100, 0, @"C'est un jet contre un Personnage Non Joueur.");
            }
            else if (from == target)
            {
                this.AddLabel(94, 100, 0, @"Vous faites un test Roleplay sur vous-même.");
            }
            else if (m_playeronly)
            {
                this.AddLabel(94, 100, 0, @"Un Test Roleplay est demandé par un Maitre de Jeu.");
            }
            else
            {
                this.AddLabel(94, 100, 0, String.Format("{0} veut faire un test Roleplay avec vous.", from.Name));
                this.AddLabel(97, 153, 0, @"Acceptez-vous ?");
                this.AddButton(281, 160, 12018, 12019, (int)Buttons.CANCEL, GumpButtonType.Reply, 0);
            }
            if (m_carac == 1)
                this.AddLabel(96, 117, 0, @"Caractéristique utilisee: FORCE");
            else if (m_carac == 2)
                this.AddLabel(96, 117, 0, @"Caractéristique utilisee: INTELLIGENCE");
            else
                this.AddLabel(96, 117, 0, @"Caractéristique utilisee: DEXTERITE");
            this.AddLabel(96, 135, 0, String.Format("Compétence utilisée: {0}", SkillInfo.Table[sk].Name));
            this.AddButton(361, 161, 12000, 12001, (int)Buttons.OK, GumpButtonType.Reply, 0);
        }

        public enum Buttons
        {
            CANCEL,
            OK
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            switch (info.ButtonID)
            {
                case (int)Buttons.CANCEL:
                    m_From.SendMessage("{0} a refusé le test.", targ.Name);
                    break;
                case (int)Buttons.OK:
                    double scoreA;
                    double scoreB;
                    int caracA;
                    int caracB;
                    if (m_carac == 1)
                    {
                        caracA = m_From.Str;
                        caracB = targ.Str;
                    }
                    else if (m_carac == 2)
                    {
                        caracA = m_From.Int;
                        caracB = targ.Int;
                    }
                    else
                    {
                        caracA = m_From.Dex;
                        caracB = targ.Dex;
                    }
                    if ((m_From == targ) || m_playeronly)
                    {
                        if (m_dif)
                            scoreA = Utility.RandomDouble() * 100.0 + 25.0;
                        else
                            scoreA = Utility.RandomDouble() * 100.0;
                    }
                    else
                    {
                        scoreA = Utility.RandomDouble() *
                            (targ.Skills[(SkillName)sk].Value + caracA / 10) / 2;

                    }
                    scoreB = Utility.RandomDouble() *
                        (targ.Skills[(SkillName)sk].Value + caracA / 10) / 2;
                    if (scoreA > scoreB)
                    {
                        if (m_From == targ)
                        {
                            m_From.SendMessage("Test RolePlay réussi.");
                            IPooledEnumerable eable =
                                m_From.Map.GetMobilesInRange(m_From.Location, 30);
                            ArrayList list = new ArrayList();
                            foreach (Mobile mob in eable)
                            {
                                if (mob.AccessLevel > AccessLevel.Player)
                                {
                                    mob.SendMessage("{0} a réussi le test RolePlay.", targ.Name);
                                    if (m_carac == 1)
                                        mob.SendMessage("Caractéristique utilisee: FORCE");
                                    else if (m_carac == 2)
                                        mob.SendMessage("Caractéristique utilisee: INTELLIGENCE");
                                    else
                                        mob.SendMessage("Caractéristique utilisee: DEXTERITE");
                                    mob.SendMessage("Compétence utilisée: {0}", SkillInfo.Table[sk].Name);
                                }
                            }
                            eable.Free();
                        }
                        else if (m_playeronly)
                        {
                            targ.SendMessage("Test RolePlay réussi.");
                            m_From.SendMessage("{0} a réussi le test RolePlay.", targ.Name);
                        }
                        else
                        {
                            targ.SendMessage("Test RolePlay contre {0} réussi.", m_From.Name);
                            m_From.SendMessage("Test RolePlay contre {0} raté.", targ.Name);
                        }
                    }
                    else
                    {
                        if (m_From == targ)
                        {
                            m_From.SendMessage("Test RolePlay raté.");
                            IPooledEnumerable eable =
                                m_From.Map.GetMobilesInRange(m_From.Location, 30);
                            ArrayList list = new ArrayList();
                            foreach (Mobile mob in eable)
                            {
                                if (mob.AccessLevel > AccessLevel.Player)
                                {
                                    mob.SendMessage("{0} a raté le test RolePlay.", targ.Name);
                                    if (m_carac == 1)
                                        mob.SendMessage("Caractéristique utilisee: FORCE");
                                    else if (m_carac == 2)
                                        mob.SendMessage("Caractéristique utilisee: INTELLIGENCE");
                                    else
                                        mob.SendMessage("Caractéristique utilisee: DEXTERITE");
                                    mob.SendMessage("Compétence utilisée: {0}", SkillInfo.Table[sk].Name);
                                }
                            }
                            eable.Free();
                        }
                        else if (m_playeronly)
                        {
                            targ.SendMessage("Test RolePlay raté.");
                            m_From.SendMessage("{0} a raté le test RolePlay.", targ.Name);
                        }
                        else
                        {
                            targ.SendMessage("Test RolePlay contre {0} raté.", m_From.Name);
                            m_From.SendMessage("Test RolePlay contre {0} réussi.", targ.Name);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}