using FlipEngine;
using Flipsider.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Flipsider
{
    public abstract class Arm : BodyPart
    {
        public Arm? OtherPart => Parent?.Get(OtherArm) as Arm;
        public abstract string OtherArm { get; }
        public virtual int StaticSide { get; }
        public int ArmSpan = 10;
        public int Side = 1;
        public bool Moving;

        public bool IsSupporting;
        public bool SupportListener;
        public Vector2 SupportPoint;

        float JointHori;
        int Delay;
        public Vector2 Joint;
        public Vector2 Ledge;
        private int UpFromCenter => 16;
        private int ShoulderSpan => 10;
        float JointHeight;

        public float Lerp;
        private Vector2 OffsetLerp;
        public Vector2 Hoist;

        float ArmHoistXAmp => 10;
        float X1 => MathHelper.SmoothStep(ArmHoistXAmp * (1 - (Side + 1) / 2), ArmHoistXAmp * (Side + 1) / 2, Lerp * 3) * AbsVelFunction;
        float AbsVelFunction => Math.Abs(JointHori);
        int AbsSide => Side * StaticSide;
        int Sign {
            get
            {
                if (Parent != null)
                    return Math.Sign(Parent.CoreEntity.velocity.X);
                else
                    return 0;
            }
        }

        public Vector2 Target
        {
            get
            {
                if (Parent != null)
                {
                    float ArmSwing = 8f;
                    float UpFactor = 4;

                    Vector2 Offset;
                    Offset = 
                        new Vector2(AbsSide * AbsVelFunction * (-ArmSwing) - JointHori * 4 * (-Sign * StaticSide),
                        20 - AbsVelFunction * 6 + -AbsSide * UpFactor * JointHori);

                    return Hoist + Offset;
                }
                else
                    return Vector2.Zero;
            }
        }

        Vector2[] CorrectArm(Vector2 feetVec, Vector2 jointVec)
        {
            float dx = feetVec.X - jointVec.X;
            float dy = feetVec.Y - jointVec.Y;
            float currentLength = (float)Math.Sqrt(dx * dx + dy * dy);
            float deltaLength = currentLength - (ArmSpan * 0.5f);
            float perc = (deltaLength / (float)currentLength) * 0.5f;
            float offsetX = perc * dx;
            float offsetY = perc * dy;
            Vector2 F = new Vector2(feetVec.X + offsetX, feetVec.Y + offsetY);
            Vector2 J = new Vector2(jointVec.X + offsetX, jointVec.Y + offsetY);

            return new Vector2[] { F, J };
        }

        public void Switch()
        {
            if (Delay == 0)
                Side *= -1;

            Delay = 3;
            Lerp = 0;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (Parent != null && OtherPart != null)
            {
                Utils.DrawLine(Joint, Center, Color.Magenta, 3);
                Utils.DrawLine(Hoist, Joint, Color.Purple, 2);
                Utils.DrawBoxFill(Joint - new Vector2(2), 4, 4, Color.Purple);
                Utils.DrawBoxFill(Center - new Vector2(2), 4, 4, Color.Purple);
            }
        }

        public override void Update()
        {
            if (Parent != null)
            {
                if (Delay > 0) Delay--;

                Vector2 Up = new Vector2(0, -UpFromCenter).RotatedBy(Parent.UpperBodyRotation);

                Hoist = Parent.Center + Up + new Vector2(ShoulderSpan * StaticSide - Sign - X1 * 0.6f * StaticSide, -AbsVelFunction * 1.2f);

                Vector2 VecAddJoint = new Vector2(-JointHori * X1 * 0.05f - JointHori, AbsVelFunction * (-1 + X1 * 0.3f));
                if(-Sign * StaticSide == -1)
                {
                    VecAddJoint = new Vector2(-JointHori * X1 * 0.05f - JointHori, AbsVelFunction * (3 - X1 * 0.4f));
                }
                Vector2 TargJoint = Joint + VecAddJoint;

                for (int i = 0; i < 5; i++)
                {
                    Joint = CorrectArm(Hoist, TargJoint)[1];
                }

                TargJoint = Joint + VecAddJoint;
                for (int i = 0; i < 5; i++)
                {
                    Joint = CorrectArm(Center, TargJoint)[1];
                }

                JointHori += (Parent.CoreEntity.velocity.X * -0.5f - JointHori) / 16f;

                if (Vector2.DistanceSquared(Target, Center) < 60 * 60)
                {
                    Moving = false;
                }
                else
                {
                    Moving = true;
                }

                Lerp += (1 - Lerp) / 32f;
                Center = Parent.Center + OffsetLerp;
                OffsetLerp = Vector2.Lerp(OffsetLerp, Target - Parent.Center, MathHelper.SmoothStep(0, 1, Lerp * (1f + (AbsVelFunction * 0.1f))));
            }
        }
    }
}