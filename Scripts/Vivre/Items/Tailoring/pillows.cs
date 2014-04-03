using System;
using Server.Engines.Craft;
using System.Collections.Generic;
using Server.Network;


namespace Server.Items
{
	public class SmallPillow : Item , IDyable
	{
      
		[Constructable]
		public SmallPillow() : base( 0x1397 )
		{
			Weight = 1.0;
            Name = "petit coussin";
		}
      

		

        public virtual bool Dye(Mobile from, DyeTub sender)
        {
            if (Deleted)
                return false;
            else if (RootParent is Mobile && from != RootParent)
                return false;

            Hue = sender.DyedHue;

            return true;
        }

        public SmallPillow(Serial serial)
            : base(serial)
        {
        }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

		}

        
    }

    public class Pillow : Item, IDyable
    {

        [Constructable]
        public Pillow()
            : base(0x163c)
        {
            Weight = 1.0;
            Name = "coussin";
        }
        


        public virtual bool Dye(Mobile from, DyeTub sender)
        {
            if (Deleted)
                return false;
            else if (RootParent is Mobile && from != RootParent)
                return false;

            Hue = sender.DyedHue;

            return true;
        }


        public Pillow(Serial serial)
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


    public class BigPillow : Item, IDyable
    {

        [Constructable]
        public BigPillow()
            : base(0x163A)
        {
            Weight = 1.0;
            Name = "gros coussin";
        }


        

        public virtual bool Dye(Mobile from, DyeTub sender)
        {
            if (Deleted)
                return false;
            else if (RootParent is Mobile && from != RootParent)
                return false;

            Hue = sender.DyedHue;

            return true;
        }

        public BigPillow(Serial serial)
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

    public class SmallDiagPillow : Item, IDyable
    {

        [Constructable]
        public SmallDiagPillow()
            : base(0x13AB)
        {
            Weight = 1.0;
            Name = "petit oreiller";
        }
       

        public virtual bool Dye(Mobile from, DyeTub sender)
        {
            if (Deleted)
                return false;
            else if (RootParent is Mobile && from != RootParent)
                return false;

            Hue = sender.DyedHue;

            return true;
        }

        public SmallDiagPillow(Serial serial)
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


    public class DiagPillow : Item, IDyable
    {

        [Constructable]
        public DiagPillow()
            : base(0x163B)
        {
            Weight = 1.0;
            Name = "petit oreiller";
        }


        public virtual bool Dye(Mobile from, DyeTub sender)
        {
            if (Deleted)
                return false;
            else if (RootParent is Mobile && from != RootParent)
                return false;

            Hue = sender.DyedHue;

            return true;
        }

        public DiagPillow(Serial serial)
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