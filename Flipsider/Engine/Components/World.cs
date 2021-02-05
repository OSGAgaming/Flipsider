using Flipsider.Engine;
using Microsoft.Xna.Framework;
using System;
using System.IO;

namespace Flipsider
{
    [Serializable]
    public class World
    {
        public LayerHandler layerHandler = new LayerHandler();
        public LevelInfo levelInfo => LevelInfo.Convert(this);
        public int TileRes => TileManager.tileRes;
        public int MaxTilesX { get; private set; }
        public int MaxTilesY { get; private set; }
        [NonSerialized]
        public Player? MainPlayer;
        [NonSerialized]
        public TileManager tileManager;
        [NonSerialized]
        public Manager<Water> WaterBodies = new Manager<Water>();
        [NonSerialized]
        public PropManager propManager = new PropManager();
        public bool IsTileActive(int i, int j)
        {
            if (tileManager.GetTile(i, j) == null || !tileManager.GetTile(i, j).Active)
            return false;
            return true;
        }
        public void ClearWorld()
        {
            for (int i = 0; i < propManager?.props.Count; i++)
            {
                propManager.props[i].Dispose();
            }
            for (int i = 0; i < WaterBodies?.Components.Count; i++)
            {
                WaterBodies.Components[i].Dispose();
            }
        }
        public bool IsActive(int i, int j)
        {
            if (tileManager.GetTile(i, j) != null)
            {
                if (tileManager.GetTile(i, j).Active)
                {
                   return true;
                }
            }
            return false;
        }
        public void RetreiveLevelInfo(LevelInfo levelInfo)
        {
          //  tileManager.tiles = levelInfo.tiles ?? tileManager.tiles;
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
