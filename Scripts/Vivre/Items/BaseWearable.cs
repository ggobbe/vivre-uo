using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public class BaseWearable : Item
    {
        private bool MessageColorization = true; // Messages will retain color of item equipped
        private int EquipSound = 0x57; 

        private Mobile m_Mobile;
        private Container m_Cont;

        public override void OnDoubleClick(Mobile from)
        {
            if (!this.Movable || this.Layer == Layer.Invalid || this.Parent is Corpse)
                return;

            if (!from.InRange(this.GetWorldLocation(), 2)) // Do not equip items further than 2 tiles
            {
                from.SendLocalizedMessage(500446); // That is too far away.
                return;
            }

            m_Mobile = from;
            m_Cont = this.Parent as Container;
            bool backpack = from.Backpack != null;

            if (from.FindItemOnLayer(this.Layer) == this)
            {
                if (!backpack)
                {
                    from.SendMessage(35, "You have no inventory. Equip inventory bag first.");
                }
                else if (from.Backpack.TryDropItem(from, this, true))
                {
                    from.PlaySound(EquipSound);
                    //from.SendMessage(MessageColorization ? this.Hue : 55, "You put {0} into your backpack.", this.Name != null ? this.Name : this.ItemData.Name);
                }
                return;
            }

            if (this.Layer == Layer.TwoHanded && !(this is BaseShield)) // If item is Two-handed weapon
            {
                if (from.FindItemOnLayer(Layer.OneHanded) != null || from.FindItemOnLayer(this.Layer) != null)
                {
                    if (!backpack)
                    {
                        from.SendMessage(35, "You have no inventory. Equip inventory bag first.");
                        return;
                    }
                }
                if (from.FindItemOnLayer(Layer.OneHanded) != null && from.FindItemOnLayer(this.Layer) == null)
                {
                    Item item = from.FindItemOnLayer(Layer.OneHanded);
                    DoSwap(item);
                    from.PlaySound(EquipSound);
                    return;
                }
                else if (from.FindItemOnLayer(Layer.OneHanded) != null && from.FindItemOnLayer(this.Layer) != null)
                {
                    Item firstHand = from.FindItemOnLayer(Layer.OneHanded);
                    Item secondHand = from.FindItemOnLayer(this.Layer);
                    DoSwap(firstHand, secondHand);
                    from.PlaySound(EquipSound);
                    return;
                }
            }

            if (from.FindItemOnLayer(this.Layer) != null)
            {
                if (!backpack)
                {
                    from.SendMessage(35, "You have no inventory. Equip inventory bag first.");
                    return;
                }
                Item item = from.FindItemOnLayer(this.Layer);
                DoSwap(item);
                from.PlaySound(EquipSound);
            }
            else if (this.Layer == Layer.OneHanded && from.FindItemOnLayer(Layer.TwoHanded) != null && !(from.FindItemOnLayer(Layer.TwoHanded) is BaseShield))
            {
                Item item = from.FindItemOnLayer(Layer.TwoHanded);
                DoSwap(item);
                from.PlaySound(EquipSound);
            }
            else
            {
                from.EquipItem(this);
                if (from.FindItemOnLayer(this.Layer) == this)
                {
                    from.PlaySound(EquipSound);
                    //from.SendMessage(MessageColorization ? this.Hue : 75, "You equipped {0}.", this.Name != null ? this.Name : this.ItemData.Name);
                }
            }
        }

        public void DoSwap(Item item1)
        {
            DoSwap(item1, null);
        }

        public void DoSwap(Item firstHand, Item secondHand)
        {
            if (secondHand == null)
            {
                if (m_Cont == null)
                {
                    //m_Mobile.SendMessage(MessageColorization ? this.Hue : 49, "You swapped {0} for {1}.", this.Name != null ? this.Name : this.ItemData.Name, firstHand.Name != null ? firstHand.Name : firstHand.ItemData.Name);
                    firstHand.MoveToWorld(this.Location);
                    m_Mobile.EquipItem(this);
                }
                else if (m_Cont.TryDropItem(m_Mobile, firstHand, true))
                {
                    //m_Mobile.SendMessage(MessageColorization ? this.Hue : 49, "You swapped {0} for {1}.", this.Name != null ? this.Name : this.ItemData.Name, firstHand.Name != null ? firstHand.Name : firstHand.ItemData.Name);
                    firstHand.Location = this.Location;
                    m_Mobile.EquipItem(this);
                }
            }
            else
            {
                if (m_Cont == null)
                {
                    //m_Mobile.SendMessage(MessageColorization ? this.Hue : 49, "You swapped {0} for {1} and {2}.", this.Name != null ? this.Name : this.ItemData.Name, firstHand.Name != null ? firstHand.Name : firstHand.ItemData.Name, secondHand.Name != null ? secondHand.Name : secondHand.ItemData.Name);
                    firstHand.MoveToWorld(this.Location);
                    secondHand.MoveToWorld(this.Location);
                    m_Mobile.EquipItem(this);
                }
                else if (m_Cont.TryDropItem(m_Mobile, firstHand, true) && m_Cont.TryDropItem(m_Mobile, secondHand, true))
                {
                    //m_Mobile.SendMessage(MessageColorization ? this.Hue : 49, "You swapped {0} for {1} and {2}.", this.Name != null ? this.Name : this.ItemData.Name, firstHand.Name != null ? firstHand.Name : firstHand.ItemData.Name, secondHand.Name != null ? secondHand.Name : secondHand.ItemData.Name);
                    firstHand.Location = this.Location;
                    secondHand.Location = this.Location;
                    m_Mobile.EquipItem(this);
                }
            }
        }

        public BaseWearable(int itemID)
            : base(itemID)
        {
        }

        public BaseWearable(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
}