using Flipsider.Engine;
using Microsoft.Xna.Framework;
using System;
using System.IO;

namespace Flipsider
{
    [Serializable]
    public class World
    {
        public Chunk[,]? Chunks;
        public LayerHandler layerHandler = new LayerHandler();
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
        public void ClearWorld()
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    tileManager.RemoveTile(this, new Vector2(i, j));
                }
            }
            for (int i = 0; i < propManager?.props.Count; i++)
            {
                propManager.props[i].Dispose();
            }
            for (int i = 0; i < WaterBodies?.Components.Count; i++)
            {
                WaterBodies.Components[i].Dispose();
            }
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
                ClearWorld();
                Main.Editor.CurrentSaveFile = FileName;
                LevelInfo LevelInfo = Main.serializers.Deserialize<LevelInfo>(Main.MainPath + FileName);
                LevelInfo.LoadToWorld(this);
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
