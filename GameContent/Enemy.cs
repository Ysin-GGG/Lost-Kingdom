using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LostKingdom.GameContent
{
    public class Enemy : Entity
    {
        public uint block;
        public List<Card> deck;
        public List<Card> hand;
        public List<Card> drawPile;
        public List<Card> discardPile;
        public List<Card> exhaustPile;

        public enum ModifyID
        {
            Common,
            Buff,
            Critial,
            Other,
        }
    }
}