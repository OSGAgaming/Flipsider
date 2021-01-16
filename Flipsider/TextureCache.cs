
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
        public static Texture2D SkyboxFront;
        public static Texture2D TileSet1;
        public static Texture2D TileSet2;
        public static Texture2D TileSet3;
        public static Texture2D TileSet4;
        public static Texture2D TileGUIPanels;
        public static Texture2D Blob;
        public static Texture2D GreenSlime;
        public static Texture2D NPCPanel;
        public static Texture2D Textbox;
        public static Texture2D SaveTex;
        public static Texture2D WorldSavePanel;
        public static Texture2D PointLight;
        public static Texture2D ForestTree1;
        public static Texture2D ForestTree2;
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
        public static Texture2D LayerHide;
        public static Texture2D TextLine;
        public static Texture2D ForestBackground1;
        public static Texture2D ForestBackground2;
        public static Texture2D ForestBackground3;
        public static Texture2D WhiteScreen;
        public static Texture2D TitleScreen;
        public static Texture2D TitleScreenOverlay;
        public static Texture2D MainMenuPanel;
        public static Texture2D MainMenuPanelOverlay;
        public static Texture2D ForestFlowerOne;
        public static Texture2D ForestFlowerTwo;
        public static Texture2D ForestFlowerThree;
        public static Texture2D ForestFlowerFour;
        public static Texture2D ForestFlowerFive;
        public static Texture2D ForestFlowerSix;

        public static Texture2D ForestGrassOne;
        public static Texture2D ForestGrassTwo;
        public static Texture2D ForestGrassThree;
        public static Texture2D ForestGrassFour;
        public static Texture2D ForestGrassFive;
        public static Texture2D ForestGrassSix;
        public static Texture2D ForestGrassSeven;
        public static Texture2D ForestGrassEight;
        public static Texture2D ForestGrassNine;

        public static Texture2D ForestBushOne;
        public static Texture2D ForestLogOne;
        public static Texture2D ForestDecoOne;
        public static Texture2D ForestDecoTwo;

        public static Texture2D Noise;
        public static Texture2D Noise2;
        public static Texture2D RandomPolkaDots;
        public static Texture2D Spot;
        public static Texture2D Voronoi;
        public static Texture2D WormNoisePixelated;

        public static void LoadTextures(ContentManager content)
        {
            pixel = new Texture2D(Main.graphics.GraphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });

            Noise = content.Load<Texture2D>("Textures/Noise/Noise");
            Noise2 = content.Load<Texture2D>("Textures/Noise/Noise2");
            RandomPolkaDots = content.Load<Texture2D>("Textures/Noise/RandomPolkaDots");
            Spot = content.Load<Texture2D>("Textures/Noise/Spot");
            Voronoi = content.Load<Texture2D>("Textures/Noise/VoronoiNoise");
            WormNoisePixelated = content.Load<Texture2D>("Textures/Noise/WormNoisePixelated");

            ForestBackground1 = content.Load<Texture2D>("Textures/Backgrounds/ForestBackground1");
            ForestBackground2 = content.Load<Texture2D>("Textures/Backgrounds/ForestBackground2");
            ForestBackground3 = content.Load<Texture2D>("Textures/Backgrounds/ForestBackground3");
            MainMenuPanelOverlay = content.Load<Texture2D>("Textures/GUI/MainMenuPanelOverlay");
            TileGUIPanels = content.Load<Texture2D>("Textures/TGUI");
            LayerHide = content.Load<Texture2D>("Textures/GUI/LayerHide");
            TitleScreen = content.Load<Texture2D>("Textures/GUI/TitleScreen");
            TitleScreenOverlay = content.Load<Texture2D>("Textures/GUI/TitleScreenOverlay");
            MainMenuPanel = content.Load<Texture2D>("Textures/GUI/MainMenuPanel");
            TextLine = content.Load<Texture2D>("Textures/GUI/TextLine");
            TileSet2 = content.Load<Texture2D>("Textures/TileSet2");
            TileSet1 = content.Load<Texture2D>("Textures/TileSet1");
            TileSet3 = content.Load<Texture2D>("Textures/TileSet3");
            TileSet4 = content.Load<Texture2D>("Textures/TileSet4");
            player = content.Load<Texture2D>("Textures/char");
            hudSlot = content.Load<Texture2D>("Textures/GUI/HudSlot");
            WhiteScreen = content.Load<Texture2D>("Textures/GUI/WhiteScreen");
            testGun = content.Load<Texture2D>("Textures/GUI/TestGun");
            magicPixel = content.Load<Texture2D>("Textures/GUI/MagicPixel");
            skybox = content.Load<Texture2D>("Textures/Backgrounds/Skybox");
            SkyboxFront = content.Load<Texture2D>("Textures/Backgrounds/SkyboxFront");
            Blob = content.Load<Texture2D>("Textures/Blob");
            NPCPanel = content.Load<Texture2D>("Textures/NPCPanel");
            Textbox = content.Load<Texture2D>("Textures/Textbox");
            SaveTex = content.Load<Texture2D>("Textures/SaveTex");
            WorldSavePanel = content.Load<Texture2D>("Textures/GUI/WorldSavePanel");
            GreenSlime = content.Load<Texture2D>("Textures/GreenSlime");
            PointLight = content.Load<Texture2D>("Textures/PointLight");
            Birb = content.Load<Texture2D>("Textures/Birb");
            BusStop = content.Load<Texture2D>("Textures/Props/BusStop");
            ForestTree1 = content.Load<Texture2D>("Textures/Props/ForestTree1");
            ForestTree2 = content.Load<Texture2D>("Textures/Props/ForestTree2");
            BigBusStop = content.Load<Texture2D>("Textures/Props/BigBusStop");
            TrafficLight = content.Load<Texture2D>("Textures/Props/TrafficLight");
            StopSigns = content.Load<Texture2D>("Textures/Props/StopSigns");
            BikeRack = content.Load<Texture2D>("Textures/Props/BikeRack");
            StreetLights = content.Load<Texture2D>("Textures/Props/StreetLights");

            ForestFlowerOne = content.Load<Texture2D>("Textures/Props/ForestFlowerOne");
            ForestFlowerTwo = content.Load<Texture2D>("Textures/Props/ForestFlowerTwo");
            ForestFlowerThree = content.Load<Texture2D>("Textures/Props/ForestFlowerThree");
            ForestFlowerFour = content.Load<Texture2D>("Textures/Props/ForestFlowerFour");
            ForestFlowerFive = content.Load<Texture2D>("Textures/Props/ForestFlowerFive");
            ForestFlowerSix = content.Load<Texture2D>("Textures/Props/ForestFlowerSix");

            ForestGrassOne = content.Load<Texture2D>("Textures/Props/ForestGrassOne");
            ForestGrassTwo = content.Load<Texture2D>("Textures/Props/ForestGrassTwo");
            ForestGrassThree = content.Load<Texture2D>("Textures/Props/ForestGrassThree");
            ForestGrassFour = content.Load<Texture2D>("Textures/Props/ForestGrassFour");
            ForestGrassFive = content.Load<Texture2D>("Textures/Props/ForestGrassFive");
            ForestGrassSix = content.Load<Texture2D>("Textures/Props/ForestGrassSix");
            ForestGrassSeven = content.Load<Texture2D>("Textures/Props/ForestGrassSeven");
            ForestGrassEight = content.Load<Texture2D>("Textures/Props/ForestGrassEight");
            ForestGrassNine = content.Load<Texture2D>("Textures/Props/ForestGrassNine");

            ForestBushOne = content.Load<Texture2D>("Textures/Props/ForestBushOne");
            ForestLogOne = content.Load<Texture2D>("Textures/Props/ForestLogOne");
            ForestDecoOne = content.Load<Texture2D>("Textures/Props/ForestDecoOne");
            ForestDecoTwo = content.Load<Texture2D>("Textures/Props/ForestDecoTwo");

            //FrontBicep = content.Load<Texture2D>("BodyParts/FrontBicep");
            //FrontForearm = content.Load<Texture2D>("BodyParts/FrontForearm");
            //BackBicep = content.Load<Texture2D>("BodyParts/BackBicep");
            //BackForearm = content.Load<Texture2D>("BodyParts/BackForearm");
        }
    }
}
