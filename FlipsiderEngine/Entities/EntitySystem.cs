using Flipsider.Assets;
using Flipsider.Core;
using Flipsider.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;

namespace Flipsider.Entities
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
        private readonly Dictionary<int, Entity> entities = new Dictionary<int, Entity>(100);
        private readonly Dictionary<Entity, bool> additions = new Dictionary<Entity, bool>(10);

        internal int New(Entity entity)
        {
            if (entities.Count > MaxEntityCount)
            {
                throw new InvalidOperationException($"Somehow, a maximum number of entities was reached. Failed on #{id}: {entities[id]}.");
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
            RefreshCollections();

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

            RefreshCollections();
        }

        private void RefreshCollections()
        {
            lock (entities)
            {
                foreach (var kvp in additions)
                {
                    Entity entity = kvp.Key;
                    if (kvp.Value)
                    {
                        entities[entity.ID] = entity;
                        entity.CallSpawn(id, World);
                        OnSpawn?.Invoke(entity, id, World);
                    }
                    else
                    {
                        entity.CallRemove();
                        OnRemove?.Invoke(entity);
                        entities.Remove(entity.ID);
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
