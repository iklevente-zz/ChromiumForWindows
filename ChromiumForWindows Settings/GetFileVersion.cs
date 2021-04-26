using System.Text.RegularExpressions;
using System;

namespace ChromiumForWindows_Settings
{
    class GetFileVersion
    {
        public static string finalregexresult = null;
        public static string buildLocalVersion = null;

        public static void GetOldVersionInfo()
        {
            buildLocalVersion =  System.IO.File.ReadAllText(MainWindow.chromiumPath + "\\versioninfo.txt"); // Checks the local version

            //Get the unique GitHub release filename
            string regexpattern = @"v(.*?)-";
            Regex rg = new Regex(regexpattern);

            // This is for downloading
            MatchCollection matched = rg.Matches(buildLocalVersion);
            for (int count = 0; count < matched.Count; count++)
            {
                string regexresult = matched[count].Value.ToString();
                Console.WriteLine(regexresult + " is the the found modification in the downloadable, released GitHub file name.");

                Console.WriteLine("That v and - are causing a mess, let's get rid of them.");
                finalregexresult = regexresult.Trim('v', '-');
                Console.WriteLine("The final regexed version result is: " + finalregexresult);
            }
        }
    }
}
