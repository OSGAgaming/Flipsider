using Flipsider.Engine;
using Flipsider.Engine.Interfaces;
using Flipsider.Engine.Particles;
using Flipsider.GUI.TilePlacementGUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Flipsider
{
    [Serializable]
    public class World : IComponent
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

        public ParticleSystem GlobalParticles;

        public bool IsTileActive(int i, int j)
        {
            if (tileManager.GetTile(i, j) == null || !tileManager.GetTile(i, j).Active)
            return false;
            return true;
        }
        public void ClearWorld()
        {
            int width = tileManager.chunks.GetLength(0);
            int height = tileManager.chunks.GetLength(1);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    for (int k = 0; k < tileManager.chunks[i, j].Entities.Count; k++)
                        tileManager.chunks[i, j].Entities[k].Dispose();
                }
            }
            int propLength = propManager.props.Count;
            for (int i = 0; i < propLength; i++)
            {
                propManager.props[i].Dispose();
            }
            MainPlayer?.Dispose();
        }
        public bool IsActive(int i, int j)
        {
            if (i > 0 && j > 0)
            {
                if (tileManager.GetTile(i, j) != null)
                {
                    if (tileManager.GetTile(i, j).Active)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void RetreiveLevelInfo(string FileName)
        {
            Logger.NewText(Main.MainPath + FileName);

            if (File.Exists(Main.MainPath + FileName))
            {
                ClearWorld();
                Main.Editor.CurrentSaveFile = FileName;
                Stream stream = File.OpenRead(Main.MainPath + FileName);
                LevelInfo LevelInfo = levelInfo.Deserialize(stream);
                LevelInfo.LoadToWorld(this);
            }
        }
        public Player ReplacePlayer(Player player)
        {
            if (player != null)
            {
                MainPlayer = player;
                return player;
            }
            return MainPlayer ?? new Player();
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

        public void Update()
        {
            GlobalParticles.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            GlobalParticles.Draw(spriteBatch);
        }

        public World(int Width, int Height)
        {
            MaxTilesX = Width;
            MaxTilesY = Height;
            tileManager = new TileManager(Width, Height);
            GlobalParticles = new ParticleSystem(200);
        }
    }
}
