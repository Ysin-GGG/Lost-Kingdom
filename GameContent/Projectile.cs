using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LostKingdom.GameContent
{
    public class Projectile
    {
        public ushort id;
        public ushort type;
        public float opacity;
        public float rotation;
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 size;
        public int width;
        public int height;
        public int atk;
        public Entity target;
        public Entity owner;
        public EffectArea area;
        public bool active;
        public bool behind;

        public Projectile(ushort type)
        {
            for(int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i] == null || !Main.projectile[i].active)
                {
                    this.active = true;
                    this.SetDefaults();
                }
            }
        }

        public void SetDefaults()
        {
            switch(this.type)
            {
                default:
                    this.SetDefaults(this.type);
                    break;
            }
        }

        public virtual void SetDefaults(ushort type) { }

        internal void UpdateProjectile(GameTime gameTime) => this.Update();

        public virtual void Update() { }

        internal void DrawProjectile(SpriteBatch spriteBatch) => this.Draw(spriteBatch);
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}