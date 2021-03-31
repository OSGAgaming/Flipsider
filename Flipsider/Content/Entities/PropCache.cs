
using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Flipsider.PropInteraction;

namespace Flipsider
{
    public partial class PropManager
    {
        public void LoadProps()
        {
            # region death
            AddPropType("Misc_Sky", TextureCache.GreenSlime);

            AddPropType("Ruins_1", TextureCache.BrickStructure1);
            AddPropType("Ruins_2", TextureCache.BrickStructure2);

            AddPropType("MediumTree_1", TextureCache.MediumTree1);
            AddPropType("MediumTree_2", TextureCache.MediumTree2);
            AddPropType("MediumTree_3", TextureCache.MediumTree3);
            AddPropType("MediumTree_4", TextureCache.MediumTree4);

            AddPropType("SmallTree_1", TextureCache.SmallTree1);
            AddPropType("SmallTree_2", TextureCache.SmallTree2);

            AddPropType("BackgroundTree_1", TextureCache.BackgroundTree1);
            AddPropType("BackgroundTree_2", TextureCache.BackgroundTree2);
            AddPropType("BackgroundTree_3", TextureCache.BackgroundTree3);
            AddPropType("BackgroundTree_4", TextureCache.BackgroundTree4);
            AddPropType("Foreground_Grass", TextureCache.ForegroundGrass1);

            AddPropType("City_TrafficLight", TextureCache.TrafficLight);
            AddPropType("City_BusStop", TextureCache.BusStop);
            AddPropType("City_BigBusStop", TextureCache.BigBusStop);
            AddPropType("City_BikeRack", TextureCache.BikeRack);
            AddPropType("City_StopSigns", TextureCache.StopSigns);
            AddPropType("City_StreetLights", TextureCache.StreetLights);

            AddPropType("Forest_ForestTree1", TextureCache.ForestTree1);
            AddPropType("Forest_ForestTree2", TextureCache.ForestTree2);
            AddPropType("Forest_ForestFlowerOne", TextureCache.ForestFlowerOne);
            AddPropType("Forest_ForestFlowerTwo", TextureCache.ForestFlowerTwo);
            AddPropType("Forest_ForestFlowerThree", TextureCache.ForestFlowerThree);
            AddPropType("Forest_ForestFlowerFour", TextureCache.ForestFlowerFour);
            AddPropType("Forest_ForestFlowerFive", TextureCache.ForestFlowerFive);
            AddPropType("Forest_ForestFlowerSix", TextureCache.ForestFlowerSix);

            AddPropType("Forest_ForestBushOne", TextureCache.ForestBushOne);
            AddPropType("Forest_ForestLogOne", TextureCache.ForestLogOne);
            AddPropType("Forest_ForestDecoOne", TextureCache.ForestDecoOne);
            AddPropType("Forest_ForestDecoTwo", TextureCache.ForestDecoTwo);
            AddPropType("Forest_ForestDecoThree", TextureCache.ForestDecoThree);
            AddPropType("Forest_ForestDecoFour", TextureCache.ForestDecoFour);
            AddPropType("Forest_ForestDecoFive", TextureCache.ForestDecoFive);
            AddPropType("Forest_ForestDecoSix", TextureCache.ForestDecoSix);

            AddPropType("Forest_ForestDecoBD1", TextureCache.ForestDecoBD1);
            AddPropType("Forest_ForestDecoBD2", TextureCache.ForestDecoBD2);
            AddPropType("Forest_ForestDecoBD3", TextureCache.ForestDecoBD3);
            AddPropType("Forest_ForestDecoBD4", TextureCache.ForestDecoBD4);
            AddPropType("Forest_ForestDecoBD5", TextureCache.ForestDecoBD5);
            AddPropType("Forest_ForestDecoBD6", TextureCache.ForestDecoBD6);
            AddPropType("Forest_ForestDecoBD7", TextureCache.ForestDecoBD7);
            AddPropType("Forest_ForestDecoBD8", TextureCache.ForestDecoBD8);
            AddPropType("Forest_ForestDecoBD9", TextureCache.ForestDecoBD9);
            AddPropType("Forest_ForestDecoBD10", TextureCache.ForestDecoBD10);
            AddPropType("Forest_ForestDecoBD11", TextureCache.ForestDecoBD11);
            AddPropType("Forest_ForestDecoBD12", TextureCache.ForestDecoBD12);
            AddPropType("Forest_ForestDecoBD13", TextureCache.ForestDecoBD13);

            AddPropType("Debug_Player", TextureCache.player);
            AddPropType("Debug_Blob", TextureCache.Blob);
            AddPropType("Debug_HudSlot", TextureCache.hudSlot);
            AddPropType("Debug_TestGun", TextureCache.testGun);
            AddPropType("Debug_SaveTex", TextureCache.SaveTex);

            AddPropType("Forest_ForestForegroundProp1", TextureCache.ForestForegroundProp1);
            AddPropType("Forest_ForestForegroundProp2", TextureCache.ForestForegroundProp2);
            AddPropType("Forest_ForestForegroundProp3", TextureCache.ForestForegroundProp3);
            AddPropType("Forest_ForestForegroundProp4", TextureCache.ForestForegroundProp4);
            AddPropType("Forest_ForestForegroundProp5", TextureCache.ForestForegroundProp5);
            AddPropType("Forest_ForestForegroundProp6", TextureCache.ForestForegroundProp6);
            AddPropType("Forest_ForestForegroundProp7", TextureCache.ForestForegroundProp7);
            AddPropType("Forest_ForestForegroundProp8", TextureCache.ForestForegroundProp8);
            AddPropType("Forest_ForestForegroundProp9", TextureCache.ForestForegroundProp9);
            AddPropType("Forest_ForestForegroundProp10", TextureCache.ForestForegroundProp10);
            AddPropType("Forest_ForestForegroundProp11", TextureCache.ForestForegroundProp11);

            AddPropType("Forest_ForestRockOne", TextureCache.ForestRockOne);
            AddPropType("Forest_ForestRockTwo", TextureCache.ForestRockTwo);
            AddPropType("Forest_ForestRockThree", TextureCache.ForestRockThree);
            AddPropType("Forest_ForestRockFour", TextureCache.ForestRockFour);
            AddPropType("Forest_ForestRockFive", TextureCache.ForestRockFive);
            AddPropType("Forest_ForestRockSix", TextureCache.ForestRockSix);
            AddPropType("Forest_ForestRockSeven", TextureCache.ForestRockSeven);
            AddPropType("Forest_ForestRockEight", TextureCache.ForestRockEight);
            AddPropType("Forest_ForestRockNine", TextureCache.ForestRockNine);
            AddPropType("Forest_ForestRockTen", TextureCache.ForestRockTen);
            AddPropType("Forest_ForestRockEleven", TextureCache.ForestRockEleven);

            AddPropType("Forest_ForestBigRockOne", TextureCache.ForestBigRockOne);
            AddPropType("Forest_ForestBigRockTwo", TextureCache.ForestBigRockTwo);
            AddPropType("Forest_ForestBigRockThree", TextureCache.ForestBigRockThree);

            AddPropType("Forest_Waterfall", TextureCache.Waterfall);

            ChangeFrames("City_StreetLights", 5);
            ChangeFrames("City_StopSigns", 3);
            ChangeAnimSpeed("City_StreetLights", 20);
            ChangeAnimSpeed("City_StopSigns", 20);
            # endregion
        }
   
    }
}
