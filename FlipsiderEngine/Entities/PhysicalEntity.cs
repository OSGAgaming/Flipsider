using Flipsider.Assets;
using Flipsider.Core;
using Flipsider.Extensions;
using Flipsider.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Flipsider.Graphics;

namespace Flipsider.Entities
{
    public abstract class PhysicalEntity : Entity, IPhysical, IWettable, ICollideable
    {
        protected PhysicalEntity(bool registerCollision)
        {
            OnUpdate += UpdatePhysics;
            if (registerCollision)
            {
                OnSpawn += me => me.World.Collision.AddCollideable(this);
                OnRemove += me => me.World.Collision.RemoveCollideable(this);
            }
        }

        /// <summary>
        /// Size of the physical object.
        /// </summary>
        public Vector2 Size;
        /// <summary>
        /// Added velocity per unit time.
        /// </summary>
        public Vector2 Acceleration;
        /// <summary>
        /// Added position per unit time.
        /// </summary>
        public Vector2 Velocity;
        /// <summary>
        /// How much speed the entity loses per tick, in percent.
        /// </summary>
        public float Friction;

        /// <summary>
        /// The rotation of the entity.
        /// </summary>
        public Rotation Rotation;

        /// <summary>
        /// The rate of change of <see cref="Rotation"/>.
        /// </summary>
        public Rotation RotationDelta;

        public Vector2 TopLeft => Center - Size / 2;
        public Vector2 TopRight => Center + Size / 2;

        public RectangleF Bounds => new RectangleF { Center = Center, Size = Size };

        Vector2 IPhysical.Position => Center;

        Vector2 IPhysical.Velocity => Velocity;

        /// <summary>
        /// Updates the acceleration, velocity, and position of the entity.
        /// </summary>
        protected virtual void UpdatePhysics(WorldEntity me)
        {
            Rotation += new Rotation(RotationDelta.Rad * Time.DeltaD);
            Velocity += Acceleration * Time.DeltaF;
            Center += Velocity * Time.DeltaF;
            Velocity *= 1 - Friction * Time.DeltaF;
        }

        public virtual void OnEnter(Liquid water)
        {
            
        }

        public virtual void OnExit(Liquid water)
        {

        }

        protected void Draw(SafeSpriteBatch sb, Asset<Texture2D> texture, Rectangle? frame = null)
        {
            if (texture != null)
            {
                sb.Draw(new DrawData(texture)
                {
                    WorldPosition = Center,
                    Frame = frame,
                    Scale = Size / texture.Value.Size()
                });
            }
        }
    }
}
