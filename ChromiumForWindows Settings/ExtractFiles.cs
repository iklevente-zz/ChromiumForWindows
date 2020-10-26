using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace ChromiumForWindows_Settings
{
    class ExtractFiles
    {
        public static void Extract(string nameSpace, string outDirectory, string internalFilePath, string resourceName)
        {
            Assembly assembly = Assembly.GetCallingAssembly();

            using (Stream s = assembly.GetManifestResourceStream(nameSpace + "." + (internalFilePath == "" ? "" : internalFilePath + ".") + resourceName))
            {
                using (BinaryReader r = new BinaryReader(s))
                {
                    using (FileStream fs = new FileStream(outDirectory + "\\" + resourceName, FileMode.OpenOrCreate))
                    {
                        using (BinaryWriter w = new BinaryWriter(fs))
                        {
                            w.Write(r.ReadBytes((int)s.Length));
                        }
                    }
                }
            }
        }
    }
}