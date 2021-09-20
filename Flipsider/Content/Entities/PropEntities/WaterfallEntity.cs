
using FlipEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Text;

using System.IO;
using System.Collections.Generic;

using Flipsider.GUI;
using Flipsider.Engine.Maths;

namespace Flipsider
{
    public class WaterFall : PropEntity
    {
        public override string Prop => "Forest_Waterfall";

        public override bool Draw(SpriteBatch spriteBatch, Prop prop)
        {
            Main.lighting.Maps.DrawToMap("FGWaterMap", (SpriteBatch sb) => { sb.Draw(Texture, prop.Center, Texture.Bounds, Color.White, 0f, Texture.TextureCenter(), 1.1f, SpriteEffects.None, 0f); });
            //spriteBatch.Draw(PropTypes[Prop], prop.Center, PropTypes[Prop].Bounds, Color.White * 0.2f, 0f, PropTypes[Prop].TextureCenter(), 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}

