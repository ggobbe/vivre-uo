using System;
using Server;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Items
{
	public class HeatingStand : BaseLight
	{
		public override int LitItemID{ get { return 0x184A; } }
		public override int UnlitItemID{ get { return 0x1849; } }

		[Constructable]
		public HeatingStand() : base( 0x1849 )
		{
			if ( Burnout )
				Duration = TimeSpan.FromMinutes( 25 );
			else
				Duration = TimeSpan.Zero;

			Burning = false;
			Light = LightType.Empty;
			Weight = 1.0;
		}

        public override void OnDoubleClick(Mobile from)
        {
            if(ItemID == UnlitItemID)
            {
                from.SendMessage("Que voulez vous chauffer?");
                from.BeginTarget(2, false, TargetFlags.None, new TargetCallback(OnTarget));
            }
            base.OnDoubleClick(from);
        }

        public void OnTarget(Mobile from, object obj)
        {
            if (!(obj is AlchemyVial))
            {
                from.SendMessage("Chauffer cette solution ne vous servira à rien");
                return;
            }

            AlchemyVial vial = (AlchemyVial)obj;

            if (vial.AlchemyLiquidType == LiquidType.None)
            {
                from.SendMessage("Votre fiole est vide");
                return;
            }

            if (vial.AlchemyLiquidType != LiquidType.ChangelingBlood)
            {
                from.SendMessage("Il ne servirait à rien de tenter de faire chauffer cette éprouvette");
                return;
            }

            if (!from.CheckSkill(SkillName.Alchemy,20,90))
            {
                from.SendMessage("L'éprouvette explose!");
                from.Hits -= 5;
                return;
            }

            from.SendMessage("Le liquide bout et se transforme. Vous le versez lentement dans une autre éprouvette");
            vial.Consume();
            from.AddToBackpack(new MorphBase());
        }

		public override void Ignite()
		{
			base.Ignite();

			if ( ItemID == LitItemID )
				Light = LightType.Circle150;
			else if ( ItemID == UnlitItemID )
				Light = LightType.Empty;
		}

		public override void Douse()
		{
			base.Douse();

			if ( ItemID == LitItemID )
				Light = LightType.Circle150;
			else if ( ItemID == UnlitItemID )
				Light = LightType.Empty;
		}

		public HeatingStand( Serial serial ) : base( serial )
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