using System.Net;
using System.Diagnostics;

namespace ChromiumForWindows_Updater
{
    class Update
    {
        public static void StartUpdate()
        {
            // Closes all Chromium tasks to be able to update:
            Process[] processes = Process.GetProcessesByName("Chrome");
            foreach (var process in processes)
            {
                if (process.MainModule.FileName.StartsWith(MainWindow.chromiumPath))
                {
                    process.Kill();
                }
            }

            // Lots of if statements because of multiple builds!

            // No need to delete old installer if exists because webClient.DownloadFile overwrites it.

            // Changing version info to the latest one:
            System.IO.File.WriteAllText(MainWindow.chromiumPath + "\\versioninfo.txt", MainWindow.latestVersion);

            // Downloading and updating Chromium to the latest version:

            // Downloading
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(MainWindow.latestVersion, MainWindow.chromiumPath + "\\chromium_installer.exe");
            }

            // Installing
            var installer = System.Diagnostics.Process.Start(System.IO.Path.Combine(MainWindow.chromiumPath + "\\chromium_installer.exe"));
            installer.WaitForExit();
        }
    }
}
