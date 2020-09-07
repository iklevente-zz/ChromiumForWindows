using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ChromiumForWindows
{
    class GetFileVersion
    {
        public static string finalregexresult = null;

        public static void GetVersionInfo()
        {
            //Get the unique GitHub release filename
            string regexpattern = @"v(.*?)-";
            Regex rg = new Regex(regexpattern);

            //This is for letting the program know where is the Chrome.exe file to run it. This code will let Chromium to run even if there is no internet and it can't update.
            if (MainWindow.latestVersion == MainWindow.localVersion || MainWindow.latestVersion == "No response from download server")
            {
                MatchCollection matched = rg.Matches(MainWindow.localVersion);
                for (int count = 0; count < matched.Count; count++)
                {
                    Console.WriteLine(matched[count].Value);
                    string regexresult = matched[count].Value.ToString();
                    Output.WriteLine(regexresult + " is the the found modification in the local version's file name.");

                    Output.WriteLine("That v and - are causing a mess, let's get rid of them");
                    finalregexresult = regexresult.Trim('v', '-');
                    Output.WriteLine("The final regexed version result is: " + finalregexresult);
                }
            }

            // This is for downloading
            else if (MainWindow.latestVersion != MainWindow.localVersion)
            {
                MatchCollection matched = rg.Matches(MainWindow.latestVersion);
                for (int count = 0; count < matched.Count; count++)
                {
                    Console.WriteLine(matched[count].Value);
                    string regexresult = matched[count].Value.ToString();
                    Output.WriteLine(regexresult + " is the the found modification in the downloadable, released GitHub file name.");

                    Output.WriteLine("That v and - are causing a mess, let's get rid of them");
                    finalregexresult = regexresult.Trim('v', '-');
                    Output.WriteLine("The final regexed version result is: " + finalregexresult + " Adding it to the download file function...");
                }
            }
        }

        public static void GetOldVersionInfo()
        {
            //This saves the old version info in order to delete the old version in Update.cs, if a new release is out
            //Get the unique GitHub release filename
            string regexpattern = @"v(.*?)-";
            Regex rg = new Regex(regexpattern);

            MatchCollection matched = rg.Matches(MainWindow.localVersion);
            for (int count = 0; count < matched.Count; count++)
            {
                Console.WriteLine(matched[count].Value);
                string regexresult = matched[count].Value.ToString();
                Output.WriteLine(regexresult + " is the the found modification in the downloadable, released GitHub file name.");

                Output.WriteLine("That v and - are causing a mess, let's get rid of them");
                finalregexresult = regexresult.Trim('v', '-');
                Output.WriteLine("The final regexed version result is: " + finalregexresult + " Adding it to the download file function...");
            }
        }


    }
}
