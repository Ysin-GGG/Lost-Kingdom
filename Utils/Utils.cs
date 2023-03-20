using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LostKingdom.Utils
{
    /// <summary> 实用工具库 </summary>
    public static class Utils
    {
        public static string Random(this string value, int length = 0)
        {
            Rand rand = new Rand();
            int len = length > 0 ? length : value.Length;
            char[] res = value.ToCharArray(0, len);
            for (int i = 0; i < len; i++)
                res[i] = value[rand.NextInt(0, value.Length)];
            return new string(res);
        }

        public static string ConvertMemory(float memory)
        {
            string[] res = new string[] { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB", "BB", "NB", "PB", "DB", "CB", "XB", "??" };
            int idx = 0;
            while (memory >= 1024 && ++idx < res.Length - 1) memory /= 1024;
            return string.Format("{0} {1}", Math.Round(memory, 1), res[idx]);
        }

        public static Vector2 GetPoint(Rectangle rect, int w, int h)
        {
            float[] w2 = { 0f, rect.Width / 2f, rect.Width };
            float[] h2 = { 0f, rect.Height / 2f, rect.Height };
            return new Vector2(w2[w], h2[h]);
        }

        public static Vector2 ToRotation(this Vector2 value, float degree)
        {
            Vector2 newValue = new Vector2(value.X * MyMath.cos(degree) - value.Y * MyMath.sin(degree), value.X * MyMath.sin(degree) + value.Y * MyMath.cos(degree));

            return newValue;
        }

        public static Vector2 TopLeft(this Rectangle rect) => Utils.GetPoint(rect, 0, 0);
        public static Vector2 Top(this Rectangle rect) => Utils.GetPoint(rect, 1, 0);
        public static Vector2 TopRight(this Rectangle rect) => Utils.GetPoint(rect, 2, 0);
        public static Vector2 Left(this Rectangle rect) => Utils.GetPoint(rect, 0, 1);
        public static Vector2 Center(this Rectangle rect) => Utils.GetPoint(rect, 1, 1);
        public static Vector2 Right(this Rectangle rect) => Utils.GetPoint(rect, 2, 1);
        public static Vector2 BottomLeft(this Rectangle rect) => Utils.GetPoint(rect, 0, 2);
        public static Vector2 Bottom(this Rectangle rect) => Utils.GetPoint(rect, 1, 2);
        public static Vector2 BottomRight(this Rectangle rect) => Utils.GetPoint(rect, 2, 2);

        public static Vector2 TopLeft(this Texture2D texture) => Utils.GetPoint(texture.Bounds, 0, 0);
        public static Vector2 Top(this Texture2D texture) => Utils.GetPoint(texture.Bounds, 1, 0);
        public static Vector2 TopRight(this Texture2D texture) => Utils.GetPoint(texture.Bounds, 2, 0);
        public static Vector2 Left(this Texture2D texture) => Utils.GetPoint(texture.Bounds, 0, 1);
        public static Vector2 Center(this Texture2D texture) => Utils.GetPoint(texture.Bounds, 1, 1);
        public static Vector2 Right(this Texture2D texture) => Utils.GetPoint(texture.Bounds, 2, 1);
        public static Vector2 BottomLeft(this Texture2D texture) => Utils.GetPoint(texture.Bounds, 0, 2);
        public static Vector2 Bottom(this Texture2D texture) => Utils.GetPoint(texture.Bounds, 1, 2);
        public static Vector2 BottomRight(this Texture2D texture) => Utils.GetPoint(texture.Bounds, 2, 2);

        public static Vector2 TopLeft(this Texture2D texture, Vector2 pos) => pos - Utils.GetPoint(texture.Bounds, 0, 0);
        public static Vector2 Top(this Texture2D texture, Vector2 pos) => pos - Utils.GetPoint(texture.Bounds, 1, 0);
        public static Vector2 TopRight(this Texture2D texture, Vector2 pos) => pos - Utils.GetPoint(texture.Bounds, 2, 0);
        public static Vector2 Left(this Texture2D texture, Vector2 pos) => pos - Utils.GetPoint(texture.Bounds, 0, 1);
        public static Vector2 Center(this Texture2D texture, Vector2 pos) => pos - Utils.GetPoint(texture.Bounds, 1, 1);
        public static Vector2 Right(this Texture2D texture, Vector2 pos) => pos - Utils.GetPoint(texture.Bounds, 2, 1);
        public static Vector2 BottomLeft(this Texture2D texture, Vector2 pos) => pos - Utils.GetPoint(texture.Bounds, 0, 2);
        public static Vector2 Bottom(this Texture2D texture, Vector2 pos) => pos - Utils.GetPoint(texture.Bounds, 1, 2);
        public static Vector2 BottomRight(this Texture2D texture, Vector2 pos) => pos - Utils.GetPoint(texture.Bounds, 2, 2);

        public static Rectangle Frame(this Texture2D texture, int frameX, int frameY, int frameCountX, int frameCountY)
        {
            int frameWidth = texture.Width / frameCountX;
            int frameHeight = texture.Height / frameCountY;
            return new Rectangle(frameX * frameWidth, frameY * frameHeight, frameWidth, frameHeight - 1);
        }

        public static Vector2 Size(this Texture2D tex) => new Vector2(tex.Width, tex.Height);
        public static Point ToPoint(this Vector2 vector) => new Point((int)vector.X, (int)vector.Y);
    }
}
