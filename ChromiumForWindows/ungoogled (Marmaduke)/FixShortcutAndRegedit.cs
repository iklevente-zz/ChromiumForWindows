using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using IWshRuntimeLibrary;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace ChromiumForWindows
{
    class FixShortcutAndRegedit
    {
        public static void MakeShortcutsAndDelReg()
        {
            DesktopShortcut();
            StartMenuShortcut();
            DelReg();
        }

        // Delete Chromium desktop shortcut and replace with the updater's shortcut:
        private static void DesktopShortcut()
        {
            // Delete it
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            System.IO.File.Delete(Path.Combine(desktopPath, "Chromium.lnk"));

            // Make new shortcut: 
            Output.WriteLine("Creating Desktop Shortcut");
            object shDesktop = (object)"Desktop";
            WshShell shell = new WshShell();
            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\Chromium.lnk";
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.Description = "Open Chromium";
            shortcut.TargetPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Chromium\\ChromiumLauncher.exe";
            shortcut.IconLocation = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Chromium\\ungoogled-chromium-" + GetFileVersion.finalregexresult + "-1_Win64\\chrome.exe";
            shortcut.Save();
            Output.WriteLine("Desktop Shortcut done");
        }

        // Delete Chromium Start Menu shortcut and replace with the updater's shortcut:
        private static void StartMenuShortcut()
        {
            // Delete old
            string startMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Windows\\Start Menu\\Programs\\";
            System.IO.File.Delete(Path.Combine(startMenuPath, "Chromium.lnk"));

            Output.WriteLine("Creating Start Menu Shortcuts");
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
            shortcut.IconLocation = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Chromium\\ungoogled-chromium-" + GetFileVersion.finalregexresult + "-1_Win64\\chrome.exe";
            shortcut.TargetPath = pathToExe;
            shortcut.Save();
            Output.WriteLine("Start Menu Shortcut done");
        }

        public static void DeleteRegistryFolder(RegistryHive registryHive, string fullPathKeyToDelete)
        {

            using (var baseKey = RegistryKey.OpenBaseKey(registryHive, RegistryView.Registry64))
            {
                baseKey.DeleteSubKeyTree(fullPathKeyToDelete);
            }
        }

        static void DelReg()
        {
            Output.WriteLine("Deleting Chromium registry values in order to fix the default application problem...");

            var baseKeyString = $@"SOFTWARE\Classes\Chromium*";

            DeleteRegistryFolder(RegistryHive.CurrentUser, baseKeyString);

            // Does not work currently
        }
    }
}
