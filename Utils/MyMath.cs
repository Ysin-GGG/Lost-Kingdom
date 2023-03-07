using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace LostKingdom.Utils
{
    public static class MyMath
    {
        public const double Phi0 = 0.6180339887498948;
        public const double Phi = 1.6180339887498948;
        public const double G = 9.8061905036721905;
        public const double E = 2.7182818284590451;
        public const double Pi = 3.1415926535897931;
        public const double Pi2 = pi * 2;
        public const double Deg01 = Pi / 180;
        public const double Deg15 = Pi / 12;
        public const double Deg30 = Pi / 6;
        public const double Deg45 = Pi / 4;
        public const double Deg60 = Pi / 3;
        public const double Deg90 = Pi / 2;
        public const float phi0 = 0.6180339f;
        public const float phi = 1.6180339f;
        public const float g = 9.8066500f;
        public const float e = 2.7182818f;
        public const float pi = 3.1415927f;
        public const float pi2 = pi * 2;
        public const float deg01 = pi / 180;
        public const float deg15 = pi / 12;
        public const float deg30 = pi / 6;
        public const float deg45 = pi / 4;
        public const float deg60 = pi / 3;
        public const float deg90 = pi / 2;
        public static int ceiling(double value) => (int)value;
        public static int floor(double value) => (int)value;
        public static float cos(double degree) => (float)Math.Cos(degree);
        public static float sin(double degree) => (float)Math.Sin(degree);
        public static float tan(double degree) => (float)Math.Tan(degree);
        public static float pow(double x, double a) => (float)Math.Pow(x, a);
        public static float log(double x, double a = MyMath.E) => (float)(Math.Log10(x) / Math.Log10(a));
        public static float GetZ(double x, double y) => (float)Math.Sqrt(x * x + y * y);
        public static Vector2 GetXY(double degree) => new Vector2(MyMath.cos(degree), MyMath.sin(degree));
        public static int Lerp(int value1, int value2, float percentage) => (int)(value1 * (1f - MyMath.Clamp(percentage, 0, 1)) + value2 * MyMath.Clamp(percentage, 0, 1));
        public static float Lerp(float value1, float value2, float percentage) => (float)(value1 * (1f - MyMath.Clamp(percentage, 0, 1)) + value2 * MyMath.Clamp(percentage, 0, 1));
        public static Point Lerp(Point value1, Point value2, float percentage) => MyMath.Lerp(value1, value2, new Vector2(percentage));
        public static Vector2 Lerp(Vector2 value1, Vector2 value2, float percentage) => MyMath.Lerp(value1, value2, new Vector2(percentage));
        public static Point Lerp(Point value1, Point value2, Vector2 percentage) => new Point((int)MyMath.Lerp(value1.X, value2.X, percentage.X), (int)MyMath.Lerp(value1.Y, value2.Y, percentage.Y));
        public static Vector2 Lerp(Vector2 value1, Vector2 value2, Vector2 percentage) => new Vector2((float)MyMath.Lerp(value1.X, value2.X, percentage.X), (float)MyMath.Lerp(value1.Y, value2.Y, percentage.Y));
        public static int Clamp(int value, int min, int max) => MyMath.Min(MyMath.Max(min, value), max);
        public static int Clamp(int value, int min, int max, out int extra) => MyMath.Clamp((extra = value - (value = MyMath.Clamp(value, min, max))) + value, min, max);
        public static float Clamp(float value, float min, float max) => MyMath.Min(MyMath.Max(min, value), max);
        public static float Clamp(float value, float min, float max, out float extra) => MyMath.Clamp((extra = value - (value = MyMath.Clamp(value, min, max))) + value, min, max);
        public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max) => new Vector2(MyMath.Clamp(value.X, min.X, max.X), MyMath.Clamp(value.Y, min.Y, max.Y));
        public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max, out Vector2 extra) => MyMath.Clamp((extra = value - (value = MyMath.Clamp(value, min, max))) + value, min, max);
        
        public static byte Max(params byte[] values) => values.Max();
        public static sbyte Max(params sbyte[] values) => values.Max();
        public static short Max(params short[] values) => values.Max();
        public static ushort Max(params ushort[] values) => values.Max();
        public static int Max(params int[] values) => values.Max();
        public static uint Max(params uint[] values) => values.Max();
        public static long Max(params long[] values) => values.Max();
        public static ulong Max(params ulong[] values) => values.Max();
        public static float Max(params float[] values) => values.Max();
        public static double Max(params double[] values) => values.Max();
        public static char Max(params char[] values) => values.Max();
        public static decimal Max(params decimal[] values) => values.Max();

        public static byte Min(params byte[] values) => values.Min();
        public static sbyte Min(params sbyte[] values) => values.Min();
        public static short Min(params short[] values) => values.Min();
        public static ushort Min(params ushort[] values) => values.Min();
        public static int Min(params int[] values) => values.Min();
        public static uint Min(params uint[] values) => values.Min();
        public static long Min(params long[] values) => values.Min();
        public static ulong Min(params ulong[] values) => values.Min();
        public static float Min(params float[] values) => values.Min();
        public static double Min(params double[] values) => values.Min();
        public static char Min(params char[] values) => values.Min();
        public static decimal Min(params decimal[] values) => values.Min();

        public static byte Sum(byte[] values, byte basic = (byte)0)
        {
            for (int i = 0; i < values.Length; i++) basic = (byte)(basic + values[i]);
            return basic;
        }
        public static byte Sum(byte basic = (byte)0, params byte[] values)
        {
            for (int i = 0; i < values.Length; i++) basic = (byte)(basic + values[i]);
            return basic;
        }
        public static sbyte Sum(sbyte[] values, sbyte basic = (sbyte)0)
        {
            for (int i = 0; i < values.Length; i++) basic = (sbyte)(basic + values[i]);
            return basic;
        }
        public static sbyte Sum(sbyte basic = (sbyte)0, params sbyte[] values)
        {
            for (int i = 0; i < values.Length; i++) basic = (sbyte)(basic + values[i]);
            return basic;
        }
        public static short Sum(short[] values, short basic = (short)0)
        {
            for (int i = 0; i < values.Length; i++) basic = (short)(basic + values[i]);
            return basic;
        }
        public static short Sum(short basic = (short)0, params short[] values)
        {
            for (int i = 0; i < values.Length; i++) basic = (short)(basic + values[i]);
            return basic;
        }
        public static ushort Sum(ushort[] values, ushort basic = (ushort)0)
        {
            for (int i = 0; i < values.Length; i++) basic = (ushort)(basic + values[i]);
            return basic;
        }
        public static ushort Sum(ushort basic = (ushort)0, params ushort[] values)
        {
            for (int i = 0; i < values.Length; i++) basic = (ushort)(basic + values[i]);
            return basic;
        }
        public static int Sum(int[] values, int basic = (int)0)
        {
            for (int i = 0; i < values.Length; i++) basic = (int)(basic + values[i]);
            return basic;
        }
        public static int Sum(int basic = (int)0, params int[] values)
        {
            for (int i = 0; i < values.Length; i++) basic = (int)(basic + values[i]);
            return basic;
        }
        public static uint Sum(uint[] values, uint basic = (uint)0)
        {
            for (int i = 0; i < values.Length; i++) basic = (uint)(basic + values[i]);
            return basic;
        }
        public static uint Sum(uint basic = (uint)0, params uint[] values)
        {
            for (int i = 0; i < values.Length; i++) basic = (uint)(basic + values[i]);
            return basic;
        }
        public static long Sum(long[] values, long basic = (long)0)
        {
            for (int i = 0; i < values.Length; i++) basic = (long)(basic + values[i]);
            return basic;
        }
        public static long Sum(long basic = (long)0, params long[] values)
        {
            for (int i = 0; i < values.Length; i++) basic = (long)(basic + values[i]);
            return basic;
        }
        public static ulong Sum(ulong[] values, ulong basic = (ulong)0)
        {
            for (int i = 0; i < values.Length; i++) basic = (ulong)(basic + values[i]);
            return basic;
        }
        public static ulong Sum(ulong basic = (ulong)0, params ulong[] values)
        {
            for (int i = 0; i < values.Length; i++) basic = (ulong)(basic + values[i]);
            return basic;
        }
        public static float Sum(float[] values, float basic = (float)0)
        {
            for (int i = 0; i < values.Length; i++) basic = (float)(basic + values[i]);
            return basic;
        }
        public static float Sum(float basic = (float)0, params float[] values)
        {
            for (int i = 0; i < values.Length; i++) basic = (float)(basic + values[i]);
            return basic;
        }
        public static double Sum(double[] values, double basic = (double)0)
        {
            for (int i = 0; i < values.Length; i++) basic = (double)(basic + values[i]);
            return basic;
        }
        public static double Sum(double basic = (double)0, params double[] values)
        {
            for (int i = 0; i < values.Length; i++) basic = (double)(basic + values[i]);
            return basic;
        }
        public static char Sum(char[] values, char basic = (char)0)
        {
            for (int i = 0; i < values.Length; i++) basic = (char)(basic + values[i]);
            return basic;
        }
        public static char Sum(char basic = (char)0, params char[] values)
        {
            for (int i = 0; i < values.Length; i++) basic = (char)(basic + values[i]);
            return basic;
        }
        public static decimal Sum(decimal[] values, decimal basic = (decimal)0)
        {
            for (int i = 0; i < values.Length; i++) basic = (decimal)(basic + values[i]);
            return basic;
        }
        public static decimal Sum(decimal basic = (decimal)0, params decimal[] values)
        {
            for (int i = 0; i < values.Length; i++) basic = (decimal)(basic + values[i]);
            return basic;
        }
    }
}