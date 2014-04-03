using System;
using System.Collections;

namespace Server.Items
{
    public enum CraftResource
    {
        None = 0,
        MIron = 1,
        MBronze,
        MGold,
        MCopper,
        MOldcopper,
        MDullcopper,
        MSilver,
        MShadow,
        MBloodrock,
        MBlackrock,
        MMytheril,
        MRose,
        MVerite,
        MAgapite,
        MRusty,
        MValorite,
        MDragon,
        MTitan,
        MCrystaline,
        MKrynite,
        MVulcan,
        MBloodcrest,
        MElvin,
        MAcid,
        MAqua,
        MEldar,
        MGlowing,
        MGorgan,
        MSandrock,
        MSteel,

        RegularLeather = 101,
        SpinedLeather,
        HornedLeather,
        BarbedLeather,
        DaemonLeather,

        RedScales = 201,
        YellowScales,
        BlackScales,
        GreenScales,
        WhiteScales,
        BlueScales,

        RegularWood = 301,
        OakWood,
        AshWood,
        YewWood,
        Heartwood,
        Bloodwood,
        Frostwood
    }

    public enum CraftResourceType
    {
        None,
        Metal,
        Leather,
        Scales,
        Wood
    }

    public class CraftAttributeInfo
    {
        private int m_WeaponFireDamage;
        private int m_WeaponColdDamage;
        private int m_WeaponPoisonDamage;
        private int m_WeaponEnergyDamage;
        private int m_WeaponChaosDamage;
        private int m_WeaponDirectDamage;
        private int m_WeaponDurability;
        private int m_WeaponLuck;
        private int m_WeaponGoldIncrease;
        private int m_WeaponLowerRequirements;

        private int m_ArmorPhysicalResist;
        private int m_ArmorFireResist;
        private int m_ArmorColdResist;
        private int m_ArmorPoisonResist;
        private int m_ArmorEnergyResist;
        private int m_ArmorDurability;
        private int m_ArmorLuck;
        private int m_ArmorGoldIncrease;
        private int m_ArmorLowerRequirements;

        private int m_RunicMinAttributes;
        private int m_RunicMaxAttributes;
        private int m_RunicMinIntensity;
        private int m_RunicMaxIntensity;

        public int WeaponFireDamage { get { return m_WeaponFireDamage; } set { m_WeaponFireDamage = value; } }
        public int WeaponColdDamage { get { return m_WeaponColdDamage; } set { m_WeaponColdDamage = value; } }
        public int WeaponPoisonDamage { get { return m_WeaponPoisonDamage; } set { m_WeaponPoisonDamage = value; } }
        public int WeaponEnergyDamage { get { return m_WeaponEnergyDamage; } set { m_WeaponEnergyDamage = value; } }
        public int WeaponChaosDamage { get { return m_WeaponChaosDamage; } set { m_WeaponChaosDamage = value; } }
        public int WeaponDirectDamage { get { return m_WeaponDirectDamage; } set { m_WeaponDirectDamage = value; } }
        public int WeaponDurability { get { return m_WeaponDurability; } set { m_WeaponDurability = value; } }
        public int WeaponLuck { get { return m_WeaponLuck; } set { m_WeaponLuck = value; } }
        public int WeaponGoldIncrease { get { return m_WeaponGoldIncrease; } set { m_WeaponGoldIncrease = value; } }
        public int WeaponLowerRequirements { get { return m_WeaponLowerRequirements; } set { m_WeaponLowerRequirements = value; } }

        public int ArmorPhysicalResist { get { return m_ArmorPhysicalResist; } set { m_ArmorPhysicalResist = value; } }
        public int ArmorFireResist { get { return m_ArmorFireResist; } set { m_ArmorFireResist = value; } }
        public int ArmorColdResist { get { return m_ArmorColdResist; } set { m_ArmorColdResist = value; } }
        public int ArmorPoisonResist { get { return m_ArmorPoisonResist; } set { m_ArmorPoisonResist = value; } }
        public int ArmorEnergyResist { get { return m_ArmorEnergyResist; } set { m_ArmorEnergyResist = value; } }
        public int ArmorDurability { get { return m_ArmorDurability; } set { m_ArmorDurability = value; } }
        public int ArmorLuck { get { return m_ArmorLuck; } set { m_ArmorLuck = value; } }
        public int ArmorGoldIncrease { get { return m_ArmorGoldIncrease; } set { m_ArmorGoldIncrease = value; } }
        public int ArmorLowerRequirements { get { return m_ArmorLowerRequirements; } set { m_ArmorLowerRequirements = value; } }

        public int RunicMinAttributes { get { return m_RunicMinAttributes; } set { m_RunicMinAttributes = value; } }
        public int RunicMaxAttributes { get { return m_RunicMaxAttributes; } set { m_RunicMaxAttributes = value; } }
        public int RunicMinIntensity { get { return m_RunicMinIntensity; } set { m_RunicMinIntensity = value; } }
        public int RunicMaxIntensity { get { return m_RunicMaxIntensity; } set { m_RunicMaxIntensity = value; } }

        #region Mondain's Legacy
        private int m_WeaponDamage;
        private int m_WeaponHitChance;
        private int m_WeaponHitLifeLeech;
        private int m_WeaponRegenHits;
        private int m_WeaponSwingSpeed;

        private int m_ArmorDamage;
        private int m_ArmorHitChance;
        private int m_ArmorRegenHits;
        private int m_ArmorMage;

        private int m_ShieldPhysicalResist;
        private int m_ShieldFireResist;
        private int m_ShieldColdResist;
        private int m_ShieldPoisonResist;
        private int m_ShieldEnergyResist;

        public int WeaponDamage { get { return m_WeaponDamage; } set { m_WeaponDamage = value; } }
        public int WeaponHitChance { get { return m_WeaponHitChance; } set { m_WeaponHitChance = value; } }
        public int WeaponHitLifeLeech { get { return m_WeaponHitLifeLeech; } set { m_WeaponHitLifeLeech = value; } }
        public int WeaponRegenHits { get { return m_WeaponRegenHits; } set { m_WeaponRegenHits = value; } }
        public int WeaponSwingSpeed { get { return m_WeaponSwingSpeed; } set { m_WeaponSwingSpeed = value; } }

        public int ArmorDamage { get { return m_ArmorDamage; } set { m_ArmorDamage = value; } }
        public int ArmorHitChance { get { return m_ArmorHitChance; } set { m_ArmorHitChance = value; } }
        public int ArmorRegenHits { get { return m_ArmorRegenHits; } set { m_ArmorRegenHits = value; } }
        public int ArmorMage { get { return m_ArmorMage; } set { m_ArmorMage = value; } }

        public int ShieldPhysicalResist { get { return m_ShieldPhysicalResist; } set { m_ShieldPhysicalResist = value; } }
        public int ShieldFireResist { get { return m_ShieldFireResist; } set { m_ShieldFireResist = value; } }
        public int ShieldColdResist { get { return m_ShieldColdResist; } set { m_ShieldColdResist = value; } }
        public int ShieldPoisonResist { get { return m_ShieldPoisonResist; } set { m_ShieldPoisonResist = value; } }
        public int ShieldEnergyResist { get { return m_ShieldEnergyResist; } set { m_ShieldEnergyResist = value; } }
        #endregion


        public CraftAttributeInfo()
        {
        }

        public static readonly CraftAttributeInfo Blank;
        #region Metal
        public static readonly CraftAttributeInfo
        MIron,
        MBronze,
        MGold,
        MCopper,
        MOldcopper,
        MDullcopper,
        MSilver,
        MShadow,
        MBloodrock,
        MBlackrock,
        MMytheril,
        MRose,
        MVerite,
        MAgapite,
        MRusty,
        MValorite,
        MDragon,
        MTitan,
        MCrystaline,
        MKrynite,
        MVulcan,
        MBloodcrest,
        MElvin,
        MAcid,
        MAqua,
        MEldar,
        MGlowing,
        MGorgan,
        MSandrock,
        MSteel;
        #endregion
        public static readonly CraftAttributeInfo Spined, Horned, Barbed, Daemon;
        public static readonly CraftAttributeInfo RedScales, YellowScales, BlackScales, GreenScales, WhiteScales, BlueScales;
        public static readonly CraftAttributeInfo OakWood, AshWood, YewWood, Heartwood, Bloodwood, Frostwood;

        static CraftAttributeInfo()
        {
            Blank = new CraftAttributeInfo();

            CraftAttributeInfo miron = MIron = new CraftAttributeInfo();
            CraftAttributeInfo mbronze = MBronze = new CraftAttributeInfo();
            mbronze.WeaponFireDamage = 20;
            mbronze.WeaponEnergyDamage = 20;
            mbronze.ArmorPhysicalResist = 2;
            mbronze.ArmorFireResist = 1;
            mbronze.ArmorEnergyResist = 1;

            CraftAttributeInfo mgold = MGold = new CraftAttributeInfo();

            mgold.WeaponDurability = -5;
            mgold.WeaponLuck = 30;
            mgold.WeaponGoldIncrease = 15;

            mgold.ArmorColdResist = 2;
            mgold.ArmorFireResist = 3;

            mgold.ArmorDurability = 10;
            mgold.ArmorLuck = 30;
            mgold.ArmorGoldIncrease = 2000;
            CraftAttributeInfo mcopper = MCopper = new CraftAttributeInfo();
            mcopper.WeaponEnergyDamage = 25;
            mcopper.ArmorPhysicalResist = 2;
            mcopper.ArmorEnergyResist = 4;

            CraftAttributeInfo moldcopper = MOldcopper = new CraftAttributeInfo();
            moldcopper.WeaponPoisonDamage = 20;
            moldcopper.WeaponEnergyDamage = 30;
            moldcopper.WeaponDurability = -15;
            moldcopper.ArmorPhysicalResist = 2;
            moldcopper.ArmorFireResist = 1;
            moldcopper.ArmorPoisonResist = 2;
            moldcopper.ArmorEnergyResist = 3;
            moldcopper.ArmorDurability = -15;
            CraftAttributeInfo mdullcopper = MDullcopper = new CraftAttributeInfo();
            mdullcopper.WeaponFireDamage = 5;
            mdullcopper.WeaponColdDamage = 10;
            mdullcopper.WeaponEnergyDamage = 15;

            mdullcopper.ArmorPhysicalResist = 1;
            mdullcopper.ArmorFireResist = 1;
            mdullcopper.ArmorColdResist = 1;
            mdullcopper.ArmorPoisonResist = 1;
            mdullcopper.ArmorEnergyResist = 2;

            CraftAttributeInfo msilver = MSilver = new CraftAttributeInfo();
            msilver.WeaponEnergyDamage = 15;
            msilver.WeaponDurability = 50;
            msilver.WeaponHitLifeLeech = 10;

            msilver.ArmorPhysicalResist = 4;
            msilver.ArmorPoisonResist = 3;
            msilver.ArmorRegenHits = 1;

            msilver.ShieldColdResist = 3;

            CraftAttributeInfo mshadow = MShadow = new CraftAttributeInfo();

            mshadow.WeaponColdDamage = 30;
            mshadow.WeaponPoisonDamage = 10;
            mshadow.ArmorPhysicalResist = 1;
            mshadow.ArmorFireResist = -1;
            mshadow.ArmorColdResist = 2;
            mshadow.ArmorPoisonResist = 2;

            CraftAttributeInfo mbloodrock = MBloodrock = new CraftAttributeInfo();
            CraftAttributeInfo mblackrock = MBlackrock = new CraftAttributeInfo();
            mblackrock.WeaponChaosDamage = 30;
            mblackrock.WeaponPoisonDamage = 20;
            mblackrock.WeaponColdDamage = 10;
            mblackrock.WeaponHitChance = 10;
            mblackrock.WeaponSwingSpeed = 10;

            mblackrock.ArmorPhysicalResist = 2;
            mblackrock.ArmorFireResist = 2;
            mblackrock.ArmorEnergyResist = -2;
            mblackrock.ArmorPoisonResist = 2;

            CraftAttributeInfo mmytheril = MMytheril = new CraftAttributeInfo();
            
            CraftAttributeInfo mrose = MRose = new CraftAttributeInfo();
            mrose.WeaponColdDamage = 10;
            mrose.WeaponEnergyDamage = 20;
            mrose.WeaponLowerRequirements = 20;
            mrose.WeaponDurability = -5;

            mrose.ArmorEnergyResist = 2;
            mrose.ArmorColdResist = 2;
            mrose.ArmorLowerRequirements = 20;
            mrose.ArmorDurability = -5;

            CraftAttributeInfo mverite = MVerite = new CraftAttributeInfo();
            mverite.WeaponFireDamage = 50;
            mverite.WeaponHitChance = 5;

            mverite.ArmorPhysicalResist = -1;
            mverite.ArmorColdResist = 3;
            mverite.ArmorFireResist = 5;
            mverite.ArmorPoisonResist = 7;

            CraftAttributeInfo magapite = MAgapite = new CraftAttributeInfo();
          
       magapite.WeaponDurability = 70;
      magapite.WeaponLowerRequirements = 10;

        magapite.ArmorDurability = 40;
       magapite.ArmorLowerRequirements = 10;
            CraftAttributeInfo mrusty = MRusty = new CraftAttributeInfo();
            mrusty.WeaponPoisonDamage = 40;
            mrusty.WeaponDurability = -50;
            mrusty.WeaponLowerRequirements = 30;
            mrusty.ArmorDurability = -50;
            mrusty.ArmorLowerRequirements = 30;
            CraftAttributeInfo mvalorite = MValorite = new CraftAttributeInfo();
            mvalorite.WeaponFireDamage = 15;
            mvalorite.WeaponColdDamage = 20;
            mvalorite.WeaponPoisonDamage = 10;
            mvalorite.WeaponEnergyDamage = 30;
            mvalorite.WeaponDurability = 40;

            mvalorite.ArmorPhysicalResist = 3;
            mvalorite.ArmorFireResist = 2;
            mvalorite.ArmorColdResist = 4;
            mvalorite.ArmorEnergyResist = 4;
            mvalorite.ArmorDurability = 40;

            CraftAttributeInfo mdragon = MDragon = new CraftAttributeInfo();
            mdragon.WeaponFireDamage = 70;
            mdragon.WeaponChaosDamage = 5;
            mdragon.ArmorDurability = 50;

            mdragon.ArmorFireResist = 8;
            mdragon.ArmorColdResist = -2;
            mdragon.ArmorPhysicalResist = 5;
            mdragon.ArmorPoisonResist = -2;
            mdragon.ArmorGoldIncrease = 40;
            mdragon.ArmorDurability = 20;

            CraftAttributeInfo mtitan = MTitan = new CraftAttributeInfo();
            mtitan.WeaponChaosDamage = 40;
            mtitan.WeaponDurability = 50;

            mtitan.ArmorPhysicalResist = 6;
            mtitan.ArmorFireResist = 5;
            mtitan.ArmorPoisonResist = 2;
            mtitan.ArmorEnergyResist = 2;
            mtitan.ArmorDurability = 50;

            mtitan.WeaponDamage = 20;

            mtitan.WeaponSwingSpeed = -15;

            mtitan.ArmorMage = 1;

            mtitan.ShieldPhysicalResist = 2;
            mtitan.ShieldEnergyResist = 2;

            CraftAttributeInfo mcrystaline = MCrystaline = new CraftAttributeInfo();
            mcrystaline.WeaponDurability = -25;
            mcrystaline.WeaponLuck = 40;
            mcrystaline.WeaponGoldIncrease = 50;
            mcrystaline.WeaponLowerRequirements = 20;

            mcrystaline.ArmorPhysicalResist = 2;
            mcrystaline.ArmorFireResist = 2;
            mcrystaline.ArmorDurability = -25;
            mcrystaline.ArmorLuck = 40;
            mcrystaline.ArmorGoldIncrease = 45;
            mcrystaline.ArmorLowerRequirements = 20;

            CraftAttributeInfo mkrynite = MKrynite = new CraftAttributeInfo();
            mkrynite.WeaponFireDamage = 30;
            mkrynite.WeaponColdDamage = 30;
            mkrynite.WeaponLowerRequirements = 10;

            mkrynite.ArmorPhysicalResist = 3;
            mkrynite.ArmorFireResist = 3;
            mkrynite.ArmorColdResist = 3;
            mkrynite.ArmorPoisonResist = 2;
            mkrynite.ArmorEnergyResist = 4;
            mkrynite.ArmorLowerRequirements = 2;

            CraftAttributeInfo mvulcan = MVulcan = new CraftAttributeInfo();
            mvulcan.WeaponFireDamage = 60;
            mvulcan.WeaponChaosDamage = 20;
            mvulcan.WeaponDamage = 10;

            mvulcan.ArmorPhysicalResist = 3;
            mvulcan.ArmorFireResist = 5;
            mvulcan.ArmorPoisonResist = 3;
            mvulcan.ArmorEnergyResist = 2;
            mvulcan.ShieldFireResist = 6;

            CraftAttributeInfo mbloodcrest = MBloodcrest = new CraftAttributeInfo();
            mbloodcrest.WeaponFireDamage = 20;
            mbloodcrest.WeaponPoisonDamage = 20;
            mbloodcrest.WeaponChaosDamage = 40;

            mbloodcrest.ArmorPhysicalResist = 3;
            mbloodcrest.ArmorFireResist = 4;
            mbloodcrest.ArmorColdResist = 2;
            mbloodcrest.ArmorPoisonResist = 3;
            mbloodcrest.ArmorEnergyResist = 2;

            CraftAttributeInfo melvin = MElvin = new CraftAttributeInfo();
            melvin.WeaponFireDamage = 20;
            melvin.WeaponColdDamage = 10;
            melvin.WeaponPoisonDamage = 30;
            melvin.WeaponDurability = 20;

            melvin.ArmorPhysicalResist = 3;
            melvin.ArmorFireResist = 4;
            melvin.ArmorColdResist = 3;
            melvin.ArmorPoisonResist = 1;
            melvin.ArmorEnergyResist = 1;
            melvin.ArmorDurability = 15;

            CraftAttributeInfo macid = MAcid = new CraftAttributeInfo();

            macid.WeaponPoisonDamage = 75;

            macid.ArmorFireResist = 3;
            macid.ArmorColdResist = 1;
            macid.ArmorPoisonResist = 5;
            macid.ArmorEnergyResist = 3;



            CraftAttributeInfo maqua = MAqua = new CraftAttributeInfo();

            maqua.WeaponColdDamage = 75;

            maqua.ArmorPhysicalResist = 1;
            maqua.ArmorFireResist = 5;
            maqua.ArmorColdResist = 3;
            maqua.ArmorPoisonResist = 2;

            CraftAttributeInfo meldar = MEldar = new CraftAttributeInfo();          
            meldar.WeaponFireDamage = 20;
            meldar.WeaponColdDamage = 20;
            meldar.WeaponPoisonDamage = 20;
            meldar.WeaponEnergyDamage = 40;
            meldar.WeaponDurability = -25;

            meldar.ArmorPhysicalResist = -1;
            meldar.ArmorPoisonResist = 2;
            meldar.ArmorFireResist = 2;
            meldar.ArmorColdResist = 2;
            meldar.ArmorEnergyResist = 7;
            meldar.ArmorDurability = -25;
            meldar.ShieldEnergyResist = 4;
            CraftAttributeInfo mglowing = MGlowing = new CraftAttributeInfo();
            mglowing.WeaponFireDamage = 40;
            mglowing.WeaponEnergyDamage = 40;
            mglowing.WeaponLuck = 5;
            mglowing.ShieldEnergyResist = 5;
            mglowing.ArmorPhysicalResist = 1;
            mglowing.ArmorFireResist = 4;
            mglowing.ArmorColdResist = 2;
            mglowing.ArmorPoisonResist = 2;
            mglowing.ArmorEnergyResist = 4;
            mglowing.ArmorLuck = 5;

            CraftAttributeInfo mgorgan = MGorgan = new CraftAttributeInfo();
            mgorgan.WeaponPoisonDamage = 40;
            mgorgan.WeaponChaosDamage = 60;
            mgorgan.WeaponDurability = 40;
            mgorgan.WeaponLowerRequirements = -5;
            mgorgan.ArmorPhysicalResist = 4;
            mgorgan.ArmorFireResist = 2;
            mgorgan.ArmorColdResist = 2;
            mgorgan.ArmorPoisonResist = 3;
            mgorgan.ArmorEnergyResist = 2;
            mgorgan.ArmorDurability = 30;
            mgorgan.ArmorLowerRequirements = -5;

            CraftAttributeInfo msandrock = MSandrock = new CraftAttributeInfo();
            msandrock.WeaponFireDamage = 30;
            msandrock.WeaponPoisonDamage = 10;
            msandrock.WeaponGoldIncrease = 15;
            msandrock.ArmorPhysicalResist = 1;
            msandrock.ArmorFireResist = 3;
            msandrock.ArmorPoisonResist = 2;
            msandrock.ArmorEnergyResist = 3;
            msandrock.ArmorGoldIncrease = 10;

            CraftAttributeInfo msteel = MSteel = new CraftAttributeInfo();
            msteel.WeaponDirectDamage = 60;
            msteel.WeaponDurability = 100;
            msteel.WeaponLowerRequirements = -40;
            msteel.ArmorPhysicalResist = 4;
            msteel.ArmorFireResist = 5;
            msteel.ArmorColdResist = -1;
            msteel.ArmorPoisonResist = 2;
            msteel.ArmorEnergyResist = 3;
            msteel.ArmorDurability = 80;
            msteel.ArmorLowerRequirements = -40;


            CraftAttributeInfo spined = Spined = new CraftAttributeInfo();

            spined.ArmorPhysicalResist = 5;
            spined.ArmorLuck = 40;
            spined.RunicMinAttributes = 1;
            spined.RunicMaxAttributes = 3;
            if (Core.ML)
            {
                spined.RunicMinIntensity = 40;
                spined.RunicMaxIntensity = 100;
            }
            else
            {
                spined.RunicMinIntensity = 20;
                spined.RunicMaxIntensity = 40;
            }

            CraftAttributeInfo horned = Horned = new CraftAttributeInfo();

            horned.ArmorPhysicalResist = 2;
            horned.ArmorFireResist = 3;
            horned.ArmorColdResist = 2;
            horned.ArmorPoisonResist = 2;
            horned.ArmorEnergyResist = 2;
            horned.RunicMinAttributes = 3;
            horned.RunicMaxAttributes = 4;
            if (Core.ML)
            {
                horned.RunicMinIntensity = 45;
                horned.RunicMaxIntensity = 100;
            }
            else
            {
                horned.RunicMinIntensity = 30;
                horned.RunicMaxIntensity = 70;
            }

            CraftAttributeInfo barbed = Barbed = new CraftAttributeInfo();

            barbed.ArmorPhysicalResist = 2;
            barbed.ArmorFireResist = 1;
            barbed.ArmorColdResist = 2;
            barbed.ArmorPoisonResist = 3;
            barbed.ArmorEnergyResist = 4;
            barbed.RunicMinAttributes = 4;
            barbed.RunicMaxAttributes = 5;
            if (Core.ML)
            {
                barbed.RunicMinIntensity = 50;
                barbed.RunicMaxIntensity = 100;
            }
            else
            {
                barbed.RunicMinIntensity = 40;
                barbed.RunicMaxIntensity = 100;
            }
            
            //viking : ajout du daemon
            CraftAttributeInfo daemon = Daemon = new CraftAttributeInfo();
            
            daemon.ArmorPhysicalResist = 3;
            daemon.ArmorFireResist = 4;
            daemon.ArmorColdResist = 0;
            daemon.ArmorPoisonResist = 3;
            daemon.ArmorEnergyResist = 4;
            daemon.RunicMinAttributes = 3;
            daemon.RunicMaxAttributes = 7;
            if (Core.ML)
            {
                daemon.RunicMinIntensity = 50;
                daemon.RunicMaxIntensity = 100;
            }
            else
            {
                daemon.RunicMinIntensity = 40;
                daemon.RunicMaxIntensity = 100;
            }

            CraftAttributeInfo red = RedScales = new CraftAttributeInfo();

            red.ArmorFireResist = 10;
            red.ArmorColdResist = -3;

            CraftAttributeInfo yellow = YellowScales = new CraftAttributeInfo();

            yellow.ArmorPhysicalResist = -3;
            yellow.ArmorLuck = 20;

            CraftAttributeInfo black = BlackScales = new CraftAttributeInfo();

            black.ArmorPhysicalResist = 10;
            black.ArmorEnergyResist = -3;

            CraftAttributeInfo green = GreenScales = new CraftAttributeInfo();

            green.ArmorFireResist = -3;
            green.ArmorPoisonResist = 10;

            CraftAttributeInfo white = WhiteScales = new CraftAttributeInfo();

            white.ArmorPhysicalResist = -3;
            white.ArmorColdResist = 10;

            CraftAttributeInfo blue = BlueScales = new CraftAttributeInfo();

            blue.ArmorPoisonResist = -3;
            blue.ArmorEnergyResist = 10;

            //public static readonly CraftAttributeInfo OakWood, AshWood, YewWood, Heartwood, Bloodwood, Frostwood;

            CraftAttributeInfo oak = OakWood = new CraftAttributeInfo();
            oak.ShieldEnergyResist = 4;
            oak.ShieldPhysicalResist = 1;
            oak.WeaponDamage = 5;
            oak.WeaponHitChance = 5;

            CraftAttributeInfo ash = AshWood = new CraftAttributeInfo();
            ash.ShieldPoisonResist = 3;
            ash.ShieldEnergyResist = 2;

            ash.WeaponLuck = 40;
            ash.WeaponPoisonDamage = 20;
            ash.WeaponGoldIncrease = 25;

            CraftAttributeInfo yew = YewWood = new CraftAttributeInfo();
            yew.WeaponHitChance = 10;
            yew.WeaponSwingSpeed = 15;
            yew.WeaponDamage = -5;

            yew.ShieldFireResist = 2;
            yew.ShieldPoisonResist = 2;
            yew.ShieldPhysicalResist = 1;

            CraftAttributeInfo heart = Heartwood = new CraftAttributeInfo();
            heart.WeaponFireDamage = 40;
            heart.WeaponEnergyDamage = 20;
            heart.WeaponLowerRequirements = 15;

            heart.ShieldColdResist = 1;
            heart.ShieldEnergyResist = 1;
            heart.ShieldFireResist = 2;
            heart.ShieldPoisonResist = 1;
            heart.ShieldPhysicalResist = 2;
           

            CraftAttributeInfo blood = Bloodwood = new CraftAttributeInfo();
            blood.WeaponHitLifeLeech = 10;
            blood.WeaponRegenHits = 1;
            blood.WeaponLowerRequirements = -15;

            blood.ShieldPoisonResist = 3;
            blood.ShieldFireResist = 3;

            CraftAttributeInfo frost = Frostwood = new CraftAttributeInfo();
            frost.WeaponColdDamage = 75;
            frost.WeaponSwingSpeed = -5;
            frost.WeaponDamage = 10;
            frost.WeaponHitChance = 5;

            frost.ShieldColdResist = 4;
            frost.ShieldFireResist = -1;
            frost.ShieldEnergyResist = -1;
            frost.ShieldPhysicalResist = 3;
            frost.ShieldPoisonResist = 2;
        }
    }

    public class CraftResourceInfo
    {
        private int m_Hue;
        private int m_Number;
        private string m_Name;
        private CraftAttributeInfo m_AttributeInfo;
        private CraftResource m_Resource;
        private Type[] m_ResourceTypes;

        public int Hue { get { return m_Hue; } set { m_Hue = value; } }
        public int Number { get { return m_Number; } }
        public string Name { get { return m_Name; } }
        public CraftAttributeInfo AttributeInfo { get { return m_AttributeInfo; } }
        public CraftResource Resource { get { return m_Resource; } }
        public Type[] ResourceTypes { get { return m_ResourceTypes; } }

        public CraftResourceInfo(int hue, int number, string name, CraftAttributeInfo attributeInfo, CraftResource resource, params Type[] resourceTypes)
        {
            m_Hue = hue;
            m_Number = number;
            m_Name = name;
            m_AttributeInfo = attributeInfo;
            m_Resource = resource;
            m_ResourceTypes = resourceTypes;

            for (int i = 0; i < resourceTypes.Length; ++i)
                CraftResources.RegisterType(resourceTypes[i], resource);
        }
    }

    public class CraftResources
    {
        private static CraftResourceInfo[] m_MetalInfo = new CraftResourceInfo[]
			{
				new CraftResourceInfo( 0x000, 0, "Fer",			    CraftAttributeInfo.MIron,		CraftResource.MIron,				typeof( IronIngot ),		typeof( IronOre ),			typeof( Granite ) ),
				new CraftResourceInfo( 0x6D6, 0, "Bronze",	        CraftAttributeInfo.MBronze,	    CraftResource.MBronze,		        typeof( BronzeIngot ),	    typeof( BronzeOre ),	typeof( BronzeGranite ) ),
				new CraftResourceInfo( 0x45E, 0, "Or",	            CraftAttributeInfo.MGold,	    CraftResource.MGold,		        typeof( GoldIngot ),	    typeof( GoldOre ),	typeof( GoldGranite ) ),
				new CraftResourceInfo( 0x641, 0, "Cuivre",		    CraftAttributeInfo.MCopper,		CraftResource.MCopper,			    typeof( CopperIngot ),		typeof( CopperOre ),		typeof( CopperGranite ) ),
				new CraftResourceInfo( 0x590, 0, "Vieux cuivre",	CraftAttributeInfo.MOldcopper,	CraftResource.MOldcopper,			typeof( OldcopperIngot ),	typeof( OldcopperOre ),		typeof( OldcopperGranite ) ),
				new CraftResourceInfo( 0x60A, 0, "Cuivre terni",	CraftAttributeInfo.MDullcopper,	CraftResource.MDullcopper,			typeof( DullcopperIngot ),	typeof( DullcopperOre ),			typeof( DullcopperGranite ) ),
				new CraftResourceInfo( 0x231, 0, "Argent",		    CraftAttributeInfo.MSilver,		CraftResource.MSilver,			    typeof( SilverIngot ),		typeof( SilverOre ),		typeof( SilverGranite ) ),
				new CraftResourceInfo( 0x770, 0, "Sombrine",		CraftAttributeInfo.MShadow,		CraftResource.MShadow,			    typeof( ShadowIngot ),		typeof( ShadowOre ),		typeof( ShadowGranite ) ),
				new CraftResourceInfo( 0x4C2, 0, "Pierre de sang",	CraftAttributeInfo.MBloodrock,	CraftResource.MBloodrock,			typeof( BloodrockIngot ),	typeof( BloodrockOre ),		typeof( BloodrockGranite ) ),
                new CraftResourceInfo( 0x455, 0, "Pierre noire",	CraftAttributeInfo.MBlackrock,	CraftResource.MBlackrock,			typeof( BlackrockIngot ),	typeof( BlackrockOre ),			typeof( BlackrockGranite ) ),
				new CraftResourceInfo( 0x52D, 0, "Mytheril",	    CraftAttributeInfo.MMytheril,	CraftResource.MMytheril,		    typeof( MytherilIngot ),	typeof( MytherilOre ),	typeof( MytherilGranite ) ),
				new CraftResourceInfo( 0x665, 0, "Rose",	        CraftAttributeInfo.MRose,	    CraftResource.MRose,		        typeof( RoseIngot ),	    typeof( RoseOre ),	typeof( RoseGranite ) ),
				new CraftResourceInfo( 0x7D1, 0, "Verite",		    CraftAttributeInfo.MVerite,		CraftResource.MVerite,			    typeof( VeriteIngot ),		typeof( VeriteOre ),		typeof( VeriteGranite ) ),
				new CraftResourceInfo( 0x400, 0, "Agapite",		    CraftAttributeInfo.MAgapite,	CraftResource.MAgapite,			    typeof( AgapiteIngot ),		typeof( AgapiteOre ),		typeof( AgapiteGranite ) ),
				new CraftResourceInfo( 0x750, 0, "Rouille",			CraftAttributeInfo.MRusty,		CraftResource.MRusty,				typeof( RustyIngot ),		typeof( RustyOre ),			typeof( RustyGranite ) ),
				new CraftResourceInfo( 0x515, 0, "Valorite",		CraftAttributeInfo.MValorite,	CraftResource.MValorite,			typeof( ValoriteIngot ),	typeof( ValoriteOre ),		typeof( ValoriteGranite ) ),
				new CraftResourceInfo( 0x66A, 0, "Dragon",		    CraftAttributeInfo.MDragon,		CraftResource.MDragon,			    typeof( DragonIngot ),		typeof( DragonOre ),		typeof( DragonGranite ) ),
				new CraftResourceInfo( 0x8A5, 0, "Titan",		    CraftAttributeInfo.MTitan,	    CraftResource.MTitan,			    typeof( TitanIngot ),	    typeof( TitanOre ),		typeof( TitanGranite ) ),
				new CraftResourceInfo( 0x4D5, 0, "Crystaline",		CraftAttributeInfo.MCrystaline,	CraftResource.MCrystaline,			typeof( CrystalineIngot ),	typeof( CrystalineOre ),			typeof( CrystalineGranite ) ),
				new CraftResourceInfo( 0x9C/*0x84E*/, 0, "Krynite",	        CraftAttributeInfo.MKrynite,	CraftResource.MKrynite,		        typeof( KryniteIngot ), 	typeof( KryniteOre ),	typeof( KryniteGranite ) ),
				new CraftResourceInfo( 0x4E9/*0x977*/, 0, "Vulcan",	        CraftAttributeInfo.MVulcan,	    CraftResource.MVulcan,		        typeof( VulcanIngot ),	    typeof( VulcanOre ),	typeof( VulcanGranite ) ),
				new CraftResourceInfo( 0x846, 0, "Craie de sang",	CraftAttributeInfo.MBloodcrest,	CraftResource.MBloodcrest,			typeof( BloodcrestIngot ),	typeof( BloodcrestOre ),		typeof( BloodcrestGranite ) ),
				new CraftResourceInfo( 0x8A4, 0, "Elvin",		    CraftAttributeInfo.MElvin,		CraftResource.MElvin,			    typeof( ElvinIngot ),		typeof( ElvinOre ),		typeof( ElvinGranite ) ),
				new CraftResourceInfo( 0x84F, 0, "Acid",			CraftAttributeInfo.MAcid,		CraftResource.MAcid,			    typeof( AcidIngot ),		typeof( AcidOre ),			typeof( AcidGranite ) ),
				new CraftResourceInfo( 0x48D, 0, "Aqua",		        CraftAttributeInfo.MAqua,		CraftResource.MAqua,			    typeof( AquaIngot ),		typeof( AquaOre ),		typeof( AquaGranite ) ),
				new CraftResourceInfo( 0x4DD, 0, "Eldar",		    CraftAttributeInfo.MEldar,		CraftResource.MEldar,			    typeof( EldarIngot ),		typeof( EldarOre ),		typeof( EldarGranite ) ),
				new CraftResourceInfo( 0x482, 0, "Glowing",		    CraftAttributeInfo.MGlowing,	CraftResource.MGlowing,			    typeof( GlowingIngot ),		typeof( GlowingOre ),		typeof( GlowingGranite ) ),
				new CraftResourceInfo( 0x844, 0, "Gorgan",		    CraftAttributeInfo.MGorgan,		CraftResource.MGorgan,			    typeof( GorganIngot ),		typeof( GorganOre ),		typeof( GorganGranite ) ),
				new CraftResourceInfo( 0x92/*0x8F*/, 0, "Pierre de sable",	CraftAttributeInfo.MSandrock,	CraftResource.MSandrock,			typeof( SandrockIngot ),	typeof( SandrockOre ),		typeof( SandrockGranite ) ),
               	new CraftResourceInfo( 0x977/*0x100*/, 0, "Acier",		    CraftAttributeInfo.MSteel,	    CraftResource.MSteel,			    typeof( SteelIngot ),	    typeof( SteelOre ),		typeof( SteelGranite ) ),

			};

        private static CraftResourceInfo[] m_ScaleInfo = new CraftResourceInfo[]
			{
				new CraftResourceInfo( 0x66D, 1053129, "Red Scales",	CraftAttributeInfo.RedScales,		CraftResource.RedScales,		typeof( RedScales ) ),
				new CraftResourceInfo( 0x8A8, 1053130, "Yellow Scales",	CraftAttributeInfo.YellowScales,	CraftResource.YellowScales,		typeof( YellowScales ) ),
				new CraftResourceInfo( 0x455, 1053131, "Black Scales",	CraftAttributeInfo.BlackScales,		CraftResource.BlackScales,		typeof( BlackScales ) ),
				new CraftResourceInfo( 0x851, 1053132, "Green Scales",	CraftAttributeInfo.GreenScales,		CraftResource.GreenScales,		typeof( GreenScales ) ),
				new CraftResourceInfo( 0x8FD, 1053133, "White Scales",	CraftAttributeInfo.WhiteScales,		CraftResource.WhiteScales,		typeof( WhiteScales ) ),
				new CraftResourceInfo( 0x8B0, 1053134, "Blue Scales",	CraftAttributeInfo.BlueScales,		CraftResource.BlueScales,		typeof( BlueScales ) )
			};

        private static CraftResourceInfo[] m_LeatherInfo = new CraftResourceInfo[]
			{
				new CraftResourceInfo( 0x000, 1049353, "Normal",		CraftAttributeInfo.Blank,		CraftResource.RegularLeather,	typeof( Leather ),			typeof( Hides ) ),
				new CraftResourceInfo( 0x283, 1049354, "Spined",		CraftAttributeInfo.Spined,		CraftResource.SpinedLeather,	typeof( SpinedLeather ),	typeof( SpinedHides ) ),
				new CraftResourceInfo( 0x227, 1049355, "Horned",		CraftAttributeInfo.Horned,		CraftResource.HornedLeather,	typeof( HornedLeather ),	typeof( HornedHides ) ),
				new CraftResourceInfo( 0x1C1, 1049356, "Barbed",		CraftAttributeInfo.Barbed,		CraftResource.BarbedLeather,	typeof( BarbedLeather ),	typeof( BarbedHides ) ), 
                new CraftResourceInfo( 0x25, 0, "Daemon",		CraftAttributeInfo.Daemon,		CraftResource.DaemonLeather,	typeof( DaemonLeather ),	typeof( DaemonHides ) )
			};

        private static CraftResourceInfo[] m_AOSLeatherInfo = new CraftResourceInfo[]
			{
				new CraftResourceInfo( 0x000, 1049353, "Normal",		CraftAttributeInfo.Blank,		CraftResource.RegularLeather,	typeof( Leather ),			typeof( Hides ) ),
				new CraftResourceInfo( 0x8AC, 1049354, "Spined",		CraftAttributeInfo.Spined,		CraftResource.SpinedLeather,	typeof( SpinedLeather ),	typeof( SpinedHides ) ),
				new CraftResourceInfo( 0x845, 1049355, "Horned",		CraftAttributeInfo.Horned,		CraftResource.HornedLeather,	typeof( HornedLeather ),	typeof( HornedHides ) ),
				new CraftResourceInfo( 0x851, 1049356, "Barbed",		CraftAttributeInfo.Barbed,		CraftResource.BarbedLeather,	typeof( BarbedLeather ),	typeof( BarbedHides ) ),
                new CraftResourceInfo( 0x25, 0, "Daemon",		CraftAttributeInfo.Daemon,		CraftResource.DaemonLeather,	typeof( DaemonLeather ),	typeof( DaemonHides ) )
			};

        private static CraftResourceInfo[] m_WoodInfo = new CraftResourceInfo[]
			{
				new CraftResourceInfo( 0x000, 1011542, "Normal",		CraftAttributeInfo.Blank,		CraftResource.RegularWood,	typeof( Log ),			typeof( Board ) ),
				new CraftResourceInfo( 0x7DA, 1072533, "Oak",			CraftAttributeInfo.OakWood,		CraftResource.OakWood,		typeof( OakLog ),		typeof( OakBoard ) ),
				new CraftResourceInfo( 0x4A7, 1072534, "Ash",			CraftAttributeInfo.AshWood,		CraftResource.AshWood,		typeof( AshLog ),		typeof( AshBoard ) ),
				new CraftResourceInfo( 0x4A8, 1072535, "Yew",			CraftAttributeInfo.YewWood,		CraftResource.YewWood,		typeof( YewLog ),		typeof( YewBoard ) ),
				new CraftResourceInfo( 0x4A9, 1072536, "Heartwood",		CraftAttributeInfo.Heartwood,	CraftResource.Heartwood,	typeof( HeartwoodLog ),	typeof( HeartwoodBoard ) ),
				new CraftResourceInfo( 0x4AA, 1072538, "Bloodwood",		CraftAttributeInfo.Bloodwood,	CraftResource.Bloodwood,	typeof( BloodwoodLog ),	typeof( BloodwoodBoard ) ),
				new CraftResourceInfo( 0x47F, 1072539, "Frostwood",		CraftAttributeInfo.Frostwood,	CraftResource.Frostwood,	typeof( FrostwoodLog ),	typeof( FrostwoodBoard ) )
			};

        /// <summary>
        /// Returns true if '<paramref name="resource"/>' is None, Iron, RegularLeather or RegularWood. False if otherwise.
        /// </summary>
        public static bool IsStandard(CraftResource resource)
        {
            return (resource == CraftResource.None || resource == CraftResource.MIron || resource == CraftResource.RegularLeather || resource == CraftResource.RegularWood);
        }

        private static Hashtable m_TypeTable;

        /// <summary>
        /// Registers that '<paramref name="resourceType"/>' uses '<paramref name="resource"/>' so that it can later be queried by <see cref="CraftResources.GetFromType"/>
        /// </summary>
        public static void RegisterType(Type resourceType, CraftResource resource)
        {
            if (m_TypeTable == null)
                m_TypeTable = new Hashtable();

            m_TypeTable[resourceType] = resource;
        }

        /// <summary>
        /// Returns the <see cref="CraftResource"/> value for which '<paramref name="resourceType"/>' uses -or- CraftResource.None if an unregistered type was specified.
        /// </summary>
        public static CraftResource GetFromType(Type resourceType)
        {
            if (m_TypeTable == null)
                return CraftResource.None;

            object obj = m_TypeTable[resourceType];

            if (!(obj is CraftResource))
                return CraftResource.None;

            return (CraftResource)obj;
        }

        /// <summary>
        /// Returns a <see cref="CraftResourceInfo"/> instance describing '<paramref name="resource"/>' -or- null if an invalid resource was specified.
        /// </summary>
        public static CraftResourceInfo GetInfo(CraftResource resource)
        {
            CraftResourceInfo[] list = null;

            switch (GetType(resource))
            {
                case CraftResourceType.Metal: list = m_MetalInfo; break;
                case CraftResourceType.Leather: list = Core.AOS ? m_AOSLeatherInfo : m_LeatherInfo; break;
                case CraftResourceType.Scales: list = m_ScaleInfo; break;
                case CraftResourceType.Wood: list = m_WoodInfo; break;
            }

            if (list != null)
            {
                int index = GetIndex(resource);

                if (index >= 0 && index < list.Length)
                    return list[index];
            }

            return null;
        }

        /// <summary>
        /// Returns a <see cref="CraftResourceType"/> value indiciating the type of '<paramref name="resource"/>'.
        /// </summary>
        public static CraftResourceType GetType(CraftResource resource)
        {
            if (resource >= CraftResource.MIron && resource <= CraftResource.MSteel)
                return CraftResourceType.Metal;

            if (resource >= CraftResource.RegularLeather && resource <= CraftResource.DaemonLeather)
                return CraftResourceType.Leather;

            if (resource >= CraftResource.RedScales && resource <= CraftResource.BlueScales)
                return CraftResourceType.Scales;

            if (resource >= CraftResource.RegularWood && resource <= CraftResource.Frostwood)
                return CraftResourceType.Wood;

            return CraftResourceType.None;
        }

        /// <summary>
        /// Returns the first <see cref="CraftResource"/> in the series of resources for which '<paramref name="resource"/>' belongs.
        /// </summary>
        public static CraftResource GetStart(CraftResource resource)
        {
            switch (GetType(resource))
            {
                case CraftResourceType.Metal: return CraftResource.MIron;
                case CraftResourceType.Leather: return CraftResource.RegularLeather;
                case CraftResourceType.Scales: return CraftResource.RedScales;
                case CraftResourceType.Wood: return CraftResource.RegularWood;
            }

            return CraftResource.None;
        }

        /// <summary>
        /// Returns the index of '<paramref name="resource"/>' in the seriest of resources for which it belongs.
        /// </summary>
        public static int GetIndex(CraftResource resource)
        {
            CraftResource start = GetStart(resource);

            if (start == CraftResource.None)
                return 0;

            return (int)(resource - start);
        }

        /// <summary>
        /// Returns the <see cref="CraftResourceInfo.Number"/> property of '<paramref name="resource"/>' -or- 0 if an invalid resource was specified.
        /// </summary>
        public static int GetLocalizationNumber(CraftResource resource)
        {
            CraftResourceInfo info = GetInfo(resource);

            return (info == null ? 0 : info.Number);
        }

        /// <summary>
        /// Returns the <see cref="CraftResourceInfo.Hue"/> property of '<paramref name="resource"/>' -or- 0 if an invalid resource was specified.
        /// </summary>
        public static int GetHue(CraftResource resource)
        {
            CraftResourceInfo info = GetInfo(resource);

            return (info == null ? 0 : info.Hue);
        }

        /// <summary>
        /// Returns the <see cref="CraftResourceInfo.Name"/> property of '<paramref name="resource"/>' -or- an empty string if the resource specified was invalid.
        /// </summary>
        public static string GetName(CraftResource resource)
        {
            CraftResourceInfo info = GetInfo(resource);

            return (info == null ? String.Empty : info.Name);
        }

        /// <summary>
        /// Returns the <see cref="CraftResource"/> value which represents '<paramref name="info"/>' -or- CraftResource.None if unable to convert.
        /// </summary>
        public static CraftResource GetFromOreInfo(OreInfo info)
        {
            if (info.Name.IndexOf("Spined") >= 0)
                return CraftResource.SpinedLeather;
            else if (info.Name.IndexOf("Horned") >= 0)
                return CraftResource.HornedLeather;
            else if (info.Name.IndexOf("Barbed") >= 0)
                return CraftResource.BarbedLeather;
            else if (info.Name.IndexOf("Leather") >= 0)
                return CraftResource.RegularLeather;
            else if (info.Name.IndexOf("Daemon") >= 0)
                return CraftResource.DaemonLeather;

            if (info.Level == 0)
                return CraftResource.MIron;
            else if (info.Level == 1)
                return CraftResource.MBronze;
            else if (info.Level == 2)
                return CraftResource.MGold;
            else if (info.Level == 3)
                return CraftResource.MCopper;
            else if (info.Level == 4)
                return CraftResource.MOldcopper;
            else if (info.Level == 5)
                return CraftResource.MDullcopper;
            else if (info.Level == 6)
                return CraftResource.MSilver;
            else if (info.Level == 7)
                return CraftResource.MShadow;
            else if (info.Level == 8)
                return CraftResource.MBloodrock;
            else if (info.Level == 9)
                return CraftResource.MBlackrock;
            else if (info.Level == 10)
                return CraftResource.MMytheril;
            else if (info.Level == 11)
                return CraftResource.MRose;
            else if (info.Level == 12)
                return CraftResource.MVerite;
            else if (info.Level == 13)
                return CraftResource.MAgapite;
            else if (info.Level == 14)
                return CraftResource.MRusty;
            else if (info.Level == 15)
                return CraftResource.MValorite;
            else if (info.Level == 16)
                return CraftResource.MDragon;
            else if (info.Level == 17)
                return CraftResource.MTitan;
            else if (info.Level == 18)
                return CraftResource.MCrystaline;
            else if (info.Level == 19)
                return CraftResource.MKrynite;
            else if (info.Level == 20)
                return CraftResource.MVulcan;
            else if (info.Level == 21)
                return CraftResource.MBloodcrest;
            else if (info.Level == 22)
                return CraftResource.MElvin;
            else if (info.Level == 23)
                return CraftResource.MAcid;
            else if (info.Level == 24)
                return CraftResource.MAqua;
            else if (info.Level == 25)
                return CraftResource.MEldar;
            else if (info.Level == 26)
                return CraftResource.MGlowing;
            else if (info.Level == 27)
                return CraftResource.MGorgan;
            else if (info.Level == 28)
                return CraftResource.MSandrock;
            else if (info.Level == 29)
                return CraftResource.MSteel;

            return CraftResource.None;
        }

        /// <summary>
        /// Returns the <see cref="CraftResource"/> value which represents '<paramref name="info"/>', using '<paramref name="material"/>' to help resolve leather OreInfo instances.
        /// </summary>
        public static CraftResource GetFromOreInfo(OreInfo info, ArmorMaterialType material)
        {
            if (material == ArmorMaterialType.Studded || material == ArmorMaterialType.Leather || material == ArmorMaterialType.Spined ||
                material == ArmorMaterialType.Horned || material == ArmorMaterialType.Barbed || material == ArmorMaterialType.Daemon)
            {
                if (info.Level == 0)
                    return CraftResource.RegularLeather;
                else if (info.Level == 1)
                    return CraftResource.SpinedLeather;
                else if (info.Level == 2)
                    return CraftResource.HornedLeather;
                else if (info.Level == 3)
                    return CraftResource.BarbedLeather;
                else if (info.Level == 4)
                    return CraftResource.DaemonLeather;

                return CraftResource.None;
            }

            return GetFromOreInfo(info);
        }
    }

    // NOTE: This class is only for compatability with very old RunUO versions.
    // No changes to it should be required for custom resources.
    public class OreInfo
    {
        public static readonly OreInfo MIron = new OreInfo(0, 0x000, "Fer");
        public static readonly OreInfo MBronze = new OreInfo(1, 0x973, "Bronze");
        public static readonly OreInfo MGold = new OreInfo(2, 0x966, "Or");
        public static readonly OreInfo MCopper = new OreInfo(3, 0x96D, "Cuivre");
        public static readonly OreInfo MOldcopper = new OreInfo(4, 0x972, "Vieux cuivre");
        public static readonly OreInfo MDullcopper = new OreInfo(5, 0x8A5, "Cuivre terni");
        public static readonly OreInfo MSilver = new OreInfo(6, 0x979, "Argent");
        public static readonly OreInfo MShadow = new OreInfo(7, 0x89F, "Sombrine");
        public static readonly OreInfo MBloodrock = new OreInfo(8, 0x8AB, "Pierre de sang");
        public static readonly OreInfo MBlackrock = new OreInfo(9, 0x000, "Pierre noire");
        public static readonly OreInfo MMytheril = new OreInfo(10, 0x973, "Mytheril");
        public static readonly OreInfo MRose = new OreInfo(11, 0x966, "Rose");
        public static readonly OreInfo MVerite = new OreInfo(12, 0x96D, "Verite");
        public static readonly OreInfo MAgapite = new OreInfo(13, 0x972, "Agapite");
        public static readonly OreInfo MRusty = new OreInfo(14, 0x8A5, "Rouille");
        public static readonly OreInfo MValorite = new OreInfo(15, 0x979, "Valorite");
        public static readonly OreInfo MDragon = new OreInfo(16, 0x89F, "Dragon");
        public static readonly OreInfo MTitan = new OreInfo(17, 0x8AB, "Titan");
        public static readonly OreInfo MCrystaline = new OreInfo(18, 0x000, "Crystaline");
        public static readonly OreInfo MKrynite = new OreInfo(19, 0x973, "Krynite");
        public static readonly OreInfo MVulcan = new OreInfo(20, 0x966, "Vulcan");
        public static readonly OreInfo MBloodcrest = new OreInfo(21, 0x96D, "Craie de sang");
        public static readonly OreInfo MElvin = new OreInfo(22, 0x972, "Elvin");
        public static readonly OreInfo MAcid = new OreInfo(23, 0x8A5, "Acid");
        public static readonly OreInfo MAqua = new OreInfo(24, 0x979, "Aqua");
        public static readonly OreInfo MEldar = new OreInfo(25, 0x89F, "Eldar");
        public static readonly OreInfo MGlowing = new OreInfo(26, 0x8AB, "Glowing");
        public static readonly OreInfo MGorgan = new OreInfo(27, 0x979, "Gorgan");
        public static readonly OreInfo MSandrock = new OreInfo(28, 0x89F, "Pierre de sable");
        public static readonly OreInfo MSteel = new OreInfo(29, 0x8AB, "Acier");

        private int m_Level;
        private int m_Hue;
        private string m_Name;

        public OreInfo(int level, int hue, string name)
        {
            m_Level = level;
            m_Hue = hue;
            m_Name = name;
        }

        public int Level
        {
            get
            {
                return m_Level;
            }
        }

        public int Hue
        {
            get
            {
                return m_Hue;
            }
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
        }
    }
}