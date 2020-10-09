using Flipsider.Core;
using Flipsider.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flipsider.Extensions
{
    public static class MathExtension
    {
        public static Vector2 ToScreenCoordinates(this Vector2 worldCoordinates) => worldCoordinates * Tile.TileSizePixels;
        public static Vector2 ToWorldCoordinates(this Vector2 screenCoordinates) => screenCoordinates / Tile.TileSizePixels;

        public static Rotation ToRotation(this Vector2 v) => Rotation.FromVector(v);

        public static float Slope(this Vector2 v)
        {
            return v.Y / v.X;
        }
    }
}
