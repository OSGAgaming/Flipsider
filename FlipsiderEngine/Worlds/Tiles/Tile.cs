using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Text;

namespace Flipsider.Worlds.Tiles
{
    public struct Tile
    {
        internal Tile(string name, Point pos)
        {
            this.name = name.Trim();
            Pos = pos;
            state = 0;
        }

        private long state;
        private string? name;
        public string Name
        {
            get => name ??= string.Empty;
            set => name = value.Trim();
        }
        public Point Pos { get; }
        public bool IsEmpty => Name.Length == 0;

        public void Serialize(Stream stream)
        {
            if (IsEmpty)
                return;
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(Name);
            writer.Write(Pos.X);
            writer.Write(Pos.Y);
            writer.Write(state);
        }

        public static Tile Deserialize(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            string name = reader.ReadString();
            int x = reader.ReadInt32();
            int y = reader.ReadInt32();
            long state = reader.ReadInt64();
            return new Tile(name, new Point(x, y)) { state = state };
        }

        public override bool Equals(object? obj)
        {
            return obj is Tile t && (Pos == t.Pos || Name.Length == 0 && t.Name.Length == 0);
        }

        public override int GetHashCode()
        {
            return Pos.GetHashCode();
        }

        public static bool operator ==(Tile a, Tile b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Tile a, Tile b)
        {
            return !(a == b);
        }
    }
}