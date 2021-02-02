﻿using Flipsider.Engine.Interfaces;
using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Flipsider
{
    [Serializable]
    public abstract class Entity : IComponent, ILayeredComponentActive
    {
        public bool InFrame { get; set; }
        private Vector2 ParallaxedIJ => position.AddParallaxAcrossX(Main.layerHandler.Layers[Layer].parallax);
        protected int ParallaxedI => (int)ParallaxedIJ.X;

        [NonSerialized]
        public Texture2D texture = TextureCache.magicPixel;
        public int Layer { get; set; }
        public bool Active { get; set; }
        [NonSerialized]
        public Vector2 position;
        [NonSerialized]
        public Vector2 oldPosition;
        public int height;
        public int width;
        public Rectangle CollisionFrame => new Rectangle((int)position.X, (int)position.Y, width, height);
        public Rectangle PreCollisionFrame => new Rectangle((int)oldPosition.X, (int)oldPosition.Y, width, height);
        public Point ChunkPosition => Main.CurrentWorld.tileManager.ToChunkCoords(position.ToPoint());
        public Point OldChunkPosition => Main.CurrentWorld.tileManager.ToChunkCoords(oldPosition.ToPoint());
        public Chunk Chunk => Main.CurrentWorld.tileManager.chunks[ChunkPosition.X, ChunkPosition.Y];
        public Chunk OldChunk => Main.CurrentWorld.tileManager.chunks[OldChunkPosition.X, OldChunkPosition.Y];
        public Vector2 DeltaPos => position - oldPosition; 
        protected virtual void PreDraw(SpriteBatch spriteBatch) { }
        protected virtual void OnDraw(SpriteBatch spriteBatch) { }
        protected virtual void PostDraw(SpriteBatch spriteBatch) { }
        protected virtual void PreUpdate() { }
        protected virtual void OnUpdate() { }
        protected virtual void PostUpdate() { }
        protected virtual void OnLoad() { }
        [NonSerialized]
        public readonly Dictionary<string,IEntityModifier> UpdateModules = new Dictionary<string,IEntityModifier>();
        public void AddModule(string name, IEntityModifier IEM)
        { if(!UpdateModules.ContainsKey(name)) UpdateModules.Add(name, IEM); }
        public virtual void UpdateInEditor() {  }
        public Entity()
        {
            OnLoad();
            if (Main.CurrentWorld != null)
            {
                Chunk?.Entities.Add(this);
                Main.AutoAppendToLayer(this);
            }
        }
        protected void UpdateEntityModifier(string name)
        {
            UpdateModules[name].Update(this);
        }
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

        public void TransferChunk(Chunk chunk1, Chunk chunk2)
        {
            if (!chunk2.Entities.Contains(this))
                chunk2.Entities.Add(this);
            chunk1.Entities.Remove(this);
        }
        public void Update()
        {
            oldPosition = position;
            PreUpdate();
            OnUpdate();
            PostUpdate();
            if (OldChunkPosition != ChunkPosition)
            {
                TransferChunk(OldChunk, Chunk);
                OnChunkChange();
            }
        }
        protected virtual void OnChunkChange()
        {

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            PreDraw(spriteBatch);

            OnDraw(spriteBatch);

            PostDraw(spriteBatch);
        }

    }
}