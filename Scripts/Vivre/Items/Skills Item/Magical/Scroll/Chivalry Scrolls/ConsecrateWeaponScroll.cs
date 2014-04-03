using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class ConsecrateWeaponScroll : SpellScroll
	{
		[Constructable]
		public ConsecrateWeaponScroll() : this( 1 )
		{
		}

		[Constructable]
		public ConsecrateWeaponScroll( int amount ) : base( 202, 0x227C, amount )
		{
            Name = "Consecrate Weapon Scroll";
		}

        public ConsecrateWeaponScroll(Serial serial)
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