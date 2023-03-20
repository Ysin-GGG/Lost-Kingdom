using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace LostKingdom.Utils
{
    /// <summary> 数学工具库 </summary>
    public static class MyMath
    {
        /// <summary> 纯小数黄金分割数（双精度） </summary>
        public const double Phi0 = 0.6180339887498948;
        /// <summary> 纯小数黄金分割数（双精度） </summary>
        public const double Phi1 = 1.6180339887498948;
        /// <summary> 重力加速度（双精度） </summary>
        public const double G = 9.8061905036721905;
        /// <summary> 自然对数（双精度） </summary>
        public const double E = 2.7182818284590451;
        /// <summary> 双精度圆周率（180°） </summary>
        public const double Pi = 3.1415926535897931;
        /// <summary> 双精度圆弧度（360°） </summary>
        public const double Pi2 = MyMath.Pi * 2;
        /// <summary> 双精度角度制（1°） </summary>
        public const double Deg01 = MyMath.Pi / 180;
        /// <summary> 双精度角度制（15°） </summary>
        public const double Deg15 = MyMath.Pi / 12;
        /// <summary> 双精度角度制（30°） </summary>
        public const double Deg30 = MyMath.Pi / 6;
        /// <summary> 双精度角度制（45°） </summary>
        public const double Deg45 = MyMath.Pi / 4;
        /// <summary> 双精度角度制（60°） </summary>
        public const double Deg60 = MyMath.Pi / 3;
        /// <summary> 双精度角度制（90°） </summary>
        public const double Deg90 = MyMath.Pi / 2;
        /// <summary> 双精度角度制（105°） </summary>
        public const double Deg105 = MyMath.Deg90 + MyMath.Deg15;
        /// <summary> 双精度角度制（120°） </summary>
        public const double Deg120 = MyMath.Deg90 + MyMath.Deg30;
        /// <summary> 双精度角度制（135°） </summary>
        public const double Deg135 = MyMath.Deg90 + MyMath.Deg45;
        /// <summary> 双精度角度制（150°） </summary>
        public const double Deg150 = MyMath.Deg90 + MyMath.Deg60;
        /// <summary> 纯小数黄金分割数（单精度） </summary>
        public const float phi0 = 0.6180339f;
        /// <summary> 带小数黄金分割数（单精度） </summary>
        public const float phi1 = 1.6180339f;
        /// <summary> 重力加速度（单精度） </summary>
        public const float g = 9.8066500f;
        /// <summary> 自然对数（单精度） </summary>
        public const float e = 2.7182818f;
        /// <summary> 单精度圆周率（180°） </summary>
        public const float pi = 3.1415927f;
        /// <summary> 单精度圆弧度（360°） </summary>
        public const float pi2 = MyMath.pi * 2;
        /// <summary> 单精度角度制（1°） </summary>
        public const float deg01 = MyMath.pi / 180;
        /// <summary> 单精度角度制（15°） </summary>
        public const float deg15 = MyMath.pi / 12;
        /// <summary> 单精度角度制（30°） </summary>
        public const float deg30 = MyMath.pi / 6;
        /// <summary> 单精度角度制（45°） </summary>
        public const float deg45 = MyMath.pi / 4;
        /// <summary> 单精度角度制（60°） </summary>
        public const float deg60 = MyMath.pi / 3;
        /// <summary> 单精度角度制（90°） </summary>
        public const float deg90 = MyMath.pi / 2;
        /// <summary> 单精度角度制（105°） </summary>
        public const float deg105 = MyMath.deg90 + MyMath.deg15;
        /// <summary> 单精度角度制（120°） </summary>
        public const float deg120 = MyMath.deg90 + MyMath.deg30;
        /// <summary> 单精度角度制（135°） </summary>
        public const float deg135 = MyMath.deg90 + MyMath.deg45;
        /// <summary> 单精度角度制（150°） </summary>
        public const float deg150 = MyMath.deg90 + MyMath.deg60;
        /// <summary> 向上取整 </summary>
        /// <param name="value"> 原值 </param>
        public static int ceiling(double value) => (int)value;
        /// <summary> 向下取整 </summary>
        /// <param name="value"> 原值 </param>
        public static int floor(double value) => (int)value;
        /// <summary> 正弦函数 </summary>
        /// <param name="degree"> 角度 </param>
        public static float sin(double degree) => (float)Math.Sin(degree);
        /// <summary> 余弦函数 </summary>
        /// <param name="degree"> 角度 </param>
        public static float cos(double degree) => (float)Math.Cos(degree);
        /// <summary> 正切函数 </summary>
        /// <param name="degree"> 角度 </param>
        public static float tan(double degree) => (float)Math.Tan(degree);
        /// <summary> 幂函数 </summary>
        /// <param name="x"> 自变量 </param>
        /// <param name="a"> 幂 </param>
        public static float pow(double x, double a) => (float)Math.Pow(x, a);
        /// <summary> 对数函数 </summary>
        /// <param name="x"> 自变量 </param>
        /// <param name="a"> 对数 </param>
        public static float log(double x, double a = MyMath.E) => (float)(Math.Log10(x) / Math.Log10(a));
        /// <summary> 向量转长度 </summary>
        /// <param name="x"> 横坐标 </param>
        /// <param name="y"> 纵坐标 </param>
        public static float GetZ(double x, double y) => (float)Math.Sqrt(x * x + y * y);
        /// <summary> 角度转单位向量 </summary>
        /// <param name="degree"> 角度 </param>
        public static Vector2 GetXY(double degree) => new Vector2(MyMath.cos(degree), MyMath.sin(degree));
        /// <summary> 线性插值 </summary>
        /// <param name="value1"> 值1 </param>
        /// <param name="value2"> 值2 </param>
        /// <param name="weight"> 权重（取值范围[0, 1]） </param>
        public static int Lerp(int value1, int value2, float weight) => (int)(value1 * (1f - MyMath.Clamp(weight, 0, 1)) + value2 * MyMath.Clamp(weight, 0, 1));
        /// <summary> 线性插值 </summary>
        /// <param name="value1"> 值1 </param>
        /// <param name="value2"> 值2 </param>
        /// <param name="weight"> 权重（取值范围[0, 1]） </param>
        public static float Lerp(float value1, float value2, float weight) => (float)(value1 * (1f - MyMath.Clamp(weight, 0, 1)) + value2 * MyMath.Clamp(weight, 0, 1));
        /// <summary> 线性插值 </summary>
        /// <param name="value1"> 值1 </param>
        /// <param name="value2"> 值2 </param>
        /// <param name="weight"> 权重（取值范围[0, 1]） </param>
        public static Point Lerp(Point value1, Point value2, float weight) => MyMath.Lerp(value1, value2, new Vector2(weight));
        /// <summary> 线性插值 </summary>
        /// <param name="value1"> 值1 </param>
        /// <param name="value2"> 值2 </param>
        /// <param name="weight"> 权重（取值范围[0, 1]） </param>
        public static Vector2 Lerp(Vector2 value1, Vector2 value2, float weight) => MyMath.Lerp(value1, value2, new Vector2(weight));
        /// <summary> 线性插值 </summary>
        /// <param name="value1"> 值1 </param>
        /// <param name="value2"> 值2 </param>
        /// <param name="weight"> 权重（取值范围[0, 1]） </param>
        public static Point Lerp(Point value1, Point value2, Vector2 weight) => new Point((int)MyMath.Lerp(value1.X, value2.X, weight.X), (int)MyMath.Lerp(value1.Y, value2.Y, weight.Y));
        /// <summary> 线性插值 </summary>
        /// <param name="value1"> 值1 </param>
        /// <param name="value2"> 值2 </param>
        /// <param name="weight"> 权重（取值范围[0, 1]） </param>
        public static Vector2 Lerp(Vector2 value1, Vector2 value2, Vector2 weight) => new Vector2((float)MyMath.Lerp(value1.X, value2.X, weight.X), (float)MyMath.Lerp(value1.Y, value2.Y, weight.Y));
        /// <summary> 返回范围内的值（闭区间） </summary>
        /// <param name="value"> 原值 </param>
        /// <param name="min"> 下限 </param>
        /// <param name="max"> 上限 </param>
        public static int Clamp(int value, int min, int max) => MyMath.Min(MyMath.Max(min, value), max);
        /// <summary> 返回范围内的值（闭区间） </summary>
        /// <param name="value"> 原值 </param>
        /// <param name="min"> 下限 </param>
        /// <param name="max"> 上限 </param>
        /// <param name="extra"> 差值 </param>
        public static int Clamp(int value, int min, int max, out int extra) => MyMath.Clamp((extra = value - (value = MyMath.Clamp(value, min, max))) + value, min, max);
        /// <summary> 返回范围内的值（闭区间） </summary>
        /// <param name="value"> 原值 </param>
        /// <param name="min"> 下限 </param>
        /// <param name="max"> 上限 </param>
        public static uint Clamp(uint value, uint min, uint max) => MyMath.Min(MyMath.Max(min, value), max);
        /// <summary> 返回范围内的值（闭区间） </summary>
        /// <param name="value"> 原值 </param>
        /// <param name="min"> 下限 </param>
        /// <param name="max"> 上限 </param>
        /// <param name="extra"> 差值 </param>
        public static uint Clamp(uint value, uint min, uint max, out uint extra) => MyMath.Clamp((extra = value - (value = MyMath.Clamp(value, min, max))) + value, min, max);
        /// <summary> 返回范围内的值（闭区间） </summary>
        /// <param name="value"> 原值 </param>
        /// <param name="min"> 下限 </param>
        /// <param name="max"> 上限 </param>
        public static float Clamp(float value, float min, float max) => MyMath.Min(MyMath.Max(min, value), max);
        /// <summary> 返回范围内的值（闭区间） </summary>
        /// <param name="value"> 原值 </param>
        /// <param name="min"> 下限 </param>
        /// <param name="max"> 上限 </param>
        /// <param name="extra"> 差值 </param>
        public static float Clamp(float value, float min, float max, out float extra) => MyMath.Clamp((extra = value - (value = MyMath.Clamp(value, min, max))) + value, min, max);
        /// <summary> 返回范围内的值（闭区间） </summary>
        /// <param name="value"> 原值 </param>
        /// <param name="min"> 下限 </param>
        /// <param name="max"> 上限 </param>
        public static double Clamp(double value, double min, double max) => MyMath.Min(MyMath.Max(min, value), max);
        /// <summary> 返回范围内的值（闭区间） </summary>
        /// <param name="value"> 原值 </param>
        /// <param name="min"> 下限 </param>
        /// <param name="max"> 上限 </param>
        /// <param name="extra"> 差值 </param>
        public static double Clamp(double value, double min, double max, out double extra) => MyMath.Clamp((extra = value - (value = MyMath.Clamp(value, min, max))) + value, min, max);
        /// <summary> 返回范围内的值（闭区间） </summary>
        /// <param name="value"> 原值 </param>
        /// <param name="min"> 下限 </param>
        /// <param name="max"> 上限 </param>
        public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max) => new Vector2(MyMath.Clamp(value.X, min.X, max.X), MyMath.Clamp(value.Y, min.Y, max.Y));
        /// <summary> 返回范围内的值（闭区间） </summary>
        /// <param name="value"> 原值 </param>
        /// <param name="min"> 下限 </param>
        /// <param name="max"> 上限 </param>
        /// <param name="extra"> 差值 </param>
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