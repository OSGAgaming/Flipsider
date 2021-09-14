

using Flipsider;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace FlipEngine
{
    internal class CollideableScreen : ModeScreen
    {
        private bool flag = true;
        private bool mouseStateBuffer;
        private Vector2 pos1;
        private Vector2 MouseSnap => FlipGame.MouseToDestination().ToVector2().Snap(8);

        public override Mode Mode => Mode.Collideables;

        public override void CustomDrawToScreen()
        {
            Utils.DrawSquare(MouseSnap, 4, Color.Blue);

            var PlayerChunk = FlipGame.Camera.ActiveChunk;

            foreach (Collideable col in PlayerChunk.Colliedables.collideables.ToArray())
            {
                Utils.DrawRectangle(col.CustomHitBox, Color.Red * Time.SineTime(2f), 3);
                if (col.CustomHitBox.ToR().Contains(FlipGame.MouseToDestination()))
                {
                    Utils.DrawRectangle(col.CustomHitBox.ToR().Inf(2,2), Color.White, 3);
                    if (Mouse.GetState().RightButton == ButtonState.Pressed)
                    {
                        col.Dispose();
                    }
                }
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Vector2 MouseScreen = FlipGame.MouseToDestination().ToVector2().Snap(8);
                if (!flag)
                    Utils.DrawRectangle(pos1, (int)(MouseScreen.X - pos1.X) + 4, (int)(MouseScreen.Y - pos1.Y) + 4, Color.Red, 3);
            }

        }
        public override void DrawToSelector(SpriteBatch sb)
        {
            Utils.DrawTextToLeft("Drag to place collideables", Color.White, new Vector2(10));
        }

        public override void CustomUpdate()
        {
            if (Mouse.GetState().LeftButton != ButtonState.Pressed && mouseStateBuffer && !flag)
            {
                flag = true;
                FlipGame.Camera.ActiveChunk.Colliedables.AddCustomHitBox(Main.player, true, false, new RectangleF(pos1, new Vector2((MouseSnap.X - pos1.X) + 4, (MouseSnap.Y - pos1.Y) + 4)));
            }
            mouseStateBuffer = Mouse.GetState().LeftButton == ButtonState.Pressed;
            if (mouseStateBuffer && flag)
            {
                pos1 = FlipGame.MouseToDestination().ToVector2().Snap(8);
                flag = false;
            }
        }
        protected override void OnLoad()
        {

        }

        internal override void OnDrawToScreenDirect() { }
    }

}


