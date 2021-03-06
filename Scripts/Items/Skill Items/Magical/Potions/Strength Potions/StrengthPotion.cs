using System;
using Server;

namespace Server.Items
{
	public class StrengthPotion : BaseStrengthPotion
	{
        public override int StrOffset { get { return IntensifiedStrength ? 14 : 8; } }
        public override TimeSpan Duration { get { return TimeSpan.FromMinutes(IntensifiedTime ? 2.5 : 1.5); } }

		[Constructable]
		public StrengthPotion() : base( PotionEffect.Strength )
		{
		}

		public StrengthPotion( Serial serial ) : base( serial )
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