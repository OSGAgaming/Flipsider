
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
        public static Texture2D PointLight;

        public static Texture2D BusStop;
        public static Texture2D BigBusStop;
        public static Texture2D TrafficLight;
        public static Texture2D BikeRack;
        public static Texture2D StopSigns;
        public static Texture2D StreetLights;
        public static Texture2D Birb;
        public static Texture2D FrontBicep;
        public static Texture2D FrontForearm;
        public static Texture2D BackBicep;
        public static Texture2D BackForearm;
        public static void LoadTextures(ContentManager content)
        {
            pixel = new Texture2D(Main.graphics.GraphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });
            TileGUIPanels = content.Load<Texture2D>("Textures/TGUI");
            TileSet2 = content.Load<Texture2D>("Textures/TileSet2");
            TileSet1 = content.Load<Texture2D>("Textures/TileSet1");
            TileSet3 = content.Load<Texture2D>("Textures/TileSet3");
            player = content.Load<Texture2D>("Textures/char");
            hudSlot = content.Load<Texture2D>("Textures/GUI/HudSlot");
            testGun = content.Load<Texture2D>("Textures/GUI/TestGun");
            magicPixel = content.Load<Texture2D>("Textures/GUI/MagicPixel");
            skybox = content.Load<Texture2D>("Textures/skybox");
            Blob = content.Load<Texture2D>("Textures/Blob");
            NPCPanel = content.Load<Texture2D>("Textures/NPCPanel");
            Textbox = content.Load<Texture2D>("Textures/Textbox");
            SaveTex = content.Load<Texture2D>("Textures/SaveTex");
            WorldSavePanel = content.Load<Texture2D>("Textures/GUI/WorldSavePanel");
            GreenSlime = content.Load<Texture2D>("Textures/GreenSlime");
            PointLight = content.Load<Texture2D>("Textures/PointLight");
            Birb = content.Load<Texture2D>("Textures/Birb");
            BusStop = content.Load<Texture2D>("Textures/Props/BusStop");
            BigBusStop = content.Load<Texture2D>("Textures/Props/BigBusStop");
            TrafficLight = content.Load<Texture2D>("Textures/Props/TrafficLight");
            StopSigns = content.Load<Texture2D>("Textures/Props/StopSigns");
            BikeRack = content.Load<Texture2D>("Textures/Props/BikeRack");
            StreetLights = content.Load<Texture2D>("Textures/Props/StreetLights");
            //FrontBicep = content.Load<Texture2D>("BodyParts/FrontBicep");
            //FrontForearm = content.Load<Texture2D>("BodyParts/FrontForearm");
            //BackBicep = content.Load<Texture2D>("BodyParts/BackBicep");
            //BackForearm = content.Load<Texture2D>("BodyParts/BackForearm");
        }
    }
}
