using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class NecroVendor : BaseVendor
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

        [Constructable]
        public NecroVendor()
            : base("le Disciple", "la Disciple")
        {
        }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBNecroVendor());
        }

        public override void InitOutfit()
        {
            //base.InitOutfit();

            Hue = 33777;

            Item shroud = new RobeACapuche(1109);
            shroud.Movable = false;
            AddItem(shroud);

            Item staff = new GnarledStaff();
            staff.Hue = 2211;
            staff.Movable = false;
            AddItem(staff);
        }

        public NecroVendor(Serial serial)
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