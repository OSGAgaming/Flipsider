using Flipsider.Core;
using Flipsider.Entities;
using Microsoft.Xna.Framework;

namespace Flipsider.Collision
{
    /// <summary>
    /// Represents an object that can be registered into a world's <see cref="CollisionSystem"/> and have collisions calculated on its behalf.
    /// </summary>
    public interface ICollideable
    {
        /// <summary>
        /// The maximum and minimum bounds for the object.
        /// </summary>
        RectangleF Bounds { get; }
    }
}