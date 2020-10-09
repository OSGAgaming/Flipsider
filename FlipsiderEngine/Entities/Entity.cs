using Flipsider.Core;
using Flipsider.Graphics;
using Flipsider.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Flipsider.Entities
{
    public delegate void UpdateDelegate(WorldEntity worldEntity);
    public delegate void DrawDelegate(WorldEntity worldEntity, SafeSpriteBatch spriteBatch);
    public delegate void SpawnDelegate(WorldEntity worldEntity);

    public abstract class Entity
    {
        /// <summary>
        /// A <see cref="WorldEntity"/> object that represents this entity. Null if this entity isn't in a world.
        /// </summary>
        public WorldEntity? AsWorldEntity { get; private set; }

        /// <summary>
        /// The center of the entity.
        /// </summary>
        public Vector2 Center;

        /// <summary>
        /// Call this when the entity should be removed from the world.
        /// </summary>
        protected void Delete()
        {
            AsWorldEntity?.World.Entities.Remove(AsWorldEntity.ID);
        }

        /// <summary>
        /// If this entity is not already spawned, this spawns the entity in the current world and sets the <see cref="AsWorldEntity"/> property accordingly.
        /// </summary>
        /// <returns>This entity's non-null <see cref="AsWorldEntity"/> value.</returns>
        public WorldEntity SpawnInWorld()
        {
            if (AsWorldEntity == null)
            {
                var world = FlipsiderGame.GameInstance.CurrentWorld ?? throw new InvalidOperationException("Cannot spawn entity when there is no world loaded.");
                AsWorldEntity = world.Entities.New(this);
            }
            return AsWorldEntity;
        }

        /// <summary>
        /// Called when this entity is spawned in the current world.
        /// </summary>
        /// <param name="id">The ID of this entity.</param>
        /// <param name="world">The current world.</param>
        public event EntityDelegate? OnSpawn;
        internal void CallSpawn(WorldEntity worldEntity) => OnSpawn?.Invoke(worldEntity);

        /// <summary>
        /// Called when this entity is updated.
        /// </summary>
        public event EntityDelegate? OnUpdate;
        internal void CallUpdate(WorldEntity worldEntity) => OnUpdate?.Invoke(worldEntity);

        /// <summary>
        /// Called when this entity is drawn.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to draw with.</param>
        public event EntityDrawDelegate? OnDraw;
        internal void CallDraw(WorldEntity worldEntity, SafeSpriteBatch spriteBatch) => OnDraw?.Invoke(worldEntity, spriteBatch);

        /// <summary>
        /// Use this to set any event handlers to null. Prevents any possible memory leaks.
        /// </summary>
        public event UpdateDelegate? OnRemove;
        internal void CallRemove(WorldEntity worldEntity)
        {
            OnRemove?.Invoke(worldEntity);
            OnRemove = null;
            OnUpdate = null;
            OnDraw = null;
            OnSpawn = null;
            AsWorldEntity = null;
        }
    }
}
