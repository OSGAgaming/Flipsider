using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Text;

using static FlipEngine.PropManager;
using System.IO;

namespace FlipEngine
{
    public partial class Prop : Entity
    {
        public bool draggable;
        public int frameCounter;
        public bool active = true;
        public byte[] propEncode;
        //dragging stuff
        public bool isDragging = false;
        public Vector2 offsetFromMouseWhileDragging;
        public override void Dispose()
        {
            Main.World.layerHandler.Layers[Layer].Drawables.Remove(this);
            Main.World.propManager.props.Remove(this);
            UpdateModules.Clear();
            Chunk.Entities.Remove(this);
            active = false;
        }

        public int interactRange;

        public PropEntity? PE;
        protected override void PostConstructor()
        {
            PE?.PostLoad(this);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (active && PropTypes.ContainsKey(prop))
            {
                bool? Continue = PE?.Draw(spriteBatch,this);
            if (!Continue ?? false) return;

                Rectangle r = PropTypes[prop].Bounds;
                spriteBatch.Draw(PropTypes[prop], Center, r, Color.White, 0f, r.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0f);
            }
        }
        protected override void PreUpdate()
        {
            Rectangle r = PropTypes[prop].Bounds;
            Utils.DrawToMap("LightingOcclusionMap", (SpriteBatch sb) => sb.Draw(PropTypes[prop], Center, r, Color.White, 0f, r.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0f));
            PE?.Update(this);
        }


        public override Entity Deserialize(Stream stream)
        {
            BinaryReader binaryWriter = new BinaryReader(stream);
            int length = binaryWriter.ReadInt32();
            string propEncode = Encoding.UTF8.GetString(binaryWriter.ReadBytes(length), 0, length);
            Vector2 position = binaryWriter.ReadVector2();
            bool draggable = binaryWriter.ReadBoolean();
            int Layer = binaryWriter.ReadInt32();
            Prop prop = new Prop(propEncode, position, Layer, draggable);
            return Main.World.propManager.AddProp(prop);
        }
        public override void Serialize(Stream stream)
        {
            BinaryWriter binaryWriter = new BinaryWriter(stream);
            binaryWriter.Write(propEncode.Length);
            binaryWriter.Write(propEncode);
            binaryWriter.Write(Position);
            binaryWriter.Write(draggable);
            binaryWriter.Write(Layer);
        }

        public Prop(string prop, Vector2 pos, int Layer = 0, bool Draggable = true) : base()
        {
            Active = true;
            InFrame = true;
            propEncode = Encoding.UTF8.GetBytes(prop);
            this.Layer = Layer;
            draggable = Draggable;
            Position = pos.AddParallaxAcrossX(Main.layerHandler.Layers[Layer].parallax);
            Width = PropTypes[prop].Width;
            Height = PropTypes[prop].Height;
            if(PropEntity.keyValuePairs.ContainsKey(prop))
            PE = PropEntity.keyValuePairs[prop];
        }
        public Prop(string prop) : base()
        {
            propEncode = new byte[prop.Length];
            propEncode = Encoding.UTF8.GetBytes(prop);
            Width = 1;
            Height = 1;
        }
        public Prop() : base()
        {
            Active = false;
            propEncode = new byte[1];
            interactRange = 100;
        }
    }
}
