using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace FlipEngine
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
        public static CollisionInfo Default => new CollisionInfo(Vector2.Zero, Bound.None);
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
                for (int i = 0; i < points.Length; i++)
                {
                    bufferArray[i] = points[i] + Center;
                }
                return bufferArray;
            }
        }
        public Vector2 Center;
        public int numberOfPoints;
        public static Polygon Null => new Polygon(new Vector2[] { }, Vector2.Zero);
        public Rectangle Rectangle
        {
            get
            {
                if (points.Length == 4)
                {
                    return new Rectangle(
                        varpoints[0].ToPoint(),
                        new Point((int)(varpoints[1].X - varpoints[0].X),
                        (int)(varpoints[2].Y - varpoints[0].Y)));
                }
                return Rectangle.Empty;
            }
        }

        public bool IsRectangle
        {
            get
            {
                if (points.Length == 4)
                {
                    float dot1 = Vector2.Dot(varpoints[1] - varpoints[0], varpoints[1] - varpoints[2]);
                    float dot2 = Vector2.Dot(varpoints[3] - varpoints[0], varpoints[3] - varpoints[2]);

                    return dot1 == 0 && dot2 == 0;
                }
                return false;
            }
        }

        public void Draw(Color color = default)
        {
            for (int i = 0; i < points.Length; i++)
            {
                Utils.DrawLine(varpoints[i], varpoints[(i + 1) % numberOfPoints], color, 2);
                Utils.DrawLine(varpoints[i], Center, color, 2);
            }
        }
        public static Polygon operator +(Polygon x, Polygon y)
        {
            if (x.numberOfPoints == y.numberOfPoints)
            {
                Vector2[] points = new Vector2[x.numberOfPoints];
                for (int i = 0; i < x.numberOfPoints; i++)
                {
                    points[i] = x.points[i] + y.points[i];
                }
                return new Polygon(points, x.Center + y.Center);
            }
            return new Polygon(new Vector2[] { }, Vector2.Zero);
        }

        public Polygon(Vector2[] points, Vector2 position)
        {
            this.points = points;
            Center = position;
            numberOfPoints = points.Length;
        }
    }
    public static class Collision
    {
        // Given three collinear points p, q, r,
        // the function checks if point q lies
        // on line segment 'pr'
        static bool onSegment(Vector2 p, Vector2 q, Vector2 r)
        {
            if (q.X <= Math.Max(p.X, r.X) &&
                q.X >= Math.Min(p.X, r.X) &&
                q.Y <= Math.Max(p.Y, r.Y) &&
                q.Y >= Math.Min(p.Y, r.Y))
            {
                return true;
            }
            return false;
        }

        // To find orientation of ordered triplet (p, q, r).
        // The function returns following values
        // 0 --> p, q and r are collinear
        // 1 --> Clockwise
        // 2 --> Counterclockwise
        static int orientation(Vector2 p, Vector2 q, Vector2 r)
        {
            float val = (q.Y - p.Y) * (r.X - q.X) -
                    (q.X - p.X) * (r.Y - q.Y);

            if (val == 0)
            {
                return 0; // collinear
            }
            return (val > 0) ? 1 : 2; // clock or counterclock wise
        }

        // The function that returns true if
        // line segment 'p1q1' and 'p2q2' intersect.
        static bool doIntersect(Vector2 p1, Vector2 q1,
                                Vector2 p2, Vector2 q2)
        {
            // Find the four orientations needed for
            // general and special cases
            int o1 = orientation(p1, q1, p2);
            int o2 = orientation(p1, q1, q2);
            int o3 = orientation(p2, q2, p1);
            int o4 = orientation(p2, q2, q1);

            // General case
            if (o1 != o2 && o3 != o4)
            {
                return true;
            }

            // Special Cases
            // p1, q1 and p2 are collinear and
            // p2 lies on segment p1q1
            if (o1 == 0 && onSegment(p1, p2, q1))
            {
                return true;
            }

            // p1, q1 and p2 are collinear and
            // q2 lies on segment p1q1
            if (o2 == 0 && onSegment(p1, q2, q1))
            {
                return true;
            }

            // p2, q2 and p1 are collinear and
            // p1 lies on segment p2q2
            if (o3 == 0 && onSegment(p2, p1, q2))
            {
                return true;
            }

            // p2, q2 and q1 are collinear and
            // q1 lies on segment p2q2
            if (o4 == 0 && onSegment(p2, q1, q2))
            {
                return true;
            }

            // Doesn't fall in any of the above cases
            return false;
        }

        // Returns true if the point p lies
        // inside the polygon[] with n vertices
        public static bool Contains(this Polygon polygon, Vector2 p)
        {
            // There must be at least 3 vertices in polygon[]
            if (polygon.numberOfPoints < 3)
            {
                return false;
            }

            // Create a point for line segment from p to infinite
            Vector2 extreme = new Vector2(int.MaxValue, p.Y);

            // Count intersections of the above line
            // with sides of polygon
            int count = 0, i = 0;
            do
            {
                int next = (i + 1) % polygon.numberOfPoints;

                // Check if the line segment from 'p' to
                // 'extreme' intersects with the line
                // segment from 'polygon[i]' to 'polygon[next]'
                if (doIntersect(polygon.varpoints[i],
                                polygon.varpoints[next], p, extreme))
                {
                    // If the point 'p' is collinear with line
                    // segment 'i-next', then check if it lies
                    // on segment. If it lies, return true, otherwise false
                    if (orientation(polygon.varpoints[i], p, polygon.varpoints[next]) == 0)
                    {
                        return onSegment(polygon.varpoints[i], p,
                                        polygon.varpoints[next]);
                    }
                    count++;
                }
                i = next;
            } while (i != 0);

            // Return true if count is odd, false otherwise
            return (count % 2 == 1); // Same as (count%2 == 1)
        }

        public static Rectangle ToR(this RectangleF x)
        {
            return new Rectangle((int)x.x, (int)x.y, (int)x.width, (int)x.height);
        }

        public static Polygon Add(this Polygon x, Vector2[] y)
        {
            Vector2[] points = new Vector2[x.numberOfPoints];
            for (int i = 0; i < x.numberOfPoints; i++)
            {
                points[i] = x.points[i] + y[i];
            }
            x.points = points;
            return x;
        }

        public static bool Intersects(this RectangleF r1, RectangleF r2) => 
            ((r1.x > r2.x && r1.x < r2.right) || (r2.x > r1.x && r2.x < r1.right)) &&
            ((r1.y > r2.y && r1.y < r2.bottom) || (r2.y > r1.y && r2.y < r1.bottom));

        public static bool Contains(this RectangleF r, Vector2 v) => v.X > r.x && v.Y > r.y && v.X < r.right && v.Y < r.bottom;

        public static bool Contains(this RectangleF r, Point p) => p.X > r.x && p.Y > r.y && p.X < r.right && p.Y < r.bottom;

        public static Polygon ToPolygon(this Rectangle r)
        {
            Vector2[] points = {new Vector2(-r.Width / 2, -r.Height / 2), 
              new Vector2(r.Width / 2, -r.Height / 2),
              new Vector2(r.Width / 2, r.Height / 2),
              new Vector2(-r.Width / 2, r.Height / 2)};
            return new Polygon(points, r.Center.ToVector2());
        }
        public static Polygon ToPolygon(this RectangleF r)
        {
            Vector2[] points = {new Vector2(-r.width / 2, -r.height / 2),
              new Vector2(r.width / 2, -r.height / 2),
              new Vector2(r.width / 2, r.height / 2),
              new Vector2(-r.width / 2, r.height / 2)};
            return new Polygon(points, r.Center);
        }

        public static Polygon ToPolygon(this Rectangle r, Vector2 c)
        {
            Vector2[] points = {new Vector2(-r.Width / 2, -r.Height / 2),
              new Vector2(r.Width / 2, -r.Height / 2),
              new Vector2(r.Width / 2, r.Height / 2),
              new Vector2(-r.Width / 2, r.Height / 2)};
            return new Polygon(points, c);
        }
        public static CollisionInfo Raycast(Polygon shape1, Polygon shape2, int LengthOfRaycast, int Disp)
        {
            Vector2 CollisionPoint = Utils.ReturnIntersectionLine(shape1.Center, shape1.Center + new Vector2(0, LengthOfRaycast), shape2.varpoints[0], shape2.varpoints[1]);
            Bound b = Bound.None;
            Vector2 v = Vector2.Zero;
            float d = shape1.Center.Y + Disp;
            if (CollisionPoint != Vector2.Zero)
            {
                if (d > CollisionPoint.Y)
                {
                    v = new Vector2(0, CollisionPoint.Y - d);
                }
                if (d > CollisionPoint.Y - 1)
                {
                    b = Bound.Top;
                }
            }


            return new CollisionInfo(v, b);
        }
        public static CollisionInfo SAT(Polygon shape1, Polygon shape2)
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
                        float projection = Vector2.Dot(axisNormal, shape2.varpoints[j]);
                        bMax = Math.Max(projection, bMax);
                        bMin = Math.Min(projection, bMin);
                    }
                    overlap = Math.Min(Math.Min(bMax, aMax) - Math.Max(bMin, aMin), overlap);
                    if (!(bMax >= aMin && aMax >= bMin))
                        return new CollisionInfo(Vector2.Zero, Bound.None);
                }
            }
            Vector2 disp = shape2.Center - shape1.Center;
            float s = disp.Length();
            return new CollisionInfo(new Vector2(overlap * disp.X / s, overlap * disp.Y / s), Bound.Top);
        }
        public static Vector2 TestForCollisionsDiag(Polygon shape1, Polygon shape2)
        {
            Polygon[] shapes = new Polygon[] { shape1, shape2 };
            Vector2 displacement = Vector2.Zero;
            for (int a = 0; a < 2; a++)
            {
                for (int i = 0; i < shapes[a].numberOfPoints; i++)
                {
                    for (int j = 0; j < shapes[(a + 1) % 2].numberOfPoints; j++)
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
        public static bool AABB(Rectangle A, Rectangle B)
        {
            return A.Intersects(B);
        }
        public static CollisionInfo AABBResolvePolyOut(Polygon _A, Polygon _ALast, Polygon _B)
        {
            Vector2 d = Vector2.Zero;
            Bound bound = Bound.None;
            Rectangle A = _A.Rectangle;
            Rectangle ALast = _ALast.Rectangle;
            Rectangle B = _B.Rectangle;
            if (!A.Intersects(new Rectangle(B.X, B.Y - 1, B.Width, B.Height + 2)))
                return CollisionInfo.Default;
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

        public static CollisionInfo AABBResolvePolyIn(Polygon _A, Polygon _ALast, Polygon _B)
        {
            Vector2 d = Vector2.Zero;
            Bound bound = Bound.None;
            Rectangle A = _A.Rectangle;
            Rectangle ALast = _ALast.Rectangle;
            Rectangle B = _B.Rectangle;
            if (!A.Intersects(new Rectangle(B.X, B.Y - 1, B.Width, B.Height + 2)))
                return CollisionInfo.Default;
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
        public static bool CirclevsCircle(Circle a, Circle b)
        {
            float r = a.radius + b.radius;
            r *= r;
            return r < (a.position.X + b.position.X) * (a.position.X + b.position.X) +
                       (a.position.Y + b.position.Y) * (a.position.Y + b.position.Y);
        }
    }
}
