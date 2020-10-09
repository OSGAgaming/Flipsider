using Flipsider.Core;
using Flipsider.Graphics;
using Flipsider.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;

namespace Flipsider.Entities
{
    /// <summary>
    /// Represents a delegate that handles an entity.
    /// </summary>
    /// <param name="worldEntity">The entity in the world.</param>
    public delegate void EntityDelegate(WorldEntity worldEntity);
    /// <summary>
    /// Represents a delegate that handles an entity's drawing.
    /// </summary>
    /// <param name="worldEntity">The entity in the world.</param>
    /// <param name="safeSB">The given spritebatch.</param>
    public delegate void EntityDrawDelegate(WorldEntity worldEntity, SafeSpriteBatch safeSB);

    public sealed class EntitySystem : IUpdated, IDrawn
    {
        /// <summary>
        /// Maximum number of active entities allowed before the game crashes.
        /// </summary>
        internal const int MaxEntityCount = 10_000;

        internal EntitySystem(World world)
        {
            World = world;
        }

        /// <summary>
        /// Called when any entity is spawned using <see cref="Entity.SpawnInWorld"/>.
        /// </summary>
        public event EntityDelegate? OnSpawn;
        /// <summary>
        /// Called when any entity is updated.
        /// </summary>
        public event EntityDelegate? OnUpdate;
        /// <summary>
        /// Called when any entity is drawn.
        /// </summary>
        public event EntityDrawDelegate? OnDraw;
        /// <summary>
        /// Called when any entity is removed from the world.
        /// </summary>
        public event EntityDelegate? OnRemove;

        public IEnumerable<WorldEntity> Enumerable => entities.Values;

        public World World { get; }

        private int id;
        private readonly Dictionary<int, WorldEntity> entities = new Dictionary<int, WorldEntity>(100);
        private readonly Dictionary<int, Entity?> additions = new Dictionary<int, Entity?>(10);

        internal WorldEntity New(Entity entity)
        {
            if (entities.Count > MaxEntityCount)
            {
                throw new InvalidOperationException($"Somehow, a maximum number of entities was reached. Failed on #{id}: {entities[id]}.");
            }

            // Allocate the entity to current (open) ID
            int entId = id;
            additions[entId] = entity;

            // Go to next open ID
            do
            {
                unchecked { id++; }
            }
            while (entities.ContainsKey(id));

            return new WorldEntity(entId, World, entity);
        }

        internal void Remove(int id)
        {
            additions[id] = null;
        }

        /// <summary>
        /// Tries to get an entity with the matching ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="entity">The entity, if any.</param>
        /// <returns>True for success, false for failure.</returns>
        public bool TryGet(int id, [MaybeNullWhen(false)] out WorldEntity entity)
        {
            return entities.TryGetValue(id, out entity);
        }

        /// <summary>
        /// Gets an entity with the matching ID, if any.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The matching entity.</returns>
        public WorldEntity Get(int id)
        {
            return entities[id];
        }

        void IUpdated.Update()
        {
            RefreshCollections();

            // Update existing entities
            foreach (var item in entities.Values)
            {
                try
                {
                    item.Entity.CallUpdate(item);
                    OnUpdate?.Invoke(item);
                }
                catch (Exception e)
                {
                    Logger.Warn($"Entity {item} threw an exception while updating. {e}");
                    additions[item.ID] = null;
                }
            }

            RefreshCollections();
        }

        private void RefreshCollections()
        {
            lock (entities)
            {
                foreach (var kvp in additions)
                {
                    Entity? entity = kvp.Value;
                    if (entity != null && !entities.ContainsKey(kvp.Key))
                    {
                        entities[kvp.Key] = new WorldEntity(kvp.Key, World, entity);
                        entity.CallSpawn(entities[kvp.Key]);
                        OnSpawn?.Invoke(entities[kvp.Key]);
                    }
                    else if (entities.ContainsKey(kvp.Key))
                    {
                        entities[kvp.Key].Entity.CallRemove(entities[kvp.Key]);
                        OnRemove?.Invoke(entities[kvp.Key]);
                        entities.Remove(kvp.Key);
                    }
                }

                additions.Clear();
            }
        }

        void IDrawn.Draw(SafeSpriteBatch spriteBatch)
        {
            // Draw entities
            foreach (var item in entities.Values)
            {
                try
                {
                    item.Entity.CallDraw(item, spriteBatch);
                    OnDraw?.Invoke(item, spriteBatch);
                }
                catch (Exception e)
                {
                    Logger.Warn($"Entity {item} threw an exception while drawing. {e}");
                    additions[item.ID] = null;
                }
            }
        }
    }
}
