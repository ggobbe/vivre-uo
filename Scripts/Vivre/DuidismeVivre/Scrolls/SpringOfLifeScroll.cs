using System;
using Server;
using Server.Items;

namespace Server.Items
{
   public class SpringOfLifeScroll : SpellScroll
   {
      [Constructable]
      public SpringOfLifeScroll() : this( 1 )
      {
      }

      [Constructable]
      public SpringOfLifeScroll( int amount ) : base( 304, 0xE39 )
      {
         Name = "Spring Of Life";
         Hue = 0x58B;
      }

      public SpringOfLifeScroll( Serial serial ) : base( serial )
      {
      		ItemID=0xE39;
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

