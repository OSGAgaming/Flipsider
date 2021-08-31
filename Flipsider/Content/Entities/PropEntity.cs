
using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Text;
using Flipsider.Engine.Interfaces;
using static Flipsider.PropManager;
using System.IO;
using System.Collections.Generic;
using Flipsider.Engine.Particles;
using Flipsider.GUI.TilePlacementGUI;
using Flipsider.Engine.Maths;

namespace Flipsider
{
    public abstract class PropEntity
    {
        public static Dictionary<string, PropEntity> keyValuePairs = new Dictionary<string, PropEntity>();
        protected Texture2D Texture => PropTypes[Prop];
        public abstract string Prop { get; }
        public virtual bool Draw(SpriteBatch spriteBatch, Prop prop) { return true; }
        public virtual void Update(Prop prop) { }
        public virtual void PostLoad(Prop prop) { }
    }

    public class Tree : PropEntity
    {
        public override string Prop => "MediumTree_1";

        public override bool Draw(SpriteBatch spriteBatch, Prop prop)
        {
            Main.lighting.Maps.DrawToMap("BloomMap", (SpriteBatch sb) => { sb.Draw(PropTypes[Prop], prop.Center, PropTypes[Prop].Bounds, Color.White, 0f, PropTypes[Prop].TextureCenter(), 1f, SpriteEffects.None, 0f); });

            spriteBatch.Draw(PropTypes[Prop], prop.Center, PropTypes[Prop].Bounds, Color.White, 0f, PropTypes[Prop].TextureCenter(), 1f, SpriteEffects.None, 0f);
            return false;
        }
    }

    public class WaterFall : PropEntity
    {
        public override string Prop => "Forest_Waterfall";

        public override bool Draw(SpriteBatch spriteBatch, Prop prop)
        {
            Main.lighting.Maps.DrawToMap("FGWaterMap", (SpriteBatch sb) => { sb.Draw(PropTypes[Prop], prop.Center, PropTypes[Prop].Bounds, Color.White, 0f, PropTypes[Prop].TextureCenter(), 1.1f, SpriteEffects.None, 0f); });


            //spriteBatch.Draw(PropTypes[Prop], prop.Center, PropTypes[Prop].Bounds, Color.White * 0.2f, 0f, PropTypes[Prop].TextureCenter(), 1f, SpriteEffects.None, 0f);
            return false;
        }

        public override void Update(Prop prop)
        {
            Vector2 position = prop.Position + new Vector2(Main.rand.NextFloat(prop.Width + 20) - 10, prop.Height + 65);
            int rand = Main.rand.Next(10);
            if (rand == 0)
            {
                Main.World.GlobalParticles.SpawnParticle(
                new SetPosition(position),
                new SetLightIntensityRand(0.2f, 0.3f),
                new SetColor(Color.AliceBlue),
                new SetVelocity(new Vector2(0, Main.rand.NextFloat(-200, -60))),
                new SetScale(Main.rand.NextFloat(0.5f, 2f)),
                new SetTexture(Textures._Foam1),
                new SlowDown(new Vector2(0.98f)),
                new OpacityOverLifetime(EaseFunction.ReverseLinear),
                new SetRotationSpeed(Main.rand.NextFloat(-0.007f, 0.012f)));
            }
            if (rand == 1)
            {
                Main.World.GlobalParticles.SpawnParticle(
                new SetPosition(position),
                new SetLightIntensityRand(0.2f, 0.3f),
                new SetColor(Color.AliceBlue),
                new SetVelocity(new Vector2(0, Main.rand.NextFloat(-200, -60))),
                new SetScale(Main.rand.NextFloat(0.7f, 2f)),
                new SetTexture(Textures._Foam2),
                new SlowDown(new Vector2(0.98f)),
                new OpacityOverLifetime(EaseFunction.ReverseLinear),
                new SetRotationSpeed(Main.rand.NextFloat(-0.007f, 0.012f)));
            }
            if (rand == 2)
            {
                Main.World.GlobalParticles.SpawnParticle(
                new SetPosition(position),
                new SetLightIntensityRand(0.2f, 0.3f),
                new SetColor(Color.AliceBlue),
                new SetVelocity(new Vector2(0, Main.rand.NextFloat(-200, -60))),
                new SetScale(Main.rand.NextFloat(0.8f, 2f)),
                new SetTexture(Textures._Foam3),
                new SlowDown(new Vector2(0.98f)),
                new OpacityOverLifetime(EaseFunction.ReverseLinear),
                new SetRotationSpeed(Main.rand.NextFloat(-0.007f, 0.012f)));
            }
        }
    }

    public class Leaves : PropEntity
    {
        public override string Prop => "Forest_ForestDecoOne";
        public override bool Draw(SpriteBatch spriteBatch, Prop prop)
        {
            Main.lighting.Maps.DrawToMap("LeavesMap", (SpriteBatch sb) => { sb.Draw(PropTypes[Prop], prop.Center, PropTypes[Prop].Bounds, Color.White, 0f, PropTypes[Prop].TextureCenter(), 1.1f, SpriteEffects.None, 0f); });
            //spriteBatch.Draw(PropTypes[Prop], prop.Center, PropTypes[Prop].Bounds, Color.White * 0.2f, 0f, PropTypes[Prop].TextureCenter(), 1f, SpriteEffects.None, 0f);
            return false;
        }
    }

    public class EnergyRocc : PropEntity
    {
        public override string Prop => "EnergyRocc";

        public override bool Draw(SpriteBatch spriteBatch, Prop prop)
        {
            Texture2D tex = TextureCache.PointLight;

            Main.lighting.Maps.DrawToMap("BloomMap", (SpriteBatch sb) => { sb.Draw(TextureCache.EnergyRoccGlow, prop.Center, PropTypes[Prop].Bounds, Color.White * Time.SineTime(2f), 0f, PropTypes[Prop].TextureCenter(), 1.1f, SpriteEffects.None, 0f); });

            Main.lighting.Maps.DrawToMap("LightingMap", (SpriteBatch sb) => { sb.Draw(tex, prop.Center + new Vector2(0, 26), tex.Bounds, Color.CadetBlue * Time.SineTime(2f) * 0.5f, 0f, tex.TextureCenter(), 0.8f, SpriteEffects.None, 0f); });

            spriteBatch.Draw(PropTypes[Prop], prop.Center, PropTypes[Prop].Bounds, Color.White, 0f, PropTypes[Prop].TextureCenter(), 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}

