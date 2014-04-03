using System;
using Server.Network;
using Server.Spells;

namespace Server.Items
{
	public class BookOfNinjitsu : Spellbook
	{
		public override SpellbookType SpellbookType{ get{ return SpellbookType.Ninja; } }
		public override int BookOffset{ get{ return 500; } }
		public override int BookCount{ get{ return 8; } }


		[Constructable]
		public BookOfNinjitsu() : this( (ulong)0x00 )
		{
		}

		[Constructable]
        public BookOfNinjitsu(ulong content) : base(content, 0x23A0)
		{
			Layer = (Core.ML ? Layer.OneHanded : Layer.Invalid);
            Lootable = false;
            Stealable = false;
		}

		public BookOfNinjitsu( Serial serial ) : base( serial )
		{
		}

        // Scriptiz : le livre de ninji ne peut pas quitter le sac ou le joueur
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