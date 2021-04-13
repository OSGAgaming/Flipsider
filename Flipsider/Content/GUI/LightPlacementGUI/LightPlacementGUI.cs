using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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
        protected override void OnUpdate()
        {
            if (Main.Editor.CurrentState == EditorUIState.LightEditorMode)
            {
                if (Mouse.GetState().LeftButton != ButtonState.Pressed && mouseStateBuffer && !flag)
                {
                    flag = true;
                    Vector2 dXY = Main.MouseScreen.ToVector2() - pos1;
                    //WMain.lighting.AddLight(dXY.Length(), pos1, Color.Green, 0.5f, dXY.ToRotation());
                }
                mouseStateBuffer = Mouse.GetState().LeftButton == ButtonState.Pressed;
                if (mouseStateBuffer && flag)
                {
                    pos1 = Main.MouseScreen.ToVector2();
                    flag = false;
                }

            }
        }
        internal override void DrawToScreen()
        {
            if (Main.Editor.CurrentState == EditorUIState.LightEditorMode ||
                Main.Editor.CurrentState == EditorUIState.TileEditorMode ||
                Main.Editor.CurrentState == EditorUIState.PropEditorMode)
            {
                if (mouseStateBuffer)
                {
                    Utils.DrawLine(pos1, Main.MouseScreen.ToVector2(), Color.White);
                }
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && Main.Editor.CurrentState == EditorUIState.LightEditorMode)
                {
                    Utils.DrawLine(pos1, Main.MouseScreen.ToVector2(), Color.White, 2);
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
                alpha = alpha.ReciprocateTo(1f, 16f); 
            }
            else
            {
                dimensions.X += (int)(inactivePos.X + 16 - dimensions.X) / 16;
                alpha = alpha.ReciprocateTo(0f, 16f);
            }
        }
        protected override void OnLeftClick()
        {

        }

        private float alpha = 1f;
        protected override void OnHover()
        {
        }

        protected override void NotOnHover()
        {
        }
    }
}
