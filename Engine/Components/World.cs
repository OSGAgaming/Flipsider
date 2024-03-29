using Flipsider.Engine;
using Flipsider.Engine.Interfaces;
using Flipsider.Engine.Particles;
using Flipsider.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Flipsider
{
    public class World : IComponent
    {
        public LayerHandler layerHandler = new LayerHandler();
        public LevelInfo levelInfo => LevelInfo.Convert(this);

        public int TileRes => TileManager.tileRes;

        public int MaxTilesX { get; private set; }
        public int MaxTilesY { get; private set; }
        public TileManager tileManager { get; set; }
        public Manager<Water> WaterBodies { get; set; }
        public PropManager propManager { get; set; }
        public Skybox Skybox { get; set; }

        public Player? MainPlayer;

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
            if (File.Exists(Utils.WorldPath + FileName))
            {
                ClearWorld();
                Main.Editor.CurrentSaveFile = FileName;
                Stream stream = File.OpenRead(Utils.WorldPath + FileName);
                levelInfo.Deserialize(stream);
            }
        }

        public Player ReplacePlayer(Player player)
        {
            if (player != null)
            {
                MainPlayer?.Dispose();
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

        public void SetSkybox(Skybox skybox) => Skybox = skybox;

        public void Update()
        {
            GlobalParticles.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            PropManager.ShowPropCursor();
            NPC.ShowNPCCursor();

            layerHandler.DrawLayers(spriteBatch);
            Skybox.Draw(spriteBatch);
            Main.renderer.PrintRenderTarget(layerHandler.Target);

            GlobalParticles.Draw(spriteBatch);
        }

        public World(int Width, int Height)
        {
            MaxTilesX = Width;
            MaxTilesY = Height;

            tileManager = new TileManager(Width, Height);
            WaterBodies = new Manager<Water>();
            propManager = new PropManager();

            Skybox = new Skybox();

            GlobalParticles = new ParticleSystem(200);
        }
    }
}
