//Myron - Comment des ressources pour coller � celles disponibles dans les autres menus
using System; 
using Server.Items; 
using Server.Mobiles; 

namespace Server.Engines.Craft 
{ 
	public class DefMasonry : CraftSystem 
	{ 
		public override SkillName MainSkill 
		{ 
			get{ return SkillName.Carpentry; } 
		} 

		public override int GumpTitleNumber 
		{ 
			get{ return 1044500; } // <CENTER>MASONRY MENU</CENTER> 
		} 

		private static CraftSystem m_CraftSystem; 

		public static CraftSystem CraftSystem 
		{ 
			get 
			{ 
				if ( m_CraftSystem == null ) 
					m_CraftSystem = new DefMasonry(); 

				return m_CraftSystem; 
			} 
		} 

		public override double GetChanceAtMin( CraftItem item ) 
		{ 
			return 0.0; // 0% 
		} 

		private DefMasonry() : base( 1, 1, 1.25 )// base( 1, 2, 1.7 ) 
		{ 
		} 

		public override bool RetainsColorFrom( CraftItem item, Type type )
		{
			return true;
		}

		public override int CanCraft( Mobile from, BaseTool tool, Type itemType )
		{
			if( tool == null || tool.Deleted || tool.UsesRemaining < 0 )
				return 1044038; // You have worn out your tool!
			else if ( !BaseTool.CheckTool( tool, from ) )
				return 1048146; // If you have a tool equipped, you must use that tool.
			else if ( !(from is PlayerMobile && ((PlayerMobile)from).Masonry && from.Skills[SkillName.Carpentry].Base >= 100.0) )
				return 1044633; // You havent learned stonecraft.
			else if ( !BaseTool.CheckAccessible( tool, from ) )
				return 1044263; // The tool must be on your person to use.

			return 0;
		} 

		public override void PlayCraftEffect( Mobile from ) 
		{ 
			// no effects
			//if ( from.Body.Type == BodyType.Human && !from.Mounted ) 
			//	from.Animate( 9, 5, 1, true, false, 0 ); 
			//new InternalTimer( from ).Start(); 
		} 

		// Delay to synchronize the sound with the hit on the anvil 
		private class InternalTimer : Timer 
		{ 
			private Mobile m_From; 

			public InternalTimer( Mobile from ) : base( TimeSpan.FromSeconds( 0.7 ) ) 
			{ 
				m_From = from; 
			} 

			protected override void OnTick() 
			{ 
				m_From.PlaySound( 0x23D ); 
			} 
		} 

		public override int PlayEndingEffect( Mobile from, bool failed, bool lostMaterial, bool toolBroken, int quality, bool makersMark, CraftItem item ) 
		{ 
			if ( toolBroken ) 
				from.SendLocalizedMessage( 1044038 ); // You have worn out your tool 

			if ( failed ) 
			{ 
				if ( lostMaterial ) 
					return 1044043; // You failed to create the item, and some of your materials are lost. 
				else 
					return 1044157; // You failed to create the item, but no materials were lost. 
			} 
			else 
			{ 
				if ( quality == 0 ) 
					return 502785; // You were barely able to make this item.  It's quality is below average. 
				else if ( makersMark && quality == 2 ) 
					return 1044156; // You create an exceptional quality item and affix your maker's mark. 
				else if ( quality == 2 ) 
					return 1044155; // You create an exceptional quality item. 
				else             
					return 1044154; // You create the item. 
			} 
		} 

		public override void InitCraftList() 
		{ 
			// Decorations
			AddCraft( typeof( Vase ), 1044501, 1022888, 52.5, 102.5, typeof( Granite ), 1044514, 1, 1044513 );
			AddCraft( typeof( LargeVase ), 1044501, 1022887, 52.5, 102.5, typeof( Granite ), 1044514, 3, 1044513 );
			
			
				int index = AddCraft( typeof( SmallUrn ), 1044501, 1029244, 82.0, 132.0, typeof( Granite ), 1044514, 3, 1044513 );
				SetNeededExpansion( index, Expansion.SE );

				index = AddCraft( typeof( SmallTowerSculpture ), 1044501, 1029242, 82.0, 132.0, typeof( Granite ), 1044514, 3, 1044513 );
				SetNeededExpansion( index, Expansion.SE );
			

			// Furniture
			AddCraft( typeof( StoneChair ), 1044502, 1024635, 55.0, 105.0, typeof( Granite ), 1044514, 4, 1044513 );
			AddCraft( typeof( MediumStoneTableEastDeed ), 1044502, 1044508, 65.0, 115.0, typeof( Granite ), 1044514, 6, 1044513 );
			AddCraft( typeof( MediumStoneTableSouthDeed ), 1044502, 1044509, 65.0, 115.0, typeof( Granite ), 1044514, 6, 1044513 );
			AddCraft( typeof( LargeStoneTableEastDeed ), 1044502, 1044511, 75.0, 125.0, typeof( Granite ), 1044514, 9, 1044513 );
			AddCraft( typeof( LargeStoneTableSouthDeed ), 1044502, 1044512, 75.0, 125.0, typeof( Granite ), 1044514, 9, 1044513 );

			// Statues
			AddCraft( typeof( StatueSouth ), 1044503, 1044505, 60.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatueNorth ), 1044503, 1044506, 60.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatueEast ), 1044503, 1044507, 60.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( typeof( StatuePegasus ), 1044503, 1044510, 70.0, 130.0, typeof( Granite ), 1044514, 4, 1044513 );

            index = AddCraft(typeof(SlayerForge), "Misc", "Bassine � Slayers", 70.0, 100.0, typeof(GoldGranite), "Granite d'or", 2, 1044513);
            AddRes(index, typeof(Pitcher), "Pichet d'eau", 3);
            AddRes(index, typeof(MagicReflectScroll), "Parchemin de r�flection", 2);
            AddSkill(index, SkillName.Imbuing, -10.0, 20.0);
            ForceNonExceptional(index);

			SetSubRes( typeof( Granite ), 1044525 );

			AddSubRes( typeof( Granite ),			"Fer", 00.0, 1044513);
            AddSubRes(typeof(RustyGranite), "Rusty", 65.0, 1044514);
            AddSubRes(typeof(OldcopperGranite), "Vieux Cuivre", 70.0, 1044514);
            AddSubRes(typeof(DullcopperGranite), "Cuivre terni", 75.0, 1044514);
            AddSubRes(typeof(ShadowGranite), "Sombrine", 80.0, 1044514);
            AddSubRes(typeof(CopperGranite), "Cuivre", 85.0, 1044514);
            AddSubRes(typeof(BronzeGranite), "Bronze", 90.0, 1044514);
            AddSubRes(typeof(GoldGranite), "Or", 95.0, 1044514);
            AddSubRes(typeof(RoseGranite), "Rose", 99.0, 1044514);
			AddSubRes( typeof( AgapiteGranite ),	"Agapite", 65.0, 1044514);
			AddSubRes( typeof( ValoriteGranite ),	    "Valorite", 70.0, 1044514);
			//AddSubRes( typeof( BloodrockGranite ),		"Roche de sang", 75.0, 1044514);
			AddSubRes( typeof( VeriteGranite ),		"Verite", 80.0, 1044514);
			AddSubRes( typeof( SilverGranite ),		"Argent", 85.0, 1044514);
			AddSubRes( typeof( DragonGranite ),	"Dragon", 90.0, 1044514);
			AddSubRes( typeof( TitanGranite ),		"Titan", 95.0, 1044514);
            //AddSubRes( typeof( CrystalineGranite ),	"Crystaline", 99.0, 1044514);
            //AddSubRes(typeof(KryniteGranite), "Krynite", 65.0, 1044514);
            //AddSubRes(typeof(VulcanGranite), "Vulcan", 70.0, 1044514);
            //AddSubRes(typeof(BloodcrestGranite), "Craie de sang", 75.0, 1044514);
            //AddSubRes(typeof(ElvinGranite), "Elvin", 80.0, 1044514);
            //AddSubRes(typeof(AcidGranite), "Acid", 85.0, 1044514);
            //AddSubRes(typeof(AquaGranite), "Aqua", 90.0, 1044514);
            AddSubRes(typeof(EldarGranite), "Eldar", 95.0, 1044514);
            //AddSubRes(typeof(GlowingGranite), "Glowing", 99.0, 1044514);
            //AddSubRes(typeof(GorganGranite), "Gorgan", 65.0, 1044514);
            //AddSubRes(typeof(SteelGranite), "Acier", 70.0, 1044514);
            //AddSubRes(typeof(SandrockGranite), "Pierre de sable", 75.0, 1044514);
            //AddSubRes(typeof(MytherilGranite), "Mytheril", 80.0, 1044514);
            //AddSubRes(typeof(BlackrockGranite), "Roche sombre", 85.0, 1044514);
           
		}
	}
}