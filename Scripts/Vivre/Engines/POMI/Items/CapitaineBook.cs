using System;
using System.Collections;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.IPOMI
{
	public class CapitaineBook : Item
	{
		private TownStone m_town;
		
		public CapitaineBook(TownStone town) : base(0xEFA)
		{
			m_town = town;
			LootType = LootType.Newbied;
			Weight = 0;
			Name = "Livre du Capitaine";
			Hue = town.Hue;
		}

		public CapitaineBook( Serial serial ) : base( serial )
		{
		}
		
		public override void OnDoubleClick(Mobile from)
		{
			if(m_town.Capitaine == (PlayerMobile)from)
			{
				from.SendGump(new CapitaineBookGump((PlayerMobile)from, m_town, this));
			}
			else
				from.SendMessage("Vous ne pouvez pas utiliser ce livre");
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
			writer.Write( (TownStone)m_town );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_town = (TownStone)reader.ReadItem();
		}
	}
}
