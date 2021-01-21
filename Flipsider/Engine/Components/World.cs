using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Flipsider.Prop;
using static Flipsider.PropManager;
using static Flipsider.PropInteraction;
using Flipsider.Engine;
using System.IO;

namespace Flipsider
{
    [Serializable]
    public class World
    {
        public LevelInfo levelInfo => LevelInfo.Convert(this);
        public Tile[,] tiles => tileManager.tiles;
        public int TileRes => TileManager.tileRes;
        public int MaxTilesX { get; private set; }
        public int MaxTilesY { get; private set; }
        [NonSerialized]
        public Player? MainPlayer;
        [NonSerialized]
        public EntityManager entityManager = new EntityManager();
        [NonSerialized]
        public TileManager tileManager;
        [NonSerialized]
        public Manager<Water> WaterBodies = new Manager<Water>();
        [NonSerialized]
        public PropManager propManager = new PropManager();
        public bool IsTileActive(int i, int j)
        {
            if (tiles[i, j] == null)
                return false;
            if (!tiles[i, j].Active)
            if (!tiles[i, j].Active)
                return false;
            return true;
        }

        public bool IsTileInBounds(int i, int j)
        {
            if (i >= 0 && j >= 0 && i < MaxTilesX && j < MaxTilesY && tiles[i, j] != null)
            {
                if (tiles[i, j].Active && !tiles[i, j].wall)
                {
                    if (tiles[i, j].type != -1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void RetreiveLevelInfo(LevelInfo levelInfo)
        {
            tileManager.tiles = levelInfo.tiles ?? tileManager.tiles;
        }
        public void RetreiveLevelInfo(string FileName)
        {
            if (File.Exists(Main.MainPath + FileName))
            {
                Main.Editor.CurrentSaveFile = FileName;
                for (int i = 0; i < tiles.GetLength(0); i++)
                {
                    for (int j = 0; j < tiles.GetLength(1); j++)
                    {
                        tileManager.RemoveTile(this, new Vector2(i, j));
                    }
                }
                for (int i = 0; i < propManager?.props.Count; i++)
                {
                    propManager.props[i].active = false;
                }
                LevelInfo LevelInfo = Main.serializers.Deserialize<LevelInfo>(Main.MainPath + FileName);
                for (int i = 0; i < LevelInfo?.tiles?.GetLength(0); i++)
                {
                    for (int j = 0; j < LevelInfo?.tiles?.GetLength(1); j++)
                    {
                        if (LevelInfo.tiles[i, j] != null)
                        {
                            tileManager.AddTile(this, LevelInfo.tiles[i, j].type, new Vector2(i, j));
                        }
                    }
                }

                for (int i = 0; i < LevelInfo?.props?.Length; i++)
                {
                    propManager?.AddProp(this, LevelInfo.props[i].prop, LevelInfo.props[i].position);
                }
            }
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
