using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public abstract class BaseHealPotion : BasePotion
    {
        public override bool CIT { get { return false; } }
        public override bool CIS { get { return true; } }

        public abstract int MinHeal { get; }
        public abstract int MaxHeal { get; }
        public abstract double Delay { get; }

        public BaseHealPotion(PotionEffect effect)
            : base(0xF0C, effect)
        {
        }

        public BaseHealPotion(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public void DoHeal(Mobile from)
        {
            int min = Scale(from, MinHeal);
            int max = Scale(from, MaxHeal);

            from.Heal(Utility.RandomMinMax(min, max) + (IntensifiedStrength ? 5 : 0));
        }

        public void DoHeal(Mobile from, double scalar)
        {
            int min = Scale(from, MinHeal - (int)Math.Floor(MinHeal * scalar));
            int max = Scale(from, MaxHeal - (int)Math.Floor(MaxHeal * scalar));

            from.Heal(Utility.RandomMinMax(min, max)+(IntensifiedStrength?5:0));
        }

        public override void Drink(Mobile from)
        {
            if (from.Hits < from.HitsMax)
            {
                if (from.Poisoned || MortalStrike.IsWounded(from))
                {
                    from.LocalOverheadMessage(MessageType.Regular, 0x22, 1005000); // You can not heal yourself in your current state.
                }
                else
                {
                    if (from.BeginAction(typeof(BaseHealPotion)))
                    {
                        //Plume : Addiction
                        if (from is PlayerMobile)
                        {
                            PlayerMobile drinker = from as PlayerMobile;
                           
                            double Addiction = drinker.CalculateHealAddiction(this);
                            
                            if(Addiction > 100)
                            {
                                drinker.SendMessage("Votre corps ne supporte plus ce traitement");
                                drinker.Poison = Poison.Lesser;          
                            }
                            else
                            {
                                double HealScalar = Addiction/100 * 0.95;
                                DoHeal(from, HealScalar);
                            }
                            drinker.IncAddiction(this);
                        }
                        else
                            DoHeal(from);

                        BasePotion.PlayDrinkEffect(from);

                        if (!Engines.ConPVP.DuelContext.IsFreeConsume(from))
                            this.Consume();

                        Timer.DelayCall(TimeSpan.FromSeconds(Delay), new TimerStateCallback(ReleaseHealLock), from);
                    }
                    else
                    {
                        from.LocalOverheadMessage(MessageType.Regular, 0x22, 500235); // You must wait 10 seconds before using another healing potion.
                    }
                }
            }
            else
            {
                from.SendLocalizedMessage(1049547); // You decide against drinking this potion, as you are already at full health.
            }
        }

        private static void ReleaseHealLock(object state)
        {
            ((Mobile)state).EndAction(typeof(BaseHealPotion));
        }
    }
}