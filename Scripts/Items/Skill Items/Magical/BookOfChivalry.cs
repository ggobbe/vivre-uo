using System;
using Server.Network;
using Server.Spells;

namespace Server.Items
{
	public class BookOfChivalry : Spellbook
	{
		public override SpellbookType SpellbookType{ get{ return SpellbookType.Paladin; } }
		public override int BookOffset{ get{ return 200; } }
		public override int BookCount{ get{ return 10; } }

		[Constructable]
		public BookOfChivalry() : this( (ulong)0 )
		{
		}

		[Constructable]
		public BookOfChivalry( ulong content ) : base( content, 0x2252 )
		{
			Layer = (Core.ML ? Layer.OneHanded : Layer.Invalid);
            Lootable = false;
            Stealable = false;
		}

		public BookOfChivalry( Serial serial ) : base( serial )
		{
		}

        // Scriptiz : le livre de palouf ne peut pas quitter le sac ou le joueur
        public override bool Nontransferable
        {
            get { return true; }
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if( version == 0 && Core.ML )
				Layer = Layer.OneHanded;
		}
	}
}