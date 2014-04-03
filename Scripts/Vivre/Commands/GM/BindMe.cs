using System;
using Server;
using System.Collections.Generic;
using Server.Targeting;

namespace Server.Commands
{
    public class BindMe
    {
        private static Dictionary<Mobile, Mobile> binders = new Dictionary<Mobile, Mobile>();

        public static void Initialize()
        {
            EventSink.Speech += new SpeechEventHandler(EventSink_Speech);
            EventSink.Logout += new LogoutEventHandler(EventSink_Logout);
            EventSink.Login += new LoginEventHandler(EventSink_Login);
            CommandSystem.Register("BindMe", AccessLevel.GameMaster, new CommandEventHandler(BindMe_OnCommand));
        }

        private static void BindMe_OnCommand(CommandEventArgs e)
        {
            e.Mobile.Target = new InternalTarget();
        }

        private static void EventSink_Login(LoginEventArgs args)
        {
            if (binders.ContainsKey(args.Mobile))
                binders.Remove(args.Mobile);
        }

        private static void EventSink_Logout(LogoutEventArgs args)
        {
            if (binders.ContainsKey(args.Mobile))
                binders.Remove(args.Mobile);
        }

        private static void EventSink_Speech(SpeechEventArgs args)
        {
            if (args.Mobile == null) return;

            Mobile speaker = args.Mobile;

            if (!binders.ContainsKey(speaker))
                return;

            Mobile b;
            if (binders.TryGetValue(speaker, out b) && b != null && !b.Deleted)
                b.Say(args.Speech);
            else
            {
                args.Mobile.SendMessage("Le mobile lié n'existe plus");
                binders.Remove(args.Mobile);
            }
        }

        private class InternalTarget : Target
        {
            public InternalTarget()
                : base(25, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targ)
            {
                if (targ is Mobile)
                {
                    Mobile m = (Mobile)targ;
                    if (m == null) return;

                    if (binders.ContainsKey(from))
                    {
                        Mobile b;
                        
                        if(binders.TryGetValue(from, out b) && b != null)
                            from.SendMessage(String.Format("Vous vous déliez de {0}", b.Name));

                        binders.Remove(from);
                    }

                    if (from == m) return;

                    binders.Add(from, m);
                    from.SendMessage("Vous êtes maintenant lié à {0}", m.Name);
                }
                else
                {
                    from.SendMessage("Veuillez cibler un Mobile.");
                }
            }
        }
    }
}
