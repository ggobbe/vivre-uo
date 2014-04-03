/**
 * MacroCheck's Command
 * @author Scriptiz
 * @date 20090913
 */
using System;
using Server;
using Server.Targeting;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Commands
{
    public class MacroCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("Macro", AccessLevel.Counselor, new CommandEventHandler(Macro_OnCommand));
        }

        [Usage("Macro [target]")]
        [Description("Envoie un avertissement au joueur pour déterminer s'il macrote ou non.")]
        private static void Macro_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (from != null)
            {
                from.SendMessage("Qui suspectez vous de macrotage ?");

                from.Target = new InternalTarget(e.Arguments);
            }
        }

        private class InternalTarget : Target
        {
            string[] m_parameter;

            public InternalTarget(params string[] parameter)
                : base(-1, true, TargetFlags.None)
            {
                m_parameter = parameter;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                PlayerMobile pm = null;

                if (targeted is PlayerMobile)
                    pm = (PlayerMobile)targeted;

                if (pm != null && pm.AccessLevel == AccessLevel.Player && !pm.HasGump(typeof(MacroGump)))
                {
                    pm.SendGump(new MacroGump(from, pm));
                }
            }
        }
    }
}