using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using LostKingdom.GameContent.Audio;
using LostKingdom.Utils;

namespace LostKingdom.GameContent.UI
{
    public class UIImageButton : UIElement
    {
        public Texture2D image;
        public Vector2 scale;
        public Color color;
        public float rotation;

        public UIImageButton(Texture2D image, Color? color = null)
        {
            this.image = image;
            this.color = color ?? Color.White;
            this.scale = Vector2.One;
            this.size = image.Size();
            this.size.Y /= 2.0f;
        }

        protected override void OnMouseEnter(UIElement e, MouseEvent evt)
        {
            Audio.Audio.RawSound[0].Replay();
        }

        protected override void OnMouseClick(UIElement e, MouseEvent evt)
        {
            if (evt.leftButton) Audio.Audio.RawSound[1].Play();
        }

        protected sealed override void DrawUI(SpriteBatch spriteBatch)
        {
            if (this.image != null) spriteBatch.Draw(this.image, this.location, this.image.Frame(0, this.IsMouseHovering ? 1 : 0, 1, 2), this.color, 0f, Vector2.Zero, this.scale, 0, 0f);
        }
    }
}
