using System;
using System.Collections.Generic;
using Server.Gumps;
using Server.Network;


namespace Server.Items
{
    public class RubanCheveux : Item, IDyable
    {

        [Constructable]
        public RubanCheveux()
            : base(0xE20)
        {
            Name = "Ruban à cheveux";
            Weight = 1;
            Stackable = false;
                      
        }


        public override void OnDoubleClick(Mobile from)
        {
            // Scriptiz : comment un int peut-il être null? ^^
            //if (from.HairItemID == null ) return;

            if (! ((from.BodyValue == 0x190) || (from.BodyValue == 0x191) ) )
            {
                from.SendMessage("Désolé, vous ne semblez pas morphologiquements compatible");
                return;
            }
            
            
            if ( !IsChildOf(from.Backpack) )
            {
                from.SendMessage("Ce serait beaucoup plus pratique s'il était sur vous");
                return ;
            }



            if ((from.HairItemID == 0x203C) || (from.HairItemID == 0x203D) || (from.HairItemID == 0x2049)) // respectivement cheveux longs, queue de cheval, tresses
            
                from.SendGump( new GrubanCheveuxGump() );

               
            else
            
                from.SendMessage("Désolé, il va falloir les laisser pousser un peu");
            
            
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


        public RubanCheveux(Serial serial)
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

namespace Server.Gumps
{
    public class GrubanCheveuxGump : Gump
    {
        public GrubanCheveuxGump()
            : base(0, 0)
        {
            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;
            AddPage(0);
            AddBackground(0, 0, 285, 185, 9270);
            AddButton(100, 55, 0xA9A, 0xA9B, 1, GumpButtonType.Reply, 0); 
            AddButton(100, 95, 0xA9A, 0xA9B, 2, GumpButtonType.Reply, 0);
            AddButton(100, 135, 0xA9A, 0xA9B, 3, GumpButtonType.Reply, 0);           
            AddLabel(95, 16, 0x111, "Choix de coiffure");
            AddLabel(150, 55, 0x111, "Attachés");
            AddLabel(150, 95, 0x111, "Détachés");
            AddLabel(150, 135, 0x111, "Tressés");

        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;
            int cheveux = from.HairItemID;

            switch (info.ButtonID)
            {

              

                case 1:
                    {

                        if (cheveux == 0x203d)
                        {
                            from.SendMessage("Vos cheveux sont déja attachés");
                            break;
                        }

                        else
                        {
                            from.SendMessage("Vous attachez vos cheveux ");
                            from.PlaySound(0x57);
                            from.HairItemID = 0x203D;

                            break;
                        }
                    }

                case 2:
                    {
                        if (cheveux == 0x203c)
                        {
                            from.SendMessage("Vos cheveux sont déja détachés");
                            break;
                        }

                        else
                        {
                            from.SendMessage("Vous détachez vos cheveux ");
                            from.PlaySound(0x57);
                            from.HairItemID = 0x203c;

                            break;


                        }
                    }

                        case 3:
                    {
                        if (cheveux == 0x2049)
                        {
                            from.SendMessage("Vos cheveux sont déja tressés");
                            break;
                        }

                        else
                        {
                            from.SendMessage("Vous tressez vos cheveux ");
                            from.PlaySound(0x57);
                            from.HairItemID = 0x2049;

                            break;

                        }

                    }
            }
        }
    }
}