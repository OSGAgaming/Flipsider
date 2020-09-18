using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

using Microsoft.Xna.Framework;

namespace Flipsider.Maths.Noise
{
    public class VoronoiNoise : INoise
    {
        private const int WIDTH = 128;
        private const int MASK = 127;

        private Vector2[,] _cells;

        public VoronoiNoise() : this((int)DateTime.Now.Ticks) { }
        public VoronoiNoise(int seed)
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

            Vector2 minPoint = Vector2.Zero;
            Vector2 point = new Vector2(x, y);
            float minDistance = 10f;

            for (int i = -1; i <= 1; i++)
            {
                int testCellX = Enforce(cellX + i);
                for (int j = -1; j <= 1; j++)
                {
                    int testCellY = Enforce(cellY + j);

                    Vector2 neighbour = new Vector2(cellX + i, cellY + j);
                    Vector2 p = _cells[testCellX, testCellY] + neighbour;

                    float dist = (point - p).LengthSquared();
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        minPoint = _cells[testCellX, testCellY];
                    }
                }
            }

            minPoint.Normalize();
            return Vector2.Dot(minPoint, Vector2.UnitY);
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
