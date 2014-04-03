using System;
using Server;
using Server.Items;

namespace Server.Items
{
   public class EnchantedGroveScroll : SpellScroll
   {
      [Constructable]
      public EnchantedGroveScroll() : this( 1 )
      {
      }

      [Constructable]
      public EnchantedGroveScroll( int amount ) : base( 311, 0xE39 )
      {
         Name = "Enchanted Grove";
         Hue = 0x58B;
      }

      public EnchantedGroveScroll( Serial serial ) : base( serial )
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
