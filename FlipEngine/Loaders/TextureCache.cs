using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.IO;

namespace FlipEngine
{
    // TODO dude.
#nullable disable
    public class TextureCache
    {
        public static Texture2D SwitchLayer;
        public static Texture2D pixel;
        public static Texture2D magicPixel;
        public static Texture2D TileGUIPanels;
        public static Texture2D NPCPanel;
        public static Texture2D Textbox;
        public static Texture2D SaveTex;
        public static Texture2D PointLight;
        public static Texture2D LayerHide;

        public static Texture2D Noise;
        public static Texture2D Noise2;
        public static Texture2D RandomPolkaDots;
        public static Texture2D Spot;
        public static Texture2D Voronoi;
        public static Texture2D WormNoisePixelated;

        public static Texture2D AddLayer;
        public static Texture2D RemoveLayer;

        public static void LoadTextures(ContentManager content)
        {

            pixel = new Texture2D(Main.graphics.GraphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });

            AddLayer = content.Load<Texture2D>("Textures/GUI/AddLayer");
            RemoveLayer = content.Load<Texture2D>("Textures/GUI/RemoveLayer");
            Noise = content.Load<Texture2D>("Textures/Noise/Noise");
            Noise2 = content.Load<Texture2D>("Textures/Noise/Noise2");
            RandomPolkaDots = content.Load<Texture2D>("Textures/Noise/RandomPolkaDots");
            Spot = content.Load<Texture2D>("Textures/Noise/Spot");
            Voronoi = content.Load<Texture2D>("Textures/Noise/VoronoiNoise");
            WormNoisePixelated = content.Load<Texture2D>("Textures/Noise/WormNoisePixelated");
            TileGUIPanels = content.Load<Texture2D>("Textures/TGUI");
            LayerHide = content.Load<Texture2D>("Textures/GUI/LayerHide");
            SwitchLayer = content.Load<Texture2D>("Textures/GUI/SwitchLayer");


            magicPixel = content.Load<Texture2D>("Textures/GUI/MagicPixel");
            NPCPanel = content.Load<Texture2D>("Textures/NPCPanel");
            Textbox = content.Load<Texture2D>("Textures/Textbox");
            SaveTex = content.Load<Texture2D>("Textures/SaveTex");
            PointLight = content.Load<Texture2D>("Textures/PointLight");
        }
    }
}
