using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Net.Mime;

namespace Flipsider
{
    public class Lighting
    {
        public static RenderTarget2D? lightMap;
        public static RenderTarget2D? tileMap;
        public static float baseLight
        {
            get;
            private set;
        }
        public struct LightSource
        {
            public int strength;
            public Vector2 position;
            public Color colour;
            public LightSource(int str, Vector2 pos, Color col)
            {
                strength = str;
                position = pos;
                colour = col;
            }
        }
        public static Effect? LightingEffect;
        public static void Load(ContentManager Content)
        {
            LightingEffect = Content.Load<Effect>(@"Effect/Lighting");
            LoadLightMap();
            LoadTileMap();
            SetBaseLight(0.3f);
            AddLight(1, new Vector2(100, 100), Color.Blue);
        }

        public static List<LightSource> lightSources = new List<LightSource>();
        public static void SetBaseLight(float bl) => baseLight = bl;
        public static void AddLight(int str, Vector2 pos, Color col) => lightSources.Add(new LightSource(str, pos, col));
        public static void LoadLightMap() => lightMap = new RenderTarget2D(Main.graphics.GraphicsDevice, (int)Main.ScreenSize.X, (int)Main.ScreenSize.Y);
        public static void LoadTileMap() => tileMap = new RenderTarget2D(Main.graphics.GraphicsDevice, (int)Main.ScreenSize.X, (int)Main.ScreenSize.Y);

        public static void Update()
        {
            DrawLightMap();
        }
        public static void ApplyShader()
        {
            LightingEffect?.Parameters["lightMask"]?.SetValue(lightMap);
            LightingEffect?.Parameters["tileMask"]?.SetValue(tileMap);
            LightingEffect?.Parameters["baseLight"]?.SetValue(baseLight);
            LightingEffect?.CurrentTechnique.Passes[0].Apply();
        }
        public static void DrawLightMap()
        {
            Main.graphics.GraphicsDevice.SetRenderTarget(lightMap);
            Main.graphics.GraphicsDevice.Clear(Color.Black); 
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, transformMatrix: Main.mainCamera.Transform, samplerState: SamplerState.PointClamp);
            foreach(LightSource ls in lightSources)
            {
                Main.spriteBatch.Draw(TextureCache.PointLight, ls.position, ls.colour);
            }
            
            Main.graphics.GraphicsDevice.SetRenderTarget(tileMap);
            Main.graphics.GraphicsDevice.Clear(Color.Black);
            //tileMap System here
            Main.spriteBatch.End();
            Main.graphics.GraphicsDevice.SetRenderTarget(null);
        }
    }
}