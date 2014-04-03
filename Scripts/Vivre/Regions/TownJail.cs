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
    public class TownJail : Jail
    {
        public TownJail(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent)
        {
        }

        public override bool AllowBeneficial(Mobile from, Mobile target)
        {
            return true;
        }

        public override bool AllowHarmful(Mobile from, Mobile target)
        {
            return true;
        }

        public override bool OnBeginSpellCast(Mobile m, ISpell s)
        {
            if ((s is GateTravelSpell || s is RecallSpell || s is MarkSpell || s is SacredJourneySpell || s is TeleportSpell || s is Shadowjump) && m.AccessLevel == AccessLevel.Player)
            {
                m.SendMessage("You cannot cast that spell here.");
                return false;
            }
            return true;
        }

        public override bool CanUseStuckMenu(Mobile m)
        {
            return false;
        }

        public override bool OnSkillUse(Mobile from, int Skill)
        {
            return true;
        }

        public override bool OnCombatantChange(Mobile from, Mobile Old, Mobile New)
        {
            return true;
        }
    }
}