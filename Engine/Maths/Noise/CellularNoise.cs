using Microsoft.Xna.Framework;
using System;

namespace Flipsider.Maths.Noise
{
    public class CellularNoise : INoise
    {
        private const int WIDTH = 128;
        private const int MASK = 127;

        private readonly Vector2[,] _cells;

        public CellularNoise() : this((int)DateTime.Now.Ticks) { }
        public CellularNoise(int seed)
        {
            _cells = new Vector2[WIDTH, WIDTH];
            Random random = new Random(seed);
            for (int i = 0; i < WIDTH; i++)
            {
                for (int j = 0; j < WIDTH; j++)
                {
                    _cells[i, j] = new Vector2(random.NextFloat(1f), random.NextFloat(1f));
                }
            }
        }

        public float Noise1D(float x)
        {
            return Noise2D(x, 0f);
        }

        public float Noise2D(float x, float y)
        {
            int cellX = (int)Math.Floor(x);
            int cellY = (int)Math.Floor(y);

            int maxX = cellX + 1;
            int maxY = cellY + 1;

            Vector2 point = new Vector2(x, y);
            float minDistance = 1f;

            for (cellX--; cellX <= maxX; cellX++)
            {
                int testCellX = Enforce(cellX);
                for (int j = cellY - 1; j <= maxY; j++)
                {
                    int testCellY = Enforce(j);

                    minDistance = Math.Min(minDistance, Vector2.DistanceSquared(point, _cells[testCellX, testCellY] + new Vector2(cellX, j)));
                }
            }

            return (float)Math.Sqrt(minDistance);
        }

        public float Noise2DOctaves(float x, float y, int octaves, float lacunarity = 1.75f)
        {
            //initial values
            float value = 0f;
            float gain = 1f / lacunarity;

            for (int i = 0; i < octaves; i++)
            {
                value += Noise2D(x, y) * gain;
                //multiply the values to move them and get different data
                x *= lacunarity;
                y *= lacunarity;
                //each extra octave only effects the final value by a half.
                gain *= 0.5f;
            }

            return value;
        }

        private int Enforce(int value)
        {
            return value & MASK;
        }
    }
}
