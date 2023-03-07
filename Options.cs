using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LostKingdom.Utils;

namespace LostKingdom
{
    public static class Options
    {
        /// <summary> UI缩放倍率 </summary>
        public static float UIScale
        {
            get
            {
                float width = (float)Main.screenWidth / (float)Main.BestScreenWidth;
                float height = (float)Main.screenHeight / (float)Main.BestScreenHeight;
                return (MyMath.Max(width, height) + MyMath.Min(width, height)) / 2f;
            }
        }
        
        /// <summary> 视图大小 </summary>
        public static float ViewportScale { get => MyMath.Max(Main.screenWidth / 1920.0f, Main.screenWidth / 1080.0f); }
        /// <summary> 总音量 </summary>
        public static float TotalVolume { get; set; } = 1f;
        /// <summary> 音乐音量 </summary>
        public static float MusicVolume { get; set; } = 1f;
        /// <summary> 效果音量 </summary>
        public static float EffectVolume { get; set; } = 1f;
        /// <summary> 当前使用的分辨率 </summary>
        public static int ScreenResolutionNum { get; set; } = 1;
        /// <summary> 显示调试信息 </summary>
        public static bool ShowDebugMessage { get; set; } = true;
        /// <summary> 显示玩家设备输入 </summary>
        public static bool ShowPlayerInput { get; set; } = true;
        /// <summary> 鼠标双击最大间隔（60=1000ms=1s） </summary>
        public static int DoubleClickDelay { get => Main.CurrInput.doubleClickDelay; set => Main.CurrInput.doubleClickDelay = value; }
    }
}
