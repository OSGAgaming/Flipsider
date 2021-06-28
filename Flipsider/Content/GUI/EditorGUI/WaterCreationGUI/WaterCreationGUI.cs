using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Flipsider.GUI
{
    internal class WaterCrationGUI : UIScreen
    {
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
            if (Main.Editor.CurrentState == EditorUIState.WaterEditorMode)
            {
                if (Mouse.GetState().LeftButton != ButtonState.Pressed && mouseStateBuffer && !flag)
                {
                    flag = true;
                    Main.WaterBodies.Add(new Water(new RectangleF(pos1, new Vector2((MouseSnap.X - pos1.X) + 4, (MouseSnap.Y - pos1.Y) + 4))));
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
            if (Main.Editor.StateCheck(EditorUIState.WaterEditorMode))
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
            if (Main.Editor.CurrentState == EditorUIState.WaterEditorMode)
            {
                for (int i = 0; i < Utils.WaterBodies.Count; i++)
                {
                    if (Utils.WaterBodies[i].frame.Contains(Main.MouseScreen))
                    {
                        Utils.DrawRectangle(Utils.WaterBodies[i].frame, Color.White, 3);
                        if (Mouse.GetState().RightButton == ButtonState.Pressed)
                        {
                            Utils.WaterBodies[i].Dispose();
                            Utils.WaterBodies.RemoveAt(i);
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
            if (Main.Editor.CurrentState == EditorUIState.WaterEditorMode)
            {
                Utils.DrawText("PLACE WATER YOU FUCKING MORON. DO IT!", Color.Blue, new Vector2(Main.ActualScreenSize.X / 2, position));
            }
        }
    }



}


