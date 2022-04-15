

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
        public RectangleF CollisionFrame => new RectangleF(Position, new Vector2(Width, Height));
        public Rectangle PreCollisionFrame => new Rectangle((int)OldPosition.X, (int)OldPosition.Y, Width, Height);
        public Point ChunkPosition => TileManager.ToChunkCoords(GetCameraClampedPosition().ToPoint());
        public Point OldChunkPosition => TileManager.ToChunkCoords(GetCameraClampedPosition(false).ToPoint());
        public Chunk Chunk => FlipGame.World.tileManager.chunks[ChunkPosition.X, ChunkPosition.Y];
        public Chunk OldChunk => FlipGame.World.tileManager.chunks[OldChunkPosition.X, OldChunkPosition.Y];
        public Vector2 GetCameraClampedPosition(bool New = true)
        {
            Vector2 p = New ? Position : OldPosition;
            Vector2 cam = FlipGame.Camera.Position;

            if (cam.X > p.X && cam.X < CollisionFrame.right) p.X = cam.X;
            if (cam.Y > p.Y && cam.Y < CollisionFrame.bottom) p.Y = cam.Y;

            return p;
        }
        public bool WithinChunk => ChunkPosition.X >= 0 && ChunkPosition.Y >= 0;
        public Vector2 DeltaPos => Position - OldPosition;
        public Vector2 ParallaxPosition => Position.AddParallaxAcrossX(FlipGame.layerHandler.Layers[Layer].parallax);
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
