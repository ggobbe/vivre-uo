/***************************************************************************
 *                              GenerateCoral.cs
 *                         -----------------------------
 *   begin                : July 22, 2011
 *   copyright            : (C) Scriptiz
 *   email                : maeliguul@hotmail.com
 *   version              : 2011-07-22
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/
using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Commands
{
    public class GenerateCoral
    {
        public static void Initialize()
        {
            CommandSystem.Register("GenerateCoral", AccessLevel.Owner, new CommandEventHandler(GenerateCoral_OnCommand));
            CommandSystem.Register("WipeCoral", AccessLevel.Owner, new CommandEventHandler(WipeCoral_OnCommand));
        }

        private static Map map = Map.Trammel;  // Map to generate corals

        // Limits of the coral barrier
        private static Point2D[] limits = new Point2D[]
            {
                new Point2D(2613,4050),
                new Point2D(2613,2672),
                new Point2D(2285,2672),
                new Point2D(2285,1632),
                new Point2D(5000,1632),
                new Point2D(5000,4050),
            };

        // Type of rocks used for the coral barrier
        private static List<int> rocks = new List<int>
            {
                6001, 6002, 6003, 6004,
                6005, 6006, 6007, 6008,
                6009, 6010, 6011, 6012
            };

        [Usage("GenerateCoral")]
        [Description("Génére des récifs autour des îles afin d'empêcher la navigation en dehors de cette zone.")]
        private static void GenerateCoral_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Generating Corals...");

            int count = 0;
            for(int i = 0; i < limits.Length; i++)
            {
                Point2D actual = limits[i];
                Point2D next = (i == limits.Length - 1 ? limits[0] : limits[i + 1]);

                for (int x = actual.X, y = actual.Y; x != next.X || y != next.Y;)
                {
                    if (x != next.X)
                    {
                        if (x < next.X) x++;
                        else x--;
                    }
                    if (y != next.Y)
                    {
                        if (y < next.Y) y++;
                        else y--;
                    }
                    
                    Static rock = new Static(rocks[Utility.Random(0, rocks.Count)]);
                    Item blocker = new Blocker();
                    Item losBlocker = new LOSBlocker();

                    int z = map.GetAverageZ(x, y);  // on récupère la hauteur du sol à cet endroit
                    rock.MoveToWorld(new Point3D(x, y, z), map);    // le rocher pour la forme
                    blocker.MoveToWorld(new Point3D(x, y, z), map); // un blocker pour empêcher de passer même avec un sea horse par exemple
                    losBlocker.MoveToWorld(new Point3D(x, y, z), map);  // pour empêcher tout sort de téléport de fonctionner

                    count++;
                }
            }
            e.Mobile.SendMessage(String.Format("{0} Corals have been generated !", count));
        }

        [Usage("WipeCoral")]
        [Description("Supprime les récifs autour des îles afin de rétablir la navigation en dehors de cette zone.")]
        private static void WipeCoral_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Wiping Corals...");

            for (int i = 0; i < limits.Length; i++)
            {
                Point2D actual2D = limits[i];
                Point2D next2D = (i == limits.Length - 1 ? limits[0] : limits[i + 1]);

                Point3D actual = new Point3D(actual2D, map.GetAverageZ(actual2D.X, actual2D.Y));
                Point3D next = new Point3D(next2D, map.GetAverageZ(next2D.X, next2D.Y));

                Utility.FixPoints(ref actual, ref next);    // if you miss this, it won't wipe... fucking bitch !

                Wipe.DoWipe(e.Mobile, map, actual, next, Wipe.WipeType.Items);
            }

            e.Mobile.SendMessage("Corals wiped !");
        }
    }
}
