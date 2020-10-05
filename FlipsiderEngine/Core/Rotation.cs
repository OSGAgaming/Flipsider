using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Flipsider.Core
{
    public struct Rotation
    {
        private double rad;

        /// <summary>
        /// The rotation in radians, as a double from -pi to +pi. Positive is counter-clockwise.
        /// </summary>
        public double Rad 
        { 
            get => rad; 
            set => rad = WrapAngle(value); 
        }
        /// <summary>
        /// The rotation in radians, as a float from -pi to +pi. Positive is counter-clockwise.
        /// </summary>
        public float RadF => (float)rad;

        /// <summary>
        /// Gets the difference between two rotations. Positive return values indicate that <paramref name="other"/> is more counterclockwise than <see langword="this"/>.
        /// </summary>
        /// <param name="other">A second rotation angle to compare to.</param>
        /// <returns>The difference between the two rotations.</returns>
        public double Difference(Rotation other)
        {
            double a = rad, b = other.rad;

            double d = Math.Abs(a - b) % (Math.PI * 2);
            double r = d > Math.PI ? Math.PI*2 - d : d;

            //calculate sign 
            int sign = (a - b >= 0 && a - b <= Math.PI) || (a - b <= -Math.PI && a - b >= -Math.PI*2) ? 1 : -1;
            return r * sign;
        }

        public override bool Equals(object? obj)
        {
            return obj is double d && d == Rad ||
                    obj is Rotation r && r.Rad == Rad;
        }

        public override int GetHashCode()
        {
            return Rad.GetHashCode();
        }

        public static bool operator ==(Rotation left, Rotation right) => left.Equals(right);

        public static bool operator !=(Rotation left, Rotation right) => !(left == right);

        public static implicit operator double(Rotation r) => r.rad;
        public static implicit operator Rotation(double r) => new Rotation { rad = r };

        static double WrapAngle(double angle)
        {
            if (angle > -Math.PI && angle <= Math.PI)
            {
                return angle;
            }
            angle %= Math.PI*2;
            if (angle <= -Math.PI)
            {
                return angle + Math.PI*2;
            }
            if (angle > Math.PI)
            {
                return angle - Math.PI*2;
            }
            return angle;
        }
    }
}
