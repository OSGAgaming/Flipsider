using Flipsider.Core;
using Flipsider.Worlds.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;

namespace Flipsider.Worlds.Entities
{
    public delegate void OnSpawnGlobalDelegate(Entity entity, int entityID, World world);
    public delegate void OnUpdateGlobal(Entity entity);

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
        public event OnSpawnGlobalDelegate? OnSpawn;
        /// <summary>
        /// Called when any entity is updated.
        /// </summary>
        public event OnUpdateGlobal? OnUpdate;
        /// <summary>
        /// Called when any entity is removed from the world.
        /// </summary>
        public event OnUpdateGlobal? OnRemove;

        public IEnumerable<Entity> Enumerable => entities.Values;

        public World World { get; }

        private int id;
        private readonly Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
        private readonly Dictionary<Entity, bool> additions = new Dictionary<Entity, bool>();

        internal int New(Entity entity)
        {
            if (entities.Count > MaxEntityCount)
            {
                throw new InvalidOperationException($"Somehow, a maximum number of entities was reached. Failed on #{id}: {entities[id]}.");
            }
            if (entity.id.HasValue)
            {
                throw new InvalidOperationException($"Cannot spawn an entity that is already present in the world.");
            }

            // Allocate the entity to current (open) ID
            int entId = id;
            additions[entity] = true;

            // Go to next open ID
            do
            {
                unchecked { id++; }
            }
            while (entities.ContainsKey(id));

            return entId;
        }

        internal void Remove(Entity entity)
        {
            if (!entity.id.HasValue)
            {
                throw new InvalidOperationException("Cannot remove an entity that isn't present in the world.");
            }
            additions[entity] = false;
        }

        /// <summary>
        /// Tries to get an entity with the matching ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="entity">The entity, if any.</param>
        /// <returns>True for success, false for failure.</returns>
        public bool TryGet(int id, [MaybeNullWhen(false)] out Entity entity)
        {
            return entities.TryGetValue(id, out entity);
        }

        /// <summary>
        /// Gets an entity with the matching ID, if any.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The matching entity.</returns>
        public Entity? Get(int id)
        {
            entities.TryGetValue(id, out var entity);
            return entity;
        }

        void IUpdated.Update()
        {
            // Add new/remove old entities
            lock (entities)
            {
                foreach (var kvp in additions)
                {
                    Entity entity = kvp.Key;
                    if (kvp.Value)
                    {
                        int id = entity.id!.Value;
                        entities[id] = entity;
                        entity.CallSpawn(id, World);
                        OnSpawn?.Invoke(entity, id, World);
                    }
                    else
                    {
                        entity.CallRemove();
                        OnRemove?.Invoke(entity);
                        entities.Remove(entity.id!.Value);
                        entity.id = null;
                    }
                }
            }

            additions.Clear();

            // Update existing entities
            foreach (var item in entities.Values)
            {
                try
                {
                    item.CallUpdate();
                    OnUpdate?.Invoke(item);
                }
                catch (Exception e)
                {
                    Logger.Warn($"Entity {item} threw an exception while updating. {e}");
                    additions[item] = false;
                }
            }
        }

        void IDrawn.Draw(SafeSpriteBatch spriteBatch)
        {
            // Draw entities
            foreach (var item in entities.Values)
            {
                try
                {
                    item.CallDraw(spriteBatch);
                }
                catch (Exception e)
                {
                    Logger.Warn($"Entity {item} threw an exception while drawing. {e}");
                    additions[item] = false;
                }
            }
        }
    }
}
