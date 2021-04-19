using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace ChromiumForWindows_Updater
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

            // Lots of if statements because of multiple builds!

            // Deletes old installer if exists:
            // Hibbiki build
            if (File.Exists(MainWindow.chromiumPath + "\\mini_installer.sync_Hibbiki.exe"))
            {
                System.IO.File.Delete(MainWindow.chromiumPath + "\\mini_installer.sync_Hibbiki.exe");
                Output.WriteLine("Deleted old Chromium installer (mini_installer.sync_Hibbiki.exe).");
            }
            // Marmaduke build
            if (File.Exists(MainWindow.chromiumPath + "\\ungoogled_mini_installer_Marmaduke.exe"))
            {
                System.IO.File.Delete(MainWindow.chromiumPath + "\\ungoogled_mini_installer_Marmaduke.exe");
                Output.WriteLine("Deleted old Chromium installer (ungoogled_mini_installer_Marmaduke.exe).");
            }

            // Changing version info to the latest one:
            Output.WriteLine("There is a new Chromium out there! Updating...");
            System.IO.File.WriteAllText(MainWindow.chromiumPath + "\\versioninfo.txt", MainWindow.latestVersion);
            Output.WriteLine("Updating versioninfo.txt is done! The program will update Chromium to the latest version now!");

            // Huge if statements because of multiple builds!
            // Downloading and updating Chromium to the latest version:
            // iklevente build
            // Hibbiki build
            if (AppConfig.content.Contains("\"chromiumBuild\": \"Hibbiki\""))
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(MainWindow.webRequestUrl + "download/mini_installer.sync.exe", MainWindow.chromiumPath + "\\mini_installer.sync_Hibbiki.exe");
                }
                Output.WriteLine("Latest Chromium installer (mini_installer.sync_Hibbiki.exe) downloaded");

                // Installing
                Output.WriteLine("Installing...");
                var miniinstallersync = System.Diagnostics.Process.Start(System.IO.Path.Combine(MainWindow.chromiumPath + "\\mini_installer.sync_Hibbiki.exe"));
                miniinstallersync.WaitForExit();
                Output.WriteLine("Installation is done.");
            }
            // Marmaduke build
            else if (AppConfig.content.Contains("\"chromiumBuild\": \"Marmaduke\""))
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(MainWindow.webRequestUrl + "download/" + GetFileVersion.finalregexresult + "_ungoogled_mini_installer.exe", MainWindow.chromiumPath + "\\ungoogled_mini_installer_Marmaduke.exe"); ;
                }
                Output.WriteLine("Latest Chromium installer (ungoogled_mini_installer_Marmaduke.exe) downloaded");

                // Installing
                Output.WriteLine("Installing...");
                var miniinstallersync = System.Diagnostics.Process.Start(System.IO.Path.Combine(MainWindow.chromiumPath + "\\ungoogled_mini_installer_Marmaduke.exe"));
                miniinstallersync.WaitForExit();
                Output.WriteLine("Installation is done.");
            }
        }
    }
}
