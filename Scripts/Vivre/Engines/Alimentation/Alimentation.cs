using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Misc
{
    public class Alimentation
    {
        public static void CheckHunger(Mobile m)
        {
            PlayerMobile pm = null;
            if (m != null && m is PlayerMobile)
                pm = (PlayerMobile)m;

            if (pm != null && pm.Hunger <= 10 && pm.AccessLevel == AccessLevel.Player && (DateTime.Now - pm.LastOnline > TimeSpan.FromSeconds(150)))
            {
                pm.SendMessage("La faim vous crispe de douleur.");
                int damages = (int)((11 - pm.Hunger) * (pm.Str / 40.0));
                if (damages <= 0) damages = 1;

                if(damages > pm.Hits || !pm.Warmode || pm.Target == null)
                    pm.Damage(damages);
            }
        }

        public static void CheckThirst(Mobile m)
        {
            PlayerMobile pm = null;
            if (m != null && m is PlayerMobile)
                pm = (PlayerMobile)m;

            if (pm != null && pm.Thirst <= 10 && pm.AccessLevel == AccessLevel.Player && (DateTime.Now - pm.LastOnline > TimeSpan.FromSeconds(150)))
            {
                pm.SendMessage("La soif vous crispe de douleur.");
                int damages = (int)((11 - pm.Thirst) * (pm.Str / 40.0));
                if (damages <= 0) damages = 1;

                if (damages > pm.Hits || !pm.Warmode || pm.Target == null)
                    pm.Damage(damages);
            }
        }

        public static void UpdateGump(Mobile m)
        {
            if (m != null && m is PlayerMobile && m.HasGump(typeof(AlimentationGump)))
            {
                Gump ag = m.FindGump(typeof(AlimentationGump));
                int x = ag.X;
                int y = ag.Y;
                m.CloseGump(typeof(AlimentationGump));
                m.SendGump(new AlimentationGump(m, x, y));
            }
        }

        public static void SendGump(Mobile m)
        {
            if (m != null && m is PlayerMobile)
            {
                if (m.HasGump(typeof(AlimentationGump)))
                    m.CloseGump(typeof(Alimentation));

                m.SendGump(new AlimentationGump(m));
            }
        }
    }
}