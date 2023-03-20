using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LostKingdom.GameContent.IO
{
    public static class Preferences
    {
        public static void LoadConfig(string file = "Config")
        {
            string _file = file;

            Stream stream = null;
            StreamReader reader = null;
            JsonTextReader jreader = null;
            if (!Path.HasExtension(file)) _file += ".json";
            try
            {
                if (!File.Exists(_file))
                {
                    Options.ScreenResolutionNum = (byte)(Main.SupportedScreenResolution.Length - 1);
                    Preferences.SaveConfig(file);
                }
                stream = new FileStream(_file, FileMode.Open);
                reader = new StreamReader(stream, Encoding.UTF8);
                jreader = new JsonTextReader(reader);
                void AddPair(Dictionary<string, string> dict, string key, JToken jo)
                {
                    if (jo.HasValues)
                    {
                        JToken[] tjo = jo.ToArray();
                        foreach (JToken child in tjo)
                            AddPair(dict, key, child);
                    }
                    else dict.Add($"{key}.{jo.Parent.Path}".Trim('.'), jo.ToString());
                }
                Dictionary<string, string> data = new Dictionary<string, string>();
                AddPair(data, file, JToken.ReadFrom(jreader));
                string obj = null;
                if (data.TryGetValue(string.Join(".", file, "FPS"), out obj))
                {
                    int fps = int.Parse(obj);
                    if (fps == 60 || fps == 120 || fps == 240) Options.FramePerSecond = fps;
                }
                if (data.TryGetValue(string.Join(".", file, "Language.id"), out obj)) Options.LanguageID = byte.Parse(obj);
                if (data.TryGetValue(string.Join(".", file, "ScreenResolution.id"), out obj)) Options.ScreenResolutionNum = byte.Parse(obj);
                else Options.ScreenResolutionNum = (byte)(Main.SupportedScreenResolution.Length - 1);
                if (data.TryGetValue(string.Join(".", file, "Volume.Master"), out obj)) Options.MasterVolume = float.Parse(obj);
                if (data.TryGetValue(string.Join(".", file, "Volume.Music"), out obj)) Options.MusicVolume = float.Parse(obj);
                if (data.TryGetValue(string.Join(".", file, "Volume.Sound"), out obj)) Options.SoundVolume = float.Parse(obj);
                if (data.TryGetValue(string.Join(".", file, "Volume.Ambient"), out obj)) Options.AmbientVolume = float.Parse(obj);
                reader.Close();
                jreader.Close();
                stream.Close();
            }
            catch (Exception e)
            {
                Main.logger.WrittenByErrorSender($"Invalid File \"{_file}\"");
                Main.logger.WrittenByErrorSender(e.Message);
                reader?.Close();
                jreader?.Close();
                stream?.Close();
                if (File.Exists(_file)) File.Delete(_file);
                Options.ScreenResolutionNum = (byte)(Main.SupportedScreenResolution.Length - 1);
                if (!Main.IgnoreError) throw e;
            }
        }

        public static void SaveConfig(string file = "Config")
        {
            if (!Path.HasExtension(file)) file += ".json";
            FileStream stream = null;
            try
            {
                stream = new FileStream(file, FileMode.Create);
                var data = new
                {
                    FPS = Options.FramePerSecond,
                    Language = Localization.GameCulture.GameCultureInfo[(Localization.GameCulture.CultureInfo)Options.LanguageID],
                    ScreenResolution = new
                    {
                        id = Options.ScreenResolutionNum,
                        width = (int)Main.SupportedScreenResolution[Options.ScreenResolutionNum].X,
                        height = (int)Main.SupportedScreenResolution[Options.ScreenResolutionNum].Y,
                    },
                    Volume = new
                    {
                        Master = Options.MasterVolume,
                        Music = Options.MusicVolume,
                        Sound = Options.SoundVolume,
                        Ambient = Options.AmbientVolume,
                    }
                };
                StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
                writer.Write(JsonConvert.SerializeObject(data, Formatting.Indented));
                writer.Close();
                stream.Close();
            }
            catch (Exception e)
            {
                Main.logger.WrittenByErrorSender($"Invalid File \"{file}\"");
                Main.logger.WrittenByErrorSender(e.Message);
                stream?.Close();
                if (File.Exists(file)) File.Delete(file);
                if (!Main.IgnoreError) throw e;
            }
        }

    }
}