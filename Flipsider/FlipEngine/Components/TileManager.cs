using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace FlipEngine
{
    public partial class TileManager : ISerializable<TileManager>
    {
        public const int tileRes = 32;
        public static List<Tile> tileTypes = new List<Tile>();
        public static Dictionary<int, Texture2D> tileDict = new Dictionary<int, Texture2D>();
        public Chunk[,] chunks;
        private Chunk voidChunk;

        public TileManager(int width, int height)
        {
            chunks = new Chunk[width / Chunk.width, height / Chunk.height];
            voidChunk = new Chunk();
            for (int i = 0; i < chunks.GetLength(0); i++)
            {
                for (int j = 0; j < chunks.GetLength(1); j++)
                {
                    chunks[i, j] = LoadChunk(new Point(i, j));
                }
            }
            //LoadTileTypes();
        }

        public static void AddTileType(int type, Texture2D atlas)
        {
            tileTypes.Add(new Tile(type, new Rectangle(0, 0, 32, 32)));
            tileDict.Add(type, atlas);
        }

        Chunk LoadChunk(Point pos)
        {
            try
            {
                chunks[pos.X, pos.Y] = new Chunk(pos);
                return chunks[pos.X, pos.Y];
            }
            catch
            {
                return voidChunk;
            }
        }
        public Chunk GetChunk(Point pos)
        {
            if (pos.X < chunks.GetLength(0) && pos.Y < chunks.GetLength(1))
                return chunks?[pos.X, pos.Y] ?? LoadChunk(pos);

            return LoadChunk(pos);
        }
        public Chunk GetChunkToTileCoords(Point pos)
        {
            return GetChunk(TileCoordsToChunkCoords(pos)) ?? LoadChunk(TileCoordsToChunkCoords(pos));
        }
        public Chunk GetChunkToWorldCoords(Vector2 pos)
        {
            Point p = new Point((int)pos.X / 32, (int)pos.Y / 32);
            return GetChunk(TileCoordsToChunkCoords(p)) ?? LoadChunk(TileCoordsToChunkCoords(p));
        }
        public Point ToChunkCoords(Point pos)
        {
            return new Point(pos.X / (Chunk.width * 32), pos.Y / (Chunk.height * 32));
        }
        public Point TileCoordsToChunkCoords(Point pos)
        {
            return new Point(pos.X / Chunk.width, pos.Y / Chunk.height);
        }
        public Tile GetTile(Point pos)
        {
            var Chunk = GetChunk(TileCoordsToChunkCoords(pos));
            return Chunk.tiles[pos.X % Chunk.width, pos.Y % Chunk.height];
        }
        public Tile GetTile(int i, int j)
        {
            Point pos = new Point(i, j);
            var Chunk = GetChunk(TileCoordsToChunkCoords(pos));
            return Chunk.tiles[pos.X % Chunk.width, pos.Y % Chunk.height];
        }
        public void AddTileToChunk(Tile tile, Point point)
        {
            Point pos = new Point(point.X % Chunk.width, point.Y % Chunk.height);
            GetChunkToTileCoords(point).AddTile(tile, pos);
        }
        public static bool CanPlace = true;
        public Tile AddTile(World world, Tile T, bool forcePlacement = false)
        {
            try
            {
                Point pos = new Point(T.i, T.j);
                if ((world.IsTileActive(pos.X, pos.Y) && GetTile(pos).type != T.type) || !world.IsTileActive(pos.X, pos.Y))
                {
                    if (CanPlace || forcePlacement)
                    {
                        if (world.IsTileActive(pos.X, pos.Y))
                        {
                            GetTile(pos).Dispose();
                            AddTileToChunk(T, pos);
                        }
                        else
                        {
                            AddTileToChunk(T, pos);
                        }
                    }

                    if (TileScreen.AutoFrame)
                    {
                        for (int i = pos.X - 1; i < pos.X + 2; i++)
                            for (int j = pos.Y - 1; j < pos.Y + 2; j++)
                            {
                                Point position = new Point(i, j);
                                if (i > 0 && j > 0 && i < world.MaxTilesX && j < world.MaxTilesY && GetTile(position) != null)
                                {
                                    GetTile(position).frame = Framing.GetTileFrame(world, i, j);
                                }
                            }
                    }
                    CanPlace = true;
                }
                else
                {
                    T.Active = false;
                }
            }
            catch
            {

            }
            return T;
        }

        public void RemoveTile(World world, Point pos)
        {
            try
            {
                if (EditorModeGUI.Active)
                {
                    if (GetTile(pos) != null)
                        GetTile(pos).Dispose();
                }
                if (TileScreen.AutoFrame)
                {
                    for (int i = pos.X - 1; i < pos.X + 2; i++)
                        for (int j = pos.Y - 1; j < pos.Y + 2; j++)
                        {
                            Point point = new Point(i, j);
                            if (i > 0 && j > 0 && i < world.MaxTilesX && j < world.MaxTilesY && GetTile(point) != null)
                            {
                                GetTile(point).frame = Framing.GetTileFrame(world, i, j);
                            }
                        }
                }
            }
            catch
            {

            }
        }

        public void Serialize(Stream stream)
        {
            BinaryWriter binaryWriter = new BinaryWriter(stream);
            binaryWriter.Write(chunks.GetLength(0));
            binaryWriter.Write(chunks.GetLength(1));
            for (int i = 0; i < chunks.GetLength(0); i++)
            {
                for (int j = 0; j < chunks.GetLength(1); j++)
                {
                    Chunk chunk = chunks[i, j];
                    chunk.Serialize(stream);
                }
            }
        }

        public TileManager Deserialize(Stream stream)
        {
            BinaryReader binaryReader = new BinaryReader(stream);
            int width = binaryReader.ReadInt32();
            int height = binaryReader.ReadInt32();
            TileManager tileManager = new TileManager(width * Chunk.width, height * Chunk.height);
            FlipGame.World.tileManager = tileManager;

            Chunk chunk = new Chunk();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    chunk.Deserialize(stream);
                }
            }
            return tileManager;
        }

        public static bool UselessCanPlaceBool;
    }
}
