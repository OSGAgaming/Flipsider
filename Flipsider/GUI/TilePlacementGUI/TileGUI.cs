using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Flipsider.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flipsider.GUI.TilePlacementGUI
{
    class TileGUI : UIScreen
    {

        TilePanel[] tilePanel;
        int rows = 5;
        public TileGUI()
        {
            tilePanel = new TilePanel[TileManager.tileTypes.Count];
            if (tilePanel.Length != 0)
            {
                for(int i = 0; i< tilePanel.Length;i++)
                {
                    tilePanel[i] = new TilePanel();
                    tilePanel[i].SetDimensions(10 + (i%rows)*32, 10 + (i/ rows) * 32,32,32);
                    tilePanel[i].tex = TileManager.tileTypes[i].atlas;
                    elements.Add(tilePanel[i]);
                }
            }
        }

        protected override void OnUpdate()
        {
            if (tilePanel.Length != 0)
            {
                for (int i = 0; i < tilePanel.Length; i++)
                {
                    tilePanel[i].SetDimensions(10 + (i % rows) * 32, 10 + (i / rows) * 32, 32, 32);
                    tilePanel[i].tex = TileManager.tileTypes[i].atlas;
                }
            }
        }
    }

    class TilePanel : UIElement
    {
        public Texture2D tex;
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (tex != null)
            {
               // Debug.Write();
                spriteBatch.Draw(tex, dimensions, Color.White);
            }
        }
    }

}
