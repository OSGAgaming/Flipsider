using Flipsider.Assets;
using Flipsider.Core;
using Flipsider.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flipsider.Entities
{
    public delegate void UpdateDelegate();
    public delegate void DrawDelegate(SafeSpriteBatch spriteBatch);
    public delegate void SpawnDelegate(int entityID, World world);

    public abstract class Entity
    {
        /// <summary>
        /// World that contains this entity.
        /// </summary>
        public World InWorld => inWorld ?? throw new InvalidOperationException("Entity is not spawned in any world.");

        /// <summary>
        /// ID of this entity.
        /// </summary>
        public int ID => id ?? throw new InvalidOperationException("Entity is not spawned in any world.");

        /// <summary>
        /// The center of the entity.
        /// </summary>
        public Vector2 Center;
        private World? inWorld;
        private int? id;

        /// <summary>
        /// Call this when the entity should be removed from the world.
        /// </summary>
        protected void Delete()
        {
            if (!id.HasValue) 
                return;
            InWorld.Entities.Remove(this);
        }

        /// <summary>
        /// If this entity is not already spawned, this spawns the entity in the current world and sets its ID value.
        /// </summary>
        /// <returns>True if the entity was successfully spawned; false if the entity was already spawned prior.</returns>
        public bool SpawnInWorld()
        {
            if (id.HasValue)
                return false;
            inWorld = FlipsiderGame.GameInstance.CurrentWorld ?? throw new InvalidOperationException("Cannot spawn entity when there is no world loaded.");
            id = inWorld.Entities.New(this);
            return true;
        }

        /// <summary>
        /// Called when this entity is spawned in the current world.
        /// </summary>
        /// <param name="id">The ID of this entity.</param>
        /// <param name="world">The current world.</param>
        public event SpawnDelegate? OnSpawn;
        internal void CallSpawn(int entityID, World world) => OnSpawn?.Invoke(entityID, world);

        /// <summary>
        /// Called when this entity is updated.
        /// </summary>
        public event UpdateDelegate? OnUpdate;
        internal void CallUpdate() => OnUpdate?.Invoke();

        /// <summary>
        /// Called when this entity is drawn.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to draw with.</param>
        public event DrawDelegate? OnDraw;
        internal void CallDraw(SafeSpriteBatch spriteBatch) => OnDraw?.Invoke(spriteBatch);

        /// <summary>
        /// Use this to set any event handlers to null. Prevents any possible memory leaks.
        /// </summary>
        public event UpdateDelegate? OnRemove;
        internal void CallRemove()
        {
            OnRemove?.Invoke();
            OnRemove = null;
            OnUpdate = null;
            OnDraw = null;
            OnSpawn = null;
            id = null;
        }
    }
}
