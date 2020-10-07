using Flipsider.Assets;
using Flipsider.Core;
using Flipsider.Core.Collections;
using Flipsider.Worlds.Collision;
using Flipsider.Worlds.Entities;
using Flipsider.Worlds.Particles;
using Flipsider.Worlds.Tiles;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Flipsider.Worlds
{
    public class World : IUpdated, IDrawn
    {
        public World()
        {
            Tiles = new TileSystem(this);
            OnUpdate = new OrderedSet<IUpdated>
            {
                (Entities = new EntitySystem(this)),
                (Particles = new ParticleSystem(this, 100)),
                (Collision = new CollisionSystem()),
            };
            OnDraw = new OrderedSet<IDrawn>
            {
                Entities,
                Particles
            };
        }

        public CollisionSystem Collision { get; }
        public ParticleSystem Particles { get; }
        public EntitySystem Entities { get; }
        public TileSystem Tiles { get; }

        public OrderedSet<IUpdated> OnUpdate { get; }
        public OrderedSet<IDrawn> OnDraw { get; }

        protected internal virtual void Load()
        {

        }

        protected internal virtual void Unload()
        {
            OnUpdate.Clear();
            OnDraw.Clear();
        }

        protected virtual void Update()
        {
            foreach (var item in OnUpdate)
            {
                try
                {
                    item.Update();
                }
                catch (Exception e)
                {
                    Logger.Warn($"World threw an exception while updating {item}. {e}");
                }
            }
        }

        protected virtual void Draw(SafeSpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: FlipsiderGame.GameInstance.CurrentCamera.Transform);
            foreach (var item in OnDraw)
            {
                try
                {
                    item.Draw(spriteBatch);
                }
                catch (Exception e)
                {
                    Logger.Warn($"World threw an exception while drawing {item}. {e}");
                }
            }
        }

        void IUpdated.Update() => Update();
        void IDrawn.Draw(SafeSpriteBatch spriteBatch) => Draw(spriteBatch);
    }
}