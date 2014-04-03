/***************************************************************************
 *                              NecroTeleporter.cs
 *                         -----------------------------
 *   begin                : July 10, 2011
 *   copyright            : (C) Scriptiz
 *   email                : maeliguul@hotmail.com
 *   version              : 2011-07-20
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/
using System;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Items
{
    public class NecroTeleporter : Teleporter
    {
        private string m_Keyword;   // Mot clef
        private int m_Range;    // Range pour la téléportation
        private double m_PlayerNecroRequired;   // montant de nécro requis par joueur
        private double m_PlayerSpiritRequired;  // montant de spirit speak requis par joueur
        private int m_PlayerKarmaRequired;  // karma requis par joueur
        private double m_GroupNecroRequired;    // necro totale du groupe requise
        private int m_NumNecroRequired; // nombre de joueurs nécros requis
        private int m_SpellId1; // sort 1 trouvé dans la salle
        private int m_SpellId2; // sort 2 trouvé dans la salle
        private int m_SpellId3; // sort 3 trouvé dans la salle
        private int m_SpellId4; // sort 4 trouvé dans la salle
        private int m_SpellId5; // sort 5 trouvé dans la salle
        private int m_SpellId6; // sort 6 trouvé dans la salle

        #region Getters & Setters
        [CommandProperty(AccessLevel.Administrator)]
        public string Keyword
        {
            get { return m_Keyword; }
            set { m_Keyword = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public int Range
        {
            get { return m_Range; }
            set { m_Range = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public double PlayerNecroRequired
        {
            get { return m_PlayerNecroRequired; }
            set { m_PlayerNecroRequired = value; }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public double PlayerSpiritRequired
        {
            get { return m_PlayerSpiritRequired; }
            set { m_PlayerSpiritRequired = value; }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public int PlayerKarmaRequired
        {
            get { return m_PlayerKarmaRequired; }
            set { m_PlayerKarmaRequired = value; }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public double GroupNecroRequired
        {
            get { return m_GroupNecroRequired; }
            set { m_GroupNecroRequired = value; }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public int NumNecroRequired
        {
            get { return m_NumNecroRequired; }
            set { m_NumNecroRequired = value; }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public int SpellId1
        {
            get { return m_SpellId1; }
            set { m_SpellId1 = value; }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public int SpellId2
        {
            get { return m_SpellId2; }
            set { m_SpellId2 = value; }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public int SpellId3
        {
            get { return m_SpellId3; }
            set { m_SpellId3 = value; }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public int SpellId4
        {
            get { return m_SpellId4; }
            set { m_SpellId4 = value; }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public int SpellId5
        {
            get { return m_SpellId5; }
            set { m_SpellId5 = value; }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public int SpellId6
        {
            get { return m_SpellId6; }
            set { m_SpellId6 = value; }
        }
        #endregion

        public override bool HandlesOnSpeech { get { return true; } }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (!e.Handled && Active)
            {
                Mobile m = e.Mobile;

                if (!Creatures && !m.Player)
                    return;

                if (!m.InRange(GetWorldLocation(), m_Range))
                    return;

                // Scriptiz : on vérifie si le mot clé est présent
                if (m_Keyword == null) return;
                if (e.Speech.ToLower().IndexOf(m_Keyword.ToLower()) < 0) return;

                // On vérifie que chaque joueur possède bien la nécro requise, le spirit speak requis
                // le karma requis, et que le groupe possède suffisament de nécro
                List<PlayerMobile> necros = new List<PlayerMobile>();
                double totalNecro = 0;
                foreach (Mobile mob in this.GetMobilesInRange(m_Range))
                {
                    PlayerMobile pm = mob as PlayerMobile;
                    if (pm == null) continue;
                    if (pm.AccessLevel > AccessLevel.Player) continue;
                    if (!pm.Alive) continue;

                    // Si un joueur ne possède pas le montant de nécro requis, on annule le transfert
                    Skill sk = m.Skills[SkillName.Necromancy];
                    if (sk == null || sk.Base < m_PlayerNecroRequired)
                    {
                        pm.SendMessage("Votre connaissance des arts nécromants est trop faible pour continuer.");
                        return;
                    }

                    // On ajoute le montant de nécro du joueur au total nécro pour vérif ultérieure
                    totalNecro += sk.Base;

                    // Si un joueur ne possède pas le montant de Spirit Speak requis, on annule le transfert
                    sk = pm.Skills[SkillName.SpiritSpeak];
                    if(sk == null || sk.Base < m_PlayerSpiritRequired)
                    {
                        pm.SendMessage("Votre connaissance du monde des morts est trop faible pour continuer.");
                        return;
                    }

                    // Si le joueur ne possède pas assez de Karma négatif, on annule le transfert
                    if (pm.Karma > m_PlayerKarmaRequired)
                    {
                        pm.SendMessage("Vous n'êtes pas encore assez impur pour continuer.");
                        return;
                    }

                    necros.Add(pm); // On ajoute le PM a la liste des PJ qui se présentent
                }

                // S'il n'y a pas assez de nécro on annule le transfert
                if (necros.Count < m_NumNecroRequired)
                {
                    foreach (PlayerMobile pm in necros)
                        if (pm != null)
                            pm.SendMessage("Vous n'êtes pas assez pour activer cette salle.");

                    return;
                }

                // Si le montant de nécro de l'ensemble des joueurs n'est pas suffisant on annule le transfert
                if (totalNecro < m_GroupNecroRequired)
                {
                    foreach (PlayerMobile pm in necros)
                        if (pm != null) 
                            pm.SendMessage("Vos connaissances des arts nécromants réunies ne parviennent pas à activer cette salle.");
                    
                    return;
                }

                // On regarde si tous les nécros ont les sorts que l'on peut trouver dans la salle
                bool allHaveSpells = true;
                foreach (PlayerMobile pm in necros)
                {
                    if (pm == null) continue;

                    bool haveNecroBook = false;
                    foreach (Item i in pm.Items)
                    {
                        if (i is NecromancerSpellbook)
                        {
                            haveNecroBook = true;
                            NecromancerSpellbook nsb = (NecromancerSpellbook)i;
                            if (m_SpellId1 != -1 && !nsb.HasSpell(m_SpellId1)) allHaveSpells = false;
                            else if (m_SpellId2 != -1 && !nsb.HasSpell(m_SpellId2)) allHaveSpells = false;
                            else if (m_SpellId3 != -1 && !nsb.HasSpell(m_SpellId3)) allHaveSpells = false;
                            else if (m_SpellId4 != -1 && !nsb.HasSpell(m_SpellId4)) allHaveSpells = false;
                            else if (m_SpellId5 != -1 && !nsb.HasSpell(m_SpellId5)) allHaveSpells = false;
                            else if (m_SpellId6 != -1 && !nsb.HasSpell(m_SpellId6)) allHaveSpells = false;
                            if (!allHaveSpells) break;
                        }
                    }
                    if (!haveNecroBook)
                    {
                        pm.SendMessage("Vous devez équiper votre livre afin d'ouvrir le passage.");
                        return;
                    }
                    if (!allHaveSpells) break;
                }

                // S'ils ont tous les sorts que l'on peut trouver, on annule le transfert
                if (allHaveSpells)
                {
                    foreach (PlayerMobile pm in necros)
                        if (pm != null)
                            pm.SendMessage("Vous possédez déjà tous ce que vous pourriez trouver ici.");
                    
                    return;
                }

                // Si tout est bon on téléporte les nécros !
                e.Handled = true;
                foreach(PlayerMobile pm in necros)
                    if (pm != null)
                        StartTeleport(pm);
            }
        }

        public override void StartTeleport(Mobile m)
        {
            // Faire quelque chose avant le téléport
            Effects.SendLocationParticles(EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration), 0x3789, 1, 40, 33, 3, 9907, 0);
            m.PlaySound(0x246);

            base.StartTeleport(m);
        }

        public override void DoTeleport(Mobile m)
        {
            base.DoTeleport(m);

            // Faire quelque chose après le téléport
        }

        public override bool OnMoveOver(Mobile m)
        {
            return true;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            AddNameProperties(list);
            list.Add((Active ? "Active" : "Inactive"));
        }

        [Constructable]
        public NecroTeleporter()
        {
            // On set quelques trucs par défaut sur les minimats
            m_Keyword = null;
            m_Range = 3;
            m_PlayerNecroRequired = 20;
            m_PlayerSpiritRequired = 50;
            m_PlayerKarmaRequired = -1000;
            m_GroupNecroRequired = 50;
            m_NumNecroRequired = 2;
            m_SpellId1 = -1;
            m_SpellId2 = -1;
            m_SpellId3 = -1;
            m_SpellId4 = -1;
            m_SpellId5 = -1;
            m_SpellId6 = -1;
            Delay = TimeSpan.FromMilliseconds(500);
            // Scriptiz : un son bien glauque avant de téléporter
            SoundID = 0x246;
        }

        public NecroTeleporter(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write(m_Keyword);
            writer.Write(m_Range);
            writer.Write(m_PlayerNecroRequired);
            writer.Write(m_PlayerSpiritRequired);
            writer.Write(m_PlayerKarmaRequired);
            writer.Write(m_GroupNecroRequired);
            writer.Write(m_NumNecroRequired);
            writer.Write(m_SpellId1);
            writer.Write(m_SpellId2);
            writer.Write(m_SpellId3);
            writer.Write(m_SpellId4);
            writer.Write(m_SpellId5);
            writer.Write(m_SpellId6);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_Keyword = reader.ReadString();
                        m_Range = reader.ReadInt();
                        m_PlayerNecroRequired = reader.ReadDouble();
                        m_PlayerSpiritRequired = reader.ReadDouble();
                        m_PlayerKarmaRequired = reader.ReadInt();
                        m_GroupNecroRequired = reader.ReadDouble();
                        m_NumNecroRequired = reader.ReadInt();
                        m_SpellId1 = reader.ReadInt();
                        m_SpellId2 = reader.ReadInt();
                        m_SpellId3 = reader.ReadInt();
                        m_SpellId4 = reader.ReadInt();
                        m_SpellId5 = reader.ReadInt();
                        m_SpellId6 = reader.ReadInt();

                        break;
                    }
            }
        }
    }
}
