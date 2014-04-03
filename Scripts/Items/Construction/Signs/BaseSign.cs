using System;
using Server.Network;
using Server.Prompts;

namespace Server.Items
{
	public abstract class BaseSign : Item
	{
		public BaseSign( int dispID ) : base( dispID )
		{
			Movable = false;
		}

		public BaseSign( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

        // Scriptiz : pour renommer les panneaux !
        protected class RenamePrompt : Prompt
        {
            private BaseSign m_Sign;

            public RenamePrompt(BaseSign sign)
            {
                m_Sign = sign;
            }

            public override void OnResponse(Mobile from, string text)
            {
                if (m_Sign.Deleted || !m_Sign.IsChildOf(from.Backpack))
                {
                    from.SendMessage("Mettez le panneau dans votre sac pour y graver quelque chose");
                    return;
                }

                m_Sign.Name = Utility.FixHtml(text);
            }
        }
	}
}