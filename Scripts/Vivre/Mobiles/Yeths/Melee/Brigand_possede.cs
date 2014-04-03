using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
	public class Brigand_possede : BaseCreature
	{
		public override bool ClickTitle{ get{ return false; } }

		[Constructable]
		public Brigand_possede() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			SpeechHue = Utility.RandomNeutralHue();
			Title = "Le brigand";
			Hue = Utility.RandomSkinHue();

			if ( this.Female = Utility.RandomBool() )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );
				AddItem( new Skirt( Utility.RandomNeutralHue() ) );
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );
				AddItem( new ShortPants( Utility.RandomNeutralHue() ) );
			}

            
            SetStr(66, 80);
            SetDex(81, 95);
			SetInt( 61, 75 );

			SetDamage( 10, 23 );

			SetSkill( SkillName.Swords, 35.0, 58);
			SetSkill( SkillName.Wrestling, 35.0, 58.0);
            SetSkill(SkillName.MagicResist, 45.0, 75.0);
            SetSkill(SkillName.Macing, 35.50, 59.50);
            Fame = RandomMinMaxScaled(2500, 4000);
			Karma = -3500;

			AddItem( new Boots( Utility.RandomNeutralHue() ) );
			AddItem( new FancyShirt());
			AddItem( new Bandana());

			switch ( Utility.Random( 7 ))
			{
				case 0: AddItem( new Longsword() ); break;
				case 1: AddItem( new Cutlass() ); break;
				case 2: AddItem( new Broadsword() ); break;
				case 3: AddItem( new Axe() ); break;
				case 4: AddItem( new Club() ); break;
				case 5: AddItem( new Dagger() ); break;
				case 6: AddItem( new Spear() ); break;
			}

			Utility.AssignRandomHair( this );
		}


        public override void OnDeath(Container c)
        {
           ObservateurYeth spawn = new ObservateurYeth();
           Map map = this.Map;
           Point3D loc = this.Location;
           spawn.MoveToWorld(loc, map);
           base.OnDeath(c);
        }

        public override void OnActionCombat()
        {
            int caseSwitch = RandomMinMaxScaled(1, 3);
            switch(caseSwitch)
            {
                case 1:
                    this.Say("Ils sont en moi... Raah... ! Je suis en toi !");
               break ;
                case 2:
                    this.Say("Je vois leur monde...horreur! Que d'horreur!");
               break ;
                case 3:
                    this.Say("Ils nous veulent pour esclaves...Tu es esclave !");
                break ;        
            }
            base.OnActionCombat();
        }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
		}

		public override bool AlwaysMurderer{ get{ return true; } }

		public Brigand_possede( Serial serial ) : base( serial )
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