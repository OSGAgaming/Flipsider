
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System;
using Flipsider.Engine;
using System.Diagnostics;

namespace Flipsider
{
    class WaterPrimitivesDampened : Primitive
    {
        Vector2[] Points;
        Water water;
        public WaterPrimitivesDampened(Water water)
        {
            this.water = water;
            Points = new Vector2[water.Pos.Length];
        }
        public override void SetDefaults()
        {
            _alphaValue = 0.7f;
            _width = 1;
            _cap = 1000;
        }
        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            int dXY = 5;
            Color colour = water.color;
            for (int i = 0; i < _points.Count - 1; i++)
            {
                AddVertex(_points[i] + new Vector2(dXY, -dXY), colour, new Vector2(i/(float)(_points.Count),0));
                AddVertex(new Vector2(_points[i + 1].X, Points[i + 1].Y + 2), colour, new Vector2((i + 1) / (float)(_points.Count),1));
                AddVertex(new Vector2(_points[i].X, Points[i].Y + 2), colour, new Vector2(i / (float)(_points.Count), 1));

                AddVertex(_points[i] + new Vector2(dXY, -dXY), colour, new Vector2(i / (float)(_points.Count), 0));
                AddVertex(_points[i + 1] + new Vector2(dXY, -dXY), colour, new Vector2((i + 1) / (float)(_points.Count), 0));
                AddVertex(new Vector2(_points[i + 1].X, Points[i+1].Y + 2), colour, new Vector2((i + 1) / (float)(_points.Count), 1));
            }
        }
        public override void SetShaders()
        {
            PrepareShader(Lighting.PrimtiveShader ?? new BasicEffect(Main.graphics.GraphicsDevice), "WaterDamp", _counter / 40f);
        }
        public override void OnUpdate()
        {
            _points = water.PosDampened.ToList();
            Points = water.Pos;
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