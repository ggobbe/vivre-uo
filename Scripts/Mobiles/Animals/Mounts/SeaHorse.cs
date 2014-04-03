using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a sea horse corpse" )]
	public class SeaHorse : BaseMount
	{
		[Constructable]
		public SeaHorse() : this( "a sea horse" )
		{
			CanSwim = true;
		}

        // Scriptiz : on ne peut pas bondé ces animaux !
        public override bool IsBondable
        {
            get { return false; }
        }

        // Scriptiz : il s'énerve vite !
        public override bool CanAngerOnTame
        {
            get { return true; }
        }

		[Constructable]
		public SeaHorse( string name ) : base( name, 0x90, 0x3EB3, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			InitStats( Utility.Random( 50, 30 ), Utility.Random( 50, 30 ), 10 );
			Skills[SkillName.MagicResist].Base = 25.0 + (Utility.RandomDouble() * 5.0);
			Skills[SkillName.Wrestling].Base = 35.0 + (Utility.RandomDouble() * 10.0);
			Skills[SkillName.Tactics].Base = 30.0 + (Utility.RandomDouble() * 15.0);

            // Scriptiz : seahorse tamables
            Tamable = true;
            MinTameSkill = 96;
            ControlSlots = 5;
		}

		public SeaHorse( Serial serial ) : base( serial )
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