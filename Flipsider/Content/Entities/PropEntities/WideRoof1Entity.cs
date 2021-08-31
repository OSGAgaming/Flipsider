
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
    public class WideRoof1Entity : PropEntity
    {
        public override string Prop => "WideRoof1";
        public override void PostLoad(Prop prop)
        {
            Chunk chunk = Main.tileManager.GetChunkToWorldCoords(prop.Position);
            chunk.Colliedables.AddCustomHitBox(true, false, new RectangleF(prop.Position, prop.Size));
        }
        public override bool Draw(SpriteBatch spriteBatch, Prop prop)
        {
            Main.lighting.Maps.DrawToMap("SunReflectionMap", (SpriteBatch sb) => { sb.Draw(Textures._Props_City_WideRoof1Front, prop.Center, Texture.Bounds, Color.Black, 0f, Texture.TextureCenter(), 1f, SpriteEffects.None, 0f); });

            spriteBatch.Draw(Texture, prop.Center, Texture.Bounds, Color.White, 0f, Texture.TextureCenter(), 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}

