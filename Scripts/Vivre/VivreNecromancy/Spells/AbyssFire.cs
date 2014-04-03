/***************************************************************************
 *                          AbyssFire.cs
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
    public class AbyssFire : VivreNecromancerSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Feu des abysses", "Vas Flam Por",
                269,
                9031,
                false,
                Reagent.Bloodspawn,
                Reagent.Brimstone
            );

        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(1.5); } }

        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 9; } }

        public AbyssFire(Mobile caster, Item scroll)
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
            if (CheckSequence())
            {
                Mobile target = null;
                if (obj is Mobile) target = (Mobile)obj;

                if (target == null)
                {
                    Caster.SendMessage("Cette cible ne peut pas subir cet effet.");
                    return;
                }

                SpellHelper.Turn(Caster, target);
                int damage = (int)(8 * (Caster.Skills.EvalInt.Base / 25));
                target.Damage(damage, Caster);
                Caster.MovingParticles(target, 0x36D4, 7, 0, false, true, 9502, 4019, 0x160);
                Caster.PlaySound(Core.AOS ? 0x15E : 0x44B);
            }
            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private AbyssFire m_Owner;

            public InternalTarget(AbyssFire owner)
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