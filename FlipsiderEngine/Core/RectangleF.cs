using Microsoft.Xna.Framework;
using System;

namespace Flipsider.Core
{
    public struct RectangleF
    {
        public float x;
        public float y;
        public float w;
        public float h;

        public Vector2 TR => TL + Vector2.UnitX * w;
        public Vector2 BL => TL + Vector2.UnitY * h;
        public Vector2 TL => new Vector2(x, y);
        public Vector2 BR => new Vector2(x + w, y + h);

        public Vector2 Size
        {
            get => new Vector2(w, h);
            set => (w, h) = value;
        }
        public Vector2 Center
        {
            get => TL + Size / 2;
            set => (x, y) = value - Size / 2;
        }

        public bool Intersects(RectangleF other)
        {
            bool isGap = other.x > x + w || other.x + other.w < x || other.y > y + h || other.y + other.h < y;
            return !isGap;
        }

        public RectangleF? Intersection(RectangleF other)
        {
            if (!Intersects(other))
                return null;
            var ret = new RectangleF { x = Math.Max(x, other.x), y = Math.Max(y, other.y) };
            ret.w = Math.Min(x + w, other.x + other.w) - ret.x; 
            ret.h = Math.Min(y + h, other.y + other.h) - ret.y;
            return ret;
        }

        /// <summary>
        /// Represents a rectangle that spans infinitely in all directions.
        /// </summary>
        public static RectangleF Plane { get; } =
            new RectangleF { x = float.NegativeInfinity, y = float.NegativeInfinity, w = float.PositiveInfinity, h = float.PositiveInfinity };
    }
}