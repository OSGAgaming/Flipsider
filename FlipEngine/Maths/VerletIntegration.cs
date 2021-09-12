
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FlipEngine
{
    public class Verlet : IUpdate
    {
        public Verlet()
        {
            Main.Updateables.Add(this);
        }
        private readonly float _gravity = 0.5f;
        private readonly float _bounce = 0.9f;
        private readonly float _AR = 0.99f;
        private readonly int _fluff = 1;
        private readonly float bounce = 0.9f;
        public List<Stick> stickPoints = new List<Stick>();
        public List<Point> points = new List<Point>();
        public int CreateVerletPoint(Vector2 pos, bool isStatic = false)
        {
            points.Add(new Point(pos, pos - new Vector2(FlipE.rand.Next(-10, 10), FlipE.rand.Next(-10, 10)), isStatic));

            return points.Count - 1;
        }

        public void ClearPoints()
        {
            points.Clear();
            stickPoints.Clear();
        }

        public int[] CreateVerletSquare(Vector2 pos, int size)
        {
            int a = CreateVerletPoint(pos + new Vector2(-size / 2, -size / 2));
            int b = CreateVerletPoint(pos + new Vector2(size / 2, -size / 2));
            int c = CreateVerletPoint(pos + new Vector2(size / 2, size / 2));
            int d = CreateVerletPoint(pos + new Vector2(-size / 2, size / 2));

            BindPoints(a, b);
            BindPoints(b, c);
            BindPoints(c, d);
            BindPoints(d, a);
            BindPoints(a, c);

            return new int[] { a, b, c, d };
        }

        public int[] CreateStickMan(Vector2 pos)
        {
            int first = CreateVerletPoint(pos);
            int b = CreateVerletPoint(pos + new Vector2(0, 40));
            int c = CreateVerletPoint(pos + new Vector2(0, 60));
            int d = CreateVerletPoint(pos + new Vector2(20, 90));
            int e = CreateVerletPoint(pos + new Vector2(-20, 90));
            int f = CreateVerletPoint(pos + new Vector2(-25, 120));
            int g = CreateVerletPoint(pos + new Vector2(25, 120));
            int h = CreateVerletPoint(pos + new Vector2(-20, 40));
            int i = CreateVerletPoint(pos + new Vector2(-30, 60));
            int j = CreateVerletPoint(pos + new Vector2(20, 40));
            int k = CreateVerletPoint(pos + new Vector2(30, 60));
            int a = CreateVerletPoint(pos + new Vector2(0, 20));

            BindPoints(first, a);
            BindPoints(a, b);
            BindPoints(b, c);
            BindPoints(c, d);
            BindPoints(d, g);

            BindPoints(c, e);
            BindPoints(e, f);
            BindPoints(a, h);
            BindPoints(h, i);
            BindPoints(a, j);
            BindPoints(j, k);

            BindPoints(first, c, true, Color.Yellow);
            BindPoints(a, i, true, Color.Yellow);
            BindPoints(a, k, true, Color.Yellow);
            BindPoints(j, h, true, Color.Yellow);
            BindPoints(f, d, true, Color.Yellow);

            return new int[] { first, a, b, c, d, e, f, g, h, i, j, k };
        }

        public void BindPoints(int a, int b, bool isVisible = true, Color color = default)
        {
            try
            {
                stickPoints.Add(new Stick(this, a, b, isVisible, color));
            }
            catch
            {

            }
        }

        public void Update()
        {
            UpdatePoints();

            for (int i = 0; i < 5; i++)
            {
                UpdateSticks();
                ConstrainPoints();
            }
        }

        public void GlobalRenderPoints()
        {
            RenderPoints();
            RenderSticks();
        }

        public class Point
        {
            public Vector2 point;
            public Vector2 oldPoint;
            public Vector2 vel;
            public bool isStatic;

            public Point(Vector2 point, Vector2 oldPoint, Vector2 vel)
            {
                this.point = point;
                this.oldPoint = oldPoint;
                this.vel = vel;
                isStatic = false;
            }

            public Point(Vector2 point)
            {
                this.point = point;
                oldPoint = point;
                vel = Vector2.Zero;
                isStatic = false;
            }

            public Point(Vector2 point, Vector2 oldPoint, bool isStatic)
            {
                this.point = point;
                this.oldPoint = oldPoint;
                this.isStatic = isStatic;
                vel = Vector2.Zero;
            }

            public Point(Vector2 point, Vector2 oldPoint)
            {
                this.point = point;
                this.oldPoint = oldPoint;
                vel = Vector2.Zero;
                isStatic = false;
            }
        }

        public class Stick
        {
            public Color color;
            public Vector2 p1;
            public Vector2 p2;
            public Vector2 oldP1;
            public Vector2 oldP2;
            public Vector2 vel1;
            public Vector2 vel2;
            public bool[] isStatic;
            public float Length;
            public int a;
            public int b;
            public bool isVisible;

            public Stick(Verlet verlet, int a, int b, bool isVisible = true, Color color = default)
            {
                this.a = a;
                this.b = b;
                isStatic = new bool[2];
                p1 = verlet.points[a].point;
                p2 = verlet.points[b].point;
                oldP1 = verlet.points[a].oldPoint;
                oldP2 = verlet.points[b].oldPoint;
                vel1 = verlet.points[a].vel;
                vel2 = verlet.points[b].vel;

                float disX = verlet.points[b].point.X - verlet.points[a].point.X;
                float disY = verlet.points[b].point.Y - verlet.points[a].point.Y;

                Length = (float)Math.Sqrt(disX * disX + disY * disY);
                isStatic[0] = verlet.points[a].isStatic;
                isStatic[1] = verlet.points[b].isStatic;

                if (color == default)
                {
                    this.color = Color.DarkRed;
                }
                else
                {
                    this.color = color;
                }

                this.isVisible = isVisible;
            }
        }

        private void UpdateSticks()
        {
            for (int i = 0; i < stickPoints.Count; i++)
            {
                Stick stick = stickPoints[i];
                Point p1 = points[stick.a];
                Point p2 = points[stick.b];
                float dx = p2.point.X - p1.point.X;
                float dy = p2.point.Y - p1.point.Y;
                float currentLength = (float)Math.Sqrt(dx * dx + dy * dy);
                float deltaLength = currentLength - stick.Length;
                float perc = deltaLength / currentLength * 0.5f;
                float offsetX = perc * dx;
                float offsetY = perc * dy;

                if (!stickPoints[i].isStatic[0])
                {
                    points[stick.a].point.X += offsetX;
                    points[stick.a].point.Y += offsetY;
                }

                if (!stickPoints[i].isStatic[1])
                {
                    points[stick.b].point.X -= offsetX;
                    points[stick.b].point.Y -= offsetY;
                }
            }
        }


        private void UpdatePoints()
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (!points[i].isStatic)
                {
                    points[i].vel.X = (points[i].point.X - points[i].oldPoint.X) * _AR;
                    points[i].vel.Y = (points[i].point.Y - points[i].oldPoint.Y) * _AR;
                    points[i].oldPoint.X = points[i].point.X;
                    points[i].oldPoint.Y = points[i].point.Y;
                    points[i].point.X += points[i].vel.X;
                    points[i].point.Y += points[i].vel.Y;
                    points[i].point.Y += _gravity;
                }
            }
        }

        private void RenderPoints()
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (i == 0)
                {
                    Utils.DrawPixel(points[i].point, Color.Black);
                }
                else
                {
                    //TODO: Do something.
                }
            }
        }

        private void RenderSticks()
        {
            for (int i = 0; i < stickPoints.Count; i++)
            {
                if (stickPoints[i].isVisible)
                {
                    Vector2 p1 = points[stickPoints[i].a].point;
                    Vector2 p2 = points[stickPoints[i].b].point;
                    Utils.DrawLine(p1, p2, stickPoints[i].color);
                }
            }
        }



        private void ConstrainPoints()
        {
            for (int i = 0; i < points.Count; i++)
            {
                Vector2 size = Main.ScreenSize;
                if (!points[i].isStatic)
                {
                    points[i].vel.X = (points[i].point.X - points[i].oldPoint.X) * _AR;
                    points[i].vel.Y = (points[i].point.Y - points[i].oldPoint.Y) * _AR;

                    if (points[i].point.Y > size.Y)
                    {
                        points[i].oldPoint.Y = size.Y + points[i].vel.Y * bounce;
                        points[i].point.Y = size.Y;
                    }
                    if (points[i].point.Y < 0)
                    {
                        points[i].oldPoint.Y = points[i].vel.Y * bounce;
                        points[i].point.Y = 0;
                    }
                }
            }
        }

    }
}
