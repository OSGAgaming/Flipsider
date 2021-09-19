
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using FlipEngine;

namespace Flipsider
{
    internal class WaterPrimtives : Primitive
    {
        private Vector2[] Points;
        private Water water;
        public WaterPrimtives(Water water)
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
            Color colour = water.color;
            for (int i = 0; i < _points.Count - 1; i++)
            {
                AddVertex(_points[i], colour, new Vector2(i / (float)(_points.Count), 0));
                AddVertex(new Vector2(_points[i + 1].X, water.frame.Bottom), colour, new Vector2((i + 1) / (float)(_points.Count), 1));
                AddVertex(new Vector2(_points[i].X, water.frame.Bottom), colour, new Vector2(i / (float)(_points.Count), 1));

                AddVertex(_points[i], colour, new Vector2(i / (float)(_points.Count), 0));
                AddVertex(_points[i + 1], colour, new Vector2((i + 1) / (float)(_points.Count), 0));
                AddVertex(new Vector2(_points[i + 1].X, water.frame.Bottom), colour, new Vector2((i + 1) / (float)(_points.Count), 1));
            }
        }
        public override void SetShaders()
        {
            PrepareShader(EffectCache.PrimtiveShader ?? new BasicEffect(FlipGame.graphics.GraphicsDevice), "WaterMain", TimeAlive / 40f);
        }
        public override void OnUpdate()
        {
            _points = water.Pos.ToList();
            VertexCount = _points.Count() * 6;
            if (PrimitiveCount < VertexCount / 6)
            {
                _points.RemoveAt(0);
            }
            if (water == null)
            {
                Dispose();
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