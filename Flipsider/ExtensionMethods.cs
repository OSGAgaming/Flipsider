
using Flipsider;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Flipsider
{
    public static class Extensions
    {
        public static Vector2 ToScreen(this Vector2 v) => (v + Main.mainCamera.CamPos) / Main.mainCamera.scale - Main.ScreenSize / 8f;

        public static Vector2 AddParralaxAcross(this Vector2 v, float traversingPixels)
            {

            float traverseFunction = Math.Clamp(Main.player.position.X / traversingPixels, Main.mainCamera.LeftBound / traversingPixels, 100000);
              return v - new Vector2(traverseFunction, 0);
           }
    }
}
