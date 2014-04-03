//Myron : Troisième maitre d'arme Bushido
using System;
using System.Collections.Generic;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    public class BushidoMaster3 : BaseCreature
    {
        private PlayerMobile QuestPlayer;
        [Constructable]
        public BushidoMaster3()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            SetSkill(SkillName.Swords, 80.0, 85.0);
            SetSkill(SkillName.Parry, 100.0, 100.0);
            SetSkill(SkillName.Bushido, 100.0, 100.0);
            Name = "Murray";
            Title = ", Le Pirate Honorable";
            Direction = Direction.South;

            Body = 400;
            Hue = 33825;
            Blessed = true;

            InitStats(100, 100, 25);

            SpeechHue = 802;
            EmoteHue = 802;

            Backpack bp = new Backpack();
            bp.Movable = false;
            AddItem(bp);
            AddItem(new SkullCap(1759));
            AddItem(new Cutlass());
            AddItem(new ShortPants(1759));
            AddItem(new Shirt());
            AddItem(new Shoes(1759));

            QuestPlayer = null;
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            return from.Alive && from.Skills[SkillName.Bushido].Base >= 60 && from.InRange(this, 3);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (!e.Handled)
            {
                Mobile m = e.Mobile;

                if (m == null || !m.Player) return;

                string speech = e.Speech.ToLower();

                    if (speech.IndexOf("bonjour") >= 0)
                        Say("Bonjour moussaillon! Toi aussi tu viens pour la légende?");

                    else if ((speech.IndexOf("légende") >= 0)||(speech.IndexOf("legende") >= 0))
                    {
                        Say("Et ben oui, le pirate qui a esquivé un boulet de 35, du jamais vu!");
                    }
                    else if (speech.IndexOf("boulet") >= 0)
                    {
                        Say("Ca demande de l'entrainement mais c'est faisable tu peux me croire! Si tu as le cran de regarder la mort en face.");
                    }
                    else if ((speech.IndexOf("entrainement") >= 0) || (speech.IndexOf("entrainer") >= 0))
                    {
                        Say("Je pourrais peut être te l'apprendre ouais...Mais va falloir me prouver que t'as du cran.");
                    }
                    else if (speech.IndexOf("cran") >= 0)
                    {
                        Say("Ben viens me montrer que t'en as moussaillon!");
                        QuestPlayer = (PlayerMobile)e.Mobile;
						this.Blessed = false;
                        this.Attack(e.Mobile);
                        this.Warmode = true;
                    }
                    else if (speech.IndexOf("revoir") >= 0)
                    {
                        Say("Au revoir moussaillon!");
                    }
                    else if (speech.IndexOf("je renonce au bushido") >= 0)
                    {
                        
                        BookOfBushido rb = (BookOfBushido)e.Mobile.Backpack.FindItemByType(typeof(BookOfBushido));
                        if (rb != null)
                        {
                            Say("Même les pirates ont de l'honneur et toi tu te débine...tu me dégoute.");
                            rb.Delete();
                        }
                        else { Say("Comme si t'en avais la trempe gamin...C'est un truc d'homme ca."); }

                    }
                    else
                        Emote("J'ai pas de temps à perdre avec ton baratin.");
                }
                e.Handled = true;
            }
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);
            if (defender.Hits < 10) {
                this.Combatant = null;
                QuestPlayer.Criminal = false;
                QuestPlayer = null;
                this.Warmode = false;
                this.Blessed = true;
                Say("Hahaha! Pour le cran faudra repasser. Reviens quand tu seras prêt.");
            }
        }

        public override bool OnBeforeDeath()
        {
            this.Hits = 100;
            this.Blessed = true;
             if (!(QuestPlayer.Backpack.FindItemByType(typeof(NecromancerSpellbook)) != null 
                || (QuestPlayer.FindItemOnLayer(Layer.FirstValid) != null && QuestPlayer.FindItemOnLayer(Layer.FirstValid).GetType() == typeof(NecromancerSpellbook)))){
                    if (!(QuestPlayer.Backpack.FindItemByType(typeof(BookOfNinjitsu)) != null
                       || (QuestPlayer.FindItemOnLayer(Layer.FirstValid) != null && QuestPlayer.FindItemOnLayer(Layer.FirstValid).GetType() == typeof(BookOfNinjitsu))))
                    {
                        if (!(QuestPlayer.Backpack.FindItemByType(typeof(BookOfChivalry)) != null
                            || (QuestPlayer.FindItemOnLayer(Layer.FirstValid) != null && QuestPlayer.FindItemOnLayer(Layer.FirstValid).GetType() == typeof(BookOfChivalry))))
                        {
                            if ((QuestPlayer.Backpack.FindItemByType(typeof(BookOfBushido)) != null
                                || (QuestPlayer.FindItemOnLayer(Layer.FirstValid) != null && QuestPlayer.FindItemOnLayer(Layer.FirstValid).GetType() == typeof(BookOfBushido))))
                            {
                                BookOfBushido b = (BookOfBushido)QuestPlayer.Backpack.FindItemByType(typeof(BookOfBushido));
                                if (b != null) {
                                    if (!(b.HasSpell(402)))
                                    {
                                        Say("J'ai vu ce que je voulais voir. Je vais t'apprendre ce que je sais.");
                                        QuestPlayer.Backpack.AddItem(new EvasionScroll());
                                        QuestPlayer.Backpack.AddItem(new LightningStrikeScroll());
                                    }
                                }
                            }
                        }
                    }
            }
            Say("T'as du cran c'est vrai. Bien joué moussaillon.");
            QuestPlayer.Criminal = false;
            this.Combatant = null;
            this.QuestPlayer = null;
            this.Warmode = false;
            return false;
        }
        public override bool ClickTitle { get { return false; } }

        public BushidoMaster3(Serial serial)
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
    }
}