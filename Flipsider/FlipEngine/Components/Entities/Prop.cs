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
        public byte[] PropEncode;
        public PropEntity? PE;

        Collideable? CollisionSet { get; set; }

        public override void Dispose()
        {
            foreach(IEntityModifier iem in UpdateModules.Values)
            {
                iem.Dispose();
            }

            CollisionSet?.Dispose();

            FlipGame.World.layerHandler.Layers[Layer].Drawables.Remove(this);
            UpdateModules.Clear();
            Chunk.Entities.Remove(this);
            Active = false;
        }

        protected override void PostConstructor()
        {
            PE?.PostLoad(this);

            AABBCollisionSet buffer = new AABBCollisionSet();

            string PropName = Encoding.UTF8.GetString(PropEncode);
            string Path = Utils.CollisionSetPath + PropName + ".abst";

            if (File.Exists(Path))
            {
                Stream stream = File.OpenRead(Path);
                Chunk chunk = FlipGame.tileManager.GetChunkToWorldCoords(Position);

                CollisionSet = chunk.Colliedables.AddCustomHitBox(this, true, Polygon.Null, buffer.Deserialize(stream));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Active && PropTypes.ContainsKey(prop))
            {
                bool? Continue = PE?.Draw(spriteBatch, this);
                if (!Continue ?? false) return;

                Rectangle r = PropTypes[prop].Bounds;

                spriteBatch.Draw(PropTypes[prop], Center, r, Color.White, 0f,
                    r.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0f);
            }
        }
        protected override void PreUpdate()
        {
            Rectangle r = PropTypes[prop].Bounds;

            Utils.DrawToMap("LightingOcclusionMap", (SpriteBatch sb) => sb.Draw(PropTypes[prop], Center, r, 
                Color.White, 0f, r.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0f));

            PE?.Update(this);
            CollisionSet?.Update(this);
        }


        public override Entity Deserialize(Stream stream)
        {
            BinaryReader binaryWriter = new BinaryReader(stream);
            int length = binaryWriter.ReadInt32();
            string propEncode = Encoding.UTF8.GetString(binaryWriter.ReadBytes(length), 0, length);
            Vector2 position = binaryWriter.ReadVector2();
            int Layer = binaryWriter.ReadInt32();
            Prop prop = new Prop(propEncode, position, Layer);
            return prop;
        }
        public override void Serialize(Stream stream)
        {
            BinaryWriter binaryWriter = new BinaryWriter(stream);

            binaryWriter.Write(PropEncode.Length);
            binaryWriter.Write(PropEncode);
            binaryWriter.Write(Position);
            binaryWriter.Write(Layer);
        }

        public Prop(string prop, Vector2 pos, int Layer = 0) : base()
        {
            Active = true;
            PropEncode = Encoding.UTF8.GetBytes(prop);
            this.Layer = Layer;
            Position = pos.AddParallaxAcrossX(FlipGame.layerHandler.Layers[Layer].parallax);

            Width = PropTypes[prop].Width;
            Height = PropTypes[prop].Height;

            if (PropEntity.PropEntities.ContainsKey(prop)) PE = PropEntity.PropEntities[prop];
        }
        public Prop(string prop) : base()
        {
            PropEncode = new byte[prop.Length];
            PropEncode = Encoding.UTF8.GetBytes(prop);
            Width = 1;
            Height = 1;
        }
        public Prop() : base()
        {
            Active = false;
            PropEncode = new byte[1];
        }
    }
}
