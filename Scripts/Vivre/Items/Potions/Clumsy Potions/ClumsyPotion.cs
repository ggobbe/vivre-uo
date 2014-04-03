using System;
using Server;

namespace Server.Items
{
	public class ClumsyPotion : BaseAgilityPotion
	{
        public override int DexOffset { get { return IntensifiedStrength ? 9 : 6; } }
		public override TimeSpan Duration{ get{ return TimeSpan.FromMinutes( IntensifiedTime ? 2.5:1.5); } }

		[Constructable]
		public ClumsyPotion() : base( PotionEffect.Clumsy )
		{
		}

        public ClumsyPotion(Serial serial)
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