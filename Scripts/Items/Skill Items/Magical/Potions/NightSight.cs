using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class NightSightPotion : BasePotion
    {
        public override bool CIT { get { return true; } }

        private double m_Time;

        public double Time
        {
            get
            {
                return m_Time;
            }
        }

        [Constructable]
        public NightSightPotion()
            : base(0xF06, PotionEffect.Nightsight)
        {
        }

        public NightSightPotion(Serial serial)
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

        public override void Drink(Mobile from)
        {
            if (from.BeginAction(typeof(LightCycle)))
            {
                m_Time = IntensifiedTime ? 45 : 20;
                //Plume : Addiction
                if (from is PlayerMobile)
                {
                    PlayerMobile drinker = from as PlayerMobile;

                    double Addiction = drinker.CalculateHealAddiction(this);

                    if (Addiction > 100)
                    {
                        drinker.SendMessage("Votre corps ne supporte plus ce traitement");
                        drinker.Poison = Poison.Lesser;
                        drinker.Hunger = 0;
                    }

                    m_Time -= drinker.CalculateNightSightAddiction(this);
                }
                new LightCycle.PotionNightSightTimer(from, this).Start();
                from.LightLevel = LightCycle.DungeonLevel / 2;

                from.FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
                from.PlaySound(0x1E3);

                BasePotion.PlayDrinkEffect(from);

                if (!Engines.ConPVP.DuelContext.IsFreeConsume(from))
                    this.Consume();
            }
            else
            {
                from.SendMessage("You already have nightsight.");
            }
        }
    }
}