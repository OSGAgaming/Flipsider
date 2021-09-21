
using FlipEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Flipsider
{
    public class ThreeJointQuadPrimitive : Primitive
    {
        Texture2D? TextureMap;
        public ThreeJointQuadPrimitive(Texture2D tex) { TextureMap = tex; }
        public override void SetDefaults()
        {
            Alpha = 0.7f;
            Width = 6;
            PrimitiveCount = 100;
            Effect = EffectCache.PrimtiveTexture;
        }

        public void ThreePointJointQuad(Vector2 Anchor, Vector2 Joint, Vector2 End)
        {
            Vector2 CenterToJointDist = Vector2.Normalize(Joint - Anchor) * Width;

            AddVertex(Anchor - Clockwise90(CenterToJointDist), Color.White, new Vector2(0, 0));
            AddVertex(Joint + Clockwise90(CenterToJointDist), Color.White, new Vector2(0.5f, 1));
            AddVertex(Anchor + Clockwise90(CenterToJointDist), Color.White, new Vector2(0, 1));

            AddVertex(Anchor - Clockwise90(CenterToJointDist), Color.White, new Vector2(0, 0));
            AddVertex(Joint - Clockwise90(CenterToJointDist), Color.White, new Vector2(0.4f, 0));
            AddVertex(Joint + Clockwise90(CenterToJointDist), Color.White, new Vector2(0.5f, 1));

            Vector2 LegtoJointDist = Vector2.Normalize(End - Joint) * Width;

            AddVertex(Joint - Clockwise90(LegtoJointDist), Color.White, new Vector2(0.6f, 0));
            AddVertex(End + Clockwise90(LegtoJointDist), Color.White, new Vector2(1, 1));
            AddVertex(Joint + Clockwise90(CenterToJointDist), Color.White, new Vector2(0.5f, 1));

            AddVertex(Joint - Clockwise90(LegtoJointDist), Color.White, new Vector2(0.4f, 0));
            AddVertex(End - Clockwise90(LegtoJointDist), Color.White, new Vector2(1, 0));
            AddVertex(End + Clockwise90(LegtoJointDist), Color.White, new Vector2(1, 1));

            AddVertex(Joint + Clockwise90(CenterToJointDist), Color.White, new Vector2(0.5f, 1));
            AddVertex(Joint - Clockwise90(CenterToJointDist), Color.White, new Vector2(0.4f, 0));
            AddVertex(Joint - Clockwise90(LegtoJointDist), Color.White, new Vector2(0.6f, 0));
        }

        protected virtual void AddPoints() { }

        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            if (_points.Count > 2) ThreePointJointQuad(_points[0], _points[1], _points[2]);
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