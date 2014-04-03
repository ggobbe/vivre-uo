using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a dragon corpse" )]
	public class AngryDragon : BaseCreature
	{
		[Constructable]
		public AngryDragon () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Un dragon Enragé";
			Body = Utility.RandomList( 12, 59 );
			BaseSoundID = 362;

			SetStr( 796 );
			SetDex( 86 );
			SetInt( 436 );

			SetHits( 478 );

			SetDamage( 16 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 55 );
			SetResistance( ResistanceType.Fire, 60 );
			SetResistance( ResistanceType.Cold, 30 );
			SetResistance( ResistanceType.Poison, 25 );
			SetResistance( ResistanceType.Energy, 35 );

			SetSkill( SkillName.EvalInt, 30.1 );
			SetSkill( SkillName.Magery, 30.1 );
			SetSkill( SkillName.MagicResist, 99.1 );
			SetSkill( SkillName.Tactics, 97.6 );
			SetSkill( SkillName.Wrestling, 90.1 );

			VirtualArmor = 60;

		}      

		public override bool ReacquireOnMovement{ get{ return true; } }
		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override bool AutoDispel{ get{ return true; } }
        public override bool BardImmune {get {return true;} }

        public override bool OnBeforeDeath()
        {
            this.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "*La bête s'envole, vaincue*");
            this.YellowHealthbar = true;
            Timer.DelayCall(TimeSpan.FromSeconds(3), Delete);
            return false;
        }


        public AngryDragon(Serial serial)
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