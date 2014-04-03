using System;
using Server;
using Server.Mobiles;
namespace Server.Items
{
	public class HallucinogenPotion : BasePotion
	{
        [Constructable]
		public HallucinogenPotion() : base( 0xF07, PotionEffect.Hallucinogen )
		{
            Name = "Potion hallucinogène";
            Hue = 0x369;
            Stackable =true;
		}

        public HallucinogenPotion(Serial serial)
            : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0); // version

		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();


		}

		public override void Drink( Mobile from )
		{
            from.FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
            from.PlaySound(0x1E3);

            BasePotion.PlayDrinkEffect(from);
            if(from is PlayerMobile)
               {
                   PlayerMobile drinker = from as PlayerMobile;
                   drinker.Hallucinating = true;
                   Timer.DelayCall(TimeSpan.FromSeconds(180), StopHallucinate, drinker);
               }
            this.Consume();
		}

        public static void StopHallucinate(PlayerMobile from)
        {
            
            from.Hallucinating = false;
            from.SendMessage("Cette euphorie s'est dissipée");
        }
	}
}