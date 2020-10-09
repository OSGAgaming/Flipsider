namespace Flipsider.Collision
{
    /// <summary>
    /// Represents an object that can be registered into a world's <see cref="CollisionSystem"/> and observe collisions between <see cref="ICollideable"/> objects.
    /// </summary>
    public interface ICollisionObserver : ICollideable
    {
        /// <summary>
        /// Called when there is an intersection between an ICollideable and an <see cref="ICollisionObserver"/>'s bounds.
        /// </summary>
        /// <param name="other">The other collideable. Do not call <see cref="Intersect(ICollideable)"/> on <paramref name="other"/>, or infinite loops may occur.</param>
        void Intersect(ICollideable other);
    }
}