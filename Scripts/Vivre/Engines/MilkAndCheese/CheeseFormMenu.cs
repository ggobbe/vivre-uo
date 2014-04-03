using System;
using Server.Items;

namespace Server.ContextMenus
{
    public class CheeseFormMenu : ContextMenuEntry
    {
        private Mobile m_From;
        private CheeseForm m_Cheese;

        public CheeseFormMenu(Mobile from, CheeseForm CheeseForm)
            : base(5114, 1)
        {
            m_From = from;
            m_Cheese = CheeseForm;
        }

        public override void OnClick()
        {
            DateTime now = DateTime.Now;

            if ((now - m_Cheese.TimeStart).TotalDays <= 2)
            {
                m_From.SendMessage("Il ne servirait à rien dans l'état actuel de retirer le fromage");
                return;
            }

            bool isexceptionnal = false;
            double exceptionnalchance = ((m_From.Skills[SkillName.Cooking].Value) / 2 + (m_Cheese.CookingValue / 2) + (m_From.Skills[SkillName.TasteID].Value / 5)) / 150;

            if (exceptionnalchance > Utility.RandomDouble())
                isexceptionnal = true;


            for (int i = 0; i < m_Cheese.CheeseAmount; i++)
            {

            CookableCheese wheel = new CookableCheese();
            wheel.Cook = m_From;
            //Type de lait
            wheel.CheeseMilk = m_Cheese.Content;

            double delay = (now - m_Cheese.TimeStart).TotalDays;

            if (isexceptionnal)
                wheel.Quality = CheeseQuality.Exceptionnal;

            //Poison au besoin
            if(delay > 17)
            {
                if (delay + 11 > 17)
                    wheel.Poison = Poison.Deadly;
                else if (delay + 8 > 17)
                    wheel.Poison = Poison.Greater;
                else if (delay + 5 > 17)
                    wheel.Poison = Poison.Regular;
                else if (delay + 2 > 17)
                    wheel.Poison = Poison.Lesser;
                wheel.Poisoner = m_From;
            }
            
            //Gout du fromage
            if (delay <= 5)
                wheel.Taste = CheeseTaste.Faible; 
            else if (delay <= 8)
                wheel.Taste = CheeseTaste.Leger;
            else if (delay <= 11)
                wheel.Taste = CheeseTaste.Modere;
            else if (delay <= 14)
                wheel.Taste = CheeseTaste.Prononce;
            else 
                wheel.Taste = CheeseTaste.Fort; 
            
            //Pâte du fromage
            if (m_Cheese.IsChildOf(m_From.Backpack))
                wheel.Paste = CheesePaste.Molle;
            else if (m_Cheese.IsChildOf(typeof(BaseContainer)))
                wheel.Paste = CheesePaste.Normale;
            else
                wheel.Paste = CheesePaste.Dure;

            m_From.AddToBackpack(wheel);
   
            }
            m_From.SendMessage("Vous extrayez le fromage du moule et le coupez lentement..."); 

            m_Cheese.Content = Milk.None;
            m_Cheese.Quantity = 0;
        }
    }
}