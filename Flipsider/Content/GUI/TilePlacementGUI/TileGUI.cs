using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using static Flipsider.TileManager;

namespace Flipsider.GUI.TilePlacementGUI
{
    internal class TileGUI : UIScreen
    {
        private TilePanel[]? tilePanel;
        private readonly int rows = 5;
        private readonly int widthOfPanel = 128 / 2;
        private readonly int heightOfPanel = 272 / 2;
        private readonly int paddingX = 5;
        private readonly int paddingY = 20;
        public int chosen = -1;
        public void HideExcept(int index)
        {
            if (index != -1)
            {
                for (int i = 0; i < tilePanel?.Length; i++)
                {
                    if (index != i)
                    {
                        tilePanel[i].alpha -= tilePanel[i].alpha / 16f;
                        tilePanel[i].dimensions.Y += (-200 - tilePanel[i].dimensions.Y) / 16;
                        tilePanel[i].active = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < tilePanel?.Length; i++)
                {
                    if (index != i)
                    {
                        int start = paddingY + (i / rows) * heightOfPanel;
                        tilePanel[i].dimensions.Y += ((start + 16) - tilePanel[i].dimensions.Y) / 16;
                        tilePanel[i].alpha += (1 - tilePanel[i].alpha) / 16f;
                        tilePanel[i].active = true;
                    }
                }
            }
        }
        protected override void OnLoad()
        {
            tilePanel = new TilePanel[Main.tileManager.tileTypes.Count];
            if (tilePanel.Length != 0)
            {
                for (int i = 0; i < tilePanel.Length; i++)
                {
                    Vector2 panelPoint = new Vector2((int)Main.ActualScreenSize.X - widthOfPanel - (i % rows) * (widthOfPanel + paddingX) - paddingX, paddingY + (i / rows) * heightOfPanel);
                    tilePanel[i] = new TilePanel();
                    tilePanel[i].SetDimensions((int)panelPoint.X, (int)panelPoint.Y, widthOfPanel, heightOfPanel);
                    tilePanel[i].startingDimensions = new Rectangle((int)panelPoint.X, (int)panelPoint.Y, widthOfPanel, heightOfPanel);
                    tilePanel[i].tile = Main.tileManager.tileTypes[i];
                    tilePanel[i].parent = this;
                    tilePanel[i].index = i;
                    elements.Add(tilePanel[i]);


                }
            }
            TileGUIButton TGUIB = new TileGUIButton("AutoFrame")
            {
                dimensions = new Rectangle((int)Main.ActualScreenSize.X - 100, 200, 100, 40)
            };
            elements.Add(TGUIB);
        }
        private bool flag = true;
        private bool flag2 = true;
        private bool mouseStateBuffer;
        private Vector2 pos1;
        private Rectangle CopyRect;
        private bool mouseStateBuffer2;
        private Vector2 pos2;
        private Vector2 MouseSnap => Main.MouseScreen.ToVector2().Snap(32);
        protected override void OnUpdate()
        {
            if (Main.Editor.CurrentState == EditorUIState.TileEditorMode)
            {

                HideExcept(chosen);
                if (Mouse.GetState().MiddleButton != ButtonState.Pressed && mouseStateBuffer && !flag)
                {
                    flag = true;
                    Vector2 size = new Vector2((MouseSnap.X - pos1.X) + 4, (MouseSnap.Y - pos1.Y) + 4);
                    for (int i = (int)pos1.X/32; i< pos1.X / 32 + size.X/32 - 1; i++)
                    {
                        for (int j = (int)pos1.Y / 32; j < pos1.Y / 32 + size.Y/32 - 1; j++)
                        {
                            Main.CurrentWorld.tileManager.AddTile(Main.CurrentWorld,new Tile(Main.Editor.currentType,Main.Editor.currentFrame, new Vector2(i, j)));
                        }
                    }
                }
                mouseStateBuffer = Mouse.GetState().MiddleButton == ButtonState.Pressed;
                if (mouseStateBuffer && flag)
                {
                    pos1 = Main.MouseScreen.ToVector2().Snap(32);
                    flag = false;
                }

                if(Keyboard.GetState().IsKeyDown(Keys.LeftControl) && Keyboard.GetState().IsKeyDown(Keys.V))
                {
                    if (Main.Editor.currentTileSet != null)
                    {
                        Vector2 P = Main.MouseScreen.ToVector2().Snap(32);
                        Vector2 size = new Vector2(Main.Editor.currentTileSet.GetLength(0), Main.Editor.currentTileSet.GetLength(1));
                        for (int i = (int)P.X / 32; i < P.X / 32 + size.X; i++)
                        {
                            for (int j = (int)P.Y / 32; j < P.Y / 32 + size.Y; j++)
                            {
                                Tile T = Main.Editor.currentTileSet[i - (int)P.X / 32, j - (int)P.Y / 32];
                                if (T != null)
                                {
                                    Main.CurrentWorld.tileManager.AddTile(Main.CurrentWorld, new Tile(T.type, T.frame, new Vector2(i, j)));
                                }
                            }
                        }
                        Main.Editor.currentTileSet = null;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && Keyboard.GetState().IsKeyDown(Keys.C))
                {
                    flag2 = true;
                    Main.Editor.currentTileSet = new Tile[CopyRect.Size.X, CopyRect.Size.Y];
                    for (int i = CopyRect.Location.X; i < CopyRect.Right; i++)
                    {
                        for (int j = CopyRect.Location.Y; j < CopyRect.Bottom; j++)
                        {
                            Tile T = Main.CurrentWorld.tileManager.GetTile(i,j);
                            if (T != null)
                            {
                                Main.Editor.currentTileSet[i - (int)pos2.X / 32, j - (int)pos2.Y / 32] = T;
                            }
                        }
                    }
                }
                if (!Keyboard.GetState().IsKeyDown(Keys.Space) && mouseStateBuffer2 && !flag2)
                {
                    flag2 = true;
                    Vector2 size = new Vector2((MouseSnap.X - pos2.X) + 4, (MouseSnap.Y - pos2.Y) + 4);
                    CopyRect = new Rectangle((int)pos2.X / 32, (int)pos2.Y / 32, (int)size.X / 32, (int)size.Y / 32);
                }
                mouseStateBuffer2 = Keyboard.GetState().IsKeyDown(Keys.Space);
                if (mouseStateBuffer2 && flag2)
                {
                    pos2 = Main.MouseScreen.ToVector2().Snap(32);
                    flag2 = false;
                }
            }
        }
        internal override void DrawToScreen()
        {
            if (Main.Editor.CurrentState == EditorUIState.TileEditorMode)
            {
                if (CanPlace)
                {
                    var world = Main.CurrentWorld;
                    var tileDict = Main.tileManager.tileDict;
                    int modifiedRes = (int)(tileRes * Main.mainCamera.scale);
                    Vector2 mousePos = Main.MouseScreen.ToVector2();
                    Vector2 tilePoint = new Vector2((int)mousePos.X / tileRes * tileRes, (int)mousePos.Y / tileRes * tileRes);
                    float sine = (float)Math.Sin(Main.gameTime.TotalGameTime.TotalSeconds * 6);
                    Vector2 offsetSnap = new Vector2((int)Main.mainCamera.offset.X, (int)Main.mainCamera.offset.Y);
                    Rectangle TileFrame = Main.Editor.AutoFrame ? Framing.GetTileFrame(world, (int)mousePos.X / tileRes, (int)mousePos.Y / tileRes) : Main.Editor.currentFrame;

                    if (Main.Editor.currentType == -1)
                    {
                        Utils.DrawSquare(tilePoint - offsetSnap, modifiedRes, Color.White * Math.Abs(sine));
                    }
                    else
                    {
                        if (tileDict[Main.Editor.currentType] != null)
                            Main.spriteBatch.Draw(tileDict[Main.Editor.currentType], tilePoint + new Vector2(tileRes / 2, tileRes / 2), TileFrame, Color.White * Math.Abs(sine), 0f, new Vector2(tileRes / 2, tileRes / 2), 1f, SpriteEffects.None, 0f);
                    }
                }
                CanPlace = true;
                Vector2 MouseScreen = Main.MouseScreen.ToVector2().Snap(32);

                if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
                {
                    if (!flag)
                        Utils.DrawRectangle(pos1, (int)(MouseScreen.X - pos1.X) + 4, (int)(MouseScreen.Y - pos1.Y) + 4, Color.White, 3);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    if (!flag2)
                        Utils.DrawRectangle(pos2, (int)(MouseScreen.X - pos2.X) + 4, (int)(MouseScreen.Y - pos2.Y) + 4, Color.Green, 3);
                }
                if (Main.Editor.currentTileSet != null)
                {
                    for (int i = 0; i < Main.Editor.currentTileSet.GetLength(0); i++)
                    {
                        for (int j = 0; j < Main.Editor.currentTileSet.GetLength(1); j++)
                        {
                            Tile tile = Main.Editor.currentTileSet[i, j];
                            if(tile != null)
                            Main.spriteBatch.Draw(Main.CurrentWorld.tileManager.tileDict[tile.type], MouseSnap + new Vector2(i*32,j*32), tile.frame, Color.Green*0.4f);
                        }
                    }
                }
                Utils.DrawRectangle(new Rectangle((CopyRect.Location.ToVector2()*32).ToPoint(), (CopyRect.Size.ToVector2() * 32).ToPoint()), Color.Green * Time.SineTime(6f), 3);
            }
        }
        protected override void OnDraw()
        {
            //   DrawMethods.DrawText("Tiles", Color.BlanchedAlmond, new Vector2((int)Main.ScreenSize.X - 60, paddingY - 10));
        }
    }
    internal class TextButton : UIElement
    {
        public string Text = "";
        protected string ExtraText = "";
        public float alpha;
        public override void Draw(SpriteBatch spriteBatch)
        {
            Utils.DrawTextToLeft(Text + ExtraText, Color.White * alpha, dimensions.Location.ToVector2());
        }
        protected override void OnLeftClick()
        {

        }
    }
    internal class TileGUIButton : TextButton
    {
        public TileGUIButton(string Text)
        {
            this.Text = Text;
        }

        private bool AutoFrame => Main.Editor.AutoFrame;
        protected override void OnLeftClick()
        {
            if (Main.Editor.CurrentState == EditorUIState.TileEditorMode)
            {
                Main.Editor.AutoFrame = !Main.Editor.AutoFrame;
            }
        }
        protected override void OnUpdate()
        {
            ExtraText = AutoFrame ? ":On" : ":Off";
            if (Main.Editor.CurrentState == EditorUIState.TileEditorMode)
            {
                alpha += (1 - alpha) / 16f;
            }
            else
            {
                alpha -= alpha / 16f;
            }
        }
        protected override void OnHover()
        {
            CanPlace = false;
        }
    }
    internal class TilePanel : UIElement
    {
        private readonly int widthOfPanel = 128 / 2;
        private readonly int paddingX = 5;
        public Tile tile = new Tile(0, Rectangle.Empty);
        private float lerpage = 0;
        public Rectangle startingDimensions;
        private bool chosen;
        public int goToPoint = (int)Main.ActualScreenSize.X - 140;
        private Vector2 sizeOfAtlas = new Vector2(128, 272);
        public float alpha = 0;
        private float progression = 0;
        public TileGUI? parent;
        public int index;
        public bool active = true;
        public override void Draw(SpriteBatch spriteBatch)
        {
            int fluff = 2;
            Rectangle panelDims = new Rectangle(dimensions.X - fluff, dimensions.Y - fluff, dimensions.Width + fluff, dimensions.Height + fluff);
            if (!chosen)
            {
                spriteBatch.Draw(TextureCache.TileGUIPanels, panelDims, Color.Lerp(Color.White, Color.Black, lerpage) * alpha);
                if (Main.tileManager.tileDict[tile.type] != null)
                    spriteBatch.Draw(Main.tileManager.tileDict[tile.type], dimensions, Color.Lerp(Color.White, Color.Black, lerpage) * alpha);
            }
            else
            {
                spriteBatch.Draw(TextureCache.TileGUIPanels, panelDims, Color.White * alpha);
                spriteBatch.Draw(Main.tileManager.tileDict[tile.type], dimensions, Color.White * alpha);
                Rectangle chooseArea = dimensions;
                MouseState mousestate = Mouse.GetState();
                int DimTileRes = tileRes / 2;
                if (chooseArea.Contains(mousestate.Position))
                {
                    Vector2 mousePos = new Vector2(mousestate.Position.X, mousestate.Position.Y);
                    Vector2 tilePoint = new Vector2((int)mousePos.X / DimTileRes * DimTileRes, (int)mousePos.Y / DimTileRes * DimTileRes) + new Vector2(dimensions.X % DimTileRes, dimensions.Y % DimTileRes);
                    float sine = (float)Math.Sin(Main.gameTime.TotalGameTime.TotalSeconds * 6);
                    Utils.DrawSquare(tilePoint, DimTileRes, Color.Yellow * Math.Abs(sine) * alpha);
                }
            }
        }
        protected override void OnUpdate()
        {
            if (chosen)
            {
                progression += (1 - progression) / 16f;
            }
            else
            {
                progression -= progression / 16f;

            }
            dimensions.X = (int)MathHelper.Lerp((int)Main.ActualScreenSize.X - widthOfPanel - (index % 5) * (widthOfPanel + paddingX) - paddingX, (int)Main.ActualScreenSize.X - 140, progression);
            dimensions.Width = (int)MathHelper.Lerp(startingDimensions.Width, sizeOfAtlas.X, progression);
            dimensions.Height = (int)MathHelper.Lerp(startingDimensions.Height, sizeOfAtlas.Y, progression);
            if (Main.Editor.CurrentState == EditorUIState.TileEditorMode)
            {
                alpha += (1 - alpha) / 16f;
            }
            else
            {
                alpha -= alpha / 16f;
            }
        }
        protected override void OnLeftClick()
        {
            if (active)
            {
                chosen = true;
                Main.Editor.currentType = tile.type;
                if (chosen)
                {
                    if (parent != null)
                        parent.chosen = index;
                    Rectangle chooseArea = new Rectangle(goToPoint, startingDimensions.Y, 128, 272);
                    int DimTileRes = tileRes / 2;
                    if (chooseArea.Contains(Mouse.GetState().Position))
                    {
                        Vector2 tilePoint = Mouse.GetState().Position.ToVector2().Snap(DimTileRes) + new Vector2(dimensions.X % DimTileRes, dimensions.Y % DimTileRes);
                        Main.Editor.currentFrame = new Rectangle((int)(tilePoint.X - chooseArea.X) * 2, (int)(tilePoint.Y - chooseArea.Y) * 2, tileRes, tileRes);
                    }
                }
            }
        }
        protected override void OnRightClick()
        {
            if (Main.Editor.CurrentState == EditorUIState.TileEditorMode)
            {
                chosen = false;
                if (parent != null)
                    parent.chosen = -1;
            }
        }
        protected override void OnHover()
        {
            CanPlace = false;
            if (active)
            {
                lerpage += (0.5f - lerpage) / 16f;
            }
        }

        protected override void NotOnHover()
        {
            lerpage -= lerpage / 16f;
        }
    }

}
