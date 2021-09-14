using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FlipEngine
{
    public struct Particle
    {
        public bool Alive;
        public Vector2 Center;
        public Vector2 ParalaxedCenter;
        public Vector2 Velocity;
        public Color Color;
        public float Rotation;
        public float Scale;
        public float Opacity;
        public Texture2D Texture;
        public List<IParticleModifier> RuntimeModules;
        public float Lifetime;
        public float Age;
        public float Paralax;
        public float LightIntensity;
    }

    public interface IParticleModifier
    {
        void Invoke(Particle[] particles, int index);
    }

    public interface ISpawnParticleModifier : IParticleModifier { }

    public interface ISpawnerModifier
    {
        bool CanSpawn(out float percentageOfSpeed);
    }

    public class ParticleSystem : ILoadUpdate
    {
        private float _spawnTimer;

        internal readonly Particle[] _particles;
        internal readonly int EngineMaxParticles = 200;

        public bool SpawningEnabled { get; set; }
        public float SpawnRate { get; set; }

        public bool WorldSpace { get; set; }
        public Vector2 Position { get; set; }

        public List<IParticleModifier> SpawnModules { get; private set; }
        public List<IParticleModifier> UpdateModules { get; private set; }

        public ParticleSystem()
        {
            _particles = new Particle[EngineMaxParticles];
            SpawningEnabled = true;

            SpawnModules = new List<IParticleModifier>();
            UpdateModules = new List<IParticleModifier>();
        }
 
        public void Update()
        {
            DoSpawnRate();

            for (int i = 0; i < _particles.Length; i++)
            {
                if (_particles[i].Alive)
                {
                    for (int j = 0; j < UpdateModules.Count; j++)
                    {
                        UpdateModules[j].Invoke(_particles, i);
                    }
                    for (int j = 0; j < _particles[i].RuntimeModules.Count; j++)
                    {
                        _particles[i].RuntimeModules[j].Invoke(_particles, i);
                    }
                    _particles[i].Center += _particles[i].Velocity * Time.DeltaT;
                    _particles[i].Age += Time.DeltaT;
                    _particles[i].ParalaxedCenter = _particles[i].Center.AddParallaxAcrossX(_particles[i].Paralax);

                    if (_particles[i].Age >= _particles[i].Lifetime)
                    {
                        _particles[i].Alive = false;
                    }
                }
            }
        }

        private void DoSpawnRate()
        {
            if (!SpawningEnabled) return;

            _spawnTimer += Time.DeltaT;
            float spawnMax = 1f / SpawnRate;
            int count = 0;
            while (_spawnTimer >= spawnMax)
            {
                _spawnTimer -= spawnMax;
                count++;
            }

            if (count > 0)
            {
                SpawnParticles(count);
            }
        }

        public void SpawnParticles(int amount)
        {
            for (int i = 0; i < _particles.Length; i++)
            {
                if (!_particles[i].Alive)
                {
                    NewParticle(i);
                    amount--;
                    if (amount == 0)
                    {
                        return;
                    }
                }
            }
        }

        public int SpawnParticle(params IParticleModifier[] Modules)
        {
            for (int i = 0; i < _particles.Length; i++)
            {
                if (!_particles[i].Alive)
                {
                    NewParticle(i);
                    for (int j = 0; j < Modules.Length; j++)
                    {
                        if (Modules[j] is ISpawnParticleModifier) Modules[j].Invoke(_particles, i);
                        else _particles[i].RuntimeModules.Add(Modules[j]);
                    }
                    return i;
                }
            }
            return 0;
        }

        private void NewParticle(int index)
        {
            _particles[index].Center = (WorldSpace ? Position : Vector2.Zero);
            _particles[index].Velocity = Vector2.Zero;
            _particles[index].Color = Color.White;
            _particles[index].Rotation = 0f;
            _particles[index].Scale = 1f;
            _particles[index].Opacity = 1f;
            _particles[index].Lifetime = 1f;
            _particles[index].Age = 0f;
            _particles[index].Alive = true;
            _particles[index].Texture = FlipTextureCache.pixel;
            _particles[index].RuntimeModules = new List<IParticleModifier>();

            //modify position based on spawn module
            for (int i = 0; i < SpawnModules.Count; i++)
            {
                SpawnModules[i].Invoke(_particles, index);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _particles.Length; i++)
            {
                Particle p = _particles[i];
                if (p.Alive)
                {
                    Texture2D texture = p.Texture;
                    //TODO: add a source rect for each particle? or make a texture-frame pair class so that all dusts can be put into a single texture atlas for improved performance.
                    //TODO: add a layer depth value?
                    spriteBatch.Draw(texture, WorldSpace ? p.ParalaxedCenter : Position + p.ParalaxedCenter, null, p.Color * p.Opacity, p.Rotation, texture.Bounds.Size.ToVector2() * 0.5f, p.Scale + p.Paralax*2, SpriteEffects.None, 0f);

                    Texture2D lightTexture = FlipTextureCache.PointLight;

                    FlipGame.lighting.Maps.DrawToMap("LightingMap", (SpriteBatch sb) => {
                        sb.Draw(lightTexture, WorldSpace ? p.ParalaxedCenter : Position + p.ParalaxedCenter , null, p.Color * p.Opacity * p.LightIntensity * 0.2f,
                            p.Rotation, lightTexture.Bounds.Size.ToVector2() * 0.5f, p.Scale * p.LightIntensity * 0.1f + p.Paralax * 0.2f, SpriteEffects.None, 0f);
                    }
);
                }
            }
        }
    }
}
