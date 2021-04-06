using Flipsider.Engine.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

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
            Utils.BeginAdditiveCameraSpritebatch();
            for(int i = 0; i<ParticleSystem._particles.Length; i++)
            {
                var p = ParticleSystem._particles[i];
                if (p.Alive)
                {
                    Texture2D texture = TextureCache.PointLight;
                    Main.lighting.Maps.DrawToMap("Lighting", (SpriteBatch sb) => {
                        sb.Draw(texture, ParticleSystem.WorldSpace ? p.ParalaxedCenter : ParticleSystem.Position + p.ParalaxedCenter, null, p.Color * p.Opacity * p.LightIntensity*0.4f, p.Rotation, texture.Bounds.Size.ToVector2() * 0.5f, p.Scale * p.LightIntensity * 0.1f + p.Paralax*0.2f, SpriteEffects.None, 0f);
                     }
                    );
                    //spriteBatch.Draw(texture, ParticleSystem.WorldSpace ? p.Center : ParticleSystem.Position + p.Center, null, p.Color * p.Opacity * p.LightIntensity, p.Rotation, texture.Bounds.Size.ToVector2() * 0.5f, p.Scale * p.LightIntensity * 0.1f, SpriteEffects.None, 0f);
                }
            }
            spriteBatch.End();
            Utils.BeginCameraSpritebatch();
        }
    }
}