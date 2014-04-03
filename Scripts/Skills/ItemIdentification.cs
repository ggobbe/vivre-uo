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
                                from.SendMessage("Cette arme dégage un pouvoir trop grand pour vos maigres moyens");
                                return;
                            }
                            else if ((from.Skills[SkillName.ItemID].Value - (difficulty * 10)) >= Utility.Random(difficulty * 10))
                            {
                                from.SendMessage("Vous parvenez à décerner les capacités de l'arme");
                                idarme.Identified = true;
                            }
                            else
                            {
                                int consequence = Utility.Random(from.RawInt);

                                if (consequence < difficulty)
                                {
                                    from.SendMessage("Dans votre tentative d'identifier l'arme, vous l'avez brisée");
                                    idarme.Delete();
                                }
                                else if (consequence < difficulty * 10)
                                {
                                    from.SendMessage("Vous n'avez rien décelé, mais avez abimé l'arme");
                                    idarme.MaxHitPoints -= 1;
                                }
                                else
                                    from.SendMessage("Vous n'avez rien décelé");
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
                                from.SendMessage("Cette armure dégage un pouvoir trop grand pour vos maigres moyens");
                                return;
                            }
                            else if ((from.Skills[SkillName.ItemID].Value - (difficulty * 10)) >= Utility.Random(difficulty * 10))
                            {
                                from.SendMessage("Vous parvenez à décerner les capacités de l'armure");
                                idarmure.Identified = true;
                            }
                            else
                            {
                                int consequence = Utility.Random(from.RawInt);

                                if (consequence < difficulty)
                                {
                                    from.SendMessage("Dans votre tentative d'identifier l'armure, vous l'avez brisée");
                                    idarmure.Delete();
                                }
                                else if (consequence < difficulty * 10)
                                {
                                    from.SendMessage("Vous n'avez rien décelé, mais avez abimé l'armure");
                                    idarmure.MaxHitPoints -= 1;
                                }
                                else
                                    from.SendMessage("Vous n'avez rien décelé");
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
                                from.SendMessage("Ce vêtement dégage un pouvoir trop grand pour vos maigres moyens");
                                return;
                            }
                            else if ((from.Skills[SkillName.ItemID].Value - (difficulty * 10)) >= Utility.Random(difficulty * 10))
                            {
                                from.SendMessage("Vous parvenez à décerner les capacités du vêtement");
                                idclothing.Identified = true;
                            }
                            else
                            {
                                int consequence = Utility.Random(from.RawInt);

                                if (consequence < difficulty)
                                {
                                    from.SendMessage("Dans votre tentative d'identifier le vêtement, vous l'avez brisé");
                                    idclothing.Delete();
                                }
                                else if (consequence < difficulty * 10)
                                {
                                    from.SendMessage("Vous n'avez rien décelé, mais avez abimé le vêtement");
                                    idclothing.MaxHitPoints -= 1;
                                }
                                else
                                    from.SendMessage("Vous n'avez rien décelé");
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
                                from.SendMessage("Ce bijou dégage un pouvoir trop grand pour vos maigres moyens");
                                return;
                            }
                            else if ((from.Skills[SkillName.ItemID].Value - (difficulty * 10)) >= Utility.Random(difficulty * 10))
                            {
                                from.SendMessage("Vous parvenez à décerner les capacités du bijou");
                                idjewel.Identified = true;
                            }
                            else
                            {
                                int consequence = Utility.Random(from.RawInt);

                                if (consequence < difficulty*2)
                                {
                                    from.SendMessage("Dans votre tentative d'identifier le bijou, vous l'avez brisé");
                                    idjewel.Delete();
                                } 
                                else
                                    from.SendMessage("Vous n'avez rien décelé");
                            }
                        }
                        else
                        {
                            from.SendMessage("Cet objet est déjà identifié");
                        }
                        #endregion

                        if (!Core.AOS)
                            ((Item)o).OnSingleClick(from);
                    }
                    else
                    {
                        from.SendMessage("Vous ne parvenez pas à vous concentrer.");
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