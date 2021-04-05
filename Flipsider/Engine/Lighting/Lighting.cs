using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using static Flipsider.PropManager;

namespace Flipsider
{
    internal delegate void LightTargetEvent();
    public class Lighting
    {
        public Map Maps;
        public Manager<LightSource> lightSources = new Manager<LightSource>();
        public RenderTarget2D? lightMap;
        public RenderTarget2D? tileMap;
        public RenderTarget2D? miscMap;
        public RenderTarget2D? waterMap;
        public float tileDiffusion;
        public float generalDiffusion;
        public float baseLight
        {
            get;
            private set;
        }
        public Lighting(ContentManager Content, float baseLight = 1f, float GD = 1.3f, float TD = 2f)
        {
            LightingEffect = Content.Load<Effect>(@"Effect/Lighting");
            PrimtiveShader = Content.Load<Effect>(@"Effect/PrimtiveShader");
            Bloom = Content.Load<Effect>(@"Effect/Bloom");
            LoadMap(out lightMap);
            LoadMap(out tileMap);
            LoadMap(out miscMap);
            LoadMap(out waterMap);
            this.baseLight = baseLight;
            generalDiffusion = GD;
            tileDiffusion = TD;

            Maps = new Map();

            Maps.AddMap("FGWater",0, new FgWaterPass());
            Maps.AddMap("Leaves", 1, new LeavesPass());
            Maps.AddMap("Bloom", 2, new BloomMap());
            //Maps.AddMap("Pixelation", 2, new PixelationPass());
        }
        public static Effect? LightingEffect;
        public static Effect? PrimtiveShader;
        public static Effect? Bloom;
        public void LoadMap(out RenderTarget2D renderTarget) => renderTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, 2560, 1440);
        public void AddLight(float str, Vector2 p, Color col, float ang) => lightSources.AddComponent(new DirectionalLight(str, p, col, ang, 0));
        public void AddLight(float str, Vector2 p, Color col, float ang, float rotation) => lightSources.AddComponent(new DirectionalLight(str, p, col, ang, rotation));
        public void AddLight(LivingEntity entity, Texture2D BloomMap, float str, Vector2 pos = default, Color col = default) => lightSources.AddComponent(new EntityBloom(entity, BloomMap, str,pos,col));
        public void RemoveBloom(Entity entity)
        {
            foreach (ILayeredComponent lc in Main.layerHandler.Layers[entity.Layer].Drawables.ToArray())
            {
                if(lc is EntityBloom)
                {
                    EntityBloom EB = (EntityBloom)lc;
                    if(EB.BindableEntity == entity)
                    {
                        Main.layerHandler.Layers[entity.Layer].Drawables.Remove(lc);
                    }
                }
            }
        }
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
        public void Invoke()
        {
            Main.graphics.GraphicsDevice.SetRenderTarget(lightMap);
            Main.graphics.GraphicsDevice.Clear(Color.Transparent);
            foreach (LightSource ls in lightSources.Components)
            {
                ls.Draw(Main.spriteBatch);
            }

            Main.graphics.GraphicsDevice.SetRenderTarget(tileMap);
            for (int a = 0; a < Main.layerHandler.GetLayerCount(); a++)
            {
                Main.spriteBatch.End();

                Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, transformMatrix: Main.mainCamera.Transform, samplerState: SamplerState.PointClamp);

                for (int i = 0; i < Main.WaterBodies.Count; i++)
                {
                    if (Main.WaterBodies[i].Layer == a)
                    {
                        Utils.DrawBoxFill(Main.WaterBodies[i].frame, Color.Blue);
                    }
                }
            }
            Main.graphics.GraphicsDevice.SetRenderTarget(miscMap);
            Main.graphics.GraphicsDevice.Clear(Color.Black);
            Main.renderer.PrintRenderTarget(Main.layerHandler.RTGaming);

            Main.graphics.GraphicsDevice.SetRenderTarget(waterMap);
            Main.graphics.GraphicsDevice.Clear(Color.Black);

            Main.graphics.GraphicsDevice.SetRenderTarget(null);
        }
    }
}