using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LostKingdom.GameContent.Localization
{
    public class GameCulture
    {
        public byte id;
        public string info;

        public enum CultureInfo
        {
            Unknown = 0,
            English = 1,
            Chinese = 2,
        }

        public static readonly Dictionary<CultureInfo, GameCulture> GameCultureInfo = new Dictionary<CultureInfo, GameCulture>()
        {
            {
                CultureInfo.English,
                new GameCulture(1, "en-US")
            },
            {
                CultureInfo.Chinese,
                new GameCulture(2, "zh-Hans")
            },
        };

        public GameCulture(byte id, string info)
        {
            this.id = id;
            this.info = info;
        }
    }
}