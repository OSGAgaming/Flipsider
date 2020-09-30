using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Flipsider.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Flipsider.NPC;
using static Flipsider.TileManager;
namespace Flipsider.GUI.TilePlacementGUI
{

    class LightPlacementGUI : UIScreen
    {
        public LightPlacementGUI()
        {
            Main.UIScreens.Add(this);
            LightUI LU1 = new LightUI
            {
                dimensions = new Rectangle((int)Main.ScreenSize.X, 120, TextureCache.NPCPanel.Width, TextureCache.NPCPanel.Height)
            };
            LightUI LU2 = new LightUI
            {
                dimensions = new Rectangle((int)Main.ScreenSize.X, 180, TextureCache.NPCPanel.Width, TextureCache.NPCPanel.Height)
            };
            LightUI LU3 = new LightUI
            {
                dimensions = new Rectangle((int)Main.ScreenSize.X, 240, TextureCache.NPCPanel.Width, TextureCache.NPCPanel.Height)
            };
            elements.Add(LU1);
            elements.Add(LU2);
            elements.Add(LU3);
        }

        bool flag = true;
        bool mouseStateBuffer;
        Vector2 pos1;
        protected override void OnUpdate()
        {
            if(EditorModes.CurrentState == EditorUIState.LightEditorMode)
            {
                if(Mouse.GetState().LeftButton != ButtonState.Pressed && mouseStateBuffer && !flag)
                {
                    flag = true;
                    Lighting.AddDirectionalLight(pos1, Main.MouseScreen.ToVector2(), Color.White);
                    Debug.Write(2);
                }
                mouseStateBuffer = Mouse.GetState().LeftButton == ButtonState.Pressed;
                if(mouseStateBuffer && flag)
                {
                    pos1 = Main.MouseScreen.ToVector2();
                    flag = false;
                }
                if(mouseStateBuffer)
                {
                   // DrawMethods.DrawLine(pos1, Main.MouseScreen.ToVector2(), Color.White);
                }
            }
        }
        protected override void OnDraw()
        {

        }
    }
    
    class LightUI : UIElement
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            Main.spriteBatch.Draw(TextureCache.NPCPanel, dimensions, Color.White* alpha);

        }
        protected override void OnUpdate()
        {
            Vector2 activePos = Main.ScreenSize - new Vector2(80, -10);
            Vector2 inactivePos = Main.ScreenSize;
            if (EditorModes.CurrentState == EditorUIState.LightEditorMode)
            {
                dimensions.X += (int)(activePos.X - dimensions.X) / 16;
            }
            else
            {
                dimensions.X += (int)(inactivePos.X + 16 - dimensions.X) / 16;
            }
        }
        protected override void OnLeftClick()
        {
           
        }
        float alpha = 1f;
        protected override void OnHover()
        {
           alpha.ReciprocateTo(0.5f,16f);
        }

        protected override void NotOnHover()
        {
            alpha.ReciprocateTo(1f, 16f);
        }
    }
}
