using Flipsider.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flipsider.GUI.TilePlacementGUI
{
    internal class MainMenuUI : UIScreen
    {
        public float progression;
        protected override void OnLoad()
        {
            active = true;
            StartGame SGB1 = new StartGame(TextureCache.MainMenuPanel, new Vector2(190, Main.ScreenCenterUI.Y), (int)T + 80,0)
            {
                parent = this
            };
            elements.Add(SGB1);
            StartGame SGB2 = new StartGame(TextureCache.MainMenuPanel, new Vector2(190, Main.ScreenCenterUI.Y), (int)T + 90, 1)
            {
                parent = this
            };
            elements.Add(SGB2);
            StartGame SGB3 = new StartGame(TextureCache.MainMenuPanel, new Vector2(190, Main.ScreenCenterUI.Y), (int)T + 100, 2)
            {
                parent = this
            };
            elements.Add(SGB3);
            StartGame SGB4 = new StartGame(TextureCache.MainMenuPanel, new Vector2(190, Main.ScreenCenterUI.Y), (int)T + 110, 3)
            {
                parent = this
            };
            elements.Add(SGB4);
        }

        private float T = 10;
        protected override void OnUpdate()
        {
            progression += Time.DeltaVar(60);
            if (progression < T)
            {
                lerp = lerp.ReciprocateTo(1, 64);
                colorOfLine = new Color(16, 20, 49);
            }
            if (progression.IsBetween(T, T + 7))
            {
                lerp = lerp.ReciprocateTo(0, 3);
                colorOfLine = Color.White;
                widthOfLeftPanel = widthOfLeftPanel.ReciprocateTo(390, 200);
            }
            if (progression.IsBetween(T + 7, T + 10))
            {
                lerp = lerp.ReciprocateTo(1, 20);
                colorOfLine = Color.Lerp(Color.White, new Color(16, 20, 49), lerp * 2);
                widthOfLeftPanel = widthOfLeftPanel.ReciprocateTo(390, 16);
            }
            if (progression > T + 10 && progression < 500)
            {

                lerp = lerp.ReciprocateTo(1, 20);
                colorOfLine = Color.Lerp(Color.White, new Color(16, 20, 49), lerp * 2);
                widthOfLeftPanel = widthOfLeftPanel.ReciprocateTo(390, 16);
            }
        }

        private float alpha;
        private float widthOfLeftPanel;
        private float titleStreak = 20;
        private float lerp;
        private Color colorOfLine;
        float scaling => Main.ActualScreenSize.X / Main.ScreenSize.X;

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
                Main.mainCamera.Offset.Y += (12 - Main.mainCamera.Offset.Y) / 16f;
                Utils.DrawBoxFill(Vector2.Zero, 1980, 1080, Color.Lerp(Color.Black, Color.White, lerp) * alpha);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, transformMatrix: Main.mainCamera.Transform, samplerState: SamplerState.PointClamp);
                Utils.RenderBG(Main.spriteBatch, Color.Lerp(Color.Black, Color.White, lerp) * alpha, TextureCache.skybox, -0.9f, 0.4f* scaling, new Vector2(-200, lerp + Utils.BOTTOM));
                Utils.RenderBG(Main.spriteBatch, Color.Lerp(Color.Black, Color.White, lerp) * alpha, TextureCache.ForestBackground3, -0.6f, 0.4f * scaling, new Vector2(-900, Utils.BOTTOM - 1350 / scaling), -0.6f);
                Utils.RenderBG(Main.spriteBatch, Color.Lerp(Color.Black, Color.White, lerp) * alpha, TextureCache.ForestBackground2, -0.5f, 0.4f * scaling, new Vector2(-900, Utils.BOTTOM - 1350 / scaling), -0.5f);
                Utils.RenderBG(Main.spriteBatch, Color.Lerp(Color.Black, Color.White, lerp) * alpha, TextureCache.ForestBackground1, -0.4f, 0.4f * scaling, new Vector2(-900, Utils.BOTTOM - 1350 / scaling), -0.4f);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);
                Utils.DrawBoxFill(new Vector2(0, 0), (int)(widthOfLeftPanel * scaling), (int)Main.ActualScreenSize.Y, colorOfLine * alpha);
            }


            if (progression > T + 50)
            {
                titleStreak = titleStreak.ReciprocateTo(0, 15);
                Utils.DrawBoxFill(new Vector2(0, 120 - titleStreak / 2), (int)Main.ActualScreenSize.X, (int)titleStreak, Color.White * alpha);
                Main.spriteBatch.Draw(TextureCache.TitleScreen, new Rectangle(((int)(widthOfLeftPanel * scaling) - 385)/2, 20, 385, 200), Color.White * alpha);
                Main.spriteBatch.Draw(TextureCache.TitleScreenOverlay, new Rectangle(((int)(widthOfLeftPanel * scaling) - 385) / 2, 20, 385,200), Color.White * (titleStreak / 10f * alpha));
            }

        }
    }
    internal class BasicUI : UIElement
    {
        private float alpha;
        private Texture2D tex;
        private Vector2 Center;
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
        private float alpha = 1;
        private Texture2D tex = TextureCache.TitleScreen;
        private Vector2 Center;
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
        private float alpha = 0;
        private Texture2D tex;
        private Vector2 Center;
        public int Trigger;
        public int Index;
        float scaling => Main.ActualScreenSize.X / Main.ScreenSize.X;

        public StartGame(Texture2D tex, Vector2 Center, int Trigger, int Index)
        {
            this.Index = Index;
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
            Center.X = (390 * scaling - tex.Width) / 2;
            if (parent?.progression > Trigger && Main.CurrentScene.Name == "Main Menu")
            {
              //  Utils.DrawBoxFill(Center - new Vector2(150 - tex.Width/2, -5 - ((1 - alpha) * 5) / 2), 300, (int)((1 - alpha) * 5), Color.White * alpha);
            }
            if (alpha > 0.01f)
            {
                Rectangle source = new Rectangle((int)Center.X, (int)(Center.Y - tex.Height / 2)  + (Index * 50), tex.Width, tex.Height);
                Main.spriteBatch.Draw(tex, source, Color.White * alpha);
                if (parent?.progression < 200)
                    Main.spriteBatch.Draw(TextureCache.MainMenuPanelOverlay, source, Color.White * (1 - alpha));
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



