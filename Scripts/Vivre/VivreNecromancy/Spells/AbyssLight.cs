/***************************************************************************
 *                          AbyssLight.cs
 *                          ---------------
 *   begin                : August 29 2010
 *   copyright            : (C) Scriptiz
 *   email                : maeliguul@hotmail.com
 *   version              : 2010-09-19
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
    public class AbyssLight : VivreNecromancerSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Lumière des abysses", "Kal Lor",
                269,
                9031,
                false,
                Reagent.BatWing,
                Reagent.EyeOfNewt
            );

        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(0.5); } }

        public override double RequiredSkill { get { return 10.0; } }
        public override int RequiredMana { get { return 4; } }

        public AbyssLight(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                LightSource light = new LightSource();
                light.Light = LightType.Circle300;
                light.Layer = Layer.Unused_xF;
                light.Movable = false;

                if (Caster.FindItemOnLayer(Layer.Unused_xF) != null)
                    Caster.FindItemOnLayer(Layer.Unused_xF).Delete();

                Caster.EquipItem(light);

                new InternalTimer(Caster);
                
                Caster.Karma -= 500;
                Caster.Mana -= RequiredMana;
                Effects.PlaySound(Caster.Location, Caster.Map, 0x54);
            }

            FinishSequence();
        }

        private class InternalTimer : Timer
        {
            private Mobile Caster;

            public InternalTimer(Mobile caster)
                : base(TimeSpan.FromMinutes(5))
            {
                Caster = caster;

                Delay = TimeSpan.FromSeconds((Caster.Skills.Necromancy.Base + Caster.Skills.EvalInt.Base) * 3);
                this.Start();
            }

            protected override void OnTick()
            {
                Item light = Caster.FindItemOnLayer(Layer.Unused_xF);
                if (light != null && light is LightSource)
                {
                    light.Delete();
                    Effects.PlaySound(Caster.Location, Caster.Map, 0x4BB);
                }
                this.Stop();
            }
        }
    }
}