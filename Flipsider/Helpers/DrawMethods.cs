
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

        public static void DrawPixel(Vector2 pos, Color tint) => Main.spriteBatch.Draw(TextureCache.pixel, pos, tint);
        public static void DrawLine(Vector2 p1, Vector2 p2, Color tint, float lineWidth = 1f)
        {
            /*
            float Dist = Vector2.Distance(p1, p2);
            for (float j = 0; j < 1; j += 1 / Dist)
            {
                Vector2 Lerped = p1 + j * (p2 - p1);
                DrawPixel(Lerped, tint);
            }
            */

            Vector2 between = p2 - p1;
            float length = between.Length();
            float rotation = (float)Math.Atan2(between.Y, between.X);
            Main.spriteBatch.Draw(TextureCache.pixel, p1, null, tint, rotation, new Vector2(0f, 0.5f), new Vector2(length, lineWidth), SpriteEffects.None, 0f);
        }

        public static void DrawText(string text, Color colour, Vector2 position,float rotation = 0f)
        {
            SpriteFont font = Main.font;
            Vector2 textSize = font.MeasureString(text);
            float textPositionLeft = position.X - textSize.X / 2;
            Main.spriteBatch.DrawString(font, text, new Vector2(textPositionLeft, position.Y), colour, rotation, Vector2.Zero, 1, SpriteEffects.None, 0f);
        }

        public static void DrawTextToLeft(string text, Color colour, Vector2 position)
        {
            SpriteFont font = Main.font;
            float textPositionLeft = position.X;
            Main.spriteBatch.DrawString(font, text, new Vector2(textPositionLeft, position.Y), colour, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
        }

        public static void DrawSquare(Vector2 point, float size, Color color)
        {
            DrawLine(new Vector2(point.X + size, point.Y + size), new Vector2(point.X, point.Y + size), color);
            DrawLine(new Vector2(point.X + size, point.Y + size), new Vector2(point.X + size, point.Y), color);
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
