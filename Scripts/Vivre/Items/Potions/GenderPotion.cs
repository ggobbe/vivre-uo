using System;
using Server;
//Plume : Transformation en fiole
namespace Server.Items
{
	public class GenderPotion : BasePotion
	{
        private bool m_Female;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Female
        {
            get { return m_Female; }
            set { m_Female = value; InvalidateProperties(); }
        }

		[Constructable]
		public GenderPotion() : base( 0x0E24, PotionEffect.GenderSwap )
		{
            Name = "Potion rouge et noire";
            Hue = 2075;
            Stackable =false;
            m_Female = Utility.RandomBool();

            if (!m_Female)
                Name = "Potion noire et rouge";
		}

		public GenderPotion( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

            writer.Write((bool)m_Female);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            switch(version)
            {
                case 1: m_Female = reader.ReadBool(); break;
            }

            if (ItemID == 0xF06)
                ItemID = 0x0E24;
		}

		public override void Drink( Mobile from )
		{
            from.FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
            from.PlaySound(0x1E3);

            BasePotion.PlayDrinkEffect(from);
           
            if (from.Female == this.Female)
            {
                from.SendMessage("Vous buvez le tout et attendez... vous pourriez attendre très longtemps...");
                this.Delete();
                from.AddToBackpack(new AlchemyVial());
                return;
            }
            
            from.Female = this.Female;
            
            from.Say("*Lâche la potion, une grimace de douleur défigurant son visage*");
            this.Delete();
		}
	}
}