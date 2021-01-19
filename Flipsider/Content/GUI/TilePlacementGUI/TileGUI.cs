using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Flipsider.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
            Debug.Write(Main.ScreenSize.X);
            TileGUIButton TGUIB = new TileGUIButton("AutoFrame");
            TGUIB.dimensions = new Rectangle((int)Main.ActualScreenSize.X - 100, 200, 100, 40);
            elements.Add(TGUIB);
        }

        protected override void OnUpdate()
        {
            if (Main.Editor.CurrentState == EditorUIState.TileEditorMode)
            {
                HideExcept(chosen);
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
        bool AutoFrame => Main.tileManager.AutoFrame;
        protected override void OnLeftClick()
        {
            if (Main.Editor.CurrentState == EditorUIState.TileEditorMode)
            {
                Main.tileManager.AutoFrame = !Main.tileManager.AutoFrame;
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
        private readonly int heightOfPanel = 272 / 2;
        private readonly int paddingX = 5;
        private readonly int paddingY = 20;
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
                Rectangle chooseArea = new Rectangle(goToPoint, startingDimensions.Y, (int)sizeOfAtlas.X, (int)sizeOfAtlas.Y);
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
                    MouseState mousestate = Mouse.GetState();
                    int DimTileRes = tileRes / 2;
                    if (chooseArea.Contains(mousestate.Position))
                    {
                        Vector2 mousePos = mousestate.Position.ToVector2();
                        Vector2 tilePoint = new Vector2((int)mousePos.X / DimTileRes * DimTileRes, (int)mousePos.Y / DimTileRes * DimTileRes) + new Vector2(dimensions.X % DimTileRes, dimensions.Y % DimTileRes);
                        Main.Editor.currentFrame = new Rectangle(((int)tilePoint.X - chooseArea.X) * 2, ((int)tilePoint.Y - chooseArea.Y) * 2, tileRes, tileRes);
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
