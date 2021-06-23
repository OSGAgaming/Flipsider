﻿using Flipsider.Engine.Maths;
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
                    Main.player.Chunk.Colliedables.AddCustomHitBox(true,false,new RectangleF(pos1, new Vector2((MouseSnap.X - pos1.X) + 4, (MouseSnap.Y - pos1.Y) + 4)));
                }
                mouseStateBuffer = Mouse.GetState().LeftButton == ButtonState.Pressed;
                if (mouseStateBuffer && flag)
                {
                    pos1 = Main.MouseScreen.ToVector2().Snap(8);
                    flag = false;
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
            if (Main.Editor.CurrentState == EditorUIState.CollideablesEditorMode)
            {
                var PlayerChunk = Main.player.Chunk;
                foreach(Collideable col in PlayerChunk.Colliedables.collideables.ToArray())
                {
                    Utils.DrawRectangle(col.CustomHitBox, Color.Red * Time.SineTime(2f), 3);
                    if (col.CustomHitBox.ToR().Contains(Main.MouseScreen))
                    {
                        Utils.DrawRectangle(col.CustomHitBox, Color.Red, 3);
                        if (Mouse.GetState().RightButton == ButtonState.Pressed)
                        {
                            col.Dispose();
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

