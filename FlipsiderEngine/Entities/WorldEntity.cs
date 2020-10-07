using Flipsider.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flipsider.Entities
{
    /// <summary>
    /// Represents an entity in a world.
    /// </summary>
    public sealed class WorldEntity
    {
        internal WorldEntity(int id, World world, Entity entity)
        {
            ID = id;
            World = world;
            Entity = entity;
        }

        /// <summary>
        /// The ID of the world entity.
        /// </summary>
        public int ID { get; }
        /// <summary>
        /// The entity instance itself.
        /// </summary>
        public Entity Entity { get; }
        /// <summary>
        /// The world that the entity is contained within.
        /// </summary>
        public World World { get; }

        public override bool Equals(object? obj)
        {
            return obj is WorldEntity entity &&
                   ID == entity.ID &&
                   ReferenceEquals(Entity, entity.Entity);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID, Entity);
        }

        public static bool operator ==(WorldEntity? left, WorldEntity? right)
        {
            return left?.Equals(right) ?? right is null;
        }

        public static bool operator !=(WorldEntity? left, WorldEntity? right)
        {
            return !(left == right);
        }
    }
}
