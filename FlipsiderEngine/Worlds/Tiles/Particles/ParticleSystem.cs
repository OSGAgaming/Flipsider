using System;
using System.Collections.Generic;
using System.Text;
using Flipsider.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flipsider.Worlds.Particles
{
    public struct Particle
    {
        public bool Alive;
        public Vector2 Center;
        public Vector2 Velocity;
        public Color Color;
        public float Rotation;
        public float Scale;
        public float Opacity;
        public Texture2D Texture;
        public float Lifetime;
        public float Age;
    }

    public interface IParticleModifier
    {
        void Invoke(Particle[] particles, int index);
    }

    public interface ISpawnerModifier
    {
        bool CanSpawn(out float percentageOfSpeed);
    }

    public class ParticleSystem : IUpdated, IDrawn
    {
        private float _spawnTimer;
        private Particle[] _particles;

        public bool SpawningEnabled { get; set; }
        public float SpawnRate { get; set; }

        public bool WorldSpace { get; set; }
        public Vector2 Position { get; set; }

        public List<IParticleModifier> SpawnModules { get; private set; }
        public List<IParticleModifier> UpdateModules { get; private set; }
        public World World { get; }

        internal ParticleSystem(World world, int maxParticles)
        {
            World = world;
            _particles = new Particle[maxParticles];

            SpawningEnabled = true;

            SpawnModules = new List<IParticleModifier>();
            UpdateModules = new List<IParticleModifier>();
        }

        void IUpdated.Update()
        {
            DoSpawnRate();

            for (int i = 0; i < _particles.Length; i++)
                if (_particles[i].Alive)
                {
                    for (int j = 0; j < UpdateModules.Count; j++)
                        UpdateModules[j].Invoke(_particles, i);
                    _particles[i].Center += _particles[i].Velocity * Time.DeltaF;
                    _particles[i].Age += Time.DeltaF;
                    if (_particles[i].Age >= _particles[i].Lifetime)
                        _particles[i].Alive = false;
                }
        }

        private void DoSpawnRate()
        {
            if (!SpawningEnabled) return;

            _spawnTimer += Time.DeltaF;
            float spawnMax = 1f / SpawnRate;
            int count = 0;
            while (_spawnTimer >= spawnMax)
            {
                _spawnTimer -= spawnMax;
                count++;
            }

            if (count > 0)
                SpawnParticles(count);
        }

        public void SpawnParticles(int amount)
        {
            for (int i = 0; i < _particles.Length; i++)
                if (!_particles[i].Alive)
                {
                    NewParticle(i);
                    amount--;
                    if (amount == 0)
                        return;
                }
        }

        private void NewParticle(int index)
        {
            _particles[index].Center = WorldSpace ? Position : Vector2.Zero;
            _particles[index].Velocity = Vector2.Zero;
            _particles[index].Color = Color.White;
            _particles[index].Rotation = 0f;
            _particles[index].Scale = 1f;
            _particles[index].Opacity = 1f;
            _particles[index].Lifetime = 1f;
            _particles[index].Age = 0f;
            _particles[index].Alive = true;

            //modify position based on spawn module
            for (int i = 0; i < SpawnModules.Count; i++)
                SpawnModules[i].Invoke(_particles, index);
        }

        void IDrawn.Draw(SafeSpriteBatch spriteBatch)
        {
            for (int i = 0; i < _particles.Length; i++)
            {
                Particle p = _particles[i];
                if (p.Alive)
                {
                    Texture2D texture = p.Texture;
                    //TODO: add a source rect for each particle? or make a texture-frame pair class so that all dusts can be put into a single texture atlas for improved performance.
                    //TODO: add a layer depth value?
                    spriteBatch.Sb.Draw(texture, WorldSpace ? p.Center : Position + p.Center, null, p.Color * p.Opacity, p.Rotation, texture.Bounds.Size.ToVector2() * 0.5f, p.Scale, SpriteEffects.None, 0f);
                }
            }
        }
    }
}
