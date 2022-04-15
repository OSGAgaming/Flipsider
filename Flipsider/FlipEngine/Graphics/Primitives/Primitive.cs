using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FlipEngine
{
    public partial class Primitive : IComponent
    {
        protected Effect Effect { get; set; }
        protected int IndexPointer { get; set; }
        protected VertexPositionColorTexture[] vertices { get; set; }
        protected float Width { get; set; }
        protected int TimeAlive { get; set; }
        protected float Alpha { get; set; }

        protected int PrimitiveCount { get; set; }
        protected int VertexCount { get; set; }

        protected ITrailShader _trailShader { get; private set; } = new DefaultShader();

        protected List<Vector2> _points = new List<Vector2>();

        protected GraphicsDevice _device => FlipGame.graphics.GraphicsDevice;
        protected BasicEffect _basicEffect => EffectCache.BasicEffect;

        public Primitive()
        {
            Effect = EffectCache.PrimtiveShader ?? new BasicEffect(_device);
            vertices = new VertexPositionColorTexture[PrimitiveCount];
            SetDefaults();
            FlipE.Updateables.Add(this);
        }


        public void Dispose()
        {
            FlipGame.Renderer.Primitives.Components.Remove(this);
        }
        public void Update()
        {
            TimeAlive++;
            OnUpdate();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            vertices = new VertexPositionColorTexture[VertexCount];
            IndexPointer = 0;
            PrimStructure(spriteBatch);
            SetShaders();
            if (VertexCount >= 1) _device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, VertexCount / 3);
        }
        public virtual void PrimStructure(SpriteBatch spriteBatch) { }
        public virtual void SetShaders() { }
        public virtual void SetDefaults() { }
        public virtual void OnDestroy() { }
        public virtual void OnUpdate() { }

        //Helper methods
    }
}