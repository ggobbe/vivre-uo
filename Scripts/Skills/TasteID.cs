using System;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.SkillHandlers
{
	public class TasteID
	{
		public static void Initialize()
		{
			SkillInfo.Table[(int)SkillName.TasteID].Callback = new SkillUseCallback( OnUse );
		}

		public static TimeSpan OnUse( Mobile m )
		{
			m.Target = new InternalTarget();

			m.SendMessage( "Que souhaitez-vous goûter?" ); // What would you like to taste?

			return TimeSpan.FromSeconds( 1.0 );
		}

		[PlayerVendorTarget]
		private class InternalTarget : Target
		{
			public InternalTarget() :  base ( 2, false, TargetFlags.None )
			{
				AllowNonlocal = true;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is Mobile )
				{
					from.SendMessage( "Une telle action serait inappropriée..." ); // You feel that such an action would be inappropriate.
                    return;
                }
                if (targeted is CookableCheese)
                {
                    Food food = (Food)targeted;

                    if (from.CheckTargetSkill(SkillName.TasteID, food, 0, 100))
                    {
                        if (food.Poison != null)
                        {
                            from.SendMessage("Des effluves de poison parviennent à votre nez"); // It appears to have poison smeared on it.
                        }
                        else
                        {
                            // No poison on the food
                            from.SendMessage("Vous ne décelez rien d'anormal dans ce que vous goûtez"); // You detect nothing unusual about this substance.
                        }
                    }
                    else
                    {
                        // Skill check failed
                        from.SendMessage("Vous ne discernez pas correctement les aromes"); // You cannot discern anything about this substance.
                    }
                    return;
                }

                if ( targeted is Food )
				{
					Food food = (Food) targeted;

					if ( from.CheckTargetSkill( SkillName.TasteID, food, 0, 100 ) )
					{
						if ( food.Poison != null )
						{
							from.SendMessage( "Des effluves de poison parviennent à votre nez" ); // It appears to have poison smeared on it.
						}
						else
						{
							// No poison on the food
                            from.SendMessage( "Vous ne décelez rien d'anormal dans ce que vous goûtez"); // You detect nothing unusual about this substance.
						}
					}
					else
					{
						// Skill check failed
						from.SendMessage( "Vous ne discernez pas correctement les aromes" ); // You cannot discern anything about this substance.
					}
                    return;
				}
                if (targeted is BaseBeverage)
                {
                    BaseBeverage beverage = (BaseBeverage)targeted;

                    if (from.CheckTargetSkill(SkillName.TasteID, beverage, 0, 100))
                    {
                        if (beverage.Poison != null)
                        {
                            from.SendMessage("Des effluves de poison parviennent à votre nez"); // It appears to have poison smeared on it.
                        }
                        else
                        {
                            // No poison on the food
                            from.SendMessage("Vous ne décelez rien d'anormal dans ce que vous goûtez"); // You detect nothing unusual about this substance.
                        }
                    }
                    else
                    {
                        // Skill check failed
                        from.SendMessage("Vous ne discernez pas correctement les aromes"); // You cannot discern anything about this substance.
                    }
                    return;
                }
                if (targeted is GenderPotion)
                {
                    GenderPotion potion = (GenderPotion)targeted;

                    if (from.Skills[SkillName.TasteID].Value >= 75)
                    {
                        if (potion.Female)
                            from.SendMessage("Cette fiole sent les fleurs");
                        else
                            from.SendMessage("Cette fiole sent la sueur");
                    }
                    else
                        from.SendMessage("Votre nez n'est pas encore suffisamment développé pour en dénoter la subtile effluve");
                    return;
                }
				if ( targeted is BasePotion )
				{
					BasePotion potion = (BasePotion) targeted;

					potion.SendLocalizedMessageTo( from, 502813 ); // You already know what kind of potion that is.
					potion.SendLocalizedMessageTo( from, potion.LabelNumber );
                    return;
				}
				if ( targeted is PotionKeg )
				{
					PotionKeg keg = (PotionKeg) targeted;

					if ( keg.Held <= 0 )
					{
						keg.SendLocalizedMessageTo( from, 502228 ); // There is nothing in the keg to taste!
					}
					else
					{
						keg.SendLocalizedMessageTo( from, 502229 ); // You are already familiar with this keg's contents.
						keg.SendLocalizedMessageTo( from, keg.LabelNumber );
					}
                    return;
				}
				// The target is not food or potion or potion keg.
					from.SendLocalizedMessage( 502820 ); // That's not something you can taste.
				
			}

			protected override void OnTargetOutOfRange( Mobile from, object targeted )
			{
				from.SendLocalizedMessage( 502815 ); // You are too far away to taste that.
			}
		}
	}
}