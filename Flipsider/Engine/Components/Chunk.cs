
using Flipsider.Engine.Interfaces;
using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Flipsider
{
    [Serializable]
    public class Chunk : IUpdate,ISerializable<Chunk>
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
        public Chunk()
        {
            Colliedables = new CollideableHanlder();
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
        public void Serialize(Stream stream)
        {
            BinaryWriter binaryWriter = new BinaryWriter(stream);
            binaryWriter.Write(pos.X);
            binaryWriter.Write(pos.Y);
            binaryWriter.Write(Entities.Count);
            for (int i = 0; i < Entities.Count; i++)
            {
                binaryWriter.Write(Entities[i].GetType().FullName ?? "Empty");
                if(Entities[i] != null)
                Entities[i].Serialize(stream);
            }
        }
        public Chunk Deserialize(Stream stream)
        {
            BinaryReader binaryReader = new BinaryReader(stream);

            Point pos = new Point(binaryReader.ReadInt32(), binaryReader.ReadInt32());
            int EntityLength = binaryReader.ReadInt32();

            Chunk chunk = new Chunk(pos);
            for(int i = 0; i<EntityLength; i++)
            {
                string typeName = binaryReader.ReadString();
                    Type? type = Type.GetType(typeName);
                  if (type != null)
                  {
                     Entity? entity = Activator.CreateInstance(type) as Entity;
                     if (entity != null)
                     {
                        entity.Deserialize(stream);
                     }
                  }
            }
            return chunk;
        }

        public void Update()
        {
            if(CheckActivity())
            Active = true;
            else
            {
                Active = false;
                return;
            }

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
