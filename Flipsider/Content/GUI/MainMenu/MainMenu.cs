using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Flipsider.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using static Flipsider.NPC;
using static Flipsider.TileManager;
using System.Windows.Input;
using Flipsider.Engine.Input;
using System.IO;
using Flipsider.Scenes;

namespace Flipsider.GUI.TilePlacementGUI
{
    internal class MainMenuUI : UIScreen
    {
        public float progression;
        protected override void OnLoad()
        {
            active = true;
            StartGame SGB1 = new StartGame(TextureCache.MainMenuPanel, new Vector2(190, Main.ScreenCenterUI.Y), T+80);
            SGB1.parent = this;
            elements.Add(SGB1);
            StartGame SGB2 = new StartGame(TextureCache.MainMenuPanel, new Vector2(190, Main.ScreenCenterUI.Y + 50), T + 90);
            SGB2.parent = this;
            elements.Add(SGB2);
            StartGame SGB3 = new StartGame(TextureCache.MainMenuPanel, new Vector2(190, Main.ScreenCenterUI.Y + 100), T + 100);
            SGB3.parent = this;
            elements.Add(SGB3);
            StartGame SGB4 = new StartGame(TextureCache.MainMenuPanel, new Vector2(190, Main.ScreenCenterUI.Y + 150), T + 110);
            SGB4.parent = this;
            elements.Add(SGB4);
        }
        int T = 10;
        protected override void OnUpdate()
        {
            progression++;
            if(progression < T)
            {
                lerp = lerp.ReciprocateTo(1, 64);
                colorOfLine = new Color(16, 20, 49);
            }
            if (progression.IsBetween(T, T+7))
            {
                lerp = lerp.ReciprocateTo(0, 3);
                colorOfLine = Color.White;
                widthOfLeftPanel = widthOfLeftPanel.ReciprocateTo(390, 200);
            }
            if (progression.IsBetween(T+7, T+10))
            {
                lerp = lerp.ReciprocateTo(1, 20);
                colorOfLine = Color.Lerp(Color.White,new Color(16, 20, 49),lerp*2);
                widthOfLeftPanel = widthOfLeftPanel.ReciprocateTo(390, 16);
            }
            if (progression > T+10 && progression < 500)
            {

                lerp = lerp.ReciprocateTo(1, 20);
                colorOfLine = Color.Lerp(Color.White, new Color(16, 20, 49), lerp * 2);
                widthOfLeftPanel = widthOfLeftPanel.ReciprocateTo(390, 16);
            }
        }
        float alpha;
        float widthOfLeftPanel;
        float titleStreak = 20;
        float lerp;
        Color colorOfLine; 
        protected override void OnDraw()
        {
            if (Main.CurrentScene.Name == "Main Menu")
            {
                alpha = 1;
            }
            else if (alpha > 0.005f)
            {
                alpha += -alpha / 16f;
            }
            if (alpha > 0.01f)
            {
                Main.mainCamera.offset.Y += (12 - Main.mainCamera.offset.Y) / 16f;
                Utils.DrawBoxFill(Vector2.Zero, 1980, 1080, Color.Lerp(Color.Black,Color.White,lerp) * alpha);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, transformMatrix: Main.mainCamera.Transform, samplerState: SamplerState.PointClamp);
                Utils.RenderBG(Main.spriteBatch, Color.Lerp(Color.Black, Color.White, lerp) * alpha, TextureCache.skybox, -0.9f, 0.4f, new Vector2(-200, lerp + Utils.BOTTOM));
                Utils.RenderBG(Main.spriteBatch, Color.Lerp(Color.Black, Color.White, lerp) * alpha, TextureCache.ForestBackground3, -0.6f, 0.4f, new Vector2(-900, Utils.BOTTOM -1300), - 0.6f);
                Utils.RenderBG(Main.spriteBatch, Color.Lerp(Color.Black, Color.White, lerp) * alpha, TextureCache.ForestBackground2, -0.5f, 0.4f, new Vector2(-900, Utils.BOTTOM - 1300), -0.5f);
                Utils.RenderBG(Main.spriteBatch, Color.Lerp(Color.Black, Color.White, lerp) * alpha, TextureCache.ForestBackground1, -0.4f, 0.4f, new Vector2(-900, Utils.BOTTOM -1300), -0.4f);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                Utils.DrawBoxFill(new Vector2(200 - widthOfLeftPanel / 2, 0), (int)widthOfLeftPanel, (int)Main.ScreenSize.Y, colorOfLine * alpha);
            }
           
            if (progression > T + 50)
            {
                titleStreak = titleStreak.ReciprocateTo(0,15);
                Utils.DrawBoxFill(new Vector2(0, 120 - titleStreak / 2), (int)Main.ScreenSize.X, (int)titleStreak, Color.White * alpha);
                Main.spriteBatch.Draw(TextureCache.TitleScreen, new Rectangle(10, 20, 385, 200), Color.White * alpha);
              Main.spriteBatch.Draw(TextureCache.TitleScreenOverlay, new Rectangle(10, 20, 385, 200),Color.White * (titleStreak/10f * alpha));
            }

        }
    }
    internal class BasicUI : UIElement
    {
        float alpha;
        Texture2D tex;
        Vector2 Center;
        public BasicUI(Texture2D tex, Vector2 Center)
        {
            this.tex = tex;
            this.Center = Center;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Main.spriteBatch.Draw(tex, new Rectangle((int)(Center.X - tex.Width / 2), (int)(Center.Y - tex.Height / 2), tex.Width, tex.Height), Color.White * alpha);
        }
    }
    internal class TitleScreen : UIElement
    {
        float alpha = 1;
        Texture2D tex = TextureCache.TitleScreen;
        Vector2 Center;
        public TitleScreen(Vector2 Center)
        {
            this.Center = Center;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
          //  if (alpha > 0.01f)
              //  Main.spriteBatch.Draw(tex, new Rectangle((int)(Center.X - tex.Width / 2), (int)(Center.Y - tex.Height / 2), tex.Width, tex.Height), Color.White * alpha);
        }
        protected override void OnUpdate()
        {
            if (Main.CurrentScene.Name == "Main Menu")
            {
                alpha = 1;
            }
            else if (alpha > 0.005f)
            {
                alpha += -alpha / 16f;
            }
        }

    }
    internal class StartGame : UIElement
    {
        public MainMenuUI? parent;
        float alpha = 0;
        Texture2D tex;
        Vector2 Center;
        public int Trigger;
        public StartGame(Texture2D tex, Vector2 Center, int Trigger)
        {
            this.Trigger = Trigger;
            this.tex = tex;
            this.Center = Center;
            dimensions.X = (int)(Center.X - tex.Width / 2);
            dimensions.Y = (int)(Center.Y - tex.Height / 2);
            dimensions.Width = TextureCache.MainMenuPanelOverlay.Width;
            dimensions.Height = TextureCache.MainMenuPanelOverlay.Height;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (parent?.progression > Trigger && Main.CurrentScene.Name == "Main Menu")
            {
                Utils.DrawBoxFill(Center - new Vector2(150,-5 - ((1 - alpha) * 5) / 2), 300, (int)((1 - alpha) * 5), Color.White * alpha);
            }
            if (alpha > 0.01f)
            {
                Main.spriteBatch.Draw(tex, new Rectangle((int)(Center.X - tex.Width / 2), (int)(Center.Y - tex.Height / 2), tex.Width, tex.Height), Color.White * alpha);
                if(parent?.progression < 200)
                Main.spriteBatch.Draw(TextureCache.MainMenuPanelOverlay, new Rectangle((int)(Center.X - tex.Width / 2), (int)(Center.Y - tex.Height / 2), tex.Width, tex.Height), Color.White * (1 - alpha));
            }
        }
        protected override void OnUpdate()
        {
            if (parent?.progression > Trigger && Main.CurrentScene.Name == "Main Menu")
            {
                alpha = alpha.ReciprocateTo(1, 3);
            }
            else if (alpha > 0.005f)
            {
                alpha += -alpha / 16f;
            }

        }
        protected override void OnLeftClick()
        {
            Main.instance.sceneManager.SetNextScene(new ForestArea(), null, true);
        }
    }
}



