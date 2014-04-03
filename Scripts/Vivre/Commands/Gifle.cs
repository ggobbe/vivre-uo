using System;
using System.Text;
using Server.Commands;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands
{
    /// <summary>
    /// Commandes pour donner et prendre des gifles
    /// </summary>
    public class Gifle
    {
        public static void Initialize()
        {
            // Scriptiz : les commandess sont insensibles à la casse
            CommandSystem.Register("gifle", AccessLevel.Player, new CommandEventHandler(Gifle_OnCommand));
            //CommandSystem.Register("slap", AccessLevel.Player, new CommandEventHandler(Gifle_OnCommand));
        }

        public static void Gifle_OnCommand(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(5, false, TargetFlags.None, new TargetCallback(Gifle_Callback));
        }

        /// <summary>
        /// Execution de la gifle
        /// </summary>
        public static void Gifle_Callback(Mobile mJoueur, object objCible)
        {
            if (objCible is Mobile)
            {
                Mobile mCible = objCible as Mobile;

                // si le joueur s'est ciblé lui même
                if (mJoueur == mCible)
                    mJoueur.Emote("*{0} se gifle*", mJoueur.Name);
                // si il reussi a donner la gifle
                else if (Utility.Random(10) > 3)    // Scriptiz : pas besoin de stocker le booléen
                {
                    mJoueur.Emote("*{0} gifle {1}*", mJoueur.Name, mCible.Name);
                    mCible.Emote("*{0} se prend une gifle de {1}*", mCible.Name, mJoueur.Name);
                }
                else // quel looseur il s'est manqué
                {
                    mJoueur.Emote("*{0} gifle {1}, mais ratte*", mJoueur.Name, mCible.Name );
                    mCible.Emote("*{0} esquive une gifle de {1}*", mCible.Name, mJoueur.Name);
                }

                // animation
                mCible.Animate(20, 1, 1, true, false, 2);
                mJoueur.Animate(33, 1, 1, true, false, 2);

                // Scriptiz : son
                mCible.PlaySound(0x135);
            }
        }
    }
}