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

        public bool SkeletonMode => true;
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

            HairPrimitives = new HairPrimtives(this, Textures._BodyParts_RightArm);

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

                if(IdleTimer > 30)
                {
                    Cycle = AnimationCycle.Idle;
                    entity.Center += ((new Vector2(avg, entity.Center.Y) - entity.Center) / 200f);
                }
                else
                {
                    Cycle = AnimationCycle.Walking;
                }

                if (Cycle == AnimationCycle.Idle)
                {
                    entity.Center += ((new Vector2(avg, entity.Center.Y) - entity.Center) / 200f);
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
            if(CoreEntity != null) UpperBodyRotation = CoreEntity.velocity.X / 15f + rl.XAccelInterp;
            Verlet.Instance.points[MainVerletPoint].point = Center - rl.LYOff * 0.2f - new Vector2(4 * rl.Sign, 27).RotatedBy(UpperBodyRotation);

        }

        public void Draw(in Entity entity, SpriteBatch spriteBatch)
        {
            foreach (KeyValuePair<string, BodyPart> Parts in Parts)
            {
                Parts.Value.Draw(spriteBatch);
            }

            if (!SkeletonMode)
            {
                Leg rl = Get<RightLeg>();

                Texture2D head = Textures._BodyParts_bitch_head;
                Texture2D tex = Textures._BodyParts_bitch_body;

                spriteBatch.Draw(tex, Center - rl.LYOff * 0.2f + new Vector2(0, 7), tex.Bounds, Color.White, UpperBodyRotation, new Vector2(tex.Width / 2f, tex.Height), 2f,
                    CoreEntity.velocity.X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

                spriteBatch.Draw(head, Center - rl.LYOff * 0.2f - new Vector2(-4 * rl.Sign, 11), head.Bounds, Color.White, CoreEntity.velocity.X / 7f, new Vector2(head.Width / 2f, head.Height), 2f,
                    CoreEntity.velocity.X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

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
                LeftLegPrims.Draw(sb);
                RightLegPrims.Draw(sb);
                HairPrimitives.Draw(sb);

                if (CoreEntity.velocity.X < 0) LeftArmPrims.Draw(sb);
                else RightArmPrims.Draw(sb);
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