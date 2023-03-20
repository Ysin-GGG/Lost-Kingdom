using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LostKingdom.Utils;

namespace LostKingdom.GameContent.UI
{
    public class UIImage : UIElement
    {
        public Texture2D image;
        public Color imageColor;
        public Color backgroundColor;
        public Vector2 offset;
        public Vector2 origin;
        public Vector2 scale;
        public bool trimExcess;
        public float rotation;
        private Vector2 _scale
        {
            get
            {
                switch (this.drawStyle)
                {
                    case UIImageDrawStyle.AdaptiveShorter:
                        return new Vector2(MyMath.Min(this.size.X / this.image.Width, this.size.Y / this.image.Height));
                    case UIImageDrawStyle.AdaptiveLonger:
                        return new Vector2(MyMath.Max(this.size.X / this.image.Width, this.size.Y / this.image.Height));
                    case UIImageDrawStyle.Full:
                        return this.size / this.image.Size();
                }
                return Vector2.One;
            }
        }
        public UIImageDrawStyle drawStyle;

        public enum UIImageDrawStyle
        {
            /// <summary> 原图 </summary>
            None,
            /// <summary> 适应UI最短边 </summary>
            AdaptiveShorter,
            /// <summary> 适应UI最长边 </summary>
            AdaptiveLonger,
            /// <summary> 拉伸铺满整个UI </summary>
            Full,
        }

        public UIImage(Texture2D image, UIImageDrawStyle drawStyle = UIImageDrawStyle.None)
        {
            this.size = image.Size();
            this.image = image;
            this.drawStyle = drawStyle;
            this.scale = Vector2.One;
        }

        protected sealed override void DrawUI(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.image, this.location + this.offset, null, this.imageColor, this.rotation, this.origin, this._scale * this.scale, SpriteEffects.None, 0f);
        }
    }
}
