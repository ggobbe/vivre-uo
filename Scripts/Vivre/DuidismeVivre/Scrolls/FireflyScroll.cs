using System;
using Server;
using Server.Items;

namespace Server.Items
{
   public class FireflyScroll : SpellScroll
   {
      [Constructable]
      public FireflyScroll() : this( 1 )
      {
      }

      [Constructable]
      public FireflyScroll( int amount ) : base( 316, 0xE39 )
      {
         Name = "Summon Firefly";
         Hue = 0x58B;
      }

      public FireflyScroll( Serial serial ) : base( serial )
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
