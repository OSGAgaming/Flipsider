﻿using Flipsider.Assets;
using Flipsider.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flipsider.Extensions
{
    public static class GraphicExtension
    {
        public static Vector2 Size(this Texture2D texture) => texture.Bounds.Size.ToVector2();
        
        public static Vector2 UseParallax(Vector2 position, float zDistance)
        {
            if (zDistance <= 0)
                throw new ArgumentOutOfRangeException(nameof(zDistance));

            return position / zDistance;
        }

        public static Vector2 GetParallaxOffset(Vector2 center, float distance) => (FlipsiderGame.GameInstance.CurrentCamera.Translation2D - center) * distance;

        public static void DrawPixel(this SpriteBatch sb, Vector2 pos, Color tint) => sb.Draw(Asset<Texture2D>.From("Pixel"), pos, tint);

        public static void DrawLine(this SpriteBatch sb, Vector2 p1, Vector2 p2, Color tint, float lineWidth = 1f)
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
            sb.Draw(Asset<Texture2D>.From("Pixel"), p1, null, tint, rotation, new Vector2(0f, 0.5f), new Vector2(length, lineWidth), SpriteEffects.None, 0f);
        }

        public static void DrawText(this SpriteBatch sb, string text, Color colour, Vector2 position)
        {
            SpriteFont font = Asset<SpriteFont>.From("FlipFont");
            Vector2 textSize = font.MeasureString(text);
            float textPositionLeft = position.X - textSize.X / 2;
            sb.DrawString(font, text, new Vector2(textPositionLeft, position.Y), colour, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
        }

        public static void DrawTextToLeft(this SpriteBatch sb, string text, Color colour, Vector2 position)
        {
            SpriteFont font = Asset<SpriteFont>.From("FlipFont");
            float textPositionLeft = position.X;
            sb.DrawString(font, text, new Vector2(textPositionLeft, position.Y), colour, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
        }

        public static void DrawSquare(this SpriteBatch sb, Vector2 point, float size, Color color)
        {
            sb.DrawLine(new Vector2(point.X + size, point.Y + size), new Vector2(point.X, point.Y + size), color);
            sb.DrawLine(new Vector2(point.X + size, point.Y + size), new Vector2(point.X + size, point.Y), color);
            sb.DrawLine(point, new Vector2(point.X, point.Y + size), color);
            sb.DrawLine(point, new Vector2(point.X + size, point.Y), color);
        }

        public static void DrawRectangle(this SpriteBatch sb, Vector2 point, float sizeX, float sizeY, Color color)
        {
            sb.DrawLine(new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X, point.Y + sizeY), color);
            sb.DrawLine(new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X + sizeX, point.Y), color);
            sb.DrawLine(point, new Vector2(point.X, point.Y + sizeY), color);
            sb.DrawLine(point, new Vector2(point.X + sizeX, point.Y), color);
        }
    }
}
