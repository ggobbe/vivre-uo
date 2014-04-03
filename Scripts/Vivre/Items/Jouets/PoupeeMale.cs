using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public class PoupeeMale : Item
	{
        [Constructable]
        public PoupeeMale()  : base(0x2106)
		{
            Name = NameList.RandomName("male");
			Weight = 1.0;
		}

        public PoupeeMale(Serial serial)
            : base(serial)
		{
		}

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 2) || !from.InLOS(this))
            {
                from.SendLocalizedMessage(501816);
                return;
            }

            switch (Utility.Random(5))
            {
                default:
                case 0: this.PublicOverheadMessage(MessageType.Regular, 0, false, "T'as vu comment sa poupée est bien roulée!"); break;
                case 1: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Tu me regardes encore comme ça et je t'éclate la tronche!"); break;
                case 2: this.PublicOverheadMessage(MessageType.Regular, 0, false, "De la bière, vite!"); break;
                case 3: this.PublicOverheadMessage(MessageType.Regular, 0, false, "Tu vois bien que je suis occupé!"); break;
                case 4: this.PublicOverheadMessage(MessageType.Regular, 0, false, "En tout cas, c'est moi qui ai la plus grosse..."); break;
            }
        }
    public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

    public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
    }
}
