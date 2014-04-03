using System;

namespace Server.Items
{
	public abstract class MilkBottle : Item
	{
		private Mobile m_Poisoner;
		private Poison m_Poison;
		private int m_FillFactor;
		
		public virtual Item EmptyItem{ get { return null; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Poisoner
		{
			get { return m_Poisoner; }
			set { m_Poisoner = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Poison Poison
		{
			get { return m_Poison; }
			set { m_Poison = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int FillFactor
		{
			get { return m_FillFactor; }
			set { m_FillFactor = value; }
		}
		
		public MilkBottle( int itemID ) : base( itemID )
		{
			this.FillFactor = 4;
		}
		
		public MilkBottle( Serial serial ) : base( serial )
		{
		}
		
		public void Boire( Mobile from )
		{
			if ( soif( from, m_FillFactor ) )
			{
				// Play a random drinking sound
				from.PlaySound( Utility.Random( 0x30, 2 ) );
				
				if ( from.Body.IsHuman && !from.Mounted )
					from.Animate( 34, 5, 1, true, false, 0 );
				
				if ( m_Poison != null )
					from.ApplyPoison( m_Poisoner, m_Poison );
				
				this.Consume();
				
				Item item = EmptyItem;
				
				if ( item != null )
					from.AddToBackpack( item );
			}
		}
		
		static public bool soif( Mobile from, int fillFactor )
		{
			if ( from.Thirst >= 20 )
			{
				from.SendMessage( "ah pu soif!" );
				return false;
			}
			
			int iThirst = from.Thirst + fillFactor;
			if ( iThirst >= 20 )
			{
				from.Thirst = 20;
				from.SendMessage( "c'est trop bon , mais la vous pouvez plus!" );
			}
			else
			{
				from.Thirst = iThirst;
				
				if ( iThirst < 5 )
					from.SendMessage( "Vous buvez cela , mais avez encore très très soif." );
				else if ( iThirst < 10 )
					from.SendMessage( "Vous buvez et vous sentez un peu mieux." );
				else if ( iThirst < 15 )
					from.SendMessage( "Apres avoir vu , vous vous senter bcp mieux." );
				else
					from.SendMessage( "Apres cette boisson , vous etes comblé." );
			}
			
			return true;
		}
		
		
		
		public override void OnDoubleClick( Mobile from )
		{
			if ( !Movable )
				return;
			
			if ( from.InRange( this.GetWorldLocation(), 1 ) )
				Boire( from );
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 1 ); // version
			
			writer.Write( m_Poisoner );
			
			Poison.Serialize( m_Poison, writer );
			writer.Write( m_FillFactor );
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
			
			switch ( version )
			{
				case 1:
				{
					m_Poisoner = reader.ReadMobile();

					goto case 0;
				}
				case 0:
				{
					m_Poison = Poison.Deserialize( reader );
					m_FillFactor = reader.ReadInt();
					break;
				}
			}
		}
	}
	public class BottleCowMilk : MilkBottle
	{
		public override Item EmptyItem{ get { return new Bottle(); } }
		
		[Constructable]
		public BottleCowMilk() : base( 0x0f09 )
		{
			this.Weight = 0.2;
			this.FillFactor = 4;
			this.Name ="Bouteille de lait de vache";
		}
		
		public BottleCowMilk( Serial serial ) : base( serial )
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
	public class BottleGoatMilk : MilkBottle
	{
		public override Item EmptyItem{ get { return new Bottle(); } }
		
		[Constructable]
		public BottleGoatMilk() : base( 0x0f09 )
		{
			this.Weight = 0.2;
			this.FillFactor = 4;
			this.Name ="Bouteille de lait de chevre";
		}
		
		public BottleGoatMilk( Serial serial ) : base( serial )
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
	public class BottleSheepMilk : MilkBottle
	{
		public override Item EmptyItem{ get { return new Bottle(); } }
		
		[Constructable]
		public BottleSheepMilk() : base( 0x0f09 )
		{
			this.Weight = 0.2;
			this.FillFactor = 4;
			this.Name ="Bouteille de lait de brebis";
		}
		
		public BottleSheepMilk( Serial serial ) : base( serial )
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


/* ***************************** Cheese ******************************** */



    // Cow Cheese

    public class CowCheese : Food, ICarvable
	{
		public void Carve( Mobile from, Item item )
		{
			if ( !Movable )
				return;

			if ( this.Amount > 1 )  // workaround because I can't call scissorhelper twice?
			{
				from.SendMessage( "Vous ne pouvez decouper que une meule a la fois." );
				return;
			}

			base.ScissorHelper( from, new CowCheeseWedge(), 1 );

			from.AddToBackpack( new CowCheeseWedgeSmall() );

			from.SendMessage( "Vous decouper un morceau de la meule." );
		}

		[Constructable]
		public CowCheese() : this( 1 )
		{
		}

		[Constructable]
		public CowCheese( int amount ) : base( amount, 0x97E )
		{
			this.Weight = 0.4;
			this.FillFactor = 12;
			this.Name = "Gouda";
			this.Hue = 0x481;
		}
		
		public CowCheese( Serial serial ) : base( serial )
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

    public class CowCheeseWedge : Food, ICarvable
	{
		public void Carve( Mobile from, Item item )
		{
			if ( !Movable )
				return;

			base.ScissorHelper( from, new CowCheeseWedgeSmall(), 3 );
			from.SendMessage( "Vous coupez la meule en 3 morceaux" );
		}

		[Constructable]
		public CowCheeseWedge() : this( 1 )
		{
		}

		[Constructable]
		public CowCheeseWedge( int amount ) : base( amount, 0x97D )
		{
			this.Weight = 0.3;
			this.FillFactor = 9;
			this.Name = "Gruyere";
			this.Hue = 0x481;
		}

		public CowCheeseWedge( Serial serial ) : base( serial )
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

    public class CowCheeseWedgeSmall : Food
	{
		[Constructable]
		public CowCheeseWedgeSmall() : this( 1 )
		{
		}

		[Constructable]
		public CowCheeseWedgeSmall( int amount ) : base( amount, 0x97C )
		{
			this.Weight = 0.1;
			this.FillFactor = 3;
			this.Name = "Gruyere";
			this.Hue = 0x481;
		}

		public CowCheeseWedgeSmall( Serial serial ) : base( serial )
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



    // Sheep Cheese

    public class SheepCheese : Food, ICarvable
	{
		public void Carve( Mobile from, Item item )
		{
			if ( !Movable )
				return;

			if ( this.Amount > 1 )  // workaround because I can't call scissorhelper twice?
			{
				from.SendMessage( "Vous ne pouvez decouper que 1 meule a la fois" );
				return;
			}

			base.ScissorHelper( from, new SheepCheeseWedge(), 1 );

			from.AddToBackpack( new SheepCheeseWedgeSmall() );

			from.SendMessage( "Vous decouper un morceau de la meule." );
		}

		[Constructable]
		public SheepCheese() : this( 1 )
		{
		}
		
		[Constructable]
		public SheepCheese( int amount ) : base( amount, 0x97E )
		{
			this.Weight = 0.4;
			this.FillFactor = 12;
			this.Name = "Le brebis d'Igor";
			this.Hue = 0x481;
		}
		
		public SheepCheese( Serial serial ) : base( serial )
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

    public class SheepCheeseWedge : Food, ICarvable
	{
		public void Carve( Mobile from, Item item )
		{
			if ( !Movable )
				return;

			base.ScissorHelper( from, new SheepCheeseWedgeSmall(), 3 );
			from.SendMessage( "Vous coupez la meule en 3 morceaux" );
		}

		[Constructable]
		public SheepCheeseWedge() : this( 1 )
		{
		}
		
		[Constructable]
		public SheepCheeseWedge( int amount ) : base( amount, 0x97D )
		{
			this.Weight = 0.3;
			this.FillFactor = 9;
			this.Name = "Haut paturage";
			this.Hue = 0x481;
		}
		
		public SheepCheeseWedge( Serial serial ) : base( serial )
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

    public class SheepCheeseWedgeSmall : Food
	{
		[Constructable]
		public SheepCheeseWedgeSmall() : this( 1 )
		{
		}
		
		[Constructable]
		public SheepCheeseWedgeSmall( int amount ) : base( amount, 0x97C )
		{
			this.Weight = 0.1;
			this.FillFactor = 3;
			this.Name = "Fromage de brebis";
			this.Hue = 0x481;
		}
		
		public SheepCheeseWedgeSmall( Serial serial ) : base( serial )
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



    // Goat Cheese

    public class GoatCheese : Food, ICarvable
	{
		public void Carve( Mobile from, Item item )
		{
			if ( !Movable )
				return;

			if ( this.Amount > 1 )  // workaround because I can't call scissorhelper twice?
			{
				from.SendMessage( "Vous ne pouvez decouper que 1 meule a la fois" );
				return;
			}

			base.ScissorHelper( from, new GoatCheeseWedge(), 1 );

            from.AddToBackpack( new GoatCheeseWedgeSmall() );

			from.SendMessage( "Vous decouper un morceau de la meule." );
		}

		[Constructable]
		public GoatCheese() : this( 1 )
		{
		}
		
		[Constructable]
		public GoatCheese( int amount ) : base( amount, 0x97E )
		{
			this.Weight = 0.4;
			this.FillFactor = 12;
			this.Name = "LE chevre de Yew";
			this.Hue = 0x481;
		}
		
		public GoatCheese( Serial serial ) : base( serial )
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

    public class GoatCheeseWedge : Food, ICarvable
	{
		public void Carve( Mobile from, Item item )
		{
			if ( !Movable )
				return;

            base.ScissorHelper(from, new GoatCheeseWedgeSmall(), 3);
			from.SendMessage( "Vous coupez la meule en 3 morceaux" );
		}

		[Constructable]
		public GoatCheeseWedge() : this( 1 )
		{
		}
		
		[Constructable]
		public GoatCheeseWedge( int amount ) : base( amount, 0x97D )
		{
			this.Weight = 0.3;
			this.FillFactor = 9;
			this.Name = "Chevrotin de Cove";
			this.Hue = 0x481;
		}
		
		public GoatCheeseWedge( Serial serial ) : base( serial )
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

	public class GoatCheeseWedgeSmall : Food
	{
		[Constructable]
		public GoatCheeseWedgeSmall() : this( 1 )
		{
		}
		
		[Constructable]
		public GoatCheeseWedgeSmall( int amount ) : base( amount, 0x97C )
		{
			this.Weight = 0.1;
			this.FillFactor = 3;
			this.Name = "fromage de chevre";
			this.Hue = 0x481;
		}

        public GoatCheeseWedgeSmall(Serial serial)
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
