using System;
using Server;
using System.Collections.Generic;
using Server.Network;
using Server.ContextMenus;
using Server.Targeting;

using Server.Mobiles;

namespace Server.Items
{
    public enum SuperSlayerType
    {
        None,
        Repond,
        Reptile,
        Exorcism,
        Elemental,
        Fey,
        Silver,
        Arachnid
    }
    public class SlayerForge : Item
    {
        private int[] VialTypes;
        private bool m_CanAddRelic;

        private SuperSlayerType m_SuperSlayer;



        [CommandProperty(AccessLevel.GameMaster)]
        public bool CanAddRelic
        {
            get { return m_CanAddRelic; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public SuperSlayerType SuperSlayer
        {
            get { return m_SuperSlayer; }
            set { m_SuperSlayer = value; }
        }

        [CommandProperty(AccessLevel.Administrator, true)]
        public int MaxVials
        {
            get { return 30; }
        }

        [CommandProperty(AccessLevel.GameMaster, true)]
        public int CountVial
        {
            get
            {
                int total = 0;
                for (int i = 0; i < VialTypes.Length; i++)
                {
                    total += VialTypes[i];
                }
                return total;
            }
        }

        [CommandProperty(AccessLevel.GameMaster, true)]
        public int CountType
        {
            get
            {
                int typecounter = 0;

                for (int i = 0; i < VialTypes.Length; i++)
                {
                    if (VialTypes[i] > 0)
                        typecounter++;
                }
                return typecounter;
            }
        }

        [Constructable]
        public SlayerForge()
            : base(0x44C7)
        {
            Name = "Bassin à Slayers";
            VialTypes = new int[Enum.GetValues(typeof(LiquidType)).Length];
            Weight = 50;
            EmptyForge();
        }

        public void AddVial(AlchemyVial vial)
        {
            int index = (int)vial.AlchemyLiquidType;

            if (index >= VialTypes.Length)
                return;

            VialTypes[index]++;
        }

        public void EmptyForge()
        {
            for (int i = 0; i < VialTypes.Length; i++)
                VialTypes[i] = 0;

            SuperSlayer = SuperSlayerType.None;
        }

        public override bool OnDragLift(Mobile from)
        {
            if (CountVial >= 1)
            {
                EmptyForge();
                from.SendMessage("Vous videz la forge afin de mieux la transporter");
            }
            return base.OnDragLift(from);
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);


            if (from.Alive && CountVial >= 1)
                list.Add(new ContextMenus.SlayerForgeEntry(from, this));
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (CountVial == 1)
                list.Add("Une fiole y a été versée");
            else if (CountVial > 1)
                list.Add("{0} fioles y ont été versées");

            if (CountType == 1)
                list.Add("Liquide pur");
            else if (CountType > 1)
                list.Add("Liquide impur");
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 2) ||  !from.InLOS(this))
                {
                from.SendLocalizedMessage(501816);
                return;
                }
            int VialAmount = CountVial;

            int Elemental = VialTypes[4] + VialTypes[5] + VialTypes[6] + VialTypes[7] + VialTypes[8] + VialTypes[9] + VialTypes[10];
            int Humanoid = VialTypes[1] + VialTypes[2] + VialTypes[3];
            int Arachnid = VialTypes[12] + VialTypes[13] + VialTypes[14];
            int Reptile = VialTypes[15] + VialTypes[16] + VialTypes[17] + VialTypes[18];

            double FillingPercent = VialAmount / (double)MaxVials;

            if (VialAmount == 0)
                this.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "La forge est vide");
            else if (CountType > 1 && Elemental != VialAmount && Humanoid != VialAmount && Arachnid != VialAmount && Reptile != VialAmount)
            {
                this.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "Le mélange ne donnera rien de bon... la forge le détruit");
                EmptyForge();
            }
            else if (VialAmount >= MaxVials)
            {
                if (CountType > 1 && ((Elemental == VialAmount && SuperSlayer != SuperSlayerType.Elemental) || (Humanoid == VialAmount && SuperSlayer != SuperSlayerType.Repond) || (Arachnid == VialAmount && SuperSlayer != SuperSlayerType.Arachnid) || (Reptile == VialAmount && SuperSlayer != SuperSlayerType.Reptile)))
                {
                    m_CanAddRelic = true;
                    this.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "Une relique appartenant au souverain sera nécessaire pour achever votre travail");
                }
                else if ((VialTypes[21] == VialAmount && SuperSlayer != SuperSlayerType.Exorcism) || (VialTypes[20] == VialAmount && SuperSlayer != SuperSlayerType.Fey) || (VialTypes[19] == VialAmount && SuperSlayer != SuperSlayerType.Silver))
                {
                    m_CanAddRelic = true;
                    this.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "Une relique appartenant au souverain sera nécessaire pour achever votre travail");
                }
                else
                {
                    this.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "La forge est prête à recevoir l'arme");
                    from.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(WeaponSlayerTarget));
                    
                }
            }
            else
            {
                if (FillingPercent < 0.2)
                    this.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "La forge est presque vide");
                else if (FillingPercent >= 0.2 && FillingPercent < 0.5)
                    this.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "La forge sera bientôt à moitié vide");
                else if (FillingPercent >= 0.5 && FillingPercent < 0.8)
                    this.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "La forge est plus qu'à moitié pleine");
                else
                    this.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "La forge est presque pleine");
            }
        }

        public SlayerForge(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)2); // version

            writer.Write((int)VialTypes.Length);

            for (int i = 0; i < VialTypes.Length; i++)
                writer.Write(VialTypes[i]);

            writer.WriteEncodedInt((int)m_SuperSlayer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            
            int count = 23;

            if( version > 1)
                count = reader.ReadInt();
            
            VialTypes = new int[Enum.GetValues(typeof(LiquidType)).Length];
            for (int i = 0; i < count; i++)
                VialTypes[i] = reader.ReadInt();

            m_SuperSlayer = (SuperSlayerType)reader.ReadEncodedInt();
        }

        public void WeaponSlayerTarget(Mobile from, object obj)
        {
          
            if(obj is BaseWeapon)
            {
                Console.WriteLine("Test");
            BaseWeapon slayertarget = (BaseWeapon)obj;

            if (slayertarget.Slayer2 != SlayerName.None && slayertarget.Slayer != SlayerName.None)
            {
                from.SendMessage("Cette arme a déjà des slayers, vous ne pouvez en ajouter");
                return;
            }
            Console.WriteLine("Test1");

            double skillmin = Math.Min(from.Skills[SkillName.ArmsLore].Value, from.Skills[SkillName.Blacksmith].Value) + 1;

            double malus = slayertarget.WeaponDifficulty;

            double bonus = 0;
            double superbonus = 1;
            if (slayertarget.Crafter != null && slayertarget.Crafter.Serial == from.Serial)
                superbonus = 2;
                Console.WriteLine("Test1");
            if (from.Skills[SkillName.Alchemy].Value > 80)
                bonus += 5;
            if (from.Skills[SkillName.ItemID].Value > 80)
                bonus += 5;

            foreach (Mobile m in from.GetMobilesInRange(1))
            {
                PlayerMobile pm = m as PlayerMobile;
                if (pm == null || pm == from) continue;
                if (pm.AccessLevel > AccessLevel.Player) continue;  // Scriptiz : évitons qu'un GM observant le procédé soit une aide
                if (pm.Hidden) continue;    // Scriptiz : si le PJ est caché il sert à rien
                if (!pm.Alive) continue; // Scriptiz : un PJ mort ne sert à rien non plus

                int checkBonus = 0;
                if (pm.Skills[SkillName.Alchemy].Value > 80)
                    checkBonus += 1;
                if (pm.Skills[SkillName.Blacksmith].Value > 80)
                    checkBonus += 1;
                if (pm.Skills[SkillName.ArmsLore].Value > 80)
                    checkBonus += 1;
                if (pm.Skills[SkillName.ItemID].Value > 80)
                    checkBonus += 1;

                if (checkBonus > 1)
                    bonus += 5;

                if (slayertarget.Crafter != null && slayertarget.Crafter.Serial == pm.Serial)
                    superbonus = 1.5;

                if (slayertarget.Resource == CraftResource.MGlowing && VialTypes[19] > 0)
                    superbonus += 1;
            }

            if (bonus > 25)
                bonus = 25;

            if (slayertarget.PlayerConstructed)
                bonus *=2;

            if (CountType > 1 || VialTypes[21] == CountVial || VialTypes[20] == CountVial || VialTypes[19] == CountVial)
                malus *= 2;

            if (slayertarget.Slayer != SlayerName.None)
                malus *= 2;

            double chances = Math.Round(((skillmin + bonus) / malus * superbonus),2);
            if (chances >= 90)
                chances = 100 - malus;

            if (chances < Utility.Random(100))
            {
                from.SendMessage("Vous appliquez le slayer sur l'arme.");
                if (slayertarget.Slayer == SlayerName.None)
                    slayertarget.Slayer = AddSlayer();
                else
                    slayertarget.Slayer2 = AddSlayer();
            }
            else if (Utility.Random(100) <= malus*2)
            {
                from.SendMessage("Vous échouez. Le slayer ronge lentement l'arme, tout est perdu.");
                slayertarget.Delete();
            }
            else
                from.SendMessage("Vous échouez, mais parvenez à conserver l'arme.");

            EmptyForge();
            return;
            }
            else if (obj is Spellbook)
            {
                Spellbook slayertarget = (Spellbook)obj;

                if (slayertarget.SpellbookType != SpellbookType.Regular )
                {
                    from.SendMessage("Seul les livres arcaniques peuvent accepter un slayer");
                    return;
                }

                if (slayertarget.Slayer2 != SlayerName.None && slayertarget.Slayer != SlayerName.None)
                {
                    from.SendMessage("Ce livre a déjà des slayers, vous ne pouvez en ajouter");
                    return;
                }

                double skillmin = (Math.Min(from.Skills[SkillName.EvalInt].Value, from.Skills[SkillName.Inscribe].Value) + 1);

                double malus = (65 - slayertarget.SpellCount);
                

                double bonus = 0;
                double superbonus = 1;
                if (slayertarget.Crafter != null &&  slayertarget.Crafter.Serial == from.Serial)
                    superbonus = 1.8;

                if (from.Skills[SkillName.Alchemy].Value > 80)
                    bonus += 4;
                if (from.Skills[SkillName.ItemID].Value > 80)
                    bonus += 4;

                foreach (Mobile m in from.GetMobilesInRange(1))
                {
                    PlayerMobile pm = m as PlayerMobile;
                    if (pm == null || pm == from) continue;
                    if (pm.AccessLevel > AccessLevel.Player) continue;  // Scriptiz : évitons qu'un GM observant le procédé soit une aide
                    if (pm.Hidden) continue;    // Scriptiz : si le PJ est caché il sert à rien
                    if (!pm.Alive) continue; // Scriptiz : un PJ mort ne sert à rien non plus

                    int checkBonus = 0;
                    if (pm.Skills[SkillName.Alchemy].Value > 80)
                        checkBonus += 1;
                    if (pm.Skills[SkillName.Magery].Value > 80)
                        checkBonus += 1;
                    if (pm.Skills[SkillName.EvalInt].Value > 80)
                        checkBonus += 1;
                    if (pm.Skills[SkillName.ItemID].Value > 80)
                        checkBonus += 1;

                    if (checkBonus > 2)
                        bonus += 5;

                    if (slayertarget.Crafter != null &&  slayertarget.Crafter.Serial == pm.Serial)
                        superbonus = 1.4;

                }

                if (bonus > 25)
                    bonus = 25;

                if (CountType > 1 || VialTypes[21] == CountVial || VialTypes[20] == CountVial || VialTypes[19] == CountVial)
                    malus *= 2;

                if (slayertarget.Slayer != SlayerName.None)
                    malus *= 2;

                double chances = Math.Round(((skillmin + bonus) / Math.Sqrt(malus) * superbonus), 2);
                if (chances >= 90)
                    chances = 90 - malus;

                if (chances < Utility.Random(100))
                {
                    from.SendMessage("Vous appliquez le slayer sur le livre.");
                    if (slayertarget.Slayer == SlayerName.None)
                        slayertarget.Slayer = AddSlayer();
                    else
                        slayertarget.Slayer2 = AddSlayer();
                }
                else if (Utility.Random(100) <= malus * 2)
                {
                    from.SendMessage("Vous échouez. Le slayer ronge lentement le livre, tout est perdu.");
                    slayertarget.Delete();
                }
                else
                    from.SendMessage("Vous échouez, mais parvenez à conserver le livre.");

                EmptyForge();
                return;
            }
            else
            {
                from.SendMessage("Ceci n'est pas un livre de sorts ou une arme");
            }
            
        }

        
        public SlayerName AddSlayer()
        {
            if (CountType == 1)
            {
                if (VialTypes[1] != 0)
                    return SlayerName.OgreTrashing;
                if (VialTypes[2] != 0)
                    return SlayerName.OrcSlaying;
                if (VialTypes[3] != 0)
                    return SlayerName.TrollSlaughter;
                if (VialTypes[4] != 0)
                    return SlayerName.BloodDrinking;
                if (VialTypes[5] != 0)
                    return SlayerName.EarthShatter;
                if (VialTypes[6] != 0)
                    return SlayerName.ElementalHealth;
                if (VialTypes[7] != 0)
                    return SlayerName.FlameDousing;
                if (VialTypes[8] != 0)
                    return SlayerName.SummerWind;
                if (VialTypes[9] != 0)
                    return SlayerName.Vacuum;
                if (VialTypes[10] != 0)
                    return SlayerName.WaterDissipation;
                if (VialTypes[11] != 0)
                    return SlayerName.GargoylesFoe;
                if (VialTypes[12] != 0)
                    return SlayerName.ScorpionsBane;
                if (VialTypes[13] != 0)
                    return SlayerName.SpidersDeath;
                if (VialTypes[14] != 0)
                    return SlayerName.Terathan;
                if (VialTypes[15] != 0)
                    return SlayerName.LizardmanSlaughter;
                if (VialTypes[16] != 0)
                    return SlayerName.DragonSlaying;
                if (VialTypes[17] != 0)
                    return SlayerName.Ophidian;
                if (VialTypes[18] != 0)
                    return SlayerName.SnakesBane;
                if (VialTypes[19] != 0)
                    return SlayerName.Silver;
                if (VialTypes[20] != 0)
                    return SlayerName.Fey;
                if (VialTypes[21] != 0)
                    return SlayerName.Exorcism;
            }
            if (CountType > 1)
            {
                if ((VialTypes[4] + VialTypes[5] + VialTypes[6] + VialTypes[7] + VialTypes[8] + VialTypes[9] + VialTypes[10]) == MaxVials)
                    return SlayerName.ElementalBan;
                if ((VialTypes[1] + VialTypes[2] + VialTypes[3]) == MaxVials)
                    return SlayerName.Repond;
                if ((VialTypes[15] + VialTypes[16] + VialTypes[17] + VialTypes[18]) == MaxVials)
                    return SlayerName.ReptilianDeath;
                if ((VialTypes[12] + VialTypes[13] + VialTypes[14]) == MaxVials)
                    return SlayerName.ArachnidDoom;
            }
            return SlayerName.None;
        }
    }
}