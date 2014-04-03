using System;
using Server.Targeting;

namespace Server.Items
{
    [FlipableAttribute(0xF43, 0xF44)]
    public class ThrowingAxe : BaseWeapon
    {
        public override WeaponAbility PrimaryAbility { get { return WeaponAbility.ArmorIgnore; } }
        public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Disarm; } }

        public override int AosStrengthReq { get { return 20; } }
        public override int AosDexterityReq { get { return 30; } }
        public override int AosMinDamage { get { return 9; } }
        public override int AosMaxDamage { get { return 13; } }
        public override int AosSpeed { get { return 41; } }
        public override float MlSpeed { get { return 3.00f; } }

        public override int OldStrengthReq { get { return 15; } }
        public override int OldMinDamage { get { return 2; } }
        public override int OldMaxDamage { get { return 17; } }
        public override int OldSpeed { get { return 40; } }

        public override int InitMinHits { get { return 31; } }
        public override int InitMaxHits { get { return 50; } }

        public override int DefHitSound { get { return 0x232; } }
        public override int DefMissSound { get { return 0x23A; } }

        public override SkillName DefSkill { get { return SkillName.Swords; } }
        public override WeaponType DefType { get { return WeaponType.Axe; } }
        public override WeaponAnimation DefAnimation { get { return WeaponAnimation.Slash2H; } }


        [Constructable]
        public ThrowingAxe()
            : base(0xF43)
        {
            Weight = 4.0;
            Name = "Une hache de jet";
            Layer = Layer.OneHanded;
        }

        public ThrowingAxe(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            // Scriptiz : gestion du double clic pour équipper un objet
            if (from.FindItemOnLayer(this.Layer) != this)
            {
                base.OnDoubleClick(from);
                return;
            }
            if (from.Skills[SkillName.Throwing].Value <= 41)
            {
                from.SendMessage("Vous seriez incapable de toucher votre cible, mieux vaut renoncer");
                return;
            }
            from.SendMessage("Où voulez-vous la lancer?");
            InternalTarget t = new InternalTarget(this);
            from.Target = t;
            return;

        }

        private class InternalTarget : Target
        {
            private ThrowingAxe m_Axe;

            public InternalTarget(ThrowingAxe axe)
                : base(10, false, TargetFlags.Harmful)
            {
                m_Axe = axe;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Axe.Deleted)
                {
                    return;
                }

                else if (!from.Items.Contains(m_Axe))
                {
                    from.SendMessage("You must be holding that weapon to use it.");
                }


                else if (targeted is Mobile)
                {
                    Mobile m = (Mobile)targeted;

                    if (m != from && from.HarmfulCheck(m))
                    {
                        Direction to = from.GetDirectionTo(m);

                        from.Direction = to;

                        from.Animate(from.Mounted ? 26 : 9, 7, 1, true, false, 0);

                        if (from.CheckTargetSkill(SkillName.Throwing, m, 40.0, 100.0))
                        {
                            from.MovingEffect(m, 0x1BFE, 7, 1, false, false, 0x481, 0);

                            int distance = (int)from.GetDistanceToSqrt(m.Location);

                            int mindamage = m_Axe.MinDamage;
                            if (from.Dex > 100)
                                mindamage += 2;

                            distance -= (int)from.Skills[SkillName.Tactics].Value / 20;
                            if (distance < 0)
                                distance = 0;

                            int count = (int)from.Skills[SkillName.Throwing].Value / 10;
                            count += (int)from.Skills[SkillName.Anatomy].Value / 20;
                            if (distance > 6)
                                count -= distance - 5;

                            AOS.Damage(m, from,Utility.Random(mindamage,count) - distance/2, true,0,0,0,0,0,0,100,false,false,false);

                            m_Axe.MoveToWorld(m.Location, m.Map);
                        }
                        else
                        {
                            int x = 0, y = 0;

                            switch (to & Direction.Mask)
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

                            x += Utility.Random(-1, 3);
                            y += Utility.Random(-1, 3);

                            x += m.X;
                            y += m.Y;

                            m_Axe.MoveToWorld(new Point3D(x, y, m.Z), m.Map);

                            from.MovingEffect(m_Axe, 0x1BFE, 7, 1, false, false, 0x481, 0);

                           
                            

                            from.SendMessage("You miss.");
                        }
                        m_Axe.HitPoints -= 1;
                    }
                    
                }
            }
        }
    }
}