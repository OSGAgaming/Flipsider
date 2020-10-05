using Flipsider.Core;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Flipsider.Worlds.Collision
{
    /// <summary>
    /// Represents an object that can be registered into a world's <see cref="CollisionSystem"/> and exprience collisions.
    /// </summary>
    public interface ICollideable
    {
        /// <summary>
        /// The maximum and minimum bounds for the object.
        /// </summary>
        /// <returns></returns>
        RectangleF Bounds { get; }

        /// <summary>
        /// Gets the possible intersection with another <see cref="ICollideable"/>. 
        /// Implementors should return <see cref="Intersection.None"/> for an invalid collision, 
        /// <see cref="Intersection.None"/> for no collision, and <see cref="Intersection.From(Vector2)"/> for valid collisions.
        /// </summary>
        /// <param name="other">The other collideable. Do not call <see cref="Intersect(ICollideable)"/> on <paramref name="other"/>, or infinite loops may occur.</param>
        void Intersect(ICollideable other);
    }
}