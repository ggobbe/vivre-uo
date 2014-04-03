//Myron : Second maitre d'arme Bushido
using System;
using System.Collections.Generic;
using System.Text;
using Server.Items;
using Server.Spells.Bushido;

namespace Server.Mobiles
{
    public class BushidoMaster2 : BaseCreature
    {
        private PlayerMobile QuestPlayer;

        [Constructable]
        public BushidoMaster2()
            : base(AIType.AI_None, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Lee";
            Title = ", Le Combattant";
            Direction = Direction.South;

            Body = 400;
            Hue = 33825;
            Blessed = true;

            InitStats(25, 25, 25);
            SetHits(5);

            SpeechHue = 802;
            EmoteHue = 802;

            Backpack bp = new Backpack();
            bp.Movable = false;
            AddItem(bp);
            AddItem(new LeatherDo());
            AddItem(new LeatherHaidate());
            AddItem(new LeatherHiroSode());
            AddItem(new LeatherJingasa());
            Boots b = new Boots();
            b.Hue = 1527;
            AddItem(b);
            this.HairItemID = 8253;
            this.HairHue = 1129;
            QuestPlayer = null;
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            return from.Alive && from.Skills[SkillName.Bushido].Base >= 40 && from.InRange(this, 3);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (!e.Handled)
            {
                Mobile m = e.Mobile;

                if (m == null || !m.Player) return;

                string speech = e.Speech.ToLower();

                if (speech.IndexOf("bonjour") >= 0)
                    Say("Vous arrivez trop tard l'ami...Je suis déjà condamné.");
                else if (speech.IndexOf("condamn") >= 0)
                {
                    Say("Des assassins... ils m'ont empoisonné d'un coup de dague dans le dos! Je n'ai rien pu faire.");
                }
                else if (speech.IndexOf("assassin") >= 0)
                {
                    Say("Une bande de lache sans une once d'honneur. J'aurais mérité une mort plus honorable.");
                }
                else if (speech.IndexOf("honorable execution") >= 0)
                {
                    Say("Vous me feriez cet honneur?");
                    Emote("*se met à genou et baisse la tête*");
                    QuestPlayer = (PlayerMobile)e.Mobile;
                    this.Blessed = false;
                }
                else if (speech.IndexOf("revoir") >= 0)
                {
                    Say("...adieu...");
                    QuestPlayer = null;
                    this.Blessed = true;
                }
                else
                    Emote("Arg...laissez moi donc mourrir en paix...");
            }
            e.Handled = true;
        }
        public override void OnGotMeleeAttack(Mobile attacker)
        {
            if (!(attacker.Backpack.FindItemByType(typeof(BookOfBushido)) != null
               || (attacker.FindItemOnLayer(Layer.FirstValid) != null && attacker.FindItemOnLayer(Layer.FirstValid).GetType() == typeof(BookOfBushido))))
            {
                QuestPlayer = null;
            }
            else
            {
                BookOfBushido rb = (BookOfBushido)attacker.Backpack.FindItemByType(typeof(BookOfBushido));
                if (rb.HasSpell(403)) { QuestPlayer = null; }
            }
            base.OnGotMeleeAttack(attacker);
        }

        public override void OnDeath(Container c)
        {
            if (QuestPlayer != null)
            {
                // On vérifie si le questeur est bien le tueur
                if (QuestPlayer == LastKiller)
                {
                    // DelayCall pour laisser le temps d'appliquer les bonus et tout le tralala pour le GetSwingBonus
                    Timer.DelayCall(TimeSpan.FromSeconds(1), GiveBook);
                }
            }

            base.OnDeath(c);
        }

        private void GiveBook()
        {
            // On vérifie que le questeur à réussi à tuer avec HonorableExecution
            if (QuestPlayer != null && HonorableExecution.GetSwingBonus(QuestPlayer) > 0)
            {
                QuestPlayer.Backpack.AddItem(new CounterAttackScroll());
            }

            QuestPlayer = null;
        }

        public override bool ClickTitle { get { return false; } }

        public BushidoMaster2(Serial serial)
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