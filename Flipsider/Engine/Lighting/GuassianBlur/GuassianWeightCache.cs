using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

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
                    GuassianWeight[index] = GuassianFunction((i - mid.X), (j - mid.Y), STDEV);
                }
            }
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