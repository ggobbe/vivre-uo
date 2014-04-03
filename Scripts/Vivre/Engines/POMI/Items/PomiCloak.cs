using System;
using Server.Items;
using Server.Mobiles;

namespace Server.IPOMI
{
	[FlipableAttribute( 0x1515, 0x1530 )]
	public class PomiCloak : BaseCloak
	{
		private TownStone m_town;
		private string m_titre = null;
		
		public PomiCloak(TownStone town, string titre) : base(0x1515)
		{
			m_town = town;
			m_titre = titre;
			LootType = LootType.Newbied;
			Weight = 0;
			Name = titre;
			Hue = town.Hue;
		}
		
		public TownStone Town
		{
			get{ return m_town; }
		}
		
		public PomiCloak( Serial serial ) : base( serial )
		{
		}
		
		public override bool OnEquip(Mobile from)
		{
			/*try
			{
				if((m_town.Maire == (PlayerMobile)from) && m_titre == "Bourgmestre");
				if((m_town.Conseiller == (PlayerMobile)from) && m_titre == "Conseiller");
				if((m_town.Ambassadeur == (PlayerMobile)from) && m_titre == "Ambassadeur");
				if((m_town.Capitaine == (PlayerMobile)from) && m_titre == "Capitaine");
				if(m_town.Gardes.Contains((PlayerMobile)from) && m_titre == "Garde");
			}
			catch{}
			if(m_titre != null)
			{
				from.Title = m_titre + " de " + m_town.Nom;
				//m_titre = null;
				return true;
			}
			else
			{
				from.SendMessage("Vous ne pouvez pas porter cette cape");
				return false;
			}*/
			bool Ok = false;
			
			if(m_town.Maire != null)
				if((m_town.Maire == (PlayerMobile)from) && m_titre == "Bourgmestre")
					Ok = true;
			if(m_town.Conseiller != null)
				if((m_town.Conseiller == (PlayerMobile)from) && m_titre == "Conseiller")
					Ok =  true;
			if(m_town.Ambassadeur != null)
				if((m_town.Ambassadeur == (PlayerMobile)from) && m_titre == "Ambassadeur")
					Ok =  true;
			if(m_town.Capitaine != null)
				if((m_town.Capitaine == (PlayerMobile)from) && m_titre == "Capitaine")
					Ok =  true;
			if(m_town.Gardes.Contains((PlayerMobile)from) && m_titre == "Garde")
				Ok =  true;
			
			if(Ok)
			{
				from.Title = m_titre + " de " + m_town.Nom;
				return true;
			}	
			
			from.SendMessage("Vous ne pouvez pas porter cette cape");
			return false;
		}
		
		public override void OnRemoved( object parent )
      		{
		         if ( parent is Mobile )         
		            ((Mobile)parent).Title = null;
      		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 );
			writer.Write( (TownStone)m_town );
			writer.Write((string)m_titre);
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_town = (TownStone)reader.ReadItem();
			m_titre = reader.ReadString();
		}
	}
}
