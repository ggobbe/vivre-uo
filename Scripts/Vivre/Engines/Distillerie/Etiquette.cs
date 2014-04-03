using Server;
using Server.Targeting;
using Server.Gumps;
using Server.Network;
using System.Collections;

namespace Server.Items
{

    public class BottleInscriber : Item
    {
        private int m_UsesRemaining;

        [CommandProperty(AccessLevel.GameMaster)]
        public int UsesRemaining
        {
            get { return m_UsesRemaining; }
            set { m_UsesRemaining = value; InvalidateProperties(); }
        }

        [Constructable]
        public BottleInscriber()
            : base(0x0FF3)
        {
            Weight = 1.0;
            Hue = 0x96D;
            Name = "Etiquettes";
            UsesRemaining = 20;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1060584, UsesRemaining.ToString());
        }

        public virtual void DisplayDurabilityTo(Mobile from)
        {
            LabelToAffix(from, 1017323, AffixType.Append, ": " + UsesRemaining.ToString());
        }

        public override void OnDoubleClick(Mobile from)
        {
                from.Target = new InternalTarget(from, this);
        }

        public BottleInscriber(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write((int)m_UsesRemaining); // on ecrit le nombre de charge
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);


            int version = reader.ReadInt();
            switch (version)
            {
                case 0:
                    {
                        m_UsesRemaining = reader.ReadInt();// on lit le nombre de charge
                        break;
                    }
            }
        }

        private class InternalTarget : Target
        {
            private Mobile m_from;
            private BottleInscriber m_tool;

            public InternalTarget(Mobile from, BottleInscriber tool)
                : base(10, false, TargetFlags.None)
            {
                m_from = from;
                m_tool = tool;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (!(o is Bottle) && !(o is BaseBeverage))
                        {
                        from.SendMessage("Ceci n'est pas étiquetable");
                        return;
                        }

                if ( o is Item && !((Item)o).IsChildOf( from.Backpack ))
					{
						from.SendMessage("L'objet doit être dans votre sac");
						return;
					} 
               
                        m_tool.UsesRemaining--; 
                        from.SendGump(new BottleInscriberGump(from, o));
                        if (m_tool.UsesRemaining <= 0)
                            m_tool.Delete();
            }
        }
    }
}

namespace Server.Gumps
{

    public class BottleInscriberGump : Gump
    {
        private Item m_Bottle;
        private Mobile m_From;

        public BottleInscriberGump(Mobile from, object o)
            : base(0, 0)
        {
            m_Bottle = o as Item;
            m_From = from;

            AddPage(0);
            AddBackground(1, 9, 372, 144, 5054);
            AddBackground(11, 19, 352, 124, 3500);
            AddLabel(50, 27, 0, "Quel nom désirez vous lui donner ?");
            AddImage(48, 49, 1141);
            AddTextEntry(57, 50, 91, 18, 0x000, 0, "Écrire ici");
            AddButton(62, 85, 4023, 4025, 1, GumpButtonType.Reply, 0);
            AddButton(222, 85, 4020, 4022, 2, GumpButtonType.Reply, 0);
            AddLabel(97, 93, 0, "Ok");
            AddLabel(258, 93, 0, "Annuler");
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;

            switch (info.ButtonID)
            {
                case 0:
                    break;
                case 1:
                    m_Bottle.Name = string.Format("Bouteille de {0}", (string)info.GetTextEntry(0).Text);
                    break;
                case 2:
                    break;

            }
        }
    }
}