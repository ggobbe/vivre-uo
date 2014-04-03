/***************************************************************************
 *                               BurnCorpse.cs
 *                            -------------------
 *   begin                : September 15, 2011
 *   copyright            : (C) Scriptiz
 *   email                : maeliguul@hotmail.com
 *   version              : 2011-09-15
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
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Targeting;

namespace Server.Commands
{
    public class BurnCorpse
    {
        public static void Initialize()
        {
            CommandSystem.Register("bruler", AccessLevel.Player, new CommandEventHandler(Burn_OnCommand));
        }

        [Usage("bruler")]
        [Description("Permet de cibler un corps afin de brûler celui-ci.")]
        public static void Burn_OnCommand(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(5, false, TargetFlags.None, new TargetCallback(Burn_OnTarget));
        }

        public static void Burn_OnTarget(Mobile from, object target)
        {
            if (from == null) return;

            if (!(target is Corpse))
            {
                from.SendMessage("Vous devez cibler un corps.");
                return;
            }

            Corpse c = target as Corpse;

            if (c.Owner is PlayerMobile)
            {
                from.SendMessage("Vous ne pouvez pas mettre le feu à ce corps.");
                return;
            }

            if (!from.InRange(c.Location, 3))
            {
                from.SendMessage("Le corps est trop loin pour que vous y mettiez le feu.");
                return;
            }

            Effects.PlaySound(c.Location, c.Map, 0x208);
            Effects.SendLocationParticles(EffectItem.Create(c.Location, c.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052);

            Timer.DelayCall(TimeSpan.FromSeconds(1), c.Delete);
        }
    }
}