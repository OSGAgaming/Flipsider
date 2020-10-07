using Microsoft.Xna.Framework;

namespace Flipsider.Collision
{
    /// <summary>
    /// Represents an intersection between two <see cref="ICollideable"/> objects.
    /// </summary>
    public struct Intersection
    {
        private Intersection(Vector2? offset)
        {
            Offset = offset;
        }

        /// <summary>
        /// Gets an intersection that represents no collision.
        /// </summary>
        public static Intersection None { get; } = new Intersection(null);
        /// <summary>
        /// Gets an intersection that represents a collision.
        /// </summary>
        /// <param name="offset">Points to the other collideable from this one.</param>
        public static Intersection From(Vector2 offset) => new Intersection(offset);

        /// <summary>
        /// The collision offset, or null if no collision occurred.
        /// </summary>
        internal readonly Vector2? Offset;
    }
}