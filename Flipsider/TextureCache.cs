
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
        public static Texture2D hudSlot;
        public static Texture2D testGun;
        public static Texture2D magicPixel;

        public static void LoadTextures()
        {
            player = Main.Contents.Load<Texture2D>("char");
            hudSlot = Main.Contents.Load<Texture2D>("GUI/HudSlot");
            testGun = Main.Contents.Load<Texture2D>("GUI/TestGun");
            magicPixel = Main.Contents.Load<Texture2D>("GUI/MagicPixel");
        }
    }
}
