using System;
using Server.Items;

namespace Server.ContextMenus
{
    public class BeverageEntry : ContextMenuEntry
    {
        private Mobile m_From;
        private BaseBeverage m_Beverage;

        public BeverageEntry(Mobile from, BaseBeverage beverage)
            : base(10176, 1)
        {
            m_From = from;
            m_Beverage = beverage;
        }

        public override void OnClick()
        {
            if (m_Beverage == null || m_From == null) return;
            if (m_Beverage.Deleted || !m_Beverage.Movable || !m_From.CheckAlive() || !m_Beverage.CheckItemUse(m_From))
                return;

            if (m_Beverage.Quantity > 0)
            {
                string typeName = "";
                int typeHue = 0;

                switch (m_Beverage.Content)
                {
                    case BeverageType.LaitChevre:
                    case BeverageType.LaitBrebis:
                    case BeverageType.Milk:
                        typeName = "de lait";
                        typeHue = 2050;
                        break;
                    case BeverageType.JusRaisin:
                        typeName = "de jus de raison";
                        typeHue = 1172;
                        break;
                    case BeverageType.JusPeche:
                        typeName = "de jus de pêches";
                        typeHue = 50;
                        break;
                    case BeverageType.JusPomme:
                        typeName = "de jus de pommes";
                        typeHue = 50;
                        break;
                    case BeverageType.Wine:
                        typeName = "de vin";
                        typeHue = 1172;
                        break;
                    case BeverageType.Liquor:
                        typeName = "de liqueur";
                        typeHue = 96;
                        break;
                    case BeverageType.Cider:
                        typeName = "de cidre";
                        typeHue = 50;
                        break;
                    case BeverageType.Ale:
                    case BeverageType.BiereAmbre:
                    case BeverageType.BiereBrune:
                    case BeverageType.BiereCommune:
                    case BeverageType.BiereEpice:
                    case BeverageType.BiereMiel:
                    case BeverageType.BiereSorciere:
                    case BeverageType.Kwak:
                    case BeverageType.MoinetteYew:
                        typeName = "de bière";
                        typeHue = 51;
                        break;
                    default:
                        typeName = "d'eau";
                        typeHue = 92;
                        break;
                }

                m_From.SendMessage("Vous arrosez le sol avec le contenu de votre " + m_Beverage.Name + ".");
                m_Beverage.Content = BeverageType.Water;
                m_Beverage.Quantity = 0;
                m_Beverage.Poison = null;

                int bloodId = Utility.Random(4650, 4);
                Static water = new Static(bloodId);
                water.Name = "Flaque " + typeName;
                water.Hue = typeHue;
                water.MoveToWorld(m_From.Location, m_From.Map);
                m_From.PlaySound(0x04E);
                new WateringTimer(water);
            }
            else
            {
                m_From.SendMessage("Vous ne pouvez pas arroser le sol avec quelque chose de vide...");
            }
        }

        private class WateringTimer : Timer
        {
            Static m_Water;

            public WateringTimer(Static water)
                : base(TimeSpan.FromSeconds(30))
            {
                m_Water = water;
                this.Start();
            }

            protected override void OnTick()
            {
                if(m_Water != null)
                    m_Water.Delete();
            }
        }
    }
}