using System.IO;

namespace ChromiumForWindows_Updater
{
    public class AppConfig
    {
        // The path where to save the settings.json file
        public static string filePath = Path.Combine(MainWindow.chromiumPath, "settings.json");

        // Describe all datas that we need to store
        // This needs to be the same as CFW Settings AppConfig.cs
        public string chromiumBuild { get; set; }
        public string localVersion { get; set; }

        // Write default settings
        // This only happens if the settings.json file is missing
        public static void SaveDefaultSettings()
        {
            var defaultSettings = new AppConfig();
            defaultSettings.chromiumBuild = "Hibbiki";
            defaultSettings.localVersion = ApiRequest.latestApiVersion;

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
            var defaultSettings = new AppConfig();
            defaultSettings.chromiumBuild = chromiumBuildJson;
            defaultSettings.localVersion = ApiRequest.latestApiVersion;

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
        public static string chromiumBuildJson = null;
        public static string localVerJson = null;
        public static void LoadSettings()
        {
            StreamReader sr = new StreamReader(filePath);
            string json = sr.ReadToEnd();
            sr.Close();
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Rootobject>(json);
            chromiumBuildJson = result.chromiumBuild;
            localVerJson = result.localVersion;
        }

        public class Rootobject
        {
            public string chromiumBuild { get; set; }
            public string localVersion { get; set; }
        }
    }
}
