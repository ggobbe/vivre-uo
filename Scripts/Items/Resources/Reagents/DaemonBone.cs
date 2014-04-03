using System;
using Server;
using Server.Items;

namespace Server.Items
{
    // Scriptiz : Ajout du ICommodity
    public class DaemonBone : BaseReagent, ICommodity
    {
        // Scriptiz : Ajout du ICommodity
        string Description
        {
            get
            {
                return String.Format("{0} daemon's bone", Amount);
            }
        }

        int ICommodity.DescriptionNumber { get { return LabelNumber; } }
        bool ICommodity.IsDeedable { get { return true; } }

		public override double DefaultWeight
		{
			get { return 1.0; }
		}

		[Constructable]
		public DaemonBone() : this( 1 )
		{
		}

		[Constructable]
		public DaemonBone( int amount ) : base( 0xF80, amount )
		{
		}

		public DaemonBone( Serial serial ) : base( serial )
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