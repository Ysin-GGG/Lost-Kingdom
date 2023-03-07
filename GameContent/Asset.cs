using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace LostKingdom.GameContent
{
    /// <summary> 游戏资源 </summary>
    public static class Asset
    {
        public static Dictionary<string, SpriteFont> Font { get; private set; }
        public static Dictionary<string, Texture2D> Cursor { get; private set; }
        public static Dictionary<string, Texture2D> Card { get; private set; }
        public static Dictionary<string, Texture2D> Player { get; private set; }
        public static Dictionary<string, Texture2D> Enemy { get; private set; }
        public static Dictionary<string, Texture2D> NPC { get; private set; }
        public static Dictionary<string, Texture2D> Buff { get; private set; }
        public static Dictionary<string, Texture2D> UI { get; private set; }
        public static Dictionary<string, Texture2D> Splash { get; private set; }
        public static Dictionary<string, Texture2D> Image { get; private set; }
        public static Dictionary<string, Effect> Shader { get; private set; }
        public static Dictionary<string, SoundEffect> Sound { get; private set; }

        public static int LoadFonts(string title = null)
        {
            title = title ?? Localization.Localization.GetText("Logger.Title.Font");
            Asset.Font = Asset.Load<SpriteFont>(title, @"Fonts");
            return Asset.Font == null ? -1 : Asset.Font.Count;
        }

        public static int LoadImages(string title = null)
        {
            title = title ?? Localization.Localization.GetText("Logger.Title.Image");
            Asset.Image = Asset.Load<Texture2D>(title, @"Images");
            return Asset.Image == null ? -1 : Asset.Image.Count;
        }

        public static int LoadUIs(string title = null)
        {
            title = title ?? Localization.Localization.GetText("Logger.Title.ImageOfUI");
            Asset.UI = Asset.Load<Texture2D>(title, @"Images\UI");
            return Asset.UI == null ? -1 : Asset.UI.Count;
        }

        public static int LoadCursor(string title = null)
        {
            title = title ?? Localization.Localization.GetText("Logger.Title.ImageOfCursor");
            Asset.Cursor = Asset.Load<Texture2D>(title, @"Images\Cursor");
            return Asset.Cursor == null ? -1 : Asset.Cursor.Count;
        }

        public static int LoadSplashs(string title = null)
        {
            title = title ?? Localization.Localization.GetText("Logger.Title.ImageOfCursor");
            Asset.Splash = Asset.Load<Texture2D>(title, @"Images\Splash");
            return Asset.Splash == null ? -1 : Asset.Splash.Count;
        }

        public static int LoadSounds(string title = null)
        {
            title = title ?? Localization.Localization.GetText("Logger.Title.Sound");
            Asset.Sound = Asset.Load<SoundEffect>(title, @"Sounds");
            return Asset.Sound == null ? -1 : Asset.Sound.Count;
        }

        private static Dictionary<string, T> Load<T>(string title, string child)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine(Main.instance.Content.RootDirectory, child));
            string name = Localization.Localization.GetText("Logger.Sender.GameName");
            if (dirInfo.Exists)
            {
                FileInfo[] files = dirInfo.GetFiles("*.xnb");
                if (files.Length > 0)
                {
                    Dictionary<string, T> dict = new Dictionary<string, T>();
                    Main.logger.Write(Localization.Localization.GetText("Logger.Message.CheckLoadingAssets", title, dirInfo.FullName), name);
                    Main.logger.Write(Localization.Localization.GetText("Logger.Message.StartLoadingAssets", files.Length), name);
                    for (int i = 0; i < files.Length;)
                    {
                        FileInfo file = files[i++];
                        string filename = Path.GetFileNameWithoutExtension(file.Name);
                        try
                        {
                            dict.Add(filename, Main.instance.Content.Load<T>(Path.Combine(child, filename)));
                            Main.logger.Write(Localization.Localization.GetText("Logger.Message.LoadingAssetSucceeded", file.Name, i, files.Length), name);
                        }
                        catch (Exception e)
                        {
                            Main.logger.Write(Localization.Localization.GetText("Logger.Message.LoadingAssetFailed", file.Name, i, files.Length), name);
                            Main.logger.WrittenByErrorSender(e.Message, null);
                        }
                    }
                    Main.logger.Write(Localization.Localization.GetText("Logger.Message.LoadingAssetsSucceeded", dict.Count), name);
                    Main.logger.Write(Localization.Localization.GetText("Logger.Message.StopLoadingAssets", title), name);
                    return dict;
                }
            }
            Main.logger.Write(Localization.Localization.GetText("Logger.Message.LoadingAssetsFailed", title, dirInfo.FullName), name);
            Main.logger.Write(Localization.Localization.GetText("Logger.Message.InvalidAssetPath"), name);
            return null;
        }
    }
}
