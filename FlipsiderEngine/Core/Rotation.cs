using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Flipsider.Core
{
    public struct Rotation
    {
        public Rotation(double radians) => Rad = WrapAngle(radians);

        public static Rotation FromDegrees(double degrees) => new Rotation(degrees * Math.PI / 180);
        public static Rotation FromRadians(double radians) => new Rotation(radians);
        public static Rotation FromVector(Vector2 vector) => new Rotation(Math.Atan2(vector.Y, vector.X));
        public Vector2 ToUnitVector() => new Vector2((float)Math.Cos(Rad), (float)Math.Sin(Rad));

        /// <summary>
        /// The rotation in degrees, in the range -180 to +180. Positive is counter-clockwise.
        /// </summary>
        public double Deg => Rad * 180 / Math.PI;
        /// <summary>
        /// Float cast for <see cref="Deg"/>.
        /// </summary>
        public float DegF => (float)Deg;

        /// <summary>
        /// The rotation in radians, in the range -pi to +pi. Positive is counter-clockwise.
        /// </summary>
        public double Rad { get; }
        /// <summary>
        /// Float cast for <see cref="Rad"/>.
        /// </summary>
        public float RadF => (float)Rad;

        public override bool Equals(object? obj)
        {
            return obj is double d && d == Rad ||
                    obj is Rotation r && r.Rad == Rad;
        }

        public override int GetHashCode()
        {
            return Rad.GetHashCode();
        }

        public static Rotation operator +(Rotation left, Rotation right) => FromRadians(left.Rad + right.Rad);
        public static Rotation operator -(Rotation left, Rotation right) => FromDegrees(left.Rad - right.Rad);

        public static bool operator ==(Rotation left, Rotation right) => left.Equals(right);
        public static bool operator !=(Rotation left, Rotation right) => !(left == right);

        public static double WrapAngle(double angle)
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
