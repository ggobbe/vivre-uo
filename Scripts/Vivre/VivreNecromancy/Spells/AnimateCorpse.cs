/***************************************************************************
 *                          AnimateDead.cs
 *                          ---------------
 *   begin                : August 26, 2010
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
    public class AnimateCorpse : VivreNecromancerSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Libérer la mort", "Ex Corp",
                269,
                9031,
                false,
                Reagent.BloodVial,
                Reagent.ExecutionersCap
            );

        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(2.5); } }    // TODO : Cast doublé?

        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 14; } } // TODO : mana doublée?

        public AnimateCorpse(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Mana -= RequiredMana;
            Caster.Target = new InternalTarget(this);
            Caster.SendMessage("Quel cadavre souhaitez vous animer?");
        }

        public void Target(object obj)
        {
            if (CheckSequence())
            {
                Corpse c = obj as Corpse;

                if (c == null)
                {
                    Caster.SendLocalizedMessage(1061084); // You cannot animate that.
                }
                else
                {
                    SpellHelper.Turn(Caster, c);
                    Type type = null;

                    if (c.Owner != null)
                        type = c.Owner.GetType();

                    if (c.ItemID != 0x2006 || c.Channeled || type == typeof(PlayerMobile) || type == null || (c.Owner != null && c.Owner.Fame < 100) || ((c.Owner != null) && (c.Owner is BaseCreature) && (((BaseCreature)c.Owner).Summoned || ((BaseCreature)c.Owner).IsBonded)))
                    {
                        Caster.SendLocalizedMessage(1061085); // There's not enough life force there to animate.
                    }
                    else
                    {
                        object[] paramObject = new object[] { };
                        object summoned = Activator.CreateInstance(type, paramObject);

                        if (summoned is BaseCreature)
                        {
                            BaseCreature bc = (BaseCreature)summoned;

                            bc.Tamable = false;

                            if (bc is BaseMount)
                                bc.ControlSlots = 1;
                            else
                                bc.ControlSlots = 0;

                            Effects.PlaySound(c.Location, c.Map, 0x1FB);
                            //Effects.PlaySound(loc, map, bc.GetAngerSound());
                            Effects.SendLocationParticles(EffectItem.Create(c.Location, c.Map, EffectItem.DefaultDuration), 0x3789, 1, 40, 0x3F, 3, 9907, 0);

                            // On s'occupe du corps
                            c.Items.Clear();
                            c.TurnToBones();

                            TimeSpan delay = TimeSpan.FromMinutes((Caster.Skills.Necromancy.Base + Caster.Skills.EvalInt.Base) * 3);
                            BaseCreature.Summon(bc, false, Caster, c.Location, 0x28, delay);
                        }
                    }
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private AnimateCorpse m_Owner;

            public InternalTarget(AnimateCorpse owner)
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