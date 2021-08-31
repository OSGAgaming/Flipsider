using Flipsider.Engine;
using Flipsider.Engine.Interfaces;
using Flipsider.Engine.Particles;
using Flipsider.GUI.TilePlacementGUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Flipsider
{
    public struct ParalaxLayer
    {
        public int Priority;
        public float Parallax;
        public float Scale;
        public string? Path;
        public Vector2 Offset;

        public ParalaxLayer(string? Path, float Parallax = 1, int Priority = 0, Vector2 Offset = default, float Scale = 1)
        {
            this.Path = Path;
            this.Priority = Priority;
            this.Parallax = Parallax;
            this.Scale = Scale;
            this.Offset = Offset;
        }

        public void Serialize(Stream stream)
        {
            BinaryWriter writer = new BinaryWriter(stream);

            writer.Write(Priority);
            writer.Write(Parallax);
            writer.Write(Scale);
            if(Path != null) writer.Write(Path);
            writer.Write(Offset);
        }

        public ParalaxLayer Deserialize(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);

            int Priority = reader.ReadInt32();
            float Parallax = reader.ReadSingle();
            float Scale = reader.ReadSingle();
            string Path = reader.ReadString();
            Vector2 Offset = reader.ReadVector2();

            return new ParalaxLayer(Path, Parallax, Priority, Offset, Scale);
        }
    }
}
