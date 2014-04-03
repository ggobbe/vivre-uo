using System;

namespace Server.Items
{
    [FlipableAttribute(0x11EA, 0x11EB)]
	public class Sugar : Item
	{

		[Constructable]
        public Sugar() : base(0x11EA)
		{
			Movable = true;
			Stackable = true;
            Name = "Sucre";
            Hue = 0x3E9;
            Amount = Utility.Random(1, 4);
		}

        public Sugar(Serial serial)
            : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
