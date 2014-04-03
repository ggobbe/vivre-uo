using System;
using Server;
using Server.Items;

namespace Server.Items
{
   public class VolcanicEruptionScroll : SpellScroll
   {
      [Constructable]
      public VolcanicEruptionScroll() : this( 1 )
      {
      }

      [Constructable]
      public VolcanicEruptionScroll( int amount ) : base( 308, 0xE39 )
      {
         Name = "Volcanic Eruption";
         Hue = 0x58B;
      }

      public VolcanicEruptionScroll( Serial serial ) : base( serial )
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
