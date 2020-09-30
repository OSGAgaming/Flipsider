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

using System.Reflection;
using System.Linq;
using System.Threading;

namespace Flipsider
{
    public class Renderer
    {
        public static void Render()
        {
            RenderSkybox();
            RenderTiles();
            //TODO: Move this later
            for (int k = 0; k < Main.entities.Count; k++)
            {
                Entity entity = Main.entities[k];
                entity.Draw(Main.spriteBatch);
            }

            // verletEngine.GlobalRenderPoints();
            for (int i = 0; i < Water.WaterBodies.Count; i++)
            {
                Water.WaterBodies[i].Render();
            }

            ShowTileCursor();
            RenderUI();
            Lighting.DrawLightMap();
        }

        static void RenderSkybox()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate);
            Main.spriteBatch.Draw(TextureCache.skybox, Vector2.Zero.AddParralaxAcross(5), Color.White);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(transformMatrix: Main.mainCamera.Transform, samplerState: SamplerState.PointClamp);
        }

        static void RenderUI()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate,BlendState.AlphaBlend);

            for (int i = 0; i < Main.UIScreens.Count; i++)
            {
                Main.UIScreens[i].active = true;
                Main.UIScreens[i].Draw(Main.spriteBatch);
            }
            //debuganthinghere
           // Main.instance.fps.DrawFps(Main.spriteBatch, Main.font, new Vector2(10, 36), Color.Black);
        }
    }
}
