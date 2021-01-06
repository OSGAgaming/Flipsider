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
    internal class LightPlacementGUI : UIScreen
    {
        protected override void OnLoad()
        {
            LightUI LU1 = new LightUI
            {
                dimensions = new Rectangle((int)Main.ActualScreenSize.X, 120, TextureCache.NPCPanel.Width, TextureCache.NPCPanel.Height)
            };
            LightUI LU2 = new LightUI
            {
                dimensions = new Rectangle((int)Main.ActualScreenSize.X, 180, TextureCache.NPCPanel.Width, TextureCache.NPCPanel.Height)
            };
            LightUI LU3 = new LightUI
            {
                dimensions = new Rectangle((int)Main.ActualScreenSize.X, 240, TextureCache.NPCPanel.Width, TextureCache.NPCPanel.Height)
            };
            elements.Add(LU1);
            elements.Add(LU2);
            elements.Add(LU3);
        }

        private bool flag = true;
        private bool mouseStateBuffer;
        private Vector2 pos1;
        private Vector2 pos1Inv;
        protected override void OnUpdate()
        {
            if (Main.Editor.CurrentState == EditorUIState.LightEditorMode)
            {
                if (Mouse.GetState().LeftButton != ButtonState.Pressed && mouseStateBuffer && !flag)
                {
                    flag = true;
                    Main.lighting.AddDirectionalLight(pos1, Main.MouseScreen.ToVector2(), Color.White);
                }
                mouseStateBuffer = Mouse.GetState().LeftButton == ButtonState.Pressed;
                if (mouseStateBuffer && flag)
                {
                    pos1 = Main.MouseScreen.ToVector2();
                    pos1Inv = Mouse.GetState().Position.ToVector2();
                    flag = false;
                }
                if (mouseStateBuffer)
                {
                    // DrawMethods.DrawLine(pos1, Main.MouseScreen.ToVector2(), Color.White);
                }
            }
        }
        protected override void OnDraw()
        {
            if (Main.Editor.CurrentState == EditorUIState.LightEditorMode ||
                Main.Editor.CurrentState == EditorUIState.TileEditorMode ||
                Main.Editor.CurrentState == EditorUIState.PropEditorMode)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && Main.Editor.CurrentState == EditorUIState.LightEditorMode)
                {
                    DrawMethods.DrawLine(pos1Inv, Mouse.GetState().Position.ToVector2(), Color.White, 2);
                }
                for (int i = 0; i < Main.lighting.directionalLightSources.Count; i++)
                {
                    float sine = (float)Math.Sin(Main.gameTime.TotalGameTime.TotalSeconds * 2);
                    DrawMethods.DrawLine(Main.lighting.directionalLightSources[i].position1.ToScreenInv(), Main.lighting.directionalLightSources[i].position2.ToScreenInv(), Color.White * 0.2f, sine + 1);
                }
            }
        }
    }

    internal class LightUI : UIElement
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            Main.spriteBatch.Draw(TextureCache.NPCPanel, dimensions, Color.White * alpha);

        }
        protected override void OnUpdate()
        {
            Vector2 activePos = Main.ActualScreenSize - new Vector2(80, -10);
            Vector2 inactivePos = Main.ActualScreenSize;
            if (Main.Editor.CurrentState == EditorUIState.LightEditorMode)
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

        private float alpha = 1f;
        protected override void OnHover()
        {
            alpha.ReciprocateTo(0.5f, 16f);
        }

        protected override void NotOnHover()
        {
            alpha.ReciprocateTo(1f, 16f);
        }
    }
}
