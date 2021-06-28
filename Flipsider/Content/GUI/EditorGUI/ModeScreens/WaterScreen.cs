using Flipsider.Engine.Input;
using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Flipsider.TileManager;

namespace Flipsider.GUI.TilePlacementGUI
{
    internal class WaterScreen : ModeScreen
    {
        private bool flag = true;
        private bool mouseStateBuffer;
        private Vector2 pos1;
        private Vector2 MouseSnap => Main.MouseToDestination().ToVector2().Snap(8);

        public override Mode Mode => Mode.Water;

        public override void CustomDrawToScreen()
        {
            Utils.DrawSquare(MouseSnap, 4, Color.Blue);

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
                Vector2 MouseScreen = Main.MouseToDestination().ToVector2().Snap(8);
                if (!flag)
                    Utils.DrawRectangle(pos1, (int)(MouseScreen.X - pos1.X) + 4, (int)(MouseScreen.Y - pos1.Y) + 4, Color.White, 3);
            }

        }
        public override void DrawToSelector(SpriteBatch sb)
        {
            Utils.DrawTextToLeft("Drag and place water", Color.White, new Vector2(10));
        }

        public override void CustomUpdate()
        {
            if (Mouse.GetState().LeftButton != ButtonState.Pressed && mouseStateBuffer && !flag)
            {
                flag = true;
                Main.WaterBodies.Add(new Water(new RectangleF(pos1, new Vector2((MouseSnap.X - pos1.X) + 4, (MouseSnap.Y - pos1.Y) + 4))));
            }
            mouseStateBuffer = Mouse.GetState().LeftButton == ButtonState.Pressed;
            if (mouseStateBuffer && flag)
            {
                pos1 = Main.MouseToDestination().ToVector2().Snap(8);
                flag = false;
            }
            if (mouseStateBuffer)
            {
                // DrawMethods.DrawLine(pos1, Main.MouseScreen.ToVector2(), Color.White);
            }
        }
        protected override void OnLoad()
        {

        }

        internal override void OnDrawToScreenDirect() { }
    }

}


