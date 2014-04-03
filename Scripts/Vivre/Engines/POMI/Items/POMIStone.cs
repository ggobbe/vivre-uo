using System;
using Server.Network;
using System.Collections;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server.IPOMI
{
	public class POMI : Item
	{
		private ArrayList m_Villes;

        // Scriptiz : possibilité de changer la hue avec un .set hue xxx tout en impactant les changements sur tout le pomi
        public override int Hue
        {
            get
            {
                return base.Hue;
            }
            set
            {
                base.Hue = value;

            }
        }
		
		[Constructable]
		public POMI() : base( 0xED4 )
		{
			Movable = false;
			m_Villes = new ArrayList();
			Hue = 0x489;
			Name = "POMI";
		}
		
		public POMI( Serial serial ) : base( serial )
		{
		}
		
		[CommandProperty( AccessLevel.GameMaster )] 
		public ArrayList Villes
		{	
			get{return m_Villes;}
		}
		
		[CommandProperty( AccessLevel.GameMaster )] 
		public int NbVilles
		{
			get{return m_Villes.Count;}
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			
			writer.WriteItemList( m_Villes, true );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			m_Villes = reader.ReadItemList();
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if ( from.AccessLevel >= AccessLevel.Administrator )
			{
				TownStone town = new TownStone(this);
				from.Backpack.DropItem(town);
				m_Villes.Add(town);
				from.Backpack.DropItem(new TownBox(town));
				foreach(TownStone othertown in m_Villes)
				{
					if(othertown != town )
						othertown.Neutre.Add(town);
					town.Neutre.Add(othertown);
				}
				from.SendAsciiMessage( 0x482, "Nouvelle pierre de ville creee" );
			}
			else
			{
				from.SendMessage( "Vous n'avez pas les acces pour administrer POMI" );
			}
		}

        /**
         * Scriptiz : divers ajouts pour le POMI
         */
        // Scriptiz : méthode pour trouver la pierre POMI principale (celle qui a le plus de villes)
        public static POMI FindPomi()
        {
            POMI mainPomi = null;

            foreach (Item i in World.Items.Values)
            {
                if (i is POMI)
                {
                    if (mainPomi == null) 
                        mainPomi = (POMI)i;
                    else if (((POMI)i).Villes.Count > mainPomi.Villes.Count)
                        mainPomi = (POMI)i;
                }
            }
            return mainPomi;
        }

        // Scriptiz : méthode pour vérifier si un point se trouve à l'intérieur de la ville d'un PJ
        public static bool IsPointInPlayerTown(PlayerMobile from, Point2D location)
        {
            if (from == null) return false;

            ArrayList villesPomi = POMI.FindPomi().Villes;
            TownStone ts = null;
            foreach (object o in villesPomi)
            {
                if (o is TownStone)
                    ts = (TownStone)o;

                if (ts != null)
                {
                    if (ts.Citoyens.Contains(from) && ts.Map == from.Map)
                    {
                        double distance = Math.Sqrt(Math.Pow(ts.Location.X - location.X, 2) + Math.Pow(ts.Location.Y - location.Y, 2));
                        if (distance > ts.MaxDistance)
                        {
                            from.SendMessage("Vous ne pouvez pas bâtir en dehors de votre ville.");
                            return false;
                        }
                    }
                }
            } 
            return true;
        }

        // Méthode pour vérifier qu'un joueur soit bien citoyen d'une ville donnée
        public static bool IsPlayerCitizenOf(Mobile from, string town)
        {
            if (from == null) return false;

            POMI thePomi = POMI.FindPomi();
            if (thePomi == null) return false;

            foreach (object v in thePomi.Villes)
            {
                if(v == null || !(v is TownStone)) continue;

                TownStone ville = (TownStone)v;

                if (ville.Nom.ToLower() == town.ToLower())
                    return ville.Citoyens.Contains(from);
            }
            return false;
        }
	}
}
