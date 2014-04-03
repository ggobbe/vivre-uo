/***************************************************************************
 *                          VivreNecromancerSpell.cs
 *                          ------------------------
 *   begin                : July 25, 2010
 *   copyright            : (C) Scriptiz
 *   email                : maeliguul@hotmail.com
 *   version              : 2010-07-25
 *
 ***************************************************************************/
using System;
using Server;
using Server.Items;

namespace Server.Spells.VivreNecromancy
{
    public abstract class VivreNecromancerSpell : Spell
    {
        public abstract double RequiredSkill { get; }
        public abstract int RequiredMana { get; }

        public override SkillName CastSkill { get { return SkillName.Necromancy; } }
        public override SkillName DamageSkill { get { return SkillName.EvalInt; } }

        //public override int CastDelayBase{ get{ return base.CastDelayBase; } } // Reference, 3

        public override bool ClearHandsOnCast { get { return false; } }

        public override double CastDelayFastScalar { get { return (Core.SE ? base.CastDelayFastScalar : 0); } } // Necromancer spells are not affected by fast cast items, though they are by fast cast recovery

        public VivreNecromancerSpell(Mobile caster, Item scroll, SpellInfo info)
            : base(caster, scroll, info)
        {
        }

        public override int ComputeKarmaAward()
        {
            //TODO: Verify this formula being that Necro spells don't HAVE a circle.

            //return -(70 + (10 * (int)Circle));

            return -(40 + (int)(10 * (CastDelayBase.TotalSeconds / CastDelaySecondsPerTick)));
        }

        public override void GetCastSkills(out double min, out double max)
        {
            min = RequiredSkill;
            max = Scroll != null ? min : RequiredSkill + 25.0;
        }

        public override bool ConsumeReagents()
        {
            if (base.ConsumeReagents())
                return true;

            if (ArcaneGem.ConsumeCharges(Caster, 1))
                return true;

            return false;
        }

        public override int GetMana()
        {
            return RequiredMana;
        }

        public bool CheckCast(Mobile m)
        {
            if (m == null) return false;

            // Scriptiz : On vérifie que le joueur porte bien sa robe de nécromant
            Item necroRobe = m.FindItemOnLayer(Layer.OuterTorso);

            if (necroRobe == null || !(necroRobe is NecroRobe))
            {
                Caster.SendMessage("Vous devez porter des habits particuliers pour pouvoir incanter ce sort.");
                return false;
            }

            return base.CheckCast();
        }

        public override bool CheckCast()
        {
            if(CheckCast(Caster)) return base.CheckCast();
            return false;
        }
    }
}