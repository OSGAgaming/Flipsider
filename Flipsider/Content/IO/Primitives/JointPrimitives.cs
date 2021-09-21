
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
            if (leg != null && leg.Parent != null)
            {
                _points.Add(leg.Parent.Center);
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
            Width = 5;
            if (arm != null && arm.Parent != null)
            {
                _points.Add(arm.Hoist);
                _points.Add(arm.Joint);
                _points.Add(arm.Center);
            }

        }
    }
}