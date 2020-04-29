using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

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
                System.Threading.Thread.Sleep(3500);
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


            if (Directory.Exists(MainWindow.chromiumPath + "\\Application"))
            {
                Console.WriteLine("Deleting old Chromium...");
                DeleteOldChromium();
                Directory.Delete(MainWindow.chromiumPath + "\\Application");
                Console.WriteLine("Done!");
            }

            // Downloading and updating Chromium to the latest version:
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile("https://github.com/Hibbiki/chromium-win64/releases/latest/download/mini_installer.sync.exe", MainWindow.chromiumPath + "\\mini_installer.sync.exe");
            }
            Console.WriteLine("Latest Chromium installer (mini_installer.sync.exe) downloaded");

            Console.WriteLine("Installing...");
            System.Diagnostics.Process.Start(System.IO.Path.Combine(MainWindow.chromiumPath + "\\mini_installer.sync.exe"));
            System.Threading.Thread.Sleep(7000);
        }

        static void DeleteOldChromium()
        {
            DirectoryInfo chromiumDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Chromium\\Application\\");

            foreach (FileInfo file in chromiumDir.GetFiles())
            {
                System.Threading.Thread.Sleep(5000);
                file.Delete();
            }
            foreach (DirectoryInfo dir in chromiumDir.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}
