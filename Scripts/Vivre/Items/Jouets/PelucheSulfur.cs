using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public class PelucheSulfur : Item
	{
        [Constructable]
        public PelucheSulfur()       : base(0x25FB)
		{
			Name = "Aventurier audacieux";
			LootType = LootType.Newbied;
			Weight = 1.0;
		}

        public PelucheSulfur(Serial serial)       : base(serial)
		{
		}

    public override void OnDoubleClick( Mobile from )
		{
            switch (Utility.Random(5))
                {
				default:
                case 0: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Tu oses me defier !"); break;
                case 1: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Je vais faire un petit tour a Hythloth,moi !"); break;
                case 2: this.PublicOverheadMessage(MessageType.Regular, 0, false, "J'ai 1444 décès !"); break;
                case 3: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Prend donc ca dans ta tronche ridicule vermine"); break;
                case 4: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Arretons les blabla passons a l'action ! aux armes !"); break;
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
