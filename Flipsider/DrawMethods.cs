
using Flipsider;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace EEMod
{
    public static class DrawMethods
    {
        static Texture2D pixel = new Texture2D(Main._graphics.GraphicsDevice, 1, 1);
        static Color[] colors = new Color[1];
        public static void DrawPixel(Vector2 pos, Color tint)
        {
            colors[0] = Color.White;
            pixel.SetData(colors);
            Main._spriteBatch.Draw(pixel, pos, tint);
        }
    }
}
