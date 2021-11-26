using Microsoft.Xna.Framework;
using System;

namespace FlipEngine
{
    [Serializable]
    public struct RectangleF
    {
        public float x;
        public float y;
        public float width;
        public float height;
        public RectangleF(float x, float y, float w, float h)
        {
            this.x = x;
            this.y = y;
            this.width = w;
            this.height = h;
        }
        public RectangleF(Vector2 XY, Vector2 WH)
        {
            x = XY.X;
            y = XY.Y;
            width = WH.X;
            height = WH.Y;
        }
        public Vector2 TR => TL + Vector2.UnitX * width;
        public Vector2 BL => TL + Vector2.UnitY * height;
        public Vector2 TL => new Vector2(x, y);
        public Vector2 BR => new Vector2(x + width, y + height);

        public float right => x + width;
        public float bottom => y + height;

        public Vector2 Size
        {
            get => new Vector2(width, height);
            set => (width, height) = value;
        }
        public Vector2 Center
        {
            get => TL + Size / 2;
            set => (x, y) = value - Size / 2;
        }

        public static implicit operator Rectangle(RectangleF d) => new Rectangle(d.TL.ToPoint(), d.Size.ToPoint());
        public static implicit operator RectangleF(Rectangle d) => new RectangleF(d.Location.ToVector2(), d.Size.ToVector2());

        /// <summary>
        /// Represents a rectangle that spans infinitely in all directions.
        /// </summary>
        public static RectangleF Plane { get; } =
            new RectangleF { x = float.NegativeInfinity, y = float.NegativeInfinity, width = float.PositiveInfinity, height = float.PositiveInfinity };
    }
}
