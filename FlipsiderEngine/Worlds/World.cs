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
    public sealed class World : IUpdated, IDrawn
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

        public OrderedSet<IUpdated> OnUpdate { get; private set; }
        public OrderedSet<IDrawn> OnDraw { get; private set; }

        public static event Action? OnLoad;
        public static event Action? OnUnload;

        public void Load()
        {
            Collision.Add(new TileCollision());
            OnLoad?.Invoke();
        }

        public void Unload()
        {
            OnUnload?.Invoke();
            OnUpdate = new OrderedSet<IUpdated>();
            OnDraw = new OrderedSet<IDrawn>();
        }

        void IUpdated.Update()
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

        void IDrawn.Draw(SafeSpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: FlipsiderGame.CurrentCamera.Transform);
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
    }
}