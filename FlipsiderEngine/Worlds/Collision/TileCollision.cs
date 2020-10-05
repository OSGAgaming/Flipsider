using Flipsider.Core;
using Flipsider.Worlds.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flipsider.Worlds.Collision
{
    public sealed class TileCollision : ICollideable
    {
        public RectangleF Bounds => RectangleF.Plane;

        void ICollideable.Intersect(ICollideable other) { }
    }
}
