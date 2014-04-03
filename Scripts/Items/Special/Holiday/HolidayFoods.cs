using System;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;

namespace Server.Items
{
	public class BaseCandyCane  : Food
	{
		private static Dictionary<Mobile, CandyCaneTimer> m_ToothAches;

		public static Dictionary<Mobile, CandyCaneTimer> ToothAches
		{
			get { return m_ToothAches; }
			set { m_ToothAches = value; }
		}

		public static void Initialize()
		{
			m_ToothAches = new Dictionary<Mobile, CandyCaneTimer>();
		}

		public class CandyCaneTimer : Timer
		{
			private int m_Eaten;
			private Mobile m_Eater;

			public Mobile Eater { get { return m_Eater; } }
			public int Eaten { get { return m_Eaten; } set { m_Eaten = value; } }

			public CandyCaneTimer( Mobile eater ) 
				: base( TimeSpan.FromSeconds( 30 ), TimeSpan.FromSeconds( 30 ) )
			{
				m_Eater = eater;
				Priority = TimerPriority.FiveSeconds;
				Start();
			}

            private string ToothAchesMsg()
            {
            int msg = Utility.Random(5);

            switch (msg)
            {
                case 0: return "ARRGH! Ma dent me fait mal!";
                case 1: return "Y a-t-il un dentiste parmis vous?";
                case 2: return "Ma dent!!!";
                case 3: return "Ça fait trop mal, Maman!";
                case 4: return "Qu'on m'arrache cette foutue dent!";
            }
            return "ARRGH! Ma dent me fait mal!";
            }
            protected override void OnTick()
            {
                --m_Eaten;

                if (m_Eater == null || m_Eater.Deleted || m_Eaten <= 0)
                {
                    Stop();
                    m_ToothAches.Remove(m_Eater);
                }
                else if (m_Eater.Map != Map.Internal && m_Eater.Alive)
                {
                    if (m_Eaten > 60)
                    {
                        m_Eater.Say(1077388 + Utility.Random(5));
                        /* ARRGH! My tooth hurts sooo much!
                         * You just can't find a good Britannian dentist these days...
                         * My teeth!
                         * MAKE IT STOP!
                         * AAAH! It feels like someone kicked me in the teeth!
                         */

                        if (Utility.RandomBool() && m_Eater.Body.IsHuman && !m_Eater.Mounted)
                            m_Eater.Animate(32, 5, 1, true, false, 0);
                    }
                    else if (m_Eaten == 60)
                    {
                        m_Eater.SendMessage("La douleur s'estompe"); // The extreme pain in your teeth subsides.
                    }
                }
            }
        }

        private static CandyCaneTimer EnsureTimer(Mobile from)
        {
            CandyCaneTimer timer;

            if (!m_ToothAches.TryGetValue(from, out timer))
                m_ToothAches[from] = timer = new CandyCaneTimer(from);

            return timer;
        }

        public static int GetToothAche(Mobile from)
        {
            CandyCaneTimer timer;

            if (m_ToothAches.TryGetValue(from, out timer))
                return timer.Eaten;

            return 0;
        }

        public static void SetToothAche(Mobile from, int value)
        {
            EnsureTimer(from).Eaten = value;
        }


		public BaseCandyCane( int itemID ) : this( 1, itemID )
		{
		}

        public BaseCandyCane(int amount, int itemID)
            : base(itemID)
		{
            Stackable = false;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( IsChildOf( from.Backpack ) || from.InRange(this, 1) )
			{
				from.PlaySound( 0x3a + Utility.Random(3) ); 
				from.Animate( 34, 5, 1, true, false, 0 );

				if ( !ToothAches.ContainsKey( from ) )
				{
					ToothAches.Add( from, new CandyCaneTimer( from ) );
				}

				ToothAches[from].Eaten += 32;

				from.SendMessage( "Ceci est si bon que vous pourriez en manger à l'infini" ); // You feel as if you could eat as much as you wanted!
				Delete();
			}
		}

        public BaseCandyCane(Serial serial) : base(serial)
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

    public class RedCandyCane : BaseCandyCane
    {

        [Constructable]
		public RedCandyCane() : this( 1 )
		{
		}

		[Constructable]
        public RedCandyCane(int amount) : base(amount, 0x2bdd)
		{
            ItemID = 0x2bdd + Utility.Random(1);
            Name = "Canne aux fruits";
		}

        public RedCandyCane(Serial serial)
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
    }

    public class GreenCandyCane : BaseCandyCane
    {
        [Constructable]
        public GreenCandyCane()
            : this(1)
        {
        }

        [Constructable]
        public GreenCandyCane(int amount)
            : base(amount, 0x2bdf)
        {
            ItemID = 0x2bdf + Utility.Random(1);
            Name = "Canne à la limette";
        }

        public GreenCandyCane(Serial serial)
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
    }

	public class GingerBreadCookie : Food
	{
		private readonly int[] m_Messages = 
		{ 
			0, 
			1077396, // Noooo!
			1077397, // Please don't eat me... *whimper*
			1077405, // etc etc etc ..
			1077406, 
			1077407, 
			1077408, 
			1077409
		};

        private string GingerBreadMsg()
        {
            int msg = Utility.Random(7);

            switch (msg)
            {
                case 0: return "Noooooooon!";
                case 1: return "Je t'en supplie ne me mange pas! *Gémit*";
                case 2: return "Pas la tête! Pas la tête!";
                case 3: return "Ahhh! Mon pied! Ça fait mal!";
                case 4: return "S'il vous plait, j'ai des enfants biscuits!";
                case 5: return "Je suis empoisonné! Vraiment...";
                case 6: return "Je me sauverai! Je suis SuperÉpice!";
            }
            return "Noooooooon!";
        }

		[Constructable]
		public GingerBreadCookie() : base( 0x2be1 )
		{
			ItemID = Utility.RandomBool() ? 0x2be1 : 0x2be2;
			Stackable = false;
            Name = Utility.RandomBool() ? "Homme en pain d'epices" : "Femme en pain d'epices";
		}

		public GingerBreadCookie( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( IsChildOf( from.Backpack ) || from.InRange(this, 1) )
			{
                if (Utility.Random(8) == 7)
                    base.OnDoubleClick( from );
                else
                    this.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, GingerBreadMsg()); ;
				/*int result;
                if ((result = m_Messages[Utility.Random(0,m_Messages.Length)]) == 0)
				{
					base.OnDoubleClick( from );
				}
				else
				{
					this.SendLocalizedMessageTo( from, result );
				}*/
			}
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

