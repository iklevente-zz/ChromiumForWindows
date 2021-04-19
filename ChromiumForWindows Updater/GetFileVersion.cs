using System.Text.RegularExpressions;
using System;

namespace ChromiumForWindows_Updater
{
    class GetFileVersion
    {
        public static string finalregexresult = null;

        public static void GetVersionInfo()
        {
            //Get the unique GitHub release filename
            string regexpattern = @"v(.*?)-";
            Regex rg = new Regex(regexpattern);

            // This is for downloading
            MatchCollection matched = rg.Matches(MainWindow.latestVersion);
            for (int count = 0; count < matched.Count; count++)
            {
                string regexresult = matched[count].Value.ToString();
                Console.WriteLine(regexresult + " is the the found modification in the downloadable, released GitHub file name.");

                Console.WriteLine("That v and - are causing a mess, let's get rid of them.");
                finalregexresult = regexresult.Trim('v', '-');
                Console.WriteLine("The final regexed version result is: " + finalregexresult + " Adding it to the download file function...");
            }
        }
    }
}
