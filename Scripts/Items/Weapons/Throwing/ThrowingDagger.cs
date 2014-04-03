using System;
using Server.Targeting;
using Server.Network;

namespace Server.Items
{
	[FlipableAttribute( 0xF52, 0xF51 )]
    public class ThrowingDagger : BaseWeapon
	{
		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.InfectiousStrike; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ShadowStrike; } }

		public override int AosStrengthReq{ get{ return 10; } }
        public override int AosDexterityReq { get { return 30; } }
		public override int AosMinDamage{ get{ return 6; } }
        public override int AosMaxDamage { get { return 8; } }
		public override int AosSpeed{ get{ return 56; } }
		public override float MlSpeed{ get{ return 2.25f; } }

		public override int OldStrengthReq{ get{ return 1; } }
		public override int OldMinDamage{ get{ return 3; } }
		public override int OldMaxDamage{ get{ return 15; } }
		public override int OldSpeed{ get{ return 55; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 40; } }

		public override SkillName DefSkill{ get{ return SkillName.Fencing; } }
		public override WeaponType DefType{ get{ return WeaponType.Piercing; } }
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Pierce1H; } }

        public override int DefHitSound { get { return 0x23B; } }
        public override int DefMissSound { get { return 0x238; } }


		public override string DefaultName
		{
			get { return "Une dague de jet"; }
		}

		[Constructable]
		public ThrowingDagger() : base( 0xF52 )
		{
			Weight = 1.0;
			Layer = Layer.OneHanded;
		}

		public ThrowingDagger( Serial serial ) : base( serial )
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

		public override void OnDoubleClick( Mobile from )
		{
            // Scriptiz : gestion du double clic pour équipper un objet
            if (from.FindItemOnLayer(this.Layer) != this)
            {
                base.OnDoubleClick(from);
                return;
            }
                from.SendMessage("Où voulez-vous la lancer?");
				InternalTarget t = new InternalTarget( this );
				from.Target = t;
                return;
			    
		}

		private class InternalTarget : Target
		{
			private ThrowingDagger m_Dagger;

			public InternalTarget( ThrowingDagger dagger ) : base( 10, false, TargetFlags.Harmful )
			{
				m_Dagger = dagger;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Dagger.Deleted )
				{
					return;
				}
				else if ( !from.Items.Contains( m_Dagger ) )
				{
					from.SendMessage( "You must be holding that weapon to use it." );
				}


				else if ( targeted is Mobile )
				{
					Mobile m = (Mobile)targeted;

					if ( m != from && from.HarmfulCheck( m ) )
					{
						Direction to = from.GetDirectionTo( m );

						from.Direction = to;

						from.Animate( from.Mounted ? 26 : 9, 7, 1, true, false, 0 );


						if ( from.CheckTargetSkill( SkillName.Throwing, m, 0.0, 60.00 ) )
						{
							from.MovingEffect( m, 0x1BFE, 7, 1, false, false, 0x481, 0 );

                            int distance = (int)from.GetDistanceToSqrt(m.Location);

                            int mindamage = m_Dagger.MinDamage;
                            if (from.Dex > 100)
                                mindamage += 2;

                            distance -= (int)from.Skills[SkillName.Tactics].Value / 20;
                            if (distance < 0)
                                distance = 0;

                            int count = (int)from.Skills[SkillName.Throwing].Value / 10;
                            count += (int)from.Skills[SkillName.Anatomy].Value / 20;
                            if (distance > 6)
                                count -= distance - 5;

                            AOS.Damage(m, from, Utility.Random(mindamage, count) - distance / 2, true, 0, 0, 0, 0, 0, 0, 100, false, false, false);

							m_Dagger.MoveToWorld( m.Location, m.Map );
						}
						else
						{
							int x = 0, y = 0;

							switch ( to & Direction.Mask )
							{
								case Direction.North: --y; break;
								case Direction.South: ++y; break;
								case Direction.West: --x; break;
								case Direction.East: ++x; break;
								case Direction.Up: --x; --y; break;
								case Direction.Down: ++x; ++y; break;
								case Direction.Left: --x; ++y; break;
								case Direction.Right: ++x; --y; break;
							}

							x += Utility.Random( -1, 3 );
							y += Utility.Random( -1, 3 );

							x += m.X;
							y += m.Y;

							m_Dagger.MoveToWorld( new Point3D( x, y, m.Z ), m.Map );

							from.MovingEffect( m_Dagger, 0x1BFE, 7, 1, false, false, 0x481, 0 );
                            
                            m_Dagger.HitPoints -= 1; 
							
                            from.SendMessage( "You miss." );
						}
                        m_Dagger.HitPoints -= 1; 
					}
                } 
			}
		}
	}
}