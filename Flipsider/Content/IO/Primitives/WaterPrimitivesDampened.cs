using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using FlipEngine;

namespace Flipsider
{
    internal class WaterPrimitivesDampened : Primitive
    {
        private Vector2[] Points;
        private Water water;
        public WaterPrimitivesDampened(Water water)
        {
            this.water = water;
            Points = new Vector2[water.Pos.Length];
        }
        public override void SetDefaults()
        {
            Alpha = 0.7f;
            Width = 1;
            PrimitiveCount = 1000;
        }
        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            int dXY = 5;
            Color c = water.color;
            for (int i = 0; i < _points.Count - 1; i++)
            {
                Color colour = c;
                Color colour2 = c;
                if (i < _points.Count - 2)
                {
                    float aD = _points[i + 1].Y - _points[i].Y;
                    float aD2 = _points[i + 2].Y - _points[i + 1].Y;
                    colour = Color.Lerp(c, Color.Black, aD / 2.5f);
                    colour2 = Color.Lerp(c, Color.Black, aD2 / 2.5f);
                }
                AddVertex(_points[i] + new Vector2(dXY, -dXY), colour, new Vector2(i / (float)(_points.Count), 0));
                AddVertex(new Vector2(_points[i + 1].X, Points[i + 1].Y + 2), colour2, new Vector2((i + 1) / (float)(_points.Count), 1));
                AddVertex(new Vector2(_points[i].X, Points[i].Y + 2), colour, new Vector2(i / (float)(_points.Count), 1));

                AddVertex(_points[i] + new Vector2(dXY, -dXY), colour, new Vector2(i / (float)(_points.Count), 0));
                AddVertex(_points[i + 1] + new Vector2(dXY, -dXY), colour2, new Vector2((i + 1) / (float)(_points.Count), 0));
                AddVertex(new Vector2(_points[i + 1].X, Points[i + 1].Y + 2), colour2, new Vector2((i + 1) / (float)(_points.Count), 1));
            }
        }
        public override void SetShaders()
        {
            PrepareShader(EffectCache.PrimtiveShader ?? new BasicEffect(FlipGame.graphics.GraphicsDevice), "WaterDamp", TimeAlive / 40f);
        }
        public override void OnUpdate()
        {
            _points = water.PosDampened.ToList();
            Points = water.Pos;
            VertexCount = _points.Count() * 6;
            if (PrimitiveCount < VertexCount / 6)
            {
                _points.RemoveAt(0);
            }
        }
        public override void OnDestroy()
        {
            Width *= 0.9f;
            if (Width < 0.05f)
            {
                Dispose();
            }
        }
    }
}