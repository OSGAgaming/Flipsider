using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using static Flipsider.Prop;
using static Flipsider.TileManager;
using static Flipsider.PropManager;
namespace Flipsider
{
    public class PointLight : LightSource
    {
        public PointLight(float str, Vector2 pos, Color col) : base(str,pos,col)
        {

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}