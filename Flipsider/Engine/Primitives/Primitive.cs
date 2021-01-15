
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System.Collections.Generic;

using System.Reflection;
using Flipsider.Engine.Interfaces;
using System.Diagnostics;

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
        public bool behindTiles = false;
        protected GraphicsDevice _device;
        protected Effect _effect;
        protected BasicEffect _basicEffect;
        protected int RENDERDISTANCE => 2000;
        protected VertexPositionColorTexture[] vertices;
        protected int currentIndex;
        public Primitive()
        {
            _effect = Lighting.PrimtiveShader ?? new BasicEffect(_device);
            _trailShader = new DefaultShader();
            _device = Main.graphics.GraphicsDevice;
            _basicEffect = new BasicEffect(_device);
            _basicEffect.VertexColorEnabled = true;
            SetDefaults();
            vertices = new VertexPositionColorTexture[_cap];
        }


        public void Dispose()
        {
            //EEMod.primitives._trails.Remove(this);
        }
        public void Update()
        {
            OnUpdate();
        }
        public virtual void OnUpdate()
        {

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
        public virtual void PrimStructure(SpriteBatch spriteBatch)
        {

        }
        public virtual void SetShaders()
        {

        }
        public virtual void SetDefaults()
        {

        }

        public virtual void OnDestroy()
        {

        }
        //Helper methods
    }
}