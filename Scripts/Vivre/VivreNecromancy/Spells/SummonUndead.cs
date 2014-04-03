/***************************************************************************
 *                          SummonUndead.cs
 *                          ---------------
 *   begin                : July 25, 2010
 *   copyright            : (C) Scriptiz
 *   email                : maeliguul@hotmail.com
 *   version              : 2010-08-30
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
    public class SummonUndead : VivreNecromancerSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Invocation des morts", "Kal An Mani",
                269,
                9031,
                false,
                Reagent.GraveDust,  // orbisdian et blackmoor
                Reagent.DaemonBlood
            );

        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(6.0); } }

        public override double RequiredSkill { get { return 40.0; } }
        public override int RequiredMana { get { return 30; } }

        public SummonUndead(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Mana -= RequiredMana;
            Caster.Target = new InternalTarget(this);
            Caster.SendMessage("Où voulez-vous invoquer un mort?"); // Animate what corpse?
        }

        public void Target(object obj)
        {
            if (CheckSequence())
            {
                // Scriptiz : Amélioration en ciblant un Mobile, le mort l'attaque
                Point3D location;

                if (obj is LandTarget)
                    location = ((LandTarget)obj).Location;
                else if(obj is Mobile)
                    location = ((Mobile)obj).Location;
                else
                {
                    Caster.SendMessage("Veuillez cibler une zone valide. (" + obj.GetType().ToString() + ")");
                    return;
                }

                SpellHelper.Turn(Caster, location);

                double getLich = Caster.Skills.Necromancy.Value / 10;
                double getSkeleton = Caster.Skills.Necromancy.Value / 5;

                int chance = Utility.Random(100);

                BaseCreature undead;

                if (chance <= getLich)
                    undead = new Lich();
                else if (chance <= getSkeleton)
                    undead = new Skeleton();
                else
                    undead = new Zombie();

                undead.ControlSlots = 1;
                undead.Fame = 0;
                undead.Karma = -1500;

                Caster.Karma -= 500;
                Effects.PlaySound(location, Caster.Map, 0x1FB);
                Effects.SendLocationParticles(EffectItem.Create(location, Caster.Map, EffectItem.DefaultDuration), 0x3789, 1, 40, 0x3F, 3, 9907, 0);

                TimeSpan delay = TimeSpan.FromMinutes((Caster.Skills.Necromancy.Base + Caster.Skills.EvalInt.Base) * 3);
                BaseCreature.Summon(undead, false, Caster, location, 0x28, delay);

                // Si la cible est une mobile on l'attaque
                if (obj is Mobile)
                {
                    undead.Attack((Mobile)obj);
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private SummonUndead m_Owner;

            public InternalTarget(SummonUndead owner)
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