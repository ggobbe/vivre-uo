using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public class PoupeeFemale : Item
	{
        [Constructable]
        public PoupeeFemale()
            : base(0x2107)
		{
            Name = NameList.RandomName("female");
			Weight = 1.0;
		}

        public PoupeeFemale(Serial serial)
            : base(serial)
		{
		}

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 2) || !from.InLOS(this))
            {
                from.SendLocalizedMessage(501816);
                return;
            }


            switch (Utility.Random(6))
            {
                default:
                case 0: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Oh non! Mes ongles!"); break;
                case 1: this.PublicOverheadMessage(MessageType.Regular, 0, false, "J'ai une amie qui... oh non... je ne suis pas une commère comme elle"); break;
                case 2: this.PublicOverheadMessage(MessageType.Regular, 0, false, "S'il te plaît, laisse-moi faire le ménage!"); break;
                case 3: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Mon rêve? La paix dans le monde!"); break;
                case 4: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Quand je serai grande, je veux un mari pour qui cuisiner!"); break;
                case 5: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Arrête, j'ai mal à la tête!"); break;

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
