using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0xF62, 0xF63 )]
	public class Spear : BaseSpear
	{
		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ArmorIgnore; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ParalyzingBlow; } }

		public override int AosStrengthReq{ get{ return 50; } }
		public override int AosMinDamage{ get{ return 13; } }
		public override int AosMaxDamage{ get{ return 15; } }
		public override int AosSpeed{ get{ return 42; } }
		public override float MlSpeed{ get{ return 2.75f; } }

		public override int OldStrengthReq{ get{ return 30; } }
		public override int OldMinDamage{ get{ return 2; } }
		public override int OldMaxDamage{ get{ return 36; } }
		public override int OldSpeed{ get{ return 46; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 80; } }

		[Constructable]
		public Spear() : base( 0xF62 )
		{
			Weight = 7.0;
		}

		public Spear( Serial serial ) : base( serial )
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
               && !(defender.Mounted)   // d�fenseur pas sur un cheval
               && ((attacker.Direction & Direction.Running) != 0)   // cours
               && (skill != null && (Utility.Random(120) <= ((int)(skill.Value) + 10)))
               && attacker.CheckTargetSkill(SkillName.Chivalry, defender, 0.0, 120.0)
               )
            {
                attacker.SendMessage("Votre attaque mont�e disloque votre adversaire!");
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