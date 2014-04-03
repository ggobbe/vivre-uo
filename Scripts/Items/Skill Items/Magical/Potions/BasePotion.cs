using System;
using Server;
using Server.Engines.Craft;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Items
{
	public enum PotionEffect
	{
		Nightsight, // 0
		CureLesser, // 1
		Cure, // 2
		CureGreater, // 3
		Agility, // 4 
		AgilityGreater, // 5
		Strength, // 6
		StrengthGreater, // 7
		PoisonLesser, // 8
		Poison, // 9
		PoisonGreater, // 10
        PoisonDeadly, // 11
        Refresh, // 12
        RefreshTotal, // 13
        HealLesser, // 14
        Heal, // 15
        HealGreater, // 16
        ExplosionLesser, // 17
        Explosion, // 18
        ExplosionGreater, // 19
        Conflagration, // 20
        ConflagrationGreater, // 21
        MaskOfDeath, // 22		// Mask of Death is not available in OSI but does exist in cliloc files
        MaskOfDeathGreater, // 23	// included in enumeration for compatability if later enabled by OSI
        ConfusionBlast, // 24
        ConfusionBlastGreater, // 25
        //Ajout Myron Gender Potion
        GenderSwap, // 26
        FrogMorph, // 27
        Invisibility, // 28
        Parasitic, // 29
        Darkglow, // 30
        Hallucinogen, //31
        Clumsy,
        ClumsyGreater
	}

	public abstract class BasePotion : Item, ICraftable, ICommodity
	{
        public virtual bool CIT { get { return false; } }
        public virtual bool CIS { get { return false; } }

        private bool m_IntensifiedTime;
        
        [CommandProperty(AccessLevel.GameMaster)]
        public bool IntensifiedTime
        {
			get
			{
				return m_IntensifiedTime;
			}
			set
			{
				m_IntensifiedTime = value;
			}
		}

        private bool m_IntensifiedStrength;
        [CommandProperty(AccessLevel.GameMaster)]
        public bool IntensifiedStrength
        {
			get
			{
				return m_IntensifiedStrength;
			}
			set
			{
				m_IntensifiedStrength = value;
			}
		}

        private PotionEffect m_PotionEffect;

		public PotionEffect PotionEffect
		{
			get
			{
				return m_PotionEffect;
			}
			set
			{
				m_PotionEffect = value;
				InvalidateProperties();
			}
		}

		int ICommodity.DescriptionNumber { get { return LabelNumber; } }
		bool ICommodity.IsDeedable { get { return (Core.ML); } }

		public override int LabelNumber{ get{ return 1041314 + (int)m_PotionEffect; } }

		public BasePotion( int itemID, PotionEffect effect ) : base( itemID )
		{
			m_PotionEffect = effect;

			Stackable = Core.ML;
			Weight = 1.0;
		}

		public BasePotion( Serial serial ) : base( serial )
		{
		}

		public virtual bool RequireFreeHand{ get{ return true; } }

		public static bool HasFreeHand( Mobile m )
		{
			Item handOne = m.FindItemOnLayer( Layer.OneHanded );
			Item handTwo = m.FindItemOnLayer( Layer.TwoHanded );

			if ( handTwo is BaseWeapon )
				handOne = handTwo;

            if (handTwo is BaseRanged)
            {
                BaseRanged ranged = (BaseRanged)handTwo;

				if ( ranged.Balanced )
					return true;
			}

			return ( handOne == null || handTwo == null );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !Movable )
				return;

			if ( from.InRange( this.GetWorldLocation(), 1 ) )
			{
				if (!RequireFreeHand || HasFreeHand(from))
				{
					if (this is BaseExplosionPotion && Amount > 1)
					{
						BasePotion pot = (BasePotion)Activator.CreateInstance(this.GetType());

						if (pot != null)
						{
							Amount--;

							if (from.Backpack != null && !from.Backpack.Deleted)
							{
								from.Backpack.DropItem(pot);
							}
							else
							{
								pot.MoveToWorld(from.Location, from.Map);
							}
							pot.Drink( from );
						}
					}
					else
					{
						this.Drink( from );
					}
				}
				else
				{
					from.SendLocalizedMessage(502172); // You must have a free hand to drink a potion.
				}
			}
			else
			{
				from.SendLocalizedMessage( 502138 ); // That is too far away for you to use
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 ); // version

            writer.Write((bool)m_IntensifiedTime);
            writer.Write((bool)m_IntensifiedStrength);
			writer.Write( (int) m_PotionEffect );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
                case 2:
                    {
                        m_IntensifiedTime = reader.ReadBool();
                        m_IntensifiedStrength = reader.ReadBool();
                        
                        goto case 1;
                    }
				case 1:
				case 0:
				{
					m_PotionEffect = (PotionEffect)reader.ReadInt();
					break;
				}
			}

			if( version ==  0 )
				Stackable = Core.ML;
		}

        public abstract void Drink(Mobile from);      
        
		public static void PlayDrinkEffect( Mobile m )
		{
			m.RevealingAction();

			m.PlaySound( 0x2D6 );

            #region Dueling
            if (!Engines.ConPVP.DuelContext.IsFreeConsume(m))
                m.AddToBackpack(new Bottle());
            #endregion

			//m.AddToBackpack( new Bottle() );

			if ( m.Body.IsHuman && !m.Mounted )
				m.Animate( 34, 5, 1, true, false, 0 );
		}

		public static int EnhancePotions( Mobile m )
		{
			int EP = AosAttributes.GetValue( m, AosAttribute.EnhancePotions );
			int skillBonus = m.Skills.Alchemy.Fixed / 330 * 10;

			if ( Core.ML && EP > 50 && m.AccessLevel <= AccessLevel.Player )
				EP = 50;

			return ( EP + skillBonus );
		}

		public static TimeSpan Scale( Mobile m, TimeSpan v )
		{
			if ( !Core.AOS )
				return v;

			double scalar = 1.0 + ( 0.01 * EnhancePotions( m ) );

			return TimeSpan.FromSeconds( v.TotalSeconds * scalar );
		}

		public static double Scale( Mobile m, double v )
		{
			if ( !Core.AOS )
				return v;

			double scalar = 1.0 + ( 0.01 * EnhancePotions( m ) );

			return v * scalar;
		}

		public static int Scale( Mobile m, int v )
		{
			if ( !Core.AOS )
				return v;

			return AOS.Scale( v, 100 + EnhancePotions( m ) );
		}

		public override bool StackWith( Mobile from, Item dropped, bool playSound )
		{
			if( dropped is BasePotion && ((BasePotion)dropped).m_PotionEffect == m_PotionEffect )
				return base.StackWith( from, dropped, playSound );

			return false;
		}

		#region ICraftable Members

		public int OnCraft( int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue )
		{
			if ( craftSystem is DefAlchemy )
			{
				Container pack = from.Backpack;

				if ( pack != null )
				{
                    if ((int)PotionEffect >= (int)PotionEffect.Invisibility)
                        return 1;

					List<PotionKeg> kegs = pack.FindItemsByType<PotionKeg>();

					for ( int i = 0; i < kegs.Count; ++i )
					{
						PotionKeg keg = kegs[i];

						if ( keg == null )
							continue;

						if ( keg.Held <= 0 || keg.Held >= 100 )
							continue;

						if ( keg.Type != PotionEffect )
							continue;

						++keg.Held;

						Consume();
						from.AddToBackpack( new Bottle() );

						return -1; // signal placed in keg
					}
				}
			}

			return 1;
		}

		#endregion
	}
}