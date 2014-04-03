using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public class PelucheDrow : Item
	{
        [Constructable]
        public PelucheDrow()       : base(0x25A4)
		{
			Name = "L'Elfe Noire";
			LootType = LootType.Newbied;
			Weight = 1.0;
		}

        public PelucheDrow(Serial serial)            : base(serial)
		{
		}

    public override void OnDoubleClick( Mobile from )
		{
            switch (Utility.Random(5))
                {
				default:
                case 0: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Je suis votre pire cauchemar"); break;
                case 1: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Venez vous battre!"); break;
                case 2: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Vous brulerez en enfer"); break;
                case 3: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Vous n'avez pas du Blackrock?"); break;
                case 4: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Par tous le chaos que vous êtes stupide"); break;
			}

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
