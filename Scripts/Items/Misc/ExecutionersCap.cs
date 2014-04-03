using System;

namespace Server.Items
{
    // Scriptiz : on en fait un réactif pour la nécro
    // public class ExecutionersCap : Item
    public class ExecutionersCap : BaseReagent, ICommodity
	{
        string Description
        {
            get
            {
                return String.Format("{0} executioner's cap", Amount);
            }
        }

        int ICommodity.DescriptionNumber { get { return LabelNumber; } }
        bool ICommodity.IsDeedable { get { return true; } }

		[Constructable]
		public ExecutionersCap() : base(0xF83)
		{
			Weight = 1.0;
		}

		public ExecutionersCap(Serial serial) : base(serial)
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
}