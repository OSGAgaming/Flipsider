
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

        public static Vector2 ToScreen(this Vector2 v) => (v / Main.mainCamera.scale + Main.mainCamera.CamPos);
        public static Vector2 AddParralaxAcross(this Vector2 v, float traversingPixels)
        {
            float traverseFunction = Math.Clamp(Main.player.position.X / traversingPixels, Main.mainCamera.LeftBound / traversingPixels, 100000);
            return v - new Vector2(traverseFunction, 0);
        }
    }
}
