using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    class ObservateurYeth : BaseCreature
    {
        [Constructable]
        public ObservateurYeth() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "Observateur de Yeth";
            Body = 13;
			BaseSoundID = 263;
            Hue = 0455;
			SetStr(60, 70);
			SetDex(40, 50);
			SetInt(50, 60);

			SetHits( 196, 213 );
			SetDamage(3, 7);

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Cold, 80 );

			SetResistance( ResistanceType.Physical, 45, 55 );
			SetResistance( ResistanceType.Fire, 10, 15 );
			SetResistance( ResistanceType.Cold, 50, 60 );
            SetResistance(ResistanceType.Poison, 100, 100); 
            SetResistance(ResistanceType.Energy, 25, 35);

			SetSkill( SkillName.MagicResist, 15.0, 35.0 );
			SetSkill( SkillName.Tactics, 35.1, 45.0 );
			SetSkill( SkillName.Wrestling, 35.0, 50.0);

			Fame = 4000;
			Karma = -7000;

			VirtualArmor = 50;

			PackItem( new BlackPearl( 3 ) );
		}

        public override void OnActionCombat()
        {
            int caseSwitch = RandomMinMaxScaled(1, 4);
            switch(caseSwitch)
            {
                case 1:
                     this.Say("Ce n'etait qu'un esclave parmi tant d'autres...");
               break ;
                case 2:
                     this.Say("Me tuer ne suffira pas...");
               break ;
                case 3:
                     this.Say("Je n'en suis qu'un parmi des milliers...");
            break ;
                case 4:
                     this.Say("Vous etes revenus pour mieux nous servir...");
            break ;
            }
            base.OnActionCombat();
        }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor );
		}

		public override bool BleedImmune{ get{ return true; } }

		public override int TreasureMapLevel{ get{ return Utility.RandomList( 2, 3 ); } }

		public ObservateurYeth( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}

