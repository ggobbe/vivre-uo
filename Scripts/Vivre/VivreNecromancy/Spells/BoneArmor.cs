/***************************************************************************
 *                          BoneArmor.cs
 *                          ---------------
 *   begin                : August 29 2010
 *   copyright            : (C) Scriptiz
 *   email                : maeliguul@hotmail.com
 *   version              : 2010-08-29
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
    public class BoneArmor : VivreNecromancerSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Armure d'os", "In Sanct Ylem Corp",
                269,
                9031,
                false,
                Reagent.Bone,
                Reagent.FertileDirt
            );

        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(0.0); } }

        public override double RequiredSkill { get { return 40.0; } }
        public override int RequiredMana { get { return 30; } }

        public BoneArmor(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                List<Item> bonePieces = new List<Item>();
                List<Item> equipedPieces = new List<Item>();

                bonePieces.Add(new BoneArms());
                bonePieces.Add(new BoneChest());
                bonePieces.Add(new BoneGloves());
                bonePieces.Add(new BoneHelm());
                bonePieces.Add(new BoneLegs());

                foreach (Item i in bonePieces)
                {
                    // On retire les pièces d'armures déjà équipées et on les met dans le sac
                    Item equiped = Caster.FindItemOnLayer(i.Layer);

                    if (equiped != null && equiped.Movable)
                    {
                        equipedPieces.Add(equiped);
                        Caster.RemoveItem(equiped);
                        Caster.AddToBackpack(equiped);
                    }

                    if (equiped != null && !equiped.Movable)
                    {
                        Caster.SendMessage("Vous portez déjà des objets invoqués.");
                        return;
                    }

                    // On équipe la pièce d'armure
                    BaseArmor ba = null;
                    if(i is BaseArmor) ba = (BaseArmor)i;

                    if (ba != null)
                    {
                        ba.Movable = false;
                        ba.Hue = 1109;
                        ba.ArmorAttributes.LowerStatReq = 100;
                        Caster.EquipItem(ba);
                    }
                }

                new InternalTimer(Caster, bonePieces, equipedPieces);

                Caster.Karma -= 500;
                Caster.Mana -= RequiredMana;
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1FB);
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x3789, 1, 40, 0x3F, 3, 9907, 0);
            }

            FinishSequence();
        }

        private class InternalTimer : Timer
        {
            private Mobile Caster;
            private List<Item> BonePieces;
            private List<Item> EquipedPieces;

            public InternalTimer(Mobile caster, List<Item> bonePieces, List<Item> equipedPieces)
                : base(TimeSpan.FromMinutes(5))
            {
                Caster = caster;
                BonePieces = bonePieces;
                EquipedPieces = equipedPieces;

                Delay = TimeSpan.FromSeconds((Caster.Skills.Necromancy.Base + Caster.Skills.EvalInt.Base) * 3);
                this.Start();
            }

            protected override void OnTick()
            {
                foreach (Item i in BonePieces)
                {
                    if (i == null || i.Deleted) continue;

                    i.Delete();
                }

                foreach (Item i in EquipedPieces)
                {
                    if (i == null || i.Deleted) continue;

                    if (i.IsChildOf(Caster.Backpack))
                    {
                        Caster.EquipItem(i);
                    }
                    else
                    {
                        Caster.SendMessage("Un objet ne se trouve plus dans votre sac et n'a donc pas été rééquipé.");
                    }
                }
                this.Stop();
            }
        }
    }
}