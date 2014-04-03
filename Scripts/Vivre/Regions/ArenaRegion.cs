using System;
using System.Xml;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Spells;
using Server.Spells.Seventh;
using Server.Spells.Fourth;
using Server.Spells.Third;
using Server.Spells.Sixth;
using Server.Spells.Chivalry;
using Server.Spells.Ninjitsu;
using Server.ServerSeasons;

namespace Server.Regions
{
    public class ArenaRegion : BaseRegion
    {
        public ArenaRegion(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent)
        {
        }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return false;
        }

        public override void OnEnter(Mobile m)
        {
            if (m is PlayerMobile && ((PlayerMobile)m).Young)
                ((PlayerMobile)m).Young = false;

            base.OnEnter(m);
        }

        public override bool OnBeginSpellCast(Mobile m, ISpell s)
        {
            // Si ce sont des joueurs
            PlayerMobile pm = m as PlayerMobile;
            if (pm != null && pm.AccessLevel == AccessLevel.Player)
            {
                // Ok si duelistes
                if (pm.IsInChallenge) return base.OnBeginSpellCast(m, s);

                // Interdit si spectateurs
                pm.SendMessage("Les spectateurs ne sont pas autorisés à faire cela !");
                return false;
            }
            return base.OnBeginSpellCast(m, s);
        }

        public override bool OnSkillUse(Mobile from, int Skill)
        {
            // Si ce sont des joueurs
            PlayerMobile pm = from as PlayerMobile;
            if (pm != null && pm.AccessLevel == AccessLevel.Player)
            {
                // Ok si duelistes
                if (pm.IsInChallenge) return base.OnSkillUse(from, Skill);

                // Interdit si spectateurs
                pm.SendMessage("Les spectateurs ne sont pas autorisés à faire cela !");
                return false;
            }
            return base.OnSkillUse(from, Skill);
        }
    }
}