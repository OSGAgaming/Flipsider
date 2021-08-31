using Flipsider.Engine.Interfaces;
using Flipsider.Engine.Maths;
using Flipsider.GUI.TilePlacementGUI;
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
        public bool InFrame { get; set; }
        public int Layer { get; set; }
        public bool Active { get; set; }
        public Texture2D Texture { get; set; } = TextureCache.magicPixel;

        public Vector2 Position;
        public Vector2 OldPosition;
        public int Height;
        public int Width;
        public Vector2 Size => new Vector2(Width, Height);

        protected virtual void PreDraw(SpriteBatch spriteBatch) { }
        protected virtual void OnDraw(SpriteBatch spriteBatch) { }
        protected virtual void PostDraw(SpriteBatch spriteBatch) { }
        protected virtual void PreUpdate() { }
        protected virtual void OnUpdate() { }
        protected virtual void PostUpdate() { }
        protected virtual void OnLoad() { }
        protected virtual void PostConstructor() { }
        public virtual void Dispose() { }
        public virtual void Serialize(Stream stream) { }
        protected virtual void OnChunkChange() { }
        public virtual void OnUpdateInEditor() { }


        public readonly Dictionary<string,IEntityModifier> UpdateModules = new Dictionary<string,IEntityModifier>();
        public void AddModule(string name, IEntityModifier IEM){ if(!UpdateModules.ContainsKey(name)) UpdateModules.Add(name, IEM); }
        public void UpdateInEditor()
        {
            OnUpdateInEditor();
        }

        public T GetEntityModifier<T>() where T : IEntityModifier
        {
            if (Active)
            {
                foreach (KeyValuePair<string, IEntityModifier> kvp in UpdateModules)
                {
                    if (kvp.Value is T) return (T)kvp.Value;
                }
            }

            throw new Exception("Entity Modifier Doesnt Exist");
        }

        public Entity()
        {
            OnLoad();
            ActionCache.Instance.EntityQueue.Add(this);
            Main.LoadQueue += AfterLoad;
        }
        protected void UpdateEntityModifier(string name)
        {
            if(UpdateModules.ContainsKey(name))
            UpdateModules[name].Update(this);
        }

        public void TransferChunk(Chunk chunk1, Chunk chunk2)
        {
            if (!chunk2.Entities.Contains(this))
                chunk2.Entities.Add(this);

            chunk1.Entities.Remove(this);
        }
        public void Update()
        {
            if (OldChunkPosition != ChunkPosition)
            {
                TransferChunk(OldChunk, Chunk);
                OnChunkChange();
            }

            OldPosition = Position;

            PreUpdate();
            OnUpdate();
            PostUpdate();
        }
        public void AfterLoad()
        {
            if (Active)
            {
                PostConstructor();
                if (Main.World != null)
                {
                    Main.AppendToLayer(this);
                    Chunk?.Entities.Add(this);
                }
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            PreDraw(spriteBatch);

            OnDraw(spriteBatch);

            PostDraw(spriteBatch);
        }


        public virtual Entity Deserialize(Stream stream)
        {
            return this;
        }
    }
}
