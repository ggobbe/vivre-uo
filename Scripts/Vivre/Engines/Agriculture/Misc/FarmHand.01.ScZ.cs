using System;
using System.Collections.Generic;
using Server;

namespace Server.Mobiles
{
	public class FarmHand : BaseVendor
	{
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

		[Constructable]
		public FarmHand() : base("Le Fermier")
		{
			SetSkill( SkillName.Lumberjacking, 80.0, 100.0 );
			SetSkill( SkillName.TasteID, 80.0, 100.0 );
            Female = false;
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBFarmHand() );
		}

		public override VendorShoeType ShoeType
		{
			get{ return VendorShoeType.Sandals; }
		}

		public override int GetShoeHue()
		{
			return 0;
		}

		public override void InitOutfit()
		{
			base.InitOutfit();

			AddItem( new Server.Items.WideBrimHat( Utility.RandomNeutralHue() ) );
		}

		public FarmHand( Serial serial ) : base( serial )
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
