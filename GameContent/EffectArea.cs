using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LostKingdom.Utils;

namespace LostKingdom.GameContent
{
    public struct EffectArea
    {
        public bool[,] Range { get; private set; }
        public ushort Width { get; private set; }
        public ushort Height { get; private set; }
        public ushort Count => (ushort)(this.Width * this.Height);
        private string visualRange;
        private static EffectArea instance = new EffectArea();
        private Style style;
        private enum Style
        {
            /// <summary> 自定 </summary>
            Custom,
            /// <summary> 线形 </summary>
            Line,
            /// <summary> 十字形 </summary>
            Cross,
            /// <summary> 三角形 </summary>
            Triangle,
            /// <summary> 矩形 </summary>
            Rectangle,
            /// <summary> 圆形 </summary>
            Circle,
            /// <summary> 扇形 </summary>
            Sector,
            /// <summary> 棱形 </summary>
            Prismatic,
        }

        public EffectArea(ushort width, ushort height)
        {
            this.style = Style.Custom;
            this.Width = 0;
            this.Height = 0;
            this.Range = null;
            this.visualRange = null;
            this.Resize(width, height);
        }

        public EffectArea(bool[,] range) : this((ushort)range.GetLength(0), (ushort)range.GetLength(1))
        {
            this.Range = range;
        }

        public EffectArea(byte[,] range) : this((ushort)range.GetLength(0), (ushort)range.GetLength(1))
        {
            this.SetRange(range);
        }

        /// <summary> 以文字形式展现范围（仅限调试使用） </summary>
        public override string ToString()
        {
            if (this.visualRange == null)
            {
                int capacity = (int)(this.Count + this.Height);
                StringBuilder builder = new StringBuilder(capacity, capacity);
                for (int i = 0; i < this.Height; i++)
                {
                    for (int j = 0; j < this.Width; j++)
                        builder.Append(this.Range[j, i] ? '口' : '×');
                    builder.Append('\n');
                }
                this.visualRange = builder.ToString();
            }
            return this.visualRange;
        }

        /// <summary> 线形 </summary>
        public static EffectArea Line(ushort length)
        {
            EffectArea.instance.style = Style.Line;
            EffectArea.instance.Resize(1, length);
            while (length-- > 0) EffectArea.instance.Range[0, length] = true;
            return EffectArea.instance;
        }

        /// <summary> 十字形 </summary>
        public static EffectArea Cross(ushort length)
        {
            ushort _length = length;
            length <<= 1;
            length++;
            EffectArea.instance.style = Style.Cross;
            EffectArea.instance.Resize(length, length);
            for (int i = 0; i < length; i++) EffectArea.instance.Range[i, _length] = EffectArea.instance.Range[_length, i] = true;
            EffectArea.instance.Range[_length, _length] = false;
            return EffectArea.instance;
        }

        /// <summary> 三角形 </summary>
        public static EffectArea Triangle(ushort length)
        {
            ushort _length = length;
            length <<= 1;
            EffectArea.instance.style = Style.Triangle;
            EffectArea.instance.Resize(--length, _length);
            for (int i = 0; i < EffectArea.instance.Width; i++)
                for (int j = 0; j < EffectArea.instance.Height; j++)
                    if (Math.Abs(i - _length + 1) < _length - j)
                        EffectArea.instance.Range[i, j] = true;
            return EffectArea.instance;
        }

        /// <summary> 矩形 </summary>
        public static EffectArea Rectangle(ushort width, ushort height)
        {
            EffectArea.instance.style = Style.Rectangle;
            EffectArea.instance.Resize(width, height);
            for (int i = 0; i < EffectArea.instance.Width; i++)
                for (int j = 0; j < EffectArea.instance.Height; j++)
                    EffectArea.instance.Range[i, j] = true;
            return EffectArea.instance;
        }

        /// <summary> 圆形 </summary>
        public static EffectArea Circle(ushort radius)
        {
            ushort _radius = radius;
            radius <<= 1;
            radius++;
            EffectArea.instance.style = Style.Circle;
            EffectArea.instance.Resize(radius, radius);
            Vector2 pos = new Vector2();
            for (int i = 0; i < EffectArea.instance.Width; i++)
                for (int j = 0; j < EffectArea.instance.Height; j++)
                {
                    pos.X = i - _radius;
                    pos.Y = j - _radius;
                    if (pos.Length() <= _radius)
                        EffectArea.instance.Range[i, j] = true;
                }
            EffectArea.instance.Range[_radius, _radius] = false;
            return EffectArea.instance;
        }

        /// <summary> 棱形 </summary>
        public static EffectArea Prismatic(ushort length)
        {
            ushort _length = length;
            length <<= 1;
            length++;
            EffectArea.instance.style = Style.Prismatic;
            EffectArea.instance.Resize(length, length);
            for (int i = 0; i < EffectArea.instance.Width; i++)
                for (int j = 0; j < EffectArea.instance.Height; j++)
                    if (Math.Abs(i - _length) <= _length - Math.Abs(j - _length))
                        EffectArea.instance.Range[i, j] = true;
            EffectArea.instance.Range[_length, _length] = false;
            return EffectArea.instance;
        }

        /// <summary> 顺时针旋转90° </summary>
        /// <param name="area"> 区域 </param>
        public static EffectArea operator ++(EffectArea area)
        {
            if (area.style == Style.Cross || area.style == Style.Circle || area.style == Style.Prismatic) return area;
            return EffectArea.instance;
        }

        /// <summary> 逆时针旋转90° </summary>
        /// <param name="area"> 区域 </param>
        public static EffectArea operator --(EffectArea area)
        {
            if (area.style == Style.Cross || area.style == Style.Circle || area.style == Style.Prismatic) return area;
            return EffectArea.instance;
        }

        /// <summary> 取反 </summary>
        /// <param name="area"> 区域 </param>
        public static EffectArea operator !(EffectArea area)
        {
            EffectArea.instance.Range = new bool[area.Width, area.Height];
            for (int i = 0; i < area.Width; i++)
                for (int j = 0; j < area.Height; j++)
                    EffectArea.instance.Range[i, j] = !area.Range[i, j];
            return EffectArea.instance;
        }

        /// <summary> 扩大范围 </summary>
        /// <param name="area"> 区域 </param>
        /// <param name="length"> 长度（正数则赋为true，负数赋为false） </param>
        public static EffectArea operator +(EffectArea area, short length)
        {
            bool positive = length > 0;
            switch(area.style)
            {
                case Style.Line:
                    EffectArea.instance = EffectArea.Line((ushort)(area.Height + Math.Abs(length)));
                    if (!positive)
                    {
                        length *= -1;
                        for (int i = 0; i < length; i++)
                            EffectArea.instance.Range[0, i] = false;
                    }
                    break;
                default:
                    if(!Main.IgnoreError) throw new ArgumentException("区域的类型不规则");
                    break;
            }
            return EffectArea.instance;
        }

        /// <summary> 缩小范围 </summary>
        /// <param name="area"> 区域 </param>
        /// <param name="length"> 长度（正数则赋为true，负数赋为false） </param>
        public static EffectArea operator -(EffectArea area, short length)
        {
            if (area.style == Style.Custom)
                if (!Main.IgnoreError) throw new ArgumentException("区域的类型不规则");
            return EffectArea.instance;
        }

        /// <summary> 并 </summary>
        /// <param name="area1"> 区域1 </param>
        /// <param name="area2"> 区域2 </param>
        public static EffectArea operator |(EffectArea area1, EffectArea area2)
            => EffectArea.instance.OpCheck(area1, area2, new Func<bool[,], bool[,], int, int, bool>((a, b, i, j) => a[i, j] || b[i, j]));

        /// <summary> 交 </summary>
        /// <param name="area1"> 区域1 </param>
        /// <param name="area2"> 区域2 </param>
        public static EffectArea operator &(EffectArea area1, EffectArea area2)
            => EffectArea.instance.OpCheck(area1, area2, new Func<bool[,], bool[,], int, int, bool>((a, b, i, j) => a[i, j] && b[i, j]));

        /// <summary> 差 </summary>
        /// <param name="area1"> 区域1 </param>
        /// <param name="area2"> 区域2 </param>
        public static EffectArea operator -(EffectArea area1, EffectArea area2)
            => EffectArea.instance.OpCheck(area1, area2, new Func<bool[,], bool[,], int, int, bool>((a, b, i, j) => a[i, j] && !b[i, j]));

        /// <summary> 异或 </summary>
        /// <param name="area1"> 区域1 </param>
        /// <param name="area2"> 区域2 </param>
        public static EffectArea operator ^(EffectArea area1, EffectArea area2)
            => EffectArea.instance.OpCheck(area1, area2, new Func<bool[,], bool[,], int, int, bool>((a, b, i, j) => a[i, j] != b[i, j]));

        private void SetRange(byte[,] range)
        {
            for (int i = 0; i < this.Width; i++)
                for (int j = 0; j < this.Height; j++)
                    this.Range[i, j] = Convert.ToBoolean(range[i, j]);
        }

        private void Resize(ushort width, ushort height)
        {
            this.Width = width;
            this.Height = height;
            int count = width * height;
            if (count > ushort.MaxValue)
                if (!Main.IgnoreError)
                    throw new ArgumentOutOfRangeException(nameof(this.Count), count, "Width和Height的乘积不得超过65535");
            this.Range = new bool[width, height];
        }

        private EffectArea OpCheck(EffectArea area1, EffectArea area2, Func<bool[,], bool[,], int, int, bool> op)
        {
            if (area1.style != area2.style && area1.style != Style.Custom)
                if (area1.Width != area1.Height || area1.Height != area2.Height)
                    if (!Main.IgnoreError)
                        throw new ArgumentException("两个区域的类型和宽高均不相同");
            int width = MyMath.Max(area1.Width, area2.Width);
            int height = MyMath.Max(area1.Height, area2.Height);
            this.style = Style.Custom;
            this.Range = new bool[width, height];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    this.Range[i, j] = op.Invoke(area1.Range, area2.Range, i, j);
            return this;
        }
    }
}