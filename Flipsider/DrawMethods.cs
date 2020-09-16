
using Flipsider;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Flipsider
{
    public static class DrawMethods
    {
        public static Vector2 GetParallaxOffset(Vector2 center, float strenght) => (Main.mainCamera.CamPos - center) * strenght;

        public static void DrawPixel(Vector2 pos, Color tint) => Main.spriteBatch.Draw(Main.pixel, pos, tint);
        public static void DrawLine(Vector2 p1, Vector2 p2, Color tint)
        {
            float Dist = Vector2.Distance(p1, p2);
            for (float j = 0; j < 1; j += 1 / Dist)
            {
                Vector2 Lerped = p1 + j * (p2 - p1);
                DrawPixel(Lerped, tint);
            }
        }

        public static void DrawSquare(Vector2 point, float size, Color color)
        {
            DrawLine(new Vector2(point.X + size, point.Y + size), new Vector2(point.X, point.Y + size), color);
            DrawLine(new Vector2(point.X + size, point.Y + size), new Vector2(point.X + size, point.Y ), color);
            DrawLine(point, new Vector2(point.X, point.Y + size), color);
            DrawLine(point, new Vector2(point.X + size, point.Y), color);
        }

        public static void DrawRectangle(Vector2 point, float sizeX, float sizeY, Color color)
        {
            DrawLine(new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X, point.Y + sizeY), color);
            DrawLine(new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X + sizeX, point.Y), color);
            DrawLine(point, new Vector2(point.X, point.Y + sizeY), color);
            DrawLine(point, new Vector2(point.X + sizeX, point.Y), color);
        }
    }
}
