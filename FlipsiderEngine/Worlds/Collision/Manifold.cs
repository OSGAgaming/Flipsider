using Microsoft.Xna.Framework;
using System;

namespace Flipsider.Worlds.Collision
{
    public struct Manifold
    {
        public static Manifold? None { get; } = null;

        public Manifold(Vector2 offset)
        {
            Offset = offset;
        }

        public Vector2 Offset { get; }

        public override bool Equals(object? obj)
        {
            return obj is Manifold manifold &&
                   Offset.Equals(manifold.Offset);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Offset);
        }

        public static bool operator ==(Manifold left, Manifold right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Manifold left, Manifold right)
        {
            return !(left == right);
        }
    }
}