using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public abstract class BaseStrengthPotion : BasePotion
	{
        public override bool CIT { get { return true; } }
        public override bool CIS { get { return true; } }

		public abstract int StrOffset{ get; }
		public abstract TimeSpan Duration{ get; }

		public BaseStrengthPotion( PotionEffect effect ) : base( 0xF09, effect )
		{
		}

		public BaseStrengthPotion( Serial serial ) : base( serial )
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

		public bool DoStrength( Mobile from )
		{
            //Plume : Addiction
            if (from is PlayerMobile)
            {
                PlayerMobile drinker = from as PlayerMobile;

                double CurrentAddiction = drinker.CalculateStrengthAddiction(this)[0];
                double GlobalAddiction = drinker.CalculateStrengthAddiction(this)[1];
                int StrScalar = (int)Math.Floor(Math.Sqrt(CurrentAddiction));
                double DurationScalar = GlobalAddiction * 0.95;

                if (GlobalAddiction > 100)
                {
                    drinker.SendMessage("Votre corps ne supporte plus ce traitement");
                    drinker.Str--;
                    this.Consume();
                    return false;
                }

                if (Spells.SpellHelper.AddStatOffset(from, StatType.Str, Scale(from, StrOffset - Math.Min(StrOffset, StrScalar)), Duration - TimeSpan.FromSeconds(DurationScalar)))
                {
                    from.FixedEffect(0x375A, 10, 15);
                    from.PlaySound(0x1E7);
                    return true;
                }
                drinker.IncAddiction(this);
            }

            if (Spells.SpellHelper.AddStatOffset(from, StatType.Dex, Scale(from, StrOffset), Duration))
            {
                from.FixedEffect(0x375A, 10, 15);
                from.PlaySound(0x1E7);
                return true;
            }

			from.SendLocalizedMessage( 502173 ); // You are already under a similar effect.
			return false;
		}

		public override void Drink( Mobile from )
		{
			if ( DoStrength( from ) )
			{
				BasePotion.PlayDrinkEffect( from );

                if (!Engines.ConPVP.DuelContext.IsFreeConsume(from))
                    this.Consume();
			}
		}
	}
}