
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

            AddPropType("Debug_Player", TextureCache.player);
            AddPropType("Debug_Blob", TextureCache.Blob);
            AddPropInteraction("Debug_Blob", BlobInteractable);
            AddPropType("Debug_HudSlot", TextureCache.hudSlot);
            AddPropType("Debug_TestGun", TextureCache.testGun);
            AddPropType("Debug_SaveTex", TextureCache.SaveTex);

            ChangeFrames("City_StreetLights", 5);
            ChangeFrames("City_StopSigns", 3);
            ChangeAnimSpeed("City_StreetLights", 20);
            ChangeAnimSpeed("City_StopSigns", 20);
            # endregion
        }
   
    }
    public partial class PropInteraction : IUpdate
    {
        public static void BlobInteractable()
        {
 
        }
    }
}
