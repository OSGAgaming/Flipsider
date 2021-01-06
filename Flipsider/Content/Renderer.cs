using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using System.Collections.Generic;

using Flipsider.GUI;
using Flipsider.GUI.HUD;
using Flipsider.Scenes;
using Flipsider.Engine.Particles;
using Flipsider.Engine;
using Flipsider.Engine.Audio;
using Flipsider.Engine.Input;
using Flipsider.GUI.TilePlacementGUI;
using static Flipsider.TileManager;
using static Flipsider.PropManager;
using System.Reflection;
using System.Linq;
using System.Threading;

namespace Flipsider
{
    public delegate void LayerEventDelegate(World world, SpriteBatch spriteBatch);
    public class Renderer
    {
        public LayerHandler layerHandler = new LayerHandler();
        internal Lighting? lighting;
        public GraphicsDeviceManager? graphics;
        public RenderTarget2D? renderTarget;
        public Game? instance;
        public SpriteBatch spriteBatch;
        public Camera? mainCamera;
        public float ScreenScale
        {
            get
            {
                if (mainCamera != null)
                    return mainCamera.scale;
                return 1;
            }
        }
        public Vector2 ScreenSize => graphics?.GraphicsDevice == null ? Vector2.One : graphics.GraphicsDevice.Viewport.Bounds.Size.ToVector2();
        public Renderer(Game game)
        {
            instance = game;
            graphics = new GraphicsDeviceManager(instance);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.ApplyChanges();
            mainCamera = new Camera();
            renderTarget = new RenderTarget2D(graphics?.GraphicsDevice, (int)1980, (int)1080);
            spriteBatch = new SpriteBatch(graphics?.GraphicsDevice);
        }

        public void Load()
        {
            layerHandler.AddLayer();
            layerHandler.AddLayer();
            layerHandler.AddLayer();
            layerHandler.AddLayer();
            if (instance != null)
                lighting = new Lighting(instance.Content, 1f);
        }
        public void Draw()
        {
            graphics?.GraphicsDevice.Clear(Color.CornflowerBlue);
            graphics?.GraphicsDevice.SetRenderTarget(renderTarget);
            Render();
            graphics?.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, transformMatrix: mainCamera?.Transform, samplerState: SamplerState.PointClamp);
            lighting?.ApplyShader();
            PrintRenderTarget();
            spriteBatch.End();
        }
        public Vector2 PreferredSize
        {
            get
            {
                if(graphics != null)
                return new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                return Vector2.Zero;
            }
        }

        public void PrintRenderTarget()
        {
            if (graphics != null)
            {
                Rectangle frame = new Rectangle(0, 0, (int)ScreenSize.X, (int)ScreenSize.Y);
                Rectangle destination = new Rectangle((int)Vector2.Zero.ToScreen().X, (int)Vector2.Zero.ToScreen().Y, (int)(ScreenSize.X * (1 / ScreenScale)), (int)(ScreenSize.Y * (1 / ScreenScale)));
                spriteBatch.Draw(renderTarget, destination, frame, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            }
        }
        public void Render()
        {
            //Todo: Events
            RenderBG(Main.spriteBatch, TextureCache.skybox, -0.9f, 0.4f);
            RenderBG(Main.spriteBatch, TextureCache.ForestBackground3, -0.6f, 0.4f);
            RenderBG(Main.spriteBatch, TextureCache.ForestBackground2, -0.5f, 0.4f);
            RenderBG(Main.spriteBatch,TextureCache.ForestBackground1,-0.4f,0.4f);

            layerHandler.DrawLayers(spriteBatch);
            spriteBatch.Begin(transformMatrix: Main.mainCamera.Transform, samplerState: SamplerState.PointClamp);

            for (int k = 0; k < Main.entities.Count; k++)
            {
                Entity entity = Main.entities[k];
                entity.Draw(spriteBatch);
            }
            for (int i = 0; i < Main.WaterBodies.Count; i++)
            {
                Main.WaterBodies[i].Draw(spriteBatch);
            }

            NPC.DTH.Draw(spriteBatch);
            Main.CurrentWorld.tileManager.ShowTileCursor(Main.CurrentWorld);
            ShowPropCursor();
            RenderUI();

            Main.Editor.Draw();
            Debug.Write("SL:" + Main.ScreenSize);
            Debug.Write(Main.ActualScreenSize);
            lighting?.DrawLightMap(Main.CurrentWorld);

            spriteBatch.End();

        }
        public void RenderBG(SpriteBatch spriteBatch,Texture2D Tex, float paralax, float scale)
        {
            spriteBatch.Begin(transformMatrix: Main.mainCamera.Transform, samplerState: SamplerState.PointClamp);
            Rectangle dims = new Rectangle(0, 0, Tex.Width, Tex.Height);
            for(int i = 0; i<6; i++)
            spriteBatch.Draw(Tex, new Vector2(i* (Tex.Width * scale), 0).AddParralaxAcross(paralax) - new Vector2(Main.mainCamera.LeftBound*-paralax, 0), dims, Color.White, 0f, new Vector2(0, Tex.Height * scale), scale, SpriteEffects.None, 0f);
            spriteBatch.End();

        }
        public void RenderUI()
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            for (int i = 0; i < UIScreenManager.Instance?.Components.Count; i++)
            {
                UIScreenManager.Instance.Components[i].active = true;
                UIScreenManager.Instance?.Components[i].Draw(spriteBatch);
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, null, transformMatrix: Main.mainCamera.Transform, samplerState: SamplerState.PointClamp);
            //debuganthinghere
            // Main.instance.fps.DrawFps(Main.spriteBatch, Main.font, new Vector2(10, 36), Color.Black);
        }
    }
}
