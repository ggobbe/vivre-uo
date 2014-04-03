using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public class PelucheBarde : Item
	{
        [Constructable]
        public PelucheBarde()       : base(0x25FB)
		{
			Name = "Le Barde";
			LootType = LootType.Newbied;
			Weight = 1.0;
		}

        public PelucheBarde(Serial serial)
            : base(serial)
		{
		}

    public override void OnDoubleClick( Mobile from )
		{
            switch (Utility.Random(5))
                {
				default:
                case 0: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Euh je suis timide"); break;
                case 1: from.PlaySound(0x004C); break;
                case 2: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Euh desolé c'est ma fille.."); break;
                case 3: from.PlaySound(0x020B); break;
                case 4: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Je sais pas je vais demander à ma femme."); break;
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
