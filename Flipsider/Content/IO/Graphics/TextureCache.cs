using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Flipsider
{
    // TODO dude.
#nullable disable
    public class TextureCache
    {
        public static Texture2D SwitchLayer;
        public static Texture2D BrickStructure1;
        public static Texture2D BrickStructure2;
        public static Texture2D MediumTree1;
        public static Texture2D MediumTree2;
        public static Texture2D MediumTree3;
        public static Texture2D MediumTree4;
        public static Texture2D SmallTree1;
        public static Texture2D SmallTree2;
        public static Texture2D BackgroundTree1;
        public static Texture2D BackgroundTree2;
        public static Texture2D BackgroundTree3;
        public static Texture2D BackgroundTree4;
        public static Texture2D ForegroundGrass1;
        public static Texture2D pixel;
        public static Texture2D player;
        public static Texture2D CrowBar;
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
        public static Texture2D ForestDecoThree;
        public static Texture2D ForestDecoFour;
        public static Texture2D ForestDecoFive;
        public static Texture2D ForestDecoSix;

        public static Texture2D ForestDecoBD1;
        public static Texture2D ForestDecoBD2;
        public static Texture2D ForestDecoBD3;
        public static Texture2D ForestDecoBD4;
        public static Texture2D ForestDecoBD5;
        public static Texture2D ForestDecoBD6;
        public static Texture2D ForestDecoBD7;
        public static Texture2D ForestDecoBD8;
        public static Texture2D ForestDecoBD9;
        public static Texture2D ForestDecoBD10;
        public static Texture2D ForestDecoBD11;
        public static Texture2D ForestDecoBD12;
        public static Texture2D ForestDecoBD13;

        public static Texture2D Noise;
        public static Texture2D Noise2;
        public static Texture2D RandomPolkaDots;
        public static Texture2D Spot;
        public static Texture2D Voronoi;
        public static Texture2D WormNoisePixelated;

        public static Texture2D AddLayer;
        public static Texture2D RemoveLayer;

        public static Texture2D ForestForegroundProp1;
        public static Texture2D ForestForegroundProp2;
        public static Texture2D ForestForegroundProp3;
        public static Texture2D ForestForegroundProp4;
        public static Texture2D ForestForegroundProp5;
        public static Texture2D ForestForegroundProp6;
        public static Texture2D ForestForegroundProp7;
        public static Texture2D ForestForegroundProp8;
        public static Texture2D ForestForegroundProp9;
        public static Texture2D ForestForegroundProp10;
        public static Texture2D ForestForegroundProp11;

        public static Texture2D ForestRockOne;
        public static Texture2D ForestRockTwo;
        public static Texture2D ForestRockThree;
        public static Texture2D ForestRockFour;
        public static Texture2D ForestRockFive;
        public static Texture2D ForestRockSix;
        public static Texture2D ForestRockSeven;
        public static Texture2D ForestRockEight;
        public static Texture2D ForestRockNine;
        public static Texture2D ForestRockTen;
        public static Texture2D ForestRockEleven;

        public static Texture2D ForestBigRockOne;
        public static Texture2D ForestBigRockTwo;
        public static Texture2D ForestBigRockThree;
        public static Texture2D Waterfall;

        public static Texture2D WaterShaderLightMap;
        public static void LoadTextures(ContentManager content)
        {
            pixel = new Texture2D(Main.graphics.GraphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });
            WaterShaderLightMap = content.Load<Texture2D>("Textures/Noise/WaterShaderLightMap");

            AddLayer = content.Load<Texture2D>("Textures/GUI/AddLayer");
            RemoveLayer = content.Load<Texture2D>("Textures/GUI/RemoveLayer");

            BrickStructure1 = content.Load<Texture2D>("Textures/Props/BrickStructure1");
            BrickStructure2 = content.Load<Texture2D>("Textures/Props/BrickStructure2");
            Noise = content.Load<Texture2D>("Textures/Noise/Noise");
            Noise2 = content.Load<Texture2D>("Textures/Noise/Noise2");
            RandomPolkaDots = content.Load<Texture2D>("Textures/Noise/RandomPolkaDots");
            Spot = content.Load<Texture2D>("Textures/Noise/Spot");
            Voronoi = content.Load<Texture2D>("Textures/Noise/VoronoiNoise");
            WormNoisePixelated = content.Load<Texture2D>("Textures/Noise/WormNoisePixelated");
            BackgroundTree1 = content.Load<Texture2D>("Textures/Props/BackgroundTree1");
            BackgroundTree2 = content.Load<Texture2D>("Textures/Props/BackgroundTree2");
            BackgroundTree3 = content.Load<Texture2D>("Textures/Props/BackgroundTree3");
            BackgroundTree4 = content.Load<Texture2D>("Textures/Props/BackgroundTree4");

            MediumTree1 = content.Load<Texture2D>("Textures/Props/MediumTree1");
            MediumTree2 = content.Load<Texture2D>("Textures/Props/MediumTree2");
            MediumTree3 = content.Load<Texture2D>("Textures/Props/MediumTree3");
            MediumTree4 = content.Load<Texture2D>("Textures/Props/MediumTree4");

            SmallTree1 = content.Load<Texture2D>("Textures/Props/SmallTree1");
            SmallTree2 = content.Load<Texture2D>("Textures/Props/SmallTree2");
    
            ForegroundGrass1 = content.Load<Texture2D>("Textures/Props/ForegroundGrass1");
            ForestBackground1 = content.Load<Texture2D>("Textures/Backgrounds/ForestBackground1");
            ForestBackground2 = content.Load<Texture2D>("Textures/Backgrounds/ForestBackground2");
            ForestBackground3 = content.Load<Texture2D>("Textures/Backgrounds/ForestBackground3");
            MainMenuPanelOverlay = content.Load<Texture2D>("Textures/GUI/MainMenuPanelOverlay");
            TileGUIPanels = content.Load<Texture2D>("Textures/TGUI");
            LayerHide = content.Load<Texture2D>("Textures/GUI/LayerHide");
            SwitchLayer = content.Load<Texture2D>("Textures/GUI/SwitchLayer");
            TitleScreen = content.Load<Texture2D>("Textures/GUI/TitleScreen");
            TitleScreenOverlay = content.Load<Texture2D>("Textures/GUI/TitleScreenOverlay");
            MainMenuPanel = content.Load<Texture2D>("Textures/GUI/MainMenuPanel");
            TextLine = content.Load<Texture2D>("Textures/GUI/TextLine");
            TileSet2 = content.Load<Texture2D>("Textures/TileSet2");
            TileSet1 = content.Load<Texture2D>("Textures/TileSet1");
            TileSet3 = content.Load<Texture2D>("Textures/TileSet3");
            TileSet4 = content.Load<Texture2D>("Textures/TileSet4");
            player = content.Load<Texture2D>("Textures/char");
            CrowBar = content.Load<Texture2D>("Textures/CrowBar");
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
            ForestDecoThree = content.Load<Texture2D>("Textures/Props/ForestDecoThree");
            ForestDecoFour = content.Load<Texture2D>("Textures/Props/ForestDecoFour");
            ForestDecoFive = content.Load<Texture2D>("Textures/Props/ForestDecoFive");
            ForestDecoSix = content.Load<Texture2D>("Textures/Props/ForestDecoSix");

            ForestDecoBD1 = content.Load<Texture2D>("Textures/Props/ForestDecoBD1");
            ForestDecoBD2 = content.Load<Texture2D>("Textures/Props/ForestDecoBD2");
            ForestDecoBD3 = content.Load<Texture2D>("Textures/Props/ForestDecoBD3");
            ForestDecoBD4 = content.Load<Texture2D>("Textures/Props/ForestDecoBD4");
            ForestDecoBD5 = content.Load<Texture2D>("Textures/Props/ForestDecoBD5");
            ForestDecoBD6 = content.Load<Texture2D>("Textures/Props/ForestDecoBD6");
            ForestDecoBD7 = content.Load<Texture2D>("Textures/Props/ForestDecoBD7");
            ForestDecoBD8 = content.Load<Texture2D>("Textures/Props/ForestDecoBD8");
            ForestDecoBD9 = content.Load<Texture2D>("Textures/Props/ForestDecoBD9");
            ForestDecoBD10 = content.Load<Texture2D>("Textures/Props/ForestDecoBD10");
            ForestDecoBD11 = content.Load<Texture2D>("Textures/Props/ForestDecoBD11");
            ForestDecoBD12 = content.Load<Texture2D>("Textures/Props/ForestDecoBD12");
            ForestDecoBD13 = content.Load<Texture2D>("Textures/Props/ForestDecoBD13");

            ForestForegroundProp1 = content.Load<Texture2D>("Textures/Props/ForegroundProp1");
            ForestForegroundProp2 = content.Load<Texture2D>("Textures/Props/ForegroundProp2");
            ForestForegroundProp3 = content.Load<Texture2D>("Textures/Props/ForegroundProp3");
            ForestForegroundProp4 = content.Load<Texture2D>("Textures/Props/ForegroundProp4");
            ForestForegroundProp5 = content.Load<Texture2D>("Textures/Props/ForegroundProp5");
            ForestForegroundProp6 = content.Load<Texture2D>("Textures/Props/ForegroundProp6");
            ForestForegroundProp7 = content.Load<Texture2D>("Textures/Props/ForegroundProp7");
            ForestForegroundProp8 = content.Load<Texture2D>("Textures/Props/ForegroundProp8");

            ForestRockOne = content.Load<Texture2D>("Textures/Props/ForestRockOne");
            ForestRockTwo = content.Load<Texture2D>("Textures/Props/ForestRockTwo");
            ForestRockThree = content.Load<Texture2D>("Textures/Props/ForestRockThree");
            ForestRockFour = content.Load<Texture2D>("Textures/Props/ForestRockFour");
            ForestRockFive = content.Load<Texture2D>("Textures/Props/ForestRockFive");
            ForestRockSix = content.Load<Texture2D>("Textures/Props/ForestRockSix");
            ForestRockSeven = content.Load<Texture2D>("Textures/Props/ForestRockSeven");
            ForestRockEight = content.Load<Texture2D>("Textures/Props/ForestRockEight");
            ForestRockNine = content.Load<Texture2D>("Textures/Props/ForestRockNine");
            ForestRockTen = content.Load<Texture2D>("Textures/Props/ForestRockTen");
            ForestRockEleven = content.Load<Texture2D>("Textures/Props/ForestRockEleven");

            ForestBigRockOne = content.Load<Texture2D>("Textures/Props/ForestBigRockOne");
            ForestBigRockTwo = content.Load<Texture2D>("Textures/Props/ForestBigRockTwo");
            ForestBigRockThree = content.Load<Texture2D>("Textures/Props/ForestBigRockThree");
            Waterfall = content.Load<Texture2D>("Textures/Props/Waterfall");
            //FrontBicep = content.Load<Texture2D>("BodyParts/FrontBicep");
            //FrontForearm = content.Load<Texture2D>("BodyParts/FrontForearm");
            //BackBicep = content.Load<Texture2D>("BodyParts/BackBicep");
            //BackForearm = content.Load<Texture2D>("BodyParts/BackForearm");
        }
    }
}
