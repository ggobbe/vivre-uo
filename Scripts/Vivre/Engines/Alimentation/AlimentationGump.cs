using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Mobiles;
using Server.Network;
using Server.Misc;

namespace Server.Gumps
{
    public class AlimentationGump : Gump
    {
        public static void Initialize()
        {
            CommandSystem.Register("Faim", AccessLevel.Player, new CommandEventHandler(Alimentation_OnCommand));
            CommandSystem.Register("Soif", AccessLevel.Player, new CommandEventHandler(Alimentation_OnCommand));
        }

        [Usage("Faim")]
        [Aliases("Soif")]
        [Description("Affiche le degré de faim et de soif du joueur en cours.")]
        private static void Alimentation_OnCommand(CommandEventArgs e)
        {
            Alimentation.SendGump(e.Mobile);
        }

        Mobile m_Owner;
        int x, y;

        public AlimentationGump(Mobile owner)
            : this(owner, PropsConfig.GumpOffsetX, PropsConfig.GumpOffsetY)
        {
        }

        public AlimentationGump(Mobile owner, int x, int y)
            : base(x, y)    // offset x et y
        {
            this.m_Owner = owner;

            this.Closable = true;
            this.Disposable = false;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(0, 0, 184, 61, 9200);

            // Hunger background (yellow or red)
            if(this.m_Owner.Hunger > 10)
                AddImageTiled(60, 15, 109, 11, 2057);
            else
                AddImageTiled(60, 15, 109, 11, 2053);

            // Hunger foreground (blue)
            int hunger = (int)((this.m_Owner.Hunger / 20.0) * 109);
			if(hunger > 109) hunger = 109;	// To enable hunger boosts
            AddImageTiled(60, 15, hunger, 11, 2054);

            // Thirst background (yellow or red)
            if(this.m_Owner.Thirst > 10)
                AddImageTiled(60, 35, 109, 11, 2057);
            else
                AddImageTiled(60, 35, 109, 11, 2053);

            // Thirst foreground (blue)
            int thirst = (int)((this.m_Owner.Thirst / 20.0) * 109);
			if(thirst > 109) thirst = 109;	// To enable thirst boosts
            AddImageTiled(60, 35, thirst, 11, 2054);

            AddLabel(15, 10, 0, @"Faim");
            AddLabel(15, 30, 0, @"Soif"); 
        }
    }
}