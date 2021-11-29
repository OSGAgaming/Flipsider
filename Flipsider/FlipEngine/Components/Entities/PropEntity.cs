

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Text;
using static FlipEngine.PropManager;
using System.IO;
using System.Collections.Generic;



namespace FlipEngine
{
    public abstract class PropEntity : Entity
    {
        public static Dictionary<string, PropEntity> PropEntities = new Dictionary<string, PropEntity>();
        protected Texture2D Texture => PropTypes[Prop];
        public abstract string Prop { get; }
        public virtual bool Draw(SpriteBatch spriteBatch, Prop prop) { return true; }
        public virtual void Update(Prop prop) { }
        public virtual void PostLoad(Prop prop) { }
    }
}

