using Flipsider.Engine.Interfaces;
using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Flipsider
{
    [Serializable]
    public abstract class Entity : IComponent, ILayeredComponentActive
    {
        public bool InFrame { get; set; }
        private Vector2 ParallaxedIJ => position.AddParallaxAcrossX(Main.layerHandler.Layers[Layer].parallax);

        private int ParallaxedI => (int)ParallaxedIJ.X;

        private Vector2 SafeBoundX => new Vector2(Main.mainCamera.CamPos.X, Main.mainCamera.CamPos.X + Main.ActualScreenSize.X / Main.ScreenScale);

        private Vector2 SafeBoundY => new Vector2(Main.mainCamera.CamPos.Y, Main.mainCamera.CamPos.Y + Main.ActualScreenSize.Y / Main.ScreenScale);
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

        public Vector2 DeltaPos => position - oldPosition; 
        protected virtual void PreDraw(SpriteBatch spriteBatch) { }
        protected virtual void OnDraw(SpriteBatch spriteBatch) { }
        protected virtual void PostDraw(SpriteBatch spriteBatch) { }
        protected virtual void PreUpdate() { }
        protected virtual void OnUpdate() { }
        protected virtual void PostUpdate() { }
        protected virtual void OnLoad() { }
        [NonSerialized]
        protected readonly Dictionary<string,IEntityModifier> UpdateModules = new Dictionary<string,IEntityModifier>();
        public void AddModule(string name,IEntityModifier IEM) => UpdateModules.Add(name,IEM);
        public Entity()
        {
            OnLoad();
            if (Main.CurrentWorld != null)
            {
                Main.CurrentWorld.entityManager.AddComponent(this);
                Main.AutoAppendToLayer(this);
            }
        }
        protected void UpdateEntityModifier(string name)
        {
            UpdateModules[name].Update(this);
        }

        public virtual void Update()
        {
            oldPosition = position;
            PreUpdate();
            OnUpdate();
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
