using Flipsider.Engine.Interfaces;
using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Flipsider
{
    public abstract partial class Entity : IComponent, ILayeredComponentActive, ISerializable<Entity>
    {
        public Rectangle CollisionFrame => new Rectangle((int)position.X, (int)position.Y, width, height);
        public Rectangle PreCollisionFrame => new Rectangle((int)oldPosition.X, (int)oldPosition.Y, width, height);
        public Point ChunkPosition => Main.CurrentWorld.tileManager.ToChunkCoords(position.ToPoint());
        public Point OldChunkPosition => Main.CurrentWorld.tileManager.ToChunkCoords(oldPosition.ToPoint());
        public Chunk Chunk => Main.CurrentWorld.tileManager.chunks[ChunkPosition.X, ChunkPosition.Y];
        public Chunk OldChunk => Main.CurrentWorld.tileManager.chunks[OldChunkPosition.X, OldChunkPosition.Y];
        public Vector2 DeltaPos => position - oldPosition;
        public Vector2 ParallaxPosition => position.AddParallaxAcrossX(Main.layerHandler.Layers[Layer].parallax);
        public Vector2 Center
        {
            get
            {
                return new Vector2(position.X + width * 0.5f, position.Y + height * 0.5f);
            }
            set
            {
                position = new Vector2(value.X - width * 0.5f, value.Y - height * 0.5f);
            }
        }
    }
}
