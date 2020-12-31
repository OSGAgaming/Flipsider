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
using static Flipsider.PropManager;
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
            for (int k = 0; k < Main.entities.Count; k++)
            {
                Entity entity = Main.entities[k];
                entity.Draw(Main.spriteBatch);
            }
            for (int i = 0; i < Water.WaterBodies.Count; i++)
            {
                Water.WaterBodies[i].Render();
            }
            RenderTiles(Main.CurrentWorld);
            RenderProps();

          /*if (Main.rand.Next(70) == 0)
            {
                Birbs.Add(new Birb(new Vector2(-80, Main.rand.Next((int)Main.ScreenSize.Y/2)), Main.rand.NextFloat(1f), Main.rand.NextFloat(0.2f,0.5f)));
            }
            for (int i = 0; i < Birbs.Count; i++)
            {
                Main.spriteBatch.Draw(TextureCache.Birb, Birbs[i].position, new Rectangle(0, TextureCache.Birb.Height/4 * Birbs[i].frame, TextureCache.Birb.Width, TextureCache.Birb.Height / 4),Color.White,0f,Vector2.Zero, Birbs[i].scale,SpriteEffects.None,0f);
                Birbs[i].position.X += Birbs[i].speed;
                Birbs[i].framecounter++;
                if (Birbs[i].framecounter % 4/ (Birbs[i].speed/5f) == 0)
                {
                    Birbs[i].frame++;
                    if(Birbs[i].frame > 3)
                    {
                        Birbs[i].frame = 0;
                    }
                }
            }*/
            //TODO: Move this later


            // verletEngine.GlobalRenderPoints();


            ShowTileCursor(Main.CurrentWorld);
            ShowPropCursor();
            RenderUI();
            EditorModes.Draw();
            Lighting.DrawLightMap(Main.CurrentWorld);
        }
        class Birb
        {
            public Vector2 position;
            public int frame;
            public float scale;
            public float framecounter;
            public float speed;
            public Birb(Vector2 pos, float scale, float sped)
            {
                position = pos;
                this.scale = scale;
                speed = sped;
            }
        }
        static void RenderSkybox()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate);
            Rectangle dims = new Rectangle(0, 0, TextureCache.skybox.Width, TextureCache.skybox.Height);
            Main.spriteBatch.Draw(TextureCache.skybox, Vector2.Zero.AddParralaxAcross(10), dims, Color.White,0f, new Vector2(0, TextureCache.skybox.Height/2), 0.7f,SpriteEffects.None,0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(transformMatrix: Main.mainCamera.Transform, samplerState: SamplerState.PointClamp);
        }
        static List<Birb> Birbs = new List<Birb>();
        static void RenderUI()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate,BlendState.AlphaBlend);
            for (int i = 0; i < Main.UIScreens.Count; i++)
            {
                Main.UIScreens[i].active = true;
                Main.UIScreens[i].Draw(Main.spriteBatch);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate,null, transformMatrix: Main.mainCamera.Transform, samplerState: SamplerState.PointClamp);
            //debuganthinghere
            // Main.instance.fps.DrawFps(Main.spriteBatch, Main.font, new Vector2(10, 36), Color.Black);
        }
    }
}
