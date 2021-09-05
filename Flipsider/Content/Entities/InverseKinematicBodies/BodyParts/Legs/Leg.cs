
using Flipsider.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Flipsider
{
    public abstract class Leg : BodyPart
    {
        public abstract string OtherLeg { get; }

        private int LegLength => 10;
        private int StrideLength => 10;
        private int XTolerance => 10;
        private int DistanceUntilFixation => 4;
        private float LegSpeed => 0.035f;
        private float ProgressionUntilNextStep => 0.8f;

        private float VelocityStrideEffect => 50;
        private float VelocityStepSpeedEffect => 0.008f;
        private float VelocityKneeEffect => 11;
        private float VelocityEffectClamp => 3;

        private float IdleStanceWidth => 10;

        private bool IsMovingLeg;
        private bool CanMoveOtherLeg = true;
        public bool JustMovedLeg;
        public bool LegInFront;

        private float VelXStepAdjustment;
        public float LegProgression;
        private float KneeSupression;
        private float KneeSupressionVar;
        private float VarWalkSpeed = 1;
        private float TimeInAir = 0;

        public Vector2 DetectedSurface;

        Vector2 LegTarget;

        Vector2 AirLegOffset;

        Vector2 LastLegTarget;
        Vector2 LastVelocity;
        Vector2 JointPosition;
        public Vector2 LegPosition;

        private float XAccelInterp;

        float Vel
        {
            get
            {
                if (Parent != null)
                    return Parent.CoreEntity.velocity.X;
                else
                    return default;
            }
        }

        float AbsVel => Math.Abs(Vel);
        int Sign => Math.Sign(Vel);

        Leg? OtherPart => Parent?.Get(OtherLeg) as Leg;

        Vector2[] CorrectLeg(Vector2 feetVec, Vector2 jointVec, int Length)
        {
            float dx = feetVec.X - jointVec.X;
            float dy = feetVec.Y - jointVec.Y;
            float currentLength = (float)Math.Sqrt(dx * dx + dy * dy);
            if (currentLength > Length)
            {
                float deltaLength = currentLength - (Length * 0.5f);
                float perc = (deltaLength / (float)currentLength) * 0.5f;
                float offsetX = perc * dx;
                float offsetY = perc * dy;
                Vector2 F = new Vector2(feetVec.X + offsetX, feetVec.Y + offsetY);
                Vector2 J = new Vector2(jointVec.X + offsetX, jointVec.Y + offsetY);

                return new Vector2[] { F, J };
            }

            return new Vector2[] { Vector2.Zero, jointVec };
        }

        Vector2[] CorrectLegStick(Vector2 feetVec, Vector2 jointVec, int Length)
        {
            float dx = feetVec.X - jointVec.X;
            float dy = feetVec.Y - jointVec.Y;
            float currentLength = (float)Math.Sqrt(dx * dx + dy * dy);

            float deltaLength = currentLength - (Length * 0.5f);
            float perc = (deltaLength / (float)currentLength) * 0.5f;
            float offsetX = perc * dx;
            float offsetY = perc * dy;
            Vector2 F = new Vector2(feetVec.X + offsetX, feetVec.Y + offsetY);
            Vector2 J = new Vector2(jointVec.X + offsetX, jointVec.Y + offsetY);

            return new Vector2[] { F, J };
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (Parent != null && OtherPart != null)
            {
                Utils.DrawLine(spritebatch, JointPosition, LegPosition, Color.Yellow);

                Utils.DrawBoxFill(LegPosition - new Vector2(2), 4, 4, IsMovingLeg ? Color.Red : Color.Green);
                Utils.DrawBoxFill(JointPosition - new Vector2(2), 4, 4, Color.Yellow);

                float Y = (LegPosition.Y + OtherPart.LegPosition.Y) * 0.5f - LegLength * 2.5f;
                Vector2 YOff = new Vector2(0, Y - Parent.Center.Y + KneeSupressionVar * 3);
                Vector2 PC = Parent.Center + YOff * 0.5f;
                float UpperRotation = Parent.CoreEntity.velocity.X / (10f * VarWalkSpeed) + VelXStepAdjustment / 360f + KneeSupressionVar * 
                    (Parent.CoreEntity.velocity.X + Sign) * 0.02f;

                Vector2 Rot = new Vector2(0, Parent.CoreEntity.Height / 2).RotatedBy(UpperRotation);
                if (ID == "R_Leg") Utils.DrawLine(spritebatch, PC, PC - Rot, Color.Yellow);
                Utils.DrawLine(spritebatch, PC, JointPosition, Color.Yellow);

                Vector2 Mid = (LastLegTarget + LegTarget) / 2f;

                Texture2D tex = Textures._BodyParts_bitch_body;
                float XAccel = Parent.CoreEntity.velocity.X - Parent.CoreEntity.oldVelocity.X;

                XAccelInterp += ((XAccel * 3) - XAccelInterp) / 64f;



                Texture2D head = Textures._BodyParts_bitch_head;

                //spritebatch.Draw(head, Parent.Center - YOff - new Vector2(0, 27), head.Bounds, Color.White, Parent.CoreEntity.velocity.X / 10f, head.TextureCenter(), 2f,
                //    Parent.CoreEntity.velocity.X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
                //spritebatch.Draw(tex, Parent.Center - YOff * 0.6f - new Vector2(0, 12), tex.Bounds, Color.White, Parent.CoreEntity.velocity.X / 20f + XAccelInterp, tex.TextureCenter(), 2f,
                //    Parent.CoreEntity.velocity.X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
                
                Texture2D upperLegs = Textures._BodyParts_bitch_upper_left_leg;
                if (ID == "R_Leg") upperLegs = Textures._BodyParts_bitch_upper_right_leg;

                spritebatch.Draw(upperLegs, (PC + JointPosition) / 2f, upperLegs.Bounds, Color.White, (PC - JointPosition).ToRotation() - 1.57f, upperLegs.TextureCenter(), 2f,
                    Parent.CoreEntity.velocity.X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

                Texture2D lowerLegs = Textures._BodyParts_bitch_lower_left_leg;
                if (ID == "R_Leg") lowerLegs = Textures._BodyParts_bitch_lower_right_leg;

                spritebatch.Draw(lowerLegs, (LegPosition + JointPosition) / 2f, lowerLegs.Bounds, Color.White, (LegPosition - JointPosition).ToRotation() - 1.57f, lowerLegs.TextureCenter(), 2f,
                    Parent.CoreEntity.velocity.X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

                

                for (float i = 0; i < 1; i += 0.1f)
                {
                    Vector2 line = Utils.TraverseBezier(LastLegTarget, Mid + new Vector2(0, -10 - AbsVel * VelocityKneeEffect), new Vector2(LegTarget.X + VelXStepAdjustment, LegTarget.Y), i);
                    Utils.DrawBoxFill(line - new Vector2(0.5f), 1, 1, Color.Pink);
                }
            }
        }

        public void StepBehaviour()
        {
            if (Parent != null && OtherPart != null)
            {
                if (LegProgression < 1) LegProgression += LegSpeed * (1 + AbsVel * VelocityStepSpeedEffect) + (VarWalkSpeed - 1) * 0.01f;

                Vector2 Mid = (LastLegTarget + LegTarget) / 2f;
                VelXStepAdjustment += ((Vel - LastVelocity.X) * 34 - VelXStepAdjustment) / 6f;

                float ClampedVXSA = Math.Clamp(Vel - LastVelocity.X, -1, 0);

                Vector2 DesiredPosition = Utils.TraverseBezier(LastLegTarget, Mid +
                    new Vector2(0, -10 - AbsVel * VelocityKneeEffect),
                    new Vector2(LegTarget.X + VelXStepAdjustment, LegTarget.Y), MathHelper.SmoothStep(0, 1, LegProgression));

                LegPosition = DesiredPosition;
                LegPosition.X += Parent.CoreEntity.velocity.X;

                if (LegProgression > ProgressionUntilNextStep -
                    AbsVel * 0.085f - (VarWalkSpeed - 1) * 0.1f
                    - ClampedVXSA * 0.05f && !CanMoveOtherLeg)
                {
                    CanMoveOtherLeg = true;
                }
            }
        }

        public void Step()
        {
            if (Parent != null && OtherPart != null)
            {
                bool LegDisplaced = Math.Abs(LegPosition.X - (Parent.Center.X + StrideLength * Sign * 0.5f)) > (XTolerance - AbsVel * 5);
                bool LegsTogether = Math.Abs(OtherPart.LegPosition.X - LegPosition.X) < XTolerance * 0.7f;

                if ((LegDisplaced || LegsTogether) && OtherPart.CanMoveOtherLeg && !IsMovingLeg)
                {
                    LegProgression = 0;
                    IsMovingLeg = true;
                    CanMoveOtherLeg = false;
                    LegTarget = DetectedSurface;
                    LastLegTarget = LegPosition;
                    LastVelocity = Parent.CoreEntity.velocity;

                    Parent.Get<LeftArm>().Switch();
                    Parent.Get<RightArm>().Switch();
                }
            }
        }

        public void InAir()
        {
            KneeSupressionVar *= 0.6f;
            int TempAirStance = 25;
            TimeInAir += 0.95f;


            if (Parent != null)
            {
                if (Parent.CoreEntity.velocity.Y > 0)
                {
                    if (KneeSupression < 16) KneeSupression += Parent.CoreEntity.velocity.Y * 0.2f;
                }
                LegPosition.X = Parent.Center.X + AirLegOffset.X;
                LegPosition.Y = Parent.Center.Y + AirLegOffset.Y;

                float ClampedVel = Math.Clamp(AbsVel * 0.6f, 0, 1f);
                float ClampedTimeInAir = Math.Clamp(TimeInAir, 0, 40f);

                if (LegInFront) AirLegOffset += (new Vector2((TempAirStance - ClampedTimeInAir * 0.5f) * Sign * ClampedVel, LegLength * 2.2f - 25 * ClampedVel + ClampedTimeInAir * ClampedVel * 0.8f) - AirLegOffset) / 12f;
                else AirLegOffset += (new Vector2(-(TempAirStance - ClampedTimeInAir * 1.1f + 16) * Sign * ClampedVel, LegLength * 2.2f - ClampedTimeInAir * 0.2f) - AirLegOffset) / 12f;

                VarWalkSpeed += (2.6f - VarWalkSpeed) / 16f;
            }
        }

        public override void Update()
        {
            JustMovedLeg = false;

            if (Parent != null && OtherPart != null)
            {
                if (JointPosition == Vector2.Zero)
                {
                    JointPosition = Parent.Center + new Vector2(StrideLength, 0);
                }
                float ClampedAbsVel = Math.Clamp(AbsVel, 0, 0.5f);

                JointPosition = CorrectLegStick(Parent.Center, JointPosition + 
                    new Vector2(Vel * 2f + 3 * Sign, -AbsVel + KneeSupressionVar + TimeInAir * 0.2f * ClampedAbsVel), LegLength)[1];

                JointPosition = CorrectLegStick(LegPosition, JointPosition + 
                    new Vector2(Vel * 2f + 3 * Sign, -AbsVel + KneeSupressionVar + TimeInAir * 0.2f * ClampedAbsVel), LegLength)[1];

                bool LegOnGround = Utils.ReturnIntersectionTile(Main.World, LegPosition + new Vector2(0,-4), LegPosition + new Vector2(0, 3)) != Vector2.Zero;

                Vector2 StridePoint1 = Vector2.Zero;
                Vector2 StridePoint2 = Vector2.Zero;

                float avg = (Parent.Center.X + OtherPart.LegPosition.X) / 2f;
                float ClampedVel = Math.Clamp(Vel, -VelocityEffectClamp, VelocityEffectClamp);
                float StrideLengthVel = StrideLength * Sign + Vel * VelocityStrideEffect;
                float ClampedStrideLengthVel = ((StrideLength) * Sign + ClampedVel * VelocityStrideEffect) / (1 + ((VarWalkSpeed - 1) * 0.2f));


                float Dist = Math.Abs(LegPosition.X - Parent.Center.X);

                if (Parent.CoreEntity.onGround)
                {
                    TimeInAir = 0;
                    if (Dist > XTolerance * 2.2f)
                    {
                        LegPosition.X += (Parent.Center.X - LegPosition.X) / 12f;
                    }

                    KneeSupression *= 0.94f;
                    KneeSupressionVar += (KneeSupression - KneeSupressionVar) / 16f;

                    AirLegOffset = LegPosition - Parent.Center;

                    if (Math.Sign(LegPosition.X - OtherPart.LegPosition.X) != Sign) LegInFront = true;
                    else LegInFront = false;
                }
                else
                {
                    InAir();
                }

                if (Parent.Cycle == AnimationCycle.Idle)
                {
                    JustMovedLeg = true;

                    if (ID == "R_Leg")
                    {
                        StridePoint1 = new Vector2(Parent.Center.X + IdleStanceWidth, Parent.Center.Y - 5);
                        StridePoint2 = new Vector2(Parent.Center.X + IdleStanceWidth, Parent.Center.Y + LegLength * 6);
                    }
                    else if (ID == "L_Leg")
                    {
                        StridePoint1 = new Vector2(Parent.Center.X - IdleStanceWidth, Parent.Center.Y - 5);
                        StridePoint2 = new Vector2(Parent.Center.X - IdleStanceWidth, Parent.Center.Y + LegLength * 6);
                    }
                }
                else
                {
                    StridePoint1 = new Vector2(avg + ClampedStrideLengthVel, Parent.Center.Y);
                    StridePoint2 = new Vector2(avg + ClampedStrideLengthVel, Parent.Center.Y + LegLength * 6);
                }

                DetectedSurface = Utils.ReturnIntersectionTile(Main.World, StridePoint1, StridePoint2);

                if (DetectedSurface != Vector2.Zero && Parent.CoreEntity.onGround && LegOnGround)
                {
                    Step();
                }

                if (!Parent.CoreEntity.onGround)
                {
                    IsMovingLeg = false;
                    CanMoveOtherLeg = true;
                }

                if (!LegOnGround && !IsMovingLeg)
                {
                    LegPosition.Y += 2f;
                }

                if (Parent.CoreEntity.onGround)
                {
                    VarWalkSpeed += (1 - VarWalkSpeed) / 16f;
                }

                if (IsMovingLeg)
                {
                    StepBehaviour();
                }

                if (Vector2.DistanceSquared(LegPosition, LegTarget + new Vector2(VelXStepAdjustment, 0)) < DistanceUntilFixation * DistanceUntilFixation && IsMovingLeg)
                {
                    IsMovingLeg = false;
                    CanMoveOtherLeg = true;
                    JustMovedLeg = true;
                }
            }
        }
    }
}