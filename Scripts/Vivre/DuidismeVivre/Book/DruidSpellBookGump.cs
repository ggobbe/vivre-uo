using System; 
using System.Collections; 
using Server; 
using Server.Items; 
using Server.Network; 
using Server.Spells; 
using Server.Spells.Druid; 
using Server.Prompts; 

namespace Server.Gumps 
{ 
   public class DruidicSpellbookGump : Gump 
   { 
      private DruidicSpellbook m_Book; 
       
      int gth = 0x903; 
      private void AddBackground() 
      { 
         AddPage( 0 ); 
          
         AddImage( 100, 10, 0x89B, 0 ); 
        //AddImage( 255, 10, 0x8AD, 0x48B ); 
          
          
          
        // AddLabel( 140, 45, gth, "Ohm - Earth" ); 
         //AddLabel( 140, 60, gth, "Ess - Air" ); 
        // AddLabel( 140, 75, gth, "Crur - Fire" ); 
         //AddLabel( 140, 90, gth, "Sepa - Water" ); 
         //AddLabel( 140, 110, gth, "Kes - One" ); 
         //AddLabel( 140, 125, gth, "Sec - Whole" ); 
         //AddLabel( 140, 140, gth, "En  - Call" ); 
         //AddLabel( 140, 155, gth, "Vauk - Banish" ); 
         //AddLabel( 140, 170, gth, "Tia - Befoul" ); 
         //AddLabel( 140, 185, gth, "Ante - Cleanse" ); 
          
      } 
       
      public bool HasSpell( Mobile from, int spellID ) 
	{
		return ( m_Book.HasSpell( spellID ) ); 
	}
       
       
      public DruidicSpellbookGump( Mobile from, DruidicSpellbook book ) : base( 150, 200 ) 
      { 
          
         m_Book = book; 
         AddBackground(); 
         AddPage( 1 ); 
         AddLabel( 150, 17, gth, "Natural Magic" ); 
         int sbtn = 0x93A; 
         int dby = 40; 
         int dbpy = 40;; 
         AddButton( 396, 14, 0x89E, 0x89E, 17, GumpButtonType.Page, 2 ); 
          
         if (this.HasSpell( from, 316) ) 
         { 
            AddLabel( 145, dbpy, gth, "Summon Firefly" ); 
            AddButton( 125, dbpy + 3, sbtn, sbtn, 16, GumpButtonType.Reply, 1 ); 
            dby = dby + 20; 
         } 
         if (this.HasSpell( from, 302) ) 
         { 
            AddLabel( 145, dby, gth, "Hollow Reed" ); 
            AddButton( 125, dby + 3, sbtn, sbtn, 2, GumpButtonType.Reply, 1 ); 
            dby = dby + 20; 
             
         } 
         if (this.HasSpell( from, 303) ) 
         { 
            AddLabel( 145, dby, gth, "Pack Of Beasts" ); 
            AddButton( 125, dby + 3, sbtn, sbtn, 3, GumpButtonType.Reply, 1 ); 
            dby = dby + 20; 
         } 
         if (this.HasSpell( from, 304) ) 
         { 
            AddLabel( 145, dby, gth, "Spring Of Life" ); 
            AddButton( 125, dby + 3, sbtn, sbtn, 4, GumpButtonType.Reply, 1 ); 
            dby = dby + 20; 
         } 
         if (this.HasSpell( from, 305) ) 
         { 
            AddLabel( 145, dby, gth, "Grasping Roots" ); 
            AddButton( 125, dby + 3, sbtn, sbtn, 5, GumpButtonType.Reply, 1 ); 
            dby = dby + 20; 
         } 
         if (this.HasSpell( from, 306) ) 
         { 
            AddLabel( 145, dby, gth, "Blend With Forest" ); 
            AddButton( 125, dby + 3, sbtn, sbtn, 6, GumpButtonType.Reply, 1 ); 
            dby = dby + 20; 
         } 
         if (this.HasSpell( from, 307) ) 
         { 
            AddLabel( 145, dby, gth, "Swarm Of Insects" ); 
            AddButton( 125, dby + 3, sbtn, sbtn, 7, GumpButtonType.Reply, 1 ); 
            dby = dby + 20; 
         } 
         if (this.HasSpell( from, 308) ) 
         { 
            AddLabel( 145, dby, gth, "Volcanic Eruption" ); 
            AddButton( 125, dby + 3, sbtn, sbtn, 8, GumpButtonType.Reply, 1 ); 
         } 
         if (this.HasSpell( from, 309) ) 
         { 
            AddLabel( 315, dbpy, gth, "Summon Treefellow" ); 
            AddButton( 295, dbpy + 3, sbtn, sbtn, 9, GumpButtonType.Reply, 1 ); 
            dbpy = dbpy + 20; 
         } 
         if (this.HasSpell( from, 310) ) 
         { 
            AddLabel( 315, dbpy, gth, "Stone Circle" ); 
            AddButton( 295, dbpy + 3, sbtn, sbtn, 10, GumpButtonType.Reply, 1 ); 
            dbpy = dbpy + 20; 
         } 
         if (this.HasSpell( from, 311) ) 
         { 
            AddLabel( 315, dbpy, gth, "Enchanted Grove" ); 
            AddButton( 295, dbpy + 3, sbtn, sbtn, 11, GumpButtonType.Reply, 1 ); 
            dbpy = dbpy + 20; 
         } 
         if (this.HasSpell( from, 312) ) 
         { 
            AddLabel( 315, dbpy, gth, "Lure Stone" ); 
            AddButton( 295, dbpy + 3, sbtn, sbtn, 12, GumpButtonType.Reply, 1 ); 
            dbpy = dbpy + 20; 
         } 
         if (this.HasSpell( from, 313) ) 
         { 
            AddLabel( 315, dbpy, gth, "Nature's Passage" ); 
            AddButton( 295, dbpy + 3, sbtn, sbtn, 13, GumpButtonType.Reply, 1 ); 
            dbpy = dbpy + 20; 
         } 
         if (this.HasSpell( from, 314) ) 
         { 
            AddLabel( 315, dbpy, gth, "Mushroom Gateway" ); 
            AddButton( 295, dbpy + 3, sbtn, sbtn, 14, GumpButtonType.Reply, 1 ); 
            dbpy = dbpy + 20; 
         } 
         if (this.HasSpell( from, 315) ) 
         { 
            AddLabel( 315, dbpy, gth, "Restorative Soil" ); 
            AddButton( 295, dbpy + 3, sbtn, sbtn, 15, GumpButtonType.Reply, 1 ); 
            dbpy = dbpy + 20; 
         } 
         if (this.HasSpell( from, 301) ) 
         { 
            AddLabel( 315, dby, gth, "Shield Of Earth" ); 
            AddButton( 295, dby + 3, sbtn, sbtn, 1, GumpButtonType.Reply, 1 ); 
             
         } 
          
         int i = 2; 
          
         if (this.HasSpell( from, 316) ) 
         { 
            AddPage( i ); 
            AddButton( 396, 14, 0x89E, 0x89E, 18, GumpButtonType.Page, i+1 ); 
            AddButton( 123, 15, 0x89D, 0x89D, 19, GumpButtonType.Page, i-1 ); 
            AddLabel( 150, 37, gth, "Summon Firefly" ); 
            AddHtml( 130, 59, 123, 132, "Summons a tiny firefly to light the Druid's path. The Firefly is a noncombatant being.", false, false ); 
            AddLabel( 123, 187, gth, "Kes En Crur" );
            AddLabel( 295, 37, gth, "Reagents:" ); 
            AddLabel( 295, 57, gth, "Sulfurous Ash" ); 
            AddLabel( 295, 77, gth, "Pumice" ); 
            AddLabel( 295, 167, gth, "Required Skill:  1" ); 
            AddLabel( 295, 187, gth, "Required Mana:  10" ); 
            i++; 
         } 
          
         if (this.HasSpell( from, 302) ) 
         { 
            AddPage( i ); 
            AddButton( 396, 14, 0x89E, 0x89E, 18, GumpButtonType.Page, i+1 ); 
            AddButton( 123, 15, 0x89D, 0x89D, 19, GumpButtonType.Page, i-1 ); 
            AddLabel( 150, 37, gth, "Hollow Reed" ); 
            AddHtml( 130, 59, 123, 132, "Increases both the strength and the intelligence of the Target.", false, false ); 
            AddLabel( 123, 187, gth, "Sec Crur Aeta" ); 
            AddLabel( 295, 37, gth, "Reagents:" ); 
            AddLabel( 295, 57, gth, "Bloodmoss" ); 
            AddLabel( 295, 77, gth, "Mandrake Root" ); 
            AddLabel( 295, 97, gth, "Sulfurous Ash" ); 
            AddLabel( 295, 167, gth, "Required Skill:  30" ); 
            AddLabel( 295, 187, gth, "Required Mana:  30" ); 
            i++; 
         } 
          
         if (this.HasSpell( from, 303) ) 
         { 
            AddPage( i ); 
            AddButton( 396, 14, 0x89E, 0x89E, 18, GumpButtonType.Page, i+1 ); 
            AddButton( 123, 15, 0x89D, 0x89D, 19, GumpButtonType.Page, i-1 ); 
            AddLabel( 150, 37, gth, "Pack Of Beasts" ); 
            AddHtml( 130, 59, 123, 132, "Summons a pack of beasts to fight for the Druid. Spell length increases with skill.", false, false ); 
            AddLabel( 123, 187, gth, "En Sec Ohm Ess Sepa" );
            AddLabel( 295, 37, gth, "Reagents:" ); 
            AddLabel( 295, 57, gth, "Bloodmoss" ); 
            AddLabel( 295, 77, gth, "Spider Silk" ); 
            AddLabel( 295, 97, gth, "Petrified Wood" ); 
            AddLabel( 295, 167, gth, "Required Skill:  50" ); 
            AddLabel( 295, 187, gth, "Required Mana:  45" ); 
            i++; 
         } 
         if (this.HasSpell( from, 304) ) 
         { 
            AddPage( i ); 
            AddButton( 396, 14, 0x89E, 0x89E, 18, GumpButtonType.Page, i+1 ); 
            AddButton( 123, 15, 0x89D, 0x89D, 19, GumpButtonType.Page, i-1 ); 
            AddLabel( 150, 37, gth, "Spring Of Life" ); 
            AddHtml( 130, 59, 123, 132, "Creates a magical spring that heals the Druid and their party.", false, false ); 
            AddLabel( 123, 187, gth, "En Sepa Aete" );
            AddLabel( 295, 37, gth, "Reagents:" ); 
            AddLabel( 295, 57, gth, "Spring Water" ); 
            AddLabel( 295, 77, gth, "Petrified Wood" ); 
            AddLabel( 295, 167, gth, "Required Skill:  40" ); 
            AddLabel( 295, 187, gth, "Required Mana:  40" ); 
            i++; 
         } 
         if (this.HasSpell( from, 305) ) 
         { 
            AddPage( i ); 
            AddButton( 396, 14, 0x89E, 0x89E, 18, GumpButtonType.Page, i+1 ); 
            AddButton( 123, 15, 0x89D, 0x89D, 19, GumpButtonType.Page, i-1 ); 
            AddLabel( 150, 37, gth, "Grasping Roots" ); 
            AddHtml( 130, 59, 123, 132, "Summons roots from the ground to entangle a single target.", false, false ); 
            AddLabel( 123, 187, gth, "En Ohm Sepa Tia Kes" );
            AddLabel( 295, 37, gth, "Reagents:" ); 
            AddLabel( 295, 57, gth, "Bloodmoss" ); 
            AddLabel( 295, 77, gth, "Spring Water" ); 
            AddLabel( 295, 97, gth, "Spiders Silk" ); 
            AddLabel( 295, 167, gth, "Required Skill:  40" ); 
            AddLabel( 295, 187, gth, "Required Mana:  40" ); 
            i++; 
         } 
         if (this.HasSpell( from, 306) ) 
         { 
            AddPage( i ); 
            AddButton( 396, 14, 0x89E, 0x89E, 18, GumpButtonType.Page, i+1 ); 
            AddButton( 123, 15, 0x89D, 0x89D, 19, GumpButtonType.Page, i-1 ); 
            AddLabel( 150, 37, gth, "Blend With Forest" ); 
            AddHtml( 130, 59, 123, 132, "Makes the Druid and surrounding group seem to vanish in their surroundings.  ", false, false ); 
            AddLabel( 123, 187, gth, "Kes Ohm" );
            AddLabel( 295, 37, gth, "Reagents:" ); 
            AddLabel( 295, 57, gth, "Bloodmoss" ); 
            AddLabel( 295, 77, gth, "Spider Silk" ); 
            AddLabel( 295, 167, gth, "Required Skill:  65" ); 
            AddLabel( 295, 187, gth, "Required Mana:  50" ); 
            i++; 
         } 
         if (this.HasSpell( from, 307) ) 
         { 
            AddPage( i ); 
            AddButton( 396, 14, 0x89E, 0x89E, 18, GumpButtonType.Page, i+1 ); 
            AddButton( 123, 15, 0x89D, 0x89D, 19, GumpButtonType.Page, i-1 ); 
            AddLabel( 150, 37, gth, "Swarm Of Insects" ); 
            AddHtml( 130, 59, 123, 132, "Summons a swam of insects that bite and sting the targeted enemy.", false, false ); 
            AddLabel( 123, 167, gth, "Es Ohm En Sec Tia" );
            AddLabel( 295, 37, gth, "Reagents:" ); 
            AddLabel( 295, 57, gth, "Sulfurous Ash" ); 
            AddLabel( 295, 77, gth, "Bloodmoss" ); 
            AddLabel( 295, 97, gth, "Pumice" ); 
            AddLabel( 295, 167, gth, "Required Skill:  75" ); 
            AddLabel( 295, 187, gth, "Required Mana:  60" ); 
            i++; 
         } 
         if (this.HasSpell( from, 308) ) 
         { 
            AddPage( i ); 
            AddButton( 396, 14, 0x89E, 0x89E, 18, GumpButtonType.Page, i+1 ); 
            AddButton( 123, 15, 0x89D, 0x89D, 19, GumpButtonType.Page, i-1 ); 
            AddLabel( 150, 37, gth, "Volcanic Eruption" ); 
            AddHtml( 130, 59, 123, 132, "A blast of molten lava bursts from the ground, hitting every enemy nearby.", false, false ); 
            AddLabel( 123, 187, gth, "Vauk Ohm En Tia Crur" );
            AddLabel( 295, 37, gth, "Reagents:" ); 
            AddLabel( 295, 57, gth, "Sulfurous Ash" ); 
            AddLabel( 295, 77, gth, "Pumice" ); 
            AddLabel( 295, 167, gth, "Required Skill:  88" ); 
            AddLabel( 295, 187, gth, "Required Mana:  65" ); 
            i++; 
         } 
         if (this.HasSpell( from, 309) ) 
         { 
            AddPage( i ); 
            AddButton( 396, 14, 0x89E, 0x89E, 18, GumpButtonType.Page, i+1 ); 
            AddButton( 123, 15, 0x89D, 0x89D, 19, GumpButtonType.Page, i-1 ); 
            AddLabel( 150, 37, gth, "Summon Treefellow" ); 
            AddHtml( 130, 59, 123, 132, "Summons a powerful woodland spirit to fight for the Druid.", false, false ); 
            AddLabel( 123, 187, gth, "Kes En Ohm Sepa" ); 
            AddLabel( 295, 37, gth, "Reagents:" ); 
            AddLabel( 295, 57, gth, "Bloodmoss" ); 
            AddLabel( 295, 77, gth, "Spring Water" ); 
            AddLabel( 295, 97, gth, "Petrified Wood" ); 
            AddLabel( 295, 167, gth, "Required Skill:  80" ); 
            AddLabel( 295, 187, gth, "Required Mana:  50" ); 
            i++; 
         } 
         if (this.HasSpell( from, 310) ) 
         { 
            AddPage( i ); 
            AddButton( 396, 14, 0x89E, 0x89E, 18, GumpButtonType.Page, i+1 ); 
            AddButton( 123, 15, 0x89D, 0x89D, 19, GumpButtonType.Page, i-1 ); 
            AddLabel( 150, 37, gth, "Stone Circle" ); 
            AddHtml( 130, 59, 123, 132, "Forms an impassable circle of stones, ideal for trapping enemies.", false, false ); 
            AddLabel( 123, 187, gth, "En Ess Ohm" );
            AddLabel( 295, 37, gth, "Reagents:" ); 
            AddLabel( 295, 57, gth, "Petrified Wood" ); 
            AddLabel( 295, 77, gth, "Sulfurous Ash" ); 
            AddLabel( 295, 97, gth, "Spring Water" ); 
            AddLabel( 295, 167, gth, "Required Skill:  60" ); 
            AddLabel( 295, 187, gth, "Required Mana:  45" ); 
            i++; 
         } 
         if (this.HasSpell( from, 311) ) 
         { 
            AddPage( i ); 
            AddButton( 396, 14, 0x89E, 0x89E, 18, GumpButtonType.Page, i+1 ); 
            AddButton( 123, 15, 0x89D, 0x89D, 19, GumpButtonType.Page, i-1 ); 
            AddLabel( 150, 37, gth, "Enchanted Grove" ); 
            AddHtml( 130, 59, 123, 132, "Causes a grove of magical trees to grow. All friendlies who enter the enchanted area regain health and mana.", false, false ); 
            AddLabel( 123, 187, gth, "En Ante Ohm Sepa" );
            AddLabel( 295, 37, gth, "Reagents:" ); 
            AddLabel( 295, 57, gth, "Petrified Wood" ); 
            AddLabel( 295, 77, gth, "Mandrake Root" ); 
            AddLabel( 295, 97, gth, "Spring Water" ); 
            AddLabel( 295, 167, gth, "Required Skill:  75" ); 
            AddLabel( 295, 187, gth, "Required Mana:  60" ); 
            i++; 
         } 
         if (this.HasSpell( from, 312) ) 
         { 
            AddPage( i ); 
            AddButton( 396, 14, 0x89E, 0x89E, 18, GumpButtonType.Page, i+1 ); 
            AddButton( 123, 15, 0x89D, 0x89D, 19, GumpButtonType.Page, i-1 ); 
            AddLabel( 150, 37, gth, "Lure Stone" ); 
            AddHtml( 130, 59, 123, 132, "Creates a magical stone that calls all nearby creatures to it.", false, false ); 
            AddLabel( 123, 187, gth, "En Kes Ohm Crur" );
            AddLabel( 295, 37, gth, "Reagents:" ); 
            AddLabel( 295, 57, gth, "Petrified Wood" ); 
            AddLabel( 295, 77, gth, "Spring Water" ); 
            AddLabel( 295, 167, gth, "Required Skill:  25" ); 
            AddLabel( 295, 187, gth, "Required Mana:  30" ); 
            i++; 
         } 
         if (this.HasSpell( from, 313) ) 
         { 
            AddPage( i ); 
            AddButton( 396, 14, 0x89E, 0x89E, 18, GumpButtonType.Page, i+1 ); 
            AddButton( 123, 15, 0x89D, 0x89D, 19, GumpButtonType.Page, i-1 ); 
            AddLabel( 150, 37, gth, "Nature's Passage" ); 
            AddHtml( 130, 59, 123, 132, "The Druid is turned into flower petals and carried on the wind to a recall rune location.", false, false ); 
            AddLabel( 123, 187, gth, "Kes Sec Vauk" );
            AddLabel( 295, 37, gth, "Reagents:" ); 
            AddLabel( 295, 57, gth, "Pumice" ); 
            AddLabel( 295, 77, gth, "Bloodmoss" ); 
            AddLabel( 295, 97, gth, "Mandrake Root" ); 
            AddLabel( 295, 167, gth, "Required Skill:  25" ); 
            AddLabel( 295, 187, gth, "Required Mana:  10" ); 
            i++; 
         } 
         if (this.HasSpell( from, 314) ) 
         { 
            AddPage( i ); 
            AddButton( 396, 14, 0x89E, 0x89E, 18, GumpButtonType.Page, i+1 ); 
            AddButton( 123, 15, 0x89D, 0x89D, 19, GumpButtonType.Page, i-1 ); 
            AddLabel( 150, 37, gth, "Mushroom Gateway" ); 
            AddHtml( 130, 59, 123, 132, "A magical circle of mushrooms opens, allowing the Druid  and companions to step through it to a marked location.", false, false ); 
            AddLabel( 123, 187, gth, "Vauk Sepa Ohm" );
            AddLabel( 295, 37, gth, "Reagents:" ); 
            AddLabel( 295, 57, gth, "Spider Silk" ); 
            AddLabel( 295, 77, gth, "Spring Water" ); 
            AddLabel( 295, 97, gth, "Mandrake Root" ); 
            AddLabel( 295, 167, gth, "Required Skill:  70" ); 
            AddLabel( 295, 187, gth, "Required Mana:  40" ); 
            i++; 
         } 
         if (this.HasSpell( from, 315) ) 
         { 
            AddPage( i ); 
            AddButton( 396, 14, 0x89E, 0x89E, 18, GumpButtonType.Page, i+1 ); 
            AddButton( 123, 15, 0x89D, 0x89D, 19, GumpButtonType.Page, i-1 ); 
            AddLabel( 150, 37, gth, "Restorative Soil" ); 
            AddHtml( 130, 59, 123, 132, "Saturates a patch of land with power, causing healing mud capable of restoring life, but only lasts a few moments.", false, false ); 
            AddLabel( 123, 187, gth, "Ohm Sepa Ante" );
            AddLabel( 295, 37, gth, "Reagents:" ); 
            AddLabel( 295, 57, gth, "Petrified Wood" ); 
            AddLabel( 295, 77, gth, "Bloodmoss" ); 
            AddLabel( 295, 97, gth, "Spring Water" ); 
            AddLabel( 295, 167, gth, "Required Skill:  85" ); 
            AddLabel( 295, 187, gth, "Required Mana:  55" ); 
            i++; 
         } 
         if (this.HasSpell( from, 301) ) 
         { 
            AddPage( i ); 
            AddButton( 123, 15, 0x89D, 0x89D, 19, GumpButtonType.Page, i-1 ); 
            AddLabel( 150, 37, gth, "Shield Of Earth" ); 
            AddHtml( 130, 59, 123, 132, "A quick-growing wall of drouse-inducing gases springs from the earth to hinder the foes of the Druid.", false, false ); 
            AddLabel( 123, 187, gth, "Kes En Sepa Ohm" );
            AddLabel( 295, 37, gth, "Reagents:" ); 
            AddLabel( 295, 57, gth, "Mandrake Root" ); 
            AddLabel( 295, 77, gth, "Spider Silk" ); 
            AddLabel( 295, 167, gth, "Required Skill:  60" ); 
            AddLabel( 295, 187, gth, "Required Mana:  45" ); 
            i++; 
         } 
          
         AddPage( i ); 
         AddButton( 123, 15, 0x89D, 0x89D, 19, GumpButtonType.Page, i-1 ); 
      } 
       
       
       
       
       
      public override void OnResponse( NetState state, RelayInfo info ) 
      { 
         Mobile from = state.Mobile; 
         switch ( info.ButtonID ) 
         { 
            case 0: 
               { 
                  break; 
               } 
                
            case 1: 
               { 
                  new ShieldOfEarthSpell( from, null ).Cast(); 
                  break; 
               } 
                   
            case 2: 
               { 
                  new HollowReedSpell( from, null ).Cast(); 
                  break; 
               } 
                   
            case 3: 
               { 
                  new PackOfBeastSpell( from, null ).Cast(); 
                  break; 
               } 
                      
            case 4: 
               { 
                  new SpringOfLifeSpell( from, null ).Cast(); 
                  break; 
               } 
                         
            case 5: 
               { 
                  new GraspingRootsSpell( from, null ).Cast(); 
                  break; 
               } 
                            
            case 6: 
               { 
                  new BlendWithForestSpell( from, null ).Cast(); 
                  break; 
               } 
                               
            case 7: 
               { 
                  new SwarmOfInsectsSpell( from, null ).Cast(); 
                  break; 
               } 
                                  
            case 8: 
               { 
                  new VolcanicEruptionSpell( from, null ).Cast(); 
                  break; 
               } 
                                     
            case 9: 
               { 
                  new TreefellowSpell( from, null ).Cast(); 
                  break; 
               } 
                                        
            case 10: 
               { 
                  new StoneCircleSpell( from, null ).Cast(); 
                  break; 
               } 
                                           
                                           
            case 11: 
               { 
                  new EnchantedGroveSpell( from, null ).Cast(); 
                  break; 
               } 
                                              
            case 12: 
               { 
                  new LureStoneSpell( from, null ).Cast(); 
                  break; 
               } 
                                                 
            case 13: 
               { 
                  new NaturesPassageSpell( from, null ).Cast(); 
                  break; 
               } 
                                                    
            case 14: 
               { 
                  new MushroomGatewaySpell( from, null ).Cast(); 
                  break; 
               } 
                                                       
            case 15: 
               { 
                  new RestorativeSoilSpell( from, null ).Cast(); 
                  break; 
               } 
                                                          
            case 16: 
               { 
                  new FireflySpell( from, null ).Cast(); 
                  break; 
               } 
                                                          
            case 17: 
               { 
                  from.PlaySound(0x55); 
                  break; 
               } 
          
            case 18: 
               { 
                  from.PlaySound(0x55); 
                  break; 
               } 
                                                                   
            case 19: 
               { 
                  from.PlaySound(0x55); 
                  break; 
               } 
                                                                      
         } 
      } 
   } 
}
