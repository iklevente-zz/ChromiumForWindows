using System;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ChromiumForWindows
{
    class Program
    {
        static string chromiumPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Chromium";

        static void Main(string[] args)
        {
            // If it's already updating in the background, don't allow running it again.
            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            Console.WriteLine("The program has started!");

            CreateChromiumDir();
            GetLatestVersion();
            StartChromium();
        }

        static void CreateChromiumDir()
        {
            string path = chromiumPath;

            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path))
                {
                    Console.WriteLine("That path exists already.");
                    return;
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally { }
        }

        static void GetLatestVersion()
        {
            if (System.IO.File.Exists(chromiumPath + "\\versioninfo.log"))
            {
                Console.WriteLine("versioninfo.log is already exists!");
            }
            else
            {
                System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Chromium\\versioninfo.log", "version");
            }

            string localVersion = System.IO.File.ReadAllText(chromiumPath + "\\versioninfo.log");
            Console.WriteLine(localVersion + " is the current local version.");

            // Checks the version from the website
            System.Net.WebClient versionInfo = new System.Net.WebClient();
            byte[] raw = versionInfo.DownloadData("https://download-chromium.appspot.com/rev/Win_x64?type=snapshots");

            string latestVersion = System.Text.Encoding.UTF8.GetString(raw);
            Console.WriteLine(latestVersion + " is the current latest version.");

            if (localVersion != latestVersion)
            {
                System.IO.File.WriteAllText(chromiumPath + "\\versioninfo.log", latestVersion);
                Console.WriteLine("There is a new Chromium out there! Updating...");
                Console.WriteLine("Updating versioninfo.log is done! The program will update Chromium to the latest version now!");

                if (Directory.Exists(chromiumPath + "\\chrome-win"))
                {
                    Console.WriteLine("Deleting old Chromium...");
                    DeleteOldChromium();
                    Directory.Delete(chromiumPath + "\\chrome-win");
                    Console.WriteLine("Done!");
                }

                UpdateChromium();
            }
            else
            {
                Console.WriteLine("Local version is the same as latest version!");
                Console.WriteLine("Exiting!");
                return;
            }
        }

        static void DeleteOldChromium()
        {
            DirectoryInfo chromiumDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Chromium\\chrome-win\\");

            foreach (FileInfo file in chromiumDir.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in chromiumDir.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        static void UpdateChromium()
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile("https://download-chromium.appspot.com/dl/Win_x64?type=snapshots", chromiumPath + "\\latestchromium.zip");
            }
            Console.WriteLine("latestchromium.zip downloaded");

            ZipFile.ExtractToDirectory(chromiumPath + "\\latestchromium.zip", chromiumPath);
            Console.WriteLine("Chromium has been extracted.");

            System.IO.File.Delete(chromiumPath + "\\latestchromium.zip");
            Console.WriteLine("Deleted latestchromium.zip");
        }

        static void StartChromium()
        {
            System.Diagnostics.Process.Start(System.IO.Path.Combine(chromiumPath + "\\chrome-win\\chrome.exe"));
        }
    }
}
