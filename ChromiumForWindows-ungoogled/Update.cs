using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;

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
                if (process.MainModule.FileName.StartsWith(MainWindow.chromiumPath))
                {
                    process.Kill();
                    Console.WriteLine("Chromium process killed!");
                }
            }

            // Deletes old installer if exists:
            if (File.Exists(MainWindow.chromiumPath + "\\latest_ungoogled_chromium.7z"))
            {
                System.IO.File.Delete(MainWindow.chromiumPath + "\\latest_ungoogled_chromium.7z");
                Console.WriteLine("Deleted old Chromium installer (latest_ungoogled_chromium.7z).");
            }

            // Deletes old Chromium directory if exists: (To not leave multiple Chromium installations) // Would be cool to make it easier
            GetFileVersion.GetOldVersionInfo();
            if (Directory.Exists(MainWindow.chromiumPath + "\\ungoogled-chromium-" + GetFileVersion.finalregexresult + "-1_windows"))
            {
                System.IO.Directory.Delete(MainWindow.chromiumPath + "\\ungoogled-chromium-" + GetFileVersion.finalregexresult + "-1_windows", true);
                Console.WriteLine("Deleted old Chromium folder.");
            }

            //Get the unique GitHub release filename
            GetFileVersion.GetVersionInfo();

            // Changing version info to the latest one:
            System.IO.File.WriteAllText(MainWindow.chromiumPath + "\\versioninfo.txt", MainWindow.latestVersion);
            Console.WriteLine("There is a new Chromium out there! Updating...");
            Console.WriteLine("Updating versioninfo.txt is done! The program will update Chromium to the latest version now!");

            // Downloading and updating Chromium to the latest version:
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile("https://github.com/macchrome/winchrome/releases/latest/download/ungoogled-chromium-" + GetFileVersion.finalregexresult + "-1_windows.7z", MainWindow.chromiumPath + "\\latest_ungoogled_chromium.7z");
            }
            Console.WriteLine("Latest Chromium (latest_ungoogled_chromium.7z) downloaded");

            // Extract new ungoogled Chromium
            Console.WriteLine("Installing...");
            string zPath = "Resources\\7-Zip x64\\7za.exe"; //add to proj and set CopyToOuputDir
            string sourceArchive = MainWindow.chromiumPath + "\\latest_ungoogled_chromium.7z";
            string destination = MainWindow.chromiumPath;
            try
            {
                ProcessStartInfo pro = new ProcessStartInfo();
                pro.WindowStyle = ProcessWindowStyle.Hidden;
                pro.FileName = zPath;
                pro.Arguments = string.Format("x \"{0}\" -y -o\"{1}\"", sourceArchive, destination);
                Process x = Process.Start(pro);
                x.WaitForExit();
            }
            catch (System.Exception)
            {
                //handle error
                MessageBox.Show("There was an error while extracting the latest Chromium.");
            }
            Console.WriteLine("Installation is done.");
        }
    }
}
