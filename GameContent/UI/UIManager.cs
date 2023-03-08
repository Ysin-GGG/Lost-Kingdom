using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LostKingdom.GameContent.UI
{
    public class UIManager
    {
        protected UILayer.LayerName layer;
        protected readonly UIElement origin = new UIElement();
        public bool active;
        public bool visible;

        public UIManager(UILayer.LayerName layer)
        {
            this.layer = layer;
            this.origin = new UIElement();
            this.active = true;
            this.visible = true;
        }

        public void Update(GameTime gameTime) { if (this.active) this.UpdateUI(gameTime); }

        protected virtual void Initialize()
        {

        }

        protected virtual void UpdateUI(GameTime gameTime)
        {

        }

        protected virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}