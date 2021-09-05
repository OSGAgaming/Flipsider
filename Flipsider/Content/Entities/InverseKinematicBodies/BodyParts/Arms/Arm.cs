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
        public int ArmSpan = 8;
        public int Side = 1;
        public bool Moving;

        public bool IsSupporting;
        public bool SupportListener;
        public Vector2 SupportPoint;

        float JointHori;
        int Delay;
        public Vector2 Joint;
        public Vector2 Ledge;
        private int UpFromCenter = 20;
        private int ShoulderSpan = 8;
        float JointHeight;
        public Vector2 Target
        {
            get
            {
                if (Parent != null)
                {
                    if (ID == "R_Arm")
                        return Parent.Center + new Vector2(2f * Side * StaticSide * JointHori - JointHori * 6 + ShoulderSpan, 
                            10 - Math.Abs(JointHori) * 2 + Side * StaticSide * 2f * Math.Abs(JointHori));
                    else
                    {
                        return Parent.Center + new Vector2(2f * Side * StaticSide * JointHori - JointHori * 6 - ShoulderSpan, 
                            10 - Math.Abs(JointHori) * 2 + Side * StaticSide * 2f * Math.Abs(JointHori));
                    }
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
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (Parent != null && OtherPart != null)
            {
                /*if (ID == "R_Arm")
                    Utils.DrawLine(Joint, Parent.Center + new Vector2(ShoulderSpan, -UpFromCenter), Color.Purple);
                else
                    Utils.DrawLine(Joint, Parent.Center + new Vector2(-ShoulderSpan, -UpFromCenter), Color.Purple);

                Utils.DrawLine(Joint, Center, Color.Purple);
                Utils.DrawBoxFill(Joint - new Vector2(2), 4, 4, Color.Purple);
                Utils.DrawBoxFill(Center - new Vector2(2), 4, 4, Color.Purple);*/

            }
        }

        public override void Update()
        {
            if (Parent != null)
            {
                if (Delay > 0) Delay--;

                Center += (Target - Center) / 11f;
                JointHeight += (((Side * StaticSide + 1) / 2) * 5 - JointHeight) / 8f;

                Vector2 TargJoint = Joint + new Vector2(JointHori, Math.Abs(JointHori) - JointHeight * 0.1f * Math.Abs(JointHori));

                if (ID == "R_Arm")
                    Joint = CorrectArm(Parent.Center + new Vector2(ShoulderSpan, -UpFromCenter), TargJoint)[1];
                else
                    Joint = CorrectArm(Parent.Center + new Vector2(-ShoulderSpan, -UpFromCenter), TargJoint)[1];

                TargJoint = Joint + new Vector2(JointHori, Math.Abs(JointHori) - JointHeight * 0.5f * Math.Abs(JointHori));

                Joint = CorrectArm(Center, TargJoint)[1];

                JointHori += (Parent.CoreEntity.velocity.X * -4 - JointHori) / 16f;

                if (Vector2.DistanceSquared(Target, Center) < 60 * 60)
                {
                    Moving = false;
                }
                else
                {
                    Moving = true;
                }
            }
        }
    }
}