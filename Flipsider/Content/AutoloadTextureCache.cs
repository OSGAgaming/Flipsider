using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
namespace Flipsider
{
 public static class Textures
 {
#nullable disable
 public static Texture2D _Birb;
 public static Texture2D _Blob;
 public static Texture2D _char;
 public static Texture2D _Foam1;
 public static Texture2D _Foam2;
 public static Texture2D _Foam3;
 public static Texture2D _GF;
 public static Texture2D _GreenSlime;
 public static Texture2D _NPCPanel;
 public static Texture2D _PointLight;
 public static Texture2D _SaveTex;
 public static Texture2D _SpikyOrbLightMap;
 public static Texture2D _StreetLights;
 public static Texture2D _Textbox;
 public static Texture2D _TGUI;
 public static Texture2D _TileGUIPanels;
 public static Texture2D _TileSet1;
 public static Texture2D _TileSet2;
 public static Texture2D _TileSet3;
 public static Texture2D _TileSet4;
 public static Texture2D _Backgrounds_ForestBackground1;
 public static Texture2D _Backgrounds_ForestBackground2;
 public static Texture2D _Backgrounds_ForestBackground3;
 public static Texture2D _Backgrounds_Skybox;
 public static Texture2D _Backgrounds_SkyboxFront;
 public static Texture2D _BodyParts_BackBicep;
 public static Texture2D _BodyParts_BackForearm;
 public static Texture2D _BodyParts_FrontBicep;
 public static Texture2D _BodyParts_FrontForearm;
 public static Texture2D _GUI_AddLayer;
 public static Texture2D _GUI_HudSlot;
 public static Texture2D _GUI_LayerHide;
 public static Texture2D _GUI_MagicPixel;
 public static Texture2D _GUI_MainMenuPanel;
 public static Texture2D _GUI_MainMenuPanelOverlay;
 public static Texture2D _GUI_RemoveLayer;
 public static Texture2D _GUI_SaveTex;
 public static Texture2D _GUI_SwitchLayer;
 public static Texture2D _GUI_TestGun;
 public static Texture2D _GUI_TextLine;
 public static Texture2D _GUI_TitleScreen;
 public static Texture2D _GUI_TitleScreenOverlay;
 public static Texture2D _GUI_WhiteScreen;
 public static Texture2D _GUI_WorldSavePanel;
 public static Texture2D _GUI_HealthAssets_HealthExtra;
 public static Texture2D _GUI_HealthAssets_HealthRight;
 public static Texture2D _GUI_HealthAssets_LeftHealthPoint;
 public static Texture2D _GUI_HealthAssets_ManaRight;
 public static Texture2D _GUI_HealthAssets_RightHealthPoint;
 public static Texture2D _GUI_HealthAssets_WeaponPanel;
 public static Texture2D _Noise_Noise;
 public static Texture2D _Noise_Noise2;
 public static Texture2D _Noise_RandomPolkaDots;
 public static Texture2D _Noise_Spot;
 public static Texture2D _Noise_VoronoiNoise;
 public static Texture2D _Noise_WaterShaderLightMap;
 public static Texture2D _Noise_WormNoisePixelated;
 public static Texture2D _Props_BackgroundTree1;
 public static Texture2D _Props_BackgroundTree2;
 public static Texture2D _Props_BackgroundTree3;
 public static Texture2D _Props_BackgroundTree4;
 public static Texture2D _Props_BigBusStop;
 public static Texture2D _Props_BikeRack;
 public static Texture2D _Props_BrickStructure1;
 public static Texture2D _Props_BrickStructure2;
 public static Texture2D _Props_BusStop;
 public static Texture2D _Props_EnergyRocc;
 public static Texture2D _Props_EnergyRoccGlow;
 public static Texture2D _Props_ForegroundGrass1;
 public static Texture2D _Props_ForegroundProp1;
 public static Texture2D _Props_ForegroundProp10;
 public static Texture2D _Props_ForegroundProp11;
 public static Texture2D _Props_ForegroundProp2;
 public static Texture2D _Props_ForegroundProp3;
 public static Texture2D _Props_ForegroundProp4;
 public static Texture2D _Props_ForegroundProp5;
 public static Texture2D _Props_ForegroundProp6;
 public static Texture2D _Props_ForegroundProp7;
 public static Texture2D _Props_ForegroundProp8;
 public static Texture2D _Props_ForegroundProp9;
 public static Texture2D _Props_ForestBigRockOne;
 public static Texture2D _Props_ForestBigRockThree;
 public static Texture2D _Props_ForestBigRockTwo;
 public static Texture2D _Props_ForestBushOne;
 public static Texture2D _Props_ForestDecoBD1;
 public static Texture2D _Props_ForestDecoBD10;
 public static Texture2D _Props_ForestDecoBD11;
 public static Texture2D _Props_forestDecoBD12;
 public static Texture2D _Props_ForestDecoBD13;
 public static Texture2D _Props_ForestDecoBD2;
 public static Texture2D _Props_ForestDecoBD3;
 public static Texture2D _Props_ForestDecoBD4;
 public static Texture2D _Props_ForestDecoBD5;
 public static Texture2D _Props_ForestDecoBD6;
 public static Texture2D _Props_ForestDecoBD7;
 public static Texture2D _Props_ForestDecoBD8;
 public static Texture2D _Props_ForestDecoBD9;
 public static Texture2D _Props_ForestDecoFive;
 public static Texture2D _Props_ForestDecoFour;
 public static Texture2D _Props_ForestDecoOne;
 public static Texture2D _Props_ForestDecoSix;
 public static Texture2D _Props_ForestDecoThree;
 public static Texture2D _Props_ForestDecoTwo;
 public static Texture2D _Props_ForestFlowerFive;
 public static Texture2D _Props_ForestFlowerFour;
 public static Texture2D _Props_ForestFlowerOne;
 public static Texture2D _Props_ForestFlowerSix;
 public static Texture2D _Props_ForestFlowerThree;
 public static Texture2D _Props_ForestFlowerTwo;
 public static Texture2D _Props_ForestGrassEight;
 public static Texture2D _Props_ForestGrassFive;
 public static Texture2D _Props_ForestGrassFour;
 public static Texture2D _Props_ForestGrassNine;
 public static Texture2D _Props_ForestGrassOne;
 public static Texture2D _Props_ForestGrassSeven;
 public static Texture2D _Props_ForestGrassSix;
 public static Texture2D _Props_ForestGrassThree;
 public static Texture2D _Props_ForestGrassTwo;
 public static Texture2D _Props_ForestLogOne;
 public static Texture2D _Props_ForestRockEight;
 public static Texture2D _Props_ForestRockEleven;
 public static Texture2D _Props_ForestRockFive;
 public static Texture2D _Props_ForestRockFour;
 public static Texture2D _Props_ForestRockNine;
 public static Texture2D _Props_ForestRockOne;
 public static Texture2D _Props_ForestRockSeven;
 public static Texture2D _Props_ForestRockSix;
 public static Texture2D _Props_ForestRockTen;
 public static Texture2D _Props_ForestRockThree;
 public static Texture2D _Props_ForestRockTwo;
 public static Texture2D _Props_ForestTree1;
 public static Texture2D _Props_ForestTree2;
 public static Texture2D _Props_MediumTree1;
 public static Texture2D _Props_MediumTree2;
 public static Texture2D _Props_MediumTree3;
 public static Texture2D _Props_MediumTree4;
 public static Texture2D _Props_SmallTree1;
 public static Texture2D _Props_SmallTree2;
 public static Texture2D _Props_StopSigns;
 public static Texture2D _Props_StreetLights;
 public static Texture2D _Props_TrafficLight;
 public static Texture2D _Props_Waterfall;
 public static Texture2D _Weapons_Crowbar;
 public static void LoadTextures()
 {
   _Birb = AutoloadTextures.Assets[@"Birb"];
   _Blob = AutoloadTextures.Assets[@"Blob"];
   _char = AutoloadTextures.Assets[@"char"];
   _Foam1 = AutoloadTextures.Assets[@"Foam1"];
   _Foam2 = AutoloadTextures.Assets[@"Foam2"];
   _Foam3 = AutoloadTextures.Assets[@"Foam3"];
   _GF = AutoloadTextures.Assets[@"GF"];
   _GreenSlime = AutoloadTextures.Assets[@"GreenSlime"];
   _NPCPanel = AutoloadTextures.Assets[@"NPCPanel"];
   _PointLight = AutoloadTextures.Assets[@"PointLight"];
   _SaveTex = AutoloadTextures.Assets[@"SaveTex"];
   _SpikyOrbLightMap = AutoloadTextures.Assets[@"SpikyOrbLightMap"];
   _StreetLights = AutoloadTextures.Assets[@"StreetLights"];
   _Textbox = AutoloadTextures.Assets[@"Textbox"];
   _TGUI = AutoloadTextures.Assets[@"TGUI"];
   _TileGUIPanels = AutoloadTextures.Assets[@"TileGUIPanels"];
   _TileSet1 = AutoloadTextures.Assets[@"TileSet1"];
   _TileSet2 = AutoloadTextures.Assets[@"TileSet2"];
   _TileSet3 = AutoloadTextures.Assets[@"TileSet3"];
   _TileSet4 = AutoloadTextures.Assets[@"TileSet4"];
   _Backgrounds_ForestBackground1 = AutoloadTextures.Assets[@"Backgrounds\ForestBackground1"];
   _Backgrounds_ForestBackground2 = AutoloadTextures.Assets[@"Backgrounds\ForestBackground2"];
   _Backgrounds_ForestBackground3 = AutoloadTextures.Assets[@"Backgrounds\ForestBackground3"];
   _Backgrounds_Skybox = AutoloadTextures.Assets[@"Backgrounds\Skybox"];
   _Backgrounds_SkyboxFront = AutoloadTextures.Assets[@"Backgrounds\SkyboxFront"];
   _BodyParts_BackBicep = AutoloadTextures.Assets[@"BodyParts\BackBicep"];
   _BodyParts_BackForearm = AutoloadTextures.Assets[@"BodyParts\BackForearm"];
   _BodyParts_FrontBicep = AutoloadTextures.Assets[@"BodyParts\FrontBicep"];
   _BodyParts_FrontForearm = AutoloadTextures.Assets[@"BodyParts\FrontForearm"];
   _GUI_AddLayer = AutoloadTextures.Assets[@"GUI\AddLayer"];
   _GUI_HudSlot = AutoloadTextures.Assets[@"GUI\HudSlot"];
   _GUI_LayerHide = AutoloadTextures.Assets[@"GUI\LayerHide"];
   _GUI_MagicPixel = AutoloadTextures.Assets[@"GUI\MagicPixel"];
   _GUI_MainMenuPanel = AutoloadTextures.Assets[@"GUI\MainMenuPanel"];
   _GUI_MainMenuPanelOverlay = AutoloadTextures.Assets[@"GUI\MainMenuPanelOverlay"];
   _GUI_RemoveLayer = AutoloadTextures.Assets[@"GUI\RemoveLayer"];
   _GUI_SaveTex = AutoloadTextures.Assets[@"GUI\SaveTex"];
   _GUI_SwitchLayer = AutoloadTextures.Assets[@"GUI\SwitchLayer"];
   _GUI_TestGun = AutoloadTextures.Assets[@"GUI\TestGun"];
   _GUI_TextLine = AutoloadTextures.Assets[@"GUI\TextLine"];
   _GUI_TitleScreen = AutoloadTextures.Assets[@"GUI\TitleScreen"];
   _GUI_TitleScreenOverlay = AutoloadTextures.Assets[@"GUI\TitleScreenOverlay"];
   _GUI_WhiteScreen = AutoloadTextures.Assets[@"GUI\WhiteScreen"];
   _GUI_WorldSavePanel = AutoloadTextures.Assets[@"GUI\WorldSavePanel"];
   _GUI_HealthAssets_HealthExtra = AutoloadTextures.Assets[@"GUI\HealthAssets\HealthExtra"];
   _GUI_HealthAssets_HealthRight = AutoloadTextures.Assets[@"GUI\HealthAssets\HealthRight"];
   _GUI_HealthAssets_LeftHealthPoint = AutoloadTextures.Assets[@"GUI\HealthAssets\LeftHealthPoint"];
   _GUI_HealthAssets_ManaRight = AutoloadTextures.Assets[@"GUI\HealthAssets\ManaRight"];
   _GUI_HealthAssets_RightHealthPoint = AutoloadTextures.Assets[@"GUI\HealthAssets\RightHealthPoint"];
   _GUI_HealthAssets_WeaponPanel = AutoloadTextures.Assets[@"GUI\HealthAssets\WeaponPanel"];
   _Noise_Noise = AutoloadTextures.Assets[@"Noise\Noise"];
   _Noise_Noise2 = AutoloadTextures.Assets[@"Noise\Noise2"];
   _Noise_RandomPolkaDots = AutoloadTextures.Assets[@"Noise\RandomPolkaDots"];
   _Noise_Spot = AutoloadTextures.Assets[@"Noise\Spot"];
   _Noise_VoronoiNoise = AutoloadTextures.Assets[@"Noise\VoronoiNoise"];
   _Noise_WaterShaderLightMap = AutoloadTextures.Assets[@"Noise\WaterShaderLightMap"];
   _Noise_WormNoisePixelated = AutoloadTextures.Assets[@"Noise\WormNoisePixelated"];
   _Props_BackgroundTree1 = AutoloadTextures.Assets[@"Props\BackgroundTree1"];
   _Props_BackgroundTree2 = AutoloadTextures.Assets[@"Props\BackgroundTree2"];
   _Props_BackgroundTree3 = AutoloadTextures.Assets[@"Props\BackgroundTree3"];
   _Props_BackgroundTree4 = AutoloadTextures.Assets[@"Props\BackgroundTree4"];
   _Props_BigBusStop = AutoloadTextures.Assets[@"Props\BigBusStop"];
   _Props_BikeRack = AutoloadTextures.Assets[@"Props\BikeRack"];
   _Props_BrickStructure1 = AutoloadTextures.Assets[@"Props\BrickStructure1"];
   _Props_BrickStructure2 = AutoloadTextures.Assets[@"Props\BrickStructure2"];
   _Props_BusStop = AutoloadTextures.Assets[@"Props\BusStop"];
   _Props_EnergyRocc = AutoloadTextures.Assets[@"Props\EnergyRocc"];
   _Props_EnergyRoccGlow = AutoloadTextures.Assets[@"Props\EnergyRoccGlow"];
   _Props_ForegroundGrass1 = AutoloadTextures.Assets[@"Props\ForegroundGrass1"];
   _Props_ForegroundProp1 = AutoloadTextures.Assets[@"Props\ForegroundProp1"];
   _Props_ForegroundProp10 = AutoloadTextures.Assets[@"Props\ForegroundProp10"];
   _Props_ForegroundProp11 = AutoloadTextures.Assets[@"Props\ForegroundProp11"];
   _Props_ForegroundProp2 = AutoloadTextures.Assets[@"Props\ForegroundProp2"];
   _Props_ForegroundProp3 = AutoloadTextures.Assets[@"Props\ForegroundProp3"];
   _Props_ForegroundProp4 = AutoloadTextures.Assets[@"Props\ForegroundProp4"];
   _Props_ForegroundProp5 = AutoloadTextures.Assets[@"Props\ForegroundProp5"];
   _Props_ForegroundProp6 = AutoloadTextures.Assets[@"Props\ForegroundProp6"];
   _Props_ForegroundProp7 = AutoloadTextures.Assets[@"Props\ForegroundProp7"];
   _Props_ForegroundProp8 = AutoloadTextures.Assets[@"Props\ForegroundProp8"];
   _Props_ForegroundProp9 = AutoloadTextures.Assets[@"Props\ForegroundProp9"];
   _Props_ForestBigRockOne = AutoloadTextures.Assets[@"Props\ForestBigRockOne"];
   _Props_ForestBigRockThree = AutoloadTextures.Assets[@"Props\ForestBigRockThree"];
   _Props_ForestBigRockTwo = AutoloadTextures.Assets[@"Props\ForestBigRockTwo"];
   _Props_ForestBushOne = AutoloadTextures.Assets[@"Props\ForestBushOne"];
   _Props_ForestDecoBD1 = AutoloadTextures.Assets[@"Props\ForestDecoBD1"];
   _Props_ForestDecoBD10 = AutoloadTextures.Assets[@"Props\ForestDecoBD10"];
   _Props_ForestDecoBD11 = AutoloadTextures.Assets[@"Props\ForestDecoBD11"];
   _Props_forestDecoBD12 = AutoloadTextures.Assets[@"Props\forestDecoBD12"];
   _Props_ForestDecoBD13 = AutoloadTextures.Assets[@"Props\ForestDecoBD13"];
   _Props_ForestDecoBD2 = AutoloadTextures.Assets[@"Props\ForestDecoBD2"];
   _Props_ForestDecoBD3 = AutoloadTextures.Assets[@"Props\ForestDecoBD3"];
   _Props_ForestDecoBD4 = AutoloadTextures.Assets[@"Props\ForestDecoBD4"];
   _Props_ForestDecoBD5 = AutoloadTextures.Assets[@"Props\ForestDecoBD5"];
   _Props_ForestDecoBD6 = AutoloadTextures.Assets[@"Props\ForestDecoBD6"];
   _Props_ForestDecoBD7 = AutoloadTextures.Assets[@"Props\ForestDecoBD7"];
   _Props_ForestDecoBD8 = AutoloadTextures.Assets[@"Props\ForestDecoBD8"];
   _Props_ForestDecoBD9 = AutoloadTextures.Assets[@"Props\ForestDecoBD9"];
   _Props_ForestDecoFive = AutoloadTextures.Assets[@"Props\ForestDecoFive"];
   _Props_ForestDecoFour = AutoloadTextures.Assets[@"Props\ForestDecoFour"];
   _Props_ForestDecoOne = AutoloadTextures.Assets[@"Props\ForestDecoOne"];
   _Props_ForestDecoSix = AutoloadTextures.Assets[@"Props\ForestDecoSix"];
   _Props_ForestDecoThree = AutoloadTextures.Assets[@"Props\ForestDecoThree"];
   _Props_ForestDecoTwo = AutoloadTextures.Assets[@"Props\ForestDecoTwo"];
   _Props_ForestFlowerFive = AutoloadTextures.Assets[@"Props\ForestFlowerFive"];
   _Props_ForestFlowerFour = AutoloadTextures.Assets[@"Props\ForestFlowerFour"];
   _Props_ForestFlowerOne = AutoloadTextures.Assets[@"Props\ForestFlowerOne"];
   _Props_ForestFlowerSix = AutoloadTextures.Assets[@"Props\ForestFlowerSix"];
   _Props_ForestFlowerThree = AutoloadTextures.Assets[@"Props\ForestFlowerThree"];
   _Props_ForestFlowerTwo = AutoloadTextures.Assets[@"Props\ForestFlowerTwo"];
   _Props_ForestGrassEight = AutoloadTextures.Assets[@"Props\ForestGrassEight"];
   _Props_ForestGrassFive = AutoloadTextures.Assets[@"Props\ForestGrassFive"];
   _Props_ForestGrassFour = AutoloadTextures.Assets[@"Props\ForestGrassFour"];
   _Props_ForestGrassNine = AutoloadTextures.Assets[@"Props\ForestGrassNine"];
   _Props_ForestGrassOne = AutoloadTextures.Assets[@"Props\ForestGrassOne"];
   _Props_ForestGrassSeven = AutoloadTextures.Assets[@"Props\ForestGrassSeven"];
   _Props_ForestGrassSix = AutoloadTextures.Assets[@"Props\ForestGrassSix"];
   _Props_ForestGrassThree = AutoloadTextures.Assets[@"Props\ForestGrassThree"];
   _Props_ForestGrassTwo = AutoloadTextures.Assets[@"Props\ForestGrassTwo"];
   _Props_ForestLogOne = AutoloadTextures.Assets[@"Props\ForestLogOne"];
   _Props_ForestRockEight = AutoloadTextures.Assets[@"Props\ForestRockEight"];
   _Props_ForestRockEleven = AutoloadTextures.Assets[@"Props\ForestRockEleven"];
   _Props_ForestRockFive = AutoloadTextures.Assets[@"Props\ForestRockFive"];
   _Props_ForestRockFour = AutoloadTextures.Assets[@"Props\ForestRockFour"];
   _Props_ForestRockNine = AutoloadTextures.Assets[@"Props\ForestRockNine"];
   _Props_ForestRockOne = AutoloadTextures.Assets[@"Props\ForestRockOne"];
   _Props_ForestRockSeven = AutoloadTextures.Assets[@"Props\ForestRockSeven"];
   _Props_ForestRockSix = AutoloadTextures.Assets[@"Props\ForestRockSix"];
   _Props_ForestRockTen = AutoloadTextures.Assets[@"Props\ForestRockTen"];
   _Props_ForestRockThree = AutoloadTextures.Assets[@"Props\ForestRockThree"];
   _Props_ForestRockTwo = AutoloadTextures.Assets[@"Props\ForestRockTwo"];
   _Props_ForestTree1 = AutoloadTextures.Assets[@"Props\ForestTree1"];
   _Props_ForestTree2 = AutoloadTextures.Assets[@"Props\ForestTree2"];
   _Props_MediumTree1 = AutoloadTextures.Assets[@"Props\MediumTree1"];
   _Props_MediumTree2 = AutoloadTextures.Assets[@"Props\MediumTree2"];
   _Props_MediumTree3 = AutoloadTextures.Assets[@"Props\MediumTree3"];
   _Props_MediumTree4 = AutoloadTextures.Assets[@"Props\MediumTree4"];
   _Props_SmallTree1 = AutoloadTextures.Assets[@"Props\SmallTree1"];
   _Props_SmallTree2 = AutoloadTextures.Assets[@"Props\SmallTree2"];
   _Props_StopSigns = AutoloadTextures.Assets[@"Props\StopSigns"];
   _Props_StreetLights = AutoloadTextures.Assets[@"Props\StreetLights"];
   _Props_TrafficLight = AutoloadTextures.Assets[@"Props\TrafficLight"];
   _Props_Waterfall = AutoloadTextures.Assets[@"Props\Waterfall"];
   _Weapons_Crowbar = AutoloadTextures.Assets[@"Weapons\Crowbar"];
 }
 }
}