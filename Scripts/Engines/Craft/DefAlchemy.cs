using System;
using Server.Items;
using Server.Engines.Plants;
using Xanthos.ShrinkSystem;

namespace Server.Engines.Craft
{
	public class DefAlchemy : CraftSystem
	{
		public override SkillName MainSkill
		{
			get	{ return SkillName.Alchemy;	}
		}

		public override int GumpTitleNumber
		{
			get { return 1044001; } // <CENTER>ALCHEMY MENU</CENTER>p
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get
			{
				if ( m_CraftSystem == null )
					m_CraftSystem = new DefAlchemy();

				return m_CraftSystem;
			}
		}

		public override double GetChanceAtMin( CraftItem item )
		{
			return 0.0; // 0%
		}

		private DefAlchemy() : base( 1, 1, 1.25 )// base( 1, 1, 3.1 )
		{
		}

		public override int CanCraft( Mobile from, BaseTool tool, Type itemType )
		{
			if( tool == null || tool.Deleted || tool.UsesRemaining < 0 )
				return 1044038; // You have worn out your tool!
			else if ( !BaseTool.CheckAccessible( tool, from ) )
				return 1044263; // The tool must be on your person to use.

			return 0;
		}

		public override void PlayCraftEffect( Mobile from )
		{
			from.PlaySound( 0x242 );
		}

		private static Type typeofPotion = typeof( BasePotion );

		public static bool IsPotion( Type type )
		{
			return typeofPotion.IsAssignableFrom( type );
		}

		public override int PlayEndingEffect( Mobile from, bool failed, bool lostMaterial, bool toolBroken, int quality, bool makersMark, CraftItem item )
		{
			if ( toolBroken )
				from.SendLocalizedMessage( 1044038 ); // You have worn out your tool

			if ( failed )
			{
				if ( IsPotion( item.ItemType ) )
				{
					from.AddToBackpack( new Bottle() );
					return 500287; // You fail to create a useful potion.
				}
				else
				{
					return 1044043; // You failed to create the item, and some of your materials are lost.
				}
			}
			else
			{
				from.PlaySound( 0x240 ); // Sound of a filling bottle

				if ( IsPotion( item.ItemType ) )
				{
					if ( quality == -1 )
						return 1048136; // You create the potion and pour it into a keg.
					else
						return 500279; // You pour the potion into a bottle...
				}
				else
				{
					return 1044154; // You create the item.
				}
			}
		}

		public override void InitCraftList()
		{
			int index = -1;

            // Nightsight Potion
            index = AddCraft(typeof(NightSightPotion), "Altérations", 1044542, -25.0, 25.0, typeof(SpidersSilk), 1044360, 1, 1044368);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);

            index = AddCraft(typeof(FrogMorphPotion), "Altérations", "Filtre d'amour", 50.0, 87.0, typeof(MorphBase), "Base de métamorphose", 1, "Il vous faut une base de métamorphose");
            AddRes(index, typeof(BullFrogLard), "Du gras de grenouille", 2, "Il vous faut une base grasse pour cette potion");
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);

            index = AddCraft(typeof(InvisibilityPotion), "Altération", "Potion d'invisibilité", 70.0, 110.00, typeof(Bloodmoss), 1044354, 5, 1044362);
            AddRes(index, typeof(Nightshade), 1044358, 4, 1044366);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);

            index = AddCraft(typeof(HallucinogenPotion), "Altération", "Potion d'hallucinations", 75.0, 120.00, typeof(HallucinogenMushroom), 1044354, 4, "Vous n'avez pas assez de champignons");
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);


			// Agility Potion
			index = AddCraft( typeof( AgilityPotion ), "Bénéfiques", 1044540, 15.0, 65.0, typeof( Bloodmoss ), 1044354, 1, 1044362 );
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );
            index = AddCraft(typeof(GreaterAgilityPotion), "Bénéfiques", 1044541, 35.0, 85.0, typeof(Bloodmoss), 1044354, 3, 1044362);
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );

            // Strength Potion
            index = AddCraft(typeof(StrengthPotion), "Bénéfiques", 1044546, 25.0, 75.0, typeof(MandrakeRoot), 1044357, 2, 1044365);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = AddCraft(typeof(GreaterStrengthPotion), "Bénéfiques", 1044547, 45.0, 95.0, typeof(MandrakeRoot), 1044357, 5, 1044365);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);



            // Explosion Potion
            index = AddCraft(typeof(LesserExplosionPotion), "Lancer", 1044555, 5.0, 55.0, typeof(SulfurousAsh), 1044359, 3, 1044367);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = AddCraft(typeof(ExplosionPotion), "Lancer", 1044556, 35.0, 85.0, typeof(SulfurousAsh), 1044359, 5, 1044367);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = AddCraft(typeof(GreaterExplosionPotion), "Lancer", 1044557, 65.0, 115.0, typeof(SulfurousAsh), 1044359, 10, 1044367);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);

            if (Core.SE)
            {
                index = AddCraft(typeof(SmokeBomb), "Lancer", 1030248, 90.0, 120.0, typeof(Eggs), 1044477, 1, 1044253);
                AddRes(index, typeof(Ginseng), 1044356, 3, 1044364);
                SetNeededExpansion(index, Expansion.SE);

                // Conflagration Potions
                index = AddCraft(typeof(ConflagrationPotion), "Lancer", 1072096, 55.0, 105.0, typeof(GraveDust), 1023983, 5, 1044253);
                AddRes(index, typeof(Bottle), 1044529, 1, 500315);
                SetNeededExpansion(index, Expansion.SE);
                index = AddCraft(typeof(GreaterConflagrationPotion), "Lancer", 1072099, 65.0, 115.0, typeof(GraveDust), 1023983, 10, 1044253);
                AddRes(index, typeof(Bottle), 1044529, 1, 500315);
                SetNeededExpansion(index, Expansion.SE);
                // Confusion Blast Potions
                index = AddCraft(typeof(ConfusionBlastPotion), "Lancer", 1072106, 55.0, 105.0, typeof(PigIron), 1023978, 5, 1044253);
                AddRes(index, typeof(Bottle), 1044529, 1, 500315);
                SetNeededExpansion(index, Expansion.SE);
                index = AddCraft(typeof(GreaterConfusionBlastPotion), "Lancer", 1072109, 65.0, 115.0, typeof(PigIron), 1023978, 10, 1044253);
                AddRes(index, typeof(Bottle), 1044529, 1, 500315);
                SetNeededExpansion(index, Expansion.SE);
            }


            // Poison Potion
            index = AddCraft(typeof(LesserPoisonPotion), "Poisons", 1044548, -5.0, 45.0, typeof(Nightshade), 1044358, 1, 1044366);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = AddCraft(typeof(PoisonPotion), "Poisons", 1044549, 15.0, 65.0, typeof(Nightshade), 1044358, 2, 1044366);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = AddCraft(typeof(GreaterPoisonPotion), "Poisons", 1044550, 55.0, 105.0, typeof(Nightshade), 1044358, 4, 1044366);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = AddCraft(typeof(DeadlyPoisonPotion), "Poisons", 1044551, 90.0, 140.0, typeof(Nightshade), 1044358, 8, 1044366);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = AddCraft(typeof(ParasiticPotion), "Poisons", "Parasitic Potion", 65.0, 105.0, typeof(ParasiticPlant), "Plantes parasites", 4, "Vous n'avez pas assez de plantes parasites");
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = AddCraft(typeof(DarkglowPotion), "Poisons", "Darkglow Potion", 65.0, 105.0, typeof(LuminescentFungi), "Fongiques luminescents", 5, "Vous n'avez pas assez de champignons");
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);

			// Heal Potion
			index = AddCraft( typeof( LesserHealPotion ), "Soins", 1044543, -25.0, 25.0, typeof( Ginseng ), 1044356, 1, 1044364 );
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );
            index = AddCraft(typeof(HealPotion), "Soins", 1044544, 15.0, 65.0, typeof(Ginseng), 1044356, 3, 1044364);
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );
            index = AddCraft(typeof(GreaterHealPotion), "Soins", 1044545, 55.0, 105.0, typeof(Ginseng), 1044356, 7, 1044364);
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );




            // Refresh Potion
            index = AddCraft(typeof(RefreshPotion), "Soins", 1044538, -25, 25.0, typeof(BlackPearl), 1044353, 1, 1044361);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = AddCraft(typeof(TotalRefreshPotion), "Soins", 1044539, 25.0, 75.0, typeof(BlackPearl), 1044353, 5, 1044361);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);


			// Cure Potion
			index = AddCraft( typeof( LesserCurePotion ), "Soins", 1044552, -10.0, 40.0, typeof( Garlic ), 1044355, 1, 1044363 );
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );
            index = AddCraft(typeof(CurePotion), "Soins", 1044553, 25.0, 75.0, typeof(Garlic), 1044355, 3, 1044363);
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );
            index = AddCraft(typeof(GreaterCurePotion), "Soins", 1044554, 65.0, 115.0, typeof(Garlic), 1044355, 6, 1044363);
			AddRes( index, typeof ( Bottle ), 1044529, 1, 500315 );






            //Todo : Set max chances 80%
            index = AddCraft(typeof(WhiteTeinture), "Teintures", "Teinture Blanche", 20.0, 60.0, typeof(GraveDust), 1023983, 6, 1044253);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            AddRes(index, typeof(BullFrogLard), "Du gras de grenouille", 1, "Il vous faut une base grasse pour la teinture");
            AddRes(index, typeof(WhitePearl), "Perle blanche", 1, "Il vous faut une perle blanche");
            MaxChance(index, 0.8);

            index = AddCraft(typeof(BlackTeinture), "Teintures", "Teinture Noire", 20,60, typeof(BlackPearl), 1044353, 12, 1044361);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            AddRes(index, typeof(BullFrogLard), "Du gras de grenouille", 1, "Il vous faut une base grasse pour la teinture");
            AddRes(index, typeof(DarkSapphire), "Saphir noir", 1, "Il vous faut un saphir noir");
            MaxChance(index, 0.8);

            index = AddCraft(typeof(CyanTeinture), "Teintures", "Teinture Cyan", 30.0, 80, typeof(TribalBerry), 1046460, 4, 1044253);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            AddRes(index, typeof(BullFrogLard), "Du gras de grenouille", 1, "Il vous faut une base grasse pour la teinture");
            AddRes(index, typeof(BlueScales), "Écaille marine", 1, "Il vous faut une écaille marine");
            MaxChance(index, 0.8);

            index = AddCraft(typeof(YellowTeinture), "Teintures", "Teinture Jaune", 30.0, 80, typeof(SulfurousAsh), 1044359, 12, 1044367);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            AddRes(index, typeof(BullFrogLard), "Du gras de grenouille", 1, "Il vous faut une base grasse pour la teinture");
            AddRes(index, typeof(YellowScales), "Écaille jaune", 1, "Il vous faut une écaille jaune");
            MaxChance(index, 0.8);

            index = AddCraft(typeof(MagentaTeinture), "Teintures", "Teinture Magenta", 30.0, 80, typeof(Bloodmoss), 1044354, 1, 1044362);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            AddRes(index, typeof(BullFrogLard), "Du gras de grenouille", 1, "Il vous faut une base grasse pour la teinture");
            AddRes(index, typeof(RedScales), "Écaille rouge", 1, "Il vous faut une écaille rouge");
            MaxChance(index, 0.8);

            // Scriptiz : ajout des potions de shrink
            index = AddCraft(typeof(ShrinkPotion), "Autres", "Potion de Shrink", 70.0, 120.0, typeof(Seed), 1060810, 3, "You do not have enough seeds to make that.");
            AddRes(index, typeof(RecallRune), 1060577, 3, 1115364);
            AddRes(index, typeof(Bottle), 1044529, 1, 500315);

            // Scriptiz : ajout des special Dye Tubs
            index = AddCraft(typeof(SpecialDyeTub), "Autres", 1006047, 70.0, 120.0, typeof(Eggs), 1044477, 5, "You do not have enough eggs to make that.");
            AddRes(index, typeof(Dyes), "Dyes", 3, "You need Dyes to make that.");
            AddRes(index, typeof(DyeTub), 1049753, 1, "You need a Dye Tub to make this.");

            // Scriptiz : ajout des furniture Dye Tubs pour teindre les couettes des lits et les oreillers des chaises
            index = AddCraft(typeof(FurnitureDyeTub), "Autres", 1006013, 80.0, 130.0, typeof(SpecialDyeTub), 1006047, 1);
            AddRes(index, typeof(Beeswax), "Cire d'abeille", 5, "Il vous manque de la Cire d'abeille");

            index = AddCraft(typeof(TitanToothPowder), "Autres", "Poudre de dent de titan", 70.0, 100.0, typeof(TitanTooth), "Dent de titan", 1, "Vous n'avez pas de dent de titan.");
            AddRes(index, typeof(GreaterPoisonPotion), "Poison fort", 1, "Vous avez besoin d'un poison suffisamment fort... mais pas trop!");
            AddRes(index, typeof(MortarPestle), "Mortier", 1, "Vous devez ruiner un mortier.");
            SetUseAllRes(index, true);

		}
	}
}