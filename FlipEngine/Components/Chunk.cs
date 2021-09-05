

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FlipEngine
{
    [Serializable]
    public class Chunk : IUpdate, ISerializable<Chunk>
    {
        public const int width = 62;
        public const int height = 34;

        public List<Entity> Entities = new List<Entity>();

        public Tile[,] tiles = new Tile[width, height];

        public Point pos;

        public bool Active;

        public CollideableHanlder Colliedables;
        public HitBoxHandler HitBoxes;
        public Chunk(Point point)
        {
            Main.Updateables.Add(this);
            Colliedables = new CollideableHanlder();
            HitBoxes = new HitBoxHandler();
            pos = point;
            Active = false;
        }
        public Chunk()
        {
            Colliedables = new CollideableHanlder();
            HitBoxes = new HitBoxHandler();
            Active = false;
        }
        public void AddTile(Tile tile, Point pos)
        {
            tiles[pos.X, pos.Y] = tile;
            tiles[pos.X, pos.Y].Active = true;
        }

        public bool CheckActivity()
        {
            /*Point PlayerChunkPos = Main.player.ChunkPosition;
            return pos.X >= PlayerChunkPos.X - 1 &&
                   pos.X <= PlayerChunkPos.X + 1 &&
                   pos.Y >= PlayerChunkPos.Y - 1 &&
                   pos.Y <= PlayerChunkPos.Y + 1;
            */
            return true;
        }
        public void Serialize(Stream stream)
        {
            BinaryWriter binaryWriter = new BinaryWriter(stream);
            binaryWriter.Write(pos);
            binaryWriter.Write(Entities.Count);
            for (int i = 0; i < Entities.Count; i++)
            {
                binaryWriter.Write(Entities[i].GetType());
                if (Entities[i] != null)
                    Entities[i].Serialize(stream);
            }
        }
        public Chunk Deserialize(Stream stream)
        {
            BinaryReader binaryReader = new BinaryReader(stream);

            Point pos = binaryReader.ReadPoint();
            int EntityLength = binaryReader.ReadInt32();

            Chunk chunk = new Chunk(pos);
            for (int i = 0; i < EntityLength; i++)
            {
                Type? type = binaryReader.ReadType();

                if (type != null)
                {
                    Entity? entity = Activator.CreateInstance(type) as Entity;
                    entity?.Deserialize(stream);
                }
            }
            return chunk;
        }

        public void Update()
        {
            if (CheckActivity())
                Active = true;
            else
            {
                Active = false;
                return;
            }
            if (!EditorModeGUI.Active)
            {
                if (Active)
                {
                    foreach (Entity entity in Entities.ToArray())
                    {
                        if (entity.Active)
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
