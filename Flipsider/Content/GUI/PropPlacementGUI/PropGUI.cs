using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using static Flipsider.PropManager;

namespace Flipsider.GUI.TilePlacementGUI
{
    internal class PropGUI : UIScreen
    {
        private PropPanel[]? tilePanel;
        private readonly int rows = 5;
        private readonly int widthOfPanel = 64;
        private readonly int heightOfPanel = 64;
        private readonly int paddingX = 5;
        private readonly int paddingY = 20;
        public int chosen = -1;
        protected override void OnLoad()
        {
            tilePanel = new PropPanel[PropTypes.Count];
            if (tilePanel.Length != 0)
            {
                for (int i = 0; i < tilePanel.Length; i++)
                {
                    Vector2 panelPoint = new Vector2((int)Main.ActualScreenSize.X - widthOfPanel - (i % rows) * (widthOfPanel + paddingX) - paddingX, paddingY + (i / rows) * (heightOfPanel + paddingY));
                    tilePanel[i] = new PropPanel();
                    tilePanel[i].SetDimensions((int)panelPoint.X, (int)panelPoint.Y, widthOfPanel, heightOfPanel);
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

    internal class PropPanel : UIElement
    {
        private Vector2 panelPoint => new Vector2((int)Main.ActualScreenSize.X - dimensions.Width - (index % 5) * (dimensions.Width + 5) - 5, 20 + (index / 5) * (dimensions.Height + 20));
        private float lerpage = 0;
        public Rectangle startingDimensions => new Rectangle((int)panelPoint.X, (int)panelPoint.Y, dimensions.Width, dimensions.Height);
        public int goToPoint => (int)Main.ActualScreenSize.X - 140;
        private Vector2 sizeOfAtlas = new Vector2(128, 272);
        public float alpha = 0;
        private readonly float progression = 0;
        public PropGUI? parent;
        public int index;
        public bool active = true;
        public override void Draw(SpriteBatch spriteBatch)
        {
            int fluff = 5;
            Rectangle panelDims = new Rectangle(dimensions.X - fluff, dimensions.Y - fluff, dimensions.Width + fluff * 2, dimensions.Height + fluff * 2);
            spriteBatch.Draw(TextureCache.NPCPanel, panelDims, Color.Lerp(Color.White, Color.Black, lerpage) * alpha);
            spriteBatch.Draw(PropTypes.Values.ToArray()[index], dimensions, Color.Lerp(Color.White, Color.Black, lerpage) * alpha);
        }
        protected override void OnUpdate()
        {
            dimensions.X = (int)MathHelper.Lerp(startingDimensions.X, goToPoint, progression);
            dimensions.Width = (int)MathHelper.Lerp(startingDimensions.Width, sizeOfAtlas.X, progression);
            dimensions.Height = (int)MathHelper.Lerp(startingDimensions.Height, sizeOfAtlas.Y, progression);
            if (Main.Editor.CurrentState == EditorUIState.PropEditorMode)
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
            Main.Editor.CurrentProp = PropTypes.Keys.ToArray()[index];
        }
        protected override void OnRightClick()
        {
            if (parent != null)
                parent.chosen = -1;
        }
        protected override void OnHover()
        {
            if (active)
            {
                TileManager.UselessCanPlaceBool = false;
                lerpage += (0.5f - lerpage) / 16f;
            }
        }

        protected override void NotOnHover()
        {
            lerpage -= lerpage / 16f;
        }
    }

}
