using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace Flipsider
{
    public class GuassianWeights
    {
       public float[] GuassianWeight;
       public Vector2[] Offsets;
       private void Load(int width, int Height, float accuracy,float STDEV)
       {
            for(int i = 0; i<width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var index = j + (i * width);
                    Vector2 mid = new Vector2(width / 2, Height / 2);
                    Vector2 dist = new Vector2(((i - mid.X) * accuracy) / width, ((j - mid.Y) * accuracy) / Height);
                    Offsets[index] = dist;
                    GuassianWeight = GaussianBlur(width, STDEV).Flatten().ToArray();
                }
            }
       }

        public static float[,] GaussianBlur(int length, float weight)
        {
            float[,] kernel = new float[length, length];
            float kernelSum = 0;
            int foff = (length - 1) / 2;
            float distance = 0;
            float constant = 1f / (2 * (float)Math.PI * weight * weight);
            for (int y = -foff; y <= foff; y++)
            {
                for (int x = -foff; x <= foff; x++)
                {
                    distance = ((y * y) + (x * x)) / (2 * weight * weight);
                    kernel[y + foff, x + foff] = constant * (float)Math.Exp(-distance);
                    kernelSum += kernel[y + foff, x + foff];
                }
            }
            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    kernel[y, x] = kernel[y, x] * 1f / kernelSum;
                }
            }
            return kernel;
        }
        public float GuassianFunction(float x, float y, float STDEV)
        {
            float exponent = -(x * x + y * y) / (2 * STDEV * STDEV);
            return (float)(1 / (2 * Math.PI * STDEV * STDEV)* Math.Exp(exponent));
        }
       public GuassianWeights(int width, int height, float accuracy, float STDEV = 0.5f)
        {
            GuassianWeight = new float[width*height];
            Offsets = new Vector2[width * height];
            Load(width, height, accuracy, STDEV);
        }
    }
}