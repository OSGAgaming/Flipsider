
using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Text;
using static Flipsider.PropManager;

namespace Flipsider
{
    [Serializable]
    public class Prop : Entity
    {
        public int noOfFrames;
        public int animSpeed;
        public float positionX;
        public float positionY;
        public bool draggable;
        public int frameCounter;
        public string prop => Encoding.UTF8.GetString(propEncode, 0, propEncode.Length);
        public bool active = true;
        public byte[] propEncode;
        //dragging stuff
        public bool isDragging = false;
        [NonSerialized]
        public Vector2 offsetFromMouseWhileDragging;
        public void Dispose()
        {
            Main.CurrentWorld.layerHandler.Layers[Layer].Drawables.Remove(this);
            Main.CurrentWorld.propManager.props.Remove(this);
            active = false;
        }
        public int alteredWidth => PropTypes[prop].Width / PropEntites[prop].noOfFrames;
        public Vector2 ParallaxedCenter => Center.AddParallaxAcrossX(-Main.layerHandler.Layers[Layer].parallax);
        public int frameX => PropEntites[prop].animSpeed == -1 ? 0 : (frameCounter / PropEntites[prop].animSpeed) % PropEntites[prop].noOfFrames;
        public Rectangle alteredFrame => new Rectangle(frameX * alteredWidth, 0, alteredWidth, PropTypes[prop].Height);

        public int interactRange;

        public TileInteraction? tileInteraction;

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                spriteBatch.Draw(PropTypes[prop], Center, alteredFrame, Color.White, 0f, alteredFrame.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0f);
            }
        }
        public Prop(string prop, Vector2 pos, TileInteraction? TileInteraction = null, int noOfFrames = 1, int animSpeed = -1, int frameCount = 0, int Layer = 0, bool Draggable = true)
        {
            Active = true;
            InFrame = true;
            this.noOfFrames = noOfFrames;
            this.animSpeed = animSpeed;
            propEncode = Encoding.UTF8.GetBytes(prop);
            interactRange = 100;
            tileInteraction = TileInteraction;
            frameCounter = frameCount;
            this.Layer = Layer;
            draggable = Draggable;
            positionX = pos.AddParallaxAcrossX(Main.layerHandler.Layers[Layer].parallax).X;
            positionY = pos.AddParallaxAcrossX(Main.layerHandler.Layers[Layer].parallax).Y;
            position = new Vector2(positionX, positionY);
            width = PropTypes[prop].Width;
            height = PropTypes[prop].Height;
            Main.CurrentWorld.layerHandler.AppendMethodToLayer(this);
        }
        public Prop(string prop, Vector2 pos, int layer, TileInteraction? TileInteraction = null, int noOfFrames = 1, int animSpeed = -1, int frameCount = 0)
        {
            this.noOfFrames = noOfFrames;
            this.animSpeed = animSpeed;
            propEncode = new byte[prop.Length];
            propEncode = Encoding.UTF8.GetBytes(prop);
            interactRange = 100;
            tileInteraction = TileInteraction;
            frameCounter = frameCount;
            Layer = layer;
            positionX = pos.AddParallaxAcrossX(Main.layerHandler.Layers[Layer].parallax).X;
            positionY = pos.AddParallaxAcrossX(Main.layerHandler.Layers[Layer].parallax).Y;
            Main.CurrentWorld.layerHandler.AppendMethodToLayer(this);
        }

    }
}
