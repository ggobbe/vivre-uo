using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class PelucheBlackBear : Item
	{
        [Constructable]
		public PelucheBlackBear() : base( 0x2118 )
		{
			Name = "Ben";
			LootType = LootType.Newbied;
			Weight = 1.0;
		}

        public PelucheBlackBear(Serial serial)  : base(serial)
		{
		}

    public override void OnDoubleClick( Mobile from )
		{
            from.PlaySound(0x005f);
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
	

       