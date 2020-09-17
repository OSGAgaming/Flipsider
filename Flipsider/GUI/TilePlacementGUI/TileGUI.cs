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
        int widthOfPanel = 128/4;
        int heightOfPanel = 272/4;
        int padding = 20;
        public TileGUI()
        {
            tilePanel = new TilePanel[tileTypes.Count];
            if (tilePanel.Length != 0)
            {
                for(int i = 0; i< tilePanel.Length;i++)
                {
                    Vector2 panelPoint = new Vector2((int)Main.ScreenSize.X - widthOfPanel * (i + 1) + (i % rows) * widthOfPanel - padding, padding + (i / rows) * heightOfPanel);
                    tilePanel[i] = new TilePanel();
                    tilePanel[i].SetDimensions((int)panelPoint.X, (int)panelPoint.Y, widthOfPanel, heightOfPanel);
                    tilePanel[i].startingPoint = panelPoint;
                    tilePanel[i].tile = tileTypes[i];
                    elements.Add(tilePanel[i]);
                }
            }
        }

        protected override void OnUpdate()
        {
        }
    }

    class TilePanel : UIElement
    {
        public Tile tile;
        float lerpage = 0;
        bool chosen;
        public Vector2 startingPoint;
        public int goToPoint = (int)Main.ScreenSize.X - 140;
        Vector2 sizeOfAtlas = new Vector2(128, 272);
        float alpha = 0;
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (chosen)
            {
                dimensions.X += (goToPoint - dimensions.X) / 16;
                dimensions.Width += ((int)sizeOfAtlas.X - dimensions.Width) / 16;
                dimensions.Height += ((int)sizeOfAtlas.Y - dimensions.Height) / (int)(16 * (sizeOfAtlas.X/ sizeOfAtlas.Y));
            }
            else
            {
                dimensions.X += (int)(startingPoint.X - dimensions.X) / 16;
                dimensions.Width += ((int)sizeOfAtlas.X / 4 - dimensions.Width) / 16;
                dimensions.Height+= ((int)sizeOfAtlas.Y / 4 - dimensions.Height) / 16;
            }
            if (Main.TileEditorMode)
            {
                alpha += (1 - alpha) / 16f;
            }
            else
            {
                alpha -= alpha / 16f;
            }
            if (tile.atlas != null)
            {

                if (!chosen)
                {
                    spriteBatch.Draw(tile.atlas, dimensions, Color.Lerp(Color.White, Color.Black, lerpage)* alpha);
                }
                else
                {
                    spriteBatch.Draw(tile.atlas, dimensions, Color.White* alpha);
                    Rectangle chooseArea = new Rectangle(goToPoint, (int)startingPoint.Y, (int)sizeOfAtlas.X, (int)sizeOfAtlas.Y);
                    MouseState mousestate = Mouse.GetState();
                    int DimTileRes = tileRes / 2;
                    if (chooseArea.Contains(mousestate.Position))
                    {
                        Vector2 mousePos = new Vector2(mousestate.Position.X, mousestate.Position.Y);
                        Vector2 tilePoint = new Vector2((int)mousePos.X / DimTileRes * DimTileRes, (int)mousePos.Y / DimTileRes * DimTileRes) + new Vector2(dimensions.X % DimTileRes, dimensions.Y % DimTileRes);
                        float sine = (float)Math.Sin(Main.gameTime.TotalGameTime.TotalSeconds * 6);
                        DrawMethods.DrawSquare(tilePoint, DimTileRes, Color.Yellow * Math.Abs(sine)*alpha);
                    }
                }
            }
        }
        protected override void OnUpdate()
        {
            
        }
        protected override void OnLeftClick()
        {
            chosen = true;
            Main.currentAtlas = tile.atlas;
            if(chosen)
            {
                Rectangle chooseArea = new Rectangle(goToPoint, (int)startingPoint.Y, 128, 272);
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
        protected override void OnRightClick()
        {
            chosen = false;
        }
        protected override void OnHover()
        {
            lerpage += (0.5f - lerpage) / 16f;
        }

        protected override void NotOnHover()
        {
            lerpage -= lerpage / 16f;
        }
    }

}
