using Flipsider.Core;
using Flipsider.Worlds.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flipsider.Worlds.Collision
{
    /// <summary>
    /// Represents a physical object with a position and a velocity.
    /// </summary>
    public interface IPhysical
    {
        Vector2 Position { get; }
        Vector2 Velocity { get; }
    }
    
    /// <summary>
    /// Represents an object that can collide with <see cref="Liquid"/>.
    /// </summary>
    public interface IWettable : IPhysical
    {
        /// <summary>
        /// The collision bounds of this object.
        /// </summary>
        RectangleF Bounds { get; }
        void OnEnter(Liquid water);
        void OnExit(Liquid water);
    }
}
