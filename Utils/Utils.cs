using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LostKingdom.Utils
{
    public static class Utils
    {
        public static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static double Clamp(double value, double min, double max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static T Min<T>(params T[] args) => args.Min();
        public static byte Min(params byte[] args) => args.Min();
        public static sbyte Min(params sbyte[] args) => args.Min();
        public static short Min(params short[] args) => args.Min();
        public static ushort Min(params ushort[] args) => args.Min();
        public static int Min(params int[] args) => args.Min();
        public static uint Min(params uint[] args) => args.Min();
        public static long Min(params long[] args) => args.Min();
        public static ulong Min(params ulong[] args) => args.Min();
        public static float Min(params float[] args) => args.Min();
        public static double Min(params double[] args) => args.Min();

        public static T Max<T>(params T[] args) => args.Max();
        public static byte Max(params byte[] args) => args.Max();
        public static sbyte Max(params sbyte[] args) => args.Max();
        public static short Max(params short[] args) => args.Max();
        public static ushort Max(params ushort[] args) => args.Max();
        public static int Max(params int[] args) => args.Max();
        public static uint Max(params uint[] args) => args.Max();
        public static long Max(params long[] args) => args.Max();
        public static ulong Max(params ulong[] args) => args.Max();
        public static float Max(params float[] args) => args.Max();
        public static double Max(params double[] args) => args.Max();

        public static string Random(this string value, int length = 0)
        {
            Random rand = new Random();
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
