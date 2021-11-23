
using FlipEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Flipsider
{
    public class LegPrimitives : ThreeJointQuadPrimitive
    {
        private Leg leg;

        public LegPrimitives(Leg leg, Texture2D tex) : base(tex)
        {
            this.leg = leg;
        }

        protected override void AddPoints()
        {
            Width = 6;
            SecondWidth = 7;

            JointUV = 0.55f;
            JointUVSpread = 0.15f;

            if (leg != null && leg.Parent != null)
            {
                if(leg is RightLeg) _points.Add(leg.Parent.Center + new Vector2(2,0));
                if(leg is LeftLeg) _points.Add(leg.Parent.Center + new Vector2(-2, 0));

                _points.Add(leg.JointPosition);
                _points.Add(leg.LegPosition);
            }

        }
    }

    public class ArmPrimitives : ThreeJointQuadPrimitive
    {
        private Arm arm;

        public ArmPrimitives(Arm arm, Texture2D tex) : base(tex)
        {
            this.arm = arm;
        }

        protected override void AddPoints()
        {
            Width = 4;
            SecondWidth = 5;

            JointUV = 0.4f;
            JointUVSpread = 0.2f;

            if (arm != null && arm.Parent != null)
            {
                _points.Add(arm.Hoist);
                _points.Add(arm.Joint);
                _points.Add(arm.Center);
            }
        }
    }
}