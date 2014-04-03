using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;

namespace Server.Misc
{
    public enum Alignment
    {
        Neutral = 0x0,
        Good = 0x1,
        Evil = 0x2
    }

    public class Alignments : Item, ISerializable
    {
    	// Scriptiz : Pattern singleton (Plume)
        private static Alignments m_Instance;
        public static Alignments Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new Alignments();

                return m_Instance;
            }
        }

        // Don't use this !!! (BBMABOB is cool)
        public static void Reset()
        {
            List<Item> toDelete = new List<Item>();
            foreach (Item i in World.Items.Values)
            {
                if (i is Alignments && i != null)
                {
                    toDelete.Add(i);
                }
            }

            for (int i = 0; i < toDelete.Count; i++)
                toDelete[i].Delete();

            if (m_Instance != null) 
                m_Instance.Delete();

            m_Instance = new Alignments();
        }

        public static void Initialize()
        {
            EventSink.PlayerDeath += new PlayerDeathEventHandler(EventSink_PlayerDeath);
        }

        public static void EventSink_PlayerDeath(PlayerDeathEventArgs e)
        {
            PlayerMobile killed = e.Mobile as PlayerMobile;
            if (killed == null || !(killed is PlayerMobile) || killed.Map != Map.Malas) return;
            PlayerMobile killer = killed.LastKiller as PlayerMobile;
            if (killer == null || killer.Map != Map.Malas || killer.AccessLevel > AccessLevel.Player) return;

            // don't mind about neutral alignment
            if (killed.Alignment != Alignment.Neutral)
            {
                // If killed is not in the same alignment, increase counter
                // otherwise, decrease it
                if (killed.Alignment != killer.Alignment)
                    Alignments.Instance.incKills(killer.Alignment, killed.Player);
                else if (killed.Alignment == killer.Alignment)
                    Alignments.Instance.decKills(killer.Alignment, killed.Player);
            }
        }

        private Dictionary<Alignment, int> m_MobilesKills;
        private Dictionary<Alignment, int> m_PlayersKills;

        public Dictionary<Alignment, int> MobilesKills
        {
            get
            {
                // TODO clone ?
                return m_MobilesKills;
            }
        }

        public Dictionary<Alignment, int> PlayersKills
        {
            get
            {
                // TODO clone ?
                return m_PlayersKills;
            }
        }

        public Alignments()
            : base(0x48E3)
        {
            Name = "Nounours de Scriptiz";
            Weight = 1e3;
            Movable = false;

            m_MobilesKills = new Dictionary<Alignment, int>();
            m_PlayersKills = new Dictionary<Alignment, int>();

            m_Instance = this;
        }

        public Alignments(Serial serial)
            : this()
        {
        }

        public void incKills(Alignment a, bool player)
        {
            if (player)
            {
                if (this.m_PlayersKills.ContainsKey(a))
                    this.m_PlayersKills[a]++;
                else
                    this.m_PlayersKills.Add(a, 1);
            }
            else
            {
                if (this.m_MobilesKills.ContainsKey(a))
                    this.m_MobilesKills[a]++;
                else
                    this.m_MobilesKills.Add(a, 1);
            }
        }

        public void decKills(Alignment a, bool player)
        {
            if (player)
            {
                if (this.m_PlayersKills.ContainsKey(a))
                    this.m_PlayersKills[a]++;
                else
                    this.m_PlayersKills.Add(a, 1);
            }
            else
            {
                if (this.m_MobilesKills.ContainsKey(a))
                    this.m_MobilesKills[a]++;
                else
                    this.m_MobilesKills.Add(a, 1);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);   // version

            // Players kills
            writer.Write(m_PlayersKills.Count);
            foreach (KeyValuePair<Alignment, int> kvp in m_PlayersKills)
            {
                writer.Write((int)kvp.Key);
                writer.Write(kvp.Value);
            }

            // Mobiles kills
            writer.Write(m_MobilesKills.Count);
            foreach (KeyValuePair<Alignment, int> kvp in m_MobilesKills)
            {
                writer.Write((int)kvp.Key);
                writer.Write(kvp.Value);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        // Players kills
                        int playersKills = reader.ReadInt();
                        for (int i = 0; i < playersKills; i++)
                        {
                            Alignment key = (Alignment)reader.ReadInt();
                            int val = reader.ReadInt();
                            m_PlayersKills[key] = val;
                        } 
                        
                        // Mobiles kills
                        int mobilesKills = reader.ReadInt();
                        for (int i = 0; i < mobilesKills; i++)
                        {
                            Alignment key = (Alignment)reader.ReadInt();
                            int val = reader.ReadInt();
                            m_MobilesKills[key] = val;
                        }
                        break;
                    }
            }

            m_Instance = this;
        }
    }
}