using System;
using Server.Items;
using Server;
using Server.Misc;

namespace Server.Mobiles
{
	public class People : BaseCreature
	{
		[Constructable]
		public People() : this(false)
		{
		}
		
		[Constructable]
		public People (bool isFemale) : base( AIType.AI_None, FightMode.None, 10, 1, 0.2, 0.4 )
		{
			
			InitStats( 30, 30, 30 );

			SpeechHue = Utility.RandomDyedHue();
			Hue = Utility.RandomSkinHue();

			if (Female = isFemale) //  this.Female = Utility.RandomBool()
			{
				this.Body = 0x191;
				this.Name = "une femme";
				AddItem( new Server.Items.Robe( Utility.RandomNeutralHue() ) );
				switch ( Utility.Random ( 2 ) )
				{
						case 0: AddItem( new Skirt ( Utility.RandomNeutralHue() ) ); break;
						case 1: AddItem( new Kilt ( Utility.RandomNeutralHue() ) ); break;
				}
			}
			else
			{
				this.Body = 0x190;
				this.Name = "un homme";
				AddItem( new ShortPants( Utility.RandomNeutralHue() ) );
				AddItem( new Server.Items.Robe( Utility.RandomNeutralHue() ) );
			}

			AddItem( new Boots( Utility.RandomNeutralHue() ) );

			Utility.AssignRandomHair( this );

			Container pack = new Backpack();
			pack.Movable = false;
			AddItem( pack );
		}


		public override bool ClickTitle{ get{ return false; } }

		public People( Serial serial ) : base( serial )
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

    public class PeopleF : People
    {
        [Constructable]
        public PeopleF()
            : base(true)
        {
        }

        public PeopleF( Serial serial ) : base( serial )
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
    }

    public class PeopleM : People
    {
        [Constructable]
        public PeopleM()
            : base(false)
        {
        }

        public PeopleM( Serial serial ) : base( serial )
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
    }
}
