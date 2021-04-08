﻿using System.IO;
using System.Net;
using System.Diagnostics;

namespace ChromiumForWindows
{
    class Update
    {
        public static void StartUpdate()
        {
            // Closes all Chromium tasks to be able to update:
            Output.WriteLine("Closing Chromium, if opened...");
            Process[] processes = Process.GetProcessesByName("Chrome");
            foreach (var process in processes)
            {
                if (process.MainModule.FileName.StartsWith(MainWindow.chromiumPath))
                {
                    process.Kill();
                    Output.WriteLine("Chromium process killed!");
                }
            }


            // Deletes old installer if exists:
            if (File.Exists(MainWindow.chromiumPath + "\\ungoogled_mini_installer.exe"))
            {
                System.IO.File.Delete(MainWindow.chromiumPath + "\\ungoogled_mini_installer.exe");
                Output.WriteLine("Deleted old Chromium installer (ungoogled_mini_installer.exe).");
            }

            // Changing version info to the latest one:
            System.IO.File.WriteAllText(MainWindow.chromiumPath + "\\versioninfo.txt", MainWindow.latestVersion);
            Output.WriteLine("There is a new Chromium out there! Updating...");
            Output.WriteLine("Updating versioninfo.txt is done! The program will update Chromium to the latest version now!");

            // Getting version info from GitHub
            GetFileVersion.GetVersionInfo();

            // Downloading and updating Chromium to the latest version:
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile("https://github.com/macchrome/winchrome/releases/latest/download/" + GetFileVersion.finalregexresult + "_ungoogled_mini_installer.exe", MainWindow.chromiumPath + "\\ungoogled_mini_installer.exe"); ;
            }
            Output.WriteLine("Latest Chromium installer (ungoogled_mini_installer.exe) downloaded");

            Output.WriteLine("Installing...");
            var miniinstallersync = System.Diagnostics.Process.Start(System.IO.Path.Combine(MainWindow.chromiumPath + "\\ungoogled_mini_installer.exe"));
            miniinstallersync.WaitForExit();
            Output.WriteLine("Installation is done.");
        }
    }
}
