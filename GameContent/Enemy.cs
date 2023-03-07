using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LostKingdom.GameContent
{
    public class Enemy
    {
        public const int ModifyCount = 4;
        public byte whoAmI;
        public bool active;
        public bool alive;
        public string name;
        public uint hp;
        public uint mp;
        public uint ap;
        public uint hpMax;
        public uint mpMax;
        public uint apMax;
        public uint speed;
        public uint block;
        public uint damage;
        public uint defense;
        public uint defDamage;
        public uint defDefense;
        public float[] damageAdd = new float[Enemy.ModifyCount];
        public float[] damageMul = new float[Enemy.ModifyCount];
        public float[] defenseAdd = new float[Enemy.ModifyCount];
        public float[] defenseMul = new float[Enemy.ModifyCount];
        public float[] dealtDamageAdd = new float[Enemy.ModifyCount];
        public float[] dealtDamageMul = new float[Enemy.ModifyCount];
        public float[] takenDamageAdd = new float[Enemy.ModifyCount];
        public float[] takenDamageMul = new float[Enemy.ModifyCount];
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

        public virtual void OnAttacked() { }
        internal void _OnAttacked()
        {
            this.OnAttacked();
        }

        public virtual bool PreUpdateEffect() => true;
        public virtual void PostUpdateEffect() { }
        internal void UpdateEffect()
        {
            if (this.PreUpdateEffect())
            {
                double dmg = this.defDamage;
                double def = this.defDefense;
                for (int i = 0; i < Player.ModifyCount; i++)
                {
                    dmg *= Utils.Utils.Max(1.0d + this.damageAdd[i], 0.0d) * Utils.Utils.Max(this.damageMul[i], 0.0d);
                    def *= Utils.Utils.Max(1.0d + this.defenseAdd[i], 0.0d) * Utils.Utils.Max(this.defenseMul[i], 0.0d);
                    this.takenDamageAdd[i] = this.dealtDamageAdd[i] = this.defenseAdd[i] = this.damageAdd[i] = 0.0f;
                    this.takenDamageMul[i] = this.dealtDamageMul[i] = this.defenseMul[i] = this.damageMul[i] = 1.0f;
                }
                this.damage = (uint)dmg;
                this.defense = (uint)def;
                this.PostUpdateEffect();
            }
        }
        
        public bool CheckDead(ushort restore)
        {
            if (this.hp == 0) this.hp += restore;
            this.alive = this.hp > 0;
            return !this.active;
        }

        public uint BeAttacked(double damage)
        {
            for (int i = 0; i < Player.ModifyCount; i++)
                damage *= Utils.Utils.Max(1.0d + this.takenDamageAdd[i], 0.0d) * Utils.Utils.Max(this.takenDamageMul[i], 0.0d);
            uint dmg = (uint)Utils.Utils.Clamp(damage -= this.defense, uint.MinValue, uint.MaxValue);
            if (this.block > 0)
            {
                uint dmg2 = Utils.Utils.Max(dmg - this.block, 0);
                uint def2 = Utils.Utils.Max(this.block - dmg, 0);
                dmg -= dmg2;
                this.block -= def2;
            }
            this.hp -= dmg;
            this.OnAttacked();
            this.CheckDead(0);
            return dmg;
        }

        public uint GetDamage()
        {
            double damage = this.damage;
            for (int i = 0; i < Player.ModifyCount; i++)
                damage *= Utils.Utils.Max(1.0d + this.dealtDamageAdd[i], 0.0d) * Utils.Utils.Max(this.dealtDamageMul[i], 0.0d);
            return (uint)damage;
        }
    }
}