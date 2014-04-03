using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x26BE, 0x26C8 )]
	public class Pike : BaseSpear
	{
		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ParalyzingBlow; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.InfectiousStrike; } }

		public override int AosStrengthReq{ get{ return 50; } }
		public override int AosMinDamage{ get{ return 14; } }
		public override int AosMaxDamage{ get{ return 16; } }
		public override int AosSpeed{ get{ return 37; } }
		public override float MlSpeed{ get{ return 3.00f; } }

		public override int OldStrengthReq{ get{ return 50; } }
		public override int OldMinDamage{ get{ return 14; } }
		public override int OldMaxDamage{ get{ return 16; } }
		public override int OldSpeed{ get{ return 37; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 110; } }

		[Constructable]
		public Pike() : base( 0x26BE )
		{
			Weight = 8.0;
		}

		public Pike( Serial serial ) : base( serial )
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

        /*** ADDED ***/
        // ALAMBIK
        // Modification implementation of mounted chivalry
        public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
        {
            Skill skill = attacker.Skills[SkillName.Chivalry];
            if (attacker.Mounted    // attaquant sur un cheval
               && !(defender.Mounted)   // défenseur pas sur un cheval
               && ((attacker.Direction & Direction.Running) != 0)   // cours
               && (skill != null && (Utility.Random(120) <= ((int)(skill.Value) + 10)))
               && attacker.CheckTargetSkill(SkillName.Chivalry, defender, 0.0, 120.0)
               )
            {
                attacker.SendMessage("Votre attaque montée disloque votre adversaire!");
                defender.PlaySound(1308);
                base.OnHit(attacker, defender, 1.20);   // bonus 1/5 au lieu de 1/4
            }
            else
            {
                base.OnHit(attacker, defender, 1.0);
            }
        }
        /*** END ***/
	}
}