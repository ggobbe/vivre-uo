using System;
using Server.Targeting;

namespace Server.Items
{
    public class Skull : Item
    {

        [Constructable]
        public Skull() : base(0x1AE2)
        {
            Movable = true;
            Stackable = false;
            Name = "Crane";
        }
  

         public override void OnDoubleClick(Mobile from)
        {
            if (from.Karma > -2500)
            {
                base.OnDoubleClick(from);
                return;
            }
            
            from.SendMessage("Vous seriez assez méchant pour en faire une décoration!");
            from.BeginTarget(2, false, TargetFlags.None, new TargetCallback(OnTarget));
        }

        public void OnTarget(Mobile from, object obj)
        {
            if (obj is Candle)
            {
                Candle targ = (Candle)obj;
                from.SendMessage("En appuyant fortement, vous parvenez à fixer la chandelle sur le crâne");      
                from.AddToBackpack(new CandleSkull());
                this.Delete();
                targ.Delete();
                return;
            }

            from.SendMessage("Vous ne pouvez fixer cela sur le crâne");
        }

        public Skull(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}