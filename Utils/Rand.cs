using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostKingdom.Utils
{
    public class Rand
    {
        private ulong seed = 0;
        private byte head = 0;
        private byte tail = 127;
        private ulong[] card = new ulong[256];

        public Rand(ulong seed = 0)
        {
            if (seed == 0) this.seed = (ulong)DateTime.Now.Ticks;
            else this.seed = seed;
            ulong ii = 1, jj = this.seed;
            this.head = 0;
            this.tail = 127;
            this.card[255] = jj;
            for (byte i, j = 255; j > 0; j--)
            {
                i = (byte)(127 * j);
                this.card[i] = ii;
                ii = jj - ii;
                jj = this.card[i];
            }
            for (byte i = 255; i > 0; i--)
                this.card[i] -= this.card[(byte)(i + 127)];
            for (byte i = 127; i > 0; i--)
                this.Next(0, 0);
        }

        public double Next(double add = 0.0d, double mul = 1.0d)
        {
            while (++this.head == 0);
            while (++this.tail == 0);
            return (this.card[this.head] -= this.card[this.tail]) / (double)ulong.MaxValue * mul + add;
        }
        
        public int NextInt(int min = int.MinValue, int max = int.MaxValue) => (int)(this.Next(0.0d, min) + this.Next(0.0d, max - min));
        public float NextFloat(float min = float.MinValue, float max = float.MaxValue) => (float)(this.Next(0.0d, min) + this.Next(0.0d, max));
        public double NextDouble(double min = double.MinValue, double max = double.MaxValue) => (double)(this.Next(0.0d, min) + this.Next(0.0d, max));
        public bool NextBool(int den) => this.Next(0.0d, 1.0d) < 1.0d / MyMath.Max(1, den);
        public bool NextBool(double chance = 0.5d) => this.Next(0.0d, 1.0d) < chance;
    }
}
