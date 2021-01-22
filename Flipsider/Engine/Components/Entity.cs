using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Flipsider
{
    [Serializable]
    public abstract class Entity : IComponent, ILayeredComponent
    {
        [NonSerialized]
        public Texture2D texture = TextureCache.magicPixel;
        public int Layer { get; set; }
        public bool Active { get; set; }
        [NonSerialized]
        public Vector2 position;
        [NonSerialized]
        public Vector2 oldPosition;
        public int height;
        public int maxHeight;
        public int width;
        public Rectangle CollisionFrame => new Rectangle((int)position.X, (int)position.Y + maxHeight - height, width, height);
        public Rectangle PreCollisionFrame => new Rectangle((int)oldPosition.X, (int)oldPosition.Y + maxHeight - height, width, height);
        protected virtual void PreDraw(SpriteBatch spriteBatch) { }
        protected virtual void OnDraw(SpriteBatch spriteBatch) { }
        protected virtual void PostDraw(SpriteBatch spriteBatch) { }
        protected virtual void PreUpdate() { }
        protected virtual void OnUpdate() { }
        protected virtual void PostUpdate() { }
        protected virtual void OnLoad() { }
        [NonSerialized]
        protected readonly HashSet<IEntityModifier> UpdateModules = new HashSet<IEntityModifier>();
        public void AddModule(IEntityModifier IEM) => UpdateModules.Add(IEM);
        public Entity()
        {
            OnLoad();
            if (Main.CurrentWorld != null)
            {
                Main.CurrentWorld.entityManager.AddComponent(this);
                Main.AutoAppendToLayer(this);
            }
        }
        protected void UpdateEntityModifiers()
        {
            foreach (IEntityModifier IEM in UpdateModules)
            {
                IEM.Update(this);
            }
        }
        public virtual void Update()
        {
            oldPosition = position;
            PreUpdate();
            OnUpdate();
            UpdateEntityModifiers();
            PostUpdate();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            PreDraw(spriteBatch);

            OnDraw(spriteBatch);

            PostDraw(spriteBatch);
        }

    }
}
