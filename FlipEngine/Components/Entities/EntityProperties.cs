

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace FlipEngine
{
    public abstract partial class Entity : IComponent, ILayeredComponentActive, ISerializable<Entity>
    {
        public Rectangle CollisionFrame => new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        public Rectangle PreCollisionFrame => new Rectangle((int)OldPosition.X, (int)OldPosition.Y, Width, Height);
        public Point ChunkPosition => Main.World.tileManager.ToChunkCoords(Position.ToPoint());
        public Point OldChunkPosition => Main.World.tileManager.ToChunkCoords(OldPosition.ToPoint());
        public Chunk Chunk => Main.World.tileManager.chunks[ChunkPosition.X, ChunkPosition.Y];
        public Chunk OldChunk => Main.World.tileManager.chunks[OldChunkPosition.X, OldChunkPosition.Y];
        public Vector2 DeltaPos => Position - OldPosition;
        public Vector2 ParallaxPosition => Position.AddParallaxAcrossX(Main.layerHandler.Layers[Layer].parallax);
        public Vector2 Center
        {
            get
            {
                return new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
            }
            set
            {
                Position = new Vector2(value.X - Width * 0.5f, value.Y - Height * 0.5f);
            }
        }
    }
}
