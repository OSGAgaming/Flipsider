
using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Flipsider.Engine
{
    public class PrimTrailManager : Manager<Primitive>
    {


    }
    public partial class Primitive : IComponent
    {
        protected float _width;
        protected float _alphaValue;
        protected int _cap;
        protected ITrailShader _trailShader;
        protected int _counter;
        protected int _noOfPoints;
        protected List<Vector2> _points = new List<Vector2>();
        protected bool _destroyed = false;
        protected GraphicsDevice _device;
        protected Effect _effect;
        protected BasicEffect _basicEffect;
        protected VertexPositionColorTexture[] vertices;
        protected int currentIndex;
        public Primitive()
        {
            _effect = EffectCache.PrimtiveShader ?? new BasicEffect(_device);
            _trailShader = new DefaultShader();
            _device = Main.graphics.GraphicsDevice;
            _basicEffect = new BasicEffect(_device)
            {
                VertexColorEnabled = true
            };
            SetDefaults();
            vertices = new VertexPositionColorTexture[_cap];
        }


        public void Dispose()
        {
            Main.Primitives.Components.Remove(this);
        }
        public void Update()
        {
            OnUpdate();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            vertices = new VertexPositionColorTexture[_noOfPoints];
            currentIndex = 0;
            PrimStructure(spriteBatch);
            SetShaders();
            if (_noOfPoints >= 1)
                _device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, _noOfPoints / 3);
        }
        public virtual void PrimStructure(SpriteBatch spriteBatch) { }
        public virtual void SetShaders() { }
        public virtual void SetDefaults() { }
        public virtual void OnDestroy() { }
        public virtual void OnUpdate() { }

        //Helper methods
    }
}