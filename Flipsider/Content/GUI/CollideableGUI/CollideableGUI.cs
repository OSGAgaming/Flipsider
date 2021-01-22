using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Flipsider.GUI
{
    internal class CollideableGUI : UIScreen
    {
        public int chosen = -1;
        private float position;
        protected override void OnLoad()
        {

        }
        private bool flag = true;
        private bool mouseStateBuffer;
        private Vector2 pos1;

        private Vector2 MouseSnap => Main.MouseScreen.ToVector2().Snap(8);
        protected override void OnUpdate()
        {
            if (Main.Editor.CurrentState == EditorUIState.CollideablesEditorMode)
            {
                if (Mouse.GetState().LeftButton != ButtonState.Pressed && mouseStateBuffer && !flag)
                {
                    flag = true;
                    Main.Colliedables.AddCustomHitBox(true,false,new RectangleF(pos1, new Vector2((MouseSnap.X - pos1.X) + 4, (MouseSnap.Y - pos1.Y) + 4)));
                }
                mouseStateBuffer = Mouse.GetState().LeftButton == ButtonState.Pressed;
                if (mouseStateBuffer && flag)
                {
                    pos1 = Main.MouseScreen.ToVector2().Snap(8);
                    flag = false;
                }
                if (mouseStateBuffer)
                {
                    // DrawMethods.DrawLine(pos1, Main.MouseScreen.ToVector2(), Color.White);
                }
            }
            if (Main.Editor.StateCheck(EditorUIState.CollideablesEditorMode))
            {
                position = position.ReciprocateTo(100, 10);
            }
            else
            {
                position = position.ReciprocateTo(-100, 10);
            }
        }
        internal override void DrawToScreen()
        {
            for (int i = 0; i < Main.Colliedables.collideables.Count; i++)
            {
                Utils.DrawRectangle(Main.Colliedables.collideables[i].CustomHitBox, Color.Red * Math.Abs(Time.SineTime(8)), 3);
            }
            if (Main.Editor.CurrentState == EditorUIState.CollideablesEditorMode)
            {
                for (int i = 0; i < Main.Colliedables.collideables.Count; i++)
                {
                    if (Main.Colliedables.collideables[i].CustomHitBox.ToR().Contains(Main.MouseScreen))
                    {
                        Utils.DrawRectangle(Main.Colliedables.collideables[i].CustomHitBox, Color.Red, 3);
                        if (Mouse.GetState().RightButton == ButtonState.Pressed)
                        {
                          //  Main.Colliedables.collideables[i];
                            Main.Colliedables.collideables.RemoveAt(i);
                        }
                    }
                }
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    Vector2 MouseScreen = Main.MouseScreen.ToVector2().Snap(8);
                    if (!flag)
                        Utils.DrawRectangle(pos1, (int)(MouseScreen.X - pos1.X) + 4, (int)(MouseScreen.Y - pos1.Y) + 4, Color.White, 3);
                }

            }
        }
        protected override void OnDraw()
        {
            if (Main.Editor.CurrentState == EditorUIState.CollideablesEditorMode)
            {
                Utils.DrawText("COLLIDEABLES BABY HAHAHA!", Color.Red, new Vector2(Main.ActualScreenSize.X / 2, position));
            }
        }
    }



}


