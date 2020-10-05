using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Flipsider.Engine.Assets;
using Flipsider.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Flipsider.NPC;
using static Flipsider.TileManager;

namespace Flipsider.GUI.TilePlacementGUI
{

    class NPCGUI : UIScreen
    {
        NPCPanel[] tilePanel;
        int rows = 5;
        int widthOfPanel = 64;
        int heightOfPanel = 64;
        int paddingX = 5;
        int paddingY = 20;
        public int chosen = -1;
        public NPCGUI()
        {
            Main.UIScreens.Add(this);
            tilePanel = new NPCPanel[NPCTypes.Length];
            if (tilePanel.Length != 0)
            {
                for (int i = 0; i < tilePanel.Length; i++)
                {
                    Vector2 panelPoint = new Vector2((int)Main.ScreenSize.X - widthOfPanel - (i % rows) * (widthOfPanel + paddingX) - paddingX, paddingY + (i / rows) * heightOfPanel);
                    tilePanel[i] = new NPCPanel();
                    tilePanel[i].SetDimensions((int)panelPoint.X, (int)panelPoint.Y, widthOfPanel, heightOfPanel);
                    tilePanel[i].startingDimensions = new Rectangle((int)panelPoint.X, (int)panelPoint.Y, widthOfPanel, heightOfPanel);
                    tilePanel[i].npc = NPCTypes[i];
                    elements.Add(tilePanel[i]);
                }
            }
        }

        protected override void OnUpdate()
        {
        }
        protected override void OnDraw()
        {

        }
    }
   
    class NPCPanel : UIElement
    {
        public NPCInfo npc;
        float lerpage = 0;
        public Rectangle startingDimensions;
        bool chosen;
        public int goToPoint = (int)Main.ScreenSize.X - 140;
        public float alpha = 0;
        float progression = 0;
        public bool active = true;
        Texture2D? tex;
        public override void Draw(SpriteBatch spriteBatch)
        {
            tex ??= npc.type.GetField("icon")?.GetValue(null) as Texture2D;
            if (chosen)
            {
                progression += (1 - progression) / 16f;
            }
            else
            {
                progression -= progression / 16f;

            }
            if (EditorModes.NPCSpawnerMode)
            {
                alpha += (1 - alpha) / 16f;
            }
            else
            {
                alpha -= alpha / 16f;
            }
            int fluff  = 1;
            Rectangle panelDims = new Rectangle(dimensions.X - fluff, dimensions.Y - fluff, dimensions.Width + fluff*2, dimensions.Height + fluff*2);
            spriteBatch.Draw(AssetManager.NPCPanel, panelDims, Color.Lerp(Color.White, Color.Black, lerpage) * alpha);
            spriteBatch.Draw(tex, dimensions, Color.Lerp(Color.White, Color.Black, lerpage) * alpha);
        }
        protected override void OnUpdate()
        {

        }
        protected override void OnLeftClick()
        {
            SpawnNPC(Main.player.position + new Vector2(0,-200), npc.type);
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
