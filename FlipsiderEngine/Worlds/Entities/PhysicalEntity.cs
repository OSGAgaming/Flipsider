using Flipsider.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Flipsider.Worlds.Entities
{
    public abstract class PhysicalEntity : Entity
    {
        protected PhysicalEntity()
        {
            OnUpdate += UpdatePhysics;
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
        /// The body of water that most recently collided with the physical entity.
        /// </summary>
        public Water? WetIn { get; internal set; }

        public Vector2 TopLeft => Center - Size / 2;
        public Vector2 TopRight => Center + Size / 2;

        /// <summary>
        /// Updates the acceleration, velocity, and position of the entity.
        /// </summary>
        protected virtual void UpdatePhysics()
        {
            Velocity += Acceleration * Time.DeltaF;
            Center += Velocity * Time.DeltaF;
            Velocity *= 1 - Friction * Time.DeltaF;
        }
    }
}
