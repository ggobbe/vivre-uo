using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a drake corpse" )]
	public class AngryDrake : BaseCreature
	{
		[Constructable]
		public AngryDrake () : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Un drake enragé";
			Body = Utility.RandomList( 60, 61 );
			BaseSoundID = 362;

			SetStr( 401 );
			SetDex( 133 );
			SetInt( 101 );

			SetHits( 241);

			SetDamage( 11 );

			SetDamageType( ResistanceType.Physical, 80 );
			SetDamageType( ResistanceType.Fire, 20 );

			SetResistance( ResistanceType.Physical, 45 );
			SetResistance( ResistanceType.Fire, 50 );
			SetResistance( ResistanceType.Cold, 40 );
			SetResistance( ResistanceType.Poison, 20 );
			SetResistance( ResistanceType.Energy, 30 );

			SetSkill( SkillName.MagicResist, 65.1 );
			SetSkill( SkillName.Tactics, 65.1 );
			SetSkill( SkillName.Wrestling, 65.1 );

			VirtualArmor = 46;


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

        public AngryDrake(Serial serial)
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