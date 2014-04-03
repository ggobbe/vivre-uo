using System;
using Server;
using Server.Items;

namespace Server.Items
{
   public class BlendWithForestScroll : SpellScroll
   {
      [Constructable]
      public BlendWithForestScroll() : this( 1 )
      {
      }

      [Constructable]
      public BlendWithForestScroll( int amount ) : base( 306, 0xE39 )
      {
         Name = "Blend With Forest";
         Hue = 0x58B;
      }

      public BlendWithForestScroll( Serial serial ) : base( serial )
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
