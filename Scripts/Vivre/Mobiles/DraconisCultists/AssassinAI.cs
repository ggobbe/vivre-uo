using System;
using System.Collections;
using Server.Targeting;
using Server.Network;

//
// Vinds : Expérimentations pour une ai type rodeur/assassin. 
// Avouons le, peut mieux faire.
//

namespace Server.Mobiles
{
    public class AssassinAI : BaseAI
    {
        public AssassinAI(BaseCreature m)
            : base(m)
        {
        }


        public override bool DoActionWander()
        {
            if (!(m_Mobile.Hidden))
            {
                m_Mobile.UseSkill(SkillName.Hiding);
            }


            if (m_Mobile.Hidden)
            {
                m_Mobile.UseSkill(SkillName.Stealth);
            }
                                         
            if (AcquireFocusMob(m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true))
            {
                if (m_Mobile.Debug)
                    m_Mobile.DebugSay("I have detected {0}, attacking", m_Mobile.FocusMob.Name);

                m_Mobile.Combatant = m_Mobile.FocusMob;

                if (!(m_Mobile.Hidden))
                {
                    m_Mobile.UseSkill(SkillName.Hiding);
                }

                  m_Mobile.UseSkill(SkillName.Stealth);
                  Action = ActionType.Combat;
            }
                      
            else
            {                
             base.DoActionWander();
            }
           
            return true;
        }

      

        public override bool DoActionCombat()
        {
            Mobile combatant = m_Mobile.Combatant;

            if (combatant == null || combatant.Deleted || combatant.Map != m_Mobile.Map || !combatant.Alive || combatant.IsDeadBondedPet)
            {
                m_Mobile.DebugSay("My combatant is gone, so my guard is up");

                if (!(m_Mobile.Hidden))
                {
                    m_Mobile.DebugSay("Il n'y a pas d'enemis en vue donc j'essaie de me cacher");

                    m_Mobile.UseSkill(SkillName.Hiding);

                }

                Action = ActionType.Guard;
                return true;
            }

            if (!m_Mobile.InRange(combatant, m_Mobile.RangePerception))
            {
                // They are somewhat far away, can we find something else?
                if (!(m_Mobile.Hidden))
                {                    
                    m_Mobile.UseSkill(SkillName.Hiding);
                }
                if (AcquireFocusMob(m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true))
                {
                    if (!(m_Mobile.Hidden))
                    {
                    
                        m_Mobile.UseSkill(SkillName.Hiding);
                    }


                    m_Mobile.Combatant = m_Mobile.FocusMob;
                    m_Mobile.FocusMob = null;
                }
                else if (!m_Mobile.InRange(combatant, m_Mobile.RangePerception * 3))
                {
                    m_Mobile.Combatant = null;
                }

                combatant = m_Mobile.Combatant;

                if (combatant == null)
                {
                    

                    if (!(m_Mobile.Hidden))
                    {
                        m_Mobile.DebugSay("l'énemi a fui, je me cache ");

                        m_Mobile.UseSkill(SkillName.Hiding);
                    }

                    else if (m_Mobile.Hidden)
                    {

                        m_Mobile.UseSkill(SkillName.Stealth);
                    }

                    Action = ActionType.Guard;

                    return true;
                }
            }

            /*if ( !m_Mobile.InLOS( combatant ) )
            {
                if ( AcquireFocusMob( m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true ) )
                {
                    m_Mobile.Combatant = combatant = m_Mobile.FocusMob;
                    m_Mobile.FocusMob = null;
                }
            }*/

            if (MoveTo(combatant, true, m_Mobile.RangeFight))
            {
                if (m_Mobile.Hidden)
                {
                    m_Mobile.UseSkill(SkillName.Stealth);

                }


                m_Mobile.Direction = m_Mobile.GetDirectionTo(combatant);//
                m_Mobile.Direction = m_Mobile.GetDirectionTo(combatant) | Direction.Running;
            }
            else if (AcquireFocusMob(m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true))
            {
                if (m_Mobile.Hidden)
                {
                    m_Mobile.UseSkill(SkillName.Stealth);

                }
                if (m_Mobile.Debug)
                    m_Mobile.DebugSay("My move is blocked, so I am going to attack {0}", m_Mobile.FocusMob.Name);

                m_Mobile.Combatant = m_Mobile.FocusMob;
                Action = ActionType.Combat;

                return true;
            }
            else if (m_Mobile.GetDistanceToSqrt(combatant) > m_Mobile.RangePerception + 1)
            {
                

                if (!(m_Mobile.Hidden))
                {
                    m_Mobile.DebugSay("j'ai perdu l'énemi de vue donc j'essaie de me cacher");

                    m_Mobile.UseSkill(SkillName.Hiding);
                }


                Action = ActionType.Guard;

                return true;
            }
            else
            {
                if (m_Mobile.Debug)
                    m_Mobile.DebugSay("I should be closer to {0}", combatant.Name);
            }

            if (!m_Mobile.Controlled && !m_Mobile.Summoned && m_Mobile.CanFlee)
            {
                if (m_Mobile.Hits < m_Mobile.HitsMax * 20 / 100)
                {
                    // We are low on health, should we flee?

                    bool flee = false;

                    if (m_Mobile.Hits < combatant.Hits)
                    {
                        // We are more hurt than them

                        int diff = combatant.Hits - m_Mobile.Hits;

                        flee = (Utility.Random(0, 100) < (10 + diff)); // (10 + diff)% chance to flee
                    }
                    else
                    {
                        flee = Utility.Random(0, 100) < 20; // 20% chance to flee
                    }

                    if (flee)
                    {
                        if (m_Mobile.Debug)
                            m_Mobile.DebugSay("I am going to flee from {0}", combatant.Name);

                        if ((m_Mobile.Hidden))
                        {
                            m_Mobile.DebugSay("je veux fuire tout en restant invisible");

                            m_Mobile.UseSkill(SkillName.Stealth);
                        }

                        Action = ActionType.Flee;
                    }
                }
            }

            return true;
        }

        public override bool DoActionGuard()
        {
            if (!(m_Mobile.Hidden))
            {
                m_Mobile.DebugSay("l'énemi a fui, je me cache ");

                m_Mobile.UseSkill(SkillName.Hiding);
            }


            if (m_Mobile.Hidden)
            {
                m_Mobile.UseSkill(SkillName.Stealth);

            }

          

            if (AcquireFocusMob(m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true))
            {
                if (m_Mobile.Debug)
                    m_Mobile.DebugSay("I have detected {0}, attacking", m_Mobile.FocusMob.Name);

                m_Mobile.Combatant = m_Mobile.FocusMob;
                Action = ActionType.Combat;
            }
            else
            {
                base.DoActionGuard();
            }

            return true;
        }

        
         

        public override bool DoActionFlee()
        {
            if (m_Mobile.Hits > m_Mobile.HitsMax / 2)
            {
                m_Mobile.DebugSay("I am stronger now, so I will continue fighting");
                Action = ActionType.Combat;
            }
            else
            {
                if ((m_Mobile.Mana >= 20) && !(m_Mobile.Hidden) && (Utility.Random(100) > 49 ) )
                {
                    m_Mobile.Emote("*Avale une potion*");
                    m_Mobile.Hits += 30;
                    m_Mobile.Hidden = true;
                  
                    m_Mobile.UseSkill(SkillName.Stealth);
                }

                m_Mobile.FocusMob = m_Mobile.Combatant;
                base.DoActionBackoff();
            }

            return true;
        }

    }
}