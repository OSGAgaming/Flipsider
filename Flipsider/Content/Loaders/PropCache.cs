using FlipEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.IO;

namespace Flipsider
{
    // TODO dude.
#nullable disable
    public class PropCache : ILoadable
    {
        public void Load()
        {
            PropManager.AddPropType("Misc_Sky", TextureCache.GreenSlime);

            PropManager.AddPropType("Ruins_1", TextureCache.BrickStructure1);
            PropManager.AddPropType("Ruins_2", TextureCache.BrickStructure2);

            PropManager.AddPropType("MediumTree_1", TextureCache.MediumTree1);
            PropManager.AddPropType("MediumTree_2", TextureCache.MediumTree2);
            PropManager.AddPropType("MediumTree_3", TextureCache.MediumTree3);
            PropManager.AddPropType("MediumTree_4", TextureCache.MediumTree4);

            PropManager.AddPropType("SmallTree_1", TextureCache.SmallTree1);
            PropManager.AddPropType("SmallTree_2", TextureCache.SmallTree2);

            PropManager.AddPropType("BackgroundTree_1", TextureCache.BackgroundTree1);
            PropManager.AddPropType("BackgroundTree_2", TextureCache.BackgroundTree2);
            PropManager.AddPropType("BackgroundTree_3", TextureCache.BackgroundTree3);
            PropManager.AddPropType("BackgroundTree_4", TextureCache.BackgroundTree4);
            PropManager.AddPropType("Foreground_Grass", TextureCache.ForegroundGrass1);

            PropManager.AddPropType("City_TrafficLight", TextureCache.TrafficLight);
            PropManager.AddPropType("City_BusStop", TextureCache.BusStop);
            PropManager.AddPropType("City_BigBusStop", TextureCache.BigBusStop);
            PropManager.AddPropType("City_BikeRack", TextureCache.BikeRack);
            PropManager.AddPropType("City_StopSigns", TextureCache.StopSigns);
            PropManager.AddPropType("City_StreetLights", TextureCache.StreetLights);

            PropManager.AddPropType("EnergyRocc", TextureCache.EnergyRocc);

            PropManager.AddPropType("Forest_ForestTree1", TextureCache.ForestTree1);
            PropManager.AddPropType("Forest_ForestTree2", TextureCache.ForestTree2);
            PropManager.AddPropType("Forest_ForestFlowerOne", TextureCache.ForestFlowerOne);
            PropManager.AddPropType("Forest_ForestFlowerTwo", TextureCache.ForestFlowerTwo);
            PropManager.AddPropType("Forest_ForestFlowerThree", TextureCache.ForestFlowerThree);
            PropManager.AddPropType("Forest_ForestFlowerFour", TextureCache.ForestFlowerFour);
            PropManager.AddPropType("Forest_ForestFlowerFive", TextureCache.ForestFlowerFive);
            PropManager.AddPropType("Forest_ForestFlowerSix", TextureCache.ForestFlowerSix);

            PropManager.AddPropType("Forest_ForestBushOne", TextureCache.ForestBushOne);
            PropManager.AddPropType("Forest_ForestLogOne", TextureCache.ForestLogOne);
            PropManager.AddPropType("Forest_ForestDecoOne", TextureCache.ForestDecoOne);
            PropManager.AddPropType("Forest_ForestDecoTwo", TextureCache.ForestDecoTwo);
            PropManager.AddPropType("Forest_ForestDecoThree", TextureCache.ForestDecoThree);
            PropManager.AddPropType("Forest_ForestDecoFour", TextureCache.ForestDecoFour);
            PropManager.AddPropType("Forest_ForestDecoFive", TextureCache.ForestDecoFive);
            PropManager.AddPropType("Forest_ForestDecoSix", TextureCache.ForestDecoSix);

            PropManager.AddPropType("Forest_ForestDecoBD1", TextureCache.ForestDecoBD1);
            PropManager.AddPropType("Forest_ForestDecoBD2", TextureCache.ForestDecoBD2);
            PropManager.AddPropType("Forest_ForestDecoBD3", TextureCache.ForestDecoBD3);
            PropManager.AddPropType("Forest_ForestDecoBD4", TextureCache.ForestDecoBD4);
            PropManager.AddPropType("Forest_ForestDecoBD5", TextureCache.ForestDecoBD5);
            PropManager.AddPropType("Forest_ForestDecoBD6", TextureCache.ForestDecoBD6);
            PropManager.AddPropType("Forest_ForestDecoBD7", TextureCache.ForestDecoBD7);
            PropManager.AddPropType("Forest_ForestDecoBD8", TextureCache.ForestDecoBD8);
            PropManager.AddPropType("Forest_ForestDecoBD9", TextureCache.ForestDecoBD9);
            PropManager.AddPropType("Forest_ForestDecoBD10", TextureCache.ForestDecoBD10);
            PropManager.AddPropType("Forest_ForestDecoBD11", TextureCache.ForestDecoBD11);
            PropManager.AddPropType("Forest_ForestDecoBD12", TextureCache.ForestDecoBD12);
            PropManager.AddPropType("Forest_ForestDecoBD13", TextureCache.ForestDecoBD13);

            PropManager.AddPropType("Debug_Player", TextureCache.player);
            PropManager.AddPropType("Debug_Blob", TextureCache.Blob);
            PropManager.AddPropType("Debug_HudSlot", TextureCache.hudSlot);
            PropManager.AddPropType("Debug_TestGun", TextureCache.testGun);
            PropManager.AddPropType("Debug_SaveTex", TextureCache.SaveTex);

            PropManager.AddPropType("Forest_ForestForegroundProp1", TextureCache.ForestForegroundProp1);
            PropManager.AddPropType("Forest_ForestForegroundProp2", TextureCache.ForestForegroundProp2);
            PropManager.AddPropType("Forest_ForestForegroundProp3", TextureCache.ForestForegroundProp3);
            PropManager.AddPropType("Forest_ForestForegroundProp4", TextureCache.ForestForegroundProp4);
            PropManager.AddPropType("Forest_ForestForegroundProp5", TextureCache.ForestForegroundProp5);
            PropManager.AddPropType("Forest_ForestForegroundProp6", TextureCache.ForestForegroundProp6);
            PropManager.AddPropType("Forest_ForestForegroundProp7", TextureCache.ForestForegroundProp7);
            PropManager.AddPropType("Forest_ForestForegroundProp8", TextureCache.ForestForegroundProp8);
            PropManager.AddPropType("Forest_ForestForegroundProp9", TextureCache.ForestForegroundProp9);
            PropManager.AddPropType("Forest_ForestForegroundProp10", TextureCache.ForestForegroundProp10);
            PropManager.AddPropType("Forest_ForestForegroundProp11", TextureCache.ForestForegroundProp11);

            PropManager.AddPropType("Forest_ForestRockOne", TextureCache.ForestRockOne);
            PropManager.AddPropType("Forest_ForestRockTwo", TextureCache.ForestRockTwo);
            PropManager.AddPropType("Forest_ForestRockThree", TextureCache.ForestRockThree);
            PropManager.AddPropType("Forest_ForestRockFour", TextureCache.ForestRockFour);
            PropManager.AddPropType("Forest_ForestRockFive", TextureCache.ForestRockFive);
            PropManager.AddPropType("Forest_ForestRockSix", TextureCache.ForestRockSix);
            PropManager.AddPropType("Forest_ForestRockSeven", TextureCache.ForestRockSeven);
            PropManager.AddPropType("Forest_ForestRockEight", TextureCache.ForestRockEight);
            PropManager.AddPropType("Forest_ForestRockNine", TextureCache.ForestRockNine);
            PropManager.AddPropType("Forest_ForestRockTen", TextureCache.ForestRockTen);
            PropManager.AddPropType("Forest_ForestRockEleven", TextureCache.ForestRockEleven);

            PropManager.AddPropType("Forest_ForestBigRockOne", TextureCache.ForestBigRockOne);
            PropManager.AddPropType("Forest_ForestBigRockTwo", TextureCache.ForestBigRockTwo);
            PropManager.AddPropType("Forest_ForestBigRockThree", TextureCache.ForestBigRockThree);

            PropManager.AddPropType("Forest_Waterfall", TextureCache.Waterfall);

            PropManager.AddPropType("WideRoof1", Textures._Props_City_WideRoof1);
            PropManager.AddPropType("RoofBuilding1", Textures._Props_City_RoofBuilding1);

            PropManager.AddPropType("Room", Textures._Props_City_Apartment_MainRoom);
        }
    }
}
