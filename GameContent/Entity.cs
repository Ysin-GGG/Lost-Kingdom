using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LostKingdom.Utils;

namespace LostKingdom.GameContent
{
    /// <summary> 可行动实体 </summary>
    public abstract class Entity
    {
        /// <summary> 数值运算区数量 </summary>
        public const byte ModifyCount = 4;
        /// <summary> 编号 </summary>
        public byte whoAmI;
        /// <summary> 是否死亡 </summary>
        public bool active;
        /// <summary> 是否存活 </summary>
        public bool alive;
        /// <summary> 名字 </summary>
        public string name;
        /// <summary> 生命值 </summary>
        public uint HP;
        /// <summary> 行动值 </summary>
        public uint AP;
        /// <summary> 攻击力 </summary>
        public uint ATK;
        /// <summary> 防御力 </summary>
        public uint DEF;
        /// <summary> 速度 </summary>
        public uint SPD;
        /// <summary> 最大生命值 </summary>
        public uint maxHP;
        /// <summary> 每回合恢复的行动值 </summary>
        public uint maxAP;
        /// <summary> 基础攻击力 </summary>
        public uint defATK;
        /// <summary> 基础防御力 </summary>
        public uint defDEF;
        /// <summary> 基础速度 </summary>
        public uint defSPD;

        /// <summary> 【攻击力】加算区 </summary>
        public readonly float[] atkAdd = new float[Entity.ModifyCount];
        /// <summary> 【攻击力】乘算区 </summary>
        public readonly float[] atkMul = new float[Entity.ModifyCount];
        /// <summary> 【防御力】加算区 </summary>
        public readonly float[] defAdd = new float[Entity.ModifyCount];
        /// <summary> 【防御力】乘算区 </summary>
        public readonly float[] defMul = new float[Entity.ModifyCount];
        /// <summary> 【速度】加算区 </summary>
        public readonly float[] spdAdd = new float[Entity.ModifyCount];
        /// <summary> 【速度】乘算区 </summary>
        public readonly float[] spdMul = new float[Entity.ModifyCount];
        /// <summary> 【造成伤害】加算区 </summary>
        public readonly float[] dealtDamageAdd = new float[Entity.ModifyCount];
        /// <summary> 【造成伤害】乘算区 </summary>
        public readonly float[] dealtDamageMul = new float[Entity.ModifyCount];
        /// <summary> 【所受伤害】加算区 </summary>
        public readonly float[] takenDamageAdd = new float[Entity.ModifyCount];
        /// <summary> 【所受伤害】乘算区 </summary>
        public readonly float[] takenDamageMul = new float[Entity.ModifyCount];

        /// <summary> 攻击时执行 </summary>
        public virtual void OnAttacked() { }

        /// <summary> 【造成伤害】运算前执行 </summary>
        /// <param name="damage"> 基础伤害 </param>
        /// <param name="add"> 【造成伤害】加算区 </param>
        /// <param name="mul"> 【造成伤害】乘算区 </param>
        public virtual void PreModifyDealtDamage(ref double damage, ref float[] add, ref float mul) { }

        /// <summary> 【造成伤害】运算后执行 </summary>
        /// <param name="damage"> 最终伤害 </param>
        public virtual void PostModifyDealtDamage(ref double damage) { }
        
        internal void _OnAttacked()
        {
            this.OnAttacked();
        }

        /// <summary> 原版绘制 </summary>
        public virtual void Draw(SpriteBatch spriteBatch) { }

        /// <summary> 原版数值刷新前执行 </summary>
        /// <returns> 是否执行原版数值刷新 </returns>
        public virtual bool PreUpdate() => !Main.GamePause;

        /// <summary> 原版数值刷新后执行 </summary>
        public virtual void PostUpdate() { }

        internal void UpdateEntity(GameTime gameTime)
        {
            if (this.PreUpdate())
            {
                double dmg = this.defATK;
                double def = this.defDEF;
                for (int i = 0; i < Player.ModifyCount; i++)
                {
                    dmg *= MyMath.Max(1.0d + this.atkAdd[i], 0.0d) * MyMath.Max(this.atkMul[i], 0.0d);
                    def *= MyMath.Max(1.0d + this.defAdd[i], 0.0d) * MyMath.Max(this.defMul[i], 0.0d);
                    this.takenDamageAdd[i] = this.dealtDamageAdd[i] = this.defAdd[i] = this.atkAdd[i] = 0.0f;
                    this.takenDamageMul[i] = this.dealtDamageMul[i] = this.defMul[i] = this.atkMul[i] = 1.0f;
                }
                this.ATK = (uint)dmg;
                this.DEF = (uint)def;
                this.PostUpdate();
            }
        }

        internal void DrawEntity(SpriteBatch spriteBatch) => this.Draw(spriteBatch);
        
        /// <summary> 死亡判定 </summary>
        /// <param name="restore"> 生命值归零时恢复的生命值 </param>
        /// <returns> 是否死亡 </returns>
        public bool CheckDead(uint restore)
        {
            if (this.HP == 0u) this.HP += restore;
            this.alive = this.HP > 0u;
            return !this.alive;
        }

        /// <summary> 受击 </summary>
        /// <param name="damage"> 基础伤害 </param>
        /// <returns> 最终伤害 </returns>
        public uint BeAttacked(double damage)
        {
            for (int i = 0; i < Player.ModifyCount; i++)
                damage *= MyMath.Max(1.0d + this.takenDamageAdd[i], 0.0d) * MyMath.Max(this.takenDamageMul[i], 0.0d);
            uint dmg = (uint)MyMath.Clamp(damage -= this.DEF, uint.MinValue, uint.MaxValue);
            /*
            if (this.block > 0)
            {
                uint dmg2 = MyMath.Max(dmg - this.block, 0u);
                uint def2 = MyMath.Max(this.block - dmg, 0u);
                dmg -= dmg2;
                this.block -= def2;
            }
            */
            this.HP -= dmg;
            this.CheckDead(0u);
            return dmg;
        }

        /// <summary> 获取【最终攻击力】 </summary>
        /// <returns> 最终攻击力 </returns>
        public uint GetDamage()
        {
            double damage = this.ATK;
            for (int i = 0; i < Player.ModifyCount; i++)
                damage *= MyMath.Max(1.0d + this.dealtDamageAdd[i], 0.0d) * MyMath.Max(this.dealtDamageMul[i], 0.0d);
            return (uint)damage;
        }

        /// <summary> 生命值变动 </summary>
        /// <param name="add"> 固定数值 </param>
        /// <param name="mul"> 百分比数值 </param>
        /// <param name="checkDead"> 是否进行死亡判定 </param>
        /// <param name="kill"> 是否致死 </param>
        public void ModifyHP(double add, double mul = 1.0d , bool checkDead = true, bool kill = true)
        {
            double hp = this.HP;
            hp += add;
            hp *= mul;
            this.HP = (uint)MyMath.Clamp(hp, uint.MinValue, this.maxHP);
            if (checkDead) this.CheckDead(kill ? 0u : 1u);
        }
    }
}