//Myron : Premier maitre d'arme Bushido
using System;
using System.Collections.Generic;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    public class BushidoMaster1 : BaseCreature
    {
        private PlayerMobile QuestPlayer;
        [Constructable]
        public BushidoMaster1()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            SetSkill(SkillName.Macing, 50.0, 55.0);
            Name = "Cedric";
            Title = ", Le Combattant";
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
            AddItem(new Robe(2224));
            AddItem(new QuarterStaff());
            AddItem(new RingmailGloves());
            AddItem(new RingmailChest());
            AddItem(new RingmailLegs());
            Boots b = new Boots();
            b.Hue = 1527;
            AddItem(b);
            this.HairItemID = 8253;
            this.HairHue = 1129;
            QuestPlayer = null;
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            return from.Alive && from.Skills[SkillName.Tactics].Base >= 40 && from.InRange(this, 3);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (!e.Handled)
            {
                Mobile m = e.Mobile;

                if (m == null || !m.Player) return;

                string speech = e.Speech.ToLower();

                    if (speech.IndexOf("bonjour") >= 0)
                        Say("Bonjour, Que diriez vous d'un petit duel d'honneur?");
                    else if (speech.IndexOf("duel") >= 0)
                    {
                        Say("En garde alors!");
                        QuestPlayer = (PlayerMobile)e.Mobile;
						this.Blessed = false;
                        this.Attack(e.Mobile);
                        this.Warmode = true;
                    }
                    else if (speech.IndexOf("revoir") >= 0)
                    {
                        Say("Au revoir!");
                    }
                    else if (speech.IndexOf("je renonce au bushido") >= 0)
                    {
                        
                        BookOfBushido rb = (BookOfBushido)e.Mobile.Backpack.FindItemByType(typeof(BookOfBushido));
                        if (rb != null)
                        {
                            Say("Si tel est votre choix.");
                            rb.Delete();
                        }
                        else { Say("Vous ne vous étiez pas encore engagé dans cette foie de toute façon."); }

                    }
                    else
                        Emote("*Ne semble pas réagir*");
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
                Say("Encore un peu et vous y passier!");
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
                            if (!(QuestPlayer.Backpack.FindItemByType(typeof(BookOfBushido)) != null
                                || (QuestPlayer.FindItemOnLayer(Layer.FirstValid) != null && QuestPlayer.FindItemOnLayer(Layer.FirstValid).GetType() == typeof(BookOfBushido))))
                            {
                                Say("Vous vous êtes battu avec Honneur, prenez ce livre et poursuivez dans cette voie.");
                                BookOfBushido b = new BookOfBushido();
                                b.Content = 3;
                                QuestPlayer.Backpack.AddItem(b);
                                if (QuestPlayer.Skills.Bushido.Base < 25.0)
                                {
                                    QuestPlayer.Skills.Bushido.Base = 25.0;
                                }
                            }
                        }
                    }
            }
            Say("C'est toujours agréable de se dégourdir un peu.");
            QuestPlayer.Criminal = false;
            this.Combatant = null;
            this.QuestPlayer = null;
            this.Warmode = false;
            return false;
        }
        public override bool ClickTitle { get { return false; } }

        public BushidoMaster1(Serial serial)
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