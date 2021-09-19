
using FlipEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Flipsider
{
    public class PlayerPrimitives : Primitive
    {
        private Leg leg;
        public PlayerPrimitives(Leg leg)
        {
            this.leg = leg;
        }
        public override void SetDefaults()
        {
            Alpha = 0.7f;
            Width = 100;
            PrimitiveCount = 100;
            Effect = EffectCache.PrimtiveShader;
        }
        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            if (leg.Parent != null)
            {
                Vector2 CenterToJointDist = Vector2.Normalize(leg.JointPosition - leg.Parent.Center) * 5;

                AddVertex(leg.Parent.Center - Clockwise90(CenterToJointDist), Color.White, new Vector2(0, 0));
                AddVertex(leg.JointPosition + Clockwise90(CenterToJointDist), Color.White, new Vector2(0.5f, 1));
                AddVertex(leg.Parent.Center + Clockwise90(CenterToJointDist), Color.White, new Vector2(0,1));

                AddVertex(leg.Parent.Center - Clockwise90(CenterToJointDist), Color.White, new Vector2(0, 0));
                AddVertex(leg.JointPosition - Clockwise90(CenterToJointDist), Color.White, new Vector2(0.4f, 0));
                AddVertex(leg.JointPosition + Clockwise90(CenterToJointDist), Color.White, new Vector2(0.5f, 1));

                Vector2 LegtoJointDist = Vector2.Normalize(leg.LegPosition - leg.JointPosition) * 5;

                AddVertex(leg.JointPosition - Clockwise90(LegtoJointDist), Color.White, new Vector2(0.6f, 0));
                AddVertex(leg.LegPosition + Clockwise90(LegtoJointDist), Color.White, new Vector2(1, 1));
                AddVertex(leg.JointPosition + Clockwise90(CenterToJointDist), Color.White, new Vector2(0.5f, 1));

                AddVertex(leg.JointPosition - Clockwise90(LegtoJointDist), Color.White, new Vector2(0.4f, 0));
                AddVertex(leg.LegPosition - Clockwise90(LegtoJointDist), Color.White, new Vector2(1, 0));
                AddVertex(leg.LegPosition + Clockwise90(LegtoJointDist), Color.White, new Vector2(1, 1));

                AddVertex(leg.JointPosition + Clockwise90(CenterToJointDist), Color.White, new Vector2(0.5f, 1));
                AddVertex(leg.JointPosition - Clockwise90(CenterToJointDist), Color.White, new Vector2(0.4f, 0));
                AddVertex(leg.JointPosition - Clockwise90(LegtoJointDist), Color.White, new Vector2(0.6f, 0));
            }

        }
        public override void SetShaders()
        {
            PrepareShader(Effect, "WaterMain", TimeAlive / 40f);
        }
        public override void OnUpdate()
        {
            _points.Clear();

            _points.Add(Main.player.Center);
            _points.Add(leg.JointPosition);
            _points.Add(leg.LegPosition);

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