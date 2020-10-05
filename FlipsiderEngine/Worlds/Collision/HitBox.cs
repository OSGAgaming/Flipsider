using Flipsider.Core;
using Flipsider.Worlds.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Flipsider.Worlds.Collision
{
    /// <summary>
    /// Used as a callback for when an ICollideable object has a collision.
    /// </summary>
    /// <param name="other">The other entity that was collided with.</param>
    /// <param name="offset">The offset to get to <paramref name="other" />.</param>
    public delegate void OnCollideDelegate(ICollideable other);

    /// <summary>
    /// Represents a collideable hitbox.
    /// </summary>
    public sealed class HitBox : ICollideable
    {
        /// <summary>
        /// The object that registered this hitbox.
        /// </summary>
        public object Owner { get; }

        private readonly OnCollideDelegate callback;
        private readonly Func<RectangleF> rectFunc;

        public HitBox(object owner, OnCollideDelegate callback, Func<RectangleF> rect)
        {
            Owner = owner;
            this.callback = callback;
            rectFunc = rect;
        }

        /// <summary>
        /// The hitbox bounds.
        /// </summary>
        public RectangleF Bounds => rectFunc();

        public void Intersect(ICollideable other)
        {
            if (other is HitBox box)
            {
                RectangleF rect = box.Bounds;
                var intersects = Bounds.TL.X < rect.BR.X && rect.TL.X < Bounds.BR.X && Bounds.TL.Y < rect.BR.Y && rect.TL.Y < Bounds.BR.Y;
                if (intersects)
                {
                    callback(other);
                }
            }
        }
    }
}