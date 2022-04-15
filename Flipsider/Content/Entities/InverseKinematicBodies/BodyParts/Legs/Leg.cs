
using FlipEngine;
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
        private float StrideLength => 5.9f;
        private int XTolerance => 10;
        private int DistanceUntilFixation => 2;
        private float LegSpeed => 0.037f;
        private float ProgressionUntilNextStep => 0.83f;

        private float VelocityStrideEffect => 45;
        private float VelocityStepSpeedEffect => 0.006f;
        private float VelocityKneeEffect => 9;
        private float VelocityEffectClamp => 4;

        private float IdleStanceWidth => 5;
        private bool LegOnGround => Utils.ReturnIntersectionTile(Main.World, LegPosition + new Vector2(0, -4), LegPosition + new Vector2(0, 3)) != Vector2.Zero;

        private bool IsMovingLeg;
        private bool BegginingOfLedge;
        private bool CanMoveOtherLeg = true;
        public bool JustMovedLeg;
        public bool LegInFront;
        public bool SteppingOnLedge;
        public bool LedgeSupportListener;

        private float VelXStepAdjustment;
        public float LegProgression;
        private float KneeSupression;
        private float KneeSupressionVar;
        private float VarWalkSpeed = 1;
        private float TimeInAir = 0;
        private float LedgeProgStutter = 0;
        private float StridePointAlter;

        public Vector2 DetectedSurface;

        Vector2 LegTarget;

        Vector2 AirLegOffset;

        Vector2 LastLegTarget;
        Vector2 LastVelocity;
        public Vector2 JointPosition;
        public Vector2 LegPosition;

        public float XAccelInterp;
        Vector2 StridePoint1;
        Vector2 StridePoint2;

        public Vector2 SecondPotentialLedgePoint;
        public Vector2 PotentialLedgePoint;
        public Vector2 CachedLedgePoint;
        float ClampedVel => Math.Clamp(Vel, -VelocityEffectClamp, VelocityEffectClamp);
        float ClampedStrideLengthVel => (StrideLength * Sign + ClampedVel * VelocityStrideEffect) / (1 + ((VarWalkSpeed - 1) * 0.2f));
        public float DeltaY
        {
            get
            {
                if (Parent != null)
                    return Parent.CoreEntity.Position.Y - Parent.CoreEntity.OldPosition.Y;
                else return 0;
            }
        }

        public Vector2 YOff
        {
            get
            {
                if (OtherPart != null && Parent != null)
                {
                    float Y = (LegPosition.Y + OtherPart.LegPosition.Y) * 0.5f - LegLength * 2.5f;
                    return new Vector2(0, (Y - Parent.Center.Y) * 2 + KneeSupressionVar * 3);
                }
                else return Vector2.Zero;
            }
        }

        public Vector2 LYOff
        {
            get
            {
                if (OtherPart != null && Parent != null)
                {
                    return new Vector2(0, Math.Abs(LegPosition.Y - OtherPart.LegPosition.Y));
                }
                else return Vector2.Zero;
            }
        }

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
        public int Sign => Math.Sign(Vel);

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
                Utils.DrawLine(spritebatch, JointPosition, LegPosition, Color.Yellow, 2);

                Utils.DrawBoxFill(LegPosition - new Vector2(2), 4, 4, IsMovingLeg ? Color.Red : Color.Green);
                Utils.DrawBoxFill(JointPosition - new Vector2(2), 4, 4, Color.Yellow);
                Utils.DrawBoxFill(DetectedSurface - new Vector2(4),8, 8, Color.Orange);

                Vector2 PC = Parent.Center + YOff * 0.5f;
                float UpperRotation = Parent.CoreEntity.velocity.X / (10f * VarWalkSpeed) + VelXStepAdjustment / 360f + KneeSupressionVar *
                    (Parent.CoreEntity.velocity.X + Sign) * 0.02f;

                Vector2 Rot = new Vector2(0, Parent.CoreEntity.Height / 2).RotatedBy(UpperRotation);
                if (ID == "R_Leg") Utils.DrawLine(spritebatch, PC, PC - Rot, Color.Yellow, 2);
                Utils.DrawLine(spritebatch, PC, JointPosition, Color.Yellow, 2);

                Vector2 Mid = (LastLegTarget + LegTarget) / 2f;

                float XAccel = Parent.CoreEntity.velocity.X - Parent.CoreEntity.oldVelocity.X;

                XAccelInterp += ((XAccel * 3) - XAccelInterp) / 64f;

                float VelXStep = VelXStepAdjustment;
                for (float i = 0; i < 1; i += 0.1f)
                {
                    Vector2 line = Utils.TraverseBezier(LastLegTarget, Mid + new Vector2(0, -10 - AbsVel * VelocityKneeEffect), new Vector2(LegTarget.X + VelXStep, LegTarget.Y), i);
                    Utils.DrawBoxFill(line - new Vector2(0.5f), 1, 1, Color.Pink);
                }

                Utils.DrawLine(spritebatch, StridePoint1, StridePoint2, Color.Pink, 2);
                Utils.DrawBoxFill(LegTarget, 6, 6, Color.Black);
            }
        }

        public void StepBehaviour()
        {
            if (Parent != null && OtherPart != null)
            {
                float LedgeUp = 0;

                float mult = Parent.Cycle == AnimationCycle.Idle ? .8f : 1;
                if (LegProgression < 1)
                {
                    if (!LedgeSupportListener)
                    {
                        LegProgression += (LegSpeed * mult) * (1 + AbsVel * VelocityStepSpeedEffect) + (VarWalkSpeed - 1) * 0.01f + LedgeProgStutter;

                        BegginingOfLedge = false;
                    }
                    else
                    {
                        int Sign = Math.Sign(LegTarget.X - OtherPart.LegTarget.X);
                        LegProgression += (((Parent.Center.X - CachedLedgePoint.X) * Sign) / (Parent.CoreEntity.Width * 0.8f + AbsVel) - LegProgression) / 16f;
                        LedgeUp = 10;

                        if (LegProgression <= 0)
                        {
                            BegginingOfLedge = true;
                            CanMoveOtherLeg = true;
                            CachedLedgePoint.X = Parent.Center.X;
                        }
                        else
                        {
                            BegginingOfLedge = false;
                        }
                    }
                }

                Vector2 Mid = (LastLegTarget + LegTarget) / 2f;
                VelXStepAdjustment += ((Vel - LastVelocity.X) * 40 - VelXStepAdjustment) / 6f;

                if ((OtherPart.SteppingOnLedge && LedgeSupportListener) || SteppingOnLedge) VelXStepAdjustment = 0f;

                float ClampedVXSA = Math.Clamp(Vel - LastVelocity.X, -1, 0);
                float X = LegTarget.X + VelXStepAdjustment;
                float Y = StridePoint2.Y > LegPosition.Y ? StridePoint2.Y : LegPosition.Y;

                Vector2 NewStride1 = new Vector2(X, StridePoint1.Y);
                Vector2 NewStride2 = new Vector2(X, Y);

                float NewY = Utils.ReturnIntersectionTile(Main.World, NewStride1, NewStride2).Y;
                if (NewY != 0 && !OtherPart.SteppingOnLedge) LegTarget.Y = NewY;

                Vector2 DesiredPosition = Utils.TraverseBezier(LastLegTarget, Mid +
                    new Vector2(0, -13 - AbsVel * VelocityKneeEffect + Parent.CoreEntity.DeltaPos.Y * 5 - LedgeUp),
                    new Vector2(X, LegTarget.Y), MathHelper.SmoothStep(0, 1, LegProgression));

                LegPosition = DesiredPosition;

                //Logger.NewText(NewY + " " + FlipE.rand.NextFloat(20));

                if (LegProgression > ProgressionUntilNextStep -
                    AbsVel * 0.085f - (VarWalkSpeed - 1) * 0.35f
                    - ClampedVXSA * 0.05f + Math.Clamp(Parent.CoreEntity.DeltaPos.Y * 0.08f, -0.02f, 0.00f) && !CanMoveOtherLeg)
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
                bool LegsTogether = Math.Abs(OtherPart.LegPosition.X - LegPosition.X) < XTolerance * 0.9f;
                bool LegFarther =
                    Sign == 1 ?
                    LegPosition.X < OtherPart.LegPosition.X :
                    LegPosition.X > OtherPart.LegPosition.X;

                if (Parent.Cycle == AnimationCycle.Idle)
                {
                    LegDisplaced = Math.Abs(LegPosition.X - DetectedSurface.X) > XTolerance * 0.9f;
                    LegFarther = true;
                }

                if ((LegDisplaced || LegsTogether) && LegFarther)
                {

                    float EdgeCheckDelta = 0;
                    float Iteration = 0;
                    int MaxCheck = (int)Math.Abs(StridePoint1.X - LegPosition.X) + (int)(AbsVel * 8 + 1);
                    float DeltaLedgeThreshold = 29 + DeltaY * 15 ;

                    PotentialLedgePoint = Vector2.Zero;
                    SecondPotentialLedgePoint = Vector2.Zero;
                    CachedLedgePoint = Vector2.Zero;
                    LedgeProgStutter = 0;
                    SteppingOnLedge = false;
                    LedgeSupportListener = false;


                    if (!SteppingOnLedge && !OtherPart.SteppingOnLedge)
                    {
                        while (EdgeCheckDelta <= DeltaLedgeThreshold && Math.Abs(Iteration) < MaxCheck)
                        {
                            float Y = StridePoint2.Y > LegPosition.Y ? StridePoint2.Y : LegPosition.Y;

                            float CurrentY = Utils.ReturnIntersectionTile(Main.World, new Vector2(LegPosition.X, StridePoint1.Y) + new Vector2(Iteration - Vel, -Math.Abs(DeltaY) * 30),
                                new Vector2(LegPosition.X, Y) + new Vector2(Iteration - Vel, Math.Abs(DeltaY) * 30)).Y;
                            float NextCurrentY = Utils.ReturnIntersectionTile(Main.World, new Vector2(LegPosition.X, StridePoint1.Y) + new Vector2(Iteration + Sign * 2 - Vel, -Math.Abs(DeltaY) * 30),
                                new Vector2(LegPosition.X, Y) + new Vector2(Iteration + Sign * 2 - Vel, Math.Abs(DeltaY) * 30)).Y;

                            EdgeCheckDelta = NextCurrentY - CurrentY;
                            if ((EdgeCheckDelta >= DeltaLedgeThreshold || (NextCurrentY == 0 && CurrentY != 0)) && !OtherPart.SteppingOnLedge &&
                                Math.Abs(Iteration - Vel) < Math.Abs(LegPosition.X - StridePoint1.X))
                            {
                                PotentialLedgePoint = new Vector2(LegPosition.X + Iteration - Vel, CurrentY);
                                SecondPotentialLedgePoint = new Vector2(LegPosition.X + Iteration + Sign * 20 - Vel, CurrentY + 10);
                                bool CanHit = Utils.ReturnIntersectionTile(Main.World, Vector2.Lerp(Parent.Center, SecondPotentialLedgePoint, 0.95f), Parent.Center) == Vector2.Zero;

                                if (CanHit)
                                {
                                    OtherPart.DetectedSurface = SecondPotentialLedgePoint;
                                    DetectedSurface = PotentialLedgePoint;

                                    SteppingOnLedge = true;

                                    LedgeProgStutter = (1 - Math.Abs(Iteration - Vel) / Math.Abs(LegPosition.X - StridePoint1.X)) * 0.02f;
                                }
                            }

                            if (Sign != 0) Iteration += Sign * 2;
                            else Iteration += 2;
                        }
                    }

                    if (OtherPart.SteppingOnLedge)
                    {
                        DetectedSurface = OtherPart.SecondPotentialLedgePoint;
                        CachedLedgePoint.X = Parent.Center.X;
                    }


                    if (DetectedSurface != Vector2.Zero || SteppingOnLedge)
                    {
                        LegProgression = 0;
                        IsMovingLeg = true;
                        CanMoveOtherLeg = false;
                        LegTarget = DetectedSurface;
                        LastLegTarget = LegPosition;
                        LastVelocity = Parent.CoreEntity.velocity;

                        if (OtherPart.SteppingOnLedge) LedgeSupportListener = true;

                        Parent.Get<LeftArm>().Switch();
                        Parent.Get<RightArm>().Switch();
                    }
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

                if (LegInFront) AirLegOffset += (new Vector2((TempAirStance - ClampedTimeInAir * 0.5f) * Sign * ClampedVel, LegLength * 2.2f - 25 * ClampedVel + ClampedTimeInAir * ClampedVel * 0.8f) - AirLegOffset) / 16f;
                else AirLegOffset += (new Vector2(-(TempAirStance - ClampedTimeInAir * 1.1f + 16) * Sign * ClampedVel, LegLength * 2.2f - ClampedTimeInAir * 0.2f) - AirLegOffset) / 16f;

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
                float ClampedAbsVel2 = Math.Clamp(AbsVel, 0, 3f);

                for (int i = 0; i < 5; i++)
                {
                    JointPosition = CorrectLegStick(Parent.Center, JointPosition +
                        new Vector2(Sign * ClampedAbsVel2 * 2,KneeSupressionVar * 0.3f - LegProgression * AbsVel * 2f + 2.5f * ClampedAbsVel), LegLength)[1];

                    JointPosition = CorrectLegStick(LegPosition, JointPosition +
                        new Vector2(Sign * ClampedAbsVel2 * 2,KneeSupressionVar * 0.3f - LegProgression * AbsVel * 2f + 2.5f * ClampedAbsVel), LegLength)[1];
                }

                StridePoint1 = Vector2.Zero;
                StridePoint2 = Vector2.Zero;

                float avg = (Parent.Center.X + OtherPart.LegPosition.X) / 2f;

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

                    if (ID == "L_Leg")
                    {
                        if (Sign == -1)
                        {
                            StridePoint1 = new Vector2(Parent.Center.X, Parent.Center.Y - 30);
                            StridePoint2 = new Vector2(Parent.Center.X, Parent.Center.Y + 30);
                        }
                        else
                        {
                            StridePoint1 = new Vector2(Parent.Center.X, Parent.Center.Y - 30);
                            StridePoint2 = new Vector2(Parent.Center.X, Parent.Center.Y + 30);
                        }
                    }
                    else if (ID == "R_Leg")
                    {
                        if (Sign == -1)
                        {
                            StridePoint1 = new Vector2(Parent.Center.X + IdleStanceWidth * 2f, Parent.Center.Y - 30);
                            StridePoint2 = new Vector2(Parent.Center.X + IdleStanceWidth * 2f, Parent.Center.Y + 30);
                        }
                        else
                        {
                            StridePoint1 = new Vector2(Parent.Center.X - IdleStanceWidth * 2f, Parent.Center.Y - 30);
                            StridePoint2 = new Vector2(Parent.Center.X - IdleStanceWidth * 2f, Parent.Center.Y + 30);
                        }
                    }
                }
                else
                {
                    float Y1 = Parent.Center.Y + 120 + DeltaY * 20;
                    float Lowest = Y1 > LegPosition.Y ? Y1 : LegPosition.Y;

                    float Y2 = Parent.Center.Y + DeltaY * 20 - StridePointAlter * 2;
                    float Highest = Y2 > LegPosition.Y ? LegPosition.Y : Y2;

                    StridePoint1 = new Vector2(avg + ClampedStrideLengthVel - DeltaY * Sign * 9, Highest);
                    StridePoint2 = new Vector2(avg + ClampedStrideLengthVel - DeltaY * Sign * 9, Lowest);
                }

                Tile tile = Main.World.tileManager.GetTileFromWorldCoords(StridePoint1);
                bool spaceFree = true;
                if (tile != null)
                {
                    Polygon TopPoly = Main.World.tileManager.GetTileFromWorldCoords(StridePoint1).GetEntityModifier<Collideable>().Polygon;
                    spaceFree = !TopPoly.Contains(StridePoint1);
                }
                Vector2 DSurfacePreQuery = Utils.ReturnIntersectionTile(Main.World, StridePoint1, StridePoint2);
                Vector2 SampUp = new Vector2(0, Math.Min(DeltaY * 20, 0));
                bool CanHit = Utils.ReturnIntersectionTile(Main.World, Vector2.Lerp(LegPosition + SampUp, DSurfacePreQuery, 0.95f), Parent.Center + SampUp) == Vector2.Zero;

                if (!OtherPart.SteppingOnLedge && spaceFree && CanHit)
                {
                    DetectedSurface = Utils.ReturnIntersectionTile(Main.World, StridePoint1, StridePoint2);
                    if (DetectedSurface != Vector2.Zero) StridePointAlter += (Math.Max(-5,LegLength * 2.5f - (DetectedSurface.Y - StridePoint1.Y)) - StridePointAlter) / 2f;
                }

                if ((Parent.CoreEntity.onGround && (LegOnGround || OtherPart.LegOnGround)) &&
                    (!IsMovingLeg || (BegginingOfLedge && !OtherPart.SteppingOnLedge)) && OtherPart.CanMoveOtherLeg &&
                    !(OtherPart.LedgeSupportListener && OtherPart.LegProgression > 0 && OtherPart.LegProgression < 1))
                {
                    Step();
                }

                if (!Parent.CoreEntity.onGround)
                {
                    IsMovingLeg = false;
                    CanMoveOtherLeg = true;
                    SteppingOnLedge = false;
                }

                if (!LegOnGround && !IsMovingLeg)
                {
                    LegPosition.Y += 3f;
                }

                if (Parent.CoreEntity.onGround)
                {
                    VarWalkSpeed += (1 - VarWalkSpeed) / 11f;
                }

                if (IsMovingLeg)
                {
                    StepBehaviour();
                }
                else
                {
                    VelXStepAdjustment = 0;
                    BegginingOfLedge = false;
                    LedgeSupportListener = false;
                }

                if (Vector2.DistanceSquared(LegPosition, LegTarget + new Vector2(VelXStepAdjustment, 0)) < DistanceUntilFixation * DistanceUntilFixation && IsMovingLeg)
                {
                    IsMovingLeg = false;
                    CanMoveOtherLeg = true;
                    JustMovedLeg = true;
                    LegProgression = 0;
                    LedgeSupportListener = false;
                }

                for (int i = 0; i < 5; i++)
                {
                    LegPosition += (CorrectLegStick(Parent.Center + new Vector2(0, 10), LegPosition, LegLength * 4)[1] - LegPosition) / 10f;
                }
            }
        }
    }
}