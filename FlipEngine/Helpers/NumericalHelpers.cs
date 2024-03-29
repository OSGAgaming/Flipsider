using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FlipEngine
{
    public static partial class Utils
    {
        public static string AssetDirectory => Environment.ExpandEnvironmentVariables($@"{MainDirectory}\Content\Textures");
        public static string MainDirectory => Environment.ExpandEnvironmentVariables($@"{Environment.CurrentDirectory}");
        public static string LocalDirectory => Environment.ExpandEnvironmentVariables($@"{Environment.CurrentDirectory.Split(@"\bin")[0]}");

        public static IEnumerable<T> Flatten<T>(this T[,] matrix)
        {
            foreach (var item in matrix) yield return item;
        }
        public static float X(float t,
        float x0, float x1, float x2, float x3)
        {
            return (float)(
                x0 * (1 - t) * (1 - t) * (1 - t) +
                x1 * 3 * t * (1 - t) * (1 - t) +
                x2 * 3 * t * t * (1 - t) +
                x3 * t * t * t
            );
        }

        public static float Y(float t,
            float y0, float y1, float y2, float y3)
        {
            return (float)(
                 y0 * (1 - t) * (1 - t) * (1 - t) +
                 y1 * 3 * t * (1 - t) * (1 - t) +
                 y2 * 3 * t * t * (1 - t) +
                 y3 * t * t * t
             );
        }
        private static float X(float t,
   float x0, float x1, float x2)
        {
            return (float)(
                x0 * (1 - t) * (1 - t) +
                x1 * 2 * t * (1 - t) +
                x2 * t * t
            );
        }

        public static float Y(float t,
            float y0, float y1, float y2)
        {
            return (float)(
                y0 * (1 - t) * (1 - t) +
                y1 * 2 * t * (1 - t) +
                y2 * t * t
            );
        }
        public static Vector2 TraverseBezier(Vector2 endPoints, Vector2 startingPos, Vector2 c1, Vector2 c2, float t)
        {
            float x = X(t, startingPos.X, c1.X, c2.X, endPoints.X);
            float y = Y(t, startingPos.Y, c1.Y, c2.Y, endPoints.Y);
            return new Vector2(x, y);
        }
        public static Vector2 TraverseBezier(Vector2 startingPos, Vector2 c1, Vector2 endPoints, float t)
        {
            float x = X(t, startingPos.X, c1.X, endPoints.X);
            float y = Y(t, startingPos.Y, c1.Y, endPoints.Y);
            return new Vector2(x, y);
        }
        public static bool LineIntersectsRect(Point p1, Point p2, Rectangle r)
        {
            return LineIntersectsLine(p1, p2, new Point(r.X, r.Y), new Point(r.X + r.Width, r.Y)) ||
                   LineIntersectsLine(p1, p2, new Point(r.X + r.Width, r.Y), new Point(r.X + r.Width, r.Y + r.Height)) ||
                   LineIntersectsLine(p1, p2, new Point(r.X + r.Width, r.Y + r.Height), new Point(r.X, r.Y + r.Height)) ||
                   LineIntersectsLine(p1, p2, new Point(r.X, r.Y + r.Height), new Point(r.X, r.Y)) ||
                   (r.Contains(p1) && r.Contains(p2));
        }
        private static bool LineIntersectsLine(Point l1p1, Point l1p2, Point l2p1, Point l2p2)
        {
            float q = (l1p1.Y - l2p1.Y) * (l2p2.X - l2p1.X) - (l1p1.X - l2p1.X) * (l2p2.Y - l2p1.Y);
            float d = (l1p2.X - l1p1.X) * (l2p2.Y - l2p1.Y) - (l1p2.Y - l1p1.Y) * (l2p2.X - l2p1.X);
            if (d == 0)
                return false;
            float r = q / d;
            q = (l1p1.Y - l2p1.Y) * (l1p2.X - l1p1.X) - (l1p1.X - l2p1.X) * (l1p2.Y - l1p1.Y);
            float s = q / d;
            if (r < 0 || r > 1 || s < 0 || s > 1)
                return false;
            return true;
        }

        public static bool LineIntersectsTile(World world, Point p1, Point p2)
        {
            Point TilePos1 = p1.ToTile();
            Point TilePos2 = p2.ToTile();
            int lowestX = (TilePos1.X < TilePos2.X) ? TilePos1.X : TilePos2.X;
            int lowestY = (TilePos1.Y < TilePos2.Y) ? TilePos1.Y : TilePos2.Y;
            int highestX = (TilePos1.X > TilePos2.X) ? TilePos1.X : TilePos2.X;
            int highestY = (TilePos1.Y > TilePos2.Y) ? TilePos1.Y : TilePos2.Y;
            for (int i = lowestX - 1; i < highestX + 1; i++)
            {
                for (int j = lowestY - 1; j < highestY + 1; j++)
                {
                    if (i >= 0 && i < world.MaxTilesX && j >= 0 && j < world.MaxTilesY)
                    {
                        if (world.IsTileActive(i, j))
                        {
                            int tileRes = TileManager.tileRes;
                            if (LineIntersectsRect(p1, p2, new Rectangle(i * tileRes, j * tileRes, tileRes, tileRes)))
                                return true;
                        }
                    }
                }
            }
            return false;
        }

        public static float ToRotation(this Vector2 v)
        {
            return (float)Math.Atan2(v.Y, v.X);
        }
        public static Vector2 ReturnIntersectRect(Vector2 p1, Vector2 p2, Rectangle r)
        {
            Vector2[] a = { ReturnIntersectionLine(p1, p2, new Vector2(r.X, r.Y), new Vector2(r.X + r.Width, r.Y)),
            ReturnIntersectionLine(p1, p2, new Vector2(r.X + r.Width, r.Y), new Vector2(r.X + r.Width, r.Y + r.Height)),
            ReturnIntersectionLine(p1, p2, new Vector2(r.X + r.Width, r.Y + r.Height), new Vector2(r.X, r.Y + r.Height)),
            ReturnIntersectionLine(p1, p2, new Vector2(r.X, r.Y + r.Height), new Vector2(r.X, r.Y)) };
            Vector2 chosen = Vector2.Zero;
            for (int i = 0; i < a.Length; i++)
            {
                if (i == 0)
                    chosen = a[0];
                else if (Vector2.Distance(a[i], p1) < Vector2.Distance(chosen, p1))
                {
                    chosen = a[i];
                }
            }
            return chosen;

        }
        public static Vector2 RotatedBy(this Vector2 spinningpoint, double radians, Vector2 center = default(Vector2))
        {
            float num = (float)Math.Cos(radians);
            float num2 = (float)Math.Sin(radians);
            Vector2 vector = spinningpoint - center;
            Vector2 result = center;
            result.X += vector.X * num - vector.Y * num2;
            result.Y += vector.X * num2 + vector.Y * num;
            return result;
        }

        public static int Round(float num, int round)
        {
            return (int)(num / round) * round;
        }
        public static Vector2 ReturnIntersectionTile(World world, Point p1, Point p2)
        {
            Point TilePos1 = p1.ToTile();
            Point TilePos2 = p2.ToTile();
            int lowestX = (TilePos1.X < TilePos2.X) ? TilePos1.X : TilePos2.X;
            int lowestY = (TilePos1.Y < TilePos2.Y) ? TilePos1.Y : TilePos2.Y;
            int highestX = (TilePos1.X > TilePos2.X) ? TilePos1.X : TilePos2.X;
            int highestY = (TilePos1.Y > TilePos2.Y) ? TilePos1.Y : TilePos2.Y;
            Vector2 chosen = Vector2.Zero;

            for (int i = lowestX - 1; i < highestX + 1; i++)
            {
                for (int j = lowestY - 1; j < highestY + 1; j++)
                {
                    if (i >= 0 && i < world.MaxTilesX && j >= 0 && j < world.MaxTilesY)
                    {
                        if (world.IsTileActive(i, j))
                        {
                            int tileRes = world.TileRes;
                            if (LineIntersectsRect(p1, p2, new Rectangle(i * tileRes, j * tileRes, tileRes, tileRes)))
                            {
                                Vector2 inter = ReturnIntersectRect(p1.ToVector2(), p2.ToVector2(), new Rectangle(i * tileRes, j * tileRes, tileRes, tileRes));
                                if (Vector2.Distance(inter, p1.ToVector2()) < Vector2.Distance(chosen, p1.ToVector2()))
                                    chosen = inter;
                            }
                        }
                    }
                }
            }

            
            Vector2 Bottom = ReturnIntersectionLine(p1.ToVector2(), p2.ToVector2(), new Vector2(0, BOTTOM), new Vector2(1000000, BOTTOM));

            if (Bottom != Vector2.Zero)
            {
                return Bottom;
            }
            return chosen;
        }

        public static Vector2 ReturnIntersectionLine(Vector2 l1p1, Vector2 l1p2, Vector2 l2p1, Vector2 l2p2)
        {
            float q = (l1p1.Y - l2p1.Y) * (l2p2.X - l2p1.X) - (l1p1.X - l2p1.X) * (l2p2.Y - l2p1.Y);
            float d = (l1p2.X - l1p1.X) * (l2p2.Y - l2p1.Y) - (l1p2.Y - l1p1.Y) * (l2p2.X - l2p1.X);
            if (d == 0)
                return Vector2.Zero;
            float r = q / d;
            q = (l1p1.Y - l2p1.Y) * (l1p2.X - l1p1.X) - (l1p1.X - l2p1.X) * (l1p2.Y - l1p1.Y);
            float s = q / d;
            if (r < 0 || r > 1 || s < 0 || s > 1)
                return Vector2.Zero;
            return Vector2.Lerp(l1p1, l1p2, r);
        }
    }
}
