using System;
using Server.Items;

namespace Server.Items
{
    // Scriptiz : on le transforme en réactif
    // public class Bone : Item, ICommodity
    public class Bone : BaseReagent, ICommodity
	{
        string Description
        {
            get
            {
                return String.Format(Amount == 1 ? "{0} bone" : "{0} bones", Amount);
            }
        }

		int ICommodity.DescriptionNumber { get { return LabelNumber; } }
		bool ICommodity.IsDeedable { get { return true; } }

		[Constructable]
		public Bone() : this( 1 )
		{
		}

		[Constructable]
		public Bone( int amount ) : base( 0xf7e )
		{
			Stackable = true;
			Amount = amount;
			Weight = 1.0;
		}

		public Bone( Serial serial ) : base( serial )
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