using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using static Flipsider.Prop;
using static Flipsider.TileManager;
using static Flipsider.PropManager;
using System.Diagnostics;

namespace Flipsider
{
    delegate void LightTargetEvent();
    public class Lighting
    {
        public Manager<LightSource> lightSources = new Manager<LightSource>();
        public RenderTarget2D? lightMap;
        public RenderTarget2D? tileMap;
        public RenderTarget2D? miscMap;
        public RenderTarget2D? waterMap;
        public float tileDiffusion;
        public float generalDiffusion;
        public List<DirectionalLightSource> directionalLightSources = new List<DirectionalLightSource>();
        public static float baseLight
        {
            get;
            private set;
        }
        public struct DirectionalLightSource
        {
            public Vector2 position1;
            public Vector2 position2;
            public Color colour;
            public float angularCoverage;
            public DirectionalLightSource(Vector2 p1, Vector2 p2, Color col, float rad = 0.2f)
            {
                position1 = p1;
                position2 = p2;
                colour = col;
                angularCoverage = rad;
            }
        }
        public Lighting(ContentManager Content, float baseLight = 1f, float GD = 1.3f, float TD = 2f)
        {
            LightingEffect = Content.Load<Effect>(@"Effect/Lighting");
            PrimtiveShader = Content.Load<Effect>(@"Effect/PrimtiveShader");
            LoadLightMap();
            LoadTileMap();
            LoadMiscMap();
            LoadWaterMap();
            SetBaseLight(baseLight);
            generalDiffusion = GD;
            tileDiffusion = TD;
        }
        public static Effect? LightingEffect;
        public static Effect? PrimtiveShader;
        internal void Load(ContentManager Content)
        {

        }
        public static void SetBaseLight(float bl) => baseLight = bl;
        public void AddLight(float str, Vector2 p, Color col, float ang) => lightSources.AddComponent(new DirectionalLight(str, p, col, ang, 0));
        public void AddLight(float str, Vector2 p, Color col, float ang, float rotation) => lightSources.AddComponent(new DirectionalLight(str, p, col, ang, rotation));
        public void LoadLightMap() => lightMap = new RenderTarget2D(Main.graphics.GraphicsDevice, 2560, 1440);
        public void LoadTileMap() => tileMap = new RenderTarget2D(Main.graphics.GraphicsDevice, 2560, 1440);
        public void LoadMiscMap() => miscMap = new RenderTarget2D(Main.graphics.GraphicsDevice, 2560, 1440);
        public void LoadWaterMap() => waterMap = new RenderTarget2D(Main.graphics.GraphicsDevice, 2560, 1440);

        public void ApplyShader()
        {
            LightingEffect?.Parameters["lightMask"]?.SetValue(lightMap);
            LightingEffect?.Parameters["tileMask"]?.SetValue(tileMap);
            LightingEffect?.Parameters["tileDiffusion"]?.SetValue(tileDiffusion);
            LightingEffect?.Parameters["generalDiffusion"]?.SetValue(generalDiffusion);
            LightingEffect?.Parameters["miscMap"]?.SetValue(miscMap);
            LightingEffect?.Parameters["baseLight"]?.SetValue(baseLight);
            LightingEffect?.Parameters["waterMap"]?.SetValue(waterMap);
            LightingEffect?.Parameters["noiseTexture"]?.SetValue(TextureCache.Noise);
            LightingEffect?.Parameters["Time"]?.SetValue(Time.TotalTimeMil / 60f);
            LightingEffect?.CurrentTechnique.Passes[0].Apply();
        }
        public void DrawLightMap(World world)
        {
            Main.graphics.GraphicsDevice.SetRenderTarget(lightMap);
            Main.graphics.GraphicsDevice.Clear(Color.Black);

            //  Main.spriteBatch.Draw(TextureCache.magicPixel, intersection,new Rectangle(0,0,5,5), Color.White);

            Main.graphics.GraphicsDevice.SetRenderTarget(tileMap);

            for (int a = 0; a < Main.layerHandler.GetLayerCount(); a++)
            {
                Main.spriteBatch.End();
                foreach (LightSource ls in lightSources.Components)
                {
                    if (ls.Layer == a)
                    {
                        ls.Draw(Main.spriteBatch);
                    }
                }
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, transformMatrix: Main.mainCamera.Transform, samplerState: SamplerState.PointClamp);

                for (int i = 0; i < Main.WaterBodies.Count; i++)
                {
                    if (Main.WaterBodies[i].Layer == a)
                    {
                        Utils.DrawBoxFill(Main.WaterBodies[i].frame, Color.Blue);
                    }
                }
                int fluff = 10;
                Vector2 SafeBoundX = new Vector2(Main.mainCamera.CamPos.X, Main.mainCamera.CamPos.X + Main.ScreenSize.X / Main.ScreenScale) / 32;
                Vector2 SafeBoundY = new Vector2(Main.mainCamera.CamPos.Y, Main.mainCamera.CamPos.Y + Main.ScreenSize.Y / Main.ScreenScale) / 32;
                for (int i = (int)SafeBoundX.X - fluff; i < (int)SafeBoundX.Y + fluff; i++)
                {
                    for (int j = (int)SafeBoundY.X - fluff; j < (int)SafeBoundY.Y + fluff; j++)
                    {
                        if (world.IsTileInBounds(i, j))
                        {
                            int TR = Main.CurrentWorld.TileRes;
                            Utils.DrawBoxFill(new Rectangle(i * TR, j * TR, TR, TR), Color.Red);
                        }
                    }
                }
            }

            Main.graphics.GraphicsDevice.SetRenderTarget(miscMap);
            Main.graphics.GraphicsDevice.Clear(Color.Black);
            for (int a = 0; a < Main.layerHandler.GetLayerCount(); a++)
            {
                for (int i = 0; i < world.propManager.props.Count; i++)
                {
                    var cprop = world.propManager.props[i];
                    if (cprop.active && cprop.Layer == a)
                        Main.spriteBatch.Draw(PropTypes[cprop.prop], cprop.Center, PropEntites[cprop.prop].alteredFrame, Color.White, 0f, PropEntites[cprop.prop].alteredFrame.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0f);
                }
            }
            for (int k = 0; k < Main.entities.Count; k++)
            {
                Entity entity = Main.entities[k];
                entity.Draw(Main.spriteBatch);
            }
            Main.graphics.GraphicsDevice.SetRenderTarget(waterMap);
            Main.graphics.GraphicsDevice.Clear(Color.Black);

            Main.graphics.GraphicsDevice.SetRenderTarget(null);
        }
    }
}