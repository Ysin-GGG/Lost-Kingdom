using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LostKingdom.GameContent
{
    public class Buff
    {
        public string name;
        public string desc;
        public ushort type;
        public bool debuff;
        public int level;
        public int loseOnTurnEnd;
        public int loseOnTurnStart;
        public BuffPriority priority;

        public enum BuffPriority
        {
            AfterCardDiscarded,
            BeforeCardDiscarded,
        }


        public virtual void SetDefaults()
        {

        }

        public virtual void OnPlayerTurnStart()
        {

        }

        public virtual void OnPlayerTurnEnd()
        {

        }

        public virtual void OnEnemyTurnStart()
        {

        }

        public virtual void OnEnemyTurnEnd()
        {

        }
    }
}