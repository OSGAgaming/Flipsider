using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace FlipEngine
{
    public class Skybox : IDrawable, ISerializable<Skybox>
    {
        internal List<ParalaxLayer> Layers = new List<ParalaxLayer>();

        public Skybox()
        {
            LoadSkyboxes();
        }

        public virtual void LoadSkyboxes() { }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(ParalaxLayer Layer in Layers)
            {
                if (Layer.Path != null)
                {
                    Texture2D tex = Assets.GetTexture(Layer.Path);
                    Utils.RenderBG(spriteBatch, Color.White, tex, Layer.Parallax, Layer.Scale, Layer.Offset);
                }
            }
        }

        public void Serialize(Stream stream)
        {
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(Layers.Count);

            for(int i = 0; i < Layers.Count; i++)
            {
                Layers[i].Serialize(stream);
            }
        }

        public Skybox Deserialize(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            int count = reader.ReadInt32();

            Skybox skybox = new Skybox();
            ParalaxLayer layer = new ParalaxLayer();

            for (int i = 0; i < count; i++)
            {
                skybox.Layers.Add(layer.Deserialize(stream));
            }

            return skybox;
        }
    }
}
