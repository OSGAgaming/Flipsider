using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Flipsider.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using static Flipsider.NPC;
using static Flipsider.TileManager;
using System.Windows.Input;
using Flipsider.Engine.Input;
using System.IO;

namespace Flipsider.GUI
{
    internal class WaterCrationGUI : UIScreen
    {
        public int chosen = -1;
        float position;
        protected override void OnLoad()
        {
            
        }
        private bool flag = true;
        private bool mouseStateBuffer;
        private Vector2 pos1;
        Vector2 MouseSnap => Main.MouseScreen.ToVector2().Snap(8);
        protected override void OnUpdate()
        {
            if (Main.Editor.CurrentState == EditorUIState.WaterEditorMode)
            {
                if (Mouse.GetState().LeftButton != ButtonState.Pressed && mouseStateBuffer && !flag)
                {
                    flag = true;
                    Main.WaterBodies.Add(new Water(new Rectangle(pos1.ToPoint(),new Point((int)(MouseSnap.X - pos1.X) + 4, (int)(MouseSnap.Y - pos1.Y) + 4))));
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
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    Vector2 MouseScreen = Main.MouseScreen.ToVector2().Snap(8);
                    if(!flag)
                    DrawMethods.DrawRectangle(pos1, (int)(MouseScreen.X - pos1.X) + 4, (int)(MouseScreen.Y - pos1.Y) + 4, Color.White,3);
                }
                
            }
        }
        protected override void OnDraw()
        {
            if (Main.Editor.CurrentState == EditorUIState.WaterEditorMode)
            {
                DrawMethods.DrawText("PLACE WATER YOU FUCKING MORON. DO IT!", Color.Blue, new Vector2(Main.ActualScreenSize.X / 2, position));
            }
        }
    }
   

   
}


