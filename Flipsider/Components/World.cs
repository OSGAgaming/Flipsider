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

        public Tile[,] tiles => tileManager.tiles;
        public int TileRes => TileManager.tileRes;
        public int MaxTilesX { get; private set; }
        public int MaxTilesY { get; private set; }

        public Player? MainPlayer;

        public EntityManager entityManager = new EntityManager();

        public TileManager tileManager;

        public Manager<Water> WaterBodies = new Manager<Water>();

        public PropManager propManager = new PropManager();
        public bool IsTileActive(int i, int j)
        {
            if (tiles[i, j] == null)
                return false;
            if (!tiles[i, j].active)
                return false;
            return true;
        }

        public bool AppendPlayer(Player player)
        {
            if (player != null)
            {
                MainPlayer = player;
                return true;
            }
            return false;
        }

        public World(int Width, int Height)
        {
            MaxTilesX = Width;
            MaxTilesY = Height;
            tileManager = new TileManager(Width, Height);
        }
    }
}
