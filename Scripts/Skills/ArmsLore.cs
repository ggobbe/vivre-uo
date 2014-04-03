using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.SkillHandlers
{
    public class ArmsLore
    {
        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.ArmsLore].Callback = new SkillUseCallback(OnUse);
        }

        public static TimeSpan OnUse(Mobile m)
        {
            m.Target = new InternalTarget();

            m.SendMessage("Quel objet souhaitez-vous faire évaluer?"); // What item do you wish to get information about?

            return TimeSpan.FromSeconds(1.0);
        }

        [PlayerVendorTarget]
        private class InternalTarget : Target
        {
            public InternalTarget()
                : base(2, false, TargetFlags.None)
            {
                AllowNonlocal = true;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseWeapon || targeted is BaseArmor || targeted is BaseJewel)
                {
                    if (from.CheckTargetSkill(SkillName.ArmsLore, targeted, 0, 100))
                    {
                        string texttype = "Inconnu", textquality = "Inconnue", textresource = "Inconnue", textdurability = "Inconnue";
                        
                        if (targeted is BaseWeapon)
                        {
                            BaseWeapon arm = (BaseWeapon)targeted;

                            double durability = 0;

                            if (arm.MaxHitPoints != 0)
                                durability = arm.HitPoints / (double)arm.MaxHitPoints;

                            switch (arm.Type)
                            {
                                case WeaponType.Axe: texttype = "Hache"; break;
                                case WeaponType.Bashing: texttype = "Arme contondante"; break;
                                case WeaponType.Fists: texttype = "Arme de pugilat"; break;
                                case WeaponType.Piercing: texttype = "Arme d'estoc"; break;
                                case WeaponType.Polearm: texttype = "Arme à longue portée"; break;
                                case WeaponType.Ranged: texttype = "Arme de jet"; break;
                                case WeaponType.Slashing: texttype = "Arme à tranchant"; break;
                                case WeaponType.Staff: texttype = "Baton"; break;
                                default: texttype = "Arme"; break;
                            }

                            if (from.Skills[SkillName.ArmsLore].Value > 20)
                            {
                                if (arm.Quality == WeaponQuality.Low)
                                    textquality = "Mauvaise";
                                else if (arm.Quality == WeaponQuality.Exceptional)
                                    textquality = "Excellente";
                                else
                                    textquality = "Commune";
                            }

                            if (from.Skills[SkillName.Mining].Value > 30 && arm.Resource != CraftResource.None)
                                textresource = string.Format("{0}", CraftResources.GetName(arm.Resource));

                            if (from.Skills[SkillName.ArmsLore].Value >= 50 && from.Skills[SkillName.ArmsLore].Value <= 75)
                            {
                                if (durability < 0.1 || arm.MaxHitPoints < 10)
                                    textdurability = "Fissurée et menace de se briser!";
                                else if (durability < 0.3)
                                    textdurability = "Signes importants de faiblesse";
                                else if (durability < 0.6)
                                    textdurability = "Endommagée";
                                else if (durability < 0.85)
                                    textdurability = "Quelques dégats";
                                else if (durability < 1)
                                    textdurability = "Presque neuve";
                                else
                                    textdurability = "Aucun défaut";
                            }
                            else if (from.Skills[SkillName.ArmsLore].Value > 75 && from.Skills[SkillName.ArmsLore].Value <= 95)
                                textdurability = string.Format("Endommagée à {0}%", (100 - Math.Round(durability * 100.0)));
                            else if (from.Skills[SkillName.ArmsLore].Value > 95)
                                textdurability = string.Format("{0} sur {1}", arm.HitPoints, arm.MaxHitPoints);
                            else if (durability < 0.15)
                                textdurability = string.Format("Cette arme tombe en ruine");
                                

                            from.SendMessage("Type : {0}", texttype);
                            from.SendMessage("Resource : {0}", textresource);
                            from.SendMessage("Qualité : {0}", textquality);
                            from.SendMessage("Durabilité : {0}", textdurability);/*, textquality, textresource, textdurability*/
                        }

                        else if (targeted is BaseArmor)
                        {
                            BaseArmor arm = (BaseArmor)targeted;

                            double durability = 0;

                            if (arm.MaxHitPoints != 0)
                                durability = arm.HitPoints / (double)arm.MaxHitPoints;

                            switch (arm.MaterialType)
                            {
                                case ArmorMaterialType.Barbed : 
                                case ArmorMaterialType.Horned :
                                case ArmorMaterialType.Spined :
                                case ArmorMaterialType.Studded :
                                case ArmorMaterialType.Daemon: texttype = "Armure de cuir de demon"; break;
                                case ArmorMaterialType.Leather: texttype = "Armure de cuir"; break;
                                case ArmorMaterialType.Bone: texttype = "Armure d'os"; break;
                                case ArmorMaterialType.Chainmail: texttype = "Armure de chaine"; break;
                                case ArmorMaterialType.Cloth: texttype = "Armure de tissus"; break;
                                case ArmorMaterialType.Dragon: texttype = "Armure d'écaille"; break;
                                case ArmorMaterialType.Ringmail: texttype = "Armure d'anneau"; break;
                                case ArmorMaterialType.Plate: texttype = "Armure de plaque"; break;
                                default: texttype= "Armure"; break;
                            }

                            if (from.Skills[SkillName.ArmsLore].Value > 20)
                            {
                                if (arm.Quality == ArmorQuality.Low)
                                    textquality = "Mauvaise";
                                else if (arm.Quality == ArmorQuality.Exceptional)
                                    textquality = "Excellente";
                                else
                                    textquality = "Commune";
                            }

                            if (arm.MaterialType != ArmorMaterialType.Bone && from.Skills[SkillName.Mining].Value > 30 && arm.Resource != CraftResource.None)
                                textresource = string.Format(" en {0}.", CraftResources.GetName(arm.Resource));

                            if (from.Skills[SkillName.ArmsLore].Value >= 50 && from.Skills[SkillName.ArmsLore].Value <= 75)
                            {
                                if (durability < 0.1 || arm.MaxHitPoints < 10)
                                    textdurability = "Fissurée et menace de se briser!";
                                else if (durability < 0.3)
                                    textdurability = "Signes importants de faiblesse";
                                else if (durability < 0.6)
                                    textdurability = "Endommagée";
                                else if (durability < 0.85)
                                    textdurability = "Quelques dégats";
                                else if (durability < 1)
                                    textdurability = "Presque neuve";
                                else
                                    textdurability = "Aucun défaut";
                            }
                            else if (from.Skills[SkillName.ArmsLore].Value > 75 && from.Skills[SkillName.ArmsLore].Value <= 95)
                                textdurability = string.Format("Endommagée à {0}%", (100 - Math.Round(durability * 100.0)));
                             else if (from.Skills[SkillName.ArmsLore].Value > 95)
                                textdurability = string.Format("{0} sur {1}", arm.HitPoints, arm.MaxHitPoints);
                            else if (durability < 0.15)
                                textdurability = string.Format("Cette arme tombe en ruine");

                            from.SendMessage("Type : {0}", texttype);
                            from.SendMessage("Resource : {0}", textresource);
                            from.SendMessage("Qualité : {0}", textquality);
                            from.SendMessage("Durabilité : {0}", textdurability);
                        }
                        else if (targeted is BaseJewel)
                        {
                            BaseJewel arm = (BaseJewel)targeted;

                            switch (arm.Layer)
                            {
                                case Layer.Earrings: texttype= "Boucles d'oreilles"; break;
                                case Layer.Neck: texttype = "Pendentif"; break;
                                case Layer.Bracelet: texttype = "Bracelet"; break;
                                default: texttype = "Bijou"; break;
                            }

                            if (from.Skills[SkillName.Mining].Value > 30 && arm.Resource != CraftResource.None)
                                textresource = string.Format("{0}.", CraftResources.GetName(arm.Resource));

                            from.SendMessage("Type : {0}", texttype);
                            from.SendMessage("Resource : {0}", textresource);
                        }
                        else
                        {
                            from.SendMessage("Ceci n'est ni une arme, ni une armure, ni un bijou"); // This is neither weapon nor armor.
                        }
                    }
                    else
                    {
                        from.SendMessage("Vous êtes incertain..."); // You are not certain...
                    }
                }
                else
                {
                    from.SendMessage("Ceci n'est ni une arme, ni une armure, ni un bijou"); // This is neither weapon nor armor.
                }
            }

        }
    }
}


    
    