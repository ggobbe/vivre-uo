using System;
using Server;
using Server.Network;
using Server.Targeting;
using Server.Mobiles;
using Server.Items;

namespace Server.Commands
{
    public class HasardCmd
    {
        public static void Initialize()
        {
            CommandSystem.Register("Hasard", AccessLevel.Player, new CommandEventHandler(Hasard_OnCommand));
            CommandSystem.Register("PublicHasard", AccessLevel.Player, new CommandEventHandler(PublicHasard_OnCommand));
        }

        [Usage("Hasard <int>")]
        [Description("Check success or fail according to <int>.")]
        public static void Hasard_OnCommand(CommandEventArgs e)
        {
           int dice = 0;
            string numString = e.ArgString.Trim();
            bool IsNumber = int.TryParse(numString, out dice);

            if (!IsNumber){
                e.Mobile.SendMessage("Veuillez entrer une valeur numérique comprise entre 2 et 100");
                return;
            }
               
            else if (dice > 100 || dice < 2){
                e.Mobile.SendMessage("Veuillez entrer une valeur numérique comprise entre 2 et 100");
                return;
            }

            int num1 = Utility.Random(dice);
            int num2 = Utility.Random(dice);

            if (num1 == num2)
                e.Mobile.SendMessage("Vous réussissez votre action");
            else
                e.Mobile.SendMessage("Vous échouez votre action par {1} contre {2} [d{3}]", e.Mobile.Name, num1, num2, dice);
        }

        [Usage("PublicHasard <int>")]
        [Description("Check success or fail according to <int>. Result is seen to everyone")]
        public static void PublicHasard_OnCommand(CommandEventArgs e)
        {
            int dice = 0;
            string numString = e.ArgString.Trim();
            bool IsNumber = int.TryParse(numString, out dice);

            if (!IsNumber){
                e.Mobile.SendMessage("Veuillez entrer une valeur numérique comprise entre 2 et 100");
                return;
            }
               
            else if (dice > 100 || dice < 2){
                e.Mobile.SendMessage("Veuillez entrer une valeur numérique comprise entre 2 et 100");
                return;
            }

            int num1 = Utility.Random(dice);
            int num2 = Utility.Random(dice);

            if (num1 == num2)
            {
                e.Mobile.Emote("{0} a réussi son action [d{1}]", e.Mobile.Name,dice);
                Map map = e.Mobile.Map;

                if (map == null || map == Map.Internal)
                    return;

                Point3D ourLoc = e.Mobile.Location;

			Point3D startLoc = new Point3D( ourLoc.X, ourLoc.Y, ourLoc.Z + 10 );
			Point3D endLoc = new Point3D( startLoc.X + Utility.RandomMinMax( -2, 2 ), startLoc.Y + Utility.RandomMinMax( -2, 2 ), startLoc.Z + 32 );

			Effects.SendMovingEffect( new Entity( Serial.Zero, startLoc, map ), new Entity( Serial.Zero, endLoc, map ),
				0x36E4, 5, 0, false, false );

            Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerStateCallback(FinishLaunch), new object[] { e.Mobile, endLoc, map });
}
                else
                e.Mobile.Emote("{0} a échoué son action par {1} contre {2} [d{3}]", e.Mobile.Name, num1, num2, dice);
        }
        private static void FinishLaunch(object state)
        {
            object[] states = (object[])state;

            Mobile from = (Mobile)states[0];
            Point3D endLoc = (Point3D)states[1];
            Map map = (Map)states[2];

            int hue = Utility.Random(40);

            if (hue < 8)
                hue = 0x66D;
            else if (hue < 10)
                hue = 0x482;
            else if (hue < 12)
                hue = 0x47E;
            else if (hue < 16)
                hue = 0x480;
            else if (hue < 20)
                hue = 0x47F;
            else
                hue = 0;

            if (Utility.RandomBool())
                hue = Utility.RandomList(0x47E, 0x47F, 0x480, 0x482, 0x66D);

            int renderMode = Utility.RandomList(0, 2, 3, 4, 5, 7);

            Effects.PlaySound(endLoc, map, Utility.Random(0x11B, 4));
            Effects.SendLocationEffect(endLoc, map, 0x373A + (0x10 * Utility.Random(4)), 16, 10, hue, renderMode);
        }
    }
}
