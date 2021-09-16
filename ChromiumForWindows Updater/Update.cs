using System.Net;
using System.Diagnostics;

namespace ChromiumForWindows_Updater
{
    class Update
    {
        public static int link;
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

            // Choosing file ending to download the proper, choosen release
            string filename = null;
            if (AppConfig.content.Contains("\"chromiumBuild\": \"Hibbiki\""))
            {
                filename = "download/mini_installer.sync.exe";
                link = 0;
            }
            else if (AppConfig.content.Contains("\"chromiumBuild\": \"Hibbiki nosync\""))
            {
                filename = "download/mini_installer.nosync.exe";
                link = 1;
            }
            else if (AppConfig.content.Contains("\"chromiumBuild\": \"Marmaduke\""))
            {
                filename = "download/" + GetFileVersion.finalregexresult + "_ungoogled_mini_installer.exe"; // Already regexed in MainWindow.xaml.cs (GetFileVersion.GetVersionInfo();)
            }

            // Downloading
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(MainWindow.webRequestUrl + filename, MainWindow.chromiumPath + "\\chromium_installer.exe");
                //webClient.DownloadFile(ApiRequest.GetApiData.result)
            }

            // Installing
            var installer = System.Diagnostics.Process.Start(System.IO.Path.Combine(MainWindow.chromiumPath + "\\chromium_installer.exe"));
            installer.WaitForExit();
        }
    }
}
