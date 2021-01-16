using System;
using System.Collections.Generic;
using System.Text;

using Flipsider.Engine;
using Flipsider.Engine.Input;
using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flipsider.Scenes
{
    public class ForestArea : Scene
    {
        public ForestArea()
        {

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
            Main.TestParticleSystem.Position = Vector2.Zero;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Main.renderer.RenderBG(spriteBatch,Color.White, TextureCache.skybox, -0.9f, 0.4f);
            Main.renderer.RenderBG(spriteBatch, Color.White, TextureCache.ForestBackground3, -0.6f, 0.4f);
            Main.renderer.RenderBG(spriteBatch, Color.White, TextureCache.ForestBackground2, -0.5f, 0.4f);
            Main.renderer.RenderBG(spriteBatch, Color.White, TextureCache.ForestBackground1, -0.4f, 0.4f);
            Main.renderer.RenderEntities();
            Main.renderer.layerHandler.DrawLayers(spriteBatch);
            NPC.DTH.Draw(spriteBatch);
            Main.CurrentWorld.tileManager.ShowTileCursor(Main.CurrentWorld);
            PropManager.ShowPropCursor();
            Main.Editor.Draw();
            Main.TestParticleSystem.Draw(spriteBatch);
            Main.renderer.RenderUI();
          //  spriteBatch.Draw(Main.renderer?.lighting?.tileMap ?? TextureCache.ForestGrassEight,new Rectangle((int)Main.mainCamera.CamPos.X, (int)Main.mainCamera.CamPos.Y, 800/5,480/5),Color.White);
            Main.renderer?.lighting?.DrawLightMap(Main.CurrentWorld);
        }
    }
}
