using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public class PelucheToubazar : Item
	{
        [Constructable]
        public PelucheToubazar()       : base(0x25FB)
		{
			Name = "Le Magnifique";
			LootType = LootType.Newbied;
			Weight = 1.0;
			//Couleur a introduire.
		}

        public PelucheToubazar(Serial serial)       : base(serial)
		{
		}

    public override void OnDoubleClick( Mobile from )
		{
            switch (Utility.Random(9))
                {
				default:
                case 0: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Comment osez vous penetrer dans l'intimite de ma chambre ! Sortez, Sortez !"); break;
                case 1: this.PublicOverheadMessage(MessageType.Regular, 0, false, "C'est agacant, hein ?"); break;
                case 2: this.PublicOverheadMessage(MessageType.Regular, 0, false, "NON!! N'essayez pas de m'approcher !! vous avez des PUCES !!"); break;
                case 3: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Regardez, je peux danser.. et courrrrrirrrr.. Sauter ! ET VOUS TUER !!!"); break;
                case 4: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Quoi ? Le Grand Toubazar ne peut plus regaler le monde de sa grace ?! Intolerable !!"); break;
                case 5: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Des murs...  DES MURS !!! j'en ai horreur, c'est a cause d'eux qu'on a besoin de portes !!!"); break;
                case 6: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Laissez moi vous dire, vos gouts vestimentaires sont ridicules !"); break;
                case 7: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Tiens un lapin !"); break;
                case 8: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Decidement, je suis incroyable !! Ah, qu'il est bon d'etre MOI !! ahaha !"); break;
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
