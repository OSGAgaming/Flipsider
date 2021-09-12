

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace FlipEngine
{
    internal class SelectScreen : ModeScreen
    {
        public override Mode Mode => Mode.Select;

        private bool flag = true;
        private bool mouseStateBuffer;
        private Vector2 pos1;
        private static List<Entity> EntityCache = new List<Entity>();
        private Vector2 DeltaMouse;
        public bool MoveMode;

        private Vector2 MouseSnap => FlipGame.MouseToDestination().ToVector2().Snap(8);
        private Rectangle DragRectangle => new Rectangle((int)pos1.X, (int)pos1.Y, (int)(MouseSnap.X - pos1.X) + 4, (int)(MouseSnap.Y - pos1.Y) + 4);

        public override void CustomDrawToScreen()
        {
            Utils.DrawSquare(MouseSnap, 4, Color.White);

            if(GameInput.Instance.JustClickingLeft)
            {
                MoveMode = false;
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if(GameInput.Instance.JustClickingLeft)
                {
                    foreach (Entity entity in EntityCache)
                    {
                        Rectangle entityRect = new Rectangle(entity.Position.ToPoint(), new Point(entity.Width, entity.Height));

                        if (entityRect.Contains(MouseSnap.ToPoint()))
                        {
                            MoveMode = true;
                        }
                    }
                }

                if (MoveMode) return;

                EntityCache.Clear();

                Chunk ActiveChunk = FlipGame.World.tileManager.GetChunkToWorldCoords(DragRectangle.Location.ToVector2());
                foreach(Entity entity in ActiveChunk.Entities)
                {
                    Rectangle entityRect = new Rectangle(entity.Position.ToPoint(), new Point(entity.Width, entity.Height));

                    if(entityRect.Intersects(DragRectangle))
                    {
                        EntityCache.Add(entity);
                    }
                }

                if (!flag)
                    Utils.DrawRectangle(DragRectangle, Color.White, 3);
            }


            MoveMode = false;

            foreach (Entity entity in EntityCache)
            {
                Rectangle entityRect = new Rectangle(entity.Position.ToPoint(), new Point(entity.Width, entity.Height));

                Utils.DrawRectangle(entityRect, Color.White * Math.Abs(Time.SineTime(4)), 2);
            }
        }
        public override void DrawToSelector(SpriteBatch sb)
        {
            Utils.DrawTextToLeft("Drag to select entities", Color.White, new Vector2(10));
        }

        public override void CustomUpdate()
        {
            if (Mouse.GetState().LeftButton != ButtonState.Pressed && mouseStateBuffer && !flag)
            {
                flag = true;

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


