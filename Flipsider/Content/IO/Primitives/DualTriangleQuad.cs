
using FlipEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Flipsider
{
    public class DualTriangleQuad : Primitive
    {
        Texture2D? TextureMap;
        protected int WidthFallOff;
        public DualTriangleQuad(Texture2D tex) { TextureMap = tex; }
        public override void SetDefaults()
        {
            Alpha = 1f;
            Width = 6;
            PrimitiveCount = 100;
            Effect = EffectCache.PrimtiveTexture;
        }

        public void ListQuad(List<Vector2> points)
        {
            for(int i = 0; i < points.Count - 1; i++)
            {
                float CurrentUV = i / (float)points.Count;
                float NextUV = (i + 1) / (float)points.Count;

                Vector2 CurrentNorm = CurveNormal(points, i) * Width * (1 - CurrentUV * WidthFallOff);
                Vector2 NextNorm = CurveNormal(points, i + 1) * Width * (1 - NextUV * WidthFallOff);
                Vector2 CurrentPoint = points[i];
                Vector2 NextPoint = points[i + 1];

                AddVertex(CurrentPoint - CurrentNorm, Color.White, new Vector2(CurrentUV, 0));
                AddVertex(NextPoint + NextNorm, Color.White, new Vector2(NextUV, 1));
                AddVertex(CurrentPoint + CurrentNorm, Color.White, new Vector2(CurrentUV, 1));

                AddVertex(CurrentPoint - CurrentNorm, Color.White, new Vector2(CurrentUV, 0));
                AddVertex(NextPoint - NextNorm, Color.White, new Vector2(NextUV, 0));
                AddVertex(NextPoint + NextNorm, Color.White, new Vector2(NextUV, 1));
            }
        }

        protected virtual void AddPoints() { }

        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            if (_points.Count > 1) ListQuad(_points);
        }
        public override void SetShaders()
        {
            if(TextureMap != null) Effect.Parameters["textureMap"].SetValue(TextureMap);
            PrepareShader(Effect);
        }
        public override void OnUpdate()
        {
            _points.Clear();

            AddPoints();

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