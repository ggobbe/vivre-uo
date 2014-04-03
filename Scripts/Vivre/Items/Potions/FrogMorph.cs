using System;
using Server;
namespace Server.Items
{
	public class FrogMorphPotion : BasePotion
	{
        [Constructable]
		public FrogMorphPotion() : base( 0xF07, PotionEffect.FrogMorph )
		{
            Name = "Potion d'amour véritable";
            Hue = 0x2D;
            Stackable =true;
		}

        public FrogMorphPotion(Serial serial)
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
           
            from.BodyValue = 0x51;
            if(from.Female)
                from.SendMessage("Trouvez-vous un Prince");
            else
                from.SendMessage("Trouvez-vous une Princesse");
            
            from.Say("*Lâche la potion, une grimace de douleur défigurant son visage*");
            this.Delete();
		}
	}
}