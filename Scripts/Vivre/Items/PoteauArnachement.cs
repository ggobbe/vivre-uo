/* 
 * MODIFICATIONS
 * 
 * NUMÉRO       	DATE        AUTEUR
 * ------       ----------      ------
 * #01         	2007-06-30      Merlock
 *    > L'animal devient invul
 * #02			2008-03-08		Gargouille
 * 	> le temps passé attaché ne compte plus pour le Bonding
 * #03			2008-03-13		Merlock
 * 	> les poteaux de villes !!!
 * #04 partage du pet entre lesmobs de l'account //Corrected by Plume
 * */
using System;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using Server.Commands;
using Server.Multis;
using Server.Accounting;//#04

namespace Server.Items
{
	public class LPTarget : Target
	{
		public LPTarget()
			: base( 5, false, TargetFlags.None )
		{
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			if ( targeted is PoteauArnachement )
			{
				PoteauArnachement pa = (PoteauArnachement)targeted;
				BaseHouse bh = BaseHouse.FindHouseAt( pa );
				if ( pa == null || bh == null || ( !bh.IsOwner( from ) && !bh.IsCoOwner( from ) ) || pa.Animal == null )
					from.SendMessage( "Vous devez cibler un poteau avec une monture attachée qui est sur votre terrain !!" );
				else
				{
					pa.Libere( null );
					from.SendMessage( "La monture s'en va d'elle-même" );
				}
			}
			else
				from.SendMessage( "Vous devez cibler un poteau" );
		}
	}

	public class CPTarget : Target
	{
		public CPTarget()
			: base( 5, false, TargetFlags.None )
		{
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			if ( targeted is PoteauArnachement && !( targeted is PoteauDeVille ) )
			{
				PoteauArnachement pa = (PoteauArnachement)targeted;
				PoteauDeVille pdv = new PoteauDeVille();
				pdv.Location = pa.Location;
				pdv.Map = pa.Map;
				pdv.Movable = false;
				pdv.Animal = pa.Animal;
				pdv.Maitre = pa.Maitre;
				pdv.Loyaute = pa.Loyaute;
				pdv.AI = pa.AI;
				pa.Delete();
				from.SendMessage( "Le poteau est maintenant un poteau de ville" );
			}
			else
				from.SendMessage( "Ceci n'est pas un poteau de joueur" );
		}
	}
	
	[FlipableAttribute( 0x14E7, 0x14E8 )]
	public class PoteauDeVille : PoteauArnachement
	{
		public new static void Initialize()
		{
			CommandSystem.Register( "ConvertPoteau", AccessLevel.Counselor, new CommandEventHandler( ConvertPoteau_OnCommand ) );
		}

		private static void ConvertPoteau_OnCommand( CommandEventArgs e )
		{
			e.Mobile.SendMessage( "Vous devez cibler un poteau de joueur a convertir" ); //You have to choose a mobile
			e.Mobile.Target = new CPTarget();
		}
		
		private DateTime m_TempsRestant;
		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime TempsRestant
		{
			get
			{
				return m_TempsRestant;
			}
			set
			{
				m_TempsRestant = value;
			}
		}
		
		[Constructable]
		public PoteauDeVille()
		{
		}

		public PoteauDeVille( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version

			writer.Write( (DateTime)m_TempsRestant );
			
			if(Animal != null && m_TempsRestant + TimeSpan.FromHours(12) < DateTime.Now)
				Libere(null);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_TempsRestant = reader.ReadDateTime();
		}
	}
	
	[FlipableAttribute( 0x14E7, 0x14E8 )]
	public class PoteauArnachement : Item
	{
		public  static void Initialize()
		{
			CommandSystem.Register( "LiberePoteau", AccessLevel.Player, new CommandEventHandler( LiberePoteau_OnCommand ) );
		}

		private static void LiberePoteau_OnCommand( CommandEventArgs e )
		{
			e.Mobile.SendMessage( "Vous devez cibler un poteau" ); //You have to choose a mobile
			e.Mobile.Target = new LPTarget();
		}
		
		private Mobile m_Maitre;
		private BaseCreature m_Animal;
		private int m_Loyaute;
		private AIType m_AI;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Maitre
		{
			get
			{
				return m_Maitre;
			}
			set
			{
				m_Maitre = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public BaseCreature Animal
		{
			get
			{
				return m_Animal;
			}
			set
			{
				m_Animal = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int Loyaute
		{
			get
			{
				return m_Loyaute;
			}
			set
			{
				m_Loyaute = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public AIType AI
		{
			get
			{
				return m_AI;
			}
			set
			{
				m_AI = value;
			}
		}

		[Constructable]
		public PoteauArnachement()
			: base( 0x14E8 )
		{
			Weight = 10.0;
			Name = "Poteau d'arnachement";
		}

		public PoteauArnachement( Serial serial )
			: base( serial )
		{
		}

		public void Libere( Mobile from )
		{
			m_Animal.Blessed = false; // Ajout #01

			/*
			//***Ajout #02 début
			if ( m_Animal.IsBondable && !m_Animal.IsBonded && ( m_Animal.BondingBegin != DateTime.MinValue ) )
			{
				TimeSpan inactive = (TimeSpan)( DateTime.Now - m_Animal.LastStable );
				m_Animal.BondingBegin += inactive;
			}
			m_Animal.LastStable = DateTime.MinValue;
			//***Ajout #02 fin
			 * */

			if ( from == null )
			{
				m_Animal.Tamable = true;
				m_Animal.Controlled = false;
				m_Animal.Loyalty = m_Loyaute;
				m_Animal.AI = m_AI;
				m_Animal.ControlOrder = OrderType.Release;
				m_Animal = null;
			}
			else
			{
				m_Animal.Tamable = true;
				m_Animal.Controlled = true;
				m_Animal.ControlMaster = m_Maitre;
				m_Animal.Loyalty = m_Loyaute;
				m_Animal.AI = m_AI;
				m_Animal.ControlTarget = m_Maitre;
				m_Animal.ControlOrder = OrderType.Follow;
				m_Animal = null;
				from.Say( "*détache sa monture*" );
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
            if (this.Movable)
            {
                //from.SendMessage("Le poteau doit être fixé au sol pour pouvoir l'utiliser");

                // Scriptiz : on ajoute le fait qu'un double clic lock l'item si il est devant la maison
                BaseHouse house = BaseHouse.FindHouseAt(this.Location, this.Map, 16);   // dans la maison

                if (house == null) house = BaseHouse.FindHouseAt(new Point3D(this.X, this.Y - 1, this.Z), this.Map, 16);    // devant la maison
                if (house == null) house = BaseHouse.FindHouseAt(new Point3D(this.X - 1, this.Y, this.Z), this.Map, 16);    // à droite de la maison

                // Pas de maison trouvée
                if (house == null)
                {
                    from.SendMessage("Pour fixer le poteau au sol il doit se trouver dans ou devant une maison.");
                    return;
                }

                // pas propriétaire de la maison
                if (!house.IsOwner(from) || !house.IsCoOwner(from))
                {
                    from.SendMessage("Ce poteau ne peux pas être placé dans ou devant une maison qui ne vous appartient pas.");
                    return;
                }

                this.Movable = false;
                from.SendMessage("Le poteau a été fixé au sol, vous pouvez désormais l'utiliser.");
                from.SendMessage("Pour le retirer double cliquer dessus puis ciblez le poteau.");
            }
            else if (!from.InRange(this.GetWorldLocation(), 2))
                from.SendLocalizedMessage(500486);   //That is too far away.
            else if (Animal != null)
            {
                if (m_Animal.Deleted)
                    m_Animal = null;
                else
                {
                    if (Maitre == null || Maitre.Deleted)
                    {
                        Libere(null);
                        from.SendMessage("La monture s'en va d'elle-même");
                    }
                    else if (from == m_Maitre || CheckAccount(from))
                    {
                        //if (from.Skills[SkillName.AnimalTaming].Base < m_Animal.MinTameSkill && m_Animal.MinTameSkill > 29.1)//#04
                            //from.SendMessage("Vous n'êtes pas en mesure de controler cette créature.");

                        if (from.Followers + m_Animal.ControlSlots > from.FollowersMax)
                            from.SendMessage("Vous avez trop de suivant pour détacher cette créature.");
                        else
                        {
                            // Scriptiz : amélioration des control chance // Plume : On retire cette condition, inutile...
                           /* if (!(m_Animal is BaseMount) && !m_Animal.CheckControlChance(from))
                                from.SendMessage("Vous n'êtes pas en mesure de controler cette créature.");
                            else*/
                                Libere(from);
                        }
                    }
                    else
                        from.SendMessage("Ce poteau est déjà utilisé par {0}", m_Maitre.Name);
                }
            }
            else
            {
                from.Target = new PoteauArnachementTarget(this);
                from.SendMessage("Quel animal voulez vous attacher ?");
            }
		}

		//#04
		private bool CheckAccount(Mobile from)
		{
			Account a = Maitre.Account as Account;

			if ( a != null )
			{
				for ( int i = 0; i < a.Length; ++i )
				{
					if ( a[i] == from )
					{
						m_Maitre = a[i];
						return true;
					}
				}
			}
			
			return false;
		}
		//
		
		private class PoteauArnachementTarget : Target
		{
			private PoteauArnachement m_Poteau;

			public PoteauArnachementTarget( PoteauArnachement poteau )
				: base( 1, true, TargetFlags.None )
			{
				m_Poteau = poteau;
			}

			protected override void OnTarget( Mobile from, object targ )
			{
				if ( !( m_Poteau.Deleted ) )
				{
                    // Scriptiz : on ajoute le fait qu'un double clic délock l'item si on le cible lui même
                    if (targ is PoteauArnachement)
                    {
                        if (targ != m_Poteau)
                        {
                            from.SendMessage("Vous devez cibler le même poteau pour le retirer du sol.");
                            return;
                        }

                        BaseHouse house = BaseHouse.FindHouseAt(m_Poteau.Location, m_Poteau.Map, 16);   // dans la maison

                        if (house == null) house = BaseHouse.FindHouseAt(new Point3D(m_Poteau.X, m_Poteau.Y - 1, m_Poteau.Z), m_Poteau.Map, 16);    // devant la maison
                        if (house == null) house = BaseHouse.FindHouseAt(new Point3D(m_Poteau.X - 1, m_Poteau.Y, m_Poteau.Z), m_Poteau.Map, 16);    // à droite de la maison

                        // pas propriétaire de la maison
                        if (house != null && (!house.IsOwner(from) || !house.IsCoOwner(from)))
                        {
                            from.SendMessage("Vous ne pouvez pas détacher un poteau d'une maison qui ne vous appartient pas.");
                            return;
                        }

                        // Pas de maison trouvée
                        if (house == null)
                            from.SendMessage("Le poteau n'est plus lié a une maison et vous pouvez donc le retirer du sol.");
                        else
                            from.SendMessage("Le poteau a été retiré du sol, vous pouvez désormais le ramasser.");

                        m_Poteau.Movable = true;

                        return;
                    }

					if ( targ is BaseMount || targ is PackHorse || targ is PackLlama )
					{
						BaseCreature creature = targ as BaseCreature;

						if ( ( creature.Controlled && creature.ControlMaster == from ) )
						{
							m_Poteau.Animal = creature;
							m_Poteau.Maitre = from;
							m_Poteau.Loyaute = creature.Loyalty;
							m_Poteau.AI = creature.AI;
                            creature.AI = AIType.AI_None;
							//creature.Controlled = false;
							creature.Tamable = false;
							creature.ControlMaster = null;
							creature.Blessed = true; // Ajout #01

							/*
							//***Ajout #02 début
							if ( creature.IsBondable && !creature.IsBonded && ( creature.BondingBegin != DateTime.MinValue ) )
							{
								creature.LastStable = DateTime.Now;
							}
							//***Ajout #02 fin
							 */

							if ( m_Poteau is PoteauDeVille )
								( (PoteauDeVille)m_Poteau ).TempsRestant = DateTime.Now;
							
							from.Say( "*attache sa monture au poteau*" );
						}
						else if ( creature.AI == AIType.AI_None )
						{
							from.SendMessage( "Cette creature est déjà attachée !" );
						}
						else
						{
							from.SendMessage( "Cette creature n'est pas sous votre controle !" );
						}
					}
					else if ( targ is PlayerMobile )
					{
						Mobile m = (Mobile)targ;

						if ( m == from )
						{
							from.Say( "*a essaye de s'attacher au poteau*" );
						}
						else
						{
							from.Say( "*a essaye d'attacher {0}, mais n'a pas reussi", m.Name );
						}
					}
					else
					{
						from.SendMessage( "Cette creature n'est pas une monture !" );
					}
				}
				return;
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version

			writer.Write( (Mobile)m_Animal );
			writer.Write( (Mobile)m_Maitre );
			writer.Write( (int)m_Loyaute );
			writer.Write( (int)m_AI );
			
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
					{
						m_Animal = (BaseCreature)reader.ReadMobile();
						m_Maitre = reader.ReadMobile();
						m_Loyaute = reader.ReadInt();
						m_AI = (AIType)reader.ReadInt();
						break;
					}
			}
		}
	}
}
