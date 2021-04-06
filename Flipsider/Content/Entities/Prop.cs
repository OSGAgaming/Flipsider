
using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Text;
using Flipsider.Engine.Interfaces;
using static Flipsider.PropManager;
using System.IO;

namespace Flipsider
{
    [Serializable]
    public partial class Prop : Entity
    {
        public int noOfFrames;
        public int animSpeed;
        public bool draggable;
        public int frameCounter;
        public bool active = true;
        public byte[] propEncode;
        //dragging stuff
        public bool isDragging = false;
        [NonSerialized]
        public Vector2 offsetFromMouseWhileDragging;
        public override void Dispose()
        {
            Main.CurrentWorld.layerHandler.Layers[Layer].Drawables.Remove(this);
            Main.CurrentWorld.propManager.props.Remove(this);
            UpdateModules.Clear();
            Chunk.Entities.Remove(this);
            active = false;
        }

        public int interactRange;

        public PropEntity? PE;

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (active && PropTypes.ContainsKey(prop))
            {
                bool? Continue = PE?.Draw(spriteBatch,this);
            if (!Continue ?? false) return;

            
                spriteBatch.Draw(PropTypes[prop], Center, alteredFrame, Color.White, 0f, alteredFrame.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0f);
            }
        }
        protected override void PreUpdate()
        {
            Utils.DrawToMap("CanLightMap", (SpriteBatch sb) => sb.Draw(PropTypes[prop], Center, alteredFrame, Color.White, 0f, alteredFrame.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0f));
            PE?.Update();
        }


        public override Entity Deserialize(Stream stream)
        {
            BinaryReader binaryWriter = new BinaryReader(stream);
            int length = binaryWriter.ReadInt32();
            string propEncode = Encoding.UTF8.GetString(binaryWriter.ReadBytes(length), 0, length);
            int noOfFrames = binaryWriter.ReadInt32();
            int animSpeed = binaryWriter.ReadInt32();
            Vector2 position = binaryWriter.ReadVector2();
            bool draggable = binaryWriter.ReadBoolean();
            int frameCounter = binaryWriter.ReadInt32();
            int Layer = binaryWriter.ReadInt32();
            Prop prop = new Prop(propEncode, position, noOfFrames, animSpeed, frameCounter, Layer, draggable);
            return Main.CurrentWorld.propManager.AddProp(prop);
        }
        public override void Serialize(Stream stream)
        {
            BinaryWriter binaryWriter = new BinaryWriter(stream);
            binaryWriter.Write(propEncode.Length);
            binaryWriter.Write(propEncode);
            binaryWriter.Write(noOfFrames);
            binaryWriter.Write(animSpeed);
            binaryWriter.Write(position);
            binaryWriter.Write(draggable);
            binaryWriter.Write(frameCounter);
            binaryWriter.Write(Layer);
        }

        public Prop(string prop, Vector2 pos, int noOfFrames = 1, int animSpeed = -1, int frameCount = 0, int Layer = 0, bool Draggable = true)
        {
            Active = true;
            InFrame = true;
            this.noOfFrames = noOfFrames;
            this.animSpeed = animSpeed;
            propEncode = Encoding.UTF8.GetBytes(prop);
            interactRange = 100;
            frameCounter = frameCount;
            this.Layer = Layer;
            draggable = Draggable;
            position = pos.AddParallaxAcrossX(Main.layerHandler.Layers[Layer].parallax);
            width = PropTypes[prop].Width;
            height = PropTypes[prop].Height;
            if(PropEntity.keyValuePairs.ContainsKey(prop))
            PE = PropEntity.keyValuePairs[prop];
        }
        public Prop(string prop, Vector2 pos, int layer, int noOfFrames = 1, int animSpeed = -1, int frameCount = 0)
        {
            this.noOfFrames = noOfFrames;
            this.animSpeed = animSpeed;
            propEncode = new byte[prop.Length];
            propEncode = Encoding.UTF8.GetBytes(prop);
            interactRange = 100;
            frameCounter = frameCount;
            Layer = layer;
            position = pos.AddParallaxAcrossX(Main.layerHandler.Layers[Layer].parallax);
        }
        public Prop(string prop)
        {
            propEncode = new byte[prop.Length];
            propEncode = Encoding.UTF8.GetBytes(prop);
            width = 1;
            height = 1;
            animSpeed = 1;
            noOfFrames = 1;
            interactRange = 100;
        }
        public Prop()
        {
            Active = false;
            propEncode = new byte[1];
            interactRange = 100;
        }
    }
}
