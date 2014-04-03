using System;
using System.Collections.Generic;
using System.Text;
using Server.Items;

namespace Server.Mobiles
{
    public class MortysTheMaster : BaseCreature
    {
        [Constructable]
        public MortysTheMaster()
            : base(AIType.AI_None, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Mortys";
            Title = "Le Maitre";
            Direction = Direction.South;

            Body = 400;
            Hue = 33797;
            Blessed = true;

            InitStats(75, 75, 75);

            SpeechHue = 33;
            EmoteHue = 33;

            Backpack bp = new Backpack();
            bp.Movable = false;
            AddItem(bp);
            AddItem(new RobeACapuche(1109));
            Lantern l = new Lantern();
            l.Hue = 1109;
            AddItem(l);
            l.Ignite();
            Boots b = new Boots();
            b.Hue = 1109;
            AddItem(b);
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            return from.Alive && from.Skills[SkillName.SpiritSpeak].Base >= 40 && from.InRange(this, 3);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (!e.Handled)
            {
                Mobile m = e.Mobile;

                if (m == null || !m.Player) return;

                string speech = e.Speech.ToLower();

                if (speech.IndexOf("maitre") >= 0 || speech.IndexOf("maître") >= 0)
                {
                    if (speech.IndexOf("bonjour") >= 0)
                        Say("*grogne* Que me voulez-vous?");
                    else if (speech.IndexOf("livre") >= 0)
                    {
                        Say("Vous croyez que je vais vous le donner comme ça? *ricanne*");
                        PlaySound(0x246);
                        Timer.DelayCall(TimeSpan.FromSeconds(3), Say, "Apportez-moi 50 os et je verrais ce que je peux en faire.");
                    }
                    else if (speech.IndexOf("os") >= 0)
                        Say("Si vous avez les os, donnez les moi au lieu d'attendre là !");
                    else if (speech.IndexOf("merci") >= 0)
                    {
                        Say("Ne me remerciez pas, ce n'est que le début de vos souffrances !");
                        Timer.DelayCall(TimeSpan.FromSeconds(3), Say, "N'oubliez pas qui vous servez... *retourne à son grimoire*");
                    }
                    else
                        Emote("*reste de marbre*");
                }
                else if (speech.IndexOf("scriptiz") >= 0)
                    Say("Ici c'est moi le maître et non cet hurluberlu qui parle chinois !");
                else
                    Emote("*étudie un grimoire aux pages ravagées sans se soucier de ce qui l'entoure*");

                e.Handled = true;
            }
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is Gold)
            {
                Emote("*prend l'argent et le glisse dans une petite bourse en ricannant*");
                PlaySound(0x246);
                return true;
            }
            else if (dropped is Bone)
            {
                if (dropped.Amount >= 50)
                {
                    if (from.Backpack == null)
                        Say("Et vous le mettrez où votre livre? Revenez quand vous aurez un sac!");
                    else if (from.Backpack.FindItemByType(typeof(NecromancerSpellbook)) != null || (from.FindItemOnLayer(Layer.FirstValid) != null && from.FindItemOnLayer(Layer.FirstValid).GetType() == typeof(NecromancerSpellbook)))
                        Say("Vous avez déjà un livre, déguerpissez !");
                    else if (from.Backpack.FindItemByType(typeof(BookOfBushido)) != null || (from.FindItemOnLayer(Layer.FirstValid) != null && from.FindItemOnLayer(Layer.FirstValid).GetType() == typeof(BookOfBushido)))
                        Say("Vous avez déjà choisi une voie qui n'est pas compatible avec l'art sombre que j'enseigne ici...");
                    else if (from.Backpack.FindItemByType(typeof(BookOfNinjitsu)) != null || (from.FindItemOnLayer(Layer.FirstValid) != null && from.FindItemOnLayer(Layer.FirstValid).GetType() == typeof(BookOfNinjitsu)))
                        Say("Vous avez déjà choisi une voie qui n'est pas compatible avec l'art sombre que j'enseigne ici...");
                    else
                    {
                        Emote("*prend les os et tend un livre sombre en échange*");
                        from.Backpack.AddItem(new NecromancerSpellbook());
                    }
                    return true;
                }
                else
                {
                    Emote("*ricanne*");
                    PlaySound(0x246);
                    Timer.DelayCall(TimeSpan.FromSeconds(2), Say, "Vous me croyez aveugle? Il n'y en a pas assez ! Ramenez le bon nombre la prochaine fois...");
                }
                return true;
            }

            Say("Ne me refilez pas toutes vos crasses !");
            return false;
        }

        public override bool ClickTitle { get { return false; } }

        public MortysTheMaster(Serial serial)
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
