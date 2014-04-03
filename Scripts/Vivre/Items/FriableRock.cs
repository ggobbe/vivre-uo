using System;
using System.Collections.Generic;
using Server.Engines.Harvest;

namespace Server.Items
{
    public class FriableRock : Item
    {
        // Type of rocks used for the coral barrier
        private static List<int> rocks = new List<int>
        {
            // non passables (plus que de passables)
            6002, 6010,
            //6002, 6010,
            //6002, 6010,

            // passables (retirés, si on les rajoute, pensez à mettre plus de non passables
            //6001,     // trop petit et même couleur qu'un des non passables
            //6006 
        };

        private int m_Hits;
        private int m_HitsMax;
        private DateTime lastHit;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Hits
        {
            get { return m_Hits; }
            set
            {
                if (value > HitsMax)
                    m_Hits = HitsMax;

                if (value < 0)
                    return;

                m_Hits = value;

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int HitsMax
        {
            get { return (m_HitsMax > 0 ? m_HitsMax : 1); }
            set
            {
                if (value <= 0)
                    return;

                if (value < Hits)
                    Hits = HitsMax;

                m_HitsMax = value;
                InvalidateProperties();
            }
        }

        [Constructable]
        public FriableRock()
            : this(10)
        {
        }

        [Constructable]
        public FriableRock(int hits)
            : base(6001)
        {
            Name = "Rocher friable";

            HitsMax = hits;
            Hits = hits;

            // ItemID aléatoire
            ItemID = rocks[Utility.Random(0, rocks.Count)];
        }

        // On ajoute le pourcentage de destruction lors du survol
        public override void AddNameProperty(ObjectPropertyList list)
        {
            base.AddNameProperty(list);

            list.Add(String.Format("{0} %", (int)(((double)Hits / HitsMax) * 100)));
        }

        // Pas besoin d'afficher le poids
        public override bool DisplayWeight
        {
            get { return false; }
        }

        // Ne Decay pas !
        public override bool Decays
        {
            get { return false; }
        }

        // Vu qu'on laisse Movable pour avoir le pourcentage de destruction, on interdit de le prendre
        public override bool OnDragLift(Mobile from)
        {
            from.SendMessage("C'est trop lourd ...");
            return false;
        }

        public void OnHit(Mobile from, Item tool)
        {
            // Vérifions que le joueur ne soit pas trop loin ^^
            if(from.GetDistanceToSqrt(this.Location) > 2)
            {
                from.SendMessage("Vous êtes trop loin");
                return;
            }

            // Pour ne pas taper dessus trop vite
            if (lastHit + TimeSpan.FromSeconds(1) > DateTime.Now)
                return;

            lastHit = DateTime.Now;

            // On retire un point
            Hits--;

            // Petite animation
            from.Direction = from.GetDirectionTo(this);
            from.Animate(Utility.RandomList(Mining.System.Definitions[0].EffectActions), 5, 1, true, false, 0);
            from.PlaySound(Utility.RandomList(Mining.System.Definitions[0].EffectSounds));

            // S'il n'y a plus de points on delete le rocher
            if (Hits == 0)
            {
                // Delete après l'animation de minage
                Timer.DelayCall(TimeSpan.FromMilliseconds(900), new TimerCallback(Delete));

                // On réduit de 1 le nombre d'utilisation de l'outil ayant servis
                if (tool is IUsesRemaining)
                {
                    ((IUsesRemaining)tool).UsesRemaining--;
                }

                // Et la récompense tordue :p
                int max = Utility.Random(5);
                int countGem = 0;
                int countOre = 0;
                for (int i = 0; i < max; i++)
                {
                    if (Utility.RandomBool())
                    {
                        countGem++;
                        from.AddToBackpack(Loot.Construct(Loot.GemTypes, Utility.Random(Loot.GemTypes.Length)));
                    }
                    else
                    {
                        if (Utility.RandomBool())
                        {
                            countOre++;
                            int chance = Utility.Random(100);

                            if (chance > 0)
                                from.AddToBackpack(new IronOre());
                            else
                                from.AddToBackpack(new SilverOre());    // environ 1 chance sur 200
                        }
                    }
                }

                if (countGem > 0)
                {
                    from.SendMessage(String.Format("Vous avez trouver {0} gemmes", countGem));

                    if (countOre > 0)
                        from.SendMessage("ainsi qu'un peu de minerai...");
                }
                else
                {
                    if (countOre > 0)
                        from.SendMessage("Vous trouvez un peu de minerai...");
                }
            }
        }

        public FriableRock(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);

            writer.Write(Hits);
            writer.Write(HitsMax);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            Hits = reader.ReadInt();
            HitsMax = reader.ReadInt();
        }
    }
}