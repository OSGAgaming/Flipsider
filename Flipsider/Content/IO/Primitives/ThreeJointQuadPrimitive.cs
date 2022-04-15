
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
        protected bool QuadFliped;
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
            Vector2 JointInterp = Vector2.Lerp(Anchor, Joint, 0.8f);
            Vector2 EndInterp = Vector2.Lerp(End, Joint, 0.8f);

            Vector2 FirstDir = Vector2.Normalize(JointInterp - Anchor) * Width;
            Vector2 SecondDir = Vector2.Normalize(End - EndInterp) * SecondWidth;

            Vector2 triad2 = new Vector2(JointUV - JointUVSpread, 0);
            Vector2 triad3 = new Vector2(JointUV + JointUVSpread, 0);

            Vector2 triad5 = new Vector2(JointUV - JointUVSpread, 1);
            Vector2 triad6 = new Vector2(JointUV + JointUVSpread, 1);

            AddVertex(Anchor - Clockwise90(FirstDir), Color.White, new Vector2(0, 0));
            AddVertex(JointInterp + Clockwise90(FirstDir), Color.White, triad5);
            AddVertex(Anchor + Clockwise90(FirstDir), Color.White, new Vector2(0, 1));

            AddVertex(Anchor - Clockwise90(FirstDir), Color.White, new Vector2(0, 0));
            AddVertex(JointInterp - Clockwise90(FirstDir), Color.White, triad2);
            AddVertex(JointInterp + Clockwise90(FirstDir), Color.White, triad5);

            AddVertex(EndInterp - Clockwise90(SecondDir), Color.White, triad3);
            AddVertex(End + Clockwise90(SecondDir), Color.White, new Vector2(1, 1));
            AddVertex(EndInterp + Clockwise90(SecondDir), Color.White, triad6);

            AddVertex(EndInterp - Clockwise90(SecondDir), Color.White, triad3);
            AddVertex(End - Clockwise90(SecondDir), Color.White, new Vector2(1, 0));
            AddVertex(End + Clockwise90(SecondDir), Color.White, new Vector2(1, 1));

            AddVertex(JointInterp - Clockwise90(FirstDir), Color.White, triad3);
            AddVertex(EndInterp + Clockwise90(SecondDir), Color.White, triad6);
            AddVertex(JointInterp + Clockwise90(FirstDir), Color.White, triad5);

            AddVertex(JointInterp - Clockwise90(FirstDir), Color.White, triad2);
            AddVertex(EndInterp - Clockwise90(SecondDir), Color.White, triad3);
            AddVertex(EndInterp + Clockwise90(SecondDir), Color.White, triad6);
        }

        protected virtual void AddPoints() { }

        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            if (_points.Count > 2) ThreePointJointQuad(_points[0], _points[1], _points[2]);
        }
        public override void SetShaders()
        {
            if(TextureMap != null) Effect.Parameters["textureMap"].SetValue(TextureMap);
            PrepareShader(Effect, "ThePass", 0f, 0.5f);
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