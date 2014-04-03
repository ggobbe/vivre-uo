using System;
using Server;
using Server.Items;
using Server.Prompts;

namespace Server.Items
{
	// Le timer de fermentation distçillerie
	public class timerDistillerie : Timer
	{
        private FermentationBarrel barrelDistiTimer;
		

        public timerDistillerie(FermentationBarrel m_TimerDistillerie)  : base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10), 100)
            //base( TimeSpan.FromSeconds(delai avant premier tick),TimeSpan.FromSeconds(durée du tick),nombre de répétition)
		{
			Priority = TimerPriority. FiftyMS ;
            barrelDistiTimer = m_TimerDistillerie;
		}
		
		protected override void OnTick()
		{
            if (barrelDistiTimer.stadeFermentation <= 99)
			{
                ++barrelDistiTimer.stadeFermentation;
                barrelDistiTimer.Name = "Fermentation : "+ barrelDistiTimer.stadeFermentation + " %";
			}
            if (barrelDistiTimer.stadeFermentation == 100)
			{
                barrelDistiTimer.Name = "Fermentation : *Pret* plus qu'a gouter";
                barrelDistiTimer.stadeFermentation = 0;
                barrelDistiTimer.fermentationencours = false;
                barrelDistiTimer.fermentationdone = true;
                barrelDistiTimer.FermentationtimerRun = false;
                if (barrelDistiTimer.FromBonusSkill < Utility.Random(1, 1201))
				{
                    barrelDistiTimer.fermentationsuccess = true;
                }
                else
                {
                    barrelDistiTimer.fermentationsuccess = false;
                }
			}
		}
		
	}
}