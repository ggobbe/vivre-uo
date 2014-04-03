
using System;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Engines.Craft;
using Server.Mobiles;
using Server.Regions;

namespace Server.Items
{
    public abstract class BaseOre : Item
    {
        private CraftResource m_Resource;

        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource Resource
        {
            get { return m_Resource; }
            set { m_Resource = value; InvalidateProperties(); }
        }

        public abstract BaseIngot GetIngot();

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write((int)m_Resource);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        m_Resource = (CraftResource)reader.ReadInt();
                        break;
                    }
                case 0:
                    {
                        OreInfo info;

                        switch (reader.ReadInt())
                        {
                            case 0: info = OreInfo.MIron; break;
                            case 1: info = OreInfo.MBronze; break;
                            case 2: info = OreInfo.MGold; break;
                            case 3: info = OreInfo.MCopper; break;
                            case 4: info = OreInfo.MOldcopper; break;
                            case 5: info = OreInfo.MDullcopper; break;
                            case 6: info = OreInfo.MSilver; break;
                            case 7: info = OreInfo.MShadow; break;
                            case 8: info = OreInfo.MBloodrock; break;
                            case 9: info = OreInfo.MBlackrock; break;
                            case 10: info = OreInfo.MMytheril; break;
                            case 11: info = OreInfo.MRose; break;
                            case 12: info = OreInfo.MVerite; break;
                            case 13: info = OreInfo.MAgapite; break;
                            case 14: info = OreInfo.MRusty; break;
                            case 15: info = OreInfo.MValorite; break;
                            case 16: info = OreInfo.MDragon; break;
                            case 17: info = OreInfo.MTitan; break;
                            case 18: info = OreInfo.MCrystaline; break;
                            case 19: info = OreInfo.MKrynite; break;
                            case 20: info = OreInfo.MVulcan; break;
                            case 21: info = OreInfo.MBloodcrest; break;
                            case 22: info = OreInfo.MElvin; break;
                            case 23: info = OreInfo.MAcid; break;
                            case 24: info = OreInfo.MAqua; break;
                            case 25: info = OreInfo.MEldar; break;
                            case 26: info = OreInfo.MGlowing; break;
                            case 27: info = OreInfo.MGorgan; break;
                            case 28: info = OreInfo.MSandrock; break;
                            case 29: info = OreInfo.MSteel; break;
                            default: info = null; break;
                        }

                        m_Resource = CraftResources.GetFromOreInfo(info);
                        break;
                    }
            }
        }

        private static int RandomSize()
        {
            double rand = Utility.RandomDouble();

            if (rand < 0.12)
                return 0x19B7;
            else if (rand < 0.18)
                return 0x19B8;
            else if (rand < 0.25)
                return 0x19BA;
            else
                return 0x19B9;
        }

        public BaseOre(CraftResource resource)
            : this(resource, 1)
        {
        }

        public BaseOre(CraftResource resource, int amount)
            : base(RandomSize())
        {
            Stackable = true;
            Amount = amount;
            Hue = CraftResources.GetHue(resource);

            m_Resource = resource;
        }

        public BaseOre(Serial serial)
            : base(serial)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            if (Amount > 1)
                list.Add(1050039, "{0}\t#{1}", Amount, 1026583); // ~1_NUMBER~ ~2_ITEMNAME~
            else
                list.Add(1026583); // ore
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (!CraftResources.IsStandard(m_Resource))
            {
                int num = CraftResources.GetLocalizationNumber(m_Resource);

                if (num > 0)
                    list.Add(num);
                else
                    list.Add(CraftResources.GetName(m_Resource));
            }
        }

        /*public override int LabelNumber
        {
            get
            {
                if ( m_Resource >= CraftResource.DullCopper && m_Resource <= CraftResource.Valorite )
                    return 1042845 + (int)(m_Resource - CraftResource.DullCopper);

                return 1042853; // iron ore;
            }
        }*/

        public override void OnDoubleClick(Mobile from)
        {
            if (!Movable)
                return;

            if (RootParent is BaseCreature)
            {
                from.SendLocalizedMessage(500447); // That is not accessible
                return;
            }
            else if (from.InRange(this.GetWorldLocation(), 2))
            {
                from.SendLocalizedMessage(501971); // Select the forge on which to smelt the ore, or another pile of ore with which to combine it.
                from.Target = new InternalTarget(this);
            }
            else
            {
                from.SendLocalizedMessage(501976); // The ore is too far away.
            }
        }

        private class InternalTarget : Target
        {
            private BaseOre m_Ore;

            public InternalTarget(BaseOre ore)
                : base(2, false, TargetFlags.None)
            {
                m_Ore = ore;
            }

            private bool IsForge(object obj)
            {
                if (Core.ML && obj is Mobile && ((Mobile)obj).IsDeadBondedPet)
                    return false;

                if (obj.GetType().IsDefined(typeof(ForgeAttribute), false))
                    return true;

                int itemID = 0;

                if (obj is Item)
                    itemID = ((Item)obj).ItemID;
                else if (obj is StaticTarget)
                    itemID = ((StaticTarget)obj).ItemID;

                return (itemID == 4017 || (itemID >= 6522 && itemID <= 6569));
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Ore.Deleted)
                    return;

                if (!from.InRange(m_Ore.GetWorldLocation(), 2))
                {
                    from.SendLocalizedMessage(501976); // The ore is too far away.
                    return;
                }

                #region Combine Ore
                if (targeted is BaseOre)
                {
                    BaseOre ore = (BaseOre)targeted;
                    if (!ore.Movable)
                        return;
                    if (!ore.Stackable || !m_Ore.Stackable)
                        return;
                    else if (m_Ore == ore)
                    {
                        from.SendLocalizedMessage(501972); // Select another pile or ore with which to combine this.
                        from.Target = new InternalTarget(ore);
                        return;
                    }
                    else if (ore.Resource != m_Ore.Resource)
                    {
                        from.SendLocalizedMessage(501979); // You cannot combine ores of different metals.
                        return;
                    }

                    int worth = ore.Amount;
                    if (ore.ItemID == 0x19B9)
                        worth *= 8;
                    else if (ore.ItemID == 0x19B7)
                        worth *= 2;
                    else
                        worth *= 4;
                    int sourceWorth = m_Ore.Amount;
                    if (m_Ore.ItemID == 0x19B9)
                        sourceWorth *= 8;
                    else if (m_Ore.ItemID == 0x19B7)
                        sourceWorth *= 2;
                    else
                        sourceWorth *= 4;
                    worth += sourceWorth;

                    int plusWeight = 0;
                    int newID = ore.ItemID;
                    if (ore.DefaultWeight != m_Ore.DefaultWeight)
                    {
                        if (ore.ItemID == 0x19B7 || m_Ore.ItemID == 0x19B7)
                        {
                            newID = 0x19B7;
                        }
                        else if (ore.ItemID == 0x19B9)
                        {
                            newID = m_Ore.ItemID;
                            plusWeight = ore.Amount * 2;
                        }
                        else
                        {
                            plusWeight = m_Ore.Amount * 2;
                        }
                    }

                    if ((ore.ItemID == 0x19B9 && worth > 120000) || ((ore.ItemID == 0x19B8 || ore.ItemID == 0x19BA) && worth > 60000) || (ore.ItemID == 0x19B7 && worth > 30000))
                    {
                        from.SendLocalizedMessage(1062844); // There is too much ore to combine.
                        return;
                    }
                    else if (ore.RootParent is Mobile && (plusWeight + ((Mobile)ore.RootParent).Backpack.TotalWeight) > ((Mobile)ore.RootParent).Backpack.MaxWeight)
                    {
                        from.SendLocalizedMessage(501978); // The weight is too great to combine in a container.
                        return;
                    }

                    ore.ItemID = newID;

                    if (ore.ItemID == 0x19B9)
                        ore.Amount = worth / 8;
                    else if (ore.ItemID == 0x19B7)
                        ore.Amount = worth / 2;
                    else
                        ore.Amount = worth / 4;

                    m_Ore.Delete();
                    return;
                }
                #endregion

                if (IsForge(targeted))
                {
                    double difficulty;

                    switch (m_Ore.Resource)
                    {
                        default: difficulty = 50.0; break;
                        case CraftResource.MRusty: difficulty = 25.0; break;
                        case CraftResource.MOldcopper: difficulty = 30.0; break;
                        case CraftResource.MDullcopper: difficulty = 40.0; break;
                        case CraftResource.MShadow: difficulty = 60.0; break;
                        case CraftResource.MCopper: difficulty = 50.0; break;
                        case CraftResource.MBronze: difficulty = 55.0; break;
                        case CraftResource.MGold: difficulty = 65.0; break;
                        case CraftResource.MRose: difficulty = 45.0; break;
                        case CraftResource.MAgapite: difficulty = 70.0; break;
                        case CraftResource.MValorite: difficulty = 90.0; break;
                        case CraftResource.MBloodrock: difficulty = 93.0; break;
                        case CraftResource.MVerite: difficulty = 95.0; break;
                        case CraftResource.MSilver: difficulty = 85.0; break;
                        case CraftResource.MDragon: difficulty = 82.0; break;
                        case CraftResource.MTitan: difficulty = 75.0; break;
                        case CraftResource.MCrystaline: difficulty = 95.0; break;
                        case CraftResource.MKrynite: difficulty = 95.0; break;
                        case CraftResource.MVulcan: difficulty = 0; break;
                        case CraftResource.MBloodcrest: difficulty = 95.0; break;
                        case CraftResource.MElvin: difficulty = 95.0; break;
                        case CraftResource.MAcid: difficulty = 95.0; break;
                        case CraftResource.MAqua: difficulty = 95.0; break;
                        case CraftResource.MEldar: difficulty = 75.0; break;
                        case CraftResource.MGlowing: difficulty = 0.0; break;
                        case CraftResource.MGorgan: difficulty = 95.0; break;
                        case CraftResource.MSteel: difficulty = 95.5; break;
                        case CraftResource.MSandrock: difficulty = 95.0; break;
                        case CraftResource.MMytheril: difficulty = 97.5; break;
                        case CraftResource.MBlackrock: difficulty = 98.0; break;
                    }

                    double minSkill = difficulty - 25.0;
                    double maxSkill = difficulty + 25.0;

                    if (difficulty > 50.0 && difficulty > from.Skills[SkillName.Mining].Value)
                    {
                        from.SendLocalizedMessage(501986); // You have no idea how to smelt this strange ore!
                        return;
                    }

                    if (m_Ore.ItemID == 0x19B7 && m_Ore.Amount < 2)
                    {
                        from.SendLocalizedMessage(501987); // There is not enough metal-bearing ore in this pile to make an ingot.
                        return;
                    }

                    if (from.CheckTargetSkill(SkillName.Mining, targeted, minSkill, maxSkill))
                    {
                        int toConsume = m_Ore.Amount;

                        if (toConsume <= 0)
                        {
                            from.SendLocalizedMessage(501987); // There is not enough metal-bearing ore in this pile to make an ingot.
                        }
                        else
                        {
                            if (toConsume > 30000)
                                toConsume = 30000;

                            int ingotAmount;

                            if (m_Ore.Resource == CraftResource.MTitan)
                            {
                                int helper = 0;
                                foreach (Mobile m in from.GetMobilesInRange(1))
                                {
                                    // Scriptiz : vérifions qu'il s'agit bien d'un playermobile
                                    PlayerMobile pm = m as PlayerMobile;
                                    if (pm == null) continue;

                                    if (pm.Skills[SkillName.ItemID].Value > 60 && pm != from)
                                        helper++;
                                }
                                if (helper == 0)
                                {
                                    from.SendMessage("Il faudrait un identificateur avisé pour vous assister dans ce travail minutieux");
                                    return;
                                }
                            }
                            if (m_Ore.Resource == CraftResource.MSilver || m_Ore.Resource == CraftResource.MGlowing || m_Ore.Resource == CraftResource.MVulcan)
                            {
                                if (!(targeted is SpecialForge))
                                {
                                    from.SendMessage("Il vous faut trouver la forge appropriée pour ce métal");
                                    return;
                                }
                                SpecialForge targ = (SpecialForge)targeted;
                                
                                if (m_Ore.Resource == CraftResource.MSilver && targ.Resource != CraftResource.MShadow)
                                {
                                    from.SendMessage("Il vous faut trouver une forge d'ombre");
                                    return;
                                }

                                if (m_Ore.Resource == CraftResource.MGlowing)
                                  { 
                                    if(targ.Resource != CraftResource.MGlowing)
                                    {
                                    from.SendMessage("Il vous faut trouver une forge céleste");
                                    return;
                                    }
                                    else
                                    {
                                        
int hours = 0;
			int minutes = 0;
		
			Clock.GetTime( from.Map, from.Location.X, from.Location.Y, out hours, out minutes );
			
			if ( hours > 18 && hours <7)
                                        {
                                            from.SendMessage("Le ciel n'est pas assez dégagé");
                                            return;
                                        }
                                        else
                                        {
                                            from.BeginTarget(2, false, TargetFlags.None, new TargetStateCallback(TransformGlowingTarget), m_Ore);
                                            from.SendMessage("Ce métal devrait scintiller comme un millier de pierres très précieuses!");
                                            return;
                                        }
                                    }
                                }

                                if (m_Ore.Resource == CraftResource.MVulcan)
                                {
                                    if (targ.Resource != CraftResource.MVulcan)
                                    {
                                        from.SendMessage("Il vous faut trouver une forge céleste");
                                        return;
                                    }
                                    else if (from.FireResistance <= 48)
                                    {
                                        from.SendMessage("Vous risqueriez de périr brûler par l'intensité des flammes.");
                                        return;
                                    }
                                    else
                                    {
                                        from.BeginTarget(2, false, TargetFlags.None, new TargetStateCallback(TransformVulcanTarget), m_Ore);
                                        from.SendMessage("Ce métal devrait bruler comme un millier de pierres ardentes!");
                                        return;
                                    }

                                }

                                
                            }
                           
                            if (m_Ore.Resource == CraftResource.MVerite)
                            {
                                if (from.Karma <= 2000)
                                {
                                    from.SendMessage("Il vous faut devenir un forgeron plus vertueux!");
                                    return;
                                }
                            }

                            if (m_Ore.Resource == CraftResource.MDragon)
                            {
                                if (from.Region is Regions.HouseRegion || from.Region is Regions.TownRegion)
                                {
                                    from.SendMessage("Il ne serait pas adéquat de tenter cela ici");
                                    return;
                                }

                                BaseCreature Surprise = null;
                                int type = Utility.Random(100);
                                
                                if (type == 33 )
                                    Surprise = new AngryGreaterDragon();
                                else if (type < 15)
                                    Surprise = new AngryDragon();
                                else if (type >= 50)
                                    Surprise = new AngryDrake();

                                if (Surprise != null)
                                {
                                    Surprise.SummonMaster = from;   // to know who invoked it !
                                    Surprise.MoveToWorld(from.Location, from.Map);
                                    Surprise.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "*La bête atterri près de vous, l'air furieuse*");
                                }
                            }

                            if (m_Ore.Resource == CraftResource.MRose)
                            {
                                if (!from.Female)
                                {
                                    from.SendMessage("Vous n'avez pas les mains assez douces et tendres pour un métal aussi joli");

                                    if (Utility.RandomBool())
                                        m_Ore.Consume(1);

                                    return;

                                }
                            }

                            if (m_Ore.Resource == CraftResource.MEldar)
                            {
                                if((DateTime.Now - from.CreationTime).TotalDays < 35)
                                {
                                    from.SendMessage("Vous êtes trop jeune pour fondre ce métal");
                                    return;
                                }
                            }
                            if (m_Ore.ItemID == 0x19B7)
                            {
                                ingotAmount = toConsume / 2;

                                if (toConsume % 2 != 0)
                                    --toConsume;
                            }
                            else if (m_Ore.ItemID == 0x19B9)
                            {
                                ingotAmount = toConsume * 2;
                            }
                            else
                            {
                                ingotAmount = toConsume;
                            }

                            BaseIngot ingot = m_Ore.GetIngot();
                            ingot.Amount = ingotAmount;

                            m_Ore.Consume(toConsume);
                            from.AddToBackpack(ingot);

                            from.SendLocalizedMessage(501988); // You smelt the ore removing the impurities and put the metal in your backpack.
                        }
                    }
                    else
                    {
                        if (m_Ore.Amount < 2)
                        {
                            if (m_Ore.ItemID == 0x19B9)
                                m_Ore.ItemID = 0x19B8;
                            else
                                m_Ore.ItemID = 0x19B7;
                        }
                        else
                        {
                            m_Ore.Amount /= 2;
                        }

                        from.SendLocalizedMessage(501990); // You burn away the impurities but are left with less useable metal.
                    }
                }
            }
            public void TransformGlowingTarget(Mobile from, object obj, object state)
            {
                BaseOre ore = state as BaseOre;

                if (!(obj is BlueDiamond))
                {
                    from.SendMessage("Cela n'est pas assez éclatant");
                    return;
                }

                BlueDiamond targ = (BlueDiamond)obj;

                int chance = Math.Min(targ.Amount, 30)+((int)from.Skills[SkillName.Mining].Value/5);

                if (Utility.Random(0,50) < chance)
                {
                    ore.Consume(Math.Min(ore.Amount, 50) / 2);
                    targ.Consume(Utility.Random(Math.Min(targ.Amount, 30)) / 2);
                    from.SendMessage("Vous n'arrivez pas à le rendre plus éclatant", chance);
                    return;
                }
                else
                {
                    GlowingIngot success = new GlowingIngot();
                    success.Amount = Math.Min(ore.Amount, 50) * 2;
                    from.AddToBackpack(success);
                    from.SendMessage("Le métal brille de milles éclats!");
                    ore.Consume(Math.Min(ore.Amount, 50));
                    targ.Consume(Math.Min(targ.Amount, 30));
                    return;
                }
            }
            public void TransformVulcanTarget(Mobile from, object obj, object state)
            {
                BaseOre ore = state as BaseOre;

                if (!(obj is FireRuby))
                {
                    from.SendMessage("Cela n'est pas assez ardent");
                    return;
                }

                FireRuby targ = (FireRuby)obj;

                int chance = Math.Min(targ.Amount, 30) + ((int)from.Skills[SkillName.Mining].Value / 5);

                if (Utility.Random(0, 50) < chance)
                {
                    ore.Consume(Math.Min(ore.Amount, 50) / 2);
                    targ.Consume(Utility.Random(Math.Min(targ.Amount, 30)) / 2);
                    from.SendMessage("Vous n'arrivez pas à le rendre plus ardent", chance);
                    return;
                }
                else
                {
                    VulcanIngot success = new VulcanIngot();
                    success.Amount = Math.Min(ore.Amount, 50) * 2;
                    from.AddToBackpack(success);
                    from.SendMessage("Le métal brule de milles feux!");
                    ore.Consume(Math.Min(ore.Amount, 50));
                    targ.Consume(Math.Min(targ.Amount, 30));
                    return;
                }
            }
        }
    }

    public class IronOre : BaseOre
    {
        [Constructable]
        public IronOre()
            : this(1)
        {
        }

        [Constructable]
        public IronOre(int amount)
            : base(CraftResource.MIron, amount)
        {
        }

        public IronOre(bool fixedSize)
            : this(1)
        {
            if (fixedSize)
                ItemID = 0x19B8;
        }

        public IronOre(Serial serial)
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

        public override BaseIngot GetIngot()
        {
            return new IronIngot();
        }

    }


    public class BronzeOre : BaseOre
    {
        [Constructable]
        public BronzeOre()
            : this(1)
        {
        }

        [Constructable]
        public BronzeOre(int amount)
            : base(CraftResource.MBronze, amount)
        {
        }

        public BronzeOre(Serial serial)
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

        public override BaseIngot GetIngot()
        {
            return new BronzeIngot();
        }
    }


    public class GoldOre : BaseOre
    {
        [Constructable]
        public GoldOre()
            : this(1)
        {
        }

        [Constructable]
        public GoldOre(int amount)
            : base(CraftResource.MGold, amount)
        {
        }

        public GoldOre(Serial serial)
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

        public override BaseIngot GetIngot()
        {
            return new GoldIngot();
        }
    }


    public class CopperOre : BaseOre
    {
        [Constructable]
        public CopperOre()
            : this(1)
        {
        }

        [Constructable]
        public CopperOre(int amount)
            : base(CraftResource.MCopper, amount)
        {
        }

        public CopperOre(Serial serial)
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
        public override BaseIngot GetIngot()
        {
            return new CopperIngot();
        }

    }


    public class OldcopperOre : BaseOre
    {
        [Constructable]
        public OldcopperOre()
            : this(1)
        {
        }

        [Constructable]
        public OldcopperOre(int amount)
            : base(CraftResource.MOldcopper, amount)
        {
        }

        public OldcopperOre(Serial serial)
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

        public override BaseIngot GetIngot()
        {
            return new OldcopperIngot();
        }


    }


    public class DullcopperOre : BaseOre
    {
        [Constructable]
        public DullcopperOre()
            : this(1)
        {
        }

        [Constructable]
        public DullcopperOre(int amount)
            : base(CraftResource.MDullcopper, amount)
        {
        }

        public DullcopperOre(Serial serial)
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

        public override BaseIngot GetIngot()
        {
            return new DullcopperIngot();
        }


    }



    public class SilverOre : BaseOre
    {
        [Constructable]
        public SilverOre()
            : this(1)
        {
        }

        [Constructable]
        public SilverOre(int amount)
            : base(CraftResource.MSilver, amount)
        {
        }

        public SilverOre(Serial serial)
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
        public override BaseIngot GetIngot()
        {
            return new SilverIngot();
        }

    }


    public class ShadowOre : BaseOre
    {
        [Constructable]
        public ShadowOre()
            : this(1)
        {
        }

        [Constructable]
        public ShadowOre(int amount)
            : base(CraftResource.MShadow, amount)
        {
        }

        public ShadowOre(Serial serial)
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
        public override BaseIngot GetIngot()
        {
            return new ShadowIngot();
        }


    }


    public class BloodrockOre : BaseOre
    {
        [Constructable]
        public BloodrockOre()
            : this(1)
        {
        }

        [Constructable]
        public BloodrockOre(int amount)
            : base(CraftResource.MBloodrock, amount)
        {
        }

        public BloodrockOre(Serial serial)
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
        public override BaseIngot GetIngot()
        {
            return new BloodrockIngot();
        }
    }


    public class BlackrockOre : BaseOre
    {
        [Constructable]
        public BlackrockOre()
            : this(1)
        {
        }

        [Constructable]
        public BlackrockOre(int amount)
            : base(CraftResource.MBlackrock, amount)
        {
        }

        public BlackrockOre(Serial serial)
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
        public override BaseIngot GetIngot()
        {
            return new BlackrockIngot();
        }
    }


    public class MytherilOre : BaseOre
    {
        [Constructable]
        public MytherilOre()
            : this(1)
        {
        }

        [Constructable]
        public MytherilOre(int amount)
            : base(CraftResource.MMytheril, amount)
        {
        }

        public MytherilOre(Serial serial)
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
        public override BaseIngot GetIngot()
        {
            return new MytherilIngot();
        }
    }


    public class RoseOre : BaseOre
    {
        [Constructable]
        public RoseOre()
            : this(1)
        {
        }

        [Constructable]
        public RoseOre(int amount)
            : base(CraftResource.MRose, amount)
        {
        }

        public RoseOre(Serial serial)
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
        public override BaseIngot GetIngot()
        {
            return new RoseIngot();
        }


    }


    public class VeriteOre : BaseOre
    {
        [Constructable]
        public VeriteOre()
            : this(1)
        {
        }

        [Constructable]
        public VeriteOre(int amount)
            : base(CraftResource.MVerite, amount)
        {
        }

        public VeriteOre(Serial serial)
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

        public override BaseIngot GetIngot()
        {
            return new VeriteIngot();
        }
    }


    public class AgapiteOre : BaseOre
    {
        [Constructable]
        public AgapiteOre()
            : this(1)
        {
        }

        [Constructable]
        public AgapiteOre(int amount)
            : base(CraftResource.MAgapite, amount)
        {
        }

        public AgapiteOre(Serial serial)
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
        public override BaseIngot GetIngot()
        {
            return new AgapiteIngot();
        }
    }


    public class RustyOre : BaseOre
    {
        [Constructable]
        public RustyOre()
            : this(1)
        {
        }

        [Constructable]
        public RustyOre(int amount)
            : base(CraftResource.MRusty, amount)
        {
        }

        public RustyOre(Serial serial)
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
        public override BaseIngot GetIngot()
        {
            return new RustyIngot();
        }

    }


    public class ValoriteOre : BaseOre
    {
        [Constructable]
        public ValoriteOre()
            : this(1)
        {
        }

        [Constructable]
        public ValoriteOre(int amount)
            : base(CraftResource.MValorite, amount)
        {
        }

        public ValoriteOre(Serial serial)
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

        public override BaseIngot GetIngot()
        {
            return new ValoriteIngot();
        }

    }


    public class DragonOre : BaseOre
    {
        [Constructable]
        public DragonOre()
            : this(1)
        {
        }

        [Constructable]
        public DragonOre(int amount)
            : base(CraftResource.MDragon, amount)
        {
        }

        public DragonOre(Serial serial)
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

        public override BaseIngot GetIngot()
        {
            return new DragonIngot();
        }


    }



    public class TitanOre : BaseOre
    {
        [Constructable]
        public TitanOre()
            : this(1)
        {
        }

        [Constructable]
        public TitanOre(int amount)
            : base(CraftResource.MTitan, amount)
        {
        }

        public TitanOre(Serial serial)
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
        public override BaseIngot GetIngot()
        {
            return new TitanIngot();
        }


    }


    public class CrystalineOre : BaseOre
    {
        [Constructable]
        public CrystalineOre()
            : this(1)
        {
        }

        [Constructable]
        public CrystalineOre(int amount)
            : base(CraftResource.MCrystaline, amount)
        {
        }

        public CrystalineOre(Serial serial)
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
        public override BaseIngot GetIngot()
        {
            return new CrystalineIngot();
        }


    }


    public class KryniteOre : BaseOre
    {
        [Constructable]
        public KryniteOre()
            : this(1)
        {
        }

        [Constructable]
        public KryniteOre(int amount)
            : base(CraftResource.MKrynite, amount)
        {
        }

        public KryniteOre(Serial serial)
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
        public override BaseIngot GetIngot()
        {
            return new KryniteIngot();
        }
    }


    public class VulcanOre : BaseOre
    {
        [Constructable]
        public VulcanOre()
            : this(1)
        {
        }

        [Constructable]
        public VulcanOre(int amount)
            : base(CraftResource.MVulcan, amount)
        {
        }

        public VulcanOre(Serial serial)
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
        public override BaseIngot GetIngot()
        {
            return new VulcanIngot();
        }
    }


    public class BloodcrestOre : BaseOre
    {
        [Constructable]
        public BloodcrestOre()
            : this(1)
        {
        }

        [Constructable]
        public BloodcrestOre(int amount)
            : base(CraftResource.MBloodcrest, amount)
        {
        }

        public BloodcrestOre(Serial serial)
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
        public override BaseIngot GetIngot()
        {
            return new BloodcrestIngot();
        }
    }


    public class ElvinOre : BaseOre
    {
        [Constructable]
        public ElvinOre()
            : this(1)
        {
        }

        [Constructable]
        public ElvinOre(int amount)
            : base(CraftResource.MElvin, amount)
        {
        }

        public ElvinOre(Serial serial)
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
        public override BaseIngot GetIngot()
        {
            return new ElvinIngot();
        }


    }


    public class AcidOre : BaseOre
    {
        [Constructable]
        public AcidOre()
            : this(1)
        {
        }

        [Constructable]
        public AcidOre(int amount)
            : base(CraftResource.MAcid, amount)
        {
        }

        public AcidOre(Serial serial)
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

        public override BaseIngot GetIngot()
        {
            return new AcidIngot();
        }


    }


    public class AquaOre : BaseOre
    {
        [Constructable]
        public AquaOre()
            : this(1)
        {
        }

        [Constructable]
        public AquaOre(int amount)
            : base(CraftResource.MAqua, amount)
        {
        }

        public AquaOre(Serial serial)
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

        public override BaseIngot GetIngot()
        {
            return new AquaIngot();
        }

    }



    public class EldarOre : BaseOre
    {
        private DateTime m_MinedDate;
        private bool m_Ready;
        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime MinedDate
        {
            get { return m_MinedDate; }
            set { m_MinedDate = value; InvalidateProperties(); }
        }
         [CommandProperty(AccessLevel.GameMaster)]
        public bool Ready
        {
            get { return m_Ready; }
            set { m_Ready = value; }
        }

        [Constructable]
        public EldarOre()
            : this(1)
        {
            MinedDate = DateTime.Now;
            Stackable = false;
        }

        [Constructable]
        public EldarOre(int amount)
            : base(CraftResource.MRose, amount)
        {
            MinedDate = DateTime.Now;
            Stackable = false;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!Ready && (DateTime.Now - MinedDate).TotalDays >= 7)
            {
                from.SendMessage("À l'instant où vous prenez ce métal, sa couleur prend de l'éclat.");
                Resource = CraftResource.MEldar;
                Hue = 0x4DD;
                m_Ready = true;
                Stackable = true;
            }
            else if (!Ready)
                from.SendMessage("L'âge rend meilleur à ce qu'on dit...");
            
                base.OnDoubleClick(from);
        }

        public EldarOre(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write((DateTime)m_MinedDate);
            writer.Write((bool)m_Ready);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_MinedDate = reader.ReadDateTime();
            m_Ready = reader.ReadBool();
        }

        public override BaseIngot GetIngot()
        {
            if(Ready)
            return new EldarIngot();
            else
            return new RoseIngot();
        }


    }


    public class GlowingOre : BaseOre
    {
        [Constructable]
        public GlowingOre()
            : this(1)
        {
        }

        [Constructable]
        public GlowingOre(int amount)
            : base(CraftResource.MGlowing, amount)
        {
        }

        public GlowingOre(Serial serial)
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

        public override BaseIngot GetIngot()
        {
            return new GlowingIngot();
        }


    }


    public class GorganOre : BaseOre
    {
        [Constructable]
        public GorganOre()
            : this(1)
        {
        }

        [Constructable]
        public GorganOre(int amount)
            : base(CraftResource.MGorgan, amount)
        {
        }

        public GorganOre(Serial serial)
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

        public override BaseIngot GetIngot()
        {
            return new GorganIngot();
        }
    }


    public class SandrockOre : BaseOre
    {
        [Constructable]
        public SandrockOre()
            : this(1)
        {
        }

        [Constructable]
        public SandrockOre(int amount)
            : base(CraftResource.MSandrock, amount)
        {
        }

        public SandrockOre(Serial serial)
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

        public override BaseIngot GetIngot()
        {
            return new SandrockIngot();
        }
    }


    public class SteelOre : BaseOre
    {
        [Constructable]
        public SteelOre()
            : this(1)
        {
        }

        [Constructable]
        public SteelOre(int amount)
            : base(CraftResource.MSteel, amount)
        {
        }

        public SteelOre(Serial serial)
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

        public override BaseIngot GetIngot()
        {
            return new SteelIngot();
        }
    }
}