using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using LostKingdom.Utils;

namespace LostKingdom.GameContent.UI
{
    public class UIPanel : UIElement
    {
        /// <summary> 背景框 </summary>
        public Texture2D[] back;
        /// <summary> 前景框 </summary>
        public Texture2D[] front;
        /// <summary> 背景框颜色 </summary>
        public Color backColor;
        /// <summary> 前景框颜色 </summary>
        public Color frontColor;
        /// <summary> 裁剪角的尺寸 </summary>
        public float corner;

        /// <summary> 新建UI </summary>
        /// <param name="back"> 背景框（常规，鼠标悬浮，被选中） </param>
        /// <param name="front"> 前景框（常规，鼠标悬浮，被选中） </param>
        /// <param name="corner"> 裁剪角的尺寸 </param>
        public UIPanel(Texture2D back, Texture2D front, float corner = 8.0f)
        {
            if (back != null)
            {
                this.back = new Texture2D[3];
                this.back[0] =
                this.back[1] =
                this.back[2] = back;
            }
            if (front != null)
            {
                this.front = new Texture2D[3];
                this.front[0] =
                this.front[1] =
                this.front[2] = front;
            }
            this.backColor = this.frontColor = Color.White;
            this.corner = corner;
        }

        /// <summary> 新建UI </summary>
        /// <param name="back"> 背景框（常规，鼠标悬浮，被选中） </param>
        /// <param name="front"> 前景框（常规，鼠标悬浮，被选中） </param>
        /// <param name="corner"> 裁剪角的尺寸 </param>
        public UIPanel(Texture2D[] back, Texture2D[] front, float corner = 8.0f)
        {
            if (back.Length == 1)
            {
                this.back = new Texture2D[3];
                this.back[0] =
                this.back[1] =
                this.back[2] = back[0];
            }
            if (back.Length == 2)
            {
                this.back = new Texture2D[3];
                this.back[0] = back[0];
                this.back[1] =
                this.back[2] = back[1];
            }
            if (back.Length >= 3) this.back = back;

            if (front.Length == 1)
            {
                this.front = new Texture2D[3];
                this.front[0] =
                this.front[1] =
                this.front[2] = front[0];
            }
            if (front.Length == 2)
            {
                this.front = new Texture2D[3];
                this.front[0] = front[0];
                this.front[1] =
                this.front[2] = front[1];
            }
            if (front.Length >= 3) this.front = front;
            this.backColor = this.frontColor = Color.White;
            this.corner = corner;
        }

        protected sealed override void DrawUI(SpriteBatch spriteBatch)
        {
            int style = 0;
            if (this.IsMouseHovering) style = 1;
            if (this.IsSelected) style = 2;
            int c1 = (int)(this.corner * Options.UIScale);
            int sizeX = (int)(this.size.X * Options.UIScale);
            int sizeY = (int)(this.size.Y * Options.UIScale);
            int posX = (int)(this.location.X);
            int posY = (int)(this.location.Y);
            int[] x1 = { posX, posX + c1, posX + sizeX - c1 };
            int[] y1 = { posY, posY + c1, posY + sizeY - c1 };
            int[] w1 = { c1, sizeX - 2 * c1, c1 };
            int[] h1 = { c1, sizeY - 2 * c1, c1 };
            for (int i = 0; i < 3; i++)
            {
                int c2 = (int)this.corner;
                sizeX = this.back[i].Width;
                sizeY = this.back[i].Height;
                int[] x2 = { 0, c2, sizeX - c2 };
                int[] y2 = { 0, c2, sizeY - c2 };
                int[] w2 = { c2, x2[2] - c2, c2 };
                int[] h2 = { c2, y2[2] - c2, c2 };
                for (int j = 0; j < 3; j++)
                {
                    Rectangle dRect = new Rectangle(x1[i], y1[j], w1[i], h1[j]);
                    Rectangle sRect = new Rectangle(x2[i], y2[j], w2[i], h2[j]);
                    spriteBatch.Draw(this.back[style], dRect, sRect, this.backColor, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                    spriteBatch.Draw(this.front[style], dRect, sRect, this.frontColor, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                }
            }
        }
    }
}