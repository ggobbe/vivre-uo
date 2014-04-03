using System;
using Server;
using Server.Items;
using Server.Regions;
using System.Collections.Generic;
using System.Collections;

namespace Server.Commands
{
    public class DeleteBuccaneerRunes
    {
        public static void Initialize()
        {
            CommandSystem.Register("DeleteNoRecallRunes", AccessLevel.Administrator, new CommandEventHandler(DeleteBuccaneerRunes_OnCommand));
        }

        private static void DeleteBuccaneerRunes_OnCommand(CommandEventArgs e)
        {
            int count1 = 0, count2 = 0;
            foreach (Item i in World.Items.Values)
            {
                // Si c'est une rune, on la rend vierge
                if (i is RecallRune)
                {
                    RecallRune rune = (RecallRune)i;
                    Console.Write("\nRune found : " + rune.Description);

                    if (Region.Find(rune.Target, rune.Map) is NoRecallRegion)
                    {
                        Console.Write(" [cleared]");
                        rune.Target = new Point3D(0, 0, 0);
                        rune.TargetMap = null;
                        rune.Marked = false;
                        rune.House = null;
                        rune.Description = null;
                        count1++;
                    }
                }
                // Si c'est une runebook on doit rentrer dans le livre chercher les entrées
                else if (i is Runebook)
                {
                    Runebook book = (Runebook)i;

                    // On cherche les entrées vers une zone no recall
                    List<RunebookEntry> entries = new List<RunebookEntry>();
                    foreach(RunebookEntry entry in book.Entries)
                    {
                        if(entry == null) continue;
                        Console.Write("\nEntry found : " + entry.Description);
                        if(Region.Find(entry.Location, entry.Map) is NoRecallRegion)
                        {
                            Console.Write(" [deleted]");
                            entries.Add(entry);
                        }
                    }

                    // On supprime les entrées vers les zones no recall
                    foreach (RunebookEntry entry in entries)
                    {
                        book.Entries.Remove(entry);
                        count2++;
                    }
                }
            }
            Console.WriteLine();
            e.Mobile.SendMessage(String.Format("{0} runes effacées et {1} entrées de runebook retirées.", count1, count2));
        }
    }
}
