using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a dragon corpse" )]
	public class AngryGreaterDragon : BaseCreature
	{

		[Constructable]
		public AngryGreaterDragon () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.3, 0.5 )
		{
			Name = "Un grand dragon enragé";
			Body = Utility.RandomList( 12, 59 );
			BaseSoundID = 362;

			SetStr( 1025 );
			SetDex( 81);
			SetInt( 475 );

			SetHits( 1000 );
			SetStam( 120);

			SetDamage( 24);

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 60 );
			SetResistance( ResistanceType.Fire, 65 );
			SetResistance( ResistanceType.Cold, 40 );
			SetResistance( ResistanceType.Poison, 40 );
			SetResistance( ResistanceType.Energy, 50 );

			SetSkill( SkillName.EvalInt, 110.0 );
			SetSkill( SkillName.Magery, 110.0 );
			SetSkill( SkillName.MagicResist, 110.0);
			SetSkill( SkillName.Tactics, 110.0 );
			SetSkill( SkillName.Wrestling, 115.0 );


			VirtualArmor = 60;

		}

        public override bool ReacquireOnMovement { get { return true; } }
        public override bool HasBreath { get { return true; } } // fire breath enabled
        public override bool AutoDispel { get { return true; } }
        public override bool BardImmune { get { return true; } }

        public override bool OnBeforeDeath()
        {
            this.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "*La bête s'envole, vaincue*");
            this.YellowHealthbar = true;
            Timer.DelayCall(TimeSpan.FromSeconds(3), Delete);
            return false;
        }


        public AngryGreaterDragon(Serial serial)
            : base(serial)
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
