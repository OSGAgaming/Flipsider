using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace FlipEngine
{
    public class GuassianWeights
    {
        public float[] GuassianWeight;
        public float[] Offsets;

        public static GuassianWeights DefaultGuassianWeights = new GuassianWeights(20, 1, 4f);
        private void Load(int width, float accuracy, float STDEV)
        {
            for (int i = 0; i < width; i++)
            {
                var index = i;
                float mid = width/2;
                float dist = (i - mid) * accuracy;
                Offsets[index] = dist + 1.5f;
            }
            GuassianWeight = GaussianBlurHori(width, STDEV);
        }

        public static float[] GaussianBlurHori(int length, float weight)
        {
            float[] kernel = new float[length];
            float kernelSum = 0;
            int foff = (length - 1) / 2;
            float distance = 0;
            float constant = 1f / (2 * (float)Math.PI * weight * weight);
            for (int y = -foff; y <= foff; y++)
            {

                distance = ((y * y)) / (2 * weight * weight);
                kernel[y + foff] = constant * (float)Math.Exp(-distance);
                kernelSum += kernel[y + foff];
            }
            for (int y = 0; y < length; y++)
            {
                kernel[y] /=  kernelSum;
            }
            return kernel;
        }
        public float GuassianFunction(float x, float y, float STDEV)
        {
            float exponent = -(x * x + y * y) / (2 * STDEV * STDEV);
            return (float)((1 / (2 * Math.PI * STDEV * STDEV)) * (Math.Exp(exponent)));
        }
        public GuassianWeights(int width, float accuracy, float STDEV = 0.5f)
        {
            GuassianWeight = new float[width];
            Offsets = new float[width];
            Load(width, accuracy, STDEV);
        }
    }
}