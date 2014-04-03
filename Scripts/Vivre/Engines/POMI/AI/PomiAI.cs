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
using Server.IPOMI;
using Server.ContextMenus;

namespace Server.Mobiles 
{ 
   public class PomiAI: BaseAI 
   { 
      public PomiAI(BaseCreature m) : base (m) 
      { 
      }
      
      private bool isEnnemi(PlayerMobile from, TownStone town)
      {    	
      	PomiCloak pomicloak = from.FindItemOnLayer(Layer.Cloak) as PomiCloak;
      	if(pomicloak != null && pomicloak.Name == "Ambassadeur")
				return false;

        foreach (TownStone ville in town.Guerre)
            if (ville.isCitoyen(from))
                return true;

      	return false;
      }

      public override bool DoActionWander()
      {
          // Scriptiz : les gardes ne s'attaquent pas entre eux !
          if (m_Mobile.Combatant != null && m_Mobile.Combatant is PomiGuard)
          {
              m_Mobile.Combatant.Criminal = false;
              m_Mobile.Criminal = false;
              m_Mobile.Combatant.Combatant = null;
              m_Mobile.Combatant = null;
          }

          TownStone m_Town = ((PomiGuard)(m_Mobile)).Town;
          m_Mobile.DebugSay("I have no combatant");

          m_Mobile.Criminal = false;

          if (AcquireFocusMob(m_Mobile.RangePerception, m_Mobile.FightMode, true, false, true))
          {
              m_Mobile.DebugSay("I see {0}", m_Mobile.FocusMob.Name);
              if (m_Town.HLL.Contains((PlayerMobile)m_Mobile.FocusMob) ||
                  isEnnemi((PlayerMobile)m_Mobile.FocusMob, m_Town) ||
                  (m_Mobile.FocusMob.Criminal &&
                   !m_Town.isMaire((PlayerMobile)m_Mobile.FocusMob) &&
                   !m_Town.isConseiller((PlayerMobile)m_Mobile.FocusMob) &&
                   !m_Town.isAmbassadeur((PlayerMobile)m_Mobile.FocusMob) &&
                   !m_Town.isCapitaine((PlayerMobile)m_Mobile.FocusMob) &&
                   !m_Town.Gardes.Contains((PlayerMobile)m_Mobile.FocusMob)))
              {
                  m_Mobile.DebugSay("I have detected {0}, attacking", m_Mobile.FocusMob.Name);
                  m_Mobile.Combatant = m_Mobile.FocusMob;
                  Action = ActionType.Combat;
              }
              else
              {
                  base.DoActionWander();
              }
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
