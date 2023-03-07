using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LostKingdom.GameContent
{
    public class Card
    {
        public string name;
        public string desc;
        public ushort type;
        public ushort hp;
        public ushort ap;
        public bool exhaust;
        public bool ethereal;
        public bool unusable;
        public CardRare rare;
        public CardStyle style;
        public CardState state;
        public uint damage;
        public uint damageCount;

        public enum CardRare
        {
            Common,     // 白卡
            Uncommon,   // 绿卡
            Rare,       // 蓝卡
            SuperRare,  // 紫卡
            ExtremeRare,// 金卡
        }

        public enum CardStyle
        {
            Attack,     // 攻击牌（造成伤害）
            Skill,      // 技能牌（其他效果）
            Action,     // 行动牌（每场战斗只能打出一次）
        }

        public enum CardState
        {
            None,
            InDrawPile,
            InHand,
            InDiscardPile,
            InExhaustPile,
        }
        
        /// <summary> 丢弃此牌前执行 返回值决定是否可丢弃此牌 </summary>
        public virtual bool PreDiscard() => true;
        /// <summary> 丢弃此牌后执行 </summary>
        public virtual void PostDiscard() { }
        /// <summary> 消耗此牌前执行 返回值决定是否可消耗此牌 </summary>
        public virtual bool PreExhaust() => true;
        /// <summary> 消耗此牌后执行 </summary>
        public virtual void PostExhaust() { }
        /// <summary> 抽到此牌时执行 </summary>
        public virtual void OnDraw() { }
        /// <summary> 打出此牌时执行 </summary>
        public virtual void OnPlay() { }
        /// <summary> 在战斗期间执行 </summary>
        public virtual void Update() { }
        /// <summary> 战斗开始时执行 </summary>
        public virtual void OnBattleStart() { }
        /// <summary> 战斗结束后执行 </summary>
        public virtual void OnBattleEnd() { }
        /// <summary> 在牌堆时的绘制 </summary>
        public virtual void DrawSelfDeck(SpriteBatch spriteBatch) { }
        /// <summary> 在战斗时的绘制 </summary>
        public virtual void DrawSelfBattle(SpriteBatch spriteBatch) { }

        public void PlayToEnemy(Enemy enemy)
        {

        }

        public void PlayToPlayer(Player player)
        {

        }

        internal void PlayByPlayer(Player player)
        {
            if (this.unusable)
            {
                this.OnPlay();
                if (this.exhaust && this.PreExhaust())
                {
                    this.PostExhaust();
                }
            }
        }

        internal void DiscardByPlayer(Player player)
        {
            if (this.PreDiscard())
            {
                this.PostDiscard();
                if (this.ethereal && this.PreExhaust())
                {
                    this.PostExhaust();
                }
            }
        }
    }
}