
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
            spriteBatch.Draw(PropTypes[Prop], prop.Center, PropTypes[Prop].Bounds,Color.White,0f, PropTypes[Prop].TextureCenter(), 1f,SpriteEffects.None,0f);
            return false;
        }
    }
}

