using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flipsider
{
    public abstract partial class LivingEntity : Entity
    {
        public bool Animate(int per, int noOfFrames, int frameHeight, int column = 0, bool repeat = true, int startingFrame = 0)
        {
            bool hasEnded = false;
            if (frameY >= noOfFrames)
            {
                frameY = startingFrame;
            }
            if (per != 0)
            {
                if (frameCounter % per == 0)
                {
                    frameY++;
                    if (frameY >= noOfFrames)
                    {
                        if (repeat)
                        {
                            frameY = startingFrame;
                        }
                        else
                        {
                            hasEnded = true;
                            frameY = noOfFrames - 1;
                        }
                    }

                }
            }
            frame = new Rectangle(framewidth * column, frameY * frameHeight, framewidth, frameHeight);
            return hasEnded;
        }
        public void Constraints()
        {
            Position.Y += MathHelper.Clamp(Position.Y, -200, Utils.BOTTOM - Height) - Position.Y;
            Position.X = MathHelper.Clamp(Position.X, 0, 100000);
            if (Bottom >= Utils.BOTTOM)
            {
                onGround = true;
                velocity.Y = 0;
            }
        }
        public override void OnUpdateInEditor()
        {
            if (isDraggable && Layer == LayerHandler.CurrentLayer)
                CheckDrag();
            else
                isDragging = false;
        }
        public void CheckDrag()
        {
            if (mouseOverlap && !mousePressed && !isDragging && Main.Editor.IsActive)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    offsetFromMouseWhileDragging = Main.MouseScreen.ToVector2() - Center;
                    isDragging = true;
                }
            }
            if (isDragging)
            {
                Center = Main.MouseScreen.ToVector2() + offsetFromMouseWhileDragging;
                velocity = Vector2.Zero;
                if (Mouse.GetState().LeftButton != ButtonState.Pressed)
                {
                    isDragging = false;
                }
            }

            mousePressed = Mouse.GetState().LeftButton == ButtonState.Pressed;
        }
        public void DrawConstant(SpriteBatch spriteBatch) //for stuff that really shouldn't be overridden
        {
            if (isDraggable && Main.Editor.IsActive && mouseOverlap && !mousePressed && !isDragging)
            {
                Utils.DrawRectangle(CollisionFrame, Color.White, 3);
            }
        }

    }
}
