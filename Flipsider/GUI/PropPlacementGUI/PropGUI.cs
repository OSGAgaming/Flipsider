using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Flipsider.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Flipsider.PropManager;

namespace Flipsider.GUI.TilePlacementGUI
{
    class PropGUI : UIScreen
    {

        PropPanel[] tilePanel;
        int rows = 5;
        int widthOfPanel = 64;
        int heightOfPanel = 64;
        int paddingX = 5;
        int paddingY = 20;
        public int chosen = -1;
        public PropGUI()
        {
            Main.UIScreens.Add(this);
            tilePanel = new PropPanel[PropTypes.Count];
            if (tilePanel.Length != 0)
            {
                for (int i = 0; i < tilePanel.Length; i++)
                {
                    Vector2 panelPoint = new Vector2((int)Main.ScreenSize.X - widthOfPanel - (i % rows) * (widthOfPanel + paddingX) - paddingX, paddingY + (i / rows) * (heightOfPanel + paddingY));
                    tilePanel[i] = new PropPanel();
                    tilePanel[i].SetDimensions((int)panelPoint.X, (int)panelPoint.Y, widthOfPanel, heightOfPanel);
                    tilePanel[i].startingDimensions = new Rectangle((int)panelPoint.X, (int)panelPoint.Y, widthOfPanel, heightOfPanel);
                    tilePanel[i].parent = this;
                    tilePanel[i].index = i;
                    elements.Add(tilePanel[i]);
                }
            }
        }

        protected override void OnUpdate()
        {

        }
        protected override void OnDraw()
        {
          //DrawMethods.DrawText("Tiles", Color.BlanchedAlmond, new Vector2((int)Main.ScreenSize.X - 60, paddingY - 10));
        }
    }
        class PropPanel : UIElement
    {
        float lerpage = 0;
        public Rectangle startingDimensions;
        public int goToPoint = (int)Main.ScreenSize.X - 140;
        Vector2 sizeOfAtlas = new Vector2(128, 272);
        public float alpha = 0;
        float progression = 0;
        public PropGUI? parent;
        public int index;
        public bool active = true;
        public override void Draw(SpriteBatch spriteBatch)
        {

            dimensions.X = (int)MathHelper.Lerp(startingDimensions.X, goToPoint, progression);
            dimensions.Width = (int)MathHelper.Lerp(startingDimensions.Width, sizeOfAtlas.X, progression);
            dimensions.Height = (int)MathHelper.Lerp(startingDimensions.Height, sizeOfAtlas.Y, progression);
            if (EditorModes.CurrentState == EditorUIState.PropEditorMode)
            {
                alpha += (1 - alpha) / 16f;
            }
            else
            {
                alpha -= alpha / 16f;
            }
            int fluff = 5;
            Rectangle panelDims = new Rectangle(dimensions.X - fluff, dimensions.Y - fluff, dimensions.Width + fluff*2, dimensions.Height + fluff*2);
            spriteBatch.Draw(TextureCache.NPCPanel, panelDims, Color.Lerp(Color.White, Color.Black, lerpage) * alpha);
            spriteBatch.Draw(PropTypes.Values.ToArray()[index], dimensions, Color.Lerp(Color.White, Color.Black, lerpage) * alpha);
        }
        protected override void OnUpdate()
        {

        }
        protected override void OnLeftClick()
        {
            TileManager.UselessCanPlaceBool = false;
            CurrentProp = PropTypes.Keys.ToArray()[index];
        }
        protected override void OnRightClick()
        {
            if(parent != null)
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
