
using Flipsider.Engine;
using Flipsider.Engine.Interfaces;
using Flipsider.Engine.Maths;
using Flipsider.Engine.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flipsider.Scenes
{
    public class ForestArea : Scene
    {
        public ParticleSystem ForestAreaParticles;
        public ForestArea()
        {
            ForestAreaParticles = new ParticleSystem(200);
        }

        public override void Update()
        {
            Main.renderer.RenderingWater = true;
            foreach (IUpdate updateable in Main.UpdateablesOffScreen.ToArray())
            {
                if (updateable != null)
                    updateable.Update();
            }
            foreach (IUpdate updateable in Main.Updateables.ToArray())
            {
                if (updateable != null)
                    updateable.Update();
            }

        }

        public override void OnActivate()
        {
            ForestAreaParticles.Position = Vector2.Zero;
            ForestAreaParticles.SpawnRate = 10f;
            ForestAreaParticles.WorldSpace = true;
            ForestAreaParticles.SpawnModules.Add(new SetTexture(TextureCache.pixel));
            ForestAreaParticles.SpawnModules.Add(new SetScale(2f));
            ForestAreaParticles.SpawnModules.Add(new ModifyPositionToEntity(Main.player));
            ForestAreaParticles.SpawnModules.Add(new ModifyPositionRand(new Vector2(-1000, 1000), new Vector2(600, 600), Main.rand));
            ForestAreaParticles.SpawnModules.Add(new SetColorBetweenTwoColours(Color.White, Color.Lime, Main.rand));
            ForestAreaParticles.SpawnModules.Add(new SetVelocity(Vector2.UnitY * -100f));
            ForestAreaParticles.SpawnModules.Add(new SetLifetime(10f));
            ForestAreaParticles.SpawnModules.Add(new SetLightIntensityRand(0.2f,0.5f));
            ForestAreaParticles.SpawnModules.Add(new SetParalaxRand(.8f, -.8f));
            ForestAreaParticles.UpdateModules.Add(new OpacityOverLifetime(EaseFunction.ReverseLinear));
            ForestAreaParticles.UpdateModules.Add(new TurnRand(-.5f, .5f));
            Main.lighting.lightSources.AddComponent(new ParticleLight(ForestAreaParticles));
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 offset = new Vector2(0, Utils.BOTTOM - Main.ScreenSize.Y + 50);
            Main.CurrentWorld.layerHandler.DrawLayers(spriteBatch);
            Utils.BeginEndCameraSpritebatch();
            Utils.RenderBG(spriteBatch, Color.White, TextureCache.skybox, -0.9f, 0.8f, offset);
            Utils.RenderBGMoving(spriteBatch, 6f, Color.White, TextureCache.SkyboxFront, -0.9f, 0.8f, offset + new Vector2(0, -400));
            Utils.RenderBG(spriteBatch, Color.White, TextureCache.ForestBackground3, -0.8f, 0.7f, offset + new Vector2(0, 100));
            Utils.RenderBG(spriteBatch, Color.White, TextureCache.ForestBackground2, -0.7f, 0.7f, offset);
            Utils.RenderBG(spriteBatch, Color.White, TextureCache.ForestBackground1, -0.6f, 0.7f, offset);
            //Main.lighting.Maps.DrawToMap("Bloom", (SpriteBatch sb) => { Utils.RenderBG(spriteBatch, Color.White, TextureCache.ForestBackground1, -0.6f, 0.7f, offset); });
            Utils.BeginEndCameraSpritebatch();
            Main.renderer.PrintRenderTarget(Main.layerHandler.RTGaming);
            NPC.DTH.Draw(spriteBatch);
            PropManager.ShowPropCursor();
            NPC.ShowNPCCursor();
            Main.Editor.Draw();
            ForestAreaParticles.Update();
            ForestAreaParticles.Draw(spriteBatch);
            Main.renderer.RenderUI();
            //spriteBatch.Draw(Main.lighting.Maps.Get("Lighting").MapTarget ?? TextureCache.ForestGrassEight,new Rectangle((int)Main.mainCamera.CamPos.X, (int)Main.mainCamera.CamPos.Y, 800/5,480/5),Color.White);
            Main.renderer?.lighting?.Invoke();
        }
    }
}
