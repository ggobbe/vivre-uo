using System;
using Server;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Items
{
   
    public class ItemIdentification
    {
        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.ItemID].Callback = new SkillUseCallback(OnUse);
        }

        public static TimeSpan OnUse(Mobile from)
        {
            from.SendLocalizedMessage(500343); // What do you wish to appraise and identify?
            from.Target = new InternalTarget();

            return TimeSpan.FromSeconds(1.0);
        }

        

        [PlayerVendorTarget]
        private class InternalTarget : Target
        {
            public InternalTarget()
                : base(8, false, TargetFlags.None)
            {
                AllowNonlocal = true;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Item)
                {
                    if (from.CheckTargetSkill(SkillName.ItemID, o, 0, 100))
                    {
                       
                        #region BaseWeapon
                        if (o is BaseWeapon && !((BaseWeapon)o).Identified)
                        {
                            BaseWeapon idarme = (BaseWeapon)o;

                            int difficulty = idarme.WeaponDifficulty;

                            if ((from.Skills[SkillName.ItemID].Value - (difficulty * 10)) <= 5)
                             {
                                from.SendMessage("Cette arme d�gage un pouvoir trop grand pour vos maigres moyens");
                                return;
                            }
                            else if ((from.Skills[SkillName.ItemID].Value - (difficulty * 10)) >= Utility.Random(difficulty * 10))
                            {
                                from.SendMessage("Vous parvenez � d�cerner les capacit�s de l'arme");
                                idarme.Identified = true;
                            }
                            else
                            {
                                int consequence = Utility.Random(from.RawInt);

                                if (consequence < difficulty)
                                {
                                    from.SendMessage("Dans votre tentative d'identifier l'arme, vous l'avez bris�e");
                                    idarme.Delete();
                                }
                                else if (consequence < difficulty * 10)
                                {
                                    from.SendMessage("Vous n'avez rien d�cel�, mais avez abim� l'arme");
                                    idarme.MaxHitPoints -= 1;
                                }
                                else
                                    from.SendMessage("Vous n'avez rien d�cel�");
                            }
                        }
                        #endregion
                        #region BaseArmor
                        else if (o is BaseArmor && !((BaseArmor)o).Identified)
                        {
                            BaseArmor idarmure = (BaseArmor)o;

                            int difficulty = idarmure.ArmorDifficulty;

                            if ((from.Skills[SkillName.ItemID].Value - (difficulty * 10)) <= 5)
                            {
                                from.SendMessage("Cette armure d�gage un pouvoir trop grand pour vos maigres moyens");
                                return;
                            }
                            else if ((from.Skills[SkillName.ItemID].Value - (difficulty * 10)) >= Utility.Random(difficulty * 10))
                            {
                                from.SendMessage("Vous parvenez � d�cerner les capacit�s de l'armure");
                                idarmure.Identified = true;
                            }
                            else
                            {
                                int consequence = Utility.Random(from.RawInt);

                                if (consequence < difficulty)
                                {
                                    from.SendMessage("Dans votre tentative d'identifier l'armure, vous l'avez bris�e");
                                    idarmure.Delete();
                                }
                                else if (consequence < difficulty * 10)
                                {
                                    from.SendMessage("Vous n'avez rien d�cel�, mais avez abim� l'armure");
                                    idarmure.MaxHitPoints -= 1;
                                }
                                else
                                    from.SendMessage("Vous n'avez rien d�cel�");
                            }
                        }
                        #endregion
                        #region BaseClothing
                        else if (o is BaseClothing && !((BaseClothing)o).Identified)
                        {
                            BaseClothing idclothing = (BaseClothing)o;

                            int difficulty = idclothing.ClothingDifficulty;

                            if ((from.Skills[SkillName.ItemID].Value - (difficulty * 10)) <= 5)
                            {
                                from.SendMessage("Ce v�tement d�gage un pouvoir trop grand pour vos maigres moyens");
                                return;
                            }
                            else if ((from.Skills[SkillName.ItemID].Value - (difficulty * 10)) >= Utility.Random(difficulty * 10))
                            {
                                from.SendMessage("Vous parvenez � d�cerner les capacit�s du v�tement");
                                idclothing.Identified = true;
                            }
                            else
                            {
                                int consequence = Utility.Random(from.RawInt);

                                if (consequence < difficulty)
                                {
                                    from.SendMessage("Dans votre tentative d'identifier le v�tement, vous l'avez bris�");
                                    idclothing.Delete();
                                }
                                else if (consequence < difficulty * 10)
                                {
                                    from.SendMessage("Vous n'avez rien d�cel�, mais avez abim� le v�tement");
                                    idclothing.MaxHitPoints -= 1;
                                }
                                else
                                    from.SendMessage("Vous n'avez rien d�cel�");
                            }
                        }
                        #endregion
                        #region BaseJewel
                        else if (o is BaseJewel && !((BaseJewel)o).Identified)
                        {
                            BaseJewel idjewel = (BaseJewel)o;

                            int difficulty = idjewel.JewelDifficulty;

                            if ((from.Skills[SkillName.ItemID].Value - (difficulty * 10)) <= 5)
                            {
                                from.SendMessage("Ce bijou d�gage un pouvoir trop grand pour vos maigres moyens");
                                return;
                            }
                            else if ((from.Skills[SkillName.ItemID].Value - (difficulty * 10)) >= Utility.Random(difficulty * 10))
                            {
                                from.SendMessage("Vous parvenez � d�cerner les capacit�s du bijou");
                                idjewel.Identified = true;
                            }
                            else
                            {
                                int consequence = Utility.Random(from.RawInt);

                                if (consequence < difficulty*2)
                                {
                                    from.SendMessage("Dans votre tentative d'identifier le bijou, vous l'avez bris�");
                                    idjewel.Delete();
                                } 
                                else
                                    from.SendMessage("Vous n'avez rien d�cel�");
                            }
                        }
                        else
                        {
                            from.SendMessage("Cet objet est d�j� identifi�");
                        }
                        #endregion

                        if (!Core.AOS)
                            ((Item)o).OnSingleClick(from);
                    }
                    else
                    {
                        from.SendMessage("Vous ne parvenez pas � vous concentrer.");
                    }

                }
                else if (o is Mobile)
                {
                    ((Mobile)o).OnSingleClick(from);
                }
                else
                {
                    from.SendMessage("Vous ne pouvez rien apprendre de cela");
                }

            }
        }
    }
}