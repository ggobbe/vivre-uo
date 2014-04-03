using System;
using Server.Network;
using Server.Mobiles;


namespace Server.Items
{
    public class Nougat : Food
    {

        [Constructable]
        public Nougat()
            : base(1, 0x4690)
        {
            Name = "Morceau de nougat";
            Weight = 1.0;
            FillFactor = 1;
            Stackable = true;
        }

        public void ChangeSpeed(Mobile from)
        {
            from.Send(SpeedControl.Disable);
            from.SendMessage("Cette euphorie s'est dissipée");
        }


        public override bool Eat(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("Ceci doit être dans votre sac");
                return false;
            }
            
            if (Utility.Random(5) == 2)
            {
                from.SendMessage("Vous vous sentez pousser des ailes, rien ne vous arrêterait");
                from.Send(SpeedControl.MountSpeed);
                Timer.DelayCall(TimeSpan.FromSeconds((int)from.RawDex/5), ChangeSpeed, from);
            }

            return base.Eat(from);
        }

        public Nougat(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
