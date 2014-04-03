using System;
using Server.Engines.Craft;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Items
{
    public enum GemType
    {
        None,
        StarSapphire,
        Emerald,
        Sapphire,
        Ruby,
        Citrine,
        Amethyst,
        Tourmaline,
        Amber,
        Diamond
    }

    public abstract class BaseJewel : BaseWearable, ICraftable
    {
        private int m_MaxHitPoints;
        private int m_HitPoints;
        private Mobile m_Crafter;
        private bool m_Identified;
        private bool m_PlayerConstructed;

        private AosAttributes m_AosAttributes;
        private AosElementAttributes m_AosResistances;
        private AosSkillBonuses m_AosSkillBonuses;
        private CraftResource m_Resource;
        private GemType m_GemType;

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxHitPoints
        {
            get { return m_MaxHitPoints; }
            set { m_MaxHitPoints = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int HitPoints
        {
            get
            {
                return m_HitPoints;
            }
            set
            {
                if (value != m_HitPoints && MaxHitPoints > 0)
                {
                    m_HitPoints = value;

                    if (m_HitPoints < 0)
                        Delete();
                    else if (m_HitPoints > MaxHitPoints)
                        m_HitPoints = MaxHitPoints;

                    InvalidateProperties();
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Crafter
        {
            get { return m_Crafter; }
            set { m_Crafter = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.Player)]
        public AosAttributes Attributes
        {
            get { return m_AosAttributes; }
            set { }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public AosElementAttributes Resistances
        {
            get { return m_AosResistances; }
            set { }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public AosSkillBonuses SkillBonuses
        {
            get { return m_AosSkillBonuses; }
            set { }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource Resource
        {
            get { return m_Resource; }
            set { m_Resource = value; Hue = CraftResources.GetHue(m_Resource); GetNameProperty(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public GemType GemType
        {
            get { return m_GemType; }
            set { m_GemType = value; InvalidateProperties(); GetNameProperty(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool PlayerConstructed
        {
            get { return m_PlayerConstructed; }
            set { m_PlayerConstructed = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Identified
        {
            get { return m_Identified; }
            set { m_Identified = value; InvalidateProperties(); }
        }
        #region difficulty
        //Plume ajout de la difficulté
        [CommandProperty(AccessLevel.GameMaster)]
        public int JewelDifficulty
        {
            get
            {
                int difficulty = 0;

                if (Attributes.AttackChance != 0)
                    difficulty++;
                if (Attributes.BonusDex != 0)
                    difficulty++;
                if (Attributes.BonusHits != 0)
                    difficulty++;
                if (Attributes.BonusInt != 0)
                    difficulty++;
                if (Attributes.BonusMana != 0)
                    difficulty++;
                if (Attributes.BonusStam != 0)
                    difficulty++;
                if (Attributes.BonusStr != 0)
                    difficulty++;
                if (Attributes.CastRecovery != 0)
                    difficulty++;
                if (Attributes.CastSpeed != 0)
                    difficulty++;
                if (Attributes.DefendChance != 0)
                    difficulty++;
                if (Attributes.EnhancePotions != 0)
                    difficulty++;
                if (Attributes.LowerManaCost != 0)
                    difficulty++;
                if (Attributes.LowerRegCost != 0)
                    difficulty++;
                if (Attributes.Luck != 0)
                    difficulty++;
                if (Attributes.NightSight != 0)
                    difficulty++;
                if (Attributes.ReflectPhysical != 0)
                    difficulty++;
                if (Attributes.RegenHits != 0)
                    difficulty++;
                if (Attributes.RegenMana != 0)
                    difficulty++;
                if (Attributes.RegenStam != 0)
                    difficulty++;
                if (Attributes.SpellChanneling != 0)
                    difficulty++;
                if (Attributes.SpellDamage != 0)
                    difficulty++;
                if (Attributes.WeaponDamage != 0)
                    difficulty++;
                if (Attributes.WeaponSpeed != 0)
                    difficulty++;
                if (Resistances.Chaos != 0)
                    difficulty++;
                if (Resistances.Cold != 0)
                    difficulty++;
                if (Resistances.Direct != 0)
                    difficulty++;
                if (Resistances.Energy != 0)
                    difficulty++;
                if (Resistances.Fire != 0)
                    difficulty++;
                if (Resistances.Physical != 0)
                    difficulty++;
                if (Resistances.Poison != 0)
                    difficulty++;
                if (SkillBonuses.Skill_1_Value != 0)
                    difficulty++;
                if (SkillBonuses.Skill_2_Value != 0)
                    difficulty++;
                if (SkillBonuses.Skill_3_Value != 0)
                    difficulty++;
                if (SkillBonuses.Skill_4_Value != 0)
                    difficulty++;
                if (SkillBonuses.Skill_5_Value != 0)
                    difficulty++;

                if (difficulty > 5)
                    difficulty = 5;

                return difficulty;
            }
        }
        #endregion
        public override int PhysicalResistance { get { return m_AosResistances.Physical; } }
        public override int FireResistance { get { return m_AosResistances.Fire; } }
        public override int ColdResistance { get { return m_AosResistances.Cold; } }
        public override int PoisonResistance { get { return m_AosResistances.Poison; } }
        public override int EnergyResistance { get { return m_AosResistances.Energy; } }
        public virtual int BaseGemTypeNumber { get { return 0; } }

        public virtual int InitMinHits { get { return 0; } }
        public virtual int InitMaxHits { get { return 0; } }

        public override int LabelNumber
        {
            get
            {
                if (m_GemType == GemType.None)
                    return base.LabelNumber;

                return BaseGemTypeNumber + (int)m_GemType - 1;
            }
        }
        // Plume : Ajout du sertissage
        public override void OnDoubleClick(Mobile from)
        {
            if (PlayerConstructed && GemType == GemType.None)
            {
                if (from.Skills[SkillName.Tinkering].Value > 80)
                {
                    from.BeginTarget(1, false, TargetFlags.None, new TargetCallback(SertissageTarget));
                    from.SendMessage("Quelle pierre voulez-vous y sertir?");
                }
                else
                    from.SendMessage("Un bijoutier expérimenté pourrait y sertir une pierre précieuse");
                return;
            }

            base.OnDoubleClick(from);
        }

        public void SertissageTarget(Mobile from, object obj)
        {
            if (!(obj is BaseGem))
            {
                from.SendMessage("Ceci ne pourrait être enchâssé sur un bijou.");
                return;
            }

            BaseGem targ = obj as BaseGem;
            int amount = 1;

            if (this is BaseBracelet)
                amount = 4;
            else if (this is BaseRing)
                amount = 1;
            else if (this is BaseNecklace)
                amount = 3;
            else if (this is BaseEarrings)
                amount = 2;
            else if (this is Diademe)
                amount = 1;
            else if (this is DiademeDecore)
                amount = 4;

            if (targ.Amount < amount)
            {
                from.SendMessage("Vous n'avez pas assez de pierres pour sertir un bijou");
                return;
            }
            if (!from.CheckSkill(SkillName.Tinkering, 60.0, 120.0))
            {
                from.SendMessage("Vous ne réussisez pas à sertir le bijou et brisez les pierres");
                targ.Consume(amount);
                return;
            }
            from.SendMessage("Vous sertissez le bijou");

            GemType = targ.Gems;

            targ.Consume(amount);
            return;
        }

        public void GetNameProperty()
        {
            if (!PlayerConstructed)
                return;

            string metaltype = "";
            string gemtype = "";

            if (Resource != CraftResource.None)
                metaltype = CraftResources.GetName(m_Resource);

            if (Resource == CraftResource.MIron)
                Hue = 2105;
            if (Resource == CraftResource.MGold)
                Hue = 0;


            switch (GemType)
            {
                case GemType.Amber: gemtype = "d'ambres"; break;
                case GemType.Amethyst: gemtype = "d'améthystes"; break;
                case GemType.Citrine: gemtype = "de citrines"; break;
                case GemType.Diamond: gemtype = "de diamants"; break;
                case GemType.Emerald: gemtype = "d'émeraudes"; break;
                case GemType.Ruby: gemtype = "de rubis"; break;
                case GemType.Sapphire: gemtype = "de saphirs"; break;
                case GemType.StarSapphire: gemtype = "de saphirs étoilés"; break;
                case GemType.Tourmaline: gemtype = "de tourmalines"; break;
            }

            if (this is GoldBracelet)
                Name = string.Format("Un bracelet{0}{1}", Resource != CraftResource.None ? " en " + metaltype : "", GemType != GemType.None ? " serti " + gemtype : "");
            else if (this is GoldRing)
                Name = string.Format("Une bague{0}{1}", Resource != CraftResource.None ? " en " + metaltype : "", GemType != GemType.None ? " sertie " + gemtype : "");
            else if (this is GoldBeadNecklace)
                Name = string.Format("Un collier de perle{0}{1}", Resource != CraftResource.None ? " en " + metaltype : "", GemType != GemType.None ? " serti " + gemtype : "");
            else if (this is GoldNecklace)
                Name = string.Format("Un collier{0}{1}", Resource != CraftResource.None ? " en " + metaltype : "", GemType != GemType.None ? " serti " + gemtype : "");
            else if (this is GoldEarrings)
                Name = string.Format("Des boucles d'oreille{0}{1}", Resource != CraftResource.None ? " en " + metaltype : "", GemType != GemType.None ? " serties " + gemtype : "");
            else if (this is Diademe)
                Name = string.Format("Un diadème{0}{1}", Resource != CraftResource.None ? " en " + metaltype : "", GemType != GemType.None ? " serti " + gemtype : "");
            else if (this is DiademeDecore)
                Name = string.Format("Un diadème{0}{1}", Resource != CraftResource.None ? " en " + metaltype : "", GemType != GemType.None ? " richement serti " + gemtype : "");
            else
                Name = string.Format("Un bijou{0}{1}", Resource != CraftResource.None ? " en " + metaltype : "", GemType != GemType.None ? " serti " + gemtype : "");

        }

        public override void OnAfterDuped(Item newItem)
        {
            BaseJewel jewel = newItem as BaseJewel;

            if (jewel == null)
                return;

            jewel.m_AosAttributes = new AosAttributes(newItem, m_AosAttributes);
            jewel.m_AosResistances = new AosElementAttributes(newItem, m_AosResistances);
            jewel.m_AosSkillBonuses = new AosSkillBonuses(newItem, m_AosSkillBonuses);
        }

        public virtual int ArtifactRarity { get { return 0; } }

        public BaseJewel(int itemID, Layer layer)
            : base(itemID)
        {
            m_AosAttributes = new AosAttributes(this);
            m_AosResistances = new AosElementAttributes(this);
            m_AosSkillBonuses = new AosSkillBonuses(this);
            m_Resource = CraftResource.MIron;
            m_GemType = GemType.None;
            m_Identified = true;
            m_Crafter = null;
            Layer = layer;
            m_HitPoints = m_MaxHitPoints = Utility.RandomMinMax(InitMinHits, InitMaxHits);
        }

        public override bool CanEquip(Mobile from)
        {
            if (!m_Identified)
            {
                from.SendMessage("Ce bijou vous semble inconnu");
                return false;
            }
            return base.CanEquip(from);

        }

        public override void OnAdded(object parent)
        {
            if (Core.AOS && parent is Mobile)
            {
                Mobile from = (Mobile)parent;

                m_AosSkillBonuses.AddTo(from);

                int strBonus = m_AosAttributes.BonusStr;
                int dexBonus = m_AosAttributes.BonusDex;
                int intBonus = m_AosAttributes.BonusInt;

                if (strBonus != 0 || dexBonus != 0 || intBonus != 0)
                {
                    string modName = this.Serial.ToString();

                    if (strBonus != 0)
                        from.AddStatMod(new StatMod(StatType.Str, modName + "Str", strBonus, TimeSpan.Zero));

                    if (dexBonus != 0)
                        from.AddStatMod(new StatMod(StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero));

                    if (intBonus != 0)
                        from.AddStatMod(new StatMod(StatType.Int, modName + "Int", intBonus, TimeSpan.Zero));
                }

                from.CheckStatTimers();
            }
        }

        public override void OnRemoved(object parent)
        {
            if (Core.AOS && parent is Mobile)
            {
                Mobile from = (Mobile)parent;

                m_AosSkillBonuses.Remove();

                string modName = this.Serial.ToString();

                from.RemoveStatMod(modName + "Str");
                from.RemoveStatMod(modName + "Dex");
                from.RemoveStatMod(modName + "Int");

                from.CheckStatTimers();
            }
        }

        public BaseJewel(Serial serial)
            : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (m_Identified)
            {
                m_AosSkillBonuses.GetProperties(list);

                int prop;

                if ((prop = ArtifactRarity) > 0)
                    list.Add(1061078, prop.ToString()); // artifact rarity ~1_val~

                if ((prop = m_AosAttributes.WeaponDamage) != 0)
                    list.Add(1060401, prop.ToString()); // damage increase ~1_val~%

                if ((prop = m_AosAttributes.DefendChance) != 0)
                    list.Add(1060408, prop.ToString()); // defense chance increase ~1_val~%

                if ((prop = m_AosAttributes.BonusDex) != 0)
                    list.Add(1060409, prop.ToString()); // dexterity bonus ~1_val~

                if ((prop = m_AosAttributes.EnhancePotions) != 0)
                    list.Add(1060411, prop.ToString()); // enhance potions ~1_val~%

                if ((prop = m_AosAttributes.CastRecovery) != 0)
                    list.Add(1060412, prop.ToString()); // faster cast recovery ~1_val~

                if ((prop = m_AosAttributes.CastSpeed) != 0)
                    list.Add(1060413, prop.ToString()); // faster casting ~1_val~

                if ((prop = m_AosAttributes.AttackChance) != 0)
                    list.Add(1060415, prop.ToString()); // hit chance increase ~1_val~%

                if ((prop = m_AosAttributes.BonusHits) != 0)
                    list.Add(1060431, prop.ToString()); // hit point increase ~1_val~

                if ((prop = m_AosAttributes.BonusInt) != 0)
                    list.Add(1060432, prop.ToString()); // intelligence bonus ~1_val~

                if ((prop = m_AosAttributes.LowerManaCost) != 0)
                    list.Add(1060433, prop.ToString()); // lower mana cost ~1_val~%

                if ((prop = m_AosAttributes.LowerRegCost) != 0)
                    list.Add(1060434, prop.ToString()); // lower reagent cost ~1_val~%

                if ((prop = m_AosAttributes.Luck) != 0)
                    list.Add(1060436, prop.ToString()); // luck ~1_val~

                if ((prop = m_AosAttributes.BonusMana) != 0)
                    list.Add(1060439, prop.ToString()); // mana increase ~1_val~

                if ((prop = m_AosAttributes.RegenMana) != 0)
                    list.Add(1060440, prop.ToString()); // mana regeneration ~1_val~

                if ((prop = m_AosAttributes.NightSight) != 0)
                    list.Add(1060441); // night sight

                if ((prop = m_AosAttributes.ReflectPhysical) != 0)
                    list.Add(1060442, prop.ToString()); // reflect physical damage ~1_val~%

                if ((prop = m_AosAttributes.RegenStam) != 0)
                    list.Add(1060443, prop.ToString()); // stamina regeneration ~1_val~

                if ((prop = m_AosAttributes.RegenHits) != 0)
                    list.Add(1060444, prop.ToString()); // hit point regeneration ~1_val~

                if ((prop = m_AosAttributes.SpellChanneling) != 0)
                    list.Add(1060482); // spell channeling

                if ((prop = m_AosAttributes.SpellDamage) != 0)
                    list.Add(1060483, prop.ToString()); // spell damage increase ~1_val~%

                if ((prop = m_AosAttributes.BonusStam) != 0)
                    list.Add(1060484, prop.ToString()); // stamina increase ~1_val~

                if ((prop = m_AosAttributes.BonusStr) != 0)
                    list.Add(1060485, prop.ToString()); // strength bonus ~1_val~

                if ((prop = m_AosAttributes.WeaponSpeed) != 0)
                    list.Add(1060486, prop.ToString()); // swing speed increase ~1_val~%

                if (Core.ML && (prop = m_AosAttributes.IncreasedKarmaLoss) != 0)
                    list.Add(1075210, prop.ToString()); // Increased Karma Loss ~1val~%

                base.AddResistanceProperties(list);
                // Plume : Disponible uniquement via ArmLore
                //if ( m_HitPoints >= 0 && m_MaxHitPoints > 0 )
                //    list.Add( 1060639, "{0}\t{1}", m_HitPoints, m_MaxHitPoints ); // durability ~1_val~ / ~2_val~

            }
            if (!m_Identified)
                list.Add("Non identifié");
            if (m_Crafter != null)
                list.Add(1050043, m_Crafter.Name); // crafted by ~1_NAME~

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)6); // version

            writer.Write((Mobile)m_Crafter);
            writer.Write((bool)m_PlayerConstructed);
            writer.Write((bool)m_Identified);

            writer.WriteEncodedInt((int)m_MaxHitPoints);
            writer.WriteEncodedInt((int)m_HitPoints);

            writer.WriteEncodedInt((int)m_Resource);
            writer.WriteEncodedInt((int)m_GemType);

            m_AosAttributes.Serialize(writer);
            m_AosResistances.Serialize(writer);
            m_AosSkillBonuses.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 6:
                    {
                        m_Crafter = reader.ReadMobile();
                        goto case 5;
                    }
                case 5:
                    {
                        m_PlayerConstructed = reader.ReadBool();
                        goto case 4;
                    }
                case 4:
                    {
                        m_Identified = reader.ReadBool();
                        goto case 3;
                    }
                case 3:
                    {
                        m_MaxHitPoints = reader.ReadEncodedInt();
                        m_HitPoints = reader.ReadEncodedInt();
                        goto case 2;
                    }
                case 2:
                    {
                        m_Resource = (CraftResource)reader.ReadEncodedInt();
                        m_GemType = (GemType)reader.ReadEncodedInt();

                        goto case 1;
                    }
                case 1:
                    {
                        m_AosAttributes = new AosAttributes(this, reader);
                        m_AosResistances = new AosElementAttributes(this, reader);
                        m_AosSkillBonuses = new AosSkillBonuses(this, reader);

                        if (Core.AOS && Parent is Mobile)
                            m_AosSkillBonuses.AddTo((Mobile)Parent);

                        int strBonus = m_AosAttributes.BonusStr;
                        int dexBonus = m_AosAttributes.BonusDex;
                        int intBonus = m_AosAttributes.BonusInt;

                        if (Parent is Mobile && (strBonus != 0 || dexBonus != 0 || intBonus != 0))
                        {
                            Mobile m = (Mobile)Parent;

                            string modName = Serial.ToString();

                            if (strBonus != 0)
                                m.AddStatMod(new StatMod(StatType.Str, modName + "Str", strBonus, TimeSpan.Zero));

                            if (dexBonus != 0)
                                m.AddStatMod(new StatMod(StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero));

                            if (intBonus != 0)
                                m.AddStatMod(new StatMod(StatType.Int, modName + "Int", intBonus, TimeSpan.Zero));
                        }

                        if (Parent is Mobile)
                            ((Mobile)Parent).CheckStatTimers();

                        break;
                    }
                case 0:
                    {
                        m_AosAttributes = new AosAttributes(this);
                        m_AosResistances = new AosElementAttributes(this);
                        m_AosSkillBonuses = new AosSkillBonuses(this);

                        break;
                    }
            }
            if (version < 5)
            {
                m_PlayerConstructed = false; //Assume it wasn't crafted
            }
            if (version < 4)
            {
                m_Identified = true; //Assume it was identified
            }
            if (version < 2)
            {
                m_Resource = CraftResource.MIron;
                m_GemType = GemType.None;
            }
        }
        #region ICraftable Members

        public int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue)
        {

            PlayerConstructed = true;
            Identified = true;

            if (makersMark)
                Crafter = from;

            Type resourceType = typeRes;

            if (resourceType == null)
                resourceType = craftItem.Resources.GetAt(0).ItemType;

            Resource = CraftResources.GetFromType(resourceType);
            
            CraftContext context = craftSystem.GetContext(from);

            if (context != null && context.DoNotColor)
                Hue = 0;
            //Plume Edit pour les gemmes
            /*
			if ( 1 < craftItem.Resources.Count )
			{
				resourceType = craftItem.Resources.GetAt( 1 ).ItemType;

				if ( resourceType == typeof( StarSapphire ) )
					GemType = GemType.StarSapphire;
				else if ( resourceType == typeof( Emerald ) )
					GemType = GemType.Emerald;
				else if ( resourceType == typeof( Sapphire ) )
					GemType = GemType.Sapphire;
				else if ( resourceType == typeof( Ruby ) )
					GemType = GemType.Ruby;
				else if ( resourceType == typeof( Citrine ) )
					GemType = GemType.Citrine;
				else if ( resourceType == typeof( Amethyst ) )
					GemType = GemType.Amethyst;
				else if ( resourceType == typeof( Tourmaline ) )
					GemType = GemType.Tourmaline;
				else if ( resourceType == typeof( Amber ) )
					GemType = GemType.Amber;
				else if ( resourceType == typeof( Diamond ) )
					GemType = GemType.Diamond;
			}
            */
            return 1;

        }

        #endregion
    }
}