using System;
using System.Collections.Generic;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;
using Server.Misc;
using Server.Regions;
using Server.SkillHandlers;
using Server.ContextMenus;
using Server.Spells;

namespace Server.Mobiles
{
    public class VivreGuardAI : BaseAI
    {
        public VivreGuardAI(BaseCreature m)
            : base(m)
        {
        }

        private bool isGuard(Mobile from)
        {
            if (from.FindItemOnLayer(Layer.Cloak) is GuardCloak) return true;

            return false;
        }

        private bool isEnnemi(Mobile from)
        {
            if (isGuard(from)) return false;

            if (from.Criminal == true) return true;

            if (from is BaseCreature) {
                BaseCreature mobfrom = (BaseCreature)from;

                if (mobfrom.AI == AIType.AI_Archer || mobfrom.AI == AIType.AI_Berserk || mobfrom.AI == AIType.AI_Melee || mobfrom.AI == AIType.AI_Ninja || mobfrom.AI == AIType.AI_OrcScout || mobfrom.AI == AIType.AI_Predator || mobfrom.AI == AIType.AI_Mage)
                    return !mobfrom.Controlled;
            }
            return false;
        }

        public override bool DoActionWander()
        {
            m_Mobile.DebugSay("I have no combatant");
            m_Mobile.Criminal = false;

            // Scriptiz : on dégomme les criminal
            foreach (Mobile m in m_Mobile.GetMobilesInRange(m_Mobile.RangePerception))
            {
                if (m == null) continue;
                if (m == m_Mobile) continue;
                if(!(m is PlayerMobile)) continue;

                if (isGuard(m) && m.Combatant != null && m.Combatant.Alive && !isGuard(m.Combatant) && !m.Combatant.Hidden)
                {
                    m_Mobile.Combatant = m.Combatant;
                    m.Criminal = false;
                }
                else if (m.Criminal && m.Alive && isEnnemi(m) && !m.Hidden)
                {
                    m_Mobile.Combatant = m;
                }

                // Scriptiz : criminalité descendue dans les villes, on réduit les gardes (pas de teleport)
                //if (m_Mobile.Combatant != null)
                //{
                //    Mobile crim = m_Mobile.Combatant;

                //    // Si trop proche ça ne sert à rien
                //    if (m_Mobile.GetDistanceToSqrt(crim.Location) < 10) continue;

                //    // Sinon on téléporte le garde
                //    IPoint3D p = crim.Location as IPoint3D;

                //    SpellHelper.GetSurfaceTop(ref p);

                //    Point3D fromLoc = m_Mobile.Location;
                //    Point3D toLoc = new Point3D(p);

                //    m_Mobile.Location = toLoc;
                //    m_Mobile.ProcessDelta();

                //    if (!crim.Hidden)
                //        crim.Hidden = false;

                //    Effects.SendLocationParticles(EffectItem.Create(fromLoc, m_Mobile.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
                //    Effects.SendLocationParticles(EffectItem.Create(toLoc, crim.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);

                //    m.PlaySound(0x1FE);
                //}
            }

            if (AcquireFocusMob(m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true))
            {
                m_Mobile.DebugSay("I see {0}", m_Mobile.FocusMob.Name);
                if (isEnnemi((Mobile)m_Mobile.FocusMob))
                {
                    m_Mobile.DebugSay("I have detected {0}, attacking", m_Mobile.FocusMob.Name);
                    m_Mobile.Combatant = m_Mobile.FocusMob;
                    Action = ActionType.Combat;
                }
                else
                    base.DoActionWander();
            }
            else
                base.DoActionWander();

            return true;
        }

        public override bool DoActionCombat()
        {
            Mobile combatant = m_Mobile.Combatant;

            if (combatant == null || combatant.Deleted || combatant.Map != m_Mobile.Map)
            {
                m_Mobile.DebugSay("My combatant is gone, so my guard is up");
                Action = ActionType.Wander;
                return true;
            }

            if (WalkMobileRange(combatant, 1, true, m_Mobile.RangeFight, m_Mobile.RangeFight))
            {
                m_Mobile.Direction = m_Mobile.GetDirectionTo(combatant);
            }
            else
            {
                if (m_Mobile.GetDistanceToSqrt(combatant) > m_Mobile.RangePerception + 1)
                {
                    m_Mobile.DebugSay("I cannot find {0} him", combatant.Name);
                    Action = ActionType.Wander;
                    return true;
                }
                else
                {
                    m_Mobile.DebugSay("I should be closer to {0}", combatant.Name);
                }
            }

            return true;
        }

        public override bool DoActionBackoff()
        {
            if (m_Mobile.IsHurt() || m_Mobile.Combatant != null)
            {
                Action = ActionType.Combat;
            }
            else
            {
                if (AcquireFocusMob(m_Mobile.RangePerception * 2, FightMode.Closest, true, false, true))
                {
                    if (WalkMobileRange(m_Mobile.FocusMob, 1, false, m_Mobile.RangePerception, m_Mobile.RangePerception * 2))
                    {
                        m_Mobile.DebugSay("Well, here I am safe");
                        Action = ActionType.Wander;
                    }
                }
                else
                {
                    m_Mobile.DebugSay("I have lost my focus, lets relax");
                    Action = ActionType.Wander;
                }
            }

            return true;
        }
    }
}
