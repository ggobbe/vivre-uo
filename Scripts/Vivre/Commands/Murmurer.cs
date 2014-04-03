// Scriptiz : un peu l'équivalent du ; <message> mais fonctionne sur une plus longue distance
// et avec une seule autre personne
using System;
using Server;
using Server.Targeting;
using Server.Mobiles;
using Server.Commands;

namespace Server.Commands
{
    public class Murmurer
    {
        public static void Initialize()
        {
            CommandSystem.Register("Murmurer", AccessLevel.Player, new CommandEventHandler(Murmure_OnCommand));
            CommandSystem.Register("mm", AccessLevel.Player, new CommandEventHandler(Murmure_OnCommand));
        }

        [Usage("Murmurer [string]")]
        [Description("Murmure presque inaudible.")]
        private static void Murmure_OnCommand(CommandEventArgs e)
        {
            string text = e.ArgString;//e.GetString(0);
            e.Mobile.Target = new InternalTarget(text);
        }

        private class InternalTarget : Target
        {
            private string m_text;

            public InternalTarget(string text)
                : base(1, false, TargetFlags.None)
            {
                m_text = text;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is PlayerMobile)
                {
                    Mobile mobile = (Mobile)o;

                    if (from.InRange(mobile, 8))
                    {
                        from.Say("*Murmure à l'oreille de " + mobile.Name + " *");
                        mobile.SendMessage(from.Name + " vous murmure: " + m_text);
                    }
                    else from.SendMessage("Il est trop loin pour vous entendre...");
                }
                else
                    from.SendMessage("Il ne comprend rien à ce que vous lui murmurez!");
            }
        }
    }
}