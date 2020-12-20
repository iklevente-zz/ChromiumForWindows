/*using System;
using System.Collections.Generic;*/
using System.Diagnostics;
using System.Net;
//using System.Text;
using System.IO.Compression;

namespace ChromiumForWindows_Settings
{
    class Download
    {
        public static string latestUpdaterVersion = null;

        public static void CheckAvalibility()
        {
            // Checks the version from the release site. It will use the link below, which will redirect to the latest version. string latestVersion will be equal to the redirected URL.
            WebRequest request = WebRequest.Create("https://github.com/iklevente/ChromiumForWindows/releases/latest");
            request.Method = "HEAD"; // Use a HEAD request because we don't need to download the response body
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    latestUpdaterVersion = "https://github.com/iklevente/ChromiumForWindows/releases/latest";
                }
            }
            catch (WebException entry)
            {
                using (WebResponse response = entry.Response)
                {
                    Output.WriteLine("No internet connection or server is down.");
                    return;
                }
            }
        }

        public static void StartHibbikiDownload()
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

            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(latestUpdaterVersion + "/download/hibbiki_raw.zip", MainWindow.chromiumPath + "\\raw.zip"); // https://github.com/Hibbiki/chromium-win64/releases/latest/download/mini_installer.sync.exe was here.
            }
            Output.WriteLine("Choosen Chromium build (hibbiki raw.zip) downloaded. Extracting...");

            string zipPath = MainWindow.chromiumPath + "\\raw.zip";
            string extractPath = MainWindow.chromiumPath;
            ZipFile.ExtractToDirectory(zipPath, extractPath);

            Output.WriteLine("Extraction completed. Deleting raw.zip file");

            System.IO.File.Delete(MainWindow.chromiumPath + "\\raw.zip");
        }

        public static void StartMarmadukeDownload()
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

            using (WebClient webClient = new WebClient()) //praviously WebClient instead of var
            {
                webClient.DownloadFile(latestUpdaterVersion + "/download/marmaduke_raw.zip", MainWindow.chromiumPath + "\\raw.zip"); // https://github.com/Hibbiki/chromium-win64/releases/latest/download/mini_installer.sync.exe was here.
            }
            Output.WriteLine("Choosen Chromium build (marmaduke raw.zip) downloaded. Extracting...");

            string zipPath = MainWindow.chromiumPath + "\\raw.zip";
            string extractPath = MainWindow.chromiumPath;
            ZipFile.ExtractToDirectory(zipPath, extractPath);

            Output.WriteLine("Extraction completed. Deleting raw.zip file");

            System.IO.File.Delete(MainWindow.chromiumPath + "\\raw.zip");
        }
    }
}
