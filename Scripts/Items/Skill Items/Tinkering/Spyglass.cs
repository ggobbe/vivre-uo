using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Quests;
using Server.Engines.Quests.Hag;
using Server.Multis;    // Scriptiz : système de track des bateaux
using Server.SkillHandlers; // Scriptiz : flèche pour traquer

namespace Server.Items
{
	[Flipable( 0x14F5, 0x14F6 )]
	public class Spyglass : Item
	{
		[Constructable]
		public Spyglass() : base( 0x14F5 )
		{
			Weight = 3.0;
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1008155 ); // You peer into the heavens, seeking the moons...

			from.Send( new MessageLocalizedAffix( from.Serial, from.Body, MessageType.Regular, 0x3B2, 3, 1008146 + (int)Clock.GetMoonPhase( Map.Trammel, from.X, from.Y ), "", AffixType.Prepend, "Trammel : ", "" ) );
			
            // Scriptiz : on ne joue pas sur felucca
            //from.Send( new MessageLocalizedAffix( from.Serial, from.Body, MessageType.Regular, 0x3B2, 3, 1008146 + (int)Clock.GetMoonPhase( Map.Felucca, from.X, from.Y ), "", AffixType.Prepend, "Felucca : ", "" ) );

			PlayerMobile player = from as PlayerMobile;

            /* Scriptiz : Code pour repérer les autres bateaux (source : Alambik) */
            // Get the maximum range the player can see
            // A cartograph master with a tracking master experience lead to see to 125 tiles: A true captain!
            int MinimumRange = 25; //Regular 800x600 screen + "normal" extra
            int MaximumExtraRange = 100;
            int ExtraRange = MaximumExtraRange *
                  ((int)(from.Skills[SkillName.Cartography].Value) +
                   (int)(from.Skills[SkillName.Tracking].Value)) / 200;
            int range = MinimumRange + ExtraRange;

            foreach (Item item in from.GetItemsInRange(range))
            {
                if (item is BaseBoat)
                {
                    // Player can see the boat
                    BaseBoat baseboat = (BaseBoat)item;
                    if (!(baseboat.Contains(from))) // On va éviter de répeter qu'on voit son propre bateau
                    {
                        /* Scriptiz : implémentation du tracking */
                        // If the player is good at tracking, let him track a mobile on the boat
                        if(from.Skills[SkillName.Tracking].Value * 2 >= from.GetDistanceToSqrt(baseboat.Location))
                        {
                            foreach (Mobile m in baseboat.GetMobilesInRange(15))
                            {
                                if (m == null) continue;
                                if (baseboat.Contains(m))
                                {
                                    from.QuestArrow = new TrackArrow(from, m, range);
                                    break;
                                }
                            }
                        }

                        // Get the name if not too far
                        string name = "un navire";
                        if (from.InRange(item.Location, MinimumRange + MaximumExtraRange / 5))
                            if (baseboat.ShipName != null)
                                name = "le" + baseboat.ShipName;

                        // Is it far?
                        string distance = "à l'horizon";
                        if (from.InRange(item.Location, MinimumRange + MaximumExtraRange * 1 / 5))
                            distance = "à côté";
                        else if (from.InRange(item.Location, MinimumRange + MaximumExtraRange * 2 / 5))
                            distance = "proche";
                        else if (from.InRange(item.Location, MinimumRange + MaximumExtraRange * 3 / 5))
                            distance = "loin";
                        else if (from.InRange(item.Location, MinimumRange + MaximumExtraRange * 4 / 5))
                            distance = "très loin";

                        // Get the relative direction of the seen boat
                        string direction;
                        // north/south
                        if (from.Y < baseboat.Y)
                            direction = "Sud";
                        else
                            direction = "Nord";
                        // east/west (Scriptiz : correction est <> ouest)
                        if (from.X < baseboat.X)
                            direction = direction + " Est";
                        else
                            direction = direction + " Ouest";

                        //Does the boat is moving?
                        string mobility = "est immobile";
                        if (baseboat.IsMoving)
                        {
                            mobility = "bouge vers ";
                            switch (baseboat.Moving)
                            {
                                case Direction.North: mobility += "le nord"; break;
                                case Direction.South: mobility += "le sud"; break;
                                case Direction.East: mobility += "l'est"; break;
                                case Direction.West: mobility += "l'ouest"; break;
                                case Direction.Up: mobility += "le nord-ouest"; break;
                                case Direction.Down: mobility += "le sud-est"; break;
                                case Direction.Left: mobility += "le sud-ouest"; break;
                                case Direction.Right: mobility += "le nord-est"; break;
                                default: break;
                            }
                        }
                        from.SendMessage("Vous voyez {0} au {1}. Il est {2} et {3}.", name, direction, distance, mobility);
                    }
                }
            }
            /* Scriptiz : fin du code de Alambik */

			if ( player != null )
			{
				QuestSystem qs = player.Quest;

				if ( qs is WitchApprenticeQuest )
				{
					FindIngredientObjective obj = qs.FindObjective( typeof( FindIngredientObjective ) ) as FindIngredientObjective;

					if ( obj != null && !obj.Completed && obj.Ingredient == Ingredient.StarChart )
					{
						int hours, minutes;
						Clock.GetTime( from.Map, from.X, from.Y, out hours, out minutes );

						if ( hours < 5 || hours > 17 )
						{
							player.SendLocalizedMessage( 1055040 ); // You gaze up into the glittering night sky.  With great care, you compose a chart of the most prominent star patterns.

							obj.Complete();
						}
						else
						{
							player.SendLocalizedMessage( 1055039 ); // You gaze up into the sky, but it is not dark enough to see any stars.
						}
					}
				}
			}
		}

		public Spyglass( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}