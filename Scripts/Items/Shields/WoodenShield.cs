using System;
using Server;

namespace Server.Items
{
	public class WoodenShield : BaseShield
	{
        #region Mondain's Legacy
        public override int PhysicalResistance { get { return BasePhysicalResistance + GetProtOffset() + GetResourceAttrs().ShieldPhysicalResist + PhysicalBonus; } }
        public override int FireResistance { get { return BaseFireResistance + GetProtOffset() + GetResourceAttrs().ShieldFireResist + FireBonus ; } }
        public override int ColdResistance { get { return BaseColdResistance + GetProtOffset() + GetResourceAttrs().ShieldColdResist + ColdBonus ; } }
        public override int PoisonResistance { get { return BasePoisonResistance + GetProtOffset() + GetResourceAttrs().ShieldPoisonResist + PoisonBonus ; } }
        public override int EnergyResistance { get { return BaseEnergyResistance + GetProtOffset() + GetResourceAttrs().ShieldEnergyResist + EnergyBonus; } }
        #endregion

		public override int InitMinHits{ get{ return 20; } }
		public override int InitMaxHits{ get{ return 25; } }

		public override int AosStrReq{ get{ return 20; } }

		public override int ArmorBase{ get{ return 8; } }

		[Constructable]
		public WoodenShield() : base( 0x1B7A )
		{
			Weight = 5.0;
		}

		public WoodenShield( Serial serial ) : base(serial)
		{
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );//version
		}
	}
}
