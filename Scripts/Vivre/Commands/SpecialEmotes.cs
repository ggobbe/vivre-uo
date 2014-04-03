using System;
using System.Text;
using System.Reflection;
using System.Collections;
using Server;
using Server.Network;
using Server.Targeting;

namespace Server.Commands
{
    public class SpecialEmotes
    {
        enum EmoteList
        {
            Kiss,
            Slap,
            Tete,
        }

        public static void Initialize()
        {
            // Scriptiz : commande anim à partir de Counselor
            CommandSystem.Register("Anim", AccessLevel.Counselor, new CommandEventHandler(Anim_OnCommand));
            CommandSystem.Register("Kiss", AccessLevel.Player, new CommandEventHandler(Kiss_OnCommand));
            CommandSystem.Register("Slap", AccessLevel.Player, new CommandEventHandler(Slap_OnCommand));
            CommandSystem.Register("Tete", AccessLevel.Player, new CommandEventHandler(Tete_OnCommand));
        }

        [Usage("Anim")]
        [Description("Anime le personnage")]
        public static void Anim_OnCommand(CommandEventArgs e)
        {
            if (e.Length == 1)
                e.Mobile.Animate(e.GetInt32(0), 5, 1, true, false, 1);
            else
                e.Mobile.SendMessage("Format: Anim <action>");
        }

        [Usage("Kiss")]
        [Description("Embrasser")]
        public static void Kiss_OnCommand(CommandEventArgs e)
        {
            e.Mobile.Target = new GetTarget(EmoteList.Kiss);
        }

        [Usage("Slap")]
        [Description("Donner une baffe")]
        public static void Slap_OnCommand(CommandEventArgs e)
        {
            e.Mobile.Target = new GetTarget(EmoteList.Slap);
        }

        [Usage("Tete")]
        [Description("Donner un coup de tete")]
        public static void Tete_OnCommand(CommandEventArgs e)
        {
            e.Mobile.Target = new GetTarget(EmoteList.Tete);
        }

        private class GetTarget : Target
        {
            private EmoteList m_Emote;

            public GetTarget(EmoteList emote)
                : base(-1, true, TargetFlags.None)
            {
                this.m_Emote = emote;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (!from.Alive)
                {
                    from.SendMessage("Vous ne pouvez pas faire cela en étant mort !");
                    return;
                }
                else if (from.Warmode)
                {
                    from.SendMessage("Vous ne pouvez pas faire cela en combat!");
                    return;
                }
                else if (!(targeted is Mobile))
                {
                    from.SendMessage("Vous ne pouvez pas cibler cela !");
                    return;
                }

                Mobile targ = (Mobile)targeted;

                switch (m_Emote)
                {
                    case EmoteList.Kiss:
                        from.Emote("*Tu vois {0} envoyer un baiser à {1}*", from.Name, targ.Name);
                        from.Animate(34, 5, 1, true, false, 1);
                        from.PlaySound(from.Female ? 0x320 : 0x430);
                        if(targ.BodyValue == 0x51 && from.Female != targ.Female)
                        {
                                targ.BodyValue = targ.Female ? 401 : 400;
                                targ.SendMessage("Au final, vous aurez peut-être trouvé l'amour?");
                        }
                        break;
                    case EmoteList.Slap:
                        if (from.InRange(targ.Location, 1))
                        {
                            from.Emote("*Tu vois {0} envoyer une baffe à {1}*", from.Name, targ.Name);
                            from.Animate(9, 5, 1, true, false, 1);
                            targ.Emote("*Tu vois {0} recevoir une baffe de {1}*", targ.Name, from.Name);
                            targ.Animate(20, 5, 1, true, false, 1);
                            targ.PlaySound(0x135);

                        }
                        else
                            from.SendMessage("Vous êtes trop loin pour le frapper");
                        break;
                    case EmoteList.Tete:
                        if (from.InRange(targ.Location, 1))
                        {
                            from.Emote("*Tu vois {0} envoyer un coup de boule à {1}*", from.Name, targ.Name);
                            from.Animate(12, 5, 1, true, false, 1);
                            targ.Emote("*Tu vois {0} recevoir un coup de boule de {1}*", targ.Name, from.Name);
                            if (from.Direction == targ.Direction)
                                targ.Animate(22, 5, 1, true, false, 1);
                            else
                                targ.Animate(21, 5, 1, true, false, 1);
                            targ.PlaySound(0x135);
                        }
                        else
                            from.SendMessage("Vous êtes trop loin pour le frapper");
                        break;
                }
            }
        }
    }
}
