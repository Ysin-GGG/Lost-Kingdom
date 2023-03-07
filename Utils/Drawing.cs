using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LostKingdom.Utils
{
    public static class Drawing
    {
        private static SpriteBatch stringSprite = Main.MySpriteBatch;
        private static SpriteFont stringFont = Main.MySpriteFont;
        private static Color stringColor = Color.White * .8f;
        private static Vector2 stringLocation = Vector2.Zero;
        private static Vector2 stringSize = Vector2.Zero;
        private static Vector2 stringScale = Vector2.One;
        private static Vector2 stringCurPos = Vector2.Zero;
        private static Color backStringColor = Color.Black * .3f;
        private static Vector2 backStringScale = new Vector2(1.05f) * Options.UIScale;
        private static Vector2[] backStringOffset = new Vector2[4];

        public static void Draw(this Texture2D tex) { }
        public static void DrawColorfulString(SpriteBatch spriteBatch, string text, Vector2 position, Vector2 scale, uint index, uint split = 1, float opacity = 1f, params Color[] colors)
        {
            int chrCount = text.Length;
            int colorCount = colors.Length;
            float left = position.X;
            scale *= Options.UIScale;
            for (int i = 0; i < chrCount; i++)
            {
                string s = text[i].ToString();
                if (s == "\n")
                {
                    position.X = left;
                    position.Y += Main.MySpriteFont.LineSpacing * scale.Y;
                }
                else
                {
                    spriteBatch.DrawString(Main.MySpriteFont, s, position, colors[(index + i * split) % colorCount] * opacity, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                    position.X += (Main.MySpriteFont.MeasureString(s).X + Main.MySpriteFont.Spacing) * scale.X;
                }
            }
        }

        public static void DrawColorfulString(SpriteFont font, SpriteBatch spriteBatch, string text, Vector2 position, Vector2 scale, uint index, uint split = 1, float opacity = 1f, params Color[] colors)
        {
            int chrCount = text.Length;
            int colorCount = colors.Length;
            float left = position.X;
            scale *= Options.UIScale;
            for (int i = 0; i < chrCount; i++)
            {
                string s = text[i].ToString();
                if (s == "\n")
                {
                    position.X = left;
                    position.Y += font.LineSpacing * scale.Y;
                }
                else
                {
                    spriteBatch.DrawString(font, s, position, colors[(index + i * split) % colorCount] * opacity, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                    position.X += (font.MeasureString(s).X + font.Spacing) * scale.X;
                }
            }
        }

        /// <summary> 初始化字符串绘制器 </summary>
        /// <param name="location"> 初始坐标 </param>
        /// <param name="size"> 绘制区域 </param>
        /// <param name="scale"> 文本大小 </param>
        /// <param name="color"> 文本颜色 </param>
        /// <param name="spriteFont"> 字体 </param>
        /// <param name="spriteBatch"> 绘制器 </param>
        public static void InitDrawString(Vector2 location, Vector2 size, Vector2 scale, Color? color = null, SpriteFont spriteFont = null, SpriteBatch spriteBatch = null)
        {
            Drawing.stringCurPos = location * Options.UIScale;
            Drawing.stringLocation = location * Options.UIScale;
            Drawing.stringSize = size * Options.UIScale;
            Drawing.stringScale = scale * Options.UIScale;
            Drawing.stringColor = color ?? Drawing.stringColor;
            Drawing.stringSprite = spriteBatch ?? Drawing.stringSprite ?? Main.MySpriteBatch;
            Drawing.stringFont = spriteFont ?? Drawing.stringFont ?? Main.MySpriteFont;
        }

        /// <summary> 初始化字符串投影绘制器 </summary>
        /// <param name="offset"> 投影偏移 </param>
        /// <param name="scale"> 投影大小 </param>
        /// <param name="color"> 投影颜色 </param>
        public static void InitDrawStringBack(float offset = 1.2f, Vector2? scale = null, Color? color = null)
        {
            Drawing.backStringOffset[0].X = Drawing.backStringOffset[3].X = offset;
            Drawing.backStringOffset[1].X = Drawing.backStringOffset[2].X = -offset;
            Drawing.backStringOffset[0].Y = Drawing.backStringOffset[1].Y = offset;
            Drawing.backStringOffset[2].Y = Drawing.backStringOffset[3].Y = -offset;
            Drawing.backStringColor = color ?? Drawing.backStringColor;
            Drawing.backStringScale = (scale ?? new Vector2(1.05f)) * Options.UIScale;
        }

        /// <summary> 绘制文本 </summary>
        /// <param name="text"> 文本 </param>
        /// <param name="color"> 文本颜色 </param>
        public static void DrawString(string text, Color? color = null)
        {
            foreach (char c in text)
            {
                string s = c.ToString();
                if (s == "\n" || Drawing.stringCurPos.X >= Drawing.stringLocation.X + Drawing.stringSize.X)
                {
                    Drawing.stringCurPos.X = Drawing.stringLocation.X;
                    Drawing.stringCurPos.Y += Drawing.stringFont.LineSpacing * Drawing.stringScale.Y;
                }
                else
                {
                    foreach (Vector2 offset in Drawing.backStringOffset)
                        Drawing.stringSprite.DrawString(Drawing.stringFont, s, Drawing.stringCurPos, Drawing.backStringColor, 0f, Vector2.Zero, Drawing.backStringScale, SpriteEffects.None, 0f);
                    Drawing.stringSprite.DrawString(Drawing.stringFont, s, Drawing.stringCurPos, color ?? Drawing.stringColor, 0f, Vector2.Zero, Drawing.stringScale, SpriteEffects.None, 0f);
                    Drawing.stringCurPos.X += (Drawing.stringFont.MeasureString(s).X + Drawing.stringFont.Spacing) * Drawing.stringScale.X;
                }
            }
        }
    }
}
