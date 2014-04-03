using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    public class VivreGuard : BaseCreature
    {
        private String m_TownName;

        [CommandProperty(AccessLevel.GameMaster)]
        public int TownHue
        {
            get
            {
                if (m_TownName.ToLower() == "haven")
                    return 1779;
                else if (m_TownName.ToLower() == "fort serpent")
                    return 2112;
                else
                    return 0;
            }
        }

        [Constructable]
        public VivreGuard(String townName)
            : base(AIType.AI_VivreGuard, FightMode.Closest, 18, 1, 0.12, 1) // 0.15 echapable à pied, 0.05 = très rapide
        {
            m_TownName = townName;
            InitStats(200, 200, 200);
            SpeechHue = Utility.RandomDyedHue();
            Hue = Utility.RandomSkinHue();
            Body = 0x190;
            Name = NameList.RandomName("male");
            Title = ", Garde de " + m_TownName;
            Karma = 12000;

            PlateChest chest = new PlateChest();
            chest.Hue = 0;
            chest.Resource = CraftResource.MBronze;
            chest.Movable = false;
            AddItem(chest);
            PlateArms arms = new PlateArms();
            arms.Hue = 0;
            arms.Movable = false;
            arms.Resource = CraftResource.MBronze;
            AddItem(arms);
            PlateGloves gloves = new PlateGloves();
            gloves.Hue = 0;
            gloves.Movable = false;
            gloves.Resource = CraftResource.MBronze;
            AddItem(gloves);
            PlateGorget gorget = new PlateGorget();
            gorget.Hue = 0;
            gorget.Movable = false;
            gorget.Resource = CraftResource.MBronze;
            AddItem(gorget);
            PlateLegs legs = new PlateLegs();
            legs.Hue = 0;
            legs.Movable = false;
            legs.Resource = CraftResource.MBronze;
            AddItem(legs);
            NorseHelm helm = new NorseHelm();
            helm.Hue = 0;
            helm.Movable = false;
            helm.Resource = CraftResource.MBronze;
            AddItem(helm);
            Surcoat surcoat = new Surcoat();
            surcoat.Hue = 0;
            surcoat.Movable = false;
            AddItem(surcoat);
            Cloak cloak = new Cloak();
            cloak.Hue = TownHue;
            cloak.Movable = false;
            AddItem(cloak);


            HairItemID = Utility.RandomList(0x203B, 0x203C, 0x203D, 0x2044, 0x2045, 0x2047, 0x2049, 0x204A);
            HairHue = Utility.RandomHairHue();

            if (Utility.RandomBool())
            {
                FacialHairItemID = Utility.RandomList(0x203E, 0x203F, 0x2040, 0x2041, 0x204B, 0x204C, 0x204D);
                FacialHairHue = HairHue;
            }

            Halberd weapon = new Halberd();
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

        // Scriptiz : pour capté les baseCreature ;)
        public override bool IsEnemy(Mobile m)
        {
            if (m != null && (m.Criminal == true || m is BaseCreature))
                return true;
            else
                return base.IsEnemy(m);
        }

        public override bool Unprovokable { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }

        public override bool BardImmune { get { return true; } }
        public VivreGuard(Serial serial)
            : base(serial)
        {
        }

        public String TownName
        {
            get { return m_TownName; }
            set { m_TownName = value; }
        }

        public override bool OnBeforeDeath()
        {
            return true;
        }

        public override void OnCombatantChange()
        {
            base.OnCombatantChange();

            if (Combatant != null)
                this.Say("Un Hors la Loi !!! Sortez de cette ville ou mourez!");
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            if (from is PlayerMobile) return true;
            return base.HandlesOnSpeech(from);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (!e.Handled && e.Mobile.InRange(this.Location, 1))
            {
                PlayerMobile from = e.Mobile as PlayerMobile;
                {
                    e.Handled = true;
                    this.Say("Veuillez circuler citoyen...");
                    base.OnSpeech(e);
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write(m_TownName);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_TownName = reader.ReadString();
        }
    }
}