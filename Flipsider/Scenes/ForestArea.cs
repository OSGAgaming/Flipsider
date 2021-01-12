using System;
using System.Collections.Generic;
using System.Text;

using Flipsider.Engine;
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            Main.renderer.RenderBG(spriteBatch,Color.White, TextureCache.skybox, -0.9f, 0.4f);
            Main.renderer.RenderBG(spriteBatch, Color.White, TextureCache.ForestBackground3, -0.6f, 0.4f);
            Main.renderer.RenderBG(spriteBatch, Color.White, TextureCache.ForestBackground2, -0.5f, 0.4f);
            Main.renderer.RenderBG(spriteBatch, Color.White, TextureCache.ForestBackground1, -0.4f, 0.4f);
            Main.renderer.layerHandler.DrawLayers(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, null, transformMatrix: Main.mainCamera.Transform, samplerState: SamplerState.PointClamp);
            Main.Primitives.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, null, transformMatrix: Main.mainCamera.Transform, samplerState: SamplerState.PointClamp);
            Main.renderer.RenderEntities();
            Main.renderer.RenderWater();
            NPC.DTH.Draw(spriteBatch);
            Main.CurrentWorld.tileManager.ShowTileCursor(Main.CurrentWorld);
            PropManager.ShowPropCursor();
            Main.Editor.Draw();
            Main.renderer.RenderUI();
            Main.renderer.lighting?.DrawLightMap(Main.CurrentWorld);
        }
    }
}
