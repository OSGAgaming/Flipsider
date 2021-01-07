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
namespace Flipsider
{
    public class Lighting
    {
        public RenderTarget2D? lightMap;
        public RenderTarget2D? tileMap;
        public RenderTarget2D? miscMap;
        public float tileDiffusion;
        public float generalDiffusion;
        public List<LightSource> lightSources = new List<LightSource>();
        public List<DirectionalLightSource> directionalLightSources = new List<DirectionalLightSource>();
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
        public Lighting(ContentManager Content, float baseLight = 1f, float GD = 1.3f, float TD = 2f)
        {
            LightingEffect = Content.Load<Effect>(@"Effect/Lighting");
            LoadLightMap();
            LoadTileMap();
            LoadMiscMap();
            SetBaseLight(baseLight);
            generalDiffusion = GD;
            tileDiffusion = TD;
        }
        public static Effect? LightingEffect;
        internal void Load(ContentManager Content)
        {

        }
        public static void SetBaseLight(float bl) => baseLight = bl;
        public void AddLight(int str, Vector2 pos, Color col) => lightSources.Add(new LightSource(str, pos, col));
        public void AddDirectionalLight(Vector2 p1, Vector2 p2, Color col) => directionalLightSources.Add(new DirectionalLightSource(p1, p2, col));
        public void LoadLightMap() => lightMap = new RenderTarget2D(Main.graphics.GraphicsDevice, 1980, 1080);
        public void LoadTileMap() => tileMap = new RenderTarget2D(Main.graphics.GraphicsDevice, 1980, 1080);
        public void LoadMiscMap() => miscMap = new RenderTarget2D(Main.graphics.GraphicsDevice, 1980, 1080);

        public void ApplyShader()
        {
            LightingEffect?.Parameters["lightMask"]?.SetValue(lightMap);
            LightingEffect?.Parameters["tileMask"]?.SetValue(tileMap);
            LightingEffect?.Parameters["tileDiffusion"]?.SetValue(tileDiffusion);
            LightingEffect?.Parameters["generalDiffusion"]?.SetValue(generalDiffusion);
            LightingEffect?.Parameters["miscMap"]?.SetValue(miscMap);
            LightingEffect?.Parameters["baseLight"]?.SetValue(baseLight);
            LightingEffect?.CurrentTechnique.Passes[0].Apply();
        }
        public void DrawLightMap(World world)
        {
            Main.graphics.GraphicsDevice.SetRenderTarget(lightMap);
            Main.graphics.GraphicsDevice.Clear(Color.Black);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, transformMatrix: Main.mainCamera.Transform, samplerState: SamplerState.PointClamp);
            foreach (LightSource ls in lightSources)
            {
                Main.spriteBatch.Draw(TextureCache.PointLight, ls.position, ls.colour);
            }
            foreach (DirectionalLightSource dl in directionalLightSources)
            {
                Vector2 origin = dl.position1;
                for (int i = -50; i < 50; i++)
                {
                    Vector2 diffVec = dl.position2 - origin;
                    Vector2 secondPos = origin + diffVec.RotatedBy(i / (100 / dl.angularCoverage));
                    Vector2 intersection = NumericalHelpers.ReturnIntersectionTile(Main.CurrentWorld, origin.ToPoint(), secondPos.ToPoint());
                    bool intersectionState = NumericalHelpers.LineIntersectsTile(Main.CurrentWorld, origin.ToPoint(), secondPos.ToPoint());
                    if (intersectionState)
                    {
                        DrawMethods.DrawLine(origin, intersection - (origin - intersection) / 10f, Color.White * 0.2f, 4);
                    }
                    else
                    {
                        DrawMethods.DrawLine(origin, secondPos, Color.White * 0.2f, 4);
                    }

                }
            }
            //  Main.spriteBatch.Draw(TextureCache.magicPixel, intersection,new Rectangle(0,0,5,5), Color.White);

            Main.graphics.GraphicsDevice.SetRenderTarget(tileMap);


            float scale = Main.mainCamera.scale;
            scale = Math.Clamp(scale, 0.5f, 1);
            int fluff = 10;
            Vector2 SafeBoundX = new Vector2(Main.mainCamera.CamPos.X, Main.mainCamera.CamPos.X + Main.ScreenSize.X / Main.ScreenScale) / 32;
            Vector2 SafeBoundY = new Vector2(Main.mainCamera.CamPos.Y, Main.mainCamera.CamPos.Y + Main.ScreenSize.Y / Main.ScreenScale) / 32;
            for (int i = (int)SafeBoundX.X - fluff; i < (int)SafeBoundX.Y + fluff; i++)
            {
                for (int j = (int)SafeBoundY.X - fluff; j < (int)SafeBoundY.Y + fluff; j++)
                {
                    if (i > 0 && j > 0 && i < world.MaxTilesX && j < world.MaxTilesY && world.tiles[i, j] != null)
                    {
                        if (world.tiles[i, j].active)
                        {
                            if (world.tiles[i, j].type == -1)
                            {
                                //  DrawMethods.DrawSquare(new Vector2(i * tileRes, j * tileRes), tileRes, Color.White);
                            }
                            else
                            {
                                int TR = Main.CurrentWorld.TileRes;
                              
                                Main.spriteBatch.Draw(Main.CurrentWorld.tileManager.tileDict[world.tiles[i, j].type], new Rectangle(i * Main.CurrentWorld.TileRes, j * TR, TR, TR), world.tiles[i, j].frame, Color.White);
                            }
                        }
                    }
                }
            }

            Main.graphics.GraphicsDevice.SetRenderTarget(miscMap);
            Main.graphics.GraphicsDevice.Clear(Color.Black);
            for (int i = 0; i < world.propManager.props.Count; i++)
            {
                var cprop = world.propManager.props[i];
                Main.spriteBatch.Draw(PropTypes[cprop.prop], cprop.Center, PropEntites[cprop.prop].alteredFrame, Color.White, 0f, PropEntites[cprop.prop].alteredFrame.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0f);
            }
            for (int k = 0; k < Main.entities.Count; k++)
            {
                Entity entity = Main.entities[k];
                entity.Draw(Main.spriteBatch);
            }
            for (int i = 0; i < Main.WaterBodies.Count; i++)
            {
                Main.WaterBodies[i].Draw(Main.spriteBatch);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin();
            Main.graphics.GraphicsDevice.SetRenderTarget(null);
        }
    }
}