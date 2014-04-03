/***************************************************************************
 *                         SphereAccountsImporter.cs
 *                         -------------------------
 *   begin                : May 25, 2011
 *   copyright            : (C) Scriptiz
 *   email                : maeliguul@hotmail.com
 *   version              : 2011-06-25
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
using System.IO;
using Server.Accounting;
using Server.Misc;

namespace Server.Commands
{
    public class AccountImporter
    {
        public static void Initialize()
        {
            CommandSystem.Register("importSphereAccounts", AccessLevel.Owner, new CommandEventHandler(ImportSphereAccounts_OnCommand));
        }

        [Usage("importSphereAccounts")]
        [Description("Importe les comptes du fichier de sauvegarde sphere")]
        private static void ImportSphereAccounts_OnCommand(CommandEventArgs e)
        {
            // Vérification de l'existence des fichiers nécessaires
            if (!SphereFiles.checkSourceFiles()) return;
            if (!SphereFiles.checkDataFiles()) return;

            // On lit tout le fichier des comptes
            int count = 0;
            Dictionary<string, string> accountData = null;
            foreach (string line in File.ReadAllLines(SphereFiles.accountFile))
            {
                // Si la ligne définit un nouvel enregistrement
                if(line.StartsWith("["))
                {
                    // et que l'on avait déjà des données d'un compte, on l'ajoute
                    if (accountData != null)
                    {
                        // On essaye d'importer le compte avec les données récupérée
                        if(ImportAccount(accountData))
                            count++;
                    }

                    // Ensuite on réinitialise les données du compte et on enregistre le LOGIN du nouveau compte
                    accountData = new Dictionary<string, string>();
                    accountData.Add("LOGIN", line.Substring(1, line.Length - 2));
                }

                // Si la ligne n'est pas vide et contient un =
                if (line.Trim() != "" && line.Contains("="))
                {
                    // On coupe la ligne en deux au niveau du =
                    string[] split = line.Split('=');
                    try
                    {
                        // On ajoute les données récupérées au données du compte
                        accountData.Add(split[0], split[1]);
                    }
                    catch //(ArgumentException ae)
                    {
                    }
                }
            }
            World.Save();   // On fait une save pour sauvegarder les comptes importés
            e.Mobile.SendMessage(count + " accounts imported.");
        }

        private static bool ImportAccount(Dictionary<string, string> accountData)
        {
            try
            {
                // On vérifie s'il y a déjà un compte qui existe avec ce login, si oui on ne fait rien
                IAccount account = Accounts.GetAccount(accountData["LOGIN"]);
                if (account != null) return false;

                // On vérifie que le joueur n'ait pas de plevel pour ne pas importer les comptes GM et autres
                string plevel = null;
                accountData.TryGetValue("PLEVEL", out plevel);
                if (plevel != null) return false;

                // On crée un nouveau compte et on l'ajoute à la liste des comtes
                IAccount newAccount = new Account(accountData["LOGIN"], accountData["PASSWORD"]);
                Accounts.Add(newAccount);
            }
            catch //(KeyNotFoundException knfe)
            {
                // S'il manque des informations comme le login ou le password on n'importe pas le compte
                return false;
            }
            return true;
        }
    }
}