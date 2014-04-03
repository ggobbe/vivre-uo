using System;
using Server;
using Server.Items;

namespace Server.Items
{
   public class PackOfBeastScroll : SpellScroll
   {
      [Constructable]
      public PackOfBeastScroll() : this( 1 )
      {
      }

      [Constructable]
      public PackOfBeastScroll( int amount ) : base( 303, 0xE39 )
      {
         Name = "Pack Of Beast";
         Hue = 0x58B;
      }

      public PackOfBeastScroll( Serial serial ) : base( serial )
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
