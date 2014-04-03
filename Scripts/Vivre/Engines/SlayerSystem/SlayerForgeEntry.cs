using System;
using Server.Items;

namespace Server.ContextMenus
{
    public class SlayerForgeEntry : ContextMenuEntry
    {
        private Mobile m_From;
        private SlayerForge m_Forge;

        public SlayerForgeEntry(Mobile from, SlayerForge SlayerForge)
            : base(5043, 1)
        {
            m_From = from;
            m_Forge = SlayerForge;
        }

        public override void OnClick()
        {
            m_Forge.EmptyForge();
            m_From.SendMessage("Vous videz le contenu de la forge, annulant tous vos efforts");
        }
    }
}