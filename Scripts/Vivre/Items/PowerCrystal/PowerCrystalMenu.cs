using System;
using Server.Items;
using Server.Targeting;

namespace Server.ContextMenus
{
    public class PowerCrystalMenu : ContextMenuEntry
    {
        private Mobile m_From;
        private MysticPowerCrystal m_Crystal;

        public PowerCrystalMenu(Mobile from, MysticPowerCrystal Crystal)
            : base(5114, 1)
        {
            m_From = from;
            m_Crystal = Crystal;
        }

        public override void OnClick()
        {
            m_From.SendMessage("Que souhaitez-vous imprégner de ce pouvoir?");
            m_From.BeginTarget(2, false, TargetFlags.None, new TargetCallback(PowerTarget));
        }

        public void PowerTarget(Mobile from, object obj)
        {
            if (!(obj is IronBeetleBody))
            {
                m_From.SendMessage("Cela ne servirait à rien sur cet objet");
                return;
            }

            IronBeetleBody targ = (IronBeetleBody)obj;

            if (targ.SummonScalar != 0)
            {
                from.SendMessage("Ce corps est déjà été imprégné d'une âme");
                return;
            }
            from.SendMessage("L'esprit contenu dans le Crystal se dissipe sur la carapace");
            targ.SummonScalar = m_Crystal.Completion / 100;

            if (!from.CheckSkill(SkillName.Mysticism, 60))
            {
                from.SendMessage("Le crystal se brise pendant le transfert");
                m_Crystal.Delete();
            }

            m_Crystal.Completion = 0;
            return;
        }
    }
}