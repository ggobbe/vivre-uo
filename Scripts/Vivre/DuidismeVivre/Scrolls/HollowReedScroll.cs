using System;
using Server;
using Server.Items;

namespace Server.Items
{
   public class HollowReedScroll : SpellScroll
   {
      [Constructable]
      public HollowReedScroll() : this( 1 )
      {
      }

      [Constructable]
      public HollowReedScroll( int amount ) : base( 302, 0xE39 )
      {
         Name = "Hollow Reed";
         Hue = 0x58B;
      }

      public HollowReedScroll( Serial serial ) : base( serial )
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
