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
    public class CorePart : IDrawableEntityModifier
    {
        public AnimationCycle Cycle = AnimationCycle.Walking;
        public Player CoreEntity { get; set; }
        public int Layer { get; set; }
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

                if (Math.Abs(CoreEntity.velocity.X) <= 0.1f)
                {
                    Cycle = AnimationCycle.Idle;
                }
                else
                {
                    Cycle = AnimationCycle.Walking;
                }
            }

            float avg = (Get<LeftLeg>().LegPosition.X + Get<RightLeg>().LegPosition.X) / 2f;
            float Y = Math.Abs(Get<LeftLeg>().LegPosition.Y - Get<RightLeg>().LegPosition.Y);
            entity.Center += (new Vector2(avg, entity.Center.Y) - entity.Center) / 20f;

            foreach (KeyValuePair<string, BodyPart> Parts in Parts)
            {
                Parts.Value.Update();
            }
        }

        public void Draw(in Entity entity, SpriteBatch spriteBatch)
        {
            foreach (KeyValuePair<string, BodyPart> Parts in Parts)
            {
                Parts.Value.Draw(spriteBatch);
            }
        }
    }
}