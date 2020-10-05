using Microsoft.Xna.Framework;

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

        /// <summary>
        /// Represents a rectangle that spans infinitely in all directions.
        /// </summary>
        public static RectangleF Plane { get; } =
            new RectangleF { x = float.NegativeInfinity, y = float.NegativeInfinity, w = float.PositiveInfinity, h = float.PositiveInfinity };
    }
}