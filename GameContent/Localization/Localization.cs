using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LostKingdom.GameContent.Localization
{
    public static class Localization
    {
        public static byte CurLangId = (byte)GameCulture.CultureInfo.Chinese;
        public static Dictionary<string, string> RawText => Localization.Culture.Keys.Contains(Localization.CurLangId) ? Localization.Culture[Localization.CurLangId] : new Dictionary<string, string>();
        private static Dictionary<byte, Dictionary<string, string>> Culture;

        public static void Initialize()
        {
            Localization.Culture = new Dictionary<byte, Dictionary<string, string>>();
            foreach (var culture in GameCulture.GameCultureInfo.Values) Localization.LoadCulture(culture);
        }

        public static Dictionary<string, string> LoadCulture(GameCulture culture)
        {
            string path = Path.Combine("GameContent\\Localization", culture.info);
            if (!Directory.Exists(path)) return null;
            Dictionary<string, string> dict = new Dictionary<string, string>();
            Queue<DirectoryInfo> queue = new Queue<DirectoryInfo>(30);
            DirectoryInfo curDir = new DirectoryInfo(path);
            while (true)
            {
                FileInfo[] files = curDir.GetFiles("*.json");
                DirectoryInfo[] dirs = curDir.GetDirectories();
                foreach (DirectoryInfo dir in dirs) queue.Enqueue(dir);
                foreach (FileInfo file in files)
                {
                    StreamReader stream = file.OpenText();
                    JsonTextReader reader = new JsonTextReader(stream);
                    JToken token = JToken.ReadFrom(reader);
                    Localization.AddPair(dict, Path.GetFileNameWithoutExtension(file.Name), token);
                }
                if (queue.Count > 0) curDir = queue.Dequeue();
                else break;
            }
            if (!Localization.Culture.Values.Contains(dict)) Localization.Culture.Add(culture.id, dict); 
            else Localization.Culture[culture.id] = dict;
            return dict;
        }

        private static void AddPair(Dictionary<string, string> dict, string key, JToken jo)
        {
            if (jo.HasValues)
            {
                JToken[] tjo = jo.ToArray();
                foreach (JToken child in tjo)
                    AddPair(dict, key, child);
            }
            else dict.Add($"{key}.{jo.Parent.Path}".Trim('.'), jo.ToString());
        }

        public static string GetText(string key, params object[] args)
        {
            if (Localization.RawText == null) return key;
            // 若该key不存在，则放宽限制，查看是否有包含这个key的key
            if (!Localization.RawText.Keys.Contains(key))
            {
                foreach (string tkey in Localization.RawText.Keys)
                    if (tkey.Contains(key)) return Localization.GetText(tkey, args);
                return key;
            }

            int startIndex, endIndex;
            startIndex = Localization.RawText[key].IndexOf("{$");
            if (startIndex >= 0)
            {
                endIndex = Localization.RawText[key].IndexOf("}", startIndex);
                if (endIndex > startIndex)
                {
                    string nkey = Localization.RawText[key].Substring(startIndex, endIndex - startIndex + 1);
                    string nvalue = Localization.GetText(nkey.Substring(2, nkey.Length - 3)); //替换时产生的新args将会保留
                    // 修正调用：以避免反复执行替换
                    Localization.RawText[key] = Localization.RawText[key].Replace(nkey, nvalue);
                }
            }
            return string.Format(Localization.RawText[key], args);
        }
    }
}