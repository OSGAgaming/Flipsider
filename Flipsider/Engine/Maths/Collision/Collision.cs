using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flipsider.Engine.Maths
{
    struct Circle
    {
        public float radius;
        public Vector2 position;

        public Circle(float radius, Vector2 position)
        {
            this.radius = radius;
            this.position = position;
        }
    }
    public class Collision
    {
        bool AABB(Rectangle A, Rectangle B)
        {
            return A.Intersects(B);
        }
        bool CirclevsCircle(Circle a, Circle b)
        {
            float r = a.radius + b.radius;
            r *= r;
            return r < (a.position.X + b.position.X)* (a.position.X + b.position.X) + 
                       (a.position.Y + b.position.Y)* (a.position.Y + b.position.Y);
        }
    }
}
