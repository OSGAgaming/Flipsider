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
    class TileGUI : UIScreen
    {

        TilePanel[] tilePanel;
        int rows = 5;
        int widthOfPanel = 128 / 2;
        int heightOfPanel = 272 / 2;
        int paddingX = 5;
        int paddingY = 20;
        public int chosen = -1;
        public void HideExcept(int index)
        {
            if (index != -1)
            {
                for (int i = 0; i < tilePanel.Length; i++)
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
                for (int i = 0; i < tilePanel.Length; i++)
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
        public TileGUI()
        {
            Main.UIScreens.Add(this);
            tilePanel = new TilePanel[tileTypes.Count];
            if (tilePanel.Length != 0)
            {
                for (int i = 0; i < tilePanel.Length; i++)
                {
                    Vector2 panelPoint = new Vector2((int)Main.ScreenSize.X - widthOfPanel - (i % rows) * (widthOfPanel + paddingX) - paddingX, paddingY + (i / rows) * heightOfPanel);
                    tilePanel[i] = new TilePanel();
                    tilePanel[i].SetDimensions((int)panelPoint.X, (int)panelPoint.Y, widthOfPanel, heightOfPanel);
                    tilePanel[i].startingDimensions = new Rectangle((int)panelPoint.X, (int)panelPoint.Y, widthOfPanel, heightOfPanel);
                    tilePanel[i].tile = tileTypes[i];
                    tilePanel[i].parent = this;
                    tilePanel[i].index = i;
                    elements.Add(tilePanel[i]);

                    
                }
            }
        }

        protected override void OnUpdate()
        {
            HideExcept(chosen);
        }
        protected override void OnDraw()
        {
            //   DrawMethods.DrawText("Tiles", Color.BlanchedAlmond, new Vector2((int)Main.ScreenSize.X - 60, paddingY - 10));
        }
    }
        class TilePanel : UIElement
    {
        public Tile tile;
        float lerpage = 0;
        public Rectangle startingDimensions;
        bool chosen;
        public int goToPoint = (int)Main.ScreenSize.X - 140;
        Vector2 sizeOfAtlas = new Vector2(128, 272);
        public float alpha = 0;
        float progression = 0;
        public TileGUI parent;
        public int index;
        public bool active = true;
        public override void Draw(SpriteBatch spriteBatch)
        {

            if (chosen)
            {
                progression += (1 - progression) / 16f;
            }
            else
            {
                progression -= progression / 16f;

            }
            dimensions.X = (int)MathHelper.Lerp(startingDimensions.X, goToPoint, progression);
            dimensions.Width = (int)MathHelper.Lerp(startingDimensions.Width, sizeOfAtlas.X, progression);
            dimensions.Height = (int)MathHelper.Lerp(startingDimensions.Height, sizeOfAtlas.Y, progression);
            if (EditorModes.TileEditorMode)
            {
                alpha += (1 - alpha) / 16f;
            }
            else
            {
                alpha -= alpha / 16f;
            }
            int fluff = 2;
            Rectangle panelDims = new Rectangle(dimensions.X - fluff, dimensions.Y - fluff, dimensions.Width + fluff, dimensions.Height + fluff);
            if (!chosen)
            {
                spriteBatch.Draw(TextureCache.TileGUIPanels, panelDims, Color.Lerp(Color.White, Color.Black, lerpage) * alpha);
                spriteBatch.Draw(tileDict[tile.type], dimensions, Color.Lerp(Color.White, Color.Black, lerpage) * alpha);
            }
            else
            {
                spriteBatch.Draw(TextureCache.TileGUIPanels, panelDims, Color.White * alpha);
                spriteBatch.Draw(tileDict[tile.type], dimensions, Color.White * alpha);
                Rectangle chooseArea = new Rectangle(goToPoint, (int)startingDimensions.Y, (int)sizeOfAtlas.X, (int)sizeOfAtlas.Y);
                MouseState mousestate = Mouse.GetState();
                int DimTileRes = tileRes / 2;
                if (chooseArea.Contains(mousestate.Position))
                {
                    Vector2 mousePos = new Vector2(mousestate.Position.X, mousestate.Position.Y);
                    Vector2 tilePoint = new Vector2((int)mousePos.X / DimTileRes * DimTileRes, (int)mousePos.Y / DimTileRes * DimTileRes) + new Vector2(dimensions.X % DimTileRes, dimensions.Y % DimTileRes);
                    float sine = (float)Math.Sin(Main.gameTime.TotalGameTime.TotalSeconds * 6);
                    DrawMethods.DrawSquare(tilePoint, DimTileRes, Color.Yellow * Math.Abs(sine) * alpha);
                }
            }
        }
        protected override void OnUpdate()
        {

        }
        protected override void OnLeftClick()
        {
            if (active)
            {
                chosen = true;
                Main.currentType = tile.type;
                if (chosen)
                {
                    parent.chosen = index;
                    Rectangle chooseArea = new Rectangle(goToPoint, (int)startingDimensions.Y, 128, 272);
                    MouseState mousestate = Mouse.GetState();
                    int DimTileRes = tileRes / 2;
                    if (chooseArea.Contains(mousestate.Position))
                    {
                        Vector2 mousePos = new Vector2(mousestate.Position.X, mousestate.Position.Y);
                        Vector2 tilePoint = new Vector2((int)mousePos.X / DimTileRes * DimTileRes, (int)mousePos.Y / DimTileRes * DimTileRes) + new Vector2(dimensions.X % DimTileRes, dimensions.Y % DimTileRes);
                        Main.currentFrame = new Rectangle((int)tilePoint.X - chooseArea.X, (int)tilePoint.Y - chooseArea.Y, tileRes, tileRes);
                    }
                }
            }
        }
        protected override void OnRightClick()
        {
            chosen = false;
            parent.chosen = -1;
        }
        protected override void OnHover()
        {
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
