
using Flipsider;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace Flipsider
{
    public class TextureCache
    {
        public static Texture2D player;

        public static void LoadTextures()
        {
            player = Main.Contents.Load<Texture2D>("char");
        }
    }
}
