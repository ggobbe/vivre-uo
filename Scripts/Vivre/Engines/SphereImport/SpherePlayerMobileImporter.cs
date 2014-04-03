/***************************************************************************
 *                         SpherePlayerMobileImporter.cs
 *                         -----------------------------
 *   begin                : June 25, 2011
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
using Server.Mobiles;
using Server.Items;
using System.Globalization;

namespace Server.Misc
{
    class SpherePlayerMobileImporter
    {
        public static void Initialize()
        {
            // Appelons l'import lors de la connexion du joueur (avant la liste des persos disponibles)
            EventSink.AccountLogin += new AccountLoginEventHandler(EventSink_AccountLogin);
        }

        public static void EventSink_AccountLogin(AccountLoginEventArgs e)
        {
            try
            {
                IAccount a = Accounts.GetAccount(e.Username);
                if (a == null) return;  // si pas de compte (bug xD) on sort

                Account acc = a as Account;
                if (a == null || a.Count > 0) return;    // Si cast en compte est null ou s'il y a déjà des joueurs pour ce compte, on n'importe pas les joueurs de la save sphere
                else if (acc != null && acc.Comments != null && acc.Comments.Count > 0) return; // pareil s'il y a déjà un commentaire sur le compte
                //else if (acc != null && acc.TotalGameTime > TimeSpan.FromMinutes(15)) return;

                // Vérifions que les fichiers nécessaires existent bien
                if (!SphereFiles.checkDataFiles()) return;

                // Commencons par récupérer les SERIAL des personnages associés au compte
                List<string> charUIDs = new List<string>();
                int totalTime = 0;  // on récupère le temps en sec joué par le joueurs sur sphere
                bool found = false;
                string username = e.Username.ToLower();
                foreach (string line in File.ReadAllLines(SphereFiles.accountFile))
                {
                    // On cherche d'abord à retrouver l'utilisateur
                    if (line.ToLower() == "[" + username + "]")
                    {
                        found = true;
                        continue;
                    }

                    // Ensuite une fois qu'on l'a trouvé on ajout les SERIAL de ses personnages 
                    // jusqu'à tombé sur un autre compte (ou la fin du fichier)
                    if (found)
                    {
                        if (line.StartsWith("[")) break;

                        if (line.StartsWith("TOTALCONNECTTIME="))
                            totalTime = Int32.Parse(line.Split('=')[1]);

                        if (line.StartsWith("CHARUID"))
                            charUIDs.Add(line.Split('=')[1]);
                    }
                }

                // On traite chacun des SERIAL des personnages des joueurs
                foreach (string c in charUIDs)
                {
                    Mobile pm = null;   // Le mobile qui sera créé
                    found = false;      // on remet found à false car on ne l'a pas encore trouvé
                    bool woman = false; // dans le cas on on serait sur une femme il faut s'en souvenir
                    bool hasHouse = false;

                    // On parcourt toutes les lignes du fichier contenant les personnages
                    foreach (string line in File.ReadAllLines(SphereFiles.playerFile))
                    {
                        // Si l'on recontre une femme on s'en souvient pour lui mettre des seins !
                        if (line.StartsWith("[WORLDCHAR"))
                            woman = line.Contains("woman");

                        // Si l'on tombe sur le SERIAL du joueur que l'on cherche on peut passer au traitement
                        if (line.StartsWith("SERIAL="))
                        {
                            if (line.Split('=')[1] == c)
                            {
                                found = true;
                                continue;
                            }
                        }

                        // On est sur le bon joueur, il faut en extraire les informations
                        if (found)
                        {
                            // Si le Mobile est encore null on l'instancie et on l'initialise
                            if (pm == null)
                            {
                                pm = new PlayerMobile();
                                pm.Player = true;   // !!! nécessaire pour lire le paperdoll
                                pm.AccessLevel = AccessLevel.Player;   
                                pm.Map = Map.Internal;   // pour que le joueur soit déconnecté
                                pm.LogoutLocation = new Point3D(3503, 2574, 14);  // endroit de départ des joueurs
                                pm.LogoutMap = Map.Trammel; // map de départ des joueurs
                                pm.BodyValue = (woman ? 401 : 400);
                                pm.Female = woman;
                                pm.SkillsCap = 7000;    // skill cap de 700
                                pm.StatCap = 225;       // stat cap de 225

                                // Ajoutons un backpack au joueur pour qu'il puisse y ranger ses affaires
                                Container pack = pm.Backpack;
                                if (pack == null)
                                {
                                    pack = new Backpack();
                                    pack.Movable = false;
                                    pm.AddItem(pack);
                                }

                                // Une cape pour les vétérans !
                                pm.Backpack.DropItem(new VeteranCloak());
                            }

                            // Si l'on arrive sur un autre joueur on ajoute l'actuel et on continue de chercher
                            // les éventuels autres joueurs du compte
                            if (line.StartsWith("["))
                            {
                                // Effectuons quelques petits trucs avant d'ajouter le joueur sur le compte
                                getHair(c, pm); // on récupère et on remet les cheveux du joueur
                                getGold(c, pm); // on récupère l'or du joueur
                                dressPlayer(pm);    // on habille le joueur

                                // On remet au joueur un cheque s'il avait une maison
                                if (hasHouse)
                                {
                                    Container bank = pm.BankBox;
                                    if (bank != null)
                                    {
                                        BankCheck check = new BankCheck(50000);
                                        check.Name = "Rémunération Maison";
                                        bank.DropItem(check);
                                    }
                                }
                                hasHouse = false;

                                // On remet de l'argent au joueur en fonction du temps joué sur Sphere
                                if (totalTime > 0)
                                {
                                    Container bank = pm.BankBox;
                                    if (bank != null)
                                    {
                                        BankCheck check = new BankCheck((int)(totalTime / (charUIDs.Count * 1.0)));
                                        check.Name = "Temps joué";
                                        bank.DropItem(check);
                                    }
                                }

                                // Et maintenant on ajoute le joueur au compte à la première place libre
                                for (int i = 0; i < a.Length; ++i)
                                {
                                    if (a[i] == null)
                                    {
                                        a[i] = pm;
                                        break;
                                    }
                                }
                                pm = null;
                                break;
                            }

                            #region Traitement des différentes propriétés à importer
                            // Nom
                            if (line.StartsWith("NAME="))
                                pm.Name = line.Split('=')[1];
                            // Body Hue
                            else if (line.StartsWith("COLOR="))
                                pm.Hue = Int32.Parse(line.Split('=')[1], NumberStyles.HexNumber);
                            // Body Hue si corps différent
                            else if (pm.Hue == 0 && line.StartsWith("OSKIN="))
                                pm.Hue = Int32.Parse(line.Split('=')[1], NumberStyles.HexNumber);
                            // Description paperdoll
                            else if (line.StartsWith("PROFILE="))
                                pm.Profile = line.Split('=')[1].Replace("\\r", "\r");
                            // Force
                            else if (line.StartsWith("STR="))
                                pm.Str = Int32.Parse(line.Split('=')[1]);
                            // Int
                            else if (line.StartsWith("INT="))
                                pm.Int = Int32.Parse(line.Split('=')[1]);
                            // Dex
                            else if (line.StartsWith("DEX="))
                                pm.Dex = Int32.Parse(line.Split('=')[1]);
                            // Karma
                            else if (line.StartsWith("KARMA="))
                                pm.Karma = Int32.Parse(line.Split('=')[1]);
                            // Fame
                            else if (line.StartsWith("FAME="))
                                pm.Fame = Int32.Parse(line.Split('=')[1]);
                            // Female
                            else if (line.StartsWith("FAME="))
                                pm.Fame = Int32.Parse(line.Split('=')[1]);
                            // Titre RP
                            else if (line.StartsWith("TITLE="))
                                pm.Title = line.Split('=')[1];
                            // Pour la rémunération de la maison
                            else if (line.StartsWith("HOME="))
                                hasHouse = true;
                            /* Position X,Y,Z
                            else if (line.StartsWith("P="))
                            {
                                string[] location = line.Split('=')[1].Split(',');
                                pm.Location = new Point3D(Int32.Parse(location[0]), Int32.Parse(location[1]), Int32.Parse(location[2]));
                            }
                            */
                            // Alchemy
                            else if (line.StartsWith("Alchemy="))
                                pm.Skills[SkillName.Alchemy].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            // Anatomy
                            else if (line.StartsWith("Anatomy="))
                                pm.Skills[SkillName.Anatomy].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            // Animal Lore
                            else if (line.StartsWith("AnimalLore="))
                                pm.Skills[SkillName.AnimalLore].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Archery
                            else if (line.StartsWith("Archery="))
                                pm.Skills[SkillName.Archery].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            // Arms Lore
                            else if (line.StartsWith("ArmsLore="))
                                pm.Skills[SkillName.ArmsLore].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Begging
                            else if (line.StartsWith("Begging="))
                                pm.Skills[SkillName.Begging].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Blacksmithing
                            else if (line.StartsWith("Blacksmithing="))
                                pm.Skills[SkillName.Blacksmith].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Bowcraft
                            else if (line.StartsWith("Bowcraft="))
                                pm.Skills[SkillName.Fletching].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            // Camping
                            else if (line.StartsWith("Camping="))
                                pm.Skills[SkillName.Camping].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Carpentry
                            else if (line.StartsWith("Carpentry="))
                                pm.Skills[SkillName.Carpentry].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Cartography
                            else if (line.StartsWith("Cartography="))
                                pm.Skills[SkillName.Cartography].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Cooking
                            else if (line.StartsWith("Cooking="))
                                pm.Skills[SkillName.Cooking].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //DetectingHidden
                            else if (line.StartsWith("DetectingHidden="))
                                pm.Skills[SkillName.DetectHidden].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Enticement
                            else if (line.StartsWith("Enticement="))
                                pm.Skills[SkillName.Discordance].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //EvaluatingIntel
                            else if (line.StartsWith("EvaluatingIntel="))
                                pm.Skills[SkillName.EvalInt].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Fencing
                            else if (line.StartsWith("Fencing="))
                                pm.Skills[SkillName.Fencing].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Fishing
                            else if (line.StartsWith("Fishing="))
                                pm.Skills[SkillName.Fishing].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Forensics
                            else if (line.StartsWith("Forensics="))
                                pm.Skills[SkillName.Forensics].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Healing
                            else if (line.StartsWith("Healing="))
                                pm.Skills[SkillName.Healing].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            // Herding
                            else if (line.StartsWith("Herding="))
                                pm.Skills[SkillName.Herding].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Hiding
                            else if (line.StartsWith("Hiding="))
                                pm.Skills[SkillName.Hiding].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Inscription
                            else if (line.StartsWith("Inscription="))
                                pm.Skills[SkillName.Inscribe].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //ItemID
                            else if (line.StartsWith("ItemID="))
                                pm.Skills[SkillName.ItemID].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //LockPicking
                            else if (line.StartsWith("LockPicking="))
                                pm.Skills[SkillName.Lockpicking].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Lumberjacking
                            else if (line.StartsWith("Lumberjacking="))
                                pm.Skills[SkillName.Lumberjacking].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Macefighting
                            else if (line.StartsWith("Macefighting="))
                                pm.Skills[SkillName.Macing].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Magery
                            else if (line.StartsWith("Magery="))
                                pm.Skills[SkillName.Magery].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //MagicResistance
                            else if (line.StartsWith("MagicResistance="))
                                pm.Skills[SkillName.MagicResist].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Meditation
                            else if (line.StartsWith("Meditation="))
                                pm.Skills[SkillName.Meditation].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Mining
                            else if (line.StartsWith("Mining="))
                                pm.Skills[SkillName.Mining].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            // Musicianship
                            else if (line.StartsWith("Musicianship="))
                                pm.Skills[SkillName.Musicianship].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Necromancy
                            else if (line.StartsWith("Necromancy="))
                                pm.Skills[SkillName.Necromancy].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Parrying
                            else if (line.StartsWith("Parrying="))
                                pm.Skills[SkillName.Parry].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Peacemaking
                            else if (line.StartsWith("Peacemaking="))
                                pm.Skills[SkillName.Peacemaking].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Poisoning
                            else if (line.StartsWith("Poisoning="))
                                pm.Skills[SkillName.Poisoning].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Provocation
                            else if (line.StartsWith("Provocation="))
                                pm.Skills[SkillName.Provocation].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //RemoveTrap
                            else if (line.StartsWith("RemoveTrap="))
                                pm.Skills[SkillName.RemoveTrap].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //SpiritSpeak
                            else if (line.StartsWith("SpiritSpeak="))
                                pm.Skills[SkillName.SpiritSpeak].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Stealth
                            else if (line.StartsWith("Stealth="))
                                pm.Skills[SkillName.Stealth].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            // Swordsmanship
                            else if (line.StartsWith("Swordsmanship="))
                                pm.Skills[SkillName.Swords].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Tactics
                            else if (line.StartsWith("Tactics="))
                                pm.Skills[SkillName.Tactics].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Tailoring
                            else if (line.StartsWith("Tailoring="))
                                pm.Skills[SkillName.Tailoring].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Taming
                            else if (line.StartsWith("Taming="))
                                pm.Skills[SkillName.AnimalTaming].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //TasteID
                            else if (line.StartsWith("TasteID="))
                                pm.Skills[SkillName.TasteID].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Tinkering
                            else if (line.StartsWith("Tinkering="))
                                pm.Skills[SkillName.Tinkering].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Tracking
                            else if (line.StartsWith("Tracking="))
                                pm.Skills[SkillName.Tracking].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Snooping
                            else if (line.StartsWith("Snooping="))
                                pm.Skills[SkillName.Snooping].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Stealing
                            else if (line.StartsWith("Stealing="))
                                pm.Skills[SkillName.Stealing].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Veterinary
                            else if (line.StartsWith("Veterinary="))
                                pm.Skills[SkillName.Veterinary].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            //Wrestling
                            else if (line.StartsWith("Wrestling="))
                                pm.Skills[SkillName.Wrestling].BaseFixedPoint = Int32.Parse(line.Split('=')[1]);
                            #endregion

                            // On cap les skills à 100 maxi !
                            for (int i = 0; i < pm.Skills.Length; i++)
                            {
                                Skill skill = pm.Skills[i];
                                if (skill.Base > 100) skill.Base = 100;
                            }
                        }
                    }
                }

                // Ajoutons un commentaire pour indiquer l'importation des personnages à cette date
                if (a.Count > 0 && a is Account)
                    ((Account)a).Comments.Add(new AccountComment("SphereImporter", a.Count + " players imported from Sphere save file."));
            }
            catch (Exception ex)
            {
                Console.WriteLine("SpherePlayerMobileImporter.EventSink_AccountLogin : " + ex.Message);
            }
        }

        private static void getHair(string charUID, Mobile pm)
        {
            // Liste des correspondances pour les cheveux
            Dictionary<string, int> hairTypes = new Dictionary<string, int>();
            hairTypes.Add("[WORLDITEM i_hair_long]", 0x203C);
            hairTypes.Add("[WORLDITEM i_hair_buns]", 0x2046);
            hairTypes.Add("[WORLDITEM i_hair_ponytail]", 0x203D);
            hairTypes.Add("[WORLDITEM i_hair_afro]", 0x2047);
            hairTypes.Add("[WORLDITEM i_hair_short]", 0x203B);
            hairTypes.Add("[WORLDITEM i_hair_receding]", 0x2048);
            hairTypes.Add("[WORLDITEM i_hair_mohawk]", 0x2044);
            hairTypes.Add("[WORLDITEM i_hair_2_pigtails]", 0x2049);
            hairTypes.Add("[WORLDITEM i_hair_pageboy]", 0x2045);
            hairTypes.Add("[WORLDITEM i_hair_krisna]", 0x204A);
            hairTypes.Add("[WORLDITEM i_hair_cutter]", 0);

            // Liste des correspondances pour les barbes
            Dictionary<string, int> beardTypes = new Dictionary<string, int>();
            beardTypes.Add("[WORLDITEM i_beard_long_med]", 0x204C);
            beardTypes.Add("[WORLDITEM i_beard_long]", 0x203E);
            beardTypes.Add("[WORLDITEM i_beard_short]", 0x203F);
            beardTypes.Add("[WORLDITEM i_beard_vandyke]", 0x204D);
            beardTypes.Add("[WORLDITEM i_beard_short_med]", 0x204B);
            beardTypes.Add("[WORLDITEM i_beard_goatee]", 0x2040);
            beardTypes.Add("[WORLDITEM i_beard_mustache]", 0x2041);

            string facialType = null;
            int color = 0;
            foreach (string line in File.ReadAllLines(SphereFiles.hairFile))
            {
                // On réinitialise les données lorsque l'on rencontre un nouvel enregistrement
                if (line.StartsWith("[")) { facialType = null; color = 0; }

                // Si l'on est bien en face d'un enregistrement de cheveux ou de barbe on note duquel il s'agit
                if (line.StartsWith("[WORLDITEM i_hair_") || line.StartsWith("[WORLDITEM i_beard_")) facialType = line;

                // On stocke la couleur lorsqu'on la rencontre
                if (line.StartsWith("COLOR="))
                    color = Int32.Parse(line.Split('=')[1], System.Globalization.NumberStyles.HexNumber);

                if (line.StartsWith("CONT="))
                {
                    // Si l'on tombe sur le champ CONT, on vérifie qu'il s'agisse bien du joueur que l'on est entrain de créé
                    if (line.Split('=')[1] == charUID)
                    {
                        // Si ce sont des cheveux on récupère l'id dans le dico et on set la couleur
                        if (facialType.StartsWith("[WORLDITEM i_hair_"))
                        {
                            pm.HairItemID = hairTypes[facialType];
                            pm.HairHue = color;
                        }
                        // Pareil dans le cas d'une barbe
                        else if (facialType.StartsWith("[WORLDITEM i_beard_"))
                        {
                            pm.FacialHairItemID = beardTypes[facialType];
                            pm.FacialHairHue = color;
                        }
                        continue;
                    }
                }
            }
        }

        private static void getGold(string charUID, Mobile pm)
        {
            int amount = 0; // montants d'or
            int bankboxAmount = 0;
            string backpack = getPlayerItemSerial(charUID, SphereFiles.backFile);   // sac du joueur
            string bankbox = getPlayerItemSerial(charUID, SphereFiles.bankFile);    // banque du joueur

            // On parcourt le fichier qui contient tous les enregistrements d'or
            foreach (string line in File.ReadAllLines(SphereFiles.goldFile))
            {
                if (line.StartsWith("AMOUNT="))
                    amount = Int32.Parse(line.Split('=')[1]);

                if (amount > 0 && line.StartsWith("CONT=") && line.Split('=')[1] == backpack)
                {
                    Item gold = new Gold(amount);
                    Container pack = pm.Backpack;

                    if (pack != null)
                        pack.DropItem(gold);
                    else
                        gold.Delete();

                    amount = 0;
                }
                else if (amount > 0 && line.StartsWith("CONT=") && line.Split('=')[1] == bankbox)
                {
                    bankboxAmount += amount;
                    amount = 0;
                    //Item gold = new Gold(amount);
                    //Container bank = pm.BankBox;

                    //if (bank != null)
                    //    bank.DropItem(gold);
                    //else
                    //    gold.Delete();
                }
            }

            if (bankboxAmount > 0)
            {
                Container bank = pm.BankBox;
                if (bank != null)
                {
                    BankCheck check = new BankCheck(bankboxAmount);
                    check.Name = "Or Banque";
                    bank.DropItem(check);
                }
            }
            bankboxAmount = 0;
        }

        private static string getPlayerItemSerial(string charUID, string file)
        {
            string serial = null;
            foreach (string line in File.ReadAllLines(file))
            {
                if (line.StartsWith("SERIAL="))
                    serial = line.Split('=')[1];

                if (line.StartsWith("CONT=") && line.Split('=')[1] == charUID)
                    return serial;
            }
            return null;
        }

        private static void dressPlayer(Mobile pm)
        {
            if (pm == null) return;

            // Une robe d'une couleur aléatoire
            Item i = new Robe();
            i.Hue = Utility.RandomDyedHue();
            pm.EquipItem(i);

            // Des chaussures
            pm.EquipItem(new Shoes());

            // Quelques objets dans le sac s'il en a un
            if (pm.Backpack != null)
            {
                pm.Backpack.DropItem(new Dagger()); // dague
                pm.Backpack.DropItem(new Candle()); // bougie
                pm.Backpack.DropItem(new RedBook());    // un petit livre rouge pour honoré le président Mao...
            }
        }
    }
}