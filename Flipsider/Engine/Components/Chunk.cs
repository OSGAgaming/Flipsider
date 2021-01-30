
using Flipsider.Engine.Interfaces;
using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static Flipsider.PropManager;

namespace Flipsider
{
    public class Chunk : IUpdate
    {
        public const int width = 62;
        public const int height = 34;

        public List<Entity> Entities = new List<Entity>();

        public Tile[,] tiles = new Tile[width, height];

        public Point pos;

        public bool Active;

        public CollideableHanlder Colliedables;
        public Chunk(Point point)
        {
            Main.Updateables.Add(this);
            Colliedables = new CollideableHanlder();
            pos = point;
            Active = false;
        }

        public void AddTile(Tile tile, Point pos)
        {
            tiles[pos.X, pos.Y] = tile;
            tiles[pos.X, pos.Y].Active = true;
        }

        public bool CheckActivity()
        {
            Point PlayerChunkPos = Main.player.ChunkPosition;
            return pos.X >= PlayerChunkPos.X - 1 &&
                   pos.X <= PlayerChunkPos.X + 1 &&
                   pos.Y >= PlayerChunkPos.Y - 1 &&
                   pos.Y <= PlayerChunkPos.Y + 1;
        }
        public void Update()
        {
            if(CheckActivity())
            Active = true;

            if (!Main.Editor.IsActive)
            {
                if (Active)
                {
                    foreach (Entity entity in Entities.ToArray())
                    {
                        if(entity.Active)
                        entity.Update();
                    }
                }
            }
            if (Active)
            {
                foreach (Entity entity in Entities.ToArray())
                {
                    if (entity.Active)
                    entity.UpdateInEditor();
                }
            }
        }
    }
}
