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
    public class World
    {
        public Tile[,] tiles;
        public int TileRes => 32;
        public int MaxTilesX { get; private set; }
        public int MaxTilesY { get; private set; }

        public bool IsTileActive(int i, int j)
        {
            if (tiles[i, j] == null)
            return false;
            if (!tiles[i, j].active)
            return false;
            return true;
        }

        public World(int Width, int Height)
        {
            MaxTilesX = Width;
            MaxTilesY = Height;
            tiles = new Tile[Width, Height];
        }
    }
}
