using System;
using System.Collections.Generic;
using System.Text;

using Flipsider.Engine;
using Flipsider.Engine.Input;
using Flipsider.Engine.Interfaces;
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
            ForestAreaParticles.SpawnModules.Add(new ModifyPositionRand(new Vector2(-1000,1000), new Vector2(300,300),Main.rand));
            ForestAreaParticles.SpawnModules.Add(new SetColorBetweenTwoColours(Color.White, Color.Lime, Main.rand));
            ForestAreaParticles.SpawnModules.Add(new SetVelocity(Vector2.UnitY * -100f));
            ForestAreaParticles.SpawnModules.Add(new SetLifetime(10f));
            ForestAreaParticles.UpdateModules.Add(new OpacityOverLifetime(Engine.Maths.EaseFunction.ReverseLinear));
            ForestAreaParticles.UpdateModules.Add(new TurnRand(-.5f,.5f));
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 offset = new Vector2(0, Utils.BOTTOM - Main.ScreenSize.Y);
            Main.renderer.RenderBG(spriteBatch,Color.White, TextureCache.skybox, -0.9f, 0.4f, offset);
            Main.renderer.RenderBG(spriteBatch, Color.White, TextureCache.ForestBackground3, -0.6f, 0.4f, offset);
            Main.renderer.RenderBG(spriteBatch, Color.White, TextureCache.ForestBackground2, -0.5f, 0.4f, offset);
            Main.renderer.RenderBG(spriteBatch, Color.White, TextureCache.ForestBackground1, -0.4f, 0.4f, offset);
            Main.renderer.RenderEntities();
            Main.renderer.layerHandler.DrawLayers(spriteBatch);
            NPC.DTH.Draw(spriteBatch);
            Main.CurrentWorld.tileManager.ShowTileCursor(Main.CurrentWorld);
            PropManager.ShowPropCursor();
            Main.Editor.Draw();
            ForestAreaParticles.Draw(spriteBatch);
            Main.renderer.RenderUI();
          //  spriteBatch.Draw(Main.renderer?.lighting?.tileMap ?? TextureCache.ForestGrassEight,new Rectangle((int)Main.mainCamera.CamPos.X, (int)Main.mainCamera.CamPos.Y, 800/5,480/5),Color.White);
            Main.renderer?.lighting?.DrawLightMap(Main.CurrentWorld);
        }
    }
}
