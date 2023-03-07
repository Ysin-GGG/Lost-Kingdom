using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LostKingdom.Utils;

namespace LostKingdom.GameContent.UI
{
    public class UIButton : UIElement
    {
        public Texture2D image;
        public Texture2D box;
        public Color imageColor;
        public Color imageColor1;
        public Color imageColor2;
        public Color boxColor;
        public Color boxColor1;
        public Color boxColor2;
        public Vector2 scale;
        public float rotation;
        public float speed;
        public float imageOpacity;
        public float imageOpacityMin;
        public float imageOpacityMax;
        public float boxOpacity;
        public float boxOpacityMin;
        public float boxOpacityMax;

        public UIButton(Texture2D image, Texture2D box = null)
        {
            this.image = image;
            this.box = box;
            this.size = box == null ? image.Size() : box.Size();
            this.speed = .2f;
            this.boxColor2 = Color.White;
            this.boxColor1 = Color.Transparent;
            this.boxColor = this.boxColor1;
            this.imageColor2 = Color.White;
            this.imageColor1 = this.imageColor2 * .7f;
            this.imageColor = this.imageColor1;
            this.boxOpacityMax = 1.0f;
            this.boxOpacityMin = 0.0f;
            this.boxOpacity = this.boxOpacityMin;
            this.imageOpacityMax = 1.0f;
            this.imageOpacityMin = 0.0f;
            this.imageOpacity = this.imageOpacityMin;
        }

        protected sealed override void UpdateUI(GameTime gameTime)
        {
            if (this.IsMouseHovering)
            {
                if (this.imageOpacity < this.imageOpacityMax)
                {
                    this.imageOpacity = Utils.Utils.Clamp(Utils.Utils.Min(this.imageOpacity + this.speed, this.imageOpacityMax), 0.0f, 1.0f);
                    this.imageColor = Color.Lerp(this.imageColor1, this.imageColor2, this.imageOpacity);
                }
                if (this.boxOpacity < this.boxOpacityMax)
                {
                    this.boxOpacity = Utils.Utils.Clamp(Utils.Utils.Min(this.boxOpacity + this.speed, this.boxOpacityMax), 0.0f, 1.0f);
                    this.boxColor = Color.Lerp(this.boxColor1, this.boxColor2, this.boxOpacity);
                }
            }
            else
            {
                if (this.imageOpacity > this.imageOpacityMin)
                {
                    this.imageOpacity = Utils.Utils.Clamp(Utils.Utils.Max(this.imageOpacity - this.speed, this.imageOpacityMin), 0.0f, 1.0f);
                    this.imageColor = Color.Lerp(this.imageColor1, this.imageColor2, this.imageOpacity);
                }
                if (this.boxOpacity > this.boxOpacityMin)
                {
                    this.boxOpacity = Utils.Utils.Clamp(Utils.Utils.Max(this.boxOpacity - this.speed, this.boxOpacityMin), 0.0f, 1.0f);
                    this.boxColor = Color.Lerp(this.boxColor1, this.boxColor2, this.boxOpacity);
                }
            }
        }
        string msg = "";
        protected override void OnMouseDown(UIElement e, MouseEvent evt)
        {
            msg += "1";
        }

        protected override void OnMouseEnter(UIElement e, MouseEvent evt)
        {
        }

        protected sealed override void DrawUI(SpriteBatch spriteBatch)
        {
            if (this.box != null) spriteBatch.Draw(this.box, this.location, null, this.boxColor, 0f, Vector2.Zero, 1f, 0, 0f);
            if (this.image != null) spriteBatch.Draw(this.image, this.location, null, this.imageColor, 0f, Vector2.Zero, 1f, 0, 0f);
            Drawing.DrawColorfulString(spriteBatch, msg, this.location, Vector2.One, Main.timer * 16, 16, 1f, Main.rainbow_raw);
        }
    }
}
