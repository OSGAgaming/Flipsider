using Flipsider.Entities;
using FMOD;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Flipsider.Tiles
{
    public class Chunk
    {
        public const int Width = 50;
        public const int Height = Width;

        private Chunk(Point pos)
        {
            Pos = pos;
        }

        private readonly Tile[] tiles = new Tile[Width * Height];

        public Point Pos { get; }

        public ref Tile this[int x, int y] => ref tiles[x + Width * y];
        public ref Tile this[Point p] => ref tiles[p.X + Width * p.Y];

        public Rectangle ToRectangle() => new Rectangle(Pos.X, Pos.Y, Width, Height);

        public static Point ToChunkCoords(Vector2 position)
        {
            return (position / new Vector2(Width, Height)).ToPoint();
        }

        private void LoadEntity()
        {
            // TODO load
            // int id = get entity id..
            // Entity e = get entity instance..
            // FlipsiderGame.Instance.CurrentWorld!.Tiles.SpawnStaticEntity(this, id, e)
        }

        public static Chunk Load(Point p)
        {
            var @this = new Chunk(p);
            // TODO get an actual chunk loading system.. lmao
            return @this;
        }

        public void Save()
        {
            // TODO get an actual chunk saving system
        }
    }
}