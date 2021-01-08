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
        protected override void OnLoad()
        {
            active = true;
            StartGame SGB1 = new StartGame(TextureCache.SaveTex, Main.ScreenCenterUI + new Vector2(0, 50));
            elements.Add(SGB1);
            TitleScreen SGB4 = new TitleScreen(TextureCache.SaveTex, Main.ScreenCenterUI + new Vector2(0, -200));
            elements.Add(SGB4);
        }

        protected override void OnUpdate()
        {
        }
        float alpha;
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
                DrawMethods.DrawBoxFill(Vector2.Zero, 1980, 1080, Color.White * alpha);
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
        Texture2D tex;
        Vector2 Center;
        public TitleScreen(Texture2D tex, Vector2 Center)
        {
            this.tex = tex;
            this.Center = Center;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if(alpha > 0.01f)
            DrawMethods.DrawText("Flipsider I think. Name May change", Color.Black*alpha, Center);
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
        float alpha = 1;
        Texture2D tex;
        Vector2 Center;
        public StartGame(Texture2D tex, Vector2 Center)
        {
            this.tex = tex;
            this.Center = Center;
            dimensions.X = (int)(Center.X - tex.Width / 2);
            dimensions.Y = (int)(Center.Y - tex.Height / 2);
            dimensions.Width = 100;
            dimensions.Height = 20;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (alpha > 0.01f)
                DrawMethods.DrawText("Start Game idk",Color.Black*alpha,Center);
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
        protected override void OnLeftClick()
        {
            Main.instance.sceneManager.SetNextScene(new ForestArea(), null, true);
        }
    }
}



