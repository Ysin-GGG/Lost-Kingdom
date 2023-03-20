using LostKingdom.GameContent;

namespace LostKingdom.GameContent.ID
{
    /// <summary> 卡牌ID </summary>
    public static class CardID
    {

        /// <summary> 无 </summary>
        public const ushort None = 0;
        /// <summary> 唤雷术 </summary>
        public const ushort ConjurationOfLightning = 1;
        /// <summary> 火焰飞弹 </summary>
        public const ushort Fireball = 2;
        /// <summary> 炽热领域 </summary>
        public const ushort Incineration = 3;
        /// <summary> 回溯 </summary>
        public const ushort TimeReversal = 4;
        /// <summary> 静止 </summary>
        public const ushort TimeStop = 5;
        /// <summary> 飞跃 </summary>
        public const ushort TimeJump = 6;
        
        /// <summary> 类型数量 </summary>
        public const int MaxTypes = 7;
        /// <summary> 内部名称 </summary>
        public static readonly string[] InnerName = new string[CardID.MaxTypes]
        {
            "None",
            "ConjurationOfLightning",
            "Fireball",
            "Incineration",
            "TimeReversal",
            "TimeStop",
            "TimeJump",
        };
    }
}