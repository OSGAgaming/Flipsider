
using Flipsider;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace Flipsider
{
    // TODO dude.
#nullable disable
    public class TextureCache
    {
        public static Texture2D pixel;
        public static Texture2D player;
        public static Texture2D hudSlot;
        public static Texture2D testGun;
        public static Texture2D magicPixel;
        public static Texture2D skybox;
        public static Texture2D TileSet1;
        public static Texture2D TileSet2;
        public static Texture2D TileSet3;
        public static Texture2D TileGUIPanels;
        public static Texture2D Blob;
        public static Texture2D GreenSlime;
        public static Texture2D NPCPanel;
        public static Texture2D Textbox;
        public static Texture2D SaveTex;
        public static Texture2D WorldSavePanel;
        public static void LoadTextures(ContentManager content)
        {
            pixel = new Texture2D(Main.graphics.GraphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });
            TileGUIPanels = content.Load<Texture2D>("TGUI");
            TileSet2 = content.Load<Texture2D>("TileSet2");
            TileSet1 = content.Load<Texture2D>("TileSet1");
            TileSet3 = content.Load<Texture2D>("TileSet3");
            player = content.Load<Texture2D>("char");
            hudSlot = content.Load<Texture2D>("GUI/HudSlot");
            testGun = content.Load<Texture2D>("GUI/TestGun");
            magicPixel = content.Load<Texture2D>("GUI/MagicPixel");
            skybox = content.Load<Texture2D>("skybox");
            Blob = content.Load<Texture2D>("Blob");
            NPCPanel = content.Load<Texture2D>("NPCPanel");
            Textbox = content.Load<Texture2D>("Textbox");
            SaveTex = content.Load<Texture2D>("SaveTex");
            WorldSavePanel = content.Load<Texture2D>("GUI/WorldSavePanel");
            GreenSlime = content.Load<Texture2D>("GreenSlime");
        }
    }
}
