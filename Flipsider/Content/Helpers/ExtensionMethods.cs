
using Flipsider;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Flipsider
{
    public static class Extensions
    {
        public static float NextFloat(this Random random, float min, float max)
        {
            if (min > max)
            {
                throw new ArgumentException("min cannot be greater than max.");
            }

            return min + (float)random.NextDouble() * (max - min);
        }
        public static float NextFloat(this Random random, float max)
        {
            return (float)(random.NextDouble() * max);
        }
        public static bool IsBetween(this float num, float min,float max)
        {
            return num > min && num < max;
        }
        public static float ReciprocateTo(this float num, float target, float ease = 16f)
        {
            return num + (target - num) / ease;
        }
        public static float ReciprocateTo(this int num, float target, float ease)
        {
            return (target - num - 16) / ease;
        }
        public static void Shuffle<T>(this Random random, ref T[] input)
        {
            for (int i = input.Length - 1; i > 0; i--)
            {
                int index = random.Next(i + 1);

                T value = input[index];
                input[index] = input[i];
                input[i] = value;
            }
        }
        public static Vector2 Snap(this Vector2 v, int snap) => new Vector2((int)(v.X / snap)*snap, (int)(v.Y / snap)*snap);
        public static Vector2 ToScreen(this Vector2 v) => (v / Main.mainCamera.scale + Main.mainCamera.CamPos);

        public static Vector2 ToScreenInv(this Vector2 v) => ((v - Main.mainCamera.CamPos) * Main.mainCamera.scale);

        public static Point ToScreen(this Point v) => (v.ToVector2() / new Vector2(Main.mainCamera.scale, Main.mainCamera.scale) + Main.mainCamera.CamPos).ToPoint();
        public static Vector2 AddParalaxAcrossX(this Vector2 v, float traversingPixels)
       =>
            v - new Vector2(Math.Clamp(Main.mainCamera.playerpos.X * traversingPixels, -100000, 100000), 0);
        public static Vector2 InvParalaxAcrossX(this Vector2 v, float traversingPixels)
        =>
             v + new Vector2(Math.Clamp(Main.mainCamera.playerpos.X * traversingPixels, -100000, 100000), 0);
        public static Vector2 AddParalaxAcrossY(this Vector2 v, float traversingPixels)
        {
            float traverseFunction = Math.Clamp(Main.mainCamera.playerpos.Y * traversingPixels, -100000, 100000);
            return v - new Vector2(0, traverseFunction);
        }
        public static Vector2 ToTile(this Vector2 v)
        {
            return v / TileManager.tileRes;
        }
        public static Point ToTile(this Point v)
        {
            return (v.ToVector2() / TileManager.tileRes).ToPoint();
        }

        public static float Slope(this Vector2 v)
        {
            return v.Y / v.X;
        }

    }
}
