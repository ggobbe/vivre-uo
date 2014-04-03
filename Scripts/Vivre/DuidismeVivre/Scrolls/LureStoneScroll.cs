using System;	
using Server;
using Server.Items;

namespace Server.Items
{
   public class LureStoneScroll : SpellScroll
   {
      [Constructable]
      public LureStoneScroll() : this( 1 )
      {
      }

      [Constructable]
      public LureStoneScroll( int amount ) : base( 312, 0xE39 )
      {
         Name = "Lure Stone";
         Hue = 0x58B;
      }

      public LureStoneScroll( Serial serial ) : base( serial )
      {
      
      }

      public override void Serialize( GenericWriter writer )
      {
         base.Serialize( writer );

         writer.Write( (int) 0 ); // version
      }

      public override void Deserialize( GenericReader reader )
      {
         base.Deserialize( reader );

         int version = reader.ReadInt();
      }
   }
}
