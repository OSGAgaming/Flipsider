﻿using Flipsider.Assets;
using Flipsider.Core;
using Flipsider.Extensions;
using Flipsider.Worlds.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Flipsider.Worlds.Entities
{
    public abstract class PhysicalEntity : Entity, IPhysical, IWettable, ICollideable
    {
        protected PhysicalEntity(bool registerCollision)
        {
            OnUpdate += UpdatePhysics;
            if (registerCollision)
            {
                OnSpawn += delegate { InWorld.Collision.Add(this); };
                OnRemove += delegate { InWorld.Collision.Remove(this); };
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
        protected virtual void UpdatePhysics()
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

        public virtual void Intersect(ICollideable other)
        {

        }

        protected void Draw(SafeSpriteBatch sb, Asset<Texture2D> texture, Rectangle? frame = null)
        {
            if (texture != null)
            {
                Vector2 scale = Size / texture.Value.Size();
                sb.Sb.Draw(texture, Center, frame, Color.White, Rotation.RadF, Size / 2, scale, SpriteEffects.None, 0);
            }
        }
    }
}
