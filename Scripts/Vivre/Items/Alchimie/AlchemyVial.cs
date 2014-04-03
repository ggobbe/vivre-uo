using System;
using System.Collections;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public enum LiquidType //Attention! Changer la version du slayerforge si modifié
    {
        None,
        OgreBlood,
        OrcBlood,
        TrollBlood,
        BloodElemental,
        EarthElemental,
        PoisonElemental,
        FireElemental,
        IceElemental,
        AirElemental,
        WaterElemental,
        GargoyleBlood,
        ScorpionBlood,
        SpiderBlood,
        TerathanBlood,
        LizardmanBlood,
        DragonBlood,
        OphidianBlood,
        SerpentBlood,
        /*Special*/
        UndeadBlood,
        FairyBlood,
        DaemonBlood,
        /*EndSpecial*/
        ChangelingBlood
    }

    public class AlchemyVial : Item
    {
        private LiquidType m_AlchemyLiquidType;

        [CommandProperty(AccessLevel.GameMaster)]
        public LiquidType AlchemyLiquidType
        {
            get { return m_AlchemyLiquidType; }
            set { m_AlchemyLiquidType = value; ComputeProperties(); }
        }

        [Constructable]
        public AlchemyVial()
            : base(0x0E24)
        {
            Name = "Éprouvette vide";
            Weight = 1.0;
            Movable = true;
            Stackable = true;
        }

        public override bool StackWith(Mobile from, Item dropped, bool playSound)
        {
            if (dropped.Hue != Hue)
                return false; 

            return base.StackWith(from, dropped, playSound);
        }

        public void ComputeProperties()
        {
            #region Choisi le Hue et le Name
            switch (AlchemyLiquidType)
            {
                case (LiquidType.OgreBlood):
                    {
                        Hue = 1372;
                        Name = "Éprouvette de sang d'ogre";
                        break;
                    }
                case (LiquidType.OrcBlood):
                    {
                        Hue = 2601;
                        Name = "Éprouvette de sang d'orc";
                        break;
                    }
                case (LiquidType.TrollBlood):
                    {
                        Hue = 70;
                        Name = "Éprouvette de sang de troll";
                        break;
                    }
                case (LiquidType.BloodElemental):
                    {
                        Hue = 32;
                        Name = "Éprouvette d'élément de sang";
                        break;
                    }
                case (LiquidType.EarthElemental):
                    {
                        Hue = 152;
                        Name = "Éprouvette d'élément de terre";
                        break;
                    }
                case (LiquidType.PoisonElemental):
                    {
                        Hue = 67;
                        Name = "Éprouvette d'élément de poison";
                        break;
                    }
                case (LiquidType.FireElemental):
                    {
                        Hue = 1260;
                        Name = "Éprouvette d'élément de feu";
                        break;
                    }
                case (LiquidType.IceElemental):
                    {
                        Hue = 86;
                        Name = "Éprouvette d'élément de glace";
                        break;
                    }
                case (LiquidType.AirElemental):
                    {
                        Hue = 2042;
                        Name = "Éprouvette d'élément d'air";
                        break;
                    }
                case (LiquidType.WaterElemental):
                    {
                        Hue = 93;
                        Name = "Éprouvette d'élément d'eau";
                        break;
                    }
                case (LiquidType.GargoyleBlood):
                    {
                        Hue = 2504;
                        Name = "Éprouvette de sang de gargouille";
                        break;
                    }
                case (LiquidType.SpiderBlood):
                    {
                        Hue = 12;
                        Name = "Éprouvette de sang d'araignée";
                        break;
                    }
                case (LiquidType.TerathanBlood):
                    {
                        Hue = 1177;
                        Name = "Éprouvette de sang de terathan";
                        break;
                    }
                case (LiquidType.LizardmanBlood):
                    {
                        Hue = 74;
                        Name = "Éprouvette de sang d'homme lézard";
                        break;
                    }
                case (LiquidType.DragonBlood):
                    {
                        Hue = 132;
                        Name = "Éprouvette de sang de dragon";
                        break;
                    }
                case (LiquidType.OphidianBlood):
                    {
                        Hue = 154;
                        Name = "Éprouvette de sang d'ophidien ";
                        break;
                    }
                case (LiquidType.SerpentBlood):
                    {
                        Hue = 168;
                        Name = "Éprouvette de sang de serpent ";
                        break;
                    }
                case (LiquidType.ScorpionBlood):
                    {
                        Hue = 1196;
                        Name = "Éprouvette de sang de scorpion ";
                        break;
                    }
                case (LiquidType.UndeadBlood):
                    {
                        Hue = 1375;
                        Name = "Éprouvette de sang de mort-vivant ";
                        break;
                    }
                case (LiquidType.FairyBlood):
                    {
                        Hue = 1166;
                        Name = "Éprouvette de sang féérique";
                        break;
                    }
                case (LiquidType.DaemonBlood):
                    {
                        Hue = 1;
                        Name = "Éprouvette de sang démoniaque";
                        break;
                    }
                case (LiquidType.ChangelingBlood):
                    {
                        Hue = 19;
                        Name = "Éprouvette de sang de changeling ";
                        break;
                    }

                default:
                    {
                        Hue = 0;
                        Name = "Éprouvette vide";
                        break;
                    }
            }
            #endregion
        }

        public override void OnDoubleClick(Mobile from)
        {

            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("Vous devez avoir l'éprouvette dans votre sac pour l'utiliser");
                return;
            }

            else if (AlchemyLiquidType != LiquidType.None)
            {
                from.SendMessage("Où voulez-vous verser le liquide?");
                from.BeginTarget(2, false, TargetFlags.None, new TargetCallback(OnDropTarget));
                return;
            }
            from.BeginTarget(2, false, TargetFlags.None, new TargetCallback(OnTarget));
            from.SendMessage("Sélectionnez l'échantillon à récolter");
        }


        public void OnTarget(Mobile from, object obj)
        {
            if (!(obj is Corpse))
            {
                from.SendMessage("Il vous faut trouver autre chose pour la remplir");
                return;
            }

            Corpse targcorpse = (Corpse)obj;

            if (targcorpse.Channeled)
            {
                from.SendMessage("Vous ne pouvez récolter que sur un corps frais");
                return;
            }

            Type type = null;

            if (targcorpse.Owner != null)
                type = targcorpse.Owner.GetType();

            AlchemyVial vial = new AlchemyVial();

            #region Check des différents corps
            // Ogre Trashing 
            if (type == typeof(Ogre) || type == typeof(OgreLord) || type == typeof(ArcticOgreLord))
            {
                from.SendMessage("Vous recueillez du sang d'ogre");
                vial.AlchemyLiquidType = LiquidType.OgreBlood;
            }
            //Orc Slaying 
            else if (type == typeof(Orc) || type == typeof(OrcBomber) || type == typeof(OrcBrute) || type == typeof(OrcCaptain) || type == typeof(OrcScout) || type == typeof(OrcishLord) || type == typeof(OrcishMage))
            {
                from.SendMessage("Vous recueillez du sang d'orc");
                vial.AlchemyLiquidType = LiquidType.OrcBlood;
            }
            //Troll Slaughter 
            else if (type == typeof(Troll) || type == typeof(FrostTroll))
            {
                from.SendMessage("Vous recueillez du sang de troll");
                vial.AlchemyLiquidType = LiquidType.TrollBlood;
            }
            //Blood Drinking
            else if (type == typeof(BloodElemental))
            {
                from.SendMessage("Vous recueillez de l'élément de sang");
                vial.AlchemyLiquidType = LiquidType.BloodElemental;
            }
            //Earth Shatter
            else if (type == typeof(AgapiteElemental) || type == typeof(BronzeElemental) || type == typeof(CopperElemental) || type == typeof(DullCopperElemental) || type == typeof(EarthElemental) || type == typeof(GoldenElemental) || type == typeof(ShadowIronElemental) || type == typeof(ValoriteElemental) || type == typeof(VeriteElemental))
            {
                from.SendMessage("Vous recueillez de l'élément de terre");
                vial.AlchemyLiquidType = LiquidType.EarthElemental;
            }
            //Elemental Health
            else if (type == typeof(PoisonElemental))
            {
                from.SendMessage("Vous recueillez de l'élément de poison");
                vial.AlchemyLiquidType = LiquidType.PoisonElemental;
            }
            //Flame Dousing
            else if (type == typeof(FireElemental))
            {
                from.SendMessage("Vous recueillez de l'élément de feu");
                vial.AlchemyLiquidType = LiquidType.FireElemental;
            }
            //Summer Wind
            else if (type == typeof(SnowElemental) || type == typeof(IceElemental))
            {
                from.SendMessage("Vous recueillez de l'élément de glace");
                vial.AlchemyLiquidType = LiquidType.IceElemental;
            }
            //Vacuum
            else if (type == typeof(AirElemental))
            {
                from.SendMessage("Vous recueillez de l'élément de vent");
                vial.AlchemyLiquidType = LiquidType.AirElemental;
            }
            //Water Dissipation
            else if (type == typeof(WaterElemental))
            {
                from.SendMessage("Vous recueillez de l'élément d'eau");
                vial.AlchemyLiquidType = LiquidType.WaterElemental;
            }
            //Gargoyle Foes
            else if (type == typeof(EnslavedGargoyle) || type == typeof(FireGargoyle) || type == typeof(Gargoyle) || type == typeof(GargoyleDestroyer) || type == typeof(GargoyleEnforcer) || type == typeof(StoneGargoyle))
            {
                from.SendMessage("Vous recueillez du sang de gargouille");
                vial.AlchemyLiquidType = LiquidType.GargoyleBlood;
            }
            //Scorpions Bane
            else if (type == typeof(Scorpion))
            {
                from.SendMessage("Vous recueillez du sang de scorpion");
                vial.AlchemyLiquidType = LiquidType.ScorpionBlood;
            }
            //Spiders Death
            else if (type == typeof(DreadSpider) || type == typeof(FrostSpider) || type == typeof(GiantBlackWidow) || type == typeof(GiantSpider) || type == typeof(Mephitis))
            {
                from.SendMessage("Vous recueillez du sang d'araignée");
                vial.AlchemyLiquidType = LiquidType.SpiderBlood;
            }
            //Terathan Avenger
            else if (type == typeof(TerathanAvenger) || type == typeof(TerathanDrone) || type == typeof(TerathanMatriarch) || type == typeof(TerathanWarrior))
            {
                from.SendMessage("Vous recueillez du sang de Terathan");
                vial.AlchemyLiquidType = LiquidType.TerathanBlood;
            }
            //Dragon Slayer
            else if (type == typeof(AncientWyrm) || type == typeof(GreaterDragon) || type == typeof(Dragon) || type == typeof(Drake) || type == typeof(Hiryu) || type == typeof(LesserHiryu) || type == typeof(SerpentineDragon) || type == typeof(ShadowWyrm) || type == typeof(SkeletalDragon) || type == typeof(SwampDragon) || type == typeof(WhiteWyrm) || type == typeof(Wyvern))
            {
                from.SendMessage("Vous recueillez du sang de dragon");
                vial.AlchemyLiquidType = LiquidType.DragonBlood;
            }
            //Lizardman Slaughter
            else if (type == typeof(Lizardman))
            {
                from.SendMessage("Vous recueillez du sang d'homme lézard");
                vial.AlchemyLiquidType = LiquidType.LizardmanBlood;
            }
            //Ophidian
            else if (type == typeof(OphidianArchmage) || type == typeof(OphidianKnight) || type == typeof(OphidianMage) || type == typeof(OphidianMatriarch) || type == typeof(OphidianWarrior))
            {
                from.SendMessage("Vous recueillez du sang d'ophidien");
                vial.AlchemyLiquidType = LiquidType.OphidianBlood;
            }
            //Snakes Bane
            else if (type == typeof(DeepSeaSerpent) || type == typeof(GiantIceWorm) || type == typeof(GiantSerpent) || type == typeof(IceSerpent) || type == typeof(IceSnake) || type == typeof(LavaSerpent) || type == typeof(LavaSnake) || type == typeof(SeaSerpent) || type == typeof(Serado) || type == typeof(SilverSerpent) || type == typeof(Snake) || type == typeof(Yamandon))
            {
                from.SendMessage("Vous recueillez du sang de serpent");
                vial.AlchemyLiquidType = LiquidType.SerpentBlood;
            }
            else if (type == typeof(AncientLich) || type == typeof(DarknightCreeper) || type == typeof(FleshGolem) || type == typeof(LadyOfTheSnow) || type == typeof(Lich) || type == typeof(LichLord) || type == typeof(Mummy) || type == typeof(PestilentBandage) || type == typeof(RevenantLion) || type == typeof(RottingCorpse) || type == typeof(ShadowKnight))
            {
                from.SendMessage("Vous recueillez du sang de mort-vivant");
                vial.AlchemyLiquidType = LiquidType.UndeadBlood;
            }
            else if (type == typeof(Centaur) || type == typeof(EtherealWarrior) || type == typeof(Kirin) || type == typeof(LordOaks) || type == typeof(Silvani) || type == typeof(Treefellow) || type == typeof(Unicorn) || type == typeof(MLDryad) || type == typeof(Satyr))
            {
                from.SendMessage("Vous recueillez du sang féérique");
                vial.AlchemyLiquidType = LiquidType.FairyBlood;
            }
            else if (type == typeof(AbysmalHorror) || type == typeof(ArcaneDaemon) || type == typeof(Balron) || type == typeof(BoneDemon) || type == typeof(ChaosDaemon) || type == typeof(Daemon) || type == typeof(DemonKnight) || type == typeof(Devourer) || type == typeof(FanDancer) || type == typeof(Gibberling) || type == typeof(IceFiend) || type == typeof(Impaler) || type == typeof(Oni) || type == typeof(Ravager) || type == typeof(Semidar) || type == typeof(Succubus) || type == typeof(TsukiWolf))
            {
                from.SendMessage("Vous recueillez du sang démoniaque");
                vial.AlchemyLiquidType = LiquidType.DaemonBlood;
            }
            //Potion de changement de sexe
            else if (type == typeof(Changeling))
            {
                from.SendMessage("Vous recueillez du sang de changeling");
                vial.AlchemyLiquidType = LiquidType.ChangelingBlood;
            }
            else
            {
                from.SendMessage("Cela ne vous servira à rien");
                return;
            }
            #endregion

            from.AddItem(vial);
            Consume();
            targcorpse.Channeled = true;
        }

        public void OnDropTarget(Mobile from, object obj)
        {
            if (!(obj is SlayerForge))
            {
                from.SendMessage("Cela ne servirait à rien de verser le liquide ici");
                return;
            }
            
            SlayerForge targ = (SlayerForge)obj;

            if (targ.Movable)
            {
                from.SendMessage("Cela doit être fixé dans une maison pour être utilisé");
                return;
            }
            
            if (this.AlchemyLiquidType == LiquidType.None)
            {
                from.SendMessage("Vous ne pouvez verser une éprouvette vide");
                return;
            }
            
            if (this.AlchemyLiquidType == LiquidType.ChangelingBlood)
            {
                from.SendMessage("Ce contenu est destiné à un autre usage");
                return;
            }
            
            if (targ.CountVial >= targ.MaxVials)
            {
                from.SendMessage("Cette bassine est pleine");
                return;
            }

            from.SendMessage("Vous jetez le contenu de l'éprouvette dans la forge");
            targ.AddVial(this);
            Consume();

            if (Utility.RandomDouble() < (from.RawDex-10))
                from.AddItem(new AlchemyVial());
            else
            {
                from.SendMessage("L'éprouvette vous glisse des mains et se brise");
            }
        }

        
        
        public AlchemyVial(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version 

            writer.WriteEncodedInt((int)m_AlchemyLiquidType);

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_AlchemyLiquidType = (LiquidType)reader.ReadEncodedInt();
        }
    }
}