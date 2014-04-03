using System;
using Server.Network;
using Server.Targeting;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0xF52, 0xF51 )]
	public class TrainingDagger : BaseKnife
	{
		public override int AosStrengthReq{ get{ return 10; } }
		public override int AosMinDamage{ get{ return 2; } }
		public override int AosMaxDamage{ get{ return 4; } }
		public override int AosSpeed{ get{ return 56; } }
		public override float MlSpeed{ get{ return 3.00f; } }

		public override int OldStrengthReq{ get{ return 1; } }
		public override int OldMinDamage{ get{ return 3; } }
		public override int OldMaxDamage{ get{ return 15; } }
		public override int OldSpeed{ get{ return 55; } }

		public override int InitMinHits{ get{ return 15; } }
		public override int InitMaxHits{ get{ return 20; } }

		public override SkillName DefSkill{ get{ return SkillName.Fencing; } }
		public override WeaponType DefType{ get{ return WeaponType.Piercing; } }
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Pierce1H; } }

		[Constructable]
		public TrainingDagger() : base( 0xF52 )
		{
			Weight = 1.0;
            Name = "Dague d'entrainement";
		}

        public TrainingDagger(Serial serial)
            : base(serial)
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