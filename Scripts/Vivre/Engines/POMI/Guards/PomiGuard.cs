using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using System.Collections.Generic;
using Server.ContextMenus;

namespace Server.IPOMI
{
    public class PomiGuard : BaseCreature
    {
        private TownStone m_Town;
        private GuardSpawner m_spawn;

        public PomiGuard(TownStone town, GuardSpawner spawn)
            : base(AIType.AI_Pomi, FightMode.Closest, 15, 1, 0.2, 1)
        {
            m_spawn = spawn;
            Location = m_spawn.Location;
            m_Town = town;
            Map = spawn.Map;
            InitStats(200, 200, 200);
            SpeechHue = Utility.RandomDyedHue();
            Hue = Utility.RandomSkinHue();
            Body = 0x190;
            Name = NameList.RandomName("male");
            Title = "Garde de " + m_Town.Nom;

            PlateChest chest = new PlateChest();
            chest.Hue = 0;
            chest.Movable = false;
            AddItem(chest);
            PlateArms arms = new PlateArms();
            arms.Hue = 0;
            arms.Movable = false;
            AddItem(arms);
            PlateGloves gloves = new PlateGloves();
            gloves.Hue = 0;
            gloves.Movable = false;
            AddItem(gloves);
            PlateGorget gorget = new PlateGorget();
            gorget.Hue = 0;
            gorget.Movable = false;
            AddItem(gorget);
            PlateLegs legs = new PlateLegs();
            legs.Hue = 0;
            legs.Movable = false;
            AddItem(legs);
            PlateHelm helm = new PlateHelm();
            helm.Hue = 0;
            helm.Movable = false;
            AddItem(helm);
            Surcoat surcoat = new Surcoat();
            surcoat.Hue = m_Town.Hue;
            surcoat.Movable = false;
            AddItem(surcoat);


            AddItem(new PomiCloak(m_Town, "Garde"));

            HairItemID = Utility.RandomList(0x203B, 0x203C, 0x203D, 0x2044, 0x2045, 0x2047, 0x2049, 0x204A);
            HairHue = Utility.RandomHairHue();

            if (Utility.RandomBool())
            {
                FacialHairItemID = Utility.RandomList(0x203E, 0x203F, 0x2040, 0x2041, 0x204B, 0x204C, 0x204D);
                FacialHairHue = HairHue;
            }

            Halberd weapon = new Halberd();
            //weapon.Hue = m_Town.Hue;
            weapon.Movable = false;
            weapon.Crafter = this;
            weapon.Quality = WeaponQuality.Exceptional;
            VirtualArmor = 100;

            AddItem(weapon);

            Skills[SkillName.Anatomy].Base = 100.0;
            Skills[SkillName.Tactics].Base = 110.0;
            Skills[SkillName.Swords].Base = 160.0;
            Skills[SkillName.MagicResist].Base = 110.0;
            Skills[SkillName.DetectHidden].Base = 100.0;
        }

        public override bool Unprovokable { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }

        public override bool BardImmune { get { return true; } }
        public PomiGuard(Serial serial)
            : base(serial)
        {
        }

        public TownStone Town
        {
            get { return m_Town; }
        }

        public override bool OnBeforeDeath()
        {
        	if(m_spawn != null)
            	m_spawn.Start();
        	
            return true;
        }

        public override void OnActionCombat()
        {
        }

        public override void OnCombatantChange()
        {
            base.OnCombatantChange();
            this.Say("Un Hors la Loi !!! Sortez de cette ville ou mourez!");
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            if (from is PlayerMobile) return true;
            return base.HandlesOnSpeech(from);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            PlayerMobile from = e.Mobile as PlayerMobile;

            if (from != null && !e.Handled && e.Mobile.InRange(this, 2))
            {
                if (m_Town.isCapitaine(from) || m_Town.Gardes.Contains(from))
                {
                    e.Handled = true;
                    if (e.Speech.ToLower() == "suivez-moi")
                    {
                        this.Say("Je vous suis!");
                        this.ControlMaster = from;
                        this.ControlOrder = OrderType.Guard;
                        this.Controlled = true;
                    }
                    else if (from != ControlMaster)
                    {
                        this.Say("Hein que dites vous?");
                    }
                }
            }
            base.OnSpeech(e);
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            if (this.Controlled && from == this.ControlMaster && from.InRange(this, 14))
            {
                list.Add(new InternalEntry(from, 6107, 14, this, this.AIObject, OrderType.Guard));  // Command: Guard
                list.Add(new InternalEntry(from, 6108, 14, this, this.AIObject, OrderType.Follow)); // Command: Follow

                //list.Add( new InternalEntry( from, 6111, 14, m_Mobile, this, OrderType.Attack ) ); // Command: Kill
                list.Add(new InternalEntry(from, 6112, 14, this, this.AIObject, OrderType.Stop));   // Command: Stop
                list.Add(new InternalEntry(from, 6114, 14, this, this.AIObject, OrderType.Stay));   // Command: Stay

                list.Add(new InternalEntry(from, 6118, 14, this, this.AIObject, OrderType.Release)); // Release
            }
            //base.GetContextMenuEntries(from, list);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write((GuardSpawner)m_spawn);
            writer.Write((TownStone)m_Town);

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_spawn = (GuardSpawner)reader.ReadItem();
            m_Town = (TownStone)reader.ReadItem();
        }


        private class InternalEntry : ContextMenuEntry
        {
            private Mobile m_From;
            private BaseCreature m_Mobile;
            private BaseAI m_AI;
            private OrderType m_Order;

            public InternalEntry(Mobile from, int number, int range, BaseCreature mobile, BaseAI ai, OrderType order)
                : base(number, range)
            {
                m_From = from;
                m_Mobile = mobile;
                m_AI = ai;
                m_Order = order;
            }

            public override void OnClick()
            {
                if (!m_Mobile.Deleted && m_Mobile.Controlled && m_From == m_Mobile.ControlMaster)
                {
                    switch (m_Order)
                    {
                        case OrderType.Follow:
                            {
                                m_Mobile.Say("Je vous suis!");
                                m_Mobile.ControlTarget = m_From;
                                m_Mobile.ControlOrder = m_Order;
                                break;
                            }
                        case OrderType.Release:
                            {
                                m_Mobile.Controlled = false;
                                m_Mobile.ControlOrder = OrderType.Release;
                                m_Mobile.Say("A vos ordres");
                                m_Mobile.ControlMaster = null;
                                break;
                            }
                        default:
                            {
                                if (Math.Sqrt((m_Mobile.X - ((PomiGuard)(m_Mobile)).Town.X) *
                                        (m_Mobile.X - ((PomiGuard)(m_Mobile)).Town.X) +
                                        (m_Mobile.Y - ((PomiGuard)(m_Mobile)).Town.Y) *
                                        (m_Mobile.Y - ((PomiGuard)(m_Mobile)).Town.Y)) <
                                        ((PomiGuard)(m_Mobile)).Town.MaxDistance)
                                {
                                    m_Mobile.Say("Très bien!");
                                    m_Mobile.ControlOrder = m_Order;
                                }
                                else
                                {
                                    m_Mobile.Location = m_Mobile.Home;
                                    m_Mobile.Controlled = false;
                                    m_Mobile.ControlOrder = OrderType.Release;
                                    m_Mobile.ControlMaster = null;
                                }
                                break;
                            }
                    }
                }
            }

        }
    }
}