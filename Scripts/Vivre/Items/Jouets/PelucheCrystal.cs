using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public class PelucheCrystal : Item
	{
        [Constructable]
        public PelucheCrystal()       : base(0x25FB)
		{
			Name = "Crystal la barbare!";
			LootType = LootType.Newbied;
			Weight = 1.0;
		}

        public PelucheCrystal(Serial serial)       : base(serial)
		{
		}

    public override void OnDoubleClick( Mobile from )
		{
            switch (Utility.Random(10))
                {
				default:
                case 0: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Ragisacam tu es un homme mort..."); break;
                case 1: from.PlaySound(0x005F); break;
                case 2: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Non c'est pas un bug..."); break;
                case 3: from.PlaySound(0x0069); break;
                case 4: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Non ça on ne fait pas!!!"); break;
                case 5: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Vous avez peté un plomb là"); break;
                case 6: from.PlaySound(0x006C); break;
                case 7: this.PublicOverheadMessage(MessageType.Regular, 0, false, "C'est en test là"); break;
                case 8: from.PlaySound(0x0085); break;
                case 9: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Encore un OUIN OUIN OUIN"); break;
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
