using System;
using System.IO;

namespace Server.Misc
{
    public class Logging
    {
        public static void DebugLog(string text)
        {
            using (StreamWriter sw = new StreamWriter("debug.log", true))
            {
                sw.WriteLine(String.Format("{0} : {1}", DateTime.Now, text));
                Console.WriteLine("[Debug] : " + text);
            }
        }

        public static void RestartLog(string text)
        {
            using (StreamWriter sw = new StreamWriter("restart.log", true))
            {
                sw.WriteLine(String.Format("{0} : {1}", DateTime.Now, text));
            }
        }
    }
}