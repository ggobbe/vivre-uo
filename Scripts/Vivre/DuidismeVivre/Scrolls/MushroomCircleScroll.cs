using System;	
using Server;
using Server.Items;

namespace Server.Items
{
   public class MushroomCircleScroll : SpellScroll
   {
      [Constructable]
      public MushroomCircleScroll() : this( 1 )
      {
      }

      [Constructable]
      public MushroomCircleScroll( int amount ) : base( 310, 0xE39 )
      {
         Name = "Stone Circle";
         Hue = 0x58B;
      }

      public MushroomCircleScroll( Serial serial ) : base( serial )
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
