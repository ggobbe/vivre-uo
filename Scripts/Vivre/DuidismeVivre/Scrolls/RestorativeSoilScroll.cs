using System;
using Server;
using Server.Items;

namespace Server.Items
{
   public class RestorativeSoilScroll : SpellScroll
   {
      [Constructable]
      public RestorativeSoilScroll() : this( 1 )
      {
      }

      [Constructable]
      public RestorativeSoilScroll( int amount ) : base( 315, 0xE39 )
      {
         Name = "Restorative Soil";
         Hue = 0x58B;
      }

      public RestorativeSoilScroll( Serial serial ) : base( serial )
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
