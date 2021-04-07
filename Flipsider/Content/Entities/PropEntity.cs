
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

namespace Flipsider
{
    public abstract class PropEntity
    {
        public static Dictionary<string, PropEntity> keyValuePairs = new Dictionary<string, PropEntity>();
        protected Texture2D Texture => PropTypes[Prop];
        public abstract string Prop { get; }
        public virtual bool Draw(SpriteBatch spriteBatch, Prop prop) { return true; }
        public virtual void Update() { }
    }

    public class Tree : PropEntity
    {
        public override string Prop => "MediumTree_1";

        public override bool Draw(SpriteBatch spriteBatch, Prop prop)
        {
            Main.lighting.Maps.DrawToMap("Bloom", (SpriteBatch sb) => { sb.Draw(PropTypes[Prop], prop.Center, PropTypes[Prop].Bounds, Color.White, 0f, PropTypes[Prop].TextureCenter(), 1f, SpriteEffects.None, 0f); });

            spriteBatch.Draw(PropTypes[Prop], prop.Center, PropTypes[Prop].Bounds,Color.White,0f, PropTypes[Prop].TextureCenter(), 1f,SpriteEffects.None,0f);
            return false;
        }
    }

    public class WaterFall : PropEntity
    {
        public override string Prop => "Forest_Waterfall";

        public override bool Draw(SpriteBatch spriteBatch, Prop prop)
        {
            Main.lighting.Maps.DrawToMap("FGWater", (SpriteBatch sb) => { sb.Draw(PropTypes[Prop], prop.Center, PropTypes[Prop].Bounds, Color.White, 0f, PropTypes[Prop].TextureCenter(), 1.1f, SpriteEffects.None, 0f); });
            //spriteBatch.Draw(PropTypes[Prop], prop.Center, PropTypes[Prop].Bounds, Color.White * 0.2f, 0f, PropTypes[Prop].TextureCenter(), 1f, SpriteEffects.None, 0f);
            return false;
        }
    }

    public class Leaves : PropEntity
    {
        public override string Prop => "Forest_ForestDecoOne";

        public override bool Draw(SpriteBatch spriteBatch, Prop prop)
        {
            Main.lighting.Maps.DrawToMap("Leaves", (SpriteBatch sb) => { sb.Draw(PropTypes[Prop], prop.Center, PropTypes[Prop].Bounds, Color.White, 0f, PropTypes[Prop].TextureCenter(), 1.1f, SpriteEffects.None, 0f); });
            //spriteBatch.Draw(PropTypes[Prop], prop.Center, PropTypes[Prop].Bounds, Color.White * 0.2f, 0f, PropTypes[Prop].TextureCenter(), 1f, SpriteEffects.None, 0f);
            return false;
        }
    }

    public class EnergyRocc : PropEntity
    {
        public override string Prop => "EnergyRocc";

        public override bool Draw(SpriteBatch spriteBatch, Prop prop)
        {
            Main.lighting.Maps.DrawToMap("Bloom", (SpriteBatch sb) => { sb.Draw(TextureCache.EnergyRoccGlow, prop.Center, PropTypes[Prop].Bounds, Color.White*Time.SineTime(2f), 0f, PropTypes[Prop].TextureCenter(), 1.1f, SpriteEffects.None, 0f); });
            spriteBatch.Draw(PropTypes[Prop], prop.Center, PropTypes[Prop].Bounds, Color.White, 0f, PropTypes[Prop].TextureCenter(), 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}

