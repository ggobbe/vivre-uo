/**************************************
*     Bloody Bandage System V2.0      *
*      Distro files: Bandage.cs       *
*                                     *
*     Made by Demortris AKA Joeku     *
*             10/11/2005              *
*                                     *
* Anyone can modify/redistribute this *
*  DO NOT REMOVE/CHANGE THIS HEADER!  *
**************************************/

using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Gumps;

namespace Server.Items
{
    //[FlipableAttribute( 0xE20, 0xE22 )] 
    public class BloodyBandage : Item
    {

        [Constructable]
        public BloodyBandage()
            : this(1)
        {
        }

        [Constructable]
        public BloodyBandage(int amount)
            : base(0xE20)
        {
            Stackable = true;
            Weight = 0.1;
            Amount = amount;
        }

        public BloodyBandage(Serial serial)
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

        public override void OnDoubleClick(Mobile from)
        {
            from.RevealingAction();
            if (this.Amount > 1)
            {
                from.SendMessage("What will you wash the bloody bandages in?");
            }
            else
            {
                from.SendMessage("What will you wash the bloody bandage in?");
            }
            from.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private BloodyBandage m_Bandage;

            public InternalTarget(BloodyBandage bandage)
                : base(3, true, TargetFlags.Beneficial)
            {
                m_Bandage = bandage;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                StaticTarget a = targeted as StaticTarget;
                Item b = targeted as Item;
                LandTarget c = targeted as LandTarget;

                if (m_Bandage.Deleted)
                    return;

                if (targeted == a)
                {
                    if ((a.ItemID >= 5937 && a.ItemID <= 5978) || (a.ItemID >= 6038 && a.ItemID <= 6066) || (a.ItemID >= 6595 && a.ItemID <= 6636) || (a.ItemID >= 8093 && a.ItemID <= 8094) || (a.ItemID >= 8099 && a.ItemID <= 8138) || (a.ItemID >= 9299 && a.ItemID <= 9309) || (a.ItemID >= 13422 && a.ItemID <= 13525) || (a.ItemID >= 13549 && a.ItemID <= 13616) || (a.ItemID == 3707) || (a.ItemID >= 4088 && a.ItemID <= 4089) || (a.ItemID == 4104) || (a.ItemID == 5453) || (a.ItemID >= 5458 && a.ItemID <= 5460) || (a.ItemID == 5465) || (a.ItemID >= 2881 && a.ItemID <= 2884))
                    {
                        int amount = m_Bandage.Amount;
                        from.AddToBackpack(new Bandage(amount));
                        if (m_Bandage.Amount > 1)
                        {
                            from.SendMessage("You wash {0} bloody bandages and put the clean bandages in your pack.", amount);
                        }
                        else
                        {
                            from.SendMessage("You wash the bloody bandage and put the clean bandage in your pack.");
                        }
                        m_Bandage.Amount = 1;
                        m_Bandage.Consume();
                    }
                    else
                    {
                        from.SendMessage("You can only wash bloody bandages in water.");
                    }
                }
                else if (targeted == b)
                {
                    if ((b.ItemID >= 5937 && b.ItemID <= 5978) || (b.ItemID >= 6038 && b.ItemID <= 6066) || (b.ItemID >= 6595 && b.ItemID <= 6636) || (b.ItemID >= 8099 && b.ItemID <= 8138) || (b.ItemID >= 9299 && b.ItemID <= 9309) || (b.ItemID >= 13422 && b.ItemID <= 13525) || (b.ItemID >= 13549 && b.ItemID <= 13616) || (b.ItemID == 3707) || (b.ItemID == 4104) || (b.ItemID == 5453) || (b.ItemID >= 5458 && b.ItemID <= 5460) || (b.ItemID == 5465) || (b.ItemID >= 2881 && b.ItemID <= 2884))
                    {
                        int amount = m_Bandage.Amount;
                        from.AddToBackpack(new Bandage(amount));
                        if (m_Bandage.Amount > 1)
                        {
                            from.SendMessage("You wash {0} bloody bandages and put the clean bandages in your pack.", amount);
                        }
                        else
                        {
                            from.SendMessage("You wash the bloody bandage and put the clean bandage in your pack.");
                        }
                        m_Bandage.Amount = 1;
                        m_Bandage.Consume();
                    }
                    else if (b is BaseBeverage)
                    {
                        BaseBeverage bev = b as BaseBeverage;

                        if (bev.Content == BeverageType.Water)
                        {
                            if (bev.Quantity == 10 && m_Bandage.Amount <= 100)
                            {
                                int amount = m_Bandage.Amount;
                                from.AddToBackpack(new Bandage(amount));
                                if (m_Bandage.Amount > 1)
                                {
                                    from.SendMessage("You wash {0} bloody bandages and put the clean bandages in your pack.", amount);
                                }
                                else
                                {
                                    from.SendMessage("You wash the bloody bandage and put the clean bandage in your pack.");
                                }
                                if (m_Bandage.Amount >= 91)
                                {
                                    bev.Quantity = (bev.Quantity - 10);
                                }
                                else if (m_Bandage.Amount >= 81)
                                {
                                    bev.Quantity = (bev.Quantity - 9);
                                }
                                else if (m_Bandage.Amount >= 71)
                                {
                                    bev.Quantity = (bev.Quantity - 8);
                                }
                                else if (m_Bandage.Amount >= 61)
                                {
                                    bev.Quantity = (bev.Quantity - 7);
                                }
                                else if (m_Bandage.Amount >= 51)
                                {
                                    bev.Quantity = (bev.Quantity - 6);
                                }
                                else if (m_Bandage.Amount >= 41)
                                {
                                    bev.Quantity = (bev.Quantity - 5);
                                }
                                else if (m_Bandage.Amount >= 31)
                                {
                                    bev.Quantity = (bev.Quantity - 4);
                                }
                                else if (m_Bandage.Amount >= 21)
                                {
                                    bev.Quantity = (bev.Quantity - 3);
                                }
                                else if (m_Bandage.Amount >= 11)
                                {
                                    bev.Quantity = (bev.Quantity - 2);
                                }
                                else if (m_Bandage.Amount >= 1)
                                {
                                    bev.Quantity = (bev.Quantity - 1);
                                }
                                from.SendMessage("Some of the water in the container has been depleted.");
                                m_Bandage.Amount = 1;
                                m_Bandage.Consume();
                            }
                            else if (bev.Quantity == 9 && m_Bandage.Amount <= 90)
                            {
                                int amount = m_Bandage.Amount;
                                from.AddToBackpack(new Bandage(amount));
                                if (m_Bandage.Amount > 1)
                                {
                                    from.SendMessage("You wash {0} bloody bandages and put the clean bandages in your pack.", amount);
                                }
                                else
                                {
                                    from.SendMessage("You wash the bloody bandage and put the clean bandage in your pack.");
                                }
                                if (m_Bandage.Amount >= 81)
                                {
                                    bev.Quantity = (bev.Quantity - 9);
                                }
                                else if (m_Bandage.Amount >= 71)
                                {
                                    bev.Quantity = (bev.Quantity - 8);
                                }
                                else if (m_Bandage.Amount >= 61)
                                {
                                    bev.Quantity = (bev.Quantity - 7);
                                }
                                else if (m_Bandage.Amount >= 51)
                                {
                                    bev.Quantity = (bev.Quantity - 6);
                                }
                                else if (m_Bandage.Amount >= 41)
                                {
                                    bev.Quantity = (bev.Quantity - 5);
                                }
                                else if (m_Bandage.Amount >= 31)
                                {
                                    bev.Quantity = (bev.Quantity - 4);
                                }
                                else if (m_Bandage.Amount >= 21)
                                {
                                    bev.Quantity = (bev.Quantity - 3);
                                }
                                else if (m_Bandage.Amount >= 11)
                                {
                                    bev.Quantity = (bev.Quantity - 2);
                                }
                                else if (m_Bandage.Amount >= 1)
                                {
                                    bev.Quantity = (bev.Quantity - 1);
                                }
                                from.SendMessage("Some of the water in the container has been depleted.");
                                m_Bandage.Amount = 1;
                                m_Bandage.Consume();
                            }
                            else if (bev.Quantity == 8 && m_Bandage.Amount <= 80)
                            {
                                int amount = m_Bandage.Amount;
                                from.AddToBackpack(new Bandage(amount));
                                if (m_Bandage.Amount > 1)
                                {
                                    from.SendMessage("You wash {0} bloody bandages and put the clean bandages in your pack.", amount);
                                }
                                else
                                {
                                    from.SendMessage("You wash the bloody bandage and put the clean bandage in your pack.");
                                }
                                if (m_Bandage.Amount >= 71)
                                {
                                    bev.Quantity = (bev.Quantity - 8);
                                }
                                else if (m_Bandage.Amount >= 61)
                                {
                                    bev.Quantity = (bev.Quantity - 7);
                                }
                                else if (m_Bandage.Amount >= 51)
                                {
                                    bev.Quantity = (bev.Quantity - 6);
                                }
                                else if (m_Bandage.Amount >= 41)
                                {
                                    bev.Quantity = (bev.Quantity - 5);
                                }
                                else if (m_Bandage.Amount >= 31)
                                {
                                    bev.Quantity = (bev.Quantity - 4);
                                }
                                else if (m_Bandage.Amount >= 21)
                                {
                                    bev.Quantity = (bev.Quantity - 3);
                                }
                                else if (m_Bandage.Amount >= 11)
                                {
                                    bev.Quantity = (bev.Quantity - 2);
                                }
                                else if (m_Bandage.Amount >= 1)
                                {
                                    bev.Quantity = (bev.Quantity - 1);
                                }
                                from.SendMessage("Some of the water in the container has been depleted.");
                                m_Bandage.Amount = 1;
                                m_Bandage.Consume();
                            }
                            else if (bev.Quantity == 7 && m_Bandage.Amount <= 70)
                            {
                                int amount = m_Bandage.Amount;
                                from.AddToBackpack(new Bandage(amount));
                                if (m_Bandage.Amount > 1)
                                {
                                    from.SendMessage("You wash {0} bloody bandages and put the clean bandages in your pack.", amount);
                                }
                                else
                                {
                                    from.SendMessage("You wash the bloody bandage and put the clean bandage in your pack.");
                                }
                                if (m_Bandage.Amount >= 61)
                                {
                                    bev.Quantity = (bev.Quantity - 7);
                                }
                                else if (m_Bandage.Amount >= 51)
                                {
                                    bev.Quantity = (bev.Quantity - 6);
                                }
                                else if (m_Bandage.Amount >= 41)
                                {
                                    bev.Quantity = (bev.Quantity - 5);
                                }
                                else if (m_Bandage.Amount >= 31)
                                {
                                    bev.Quantity = (bev.Quantity - 4);
                                }
                                else if (m_Bandage.Amount >= 21)
                                {
                                    bev.Quantity = (bev.Quantity - 3);
                                }
                                else if (m_Bandage.Amount >= 11)
                                {
                                    bev.Quantity = (bev.Quantity - 2);
                                }
                                else if (m_Bandage.Amount >= 1)
                                {
                                    bev.Quantity = (bev.Quantity - 1);
                                }
                                from.SendMessage("Some of the water in the container has been depleted.");
                                m_Bandage.Amount = 1;
                                m_Bandage.Consume();
                            }
                            else if (bev.Quantity == 6 && m_Bandage.Amount <= 60)
                            {
                                int amount = m_Bandage.Amount;
                                from.AddToBackpack(new Bandage(amount));
                                if (m_Bandage.Amount > 1)
                                {
                                    from.SendMessage("You wash {0} bloody bandages and put the clean bandages in your pack.", amount);
                                }
                                else
                                {
                                    from.SendMessage("You wash the bloody bandage and put the clean bandage in your pack.");
                                }
                                if (m_Bandage.Amount >= 51)
                                {
                                    bev.Quantity = (bev.Quantity - 6);
                                }
                                else if (m_Bandage.Amount >= 41)
                                {
                                    bev.Quantity = (bev.Quantity - 5);
                                }
                                else if (m_Bandage.Amount >= 31)
                                {
                                    bev.Quantity = (bev.Quantity - 4);
                                }
                                else if (m_Bandage.Amount >= 21)
                                {
                                    bev.Quantity = (bev.Quantity - 3);
                                }
                                else if (m_Bandage.Amount >= 11)
                                {
                                    bev.Quantity = (bev.Quantity - 2);
                                }
                                else if (m_Bandage.Amount >= 1)
                                {
                                    bev.Quantity = (bev.Quantity - 1);
                                }
                                from.SendMessage("Some of the water in the container has been depleted.");
                                m_Bandage.Amount = 1;
                                m_Bandage.Consume();
                            }
                            else if (bev.Quantity == 5 && m_Bandage.Amount <= 50)
                            {
                                int amount = m_Bandage.Amount;
                                from.AddToBackpack(new Bandage(amount));
                                if (m_Bandage.Amount > 1)
                                {
                                    from.SendMessage("You wash {0} bloody bandages and put the clean bandages in your pack.", amount);
                                }
                                else
                                {
                                    from.SendMessage("You wash the bloody bandage and put the clean bandage in your pack.");
                                }
                                if (m_Bandage.Amount >= 41)
                                {
                                    bev.Quantity = (bev.Quantity - 5);
                                }
                                else if (m_Bandage.Amount >= 31)
                                {
                                    bev.Quantity = (bev.Quantity - 4);
                                }
                                else if (m_Bandage.Amount >= 21)
                                {
                                    bev.Quantity = (bev.Quantity - 3);
                                }
                                else if (m_Bandage.Amount >= 11)
                                {
                                    bev.Quantity = (bev.Quantity - 2);
                                }
                                else if (m_Bandage.Amount >= 1)
                                {
                                    bev.Quantity = (bev.Quantity - 1);
                                }
                                from.SendMessage("Some of the water in the container has been depleted.");
                                m_Bandage.Amount = 1;
                                m_Bandage.Consume();
                            }
                            else if (bev.Quantity == 4 && m_Bandage.Amount <= 40)
                            {
                                int amount = m_Bandage.Amount;
                                from.AddToBackpack(new Bandage(amount));
                                if (m_Bandage.Amount > 1)
                                {
                                    from.SendMessage("You wash {0} bloody bandages and put the clean bandages in your pack.", amount);
                                }
                                else
                                {
                                    from.SendMessage("You wash the bloody bandage and put the clean bandage in your pack.");
                                }
                                if (m_Bandage.Amount >= 31)
                                {
                                    bev.Quantity = (bev.Quantity - 4);
                                }
                                else if (m_Bandage.Amount >= 21)
                                {
                                    bev.Quantity = (bev.Quantity - 3);
                                }
                                else if (m_Bandage.Amount >= 11)
                                {
                                    bev.Quantity = (bev.Quantity - 2);
                                }
                                else if (m_Bandage.Amount >= 1)
                                {
                                    bev.Quantity = (bev.Quantity - 1);
                                }
                                from.SendMessage("Some of the water in the container has been depleted.");
                                m_Bandage.Amount = 1;
                                m_Bandage.Consume();
                            }
                            else if (bev.Quantity == 3 && m_Bandage.Amount <= 30)
                            {
                                int amount = m_Bandage.Amount;
                                from.AddToBackpack(new Bandage(amount));
                                if (m_Bandage.Amount > 1)
                                {
                                    from.SendMessage("You wash {0} bloody bandages and put the clean bandages in your pack.", amount);
                                }
                                else
                                {
                                    from.SendMessage("You wash the bloody bandage and put the clean bandage in your pack.");
                                }
                                if (m_Bandage.Amount >= 21)
                                {
                                    bev.Quantity = (bev.Quantity - 3);
                                }
                                else if (m_Bandage.Amount >= 11)
                                {
                                    bev.Quantity = (bev.Quantity - 2);
                                }
                                else if (m_Bandage.Amount >= 1)
                                {
                                    bev.Quantity = (bev.Quantity - 1);
                                }
                                from.SendMessage("Some of the water in the container has been depleted.");
                                m_Bandage.Amount = 1;
                                m_Bandage.Consume();
                            }
                            else if (bev.Quantity == 2 && m_Bandage.Amount <= 20)
                            {
                                int amount = m_Bandage.Amount;
                                from.AddToBackpack(new Bandage(amount));
                                if (m_Bandage.Amount > 1)
                                {
                                    from.SendMessage("You wash {0} bloody bandages and put the clean bandages in your pack.", amount);
                                }
                                else
                                {
                                    from.SendMessage("You wash the bloody bandage and put the clean bandage in your pack.");
                                }
                                if (m_Bandage.Amount >= 11)
                                {
                                    bev.Quantity = (bev.Quantity - 2);
                                }
                                else if (m_Bandage.Amount >= 1)
                                {
                                    bev.Quantity = (bev.Quantity - 1);
                                }
                                from.SendMessage("Some of the water in the container has been depleted.");
                                m_Bandage.Amount = 1;
                                m_Bandage.Consume();
                            }
                            else if (bev.Quantity == 1 && m_Bandage.Amount <= 10)
                            {
                                int amount = m_Bandage.Amount;
                                from.AddToBackpack(new Bandage(amount));
                                if (m_Bandage.Amount > 1)
                                {
                                    from.SendMessage("You wash {0} bloody bandages and put the clean bandages in your pack.", amount);
                                }
                                else
                                {
                                    from.SendMessage("You wash the bloody bandage and put the clean bandage in your pack.");
                                }
                                from.SendMessage("Some of the water in the container has been depleted.");
                                bev.Quantity = (bev.Quantity - 1);
                                m_Bandage.Amount = 1;
                                m_Bandage.Consume();
                            }
                            else
                            {
                                from.SendMessage("There isn't enough water in that to wash with.");
                            }
                        }
                        else
                        {
                            from.SendMessage("You can only wash bloody bandages in water.");
                        }
                    }
                    else
                    {
                        from.SendMessage("You can only wash bloody bandages in water.");
                    }
                }
                else if (targeted == c)
                {
                    if ((c.TileID >= 168 && c.TileID <= 171) || (c.TileID >= 310 && c.TileID <= 311))
                    {
                        int amount = m_Bandage.Amount;
                        from.AddToBackpack(new Bandage(amount));
                        if (m_Bandage.Amount > 1)
                        {
                            from.SendMessage("You wash {0} bloody bandages and put the clean bandages in your pack.", amount);
                        }
                        else
                        {
                            from.SendMessage("You wash the bloody bandage and put the clean bandage in your pack.");
                        }
                        m_Bandage.Amount = 1;
                        m_Bandage.Consume();
                    }
                    else
                    {
                        from.SendMessage("You can only wash bloody bandages in water.");
                    }
                }
                else
                {
                    from.SendMessage("You can only wash bloody bandages in water.");
                }
            }
        }
    }
}