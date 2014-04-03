using System;

namespace Server.Items
{
	[FlipableAttribute(4153)]
	public class Sacter : Item
	{
		[Constructable]
		public Sacter() : base(4153)
		{
			Weight = 10.0;
			Name = "Sac de terre";
			Hue = 0x907;
		}

		public Sacter(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			if ( Weight == 6.0 )
				Weight = 10.0;
		}
	}
}	