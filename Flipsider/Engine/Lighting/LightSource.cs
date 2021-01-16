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
using Flipsider.Engine.Interfaces;
using Flipsider.Engine;

namespace Flipsider
{
    public abstract class LightSource : IComponent,ILayeredComponent
    {
        public Primitive? Mesh;
        public int Layer { get; set; }
        public float strength;
        public Vector2 position;
        public Color colour;
        public float rotation;
        public LightSource(float str, Vector2 pos, Color col)
        {
            strength = str;
            position = pos;
            colour = col;
        }
        public virtual void Update() { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}