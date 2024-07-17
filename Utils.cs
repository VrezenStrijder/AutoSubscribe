using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSubscribe
{
    public class Utils
    {

        public static ClashConfig ClashConfig { get; set; }

        public static T LoadConfigFile<T>(string fileName, string folder = "") where T : class
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "Config", fileName);
            if (!string.IsNullOrEmpty(folder))
            {
                filePath = Path.Combine(Environment.CurrentDirectory, "Config", folder, fileName);
            }

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<T>(json);
            }
            return null;
        }

        public static void SaveConfigFile(string json, string fileName, string newFolderName = "")
        {
            string dir = Path.Combine(Environment.CurrentDirectory, "Config");
            if (!string.IsNullOrEmpty(newFolderName))
            {
                dir = Path.Combine(Environment.CurrentDirectory, "Config", newFolderName);
            }
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string filePath = Path.Combine(dir, fileName);
            if (!File.Exists(filePath))
            {
                FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
                fs.Close();
            }
            File.WriteAllText(filePath, json);
        }


    }
}
