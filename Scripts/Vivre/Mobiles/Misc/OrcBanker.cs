using System;
using Server.Items;

namespace Server.Mobiles
{
    public class OrcBanker : BaseCreature
    {
        [Constructable]
        public OrcBanker()
            : base(AIType.AI_Vendor, FightMode.None, 12, 1, 2, 2)
        {
            Name = NameList.RandomName("orc");
            Title = "L'Orc Sympathique";
            Body = 17;
            BaseSoundID = 0x45A;
            Blessed = true;
            Hue = 2001;
        }

        public OrcBanker(Serial serial)
            : base(serial)
        {
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            if (from is PlayerMobile && from.InRange(this, 12))
                return true;

            return false;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            base.OnSpeech(e);

            string speech = e.Speech.ToLower();

            if (speech.IndexOf("banque") != -1)
                Say("Toi donner pomme, moi amener coffre.");
            else if (speech.IndexOf("orc") != -1)
                Say("Moi entendre toi parler de moi ! *grogne*");
            else if (speech.IndexOf("bonjour") != -1)
                Say("Toi vouloir coffre de banque ?");
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is Apple && from != null)
            {
                dropped.Consume();
                Emote("*dévore la pomme*");
                PlaySound(Utility.Random(0x3A, 3));

                if (dropped.Amount >= 2 || Utility.Random(5) > 0)
                {
                    Say("Voici coffre !");
                    from.BankBox.Open();
                }
                else
                    Say("Moi encore faim !");

                return true;
            }
            else 
                return base.OnDragDrop(from, dropped);
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
