/**
 * MacroCheck's Gump
 * @author Scriptiz
 * @date 20090913
 */
using System;
using Server;
using Server.Network;
using Server.Accounting;

namespace Server.Gumps
{
    public class MacroGump : Gump
    {
        private Mobile jailor;
        private Mobile badBoy;
        DateTime issued = DateTime.Now;
        Timer response;
        int gButton = 2;

        public MacroGump(Mobile from, Mobile m) : base(70, 40)
        {            
            jailor = from;
            badBoy = m;

            if (jailor == null)
            {
                jailor = new Mobile();
                jailor.Name = "Le Gardien";
            }

            gButton = (new System.Random()).Next(6);
            if (gButton < 1) gButton = 1;
            if (gButton > 6) gButton = 6;

            //((Account)m.Account).Comments.Add(new AccountComment("-warning", jailor.Name + " a vérifié si " + badBoy.Name + " était entrain de macroter à : " + DateTime.Now));

            Closable = false;
            Dragable = true;
            AddPage(0);
            AddBackground(0, 0, 326, 320, 5054);
            AddImageTiled(9, 65, 308, 240, 2624);
            AddAlphaRegion(9, 65, 308, 240);
            //AddLabel( 16, 20, 200, string.Format("{0} is checking to see if you are macroing unattended", jailor.Name));
            this.AddHtml(16, 10, 250, 50, string.Format("{0} vérifie si vous êtes entrain de macroter.", jailor.Name), false, false);
            //let them show that they are there by selecting these buttons
            AddButton(20, 72, 2472, 2473, 5, GumpButtonType.Reply, 0);
            AddLabel(50, 75, 200, gButton == 5 ? "Je suis la !" : "Je confesse, je suis un vilain macroteur.");
            AddButton(20, 112, 2472, 2473, 1, GumpButtonType.Reply, 0);
            AddLabel(50, 115, 200, gButton == 1 ? "Je suis la !" : "Je confesse, je suis un vilain macroteur.");
            AddButton(20, 152, 2472, 2473, 2, GumpButtonType.Reply, 0);
            AddLabel(50, 155, 200, gButton == 2 ? "Je suis la !" : "Je confesse, je suis un vilain macroteur.");
            AddButton(20, 192, 2472, 2473, 3, GumpButtonType.Reply, 0);
            AddLabel(50, 195, 200, gButton == 3 ? "Je suis la !" : "Je confesse, je suis un vilain macroteur.");
            AddButton(20, 232, 2472, 2473, 4, GumpButtonType.Reply, 0);
            AddLabel(50, 235, 200, gButton == 4 ? "Je suis la !" : "Je confesse, je suis un vilain macroteur.");
            AddButton(20, 272, 2472, 2473, 6, GumpButtonType.Reply, 0);
            AddLabel(50, 275, 200, gButton == 6 ? "Je suis la !" : "Je confesse, je suis un vilain macroteur.");

            if (jailor != null && badBoy != null)
            {
                m.SendMessage("Vous êtes suspecté de macrotage, veuillez répondre s'il vous plait.");
                response = new MacroTimer(this);
            }
            else
                Closable = true;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            if (response != null)
                response.Stop();

            if (gButton == info.ButtonID)
            {
                string mtemp = string.Format("{0} a répondu à la vérification en {1} secondes.", from.Name, ((TimeSpan)(DateTime.Now.Subtract(issued))).Seconds);
                //((Account)badBoy.Account).Comments.Add(new AccountComment("-warning", mtemp));
                jailor.SendMessage(mtemp);
            }
            else
            {
                string mtemp = string.Format("{0} a été kické pour cause de macrotage.", from.Name);
                ((Account)badBoy.Account).Comments.Add(new AccountComment("-warning", mtemp));
                caughtInTheAct(true);
            }
            from.CloseGump(typeof(MacroGump));
        }

        public void caughtInTheAct(bool confessed)
        {
            if (!confessed)
                jailor.SendMessage("{0} a été kické pour {1} suite au délai de votre avertissement.", badBoy.Name, "Macrotage");
            else
                jailor.SendMessage("{0} a été kické pour {1} en avouant ses crimes.", badBoy.Name, "Macrotage");

            NetState kicked = badBoy.NetState;

            if (kicked != null)
            {
                badBoy.SendMessage("Vous êtes kické pour cause de Macrotage.");
                kicked.Dispose();
            }

            if (response != null)
                response.Stop();
        }

        protected class MacroTimer : Timer
        {
            public MacroGump m_Gump;
            int counts = 60;

            public MacroTimer(MacroGump myGump)
                : base(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10))
            {
                m_Gump = myGump;
                this.Start();
            }

            protected override void OnTick()
            {
                counts -= this.Interval.Seconds;
                switch (counts)
                {
                    case 50:
                    case 40:
                    case 30:
                    case 20:
                        this.Interval = TimeSpan.FromSeconds(1);
                        goto case 10;
                    case 10:
                    case 9:
                    case 8:
                    case 7:
                    case 6:
                    case 5:
                    case 4:
                    case 3:
                    case 2:
                    case 1:
                        m_Gump.badBoy.SendMessage("Attention : fermeture dans {0} secondes.", counts);
                        break;
                    case 0:
                        m_Gump.caughtInTheAct(false);
                        m_Gump.badBoy.CloseGump(typeof(MacroGump));
                        this.Stop();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
