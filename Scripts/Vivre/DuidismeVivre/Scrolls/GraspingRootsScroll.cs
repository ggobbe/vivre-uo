using System;
using Server;
using Server.Items;

namespace Server.Items
{
   public class GraspingRootsScroll : SpellScroll
   {
      [Constructable]
      public GraspingRootsScroll() : this( 1 )
      {
      }

      [Constructable]
      public GraspingRootsScroll( int amount ) : base( 305, 0xE39 )
      {
         Name = "Grasping Roots";
         Hue = 0x58B;
      }

      public GraspingRootsScroll( Serial serial ) : base( serial )
      {
      	Name = "Grasping Roots";
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
