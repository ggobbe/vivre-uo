using System;
using Server;
using Server.Network;
using Server.Regions;
using Server.Mobiles;

namespace Server.Items
{
    public class HallucinogenTrap : BaseTrap, ICarvable
    {
        [Constructable]
        public HallucinogenTrap()
            : base(0x1125)
        {
        }

        public override bool PassivelyTriggered { get { return true; } }
        public override TimeSpan PassiveTriggerDelay { get { return TimeSpan.FromSeconds(6); } }
        public override int PassiveTriggerRange { get { return 6; } }
        public override TimeSpan ResetDelay { get { return TimeSpan.Zero; } }

        private DateTime m_NextHarvestTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime HarvestTime
        {
            get { return m_NextHarvestTime; }
            set { m_NextHarvestTime = value; }
        }

        public void Carve(Mobile from, Item item)
        {
            if (from.IsHallucinated || from.Poisoned)
            {
                from.PrivateOverheadMessage(MessageType.Regular, 0x3B2, true, "Vous ne pouvez cueillir convenablement dans cet état", from.NetState);
                return;
            }
            if (DateTime.Now < m_NextHarvestTime)
            {
                from.PrivateOverheadMessage(MessageType.Regular, 0x3B2, true, "Il n'y a pas assez de spores à récolter", from.NetState);
                return;
            }
            
            from.SendMessage("Vous cueillez le champignon"); 
            from.AddToBackpack(new HallucinogenMushroom());

            HarvestTime = DateTime.Now + TimeSpan.FromHours(Utility.RandomMinMax(2,10)); // TODO: Proper time delay
        }


        public override void OnTrigger(Mobile from)
        {
            if (!from.Alive || ItemID != 0x1125 || from.AccessLevel > AccessLevel.Player)
                return;

            ItemID = 0x1126;
            Effects.PlaySound(Location, Map, 0x306);

            foreach (NetState state in this.GetClientsInRange(PassiveTriggerRange))
            {
                if (state.Mobile is PlayerMobile)
                {
                    PlayerMobile junkie = state.Mobile as PlayerMobile;

                    if ( junkie.FindItemOnLayer(Layer.Helm) is VivreCagoule) //à changer!
                        return;

                    if (junkie.Hallucinating)
                        junkie.ApplyPoison(junkie, Utility.RandomBool() ? Poison.Regular : Poison.Lethal);
                    else
                    {
                        junkie.Hallucinating = true;
                        Timer.DelayCall(TimeSpan.FromMinutes(5), HallucinogenPotion.StopHallucinate, junkie);
                    }
                    junkie.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "*Semble étouffé par les spores du champignon*");
                }
                else if (!state.Mobile.Poisoned)
                    state.Mobile.ApplyPoison(state.Mobile, Utility.RandomBool() ? Poison.Regular : Poison.Greater);
            }
            Timer.DelayCall(TimeSpan.FromSeconds(2), new TimerCallback(OnMushroomReset));
        }

        public virtual void OnMushroomReset()
        {
            if(Utility.RandomBool())
                Delete();
            else
                ItemID = 0x1125; // reset
        }

        public HallucinogenTrap(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.WriteDeltaTime(m_NextHarvestTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_NextHarvestTime = reader.ReadDeltaTime();
            if (ItemID == 0x1126)
                OnMushroomReset();
        }
    }
}