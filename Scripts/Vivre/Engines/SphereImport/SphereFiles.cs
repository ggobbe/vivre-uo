/***************************************************************************
 *                              SphereFiles.cs
 *                         -----------------------------
 *   begin                : July 6, 2011
 *   copyright            : (C) Scriptiz
 *   email                : maeliguul@hotmail.com
 *   version              : 2011-07-06
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
using Server;
using System.IO;
using System.Collections.Generic;

namespace Server.Misc
{
    class SphereFiles
    {
        // Source
        public static string sphereDir = @"Data\Sphere";
        public static string accountFile = @"Data\Sphere\sphereaccu.scp";
        public static string saveFile = @"Data\Sphere\spherechars.scp";

        // Generated
        public static string playerFile = @"Data\Sphere\sphereplayers.scp";
        public static string hairFile = @"Data\Sphere\spherehair.scp";
        public static string goldFile = @"Data\Sphere\spheregold.scp";
        public static string backFile = @"Data\Sphere\spherebackpack.scp";
        public static string bankFile = @"Data\Sphere\spherebankbox.scp";

        public static bool checkSourceFiles()
        {
            string missing = null;
            if (!Directory.Exists(sphereDir)) missing = sphereDir;
            else if (!File.Exists(accountFile)) missing = accountFile;
            else if (!File.Exists(saveFile)) missing = saveFile;

            if (missing != null) Console.WriteLine("[SphereFiles] This file is missing : " + missing);
            return missing == null;
        }

        public static bool checkDataFiles()
        {
            string missing = null;

            if (!checkSourceFiles()) return false;

            if (!File.Exists(hairFile)) missing = hairFile;
            else if (!File.Exists(goldFile)) missing = goldFile;
            else if (!File.Exists(backFile)) missing = backFile;
            else if (!File.Exists(bankFile)) missing = bankFile;

            if (missing != null)
            {
                if (cutSaveFile()) return true;
                Console.WriteLine("[SphereFiles] This file is missing : " + missing);
            }

            return missing == null;
        }

        public static bool cutSaveFile()
        {
            if (!checkSourceFiles()) return false;

            List<string> hairTypes = new List<string>();

            StreamWriter srPlayer = new StreamWriter(playerFile);
            StreamWriter srHair = new StreamWriter(hairFile);
            StreamWriter srGold = new StreamWriter(goldFile);
            StreamWriter srBack = new StreamWriter(backFile);
            StreamWriter srBank = new StreamWriter(bankFile);

            bool isPlayer = false, isHair = false, isGold = false, isBack = false, isBank = false;

            foreach (string line in File.ReadAllLines(saveFile))
            {
                if (line.StartsWith("["))
                {
                    isPlayer = isHair = isGold = isBack = isBank = false;
                }

                if (line.StartsWith("[WORLDCHAR c_")) isPlayer = true;
                else if (line.StartsWith("[WORLDITEM i_hair_") || line.StartsWith("[WORLDITEM i_beard_")) isHair = true;
                else if (line.StartsWith("[WORLDITEM i_gold]")) isGold = true;
                else if (line.StartsWith("[WORLDITEM i_backpack]")) isBack = true;
                else if (line.StartsWith("[WORLDITEM i_bankbox]")) isBank = true;

                if (isPlayer) srPlayer.WriteLine(line);
                else if (isHair) srHair.WriteLine(line);
                else if (isGold) srGold.WriteLine(line);
                else if (isBack) srBack.WriteLine(line);
                else if (isBank) srBank.WriteLine(line);
            }

            srPlayer.Close();
            srHair.Close();
            srGold.Close();
            srBack.Close();
            srBank.Close();

            return true;
        }
    }
}
