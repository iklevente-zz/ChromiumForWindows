using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ChromiumForWindows
{
    class Update
    {
        public static void StartUpdate()
        {
            // Closes all Chromium tasks to be able to update:
            Console.WriteLine("Closing Chromium, if opened...");
            Process[] processes = Process.GetProcessesByName("Chrome");
            foreach (var process in processes)
            {
                process.Kill();
                Console.WriteLine("Chromium process killed!");
            }

            // Deletes old installer if exists:
            if (File.Exists(MainWindow.chromiumPath + "\\mini_installer.sync.exe"))
            {
                System.IO.File.Delete(MainWindow.chromiumPath + "\\mini_installer.sync.exe");
                Console.WriteLine("Deleted old Chromium installer (mini_installer.sync.exe).");
            }

            // Changing version info to the latest one:
            System.IO.File.WriteAllText(MainWindow.chromiumPath + "\\versioninfo.txt", MainWindow.latestVersion);
            Console.WriteLine("There is a new Chromium out there! Updating...");
            Console.WriteLine("Updating versioninfo.txt is done! The program will update Chromium to the latest version now!");

            //Get the unique GitHub release filename
            string regexpattern = @"v(.*?)-";
            Regex rg = new Regex(regexpattern);
            string regexresult = "";

            MatchCollection matched = rg.Matches(MainWindow.latestVersion);
            for (int count = 0; count < matched.Count; count++)
            {
                Console.WriteLine(matched[count].Value);
                regexresult = matched[count].Value.ToString();
                Console.WriteLine(regexresult + "is the the found modification in the downloadable file name.");
            }
            Console.WriteLine("That v and - are causing a mess, let's get rid of them");
            string finalregexresult = regexresult.Trim('v', '-');
            Console.WriteLine("The final regexed version result is: " + finalregexresult + "Adding it to the download file function...");

            // Downloading and updating Chromium to the latest version:
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile("https://github.com/macchrome/winchrome/releases/latest/download/ungoogled-chromium-" + finalregexresult + "-1_windows.7z", MainWindow.chromiumPath + "\\latest_ungoogled_chromium.7z");
            }
            Console.WriteLine("Latest Chromium (latest_ungoogled_chromium.7z) downloaded");

            /*Console.WriteLine("Installing...");
            var miniinstallersync = System.Diagnostics.Process.Start(System.IO.Path.Combine(MainWindow.chromiumPath + "\\mini_installer.sync.exe"));
            miniinstallersync.WaitForExit();
            Console.WriteLine("Installation is done."); */
        }
    }
}
