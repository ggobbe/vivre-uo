using System;
using Server.Network;
using Server.Spells;

namespace Server.Items
{
	public class BookOfBushido : Spellbook
	{
		public override SpellbookType SpellbookType{ get{ return SpellbookType.Samurai; } }
		public override int BookOffset{ get{ return 400; } }
		public override int BookCount{ get{ return 6; } }

		[Constructable]
		public BookOfBushido() : this( (ulong)0x00)
		{
		}

		[Constructable]
		public BookOfBushido( ulong content ) : base( content, 0x238C )
		{
			Layer = (Core.ML ? Layer.OneHanded : Layer.Invalid);
            Lootable = false;
            Stealable = false;
		}

		public BookOfBushido( Serial serial ) : base( serial )
		{
		}

        // Scriptiz : le livre de bushi ne peut pas quitter le sac ou le joueur
        public override bool Nontransferable
        {
            get { return true; }
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if( version == 0 && Core.ML )
				Layer = Layer.OneHanded;

            if (ItemID == 0)
                ItemID = 0x238C;
		}
	}
}