using System;
using Server;

namespace Server.Items
{
    // Scriptiz : on en fait un r�actif pour la n�cro
    // public class FertileDirt : Item
    public class FertileDirt : BaseReagent, ICommodity
    {
        // Scriptiz : ajout du ICommodity
        string Description
        {
            get
            {
                return String.Format("{0} fertile dirt", Amount);
            }
        }

        int ICommodity.DescriptionNumber { get { return LabelNumber; } }
        bool ICommodity.IsDeedable { get { return true; } }

		[Constructable]
		public FertileDirt() : this( 1 )
		{
		}

		[Constructable]
		public FertileDirt( int amount ) : base( 0xF81 )
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
		}

		public FertileDirt( Serial serial ) : base( serial )
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