/***************************************************************************
 *                          MinorLifeLeech.cs
 *                          ---------------
 *   begin                : August 29 2010
 *   copyright            : (C) Scriptiz
 *   email                : maeliguul@hotmail.com
 *   version              : 2010-09-24
 *
 ***************************************************************************/
using System;
using System.Collections.Generic;
using Server.Network;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;

namespace Server.Spells.VivreNecromancy
{
    public class MinorLifeLeech : VivreNecromancerSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Drain de vie mineur", "An Mani Vas Corp",
                269,
                9031,
                false,
                Reagent.DragonBlood,
                Reagent.DaemonBone
            );

        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(1.5); } }

        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 40; } }

        public MinorLifeLeech(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Mana -= RequiredMana;
            Caster.Target = new InternalTarget(this);
        }

        public void Target(object obj)
        {
            if (CheckSequence() && CheckCast())
            {
                Mobile target = null;
                if (obj is Mobile) target = (Mobile)obj;

                if (target == null)
                {
                    Caster.SendMessage("Cette cible ne peut pas subir cet effet.");
                    return;
                }

                SpellHelper.Turn(Caster, target);
                target.MovingParticles(Caster, 0x36D4, 7, 0, false, false, 33, 0, 9502, 1, 0, 0x100);
                Caster.PlaySound(Core.AOS ? 0x15E : 0x44B);

                int leech = (int)(6 * (Caster.Skills.EvalInt.Base / 25));
                int lifeToHeal = target.Hits;
                target.Damage(leech, Caster);
                lifeToHeal -= target.Hits;

                if (Caster.Hits == Caster.HitsMax)
                {
                    Caster.Damage(leech * 2);
                    Caster.SendMessage("L'afflux de sang vous rend malade.");
                }
                else if(lifeToHeal > 0) Caster.Heal(lifeToHeal);
            }
            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private MinorLifeLeech m_Owner;

            public InternalTarget(MinorLifeLeech owner)
                : base(Core.ML ? 10 : 12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                m_Owner.Target(o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}