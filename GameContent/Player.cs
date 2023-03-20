using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LostKingdom.GameContent
{
    public class Player : Entity
    {
        /// <summary> 战斗状态 </summary>
        public static bool isFighting = false;
        /// <summary> 绘制牌组中的牌 </summary>
        public static bool drawCardsInDeck = false;
        /// <summary> 绘制手中的牌 </summary>
        public static bool drawCardsInHand = false;
        /// <summary> 绘制抽牌堆的牌 </summary>
        public static bool drawCardsInDrawPile = false;
        /// <summary> 绘制弃牌堆的牌 </summary>
        public static bool drawCardsInDiscardPile = false;
        /// <summary> 绘制消耗堆的牌 </summary>
        public static bool drawCardsInExhaustPile = false;
        public static Player Me => Main.player[0];

        public uint block;
        public List<Card> deck;
        public List<Card> hand;
        public List<Card> drawPile;
        public List<Card> discardPile;
        public List<Card> exhaustPile;
        public PlayerClass clazz;

        public enum PlayerClass
        {

        }

        public enum ModifyID
        {
            Common,
            Buff,
            Critial,
            Other,
        }

        public Player()
        {
            this.deck = new List<Card>();
            this.hand = new List<Card>();
            this.drawPile = new List<Card>();
            this.discardPile = new List<Card>();
            this.exhaustPile = new List<Card>();
        }

        /// <summary> 添加卡牌到牌堆顶部 </summary>
        /// <param name="card"> 目标卡牌 </param>
        /// <param name="target"> 目标牌堆 </param>
        public virtual void AddCard(Card card, List<Card> target)
        {
            target.Insert(0, card);
        }

        /// <summary> 转移卡牌到牌堆顶部 </summary>
        /// <param name="card"> 目标卡牌 </param>
        /// <param name="origin"> 原牌堆 </param>
        /// <param name="target"> 新牌堆 </param>
        public virtual void MoveCard(Card card, List<Card> origin, List<Card> target)
        {
            int idx = origin.IndexOf(card);
            if(idx >= 0)
            {
                target.Insert(0, origin[idx]);
                origin.RemoveAt(idx);
            }
        }

        /// <summary> 洗牌 </summary>
        /// <param name="includeHands"> 包括手牌 </param>
        public void Shuffle(bool includeHands = false)
        {
            this.drawPile.AddRange(discardPile);
            this.discardPile.Clear();
            if (includeHands)
            {
                this.drawPile.AddRange(this.hand);
                this.hand.Clear();
            }
            for (int i = 0; i < drawPile.Count; i++)
            {
                int idx = Main.rand.NextInt(i, this.drawPile.Count);
                Card temp = this.drawPile[i];
                this.drawPile[i] = this.drawPile[idx];
                this.drawPile[idx] = temp;
            }
        }
    }
}