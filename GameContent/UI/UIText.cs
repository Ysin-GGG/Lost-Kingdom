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
    /// <summary> UI文本 </summary>
    public class UIText : UIElement
    {
        /// <summary> 文本 </summary>
        public string text;
        /// <summary> 文本颜色 </summary>
        public Color color;
        /// <summary> 文本大小 </summary>
        public Vector2 scale;
        /// <summary> 文本对齐方式 </summary>
        public TextAlign align;


        /// <summary> 对齐方式 </summary>
        public enum TextAlign
        {
            /// <summary> 左上对齐 </summary>
            TopLeft = 0,
            /// <summary> 顶部居中对齐 </summary>
            Top = 1,
            /// <summary> 右上对齐 </summary>
            TopRight = 2,
            /// <summary> 左对齐 </summary>
            Left = 4,
            /// <summary> 居中对齐 </summary>
            Center = 8,
            /// <summary> 右对齐 </summary>
            Right = 16,
            /// <summary> 左下对齐 </summary>
            BottomLeft = 32,
            /// <summary> 底部居中对齐 </summary>
            Bottom = 64,
            /// <summary> 右下对齐 </summary>
            BottomRight = 128,
        }

        /// <summary> 修改绘制 </summary>
        public event Func<SpriteBatch, UIText, bool> ModifyDrawText;

        /// <summary> 新建UI文本组件 </summary>
        /// <param name="size"> 文本绘制区域 </param>
        /// <param name="text"> 文本 </param>
        /// <param name="scale"> 文本大小 </param>
        /// <param name="color"> 文本颜色 </param>
        /// <param name="align"> 文本对齐方式 </param>
        public UIText(Vector2 size, string text, Vector2 scale, Color? color = null, TextAlign align = TextAlign.Center)
        {
            this.text = text;
            this.color = color ?? Color.White;
            this.size = size;
            this.scale = scale;
            this.align = align;
        }

        protected override void DrawUI(SpriteBatch spriteBatch)
        {
            bool? draw = this.ModifyDrawText?.Invoke(spriteBatch, this);
            if (!draw.HasValue || !draw.Value)
            {
                SpriteFont font = Main.MySpriteFont;
                Vector2 size = font.MeasureString(this.text);
                Vector2 pos = this.location;
                pos.Y -= 2f;

                switch(this.align)
                {
                    case TextAlign.TopLeft:
                        break;
                    case TextAlign.Top:
                        pos.X += (this.size.X - size.X) / 2f;
                        break;
                    case TextAlign.TopRight:
                        pos.X += this.size.X - size.X;
                        break;
                    case TextAlign.Left:
                        pos.Y += (this.size.Y - size.Y) / 2f;
                        break;
                    case TextAlign.Center:
                        pos += (this.size - size) / 2f;
                        break;
                    case TextAlign.Right:
                        pos.X += this.size.X - size.X;
                        pos.Y += (this.size.Y - size.Y) / 2f;
                        break;
                    case TextAlign.BottomLeft:
                        pos.Y += this.size.Y - size.Y;
                        break;
                    case TextAlign.Bottom:
                        pos.X += (this.size.X - size.X) / 2f;
                        pos.Y += this.size.Y - size.Y;
                        break;
                    case TextAlign.BottomRight:
                        pos += this.size - size;
                        break;
                }

                Drawing.InitDrawString(pos, this.size, Vector2.One);
                Drawing.InitDrawStringBack();
                Drawing.DrawString(this.text);
            }
        }
    }
}
