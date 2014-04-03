using System;
using Server.Targeting;

namespace Server.Items
{
    public class SmileyPumpkin : BaseLight
    {
        public override int LitItemID { get { return 0x4695; } }
        public override int UnlitItemID { get { return 0x4698; } }

        [Constructable]
        public SmileyPumpkin()
            : base(0x4698)
        {
            Burning = false;
            Light = LightType.NorthSmall;
            Weight = 1.0;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (Protected && from.AccessLevel == AccessLevel.Player)
                return;

            if (!from.InRange(this.GetWorldLocation(), 2))
                return;

            if (Burning)
            {
                from.SendMessage("Comme elle est magnifique!");      
            }
            else
            {
                from.BeginTarget(2, false, TargetFlags.None, new TargetCallback(OnTarget));
                from.SendMessage("Elle serait encore mieux avec une chandelle à l'intérieur, non?");
            }
        }

        public void OnTarget(Mobile from, object obj)
        {
            if (obj is Candle)
            {
                Candle targ = (Candle)obj;

                from.SendMessage("Vous déposez la chandelle dans la citrouille!");
                this.Ignite();
                targ.Delete();
                Timer.DelayCall(TimeSpan.FromMinutes(20), Douse);
                return;
            }
            if (obj is CandleSkull)
            {
                from.SendMessage("Ceci fait bien trop peur pour une citrouille si jolie...");
                return;
            }

            from.SendMessage("Cela n'entrera pas dans la citrouille.");
        }

        public SmileyPumpkin(Serial serial)
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

    public class EvilPumpkin : BaseLight
    {
        public override int LitItemID { get { return 0x4691; } }
        public override int UnlitItemID { get { return 0x4694; } }


        [Constructable]
        public EvilPumpkin()
            : base(0x4694)
        {
            Burning = false;
            Light = LightType.NorthSmall;
            Weight = 1.0;
        }

        public EvilPumpkin(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (Protected && from.AccessLevel == AccessLevel.Player)
                return;

            if (!from.InRange(this.GetWorldLocation(), 2))
                return;

            if (Burning)
            {
                from.SendMessage("Comme elle est effrayante!");
            }
            else
            {
                from.BeginTarget(2, false, TargetFlags.None, new TargetCallback(OnTarget));
                from.SendMessage("Elle serait encore mieux avec une chandelle à l'intérieur, non?");
            }
        }

        public void OnTarget(Mobile from, object obj)
        {
            if (obj is Candle)
            {
               
                from.SendMessage("Pour une citrouille aussi effrayante, il faut une chandelle tout aussi effrayante");
                return;
            }
            if(obj is CandleSkull)
            {
                CandleSkull targ = (CandleSkull)obj;
                from.SendMessage("Vous déposez la chandelle dans la citrouille!");
                targ.Delete();
                this.Ignite();
                Timer.DelayCall(TimeSpan.FromMinutes(20), Douse);
                return;
            }

            from.SendMessage("Cela n'entrera pas dans la citrouille.");
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