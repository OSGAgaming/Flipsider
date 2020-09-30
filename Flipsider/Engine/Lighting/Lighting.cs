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
        public static List<DirectionalLightSource> directionalLightSources = new List<DirectionalLightSource>();
        public static void SetBaseLight(float bl) => baseLight = bl;
        public static void AddLight(int str, Vector2 pos, Color col) => lightSources.Add(new LightSource(str, pos, col));
        public static void AddDirectionalLight(Vector2 p1, Vector2 p2, Color col) => directionalLightSources.Add(new DirectionalLightSource(p1,p2,col));
        public static void LoadLightMap() => lightMap = new RenderTarget2D(Main.graphics.GraphicsDevice, (int)Main.ScreenSize.X, (int)Main.ScreenSize.Y);
        public static void LoadTileMap() => tileMap = new RenderTarget2D(Main.graphics.GraphicsDevice, (int)Main.ScreenSize.X, (int)Main.ScreenSize.Y);

        public static void Update()
        {
         //   DrawLightMap();
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
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, transformMatrix: Main.mainCamera.Transform, samplerState: SamplerState.PointClamp);
            foreach(LightSource ls in lightSources)
            {
                Main.spriteBatch.Draw(TextureCache.PointLight, ls.position, ls.colour);
            }
            foreach(DirectionalLightSource dl in directionalLightSources)
            {
                Vector2 origin = dl.position1;
                for (int i = -50; i < 50; i++)
                {
                    Vector2 diffVec = dl.position2 - origin;
                    Vector2 secondPos = origin + diffVec.RotatedBy(i / (100/dl.angularCoverage));
                    Vector2 intersection = NumericalHelpers.ReturnIntersectionTile(origin.ToPoint(), secondPos.ToPoint());
                    bool intersectionState = NumericalHelpers.LineIntersectsTile(origin.ToPoint(), secondPos.ToPoint());
                    if (intersectionState)
                    {
                        DrawMethods.DrawLine(origin, intersection, Color.White * 0.2f, 4);
                    }
                    else
                    {
                        DrawMethods.DrawLine(origin, secondPos, Color.White * 0.2f, 4);
                    }

                }
            }
            //  Main.spriteBatch.Draw(TextureCache.magicPixel, intersection,new Rectangle(0,0,5,5), Color.White);

            Main.graphics.GraphicsDevice.SetRenderTarget(tileMap);
            Main.graphics.GraphicsDevice.Clear(Color.Black);
            //tileMap System here
            Main.spriteBatch.End();
            Main.spriteBatch.Begin();
            Main.graphics.GraphicsDevice.SetRenderTarget(null);
        }
    }
}