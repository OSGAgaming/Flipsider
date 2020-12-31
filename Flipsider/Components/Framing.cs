using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Flipsider.Prop;
using static Flipsider.PropManager;
using static Flipsider.PropInteraction;
namespace Flipsider
{
    public static class Framing
    {
        public static Rectangle GetTileFrame(World world,int i, int j)
        {
            //fuck this is gonna be messy:
            if (i > 0 && j > 0 && i < world.MaxTilesX && j < world.MaxTilesY && world.tiles[i,j] != null)
            {
                bool upLeft = world.tiles[i - 1, j - 1].active;
                bool upMid = world.tiles[i, j - 1].active;
                bool upRight = world.tiles[i + 1, j - 1].active;

                bool left = world.tiles[i - 1, j].active;
                bool right = world.tiles[i + 1, j].active;

                bool downLeft = world.tiles[i - 1, j + 1].active;
                bool downMid = world.tiles[i, j + 1].active;
                bool downRight = world.tiles[i + 1, j + 1].active;

                //non sloped for now

                if (!upMid && !left && right && !downMid)
                {
                    return new Rectangle(0, 0, 32, 32);
                }
                if (!upMid && left && right && !downMid)
                {
                    return new Rectangle(32, 0, 32, 32);
                }
                if (!upMid && left && !right && !downMid)
                {
                    return new Rectangle(64, 0, 32, 32);
                }
                if (!upMid && !left && !right && downMid)
                {
                    return new Rectangle(96, 0, 32, 32);
                }

                if (!upMid && !left && right && downMid)
                {
                    if (!downRight)
                    {
                        return new Rectangle(192, 128, 32, 32);
                    }
                    return new Rectangle(0, 32, 32, 32);
                }
                if (!upMid && left && right && downMid)
                {
                    if (!downLeft && !downRight)
                    {
                        return new Rectangle(224, 192, 32, 32);
                    }
                    if (!downLeft)
                    {
                        return new Rectangle(0, 352, 32, 32);
                    }
                    if (!downRight)
                    {
                        return new Rectangle(32, 352, 32, 32);
                    }
                    return new Rectangle(32, 32, 32, 32);
                }
                if (!upMid && left && !right && downMid)
                {
                    if (!downLeft)
                    {
                        return new Rectangle(224, 128, 32, 32);
                    }
                    return new Rectangle(64, 32, 32, 32);
                }
                if (upMid && !left && !right && downMid)
                {
                    return new Rectangle(96, 32, 32, 32);
                }

                if (upMid && !left && right && downMid)
                {
                    if (!upRight && !downRight)
                    {
                        return new Rectangle(160, 192, 32, 32);
                    }
                    if (!upRight)
                    {
                        return new Rectangle(64, 352, 32, 32);
                    }
                    if (!downRight)
                    {
                        return new Rectangle(64, 384, 32, 32);
                    }
                    return new Rectangle(0, 64, 32, 32);
                }
                if (upMid && left && right && downMid)
                {
                    if (!upLeft && upRight && downLeft && !downRight)
                    {
                        return new Rectangle(0, 128, 32, 32);
                    }
                    if (upLeft && !upRight && !downLeft && downRight)
                    {
                        return new Rectangle(32, 128, 32, 32);
                    }
                    if (!upLeft && !upRight && !downLeft && downRight)
                    {
                        return new Rectangle(64, 128, 32, 32);
                    }
                    if (!upLeft && !upRight && downLeft && !downRight)
                    {
                        return new Rectangle(96, 128, 32, 32);
                    }
                    if (!upLeft && upRight && downLeft && downRight)
                    {
                        return new Rectangle(128, 128, 32, 32);
                    }
                    if (upLeft && !upRight && downLeft && downRight)
                    {
                        return new Rectangle(160, 128, 32, 32);
                    }

                    if (!upLeft && upRight && !downLeft && !downRight)
                    {
                        return new Rectangle(64, 160, 32, 32);
                    }
                    if (upLeft && !upRight && !downLeft && !downRight)
                    {
                        return new Rectangle(96, 160, 32, 32);
                    }
                    if (upLeft && upRight && !downLeft && downRight)
                    {
                        return new Rectangle(128, 160, 32, 32);
                    }
                    if (upLeft && upRight && downLeft && !downRight)
                    {
                        return new Rectangle(160, 160, 32, 32);
                    }
                    if (!upLeft && !upRight && !downLeft && !downRight)
                    {
                        return new Rectangle(0, 192, 32, 32);
                    }
                    if (!upLeft && !upRight && downLeft && downRight)
                    {
                        return new Rectangle(32, 192, 32, 32);
                    }
                    if (upLeft && upRight && !downLeft && !downRight)
                    {
                        return new Rectangle(64, 192, 32, 32);
                    }
                    if (!upLeft && upRight && !downLeft && downRight)
                    {
                        return new Rectangle(96, 192, 32, 32);
                    }
                    if (upLeft && !upRight && downLeft && !downRight)
                    {
                        return new Rectangle(128, 192, 32, 32);
                    }

                    return new Rectangle(32, 64, 32, 32);
                }
                if (upMid && left && !right && downMid)
                {
                    if (!upLeft && !downLeft)
                    {
                        return new Rectangle(192, 192, 32, 32);
                    }
                    if (!upLeft)
                    {
                        return new Rectangle(96, 352, 32, 32);
                    }
                    if (!downLeft)
                    {
                        return new Rectangle(96, 384, 32, 32);
                    }
                    return new Rectangle(64, 64, 32, 32);
                }
                if (upMid && !left && !right && !downMid)
                {
                    return new Rectangle(96, 64, 32, 32);
                }

                if (upMid && !left && right && !downMid)
                {
                    if (!upRight)
                    {
                        return new Rectangle(192, 160, 32, 32);
                    }
                    return new Rectangle(0, 96, 32, 32);
                }
                if (upMid && left && right && !downMid)
                {
                    if (!upLeft && !upRight)
                    {
                        return new Rectangle(224, 224, 32, 32);
                    }
                    if (!upLeft)
                    {
                        return new Rectangle(0, 384, 32, 32);
                    }
                    if (!upRight)
                    {
                        return new Rectangle(32, 384, 32, 32);
                    }
                    return new Rectangle(32, 96, 32, 32);
                }
                if (upMid && left && !right && !downMid)
                {
                    if (!upLeft)
                    {
                        return new Rectangle(224, 160, 32, 32);
                    }
                    return new Rectangle(64, 96, 32, 32);
                }
                if (!upMid && !left && !right && !downMid)
                {
                    return new Rectangle(96, 96, 32, 32);
                }

            }
            return new Rectangle(0, 0, 32, 32);

        }
    }
}
