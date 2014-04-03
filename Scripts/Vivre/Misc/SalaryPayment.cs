using System;
using Server.Network;
using Server;
using Server.Mobiles;
using Server.Regions;
using Server.Items;
using System.Collections;

namespace Server.Misc
{
    public class SalaryTimer : Timer
    {
        public static void Initialize()
        {
            new SalaryTimer().Start();
        }


        public SalaryTimer()
            : base(TimeSpan.FromDays(1), TimeSpan.FromDays(1))
        {
            Priority = TimerPriority.OneMinute;
        }

        protected override void OnTick()
        {
            SalaryPayment();
        }

        public static void SalaryPayment()
        {
            ArrayList payment = new ArrayList();

            foreach (Item item in World.Items.Values)
            {
                if (item is SalaryChest)
                    payment.Add(item);
            }

            foreach (Item item in payment)
            {
                SalaryChest chest = item as SalaryChest;

                if (!chest.Active)
                    continue;

                if (chest.Employer == null || chest.Employer.AccessLevel >= AccessLevel.GameMaster)
                {
                  
                    Item check = chest.FindItemByType(typeof(BankCheck));
                        if (check != null)
                        {
                            BankCheck dropcheck = check as BankCheck;
                            dropcheck.Worth += chest.Salary;
                        }
                        else chest.DropItem(new BankCheck(chest.Salary));
                }
                else
                {
                    Container cont = chest.Employer.FindBankNoCreate();
                    
                    if (cont != null && Banker.Withdraw( chest.Employer, chest.Salary ))
                    {
                        Item check = chest.FindItemByType(typeof(BankCheck));
                        if (check != null)
                        {
                            BankCheck dropcheck = check as BankCheck;
                            dropcheck.Worth += chest.Salary;
                        }
                        else chest.DropItem(new BankCheck(chest.Salary));
                    }
                    else 
                    {
                        chest.Active = false;
                    }
                }
            }
        }

    }
}