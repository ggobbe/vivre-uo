using System;
using Server.Mobiles;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using Server.Gumps;

namespace Server.Items
{
    public class BeardRestylingDeed : Item
    {
        

        [Constructable]
        public BeardRestylingDeed()
            : base(0x14F0)
        {
            Weight = 1.0;
            LootType = LootType.Blessed;
            Name = "Changement de barbe";
        }

        public BeardRestylingDeed(Serial serial)
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

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack...
            }
            else if (from.Female)
            {
                from.SendMessage("Allons jolie damoiselle, ceci est pour un homme"); // That must be in your pack...
            }
            else
            {
                from.SendGump(new InternalGump(from, this));
            }
        }

        private class InternalGump : Gump
        {
            private Mobile m_From;
            private BeardRestylingDeed m_Deed;

            public InternalGump(Mobile from, BeardRestylingDeed deed)
                : base(50, 50)
            {
                m_From = from;
                m_Deed = deed;

                from.CloseGump(typeof(InternalGump));

                AddBackground(100, 10, 400, 385, 0xA28);

                AddHtmlLocalized(100, 25, 400, 35, 1013008, false, false);
                AddButton(175, 340, 0xFA5, 0xFA7, 0x0, GumpButtonType.Reply, 0); // CANCEL

                AddHtmlLocalized(210, 342, 90, 35, 1011012, false, false);// <CENTER>HAIRSTYLE SELECTION MENU</center>

                int[][] RacialData = HumanArray;

                for (int i = 1; i < RacialData.Length; i++)
                {
                    AddHtmlLocalized(LayoutArray[i][2], LayoutArray[i][3], (i == 1) ? 125 : 80, (i == 1) ? 70 : 35, RacialData[i][0], false, false);
                    if (LayoutArray[i][4] != 0)
                    {
                        AddBackground(LayoutArray[i][0], LayoutArray[i][1], 50, 50, 0xA3C);
                        AddImage(LayoutArray[i][4], LayoutArray[i][5], RacialData[i][2]);
                    }
                    AddButton(LayoutArray[i][6], LayoutArray[i][7], 0xFA5, 0xFA7, i, GumpButtonType.Reply, 0);
                }
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                if (m_From == null || !m_From.Alive)
                    return;

                if (m_Deed.Deleted)
                    return;

                if (info.ButtonID < 1 || info.ButtonID > 10)
                    return;

                int[][] RacialData = HumanArray;

                if (m_From is PlayerMobile)
                {
                    PlayerMobile pm = (PlayerMobile)m_From;

                    pm.SetHairMods(-1, -1); // clear any hairmods (disguise kit, incognito)
                    m_From.HairItemID =  RacialData[info.ButtonID][1];
                    m_Deed.Delete();
                }
            }
            /* 
                    gump data: bgX, bgY, htmlX, htmlY, imgX, imgY, butX, butY 
            */

            int[][] LayoutArray =
			{
				new int[] { 0 }, /* padding: its more efficient than code to ++ the index/buttonid */
				new int[] { 425, 280, 342, 295, 000, 000, 310, 292 },
				new int[] { 235, 060, 150, 075, 168, 020, 118, 073 },
				new int[] { 235, 115, 150, 130, 168, 070, 118, 128 },
				new int[] { 235, 170, 150, 185, 168, 130, 118, 183 },
				new int[] { 235, 225, 150, 240, 168, 185, 118, 238 },
				new int[] { 425, 060, 342, 075, 358, 018, 310, 073 },
				new int[] { 425, 115, 342, 130, 358, 075, 310, 128 },
				new int[] { 425, 170, 342, 185, 358, 125, 310, 183 },
				new int[] { 425, 225, 342, 240, 358, 185, 310, 238 },
				new int[] { 235, 280, 150, 295, 168, 245, 118, 292 } // slot 10, Curly - N/A for elfs.
			};

            /*
                    racial arrays are: cliloc_M, ItemID_M, gump_img_M
            */
            int[][] HumanArray = /* why on earth cant these utilies be consistent with hex/dec */
			{
				new int[] { 0 }, 
				new int[] { 3000340, 0, 0  }, 
				new int[] { 3000352, 0x203E, 0xC671 },
				new int[] { 3000353, 0x203F,  0xc672 }, 
				new int[] { 3000351, 0x2040, 0xc670 }, 
				new int[] { 3000354, 0x2041,  0xC673 }, 
				new int[] { 3000355, 0x204B,  0xC676 }, 
				new int[] { 3000356, 0x204C, 0xC675 }, 
				new int[] { 3000357, 0x204D,  0xC677 }
			};
           
        }
    }
}