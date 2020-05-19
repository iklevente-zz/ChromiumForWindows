using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using IWshRuntimeLibrary;

namespace ChromiumForWindows
{
    class FixShortcut
    {
        public static void MakeShortcuts()
        {
            DesktopShortcut();
            StartMenuShortcut();
        }

        // Delete Chromium desktop shortcut and replace with the updater's shortcut:
        private static void DesktopShortcut()
        {
            // Delete it
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            System.IO.File.Delete(Path.Combine(desktopPath, "Chromium.lnk"));

            // Make new shortcut: 
            object shDesktop = (object)"Desktop";
            WshShell shell = new WshShell();
            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\Chromium.lnk";
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.Description = "Open Chromium";
            shortcut.TargetPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Chromium\\ChromiumLauncher.exe";
            shortcut.IconLocation = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Chromium\\Application\\chrome.exe";
            shortcut.Save();
        }

        // Delete Chromium Start Menu shortcut and replace with the updater's shortcut:
        private static void StartMenuShortcut()
        {
            // Delete it
            string startMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Windows\\Start Menu\\Programs\\";
            System.IO.File.Delete(Path.Combine(startMenuPath, "Chromium.lnk"));

            // Make new shortcut:
            string pathToExe = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Chromium\\ChromiumLauncher.exe";
            string StartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
            string appStartMenuPath = Path.Combine(StartMenuPath, "Programs");

            if (!Directory.Exists(appStartMenuPath))
            {
                Directory.CreateDirectory(appStartMenuPath);
            }

            string shortcutLocation = Path.Combine(appStartMenuPath, "Chromium" + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = "Open Chromium";
            shortcut.IconLocation = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Chromium\\Application\\chrome.exe";
            shortcut.TargetPath = pathToExe;
            shortcut.Save();
        }
    }
}
