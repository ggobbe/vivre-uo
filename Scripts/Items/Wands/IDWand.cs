using System;
using Server;
using Server.Targeting;

namespace Server.Items
{
    public class IDWand : BaseWand
    {
        public override TimeSpan GetUseDelay { get { return TimeSpan.Zero; } }

        [Constructable]
        public IDWand()
            : base(WandEffect.Identification, 25, 50)
        {
            Name = "Baguette d'identification";
        }

        public IDWand(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override bool OnWandTarget(Mobile from, object o)
        {
            int difficulty = 0;
                    #region BaseWeapon
                    if (o is BaseWeapon && !((BaseWeapon)o).Identified)
                    {
                        BaseWeapon idarme = (BaseWeapon)o;

                        if (idarme.Attributes.AttackChance != 0)
                            difficulty++;
                        if (idarme.Attributes.BonusDex != 0)
                            difficulty++;
                        if (idarme.Attributes.BonusHits != 0)
                            difficulty++;
                        if (idarme.Attributes.BonusInt != 0)
                            difficulty++;
                        if (idarme.Attributes.BonusMana != 0)
                            difficulty++;
                        if (idarme.Attributes.BonusStam != 0)
                            difficulty++;
                        if (idarme.Attributes.BonusStr != 0)
                            difficulty++;
                        if (idarme.Attributes.CastRecovery != 0)
                            difficulty++;
                        if (idarme.Attributes.CastSpeed != 0)
                            difficulty++;
                        if (idarme.Attributes.DefendChance != 0)
                            difficulty++;
                        if (idarme.Attributes.EnhancePotions != 0)
                            difficulty++;
                        if (idarme.Attributes.LowerManaCost != 0)
                            difficulty++;
                        if (idarme.Attributes.LowerRegCost != 0)
                            difficulty++;
                        if (idarme.Attributes.Luck != 0)
                            difficulty++;
                        if (idarme.Attributes.NightSight != 0)
                            difficulty++;
                        if (idarme.Attributes.ReflectPhysical != 0)
                            difficulty++;
                        if (idarme.Attributes.RegenHits != 0)
                            difficulty++;
                        if (idarme.Attributes.RegenMana != 0)
                            difficulty++;
                        if (idarme.Attributes.RegenStam != 0)
                            difficulty++;
                        if (idarme.Attributes.SpellChanneling != 0)
                            difficulty++;
                        if (idarme.Attributes.SpellDamage != 0)
                            difficulty++;
                        if (idarme.Attributes.WeaponDamage != 0)
                            difficulty++;
                        if (idarme.Attributes.WeaponSpeed != 0)
                            difficulty++;
                        if (idarme.WeaponAttributes.DurabilityBonus != 0)
                            difficulty++;
                        if (idarme.WeaponAttributes.HitColdArea != 0)
                            difficulty++;
                        if (idarme.WeaponAttributes.HitDispel != 0)
                            difficulty++;
                        if (idarme.WeaponAttributes.HitEnergyArea != 0)
                            difficulty++;
                        if (idarme.WeaponAttributes.HitFireArea != 0)
                            difficulty++;
                        if (idarme.WeaponAttributes.HitFireball != 0)
                            difficulty++;
                        if (idarme.WeaponAttributes.HitHarm != 0)
                            difficulty++;
                        if (idarme.WeaponAttributes.HitLeechHits != 0)
                            difficulty++;
                        if (idarme.WeaponAttributes.HitLeechMana != 0)
                            difficulty++;
                        if (idarme.WeaponAttributes.HitLeechStam != 0)
                            difficulty++;
                        if (idarme.WeaponAttributes.HitLightning != 0)
                            difficulty++;
                        if (idarme.WeaponAttributes.HitLowerAttack != 0)
                            difficulty++;
                        if (idarme.WeaponAttributes.HitLowerDefend != 0)
                            difficulty++;
                        if (idarme.WeaponAttributes.HitMagicArrow != 0)
                            difficulty++;
                        if (idarme.WeaponAttributes.HitPhysicalArea != 0)
                            difficulty++;
                        if (idarme.WeaponAttributes.HitPoisonArea != 0)
                            difficulty++;
                        if (idarme.WeaponAttributes.LowerStatReq != 0)
                            difficulty++;
                        if (idarme.WeaponAttributes.MageWeapon != 0)
                            difficulty++;
                        if (idarme.WeaponAttributes.ResistColdBonus != 0)
                            difficulty++;
                        if (idarme.WeaponAttributes.ResistEnergyBonus != 0)
                            difficulty++;
                        if (idarme.WeaponAttributes.ResistFireBonus != 0)
                            difficulty++;
                        if (idarme.WeaponAttributes.ResistPhysicalBonus != 0)
                            difficulty++;
                        if (idarme.WeaponAttributes.ResistPoisonBonus != 0)
                            difficulty++;
                        if (idarme.WeaponAttributes.SelfRepair != 0)
                            difficulty++;
                        if (idarme.WeaponAttributes.UseBestSkill != 0)
                            difficulty++;
                        if (idarme.Slayer != SlayerName.None)
                            difficulty++;
                        if (idarme.Slayer2 != SlayerName.None )
                            difficulty++;
                        if (idarme.SkillBonuses.Skill_1_Value != 0)
                            difficulty++;
                        if (idarme.SkillBonuses.Skill_2_Value != 0)
                            difficulty++;
                        if (idarme.SkillBonuses.Skill_3_Value != 0)
                            difficulty++;
                        if (idarme.SkillBonuses.Skill_4_Value != 0)
                            difficulty++;
                        if (idarme.SkillBonuses.Skill_5_Value != 0)
                            difficulty++;

                        difficulty -= 2;

                        if (difficulty > 5)
                            difficulty = 5;

                        if (difficulty < 4)
                        {
                            from.SendMessage("Vous parvenez à décerner les capacités de l'arme");
                            idarme.Identified = true;
                        }
                        else
                        {
                            from.SendMessage("Cette arme renferme trop de magie pour être ainsi identifiée");
                        }
                    }
                    #endregion
                    #region BaseArmor
                    else if (o is BaseArmor && !((BaseArmor)o).Identified)
                    {
                        BaseArmor idarmure = (BaseArmor)o;

                        if (idarmure.Attributes.AttackChance != 0)
                            difficulty++;
                        if (idarmure.Attributes.BonusDex != 0)
                            difficulty++;
                        if (idarmure.Attributes.BonusHits != 0)
                            difficulty++;
                        if (idarmure.Attributes.BonusInt != 0)
                            difficulty++;
                        if (idarmure.Attributes.BonusMana != 0)
                            difficulty++;
                        if (idarmure.Attributes.BonusStam != 0)
                            difficulty++;
                        if (idarmure.Attributes.BonusStr != 0)
                            difficulty++;
                        if (idarmure.Attributes.CastRecovery != 0)
                            difficulty++;
                        if (idarmure.Attributes.CastSpeed != 0)
                            difficulty++;
                        if (idarmure.Attributes.DefendChance != 0)
                            difficulty++;
                        if (idarmure.Attributes.EnhancePotions != 0)
                            difficulty++;
                        if (idarmure.Attributes.LowerManaCost != 0)
                            difficulty++;
                        if (idarmure.Attributes.LowerRegCost != 0)
                            difficulty++;
                        if (idarmure.Attributes.Luck != 0)
                            difficulty++;
                        if (idarmure.Attributes.NightSight != 0)
                            difficulty++;
                        if (idarmure.Attributes.ReflectPhysical != 0)
                            difficulty++;
                        if (idarmure.Attributes.RegenHits != 0)
                            difficulty++;
                        if (idarmure.Attributes.RegenMana != 0)
                            difficulty++;
                        if (idarmure.Attributes.RegenStam != 0)
                            difficulty++;
                        if (idarmure.Attributes.SpellChanneling != 0)
                            difficulty++;
                        if (idarmure.Attributes.SpellDamage != 0)
                            difficulty++;
                        if (idarmure.Attributes.WeaponDamage != 0)
                            difficulty++;
                        if (idarmure.Attributes.WeaponSpeed != 0)
                            difficulty++;
                        if (idarmure.ArmorAttributes.DurabilityBonus != 0)
                            difficulty++;
                        if (idarmure.ArmorAttributes.LowerStatReq != 0)
                            difficulty++;
                        if (idarmure.ArmorAttributes.MageArmor != 0)
                            difficulty++;
                        if (idarmure.SkillBonuses.Skill_1_Value != 0)
                            difficulty++;
                        if (idarmure.SkillBonuses.Skill_2_Value != 0)
                            difficulty++;
                        if (idarmure.SkillBonuses.Skill_3_Value != 0)
                            difficulty++;
                        if (idarmure.SkillBonuses.Skill_4_Value != 0)
                            difficulty++;
                        if (idarmure.SkillBonuses.Skill_5_Value != 0)
                            difficulty++;
                        if (idarmure.ColdBonus != 0)
                            difficulty++;
                        if (idarmure.PhysicalBonus != 0)
                            difficulty++;
                        if (idarmure.FireBonus != 0)
                            difficulty++;
                        if (idarmure.PoisonBonus != 0)
                            difficulty++;
                        if (idarmure.EnergyBonus != 0)
                            difficulty++;

                        if (difficulty > 5)
                            difficulty = 5;

                        if (difficulty <= 3)
                        {
                            from.SendMessage("Vous parvenez à décerner les capacités de l'armure");
                            idarmure.Identified = true;
                        }
                        else
                        {
                            from.SendMessage("Cette armure renferme trop de magie pour être ainsi identifiée");
                        }
                    }
                    #endregion
                    #region BaseClothing
                    else if (o is BaseClothing && !((BaseClothing)o).Identified)
                    {
                        BaseClothing idclothing = (BaseClothing)o;

                        if (idclothing.Attributes.AttackChance != 0)
                            difficulty++;
                        if (idclothing.Attributes.BonusDex != 0)
                            difficulty++;
                        if (idclothing.Attributes.BonusHits != 0)
                            difficulty++;
                        if (idclothing.Attributes.BonusInt != 0)
                            difficulty++;
                        if (idclothing.Attributes.BonusMana != 0)
                            difficulty++;
                        if (idclothing.Attributes.BonusStam != 0)
                            difficulty++;
                        if (idclothing.Attributes.BonusStr != 0)
                            difficulty++;
                        if (idclothing.Attributes.CastRecovery != 0)
                            difficulty++;
                        if (idclothing.Attributes.CastSpeed != 0)
                            difficulty++;
                        if (idclothing.Attributes.DefendChance != 0)
                            difficulty++;
                        if (idclothing.Attributes.EnhancePotions != 0)
                            difficulty++;
                        if (idclothing.Attributes.LowerManaCost != 0)
                            difficulty++;
                        if (idclothing.Attributes.LowerRegCost != 0)
                            difficulty++;
                        if (idclothing.Attributes.Luck != 0)
                            difficulty++;
                        if (idclothing.Attributes.NightSight != 0)
                            difficulty++;
                        if (idclothing.Attributes.ReflectPhysical != 0)
                            difficulty++;
                        if (idclothing.Attributes.RegenHits != 0)
                            difficulty++;
                        if (idclothing.Attributes.RegenMana != 0)
                            difficulty++;
                        if (idclothing.Attributes.RegenStam != 0)
                            difficulty++;
                        if (idclothing.Attributes.SpellChanneling != 0)
                            difficulty++;
                        if (idclothing.Attributes.SpellDamage != 0)
                            difficulty++;
                        if (idclothing.Attributes.WeaponDamage != 0)
                            difficulty++;
                        if (idclothing.Attributes.WeaponSpeed != 0)
                            difficulty++;
                        if (idclothing.ClothingAttributes.DurabilityBonus != 0)
                            difficulty++;
                        if (idclothing.ClothingAttributes.LowerStatReq != 0)
                            difficulty++;
                        if (idclothing.ClothingAttributes.MageArmor != 0)
                            difficulty++;
                        if (idclothing.ClothingAttributes.SelfRepair != 0)
                            difficulty++;
                        if (idclothing.SkillBonuses.Skill_1_Value != 0)
                            difficulty++;
                        if (idclothing.SkillBonuses.Skill_2_Value != 0)
                            difficulty++;
                        if (idclothing.SkillBonuses.Skill_3_Value != 0)
                            difficulty++;
                        if (idclothing.SkillBonuses.Skill_4_Value != 0)
                            difficulty++;
                        if (idclothing.SkillBonuses.Skill_5_Value != 0)
                            difficulty++;
                        if (!(idclothing is BaseHat))
                        {
                        if (idclothing.Resistances.Chaos != 0)
                            difficulty++;
                        if (idclothing.Resistances.Cold != 0)
                            difficulty++;
                        if (idclothing.Resistances.Direct != 0)
                            difficulty++;
                        if (idclothing.Resistances.Energy != 0)
                            difficulty++;
                        if (idclothing.Resistances.Fire != 0)
                            difficulty++;
                        if (idclothing.Resistances.Physical != 0)
                            difficulty++;
                        if (idclothing.Resistances.Poison != 0)
                            difficulty++;
                        }

                        if (difficulty > 5)
                            difficulty = 5;

                        if (difficulty <= 3)
                        {
                            from.SendMessage("Vous parvenez à décerner les capacités du vêtement");
                            idclothing.Identified = true;
                        }
                        else
                        {
                            from.SendMessage("Ce vêtement renferme trop de magie pour être ainsi identifiée");
                        }
                    }
                    #endregion
                    #region BaseJewel
                    else if (o is BaseJewel && !((BaseJewel)o).Identified)
                    {
                        BaseJewel idjewel = (BaseJewel)o;

                        if (idjewel.Attributes.AttackChance != 0)
                            difficulty++;
                        if (idjewel.Attributes.BonusDex != 0)
                            difficulty++;
                        if (idjewel.Attributes.BonusHits != 0)
                            difficulty++;
                        if (idjewel.Attributes.BonusInt != 0)
                            difficulty++;
                        if (idjewel.Attributes.BonusMana != 0)
                            difficulty++;
                        if (idjewel.Attributes.BonusStam != 0)
                            difficulty++;
                        if (idjewel.Attributes.BonusStr != 0)
                            difficulty++;
                        if (idjewel.Attributes.CastRecovery != 0)
                            difficulty++;
                        if (idjewel.Attributes.CastSpeed != 0)
                            difficulty++;
                        if (idjewel.Attributes.DefendChance != 0)
                            difficulty++;
                        if (idjewel.Attributes.EnhancePotions != 0)
                            difficulty++;
                        if (idjewel.Attributes.LowerManaCost != 0)
                            difficulty++;
                        if (idjewel.Attributes.LowerRegCost != 0)
                            difficulty++;
                        if (idjewel.Attributes.Luck != 0)
                            difficulty++;
                        if (idjewel.Attributes.NightSight != 0)
                            difficulty++;
                        if (idjewel.Attributes.ReflectPhysical != 0)
                            difficulty++;
                        if (idjewel.Attributes.RegenHits != 0)
                            difficulty++;
                        if (idjewel.Attributes.RegenMana != 0)
                            difficulty++;
                        if (idjewel.Attributes.RegenStam != 0)
                            difficulty++;
                        if (idjewel.Attributes.SpellChanneling != 0)
                            difficulty++;
                        if (idjewel.Attributes.SpellDamage != 0)
                            difficulty++;
                        if (idjewel.Attributes.WeaponDamage != 0)
                            difficulty++;
                        if (idjewel.Attributes.WeaponSpeed != 0)
                            difficulty++;
                        if (idjewel.Resistances.Chaos != 0)
                            difficulty++;
                        if (idjewel.Resistances.Cold != 0)
                            difficulty++;
                        if (idjewel.Resistances.Direct != 0)
                            difficulty++;
                        if (idjewel.Resistances.Energy != 0)
                            difficulty++;
                        if (idjewel.Resistances.Fire != 0)
                            difficulty++;
                        if (idjewel.Resistances.Physical != 0)
                            difficulty++;
                        if (idjewel.Resistances.Poison != 0)
                            difficulty++;
                        if (idjewel.SkillBonuses.Skill_1_Value != 0)
                            difficulty++;
                        if (idjewel.SkillBonuses.Skill_2_Value != 0)
                            difficulty++;
                        if (idjewel.SkillBonuses.Skill_3_Value != 0)
                            difficulty++;
                        if (idjewel.SkillBonuses.Skill_4_Value != 0)
                            difficulty++;
                        if (idjewel.SkillBonuses.Skill_5_Value != 0)
                            difficulty++;

                        if (difficulty > 5)
                            difficulty = 5;

                        if (difficulty <= 3)
                        {
                            from.SendMessage("Vous parvenez à décerner les capacités du bijou");
                            idjewel.Identified = true;
                        }
                        else
                        {
                            from.SendMessage("Ce bjou renferme trop de magie pour être ainsi identifiée");
                        }
                    }
                    else
                    {
                        from.SendMessage("Cet objet est déjà identifié");
                    }
                    #endregion

            if (!Core.AOS && o is Item)
                ((Item)o).OnSingleClick(from);

            return (o is Item);
        }
    }
}
    

            
   
		
	