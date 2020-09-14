
using Flipsider;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Flipsider
{
    public static class DrawMethods
    {
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
    }
}
