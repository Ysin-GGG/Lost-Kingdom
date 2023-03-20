using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LostKingdom.GameContent.Localization;
using LostKingdom.GameContent.ID;
using LostKingdom.GameContent.UI;
using LostKingdom.Utils;

namespace LostKingdom.GameContent
{
    /// <summary> 卡牌 </summary>
    public class Card
    {
        /// <summary> 内部名称 </summary>
        public virtual string InnerName { get; private set; }
        /// <summary> 名称 </summary>
        public readonly string name;
        /// <summary> 描述 </summary>
        public readonly string desc;
        /// <summary> 编号 </summary>
        public readonly ushort type;
        /// <summary> 生成时间 </summary>
        public readonly long time;
        /// <summary> 初始消耗的生命值 </summary>
        public uint defHP;
        /// <summary> 初始消耗的行动力 </summary>
        public uint defAP;
        /// <summary> 消耗的生命值 </summary>
        public uint costHP;
        /// <summary> 消耗的行动力 </summary>
        public uint costAP;
        /// <summary> 固有 </summary>
        public bool innate;
        /// <summary> 消耗 </summary>
        public bool exhaustible;
        /// <summary> 虚无 </summary>
        public bool ethereal;
        /// <summary> 滞留 </summary>
        public bool quiescent;
        /// <summary> 锁定 </summary>
        public bool unusable;
        /// <summary> 先机 </summary>
        public bool echoic;
        /// <summary> 损耗 </summary>
        public bool broken;
        /// <summary> 无视障碍 </summary>
        public bool ignoreBarrier;
        /// <summary> 稀有度 </summary>
        public CardRare rare;
        /// <summary> 种类 </summary>
        public CardStyle style;
        /// <summary> 状态 </summary>
        public CardState state;
        /// <summary> 目标类型 </summary>
        public TargetType targetType;
        /// <summary> 作用类型 </summary>
        public RangeType rangeType;
        /// <summary> 伤害值 </summary>
        public uint damage;
        /// <summary> 连击数 </summary>
        public uint combo;
        /// <summary> 作用范围 </summary>
        public EffectArea range;
        /// <summary> 作用范围 </summary>
        public float opacity;

        /// <summary> 卡牌稀有度 </summary>
        public enum CardRare
        {
            /// <summary> 常见 </summary>
            Common,
            /// <summary> 罕见 </summary>
            Uncommon,
            /// <summary> 稀有 </summary>
            Rare,
            /// <summary> 非常稀有 </summary>
            SuperRare,
            /// <summary> 极其稀有 </summary>
            ExtremeRare,
        }

        /// <summary> 卡牌种类 </summary>
        public enum CardStyle
        {
            /// <summary> 无 </summary>
            None,
            /// <summary> 攻击牌 </summary>
            Attack,
            /// <summary> 技能牌 </summary>
            Skill,
            /// <summary> 行动牌 </summary>
            Action,
            /// <summary> 状态牌 </summary>
            Status,
            /// <summary> 诅咒牌 </summary>
            Curse,
        }

        /// <summary> 卡牌状态 </summary>
        public enum CardState
        {
            /// <summary> 无 </summary>
            None,
            /// <summary> 手牌 </summary>
            InHand,
            /// <summary> 抽牌堆 </summary>
            InDrawPile,
            /// <summary> 弃牌堆 </summary>
            InDiscardPile,
            /// <summary> 消耗堆 </summary>
            InExhaustPile,
        }

        /// <summary> 目标类型 </summary>
        public enum TargetType
        {
            /// <summary> 所有实体 </summary>
            Entity,
            /// <summary> 仅限敌人 </summary>
            OnlyEnemy,
            /// <summary> 仅限队友 </summary>
            OnlyAlly,
            /// <summary> 仅限自身 </summary>
            OnlySelf,
            /// <summary> 不包括敌人 </summary>
            ExceptEnemy,
            /// <summary> 不包括队友 </summary>
            ExceptAlly,
            /// <summary> 不包括自身 </summary>
            ExceptSelf,
        }

        /// <summary> 作用类型 </summary>
        public enum RangeType
        {
            /// <summary> 范围内的一个目标 </summary>
            Single,
            /// <summary> 范围内的多个目标 </summary>
            Multiple,
            /// <summary> 范围内的连续多个目标 </summary>
            Combo,
            /// <summary> 范围内的所有目标 </summary>
            AoE,
        }

        /// <summary> 丢弃此牌前执行 返回值决定是否可丢弃此牌（默认true） </summary>
        public virtual bool PreDiscard() => true;
        /// <summary> 丢弃此牌后执行 </summary>
        public virtual void PostDiscard() { }
        /// <summary> 消耗此牌前执行 返回值决定是否可消耗此牌（默认true） </summary>
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

        /// <summary> 生成一张卡牌 </summary>
        /// <param name="type"> 卡牌编号 </param>
        public Card(ushort type)
        {
            this.type = type;
            this.InnerName = CardID.InnerName[this.type];
            this.name = GameLocalization.GetText($"Name.Card.{type}");//改回使用内部名称
            this.desc = GameLocalization.GetText($"Desc.Card.{type}");
            this.time = DateTime.Now.Ticks;
            this.defHP = 0;
            this.defAP = 0;
            this.costHP = 0;
            this.costAP = 0;
            this.innate = false;
            this.exhaustible = false;
            this.ethereal = false;
            this.quiescent = false;
            this.unusable = false;
            this.echoic = false;
            this.broken = false;
            this.range = new EffectArea();
            this.rare = CardRare.Common;
            this.style = CardStyle.None;
            this.state = CardState.None;
            this.targetType = TargetType.Entity;
            this.rangeType = RangeType.Single;
            this.SetDefaults();
        }

        /// <summary> 为当前卡牌自动设置基础数据 </summary>
        public void SetDefaults()
        {
            switch (this.type)
            {
                case CardID.ConjurationOfLightning:
                    this.defAP = 2;
                    this.style = CardStyle.Attack;
                    this.targetType = TargetType.OnlyEnemy;
                    this.rangeType = RangeType.Single;
                    this.rare = CardRare.Uncommon;
                    this.range = EffectArea.Prismatic(2);
                    break;
                case CardID.Fireball:
                    this.defAP = 1;
                    this.style = CardStyle.Attack;
                    this.targetType = TargetType.OnlyEnemy;
                    this.rangeType = RangeType.Multiple;
                    this.rare = CardRare.Common;
                    this.range = EffectArea.Prismatic(2);
                    break;
                case CardID.Incineration:
                    this.defAP = 2;
                    this.style = CardStyle.Attack;
                    this.targetType = TargetType.OnlyEnemy;
                    this.rangeType = RangeType.AoE;
                    this.rare = CardRare.Rare;
                    this.range = EffectArea.Prismatic(2);
                    break;
                case CardID.TimeReversal:
                    this.defAP = 2;
                    this.style = CardStyle.Action;
                    this.targetType = TargetType.OnlySelf;
                    this.rangeType = RangeType.Single;
                    this.rare = CardRare.ExtremeRare;
                    break;
                case CardID.TimeStop:
                    this.defAP = 2;
                    this.style = CardStyle.Action;
                    this.targetType = TargetType.OnlySelf;
                    this.rangeType = RangeType.Single;
                    this.rare = CardRare.ExtremeRare;
                    break;
                case CardID.TimeJump:
                    this.defAP = 2;
                    this.style = CardStyle.Action;
                    this.targetType = TargetType.OnlySelf;
                    this.rangeType = RangeType.Single;
                    this.rare = CardRare.ExtremeRare;
                    break;
                default:
                    this.SetDefaults(type);
                    break;
            }
        }

        /// <summary> 为当前卡牌设置指定基础数据 </summary>
        /// <param name="type"> 卡牌编号 </param>
        public virtual void SetDefaults(ushort type) { }

        /// <summary> 克隆当前卡牌 </summary>
        public virtual Card Clone()
        {
            Card card = new Card(this.type);
            return card;
        }

        /// <summary> 绘制当前卡牌 </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            float scale = Options.UIScale * 1.5f;
            Texture2D tex = Asset.Card[CardID.InnerName[1]];
            spriteBatch.Draw(tex, Main.MouseScreen, null, Color.White, 0f, Vector2.Zero, scale, 0, 0f);
            
        }

        public void PlayToEnemy(Enemy enemy)
        {

        }

        public void PlayToPlayer(Player player)
        {

        }

        internal void PlayByPlayer(Player player)
        {
            if (!this.unusable)
            {
                this.OnPlay();
                if (this.exhaustible) this.Exhaust();
            }
        }

        internal void DiscardByPlayer(Player player)
        {
            if (this.PreDiscard())
            {
                if (this.ethereal) this.Exhaust();
                else this.PostDiscard();
            }
        }

        /// <summary> 消耗此牌 </summary>
        public void Exhaust()
        {
            if (this.PreExhaust())
            {
                this.PostExhaust();
            }
        }
    }
}