using FlipEngine;
using Flipsider.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace Flipsider
{
    public enum AnimationCycle
    { 
        Idle,
        Walking,
        Hoist
    }
    public class CorePart : IDrawableEntityModifier, IPrimitiveLayeredComponent
    {
        public AnimationCycle Cycle = AnimationCycle.Walking;
        public Player CoreEntity { get; set; }
        public int Layer { get; set; }

        public ThreeJointQuadPrimitive LeftLegPrims { get; set; }
        public ThreeJointQuadPrimitive RightLegPrims { get; set; }
        public ThreeJointQuadPrimitive LeftArmPrims { get; set; }
        public ThreeJointQuadPrimitive RightArmPrims { get; set; }

        public HairPrimtives HairPrimitives { get; set; }

        public int MainVerletPoint { get; private set; }

        public int HairPoints => 8;
        public int HairSeperation => 5;

        public float UpperBodyRotation;
        public int IdleTimer;

        public bool SkeletonMode => false;

        private int NumberOfFrames => 8;
        private int CurrentFrame;

        public Vector2 Center
        {
            get
            {
                if (CoreEntity != null) return CoreEntity.Center;
                else return Vector2.Zero;
            }
        }

        public CorePart(Player entity)
        {
            CoreEntity = entity;

            AppendBodyPart(new RightLeg());
            AppendBodyPart(new LeftLeg());

            AppendBodyPart(new RightArm());
            AppendBodyPart(new LeftArm());

            Get<LeftLeg>().LegPosition = entity.Center;
            Get<RightLeg>().LegPosition = entity.Center;

            LeftLegPrims = new LegPrimitives(Get<LeftLeg>(), Textures._BodyParts_LeftLeg);
            RightLegPrims = new LegPrimitives(Get<RightLeg>(), Textures._BodyParts_RightLeg);

            LeftArmPrims = new ArmPrimitives(Get<LeftArm>(), Textures._BodyParts_LeftArm);
            RightArmPrims = new ArmPrimitives(Get<RightArm>(), Textures._BodyParts_RightArm);

            HairPrimitives = new HairPrimtives(this, Textures._BodyParts_Ponytail);

            Main.AppendPrimitiveToLayer(this);

            MainVerletPoint = Verlet.Instance.CreateVerletPoint(Center, true);
            for (int i = 0; i < HairPoints; i++)
            {
                Verlet.Instance.CreateVerletPoint(Center + new Vector2(-HairSeperation * (i + 1), 0), false, true);
            }
        }

        public Dictionary<string, BodyPart> Parts = new Dictionary<string, BodyPart>();

        public void AppendBodyPart(BodyPart Part)
        {
            if (CoreEntity != null)
            {
                Part.Parent = this;
                Part.Center = CoreEntity.Center;
                Parts.Add(Part.ID, Part);
            }
        }

        public BodyPart Get(string PartID) => Parts[PartID];

        public T Get<T>(string Part = " ") where T : BodyPart
        {
            foreach (KeyValuePair<string, BodyPart> Parts in Parts)
            {
                if (Parts.Value is T) return (T)Parts.Value;
            }

            return (T)Parts[Part]; 
        }

        public void Update(in Entity entity)
        {
            if (CoreEntity != null)
            {
                Layer = CoreEntity.Layer;
                float avg = (Get<LeftLeg>().LegPosition.X + Get<RightLeg>().LegPosition.X) / 2f;

                if (Math.Abs(CoreEntity.velocity.X) <= 0.1f)
                {
                    IdleTimer++;
                }
                else
                {
                    IdleTimer = 0;
                }

                if(Math.Abs(CoreEntity.velocity.X) >= 0.5f)
                {
                    int Disp = 3;
                    float CorrectLerp = Math.Clamp(Get<RightArm>().Lerp * (1.8f + Math.Abs(CoreEntity.velocity.X * 0.25f)),0,1);
                    int ClampedVel = (int)Math.Clamp(Math.Abs(CoreEntity.velocity.X * 0.5f), 0, 1);
                    if (Get<RightArm>().Side == 1)
                    {
                        int frame = (int)Math.Round(Math.Clamp(CorrectLerp * NumberOfFrames * 0.35f, 0, 3) + Disp) % NumberOfFrames;
                        CurrentFrame = frame;
                    }
                    else
                    {
                        int frame = (int)Math.Round(Math.Clamp(CorrectLerp * NumberOfFrames * 0.35f + NumberOfFrames / 2, 4, 7) + Disp) % NumberOfFrames;
                        CurrentFrame = frame;
                    }

                    CurrentFrame = Math.Clamp(CurrentFrame, 1 - ClampedVel, 3 + ClampedVel * 4);
                }
                if(IdleTimer > 3)
                {
                    Cycle = AnimationCycle.Idle;
                    CurrentFrame = 1;
                }
                else
                {
                    Cycle = AnimationCycle.Walking;

                }

                if (Cycle == AnimationCycle.Idle)
                {
                    //entity.Center += ((new Vector2(avg, entity.Center.Y) - entity.Center) / 40f);
                }
                else
                {
                    entity.Center += ((new Vector2(avg, entity.Center.Y) - entity.Center) / 15f);
                }
            }

            foreach (KeyValuePair<string, BodyPart> Parts in Parts)
            {
                Parts.Value.Update();
            }
            Leg rl = Get<RightLeg>();
            if(CoreEntity != null) UpperBodyRotation = CoreEntity.velocity.X / 12f + rl.XAccelInterp;
            Verlet.Instance.points[MainVerletPoint].point = Center - rl.LYOff * 0.2f - new Vector2(4 * rl.Sign, 27).RotatedBy(UpperBodyRotation);

            if (CoreEntity != null)
            {
                float abs = Math.Sign(CoreEntity.velocity.X);

                if (abs == 1)
                {
                    LeftArmPrims.TextureMap = Textures._BodyParts_RightArmR;
                    RightArmPrims.TextureMap = Textures._BodyParts_LeftArmR;

                    LeftLegPrims.TextureMap = Textures._BodyParts_LeftLegR;
                    RightLegPrims.TextureMap = Textures._BodyParts_RightLegR;
                }
                else
                {
                    LeftArmPrims.TextureMap = Textures._BodyParts_LeftArm;
                    RightArmPrims.TextureMap = Textures._BodyParts_RightArm;

                    LeftLegPrims.TextureMap = Textures._BodyParts_LeftLeg;
                    RightLegPrims.TextureMap = Textures._BodyParts_RightLeg;
                }
            }
        }

        public void Draw(in Entity entity, SpriteBatch spriteBatch)
        {
            if (!SkeletonMode)
            {
                Leg rl = Get<RightLeg>();
                Leg ll = Get<LeftLeg>();

                Texture2D head = Textures._BodyParts_HeadAnim;
                Texture2D tex = Textures._BodyParts_BodyAnim;

                void RenderLeg(Leg leg)
                {
                    Texture2D upperLegs = Textures._BodyParts_bitch_upper_left_leg;

                    if (leg is RightLeg) upperLegs = Textures._BodyParts_bitch_upper_right_leg;

                    if (leg.Parent != null)
                    {
                        Vector2 PC = leg.Parent.Center + leg.YOff * 0.5f;

                        spriteBatch.Draw(upperLegs, (PC + leg.JointPosition) / 2f, upperLegs.Bounds, Color.White, (PC - leg.JointPosition).ToRotation() - 1.57f, upperLegs.TextureCenter(), 2f,
                            leg.Parent.CoreEntity.velocity.X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

                        Texture2D lowerLegs = Textures._BodyParts_bitch_lower_left_leg;
                        if (leg is RightLeg) lowerLegs = Textures._BodyParts_bitch_lower_right_leg;

                        spriteBatch.Draw(lowerLegs, (leg.LegPosition + leg.JointPosition) / 2f, lowerLegs.Bounds, Color.White, (leg.LegPosition - leg.JointPosition).ToRotation() - 1.57f, lowerLegs.TextureCenter(), 2f,
                            leg.Parent.CoreEntity.velocity.X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
                    }
                }

                //RenderLeg(rl);
                //RenderLeg(ll);

                Vector2 Up = new Vector2(0, -13).RotatedBy(UpperBodyRotation);

               spriteBatch.Draw(tex, Center - rl.LYOff * 0.2f + new Vector2(-4 * rl.Sign, 7), 
                    new Rectangle(0,CurrentFrame * (tex.Height / NumberOfFrames), tex.Width, tex.Height / NumberOfFrames), 
                   Color.White, UpperBodyRotation, new Vector2(tex.Width / 2f, tex.Height / NumberOfFrames), 2f,
                    CoreEntity.velocity.X < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

                spriteBatch.Draw(head, Center - rl.LYOff * 0.2f + Up - new Vector2(-1 * rl.Sign, 0), 
                    new Rectangle(0, CurrentFrame * (head.Height / NumberOfFrames), head.Width, head.Height / NumberOfFrames), 
                    Color.White, CoreEntity.velocity.X / 12f, new Vector2(head.Width / 2f, head.Height / NumberOfFrames), 2f,
                    CoreEntity.velocity.X < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
              
                for (int i = MainVerletPoint; i < MainVerletPoint + HairPoints; i++)
                {
                    //Utils.DrawLine(Verlet.Instance.points[i].point, Verlet.Instance.points[i - 1].point, Color.Aqua, 2);
                    float abs = Math.Abs(CoreEntity.velocity.X);
                    Verlet.Instance.points[i].point += new Vector2(0, Time.SineTime(abs * 0.05f, i * 0.6f) * 0.03f * abs);
                }               
            }
            else
            {
                foreach (KeyValuePair<string, BodyPart> Parts in Parts)
                {
                    Parts.Value.Draw(spriteBatch);
                }
            }
        }

        public void DrawPrimtiivesBefore(SpriteBatch sb)
        {
            if (!SkeletonMode)
            {
                if (CoreEntity.velocity.X < 0) LeftArmPrims.Draw(sb);
                else RightArmPrims.Draw(sb);

                HairPrimitives.Draw(sb);

                LeftLegPrims.Draw(sb);
                RightLegPrims.Draw(sb);
            }
        }

        public void DrawPrimtiivesAfter(SpriteBatch sb)
        {
            if (!SkeletonMode)
            {
                if (CoreEntity.velocity.X > 0) LeftArmPrims.Draw(sb);
                else RightArmPrims.Draw(sb);
            }
        }
    }
}