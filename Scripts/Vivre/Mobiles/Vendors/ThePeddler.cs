using System;
using System.Collections.Generic;
using Server;
using System.Runtime.CompilerServices;

namespace Server.Mobiles
{
    public class ThePeddler : BaseVendor
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

        private Timer peddlerSpeech;    // le timer pour qu'il parle

        // la chanson du peddler
        public static List<string> Lyrics = new List<string>() { 
            "Moi je viens d'un pays de désert infini,",
            "Où les caravanes rêvent et flânent.",
            "Où, pendant ton sommeil,",
            "Les serpents t'ensorcellent !",
            "C'est bizarre çà ?",
            "Mais, eh, c'est chez moi !",
            "Quand le vent vient de l'Est,",
            "Le soleil est à l'Ouest,",
            "Et s'endort dans les sables d'or...",
            "C'est l'instant envoûtant,",
            "Vole en tapis volant,",
            "Vers la magie des nuits d'Orient !",
            "Oh Nuits d'arabie,",
            "Mille et une folies.",
            "Insomnie d'amour,",
            "Plus chaude à minuit",
            "Qu'au soleil, en plein jour !",
            "Oh Nuits d'arabie,",
            "Au parfum de velours.",
            "Pour le fou qui se perd,",
            "Au coeur du désert,",
            "Fatal est l'amour ! "
        };

        // les phrases du peddler
        public static List<string> Speech = new List<string>() 
        {
            "Aaaah, salam ! Je vous souhaite le bonsoir mon noble ami. Approchez, approchez, venez plus prêt...", 
            "TROP PRET! Un peu trop prêt, voilà.",
            "Bienvenue à Agrabba, cité de la magie noire, des enchantements...",
            "Et les plus belles marchandises du Jourdan en soldes aujourd'hui, profitez-en !",
            "Regardez, oui, un combiné narguilé et cafetière qui fait aussi les pommes de terres frites !",
            "Incassable, incass... cassé.",
            "Ooooooh, regardez, c'est la première fois que j'en vois un aussi bien conservé, c'est le célèbre Tupperware de la Mer Morte, écoutez! *PROUT*",
            "Aaah, il fonctionne, huhu.",
            "Attendez une seconde, je vois que vous vous intéressez qu'aux objets exceptionnellement rare.",
            "Il me semble avoir ici de quoi faire votre bonheur, voyez !",
            "Ne vous laissez pas rebuter par son apparente banalité comme tant d’autres choses ce n’est pas ce qu’il y a à l’extérieur, mais ce qu’il y a à l’intérieur qui compte !",
            "Ce n’est pas n’importe quelle lampe ! Elle a même changé le cours de la vie d’un jeune homme.",
            "Et ce jeune homme, tout comme cette lampe, valait beaucoup plus qu’on ne l’estimait...",
            "Un diamant d’innocence !",
            "Je vous raconte cette histoire ?",
            "Elle commence par une nuit... noire, où l’on découvre un homme en noir, nourrissant de noirs desseins."
        };


        [Constructable]
        public ThePeddler()
            : base("le Colporteur")
        {
            Name = "Ikboul";
            Female = false;
            SpeechHue = 234;
            peddlerSpeech = new PeddlerSpeechTimer(this);
            peddlerSpeech.Start();
        }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBPeddler());
        }

        public ThePeddler(Serial serial)
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

        private class PeddlerSpeechTimer : Timer
        {
            private Mobile m_Peddler;
            private int indexLyrics;
            private int indexSpeech;

            private bool isSinging;
            private bool isSpeaking;

            public PeddlerSpeechTimer(Mobile peddler)
                : base(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10))
            {
                m_Peddler = peddler;
                indexLyrics = indexSpeech = 0;
                isSinging = isSpeaking = false;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            protected override void OnTick()
            {
                base.OnTick();

                bool doSing = false;
                bool doSpeak = false;
                
                foreach (Mobile m in m_Peddler.GetMobilesInRange(12))
                {
                    if (m is PlayerMobile && m != null && !m.Hidden && m != m_Peddler)
                    {
                        if (m_Peddler.GetDistanceToSqrt(m) < 16) doSing = true;

                        if (m_Peddler.GetDistanceToSqrt(m) < 6)
                        {
                            doSing = false;
                            doSpeak = true;
                        }
                    }
                }

                if (!isSinging && !isSpeaking)
                {
                    if (doSing) Sing();
                    else if(doSpeak) Speak();
                }
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            private void Sing()
            {
                // Si la chanson est finie on s'arrête
                if (indexLyrics >= ThePeddler.Lyrics.Count)
                {
                    indexLyrics = 0;
                    isSinging = false;
                    return;
                }
                else
                {
                    // Sinon on vérifie si personne n'est trop prêt
                    bool stop = true;
                    foreach (Mobile m in m_Peddler.GetMobilesInRange(6))
                    {
                        stop = false;
                        if (m != null && m is PlayerMobile)
                        {
                            // Si il y a quelqu'un trop prêt on arrête la chanson et on dit "approchez ..."
                            isSinging = false;
                            isSpeaking = true;
                            indexLyrics = indexSpeech = 0;
                            Speak();
                            return;
                        }
                    }

                    m_Peddler.Say(ThePeddler.Lyrics[indexLyrics++]);
                    if(!stop) Timer.DelayCall(TimeSpan.FromSeconds(8), new TimerCallback(Sing));
                }
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            private void Speak()
            {
                // Si c'est la fin du speech on s'arrête
                if (indexSpeech >= ThePeddler.Speech.Count)
                {
                    indexLyrics = 0;
                    isSpeaking = false;
                    return;
                }
                else
                {
                    // Sinon on vérifie qu'il reste des gens tout prêt
                    bool stop = true;
                    foreach (Mobile m in m_Peddler.GetMobilesInRange(6))
                    {
                        if (m != null && m == m_Peddler) continue;
                        if (m.Hidden || !(m is PlayerMobile)) continue;

                        stop = false;
                        break;
                    }

                    m_Peddler.Say(ThePeddler.Speech[indexSpeech++]);
                    if (!stop) Timer.DelayCall(TimeSpan.FromSeconds(15), new TimerCallback(Speak));
                    else
                    {
                        isSinging = false;
                        isSpeaking = false;
                    }
                }
            }
        }
    }
}