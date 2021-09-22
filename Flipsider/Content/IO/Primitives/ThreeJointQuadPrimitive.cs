
using FlipEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Flipsider
{
    public class ThreeJointQuadPrimitive : Primitive
    {
        public Texture2D? TextureMap;
        protected int SecondWidth = 6;
        protected float JointUV = 0.6f;
        protected float JointUVSpread = 0.1f;

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

            Vector2 triad1 = new Vector2(JointUV, 1);
            Vector2 triad2 = new Vector2(JointUV - JointUVSpread, 0);
            Vector2 triad3 = new Vector2(JointUV + JointUVSpread, 0);

            AddVertex(Anchor - Clockwise90(CenterToJointDist), Color.White, new Vector2(0, 0));
            AddVertex(Joint + Clockwise90(CenterToJointDist), Color.White, triad1);
            AddVertex(Anchor + Clockwise90(CenterToJointDist), Color.White, new Vector2(0, 1));

            AddVertex(Anchor - Clockwise90(CenterToJointDist), Color.White, new Vector2(0, 0));
            AddVertex(Joint - Clockwise90(CenterToJointDist), Color.White, triad2);
            AddVertex(Joint + Clockwise90(CenterToJointDist), Color.White, triad1);

            Vector2 LegtoJointDist = Vector2.Normalize(End - Joint) * SecondWidth;

            AddVertex(Joint - Clockwise90(LegtoJointDist), Color.White, triad3);
            AddVertex(End + Clockwise90(LegtoJointDist), Color.White, new Vector2(1, 1));
            AddVertex(Joint + Clockwise90(CenterToJointDist), Color.White, triad1);

            AddVertex(Joint - Clockwise90(LegtoJointDist), Color.White, triad3);
            AddVertex(End - Clockwise90(LegtoJointDist), Color.White, new Vector2(1, 0));
            AddVertex(End + Clockwise90(LegtoJointDist), Color.White, new Vector2(1, 1));

            AddVertex(Joint + Clockwise90(CenterToJointDist), Color.White, triad1);
            AddVertex(Joint - Clockwise90(CenterToJointDist), Color.White, triad2);
            AddVertex(Joint - Clockwise90(LegtoJointDist), Color.White, triad3);
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