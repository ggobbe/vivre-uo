using System;
using Server;

namespace Server.Items
{
	[Flipable]
	public class DragonLantern : BaseLight
	{
		public override int LitItemID{ get { return 0x49C1; } }
		public override int UnlitItemID{ get { return 0x49C2; } }
        public override int LitSound{ get { return 362; } }


		[Constructable]
        public DragonLantern() : base(0x49C2)
		{
            Name = "La Lanterne du Grand Dragon";
            Movable = true;
			Duration = TimeSpan.Zero; // Never burnt out
			Burning = false;
			Weight = 3.0;
            Light = LightType.Circle300;
		}

        public override void OnDoubleClick(Mobile from)
        {
            if (this.IsLockedDown)
            {
                if (this.ItemID == 0x49C2)
                    {
                    from.PlaySound(362);
                    Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052);
                    }
                else
                    Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 1, 30, 1108, 6, 9904, 0);
                
                base.OnDoubleClick(from);
            }
            else
                from.SendMessage("Vous devez d'abord poser cette oeuvre d'art dans un endroit convenable");
        }

		public DragonLantern( Serial serial ) : base( serial )
		{
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