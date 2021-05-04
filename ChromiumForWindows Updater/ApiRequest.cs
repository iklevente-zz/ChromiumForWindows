using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ChromiumForWindows_Updater
{
    public class ApiRequest
    {
        public void GetApiData()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://chromium.woolyss.com/api/v4/");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; rv:78.0) Gecko/20100101 Firefox/78.0";

            string postData = "app=MTkxMDA5";

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(response.CharacterSet));

            string json = sr.ReadToEnd();
            sr.Close();
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Rootobject>(json);

            string installerDownloadLink = result.win64[0].links[0].url;
            //Console.WriteLine(installerDownloadLink);
        }
    }
    public class Rootobject
    {
        public win64[] win64 { get; set; }
    }

    public class win64
    {
        public string tag { get; set; }
        public string editor { get; set; }
        public string channel { get; set; }
        public string version { get; set; }
        public string revision { get; set; }
        public string timestamp { get; set; }
        public links[] links { get; set; }
    }

    public class links
    {
        public string label { get; set; }
        public string url { get; set; }
        public string repository { get; set; }
    }
}
