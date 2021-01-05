
using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Flipsider.PropInteraction;
using static Flipsider.PropManager;

namespace Flipsider
{
    public class Prop : ILayeredComponent
    {
        public int noOfFrames;
        public int animSpeed;
        public Vector2 position;
        public int frameCounter;
        public string prop;
        public bool active = true;
        public int alteredWidth => PropTypes[prop].Width / PropEntites[prop].noOfFrames;
        public Vector2 Center => position + new Vector2(PropTypes[prop].Width / 2, PropTypes[prop].Height / 2);
        public int frameX => PropEntites[prop].animSpeed == -1 ? 0 : (frameCounter / PropEntites[prop].animSpeed) % PropEntites[prop].noOfFrames;
        public Rectangle alteredFrame => new Rectangle(frameX * alteredWidth, 0, alteredWidth, PropTypes[prop].Height);
        public int interactRange;
        public TileInteraction? tileInteraction;

        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                spriteBatch.Draw(PropTypes[prop], Center, alteredFrame, Color.White, 0f, alteredFrame.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0f);
            }
        }

        public int Layer { get; set; }
        public Prop(string prop, Vector2 pos, TileInteraction? TileInteraction = null, int noOfFrames = 1, int animSpeed = -1, int frameCount = 0, int Layer = 0)
        {
            this.noOfFrames = noOfFrames;
            this.animSpeed = animSpeed;
            position = pos;
            this.prop = prop;
            interactRange = 100;
            tileInteraction = TileInteraction;
            frameCounter = frameCount;
            this.Layer = LayerHandler.CurrentLayer;
            Main.renderer.layerHandler.AppendMethodToLayer(this);
        }


    }
}
