using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public class PelucheElfe : Item
	{
        [Constructable]
		public PelucheElfe() : base( 0x2D8A )
		{
			Name = "L'Elfe des Bois";
			LootType = LootType.Newbied;
			Weight = 1.0;
		}
	
        public PelucheElfe( Serial serial ) : base( serial )
		{
		}

    public override void OnDoubleClick( Mobile from )
		{
            switch (Utility.Random(5))
                {
				default:
				case 0:	this.PublicOverheadMessage( MessageType.Regular, 0, false,  "Je suis votre bien aimé" ); break;
				case 1: this.PublicOverheadMessage( MessageType.Regular, 0, false,  "Je brule pour vous" ); break;
				case 2: this.PublicOverheadMessage( MessageType.Regular, 0, false,  "Emmenez-moi dans les bois" ); break;
				case 3: this.PublicOverheadMessage( MessageType.Regular, 0, false,  "La vie est si courte! Il faut en profiter!" ); break;
                case 4: this.PublicOverheadMessage( MessageType.Regular, 0, false,  "Mon dieu que vous êtes beau!" ); break;
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
