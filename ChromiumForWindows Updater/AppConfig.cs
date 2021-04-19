using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace ChromiumForWindows_Updater
{
    public class AppConfig
    {
        // This needs to be the same as CFW Settings AppConfig.cs, except we will not write in the .json file (only if it's missing --> create it: SaveDefaultSettings(); in MainWindow.xaml.cs)
        // If it's not missing it only needs to read the choosen chromiumBuild from the CFW Settings interface

        // The path where to save the settings.json file
        public static string filePath = Path.Combine(MainWindow.chromiumPath, "settings.json");

        // Describe all datas that we need to store
        public string chromiumBuild { get; set; }
        public string appVersion { get; set; }
        public string lastUpdateTime { get; set; }

        // Write default settings
        // This only happens if the settings.json file is missing
        public static void SaveDefaultSettings()
        {
            var defaultSettings = new AppConfig();
            defaultSettings.chromiumBuild = "Hibbiki";
            defaultSettings.appVersion = "2.0";

            // Serialize it
            var serializedObject = Newtonsoft.Json.JsonConvert.SerializeObject(defaultSettings, Newtonsoft.Json.Formatting.Indented);

            // Save to file
            string filePath = Path.Combine(MainWindow.chromiumPath, "settings.json");

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(serializedObject);
                sw.Close();
            }
        }

        // Read settings from json file
        public static string content = null;
        public static void LoadSettings()
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                content = sr.ReadToEnd();
            }
        }
    }
}
