
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System;
using Flipsider.Engine;
using System.Diagnostics;

namespace Flipsider
{
    class LightPrimitives : Primitive
    {
        DirectionalLight light;
        public LightPrimitives(DirectionalLight light)
        {
            this.light = light;
        }
        public override void SetDefaults()
        {
            _alphaValue = 0.7f;
            _width = 1;
            _cap = 1000;
        }
        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            Color colour = light.colour;
            for (int i = 0; i < _points.Count - 1; i++)
            {
                AddVertex(new Vector2(light.position.X, light.position.Y), colour, new Vector2(0,0.5f));
                AddVertex(light.points[i], colour, new Vector2((light.points[i] - light.position).Length()/light.strength, i / (float)(_points.Count)));
                AddVertex(light.points[i + 1], colour, new Vector2((light.points[i + 1] - light.position).Length() / light.strength, i / (float)(_points.Count)));
            }
        }
        public override void SetShaders()
        {
            PrepareShader(Lighting.PrimtiveShader ?? new BasicEffect(Main.graphics.GraphicsDevice), "DirLight", _counter / 40f);
        }
        public override void OnUpdate()
        {
            _points = light.points.ToList();
            _counter++;
            _noOfPoints = _points.Count() * 6;
            if (_cap < _noOfPoints / 6)
            {
                _points.RemoveAt(0);
            }
        }
        public override void OnDestroy()
        {
            _width *= 0.9f;
            if (_width < 0.05f)
            {
                Dispose();
            }
        }
    }
}