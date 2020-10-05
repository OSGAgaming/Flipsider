using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flipsider.Extensions
{
    public static class RandomExtension
    {
        public static float NextFloat(this Random random, float max = 1) => (float)random.NextDouble() * max;
        public static float ReciprocateTo(this ref float num, float target, float ease)
        {
            return (target - num) / ease;
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
    }

    public static class MathExtension
    {
        public static float Slope(this Vector2 v)
        {
            return v.Y / v.X;
        }
    }
}
