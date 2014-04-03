using System;
using Server;
using Server.Targeting;
using Server.Items;
using Server.Engines.Harvest;
using Server.Mobiles;
using Server.Engines.Quests;
using Server.Engines.Quests.Hag;
using Server.Regions;

namespace Server.Targets
{
	public class BladedItemTarget : Target
	{
		private Item m_Item;

		public BladedItemTarget( Item item ) : base( 2, false, TargetFlags.None )
		{
			m_Item = item;
		}

		protected override void OnTargetOutOfRange( Mobile from, object targeted )
		{
			if ( targeted is UnholyBone && from.InRange( ((UnholyBone)targeted), 12 ) )
				((UnholyBone)targeted).Carve( from, m_Item );
			else
				base.OnTargetOutOfRange (from, targeted);
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			if ( m_Item.Deleted )
				return;

			if ( targeted is ICarvable )
			{
				((ICarvable)targeted).Carve( from, m_Item );
			}
			else if ( targeted is SwampDragon && ((SwampDragon)targeted).HasBarding )
			{
				SwampDragon pet = (SwampDragon)targeted;

				if ( !pet.Controlled || pet.ControlMaster != from )
					from.SendLocalizedMessage( 1053022 ); // You cannot remove barding from a swamp dragon you do not own.
				else
					pet.HasBarding = false;
			}
            else if (targeted is Head)
            {
                Head targ = (Head)targeted;

                if (from.Karma > - 1500)
                {
                    from.SendMessage("Vous n'avez pas le profil d'un dépeceur de crâne...");
                    return;
                }

                if (from.Dex <= Utility.Random(110))
                {
                    from.SendMessage("Vous avez été trop maladroit et avez raté le dépeçage");
                    targ.Delete();
                    return;
                }

                from.SendMessage("Vous achevez d'enlever la chair du crâne.");
                from.AddToBackpack(new Skull());
                targ.Consume();
            }
            else if (targeted is Pumpkin)
            {
                Pumpkin targ = (Pumpkin)targeted;

                if(from.Dex <= Utility.Random(100))
                {
                    from.SendMessage("Vous avez été trop maladroit et avez raté votre tracé");
                    targ.Consume();
                    return;
                }

                int karma= 0;

                if (from.Karma > 100)
                    karma ++;
                else if (from.Karma < -100)
                    karma --;
                
                int chance = Utility.Random(4) + karma;

                if (chance >=2)
                    from.AddToBackpack(new SmileyPumpkin());
                else
                    from.AddToBackpack(new EvilPumpkin());

                from.SendMessage("Vous taillez la citrouille selon votre humeur");
                targ.Consume();
            }
			else
			{
				if ( targeted is StaticTarget )
				{
					int itemID = ((StaticTarget)targeted).ItemID;

					if ( itemID == 0xD15 || itemID == 0xD16 ) // red mushroom
					{
						PlayerMobile player = from as PlayerMobile;

						if ( player != null )
						{
							QuestSystem qs = player.Quest;

							if ( qs is WitchApprenticeQuest )
							{
								FindIngredientObjective obj = qs.FindObjective( typeof( FindIngredientObjective ) ) as FindIngredientObjective;

								if ( obj != null && !obj.Completed && obj.Ingredient == Ingredient.RedMushrooms )
								{
									player.SendLocalizedMessage( 1055036 ); // You slice a red cap mushroom from its stem.
									obj.Complete();
									return;
								}
							}
						}
					}
				}

				HarvestSystem system = Lumberjacking.System;
				HarvestDefinition def = Lumberjacking.System.Definition;

				int tileID;
				Map map;
				Point3D loc;

				if ( !system.GetHarvestDetails( from, m_Item, targeted, out tileID, out map, out loc ) )
				{
					from.SendLocalizedMessage( 500494 ); // You can't use a bladed item on that!
				}
				else if ( !def.Validate( tileID ) )
				{
					from.SendLocalizedMessage( 500494 ); // You can't use a bladed item on that!
				}
				else
				{
					HarvestBank bank = def.GetBank( map, loc.X, loc.Y );

					if ( bank == null )
						return;

					if ( bank.Current < 5 )
					{
						from.SendLocalizedMessage( 500493 ); // There's not enough wood here to harvest.
					}
					else
					{
						bank.Consume( 5, from );

                        if (map.Season == (int)ServerSeasons.Season.Spring && Utility.RandomDouble() < 0.33)
                        {
                            from.PrivateOverheadMessage(Network.MessageType.Regular, 0x3B2, false, "De la sève se met à couler du tronc, souhaitez-vous la recueillir?", from.NetState);
                            from.BeginTarget(2, false, TargetFlags.None, new TargetCallback(OnSelectTarget));
                            return;
                        }

						Item item = new Kindling();

						if ( from.PlaceInBackpack( item ) )
						{
							from.SendLocalizedMessage( 500491 ); // You put some kindling into your backpack.
							from.SendLocalizedMessage( 500492 ); // An axe would probably get you more wood.
						}
						else
						{
							from.SendLocalizedMessage( 500490 ); // You can't place any kindling into your backpack!

							item.Delete();
						}
					}
				}
			}
		}
        public void OnSelectTarget(Mobile from, object obj)
        {
            if (!(obj is ResourceBucket))
            {
                from.SendMessage("Vous devriez utiliser un seau adéquat");
                return;
            }
            
            ResourceBucket bucket = (ResourceBucket)obj;

            if(bucket.MilkType != Milk.None)
            {
                from.SendMessage("Cela ferait un trop curieux mélange");
                return;
            }

            if (bucket.Quantity >= bucket.MaxQuantity)
            {
                from.SendMessage("Votre seau est plein");
                return;
            }

            bucket.ResourceType = BucketLiquid.SugarWater;
            bucket.Quantity +=Utility.Random(1,5);

            return;
        }
	}
}