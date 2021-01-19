using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Flipsider.Engine.Maths
{
    public enum Bound
    {
        None,
        Top,
        Bottom,
        Left,
        Right
    }
    public struct CollisionInfo
    {
        public Bound AABB;
        public Vector2 d;

        public CollisionInfo(Vector2 d, Bound AABB)
        {
            this.AABB = AABB;
            this.d = d;
        }
    }
    public struct Circle
    {
        public float radius;
        public Vector2 position;

        public Circle(float radius, Vector2 position)
        {
            this.radius = radius;
            this.position = position;
        }
    }
    public struct Polygon
    {
        public Vector2[] points;
        public Vector2[] varpoints
        {
            get
            {
                Vector2[] bufferArray = new Vector2[points.Length];
                for(int i = 0; i<points.Length; i++)
                {
                    bufferArray[i] = points[i] + Center;
                }
                return bufferArray;
            }
        }
        public Vector2 Center;
        public int numberOfPoints;
        public Polygon(Vector2[] points, Vector2 position)
        {
            this.points = points;
            Center = position;
            numberOfPoints = points.Length;
        }
    }
    public class Collision
    {
        public static Vector2 TestForCollisions(Polygon shape1, Polygon shape2)
        {
            Polygon[] shapes = new Polygon[] { shape1, shape2 };
            float overlap = float.PositiveInfinity;
            for (int a = 0; a < 2; a++)
            {
                for (int i = 0; i < shapes[a].numberOfPoints; i++)
                {
                    int b = (i + 1) % shapes[a].numberOfPoints;
                    Vector2 axis = shapes[a].varpoints[b] - shapes[a].varpoints[i];
                    Vector2 axisNormal = Vector2.Normalize(new Vector2(-axis.Y, axis.X));
                    float aMax = float.NegativeInfinity;
                    float aMin = float.PositiveInfinity;
                    for (int j = 0; j < shape1.numberOfPoints; j++)
                    {
                        float projection = Vector2.Dot(axisNormal, shape1.varpoints[j]);
                        aMax = Math.Max(projection, aMax);
                        aMin = Math.Min(projection, aMin);
                        
                    }
                    float bMax = float.NegativeInfinity;
                    float bMin = float.PositiveInfinity;
                    for (int j = 0; j < shape2.numberOfPoints; j++)
                    {
                        float projection = Vector2.Dot(axisNormal,shape2.varpoints[j]);
                        bMax = Math.Max(projection, bMax);
                        bMin = Math.Min(projection, bMin);
                    }
                    overlap = Math.Min(Math.Min(bMax,aMax) - Math.Max(bMin,aMin),overlap);
                    if (!(bMax >= aMin && aMax >= bMin))
                        return Vector2.Zero;
                }
            }
            Vector2 disp = shape2.Center - shape1.Center;
            float s = disp.Length();
            return new Vector2(overlap*disp.X/s, overlap * disp.Y / s);
        }
        public static Vector2 TestForCollisionsDiag(Polygon shape1, Polygon shape2)
        {
            Polygon[] shapes = new Polygon[] { shape1, shape2 };
            Vector2 displacement = Vector2.Zero;
            for (int a = 0; a < 2; a++)
            {
                for (int i = 0; i < shapes[a].numberOfPoints; i++)
                {
                    for(int j = 0; j<shapes[(a+1) % 2].numberOfPoints; j++)
                    {
                        Vector2 intersectionPoint = Utils.ReturnIntersectionLine(shapes[a].varpoints[i], shapes[a].Center, shapes[(a + 1) % 2].varpoints[j], shapes[(a + 1) % 2].varpoints[(j + 1) % shapes[(a + 1) % 2].numberOfPoints]);
                        if (intersectionPoint != Vector2.Zero)
                        {
                            displacement += intersectionPoint - shapes[a].varpoints[i];
                            if (a == 1)
                            {
                                displacement *= -1;
                            }
                        }
                    }
                    
                }
            }
            return displacement;
        }
        public bool AABB(Rectangle A, Rectangle B)
        {
            return A.Intersects(B);
        }
        public static CollisionInfo AABBResolve(Rectangle A, Rectangle B)
        {
            Vector2 d = Vector2.Zero;
            Bound bound = Bound.None;
            if (!A.Intersects(new Rectangle(B.X - 1,B.Y - 1,B.Width + 2,B.Height + 2)))
                return new CollisionInfo(d, bound);
            else
            {
                if(A.Center.X > B.Center.X && A.Bottom > B.Center.Y && A.Top < B.Center.Y)
                {
                    bound = Bound.Left;
                    d = new Vector2(B.Right - A.Left, 0);
                }
                if (A.Center.X < B.Center.X && A.Bottom > B.Center.Y && A.Top < B.Center.Y)
                {
                    bound = Bound.Right;
                    d = new Vector2(B.Left - A.Right, 0);
                }
                if (A.Center.Y > B.Center.Y && A.Top > B.Center.Y)
                {
                    bound = Bound.Bottom;
                    d = new Vector2(0, B.Top - A.Bottom);
                }
                if (A.Center.Y < B.Center.Y && A.Bottom < B.Center.Y)
                {
                    bound = Bound.Top;
                    d = new Vector2(0, B.Top - A.Bottom);
                }
            }
            return new CollisionInfo(d, bound);
        }
        public static CollisionInfo AABBResolve(Rectangle A, Rectangle ALast, Rectangle B)
        {
            Vector2 d = Vector2.Zero;
            Bound bound = Bound.None;
            if (!A.Intersects(new Rectangle(B.X , B.Y - 1, B.Width, B.Height + 2)))
                return new CollisionInfo(d, bound);
            else
            {
                if (ALast.Bottom > B.Top && ALast.Top < B.Bottom)
                {
                    if (ALast.Left > B.Center.X)
                    {
                        bound = Bound.Left;
                        d = new Vector2(B.Right - A.Left, 0);
                    }
                    if (ALast.Right < B.Center.X)
                    {
                        bound = Bound.Right;
                        d = new Vector2(B.Left - A.Right, 0);
                    }
                }
                if (ALast.Left < B.Right && ALast.Right > B.Left)
                {
                    if (ALast.Top > B.Center.Y)
                    {
                        bound = Bound.Bottom;
                        d = new Vector2(0, B.Bottom - A.Top);
                    }
                    if (ALast.Bottom < B.Center.Y)
                    {
                        bound = Bound.Top;
                        d = new Vector2(0, B.Top - A.Bottom);
                    }
                }
            }
            return new CollisionInfo(d, bound);
        }
        public bool CirclevsCircle(Circle a, Circle b)
        {
            float r = a.radius + b.radius;
            r *= r;
            return r < (a.position.X + b.position.X)* (a.position.X + b.position.X) + 
                       (a.position.Y + b.position.Y)* (a.position.Y + b.position.Y);
        }
    }
}
