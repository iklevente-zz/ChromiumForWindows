using System.IO;

namespace ChromiumForWindows_Settings
{
    public class AppConfig
    {
        // The path where to save the settings.json file
        public static string filePath = Path.Combine(MainWindow.chromiumPath, "settings.json");

        // Describe all datas that we need to store
        public string chromiumBuild { get; set; }

        // Write default settings
        // This only happens if the settings.json file is missing
        public static void SaveDefaultSettings()
        {
            var defaultSettings = new AppConfig();
            defaultSettings.chromiumBuild = "Hibbiki";

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

        public static void SaveSettings()
        {
            var settings = new AppConfig();
            settings.chromiumBuild = MainWindow.unsavedChromiumBuild;

            // Serialize it
            var serializedObject = Newtonsoft.Json.JsonConvert.SerializeObject(settings, Newtonsoft.Json.Formatting.Indented);

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
                sr.Close();
            }
        }
    }
}
