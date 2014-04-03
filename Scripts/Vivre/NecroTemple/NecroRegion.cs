/***************************************************************************
 *                              NecroRegion.cs
 *                         -----------------------------
 *   begin                : July 10, 2011
 *   copyright            : (C) Scriptiz
 *   email                : maeliguul@hotmail.com
 *   version              : 2011-07-20
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/
using System;
using System.Xml;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Spells;
using Server.Items;
using Server.Spells.Seventh;
using Server.Spells.Fourth;
using Server.Spells.Third;
using Server.Spells.Sixth;
using Server.Spells.Chivalry;
using Server.Spells.Ninjitsu;
using Server.ServerSeasons;
using Server.Misc;
using Server.Multis;

namespace Server.Regions
{
    public class NecroRegion : BaseRegion, ISeasons
    {
        public override Season Season
        {
            get
            {
                return ServerSeasons.Season.Desolation;
            }
            set
            {
                base.Season = ServerSeasons.Season.Desolation;
            }
        }

        private Point3D m_EntranceLocation;
        private Map m_EntranceMap;

        public Point3D EntranceLocation { get { return m_EntranceLocation; } set { m_EntranceLocation = value; } }
        public Map EntranceMap { get { return m_EntranceMap; } set { m_EntranceMap = value; } }

        public NecroRegion(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent)
        {
            XmlElement entrEl = xml["entrance"];

            Map entrMap = map;
            ReadMap(entrEl, "map", ref entrMap, false);

            if (ReadPoint3D(entrEl, entrMap, ref m_EntranceLocation, false))
                m_EntranceMap = entrMap;
        }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return false;
        }

        public override void OnEnter(Mobile m)
        {
            if (m is PlayerMobile && ((PlayerMobile)m).Young)
                ((PlayerMobile)m).Young = false;

            int deleted = ScrollDeleter.DeleteNecroScrolls(m);
            if (deleted > 0) m.SendMessage(ScrollDeleter.Message);

            base.OnEnter(m);
        }

        public override void OnExit(Mobile m)
        {
            int deleted = ScrollDeleter.DeleteNecroScrolls(m);
            if (deleted > 0) m.SendMessage(ScrollDeleter.Message);

            base.OnExit(m);
        }

        public override void AlterLightLevel(Mobile m, ref int global, ref int personal)
        {
            global = LightCycle.DungeonLevel;

            if(m == null || m.AccessLevel <= AccessLevel.Counselor)
                personal = 0;
        }

        public override bool CanUseStuckMenu(Mobile m)
        {
            return false;
        }

        public override bool CheckAccessibility(Item item, Mobile from)
        {
            // Scriptiz : pour empêcher de monter sur un bateau (teleport et recall déjà coupés)
            if (item is Plank && from.AccessLevel == AccessLevel.Player)
            {
                from.SendMessage("Il serait plus sage de ne pas faire cela.");
                return false;
            }

            return base.CheckAccessibility(item, from);
        }

        public override bool OnBeginSpellCast(Mobile m, ISpell s)
        {
            if ((s is GateTravelSpell || s is RecallSpell || s is MarkSpell || s is SacredJourneySpell || s is TeleportSpell || s is Shadowjump) && m.AccessLevel == AccessLevel.Player)
            {
                m.SendMessage("You cannot cast that spell here.");
                return false;
            }

            // Pour limiter blade spirit à certains endroits
            if (s is BladeSpirits)
            {
                if (this.Name == "Ilot vaseux")
                {
                    m.SendMessage("Les lames s'enfonceraient dans la vase, ce serait bête de jeter ce sort ici.");
                    return false;
                }
            }

            return base.OnBeginSpellCast(m, s);
        }

        public override bool OnSkillUse(Mobile from, int Skill)
        {
            if (from.AccessLevel == AccessLevel.Player && Skill == (int)SkillName.Chivalry)
            {
                from.SendMessage("Une ombre passe au dessus de vous et absorbe la force magique que vous essayez d'invoquer");
                return false; ;
            }
            return true;
        }
    }
}