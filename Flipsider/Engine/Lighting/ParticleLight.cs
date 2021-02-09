using Flipsider.Engine.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Flipsider
{
    public class ParticleLight : LightSource
    {
        ParticleSystem ParticleSystem;
        public ParticleLight(ParticleSystem ParticleSystem) : base(default, default, default)
        {
            this.ParticleSystem = ParticleSystem;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            Utils.BeginCameraSpritebatch();
            foreach (Particle particle in ParticleSystem._particles)
            {
                if(particle.Alive)
                spriteBatch.Draw(TextureCache.PointLight, particle.Center, TextureCache.PointLight.Bounds, particle.Color* particle.LightIntensity * particle.Opacity, particle.Rotation, TextureCache.PointLight.TextureCenter(), particle.LightIntensity* particle.Scale*0.3f, SpriteEffects.None, 0f);
            }
            spriteBatch.End();
            Utils.BeginCameraSpritebatch();
            base.Draw(spriteBatch);
        }
    }
}