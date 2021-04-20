using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTR
{
    public class Vector //Просто так
    {
        public float X { get; set; }
        public float Y { get; set; }
        public Vector()
        {
            X = Zero().X;
            Y = Zero().Y;
        }
        public Vector(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vector(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public static Vector Zero()
        {
            return new Vector(0, 0);
        }
    }
}
