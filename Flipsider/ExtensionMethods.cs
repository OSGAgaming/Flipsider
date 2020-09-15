
using Flipsider;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Flipsider
{
    public static class Extensions
    {
        public static Vector2 ToScreen(this Vector2 v) => (v + Main.mainCamera.CamPos) / Main.mainCamera.scale - Main.screenSize/8f;
    }
}
