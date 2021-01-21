using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flipsider
{
    public abstract partial class LivingEntity : Entity
    {

        public void TileCollisions(World world)
        {
            int res = world.TileRes;
            for (int i = (int)position.X / res - (width / res + 2); i < (int)position.X / res + (width / res + 2); i++)
            {
                for (int j = (int)position.Y / res - (height / res + 2); j < (int)position.Y / res + (height / res + 2); j++)
                {
                    if (world.IsTileInBounds(i, j))
                    {
                        Rectangle tileRect = new Rectangle(i * res, j * res, res, res);

                        CollisionInfo collisionInfo = Collision.AABBResolve(CollisionFrame, PreCollisionFrame, tileRect);

                        if (collisionInfo.AABB == Bound.Top)
                        {
                            velocity.Y = 0;
                            onGround = true;
                        }
                        if (collisionInfo.AABB == Bound.Bottom)
                        {
                            velocity.Y = 0;
                        }
                        if (collisionInfo.AABB == Bound.Left)
                        {
                            velocity.X = 0;
                        }
                        if (collisionInfo.AABB == Bound.Right)
                        {
                            velocity.X = 0;
                        }
                        isColliding = true;
                        position += collisionInfo.d;
                    }
                }
            }
        }


        public bool Animate(int per, int noOfFrames, int frameHeight, int column = 0, bool repeat = true, int startingFrame = 0)
        {
            bool hasEnded = false;
            if (frameY >= noOfFrames && repeat)
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
            position.Y += MathHelper.Clamp(position.Y, -200, Utils.BOTTOM - maxHeight) - position.Y;
            position.X = MathHelper.Clamp(position.X, 0, 100000);
            if (Bottom >= Utils.BOTTOM)
            {
                onGround = true;
                velocity.Y = 0;
            }
        }

        public void UpdateInEditor()
        {
            OnUpdateInEditor();
            if (isDraggable && Layer == LayerHandler.CurrentLayer)
                CheckDrag();
            else
                isDragging = false;
        }
        public void CheckDrag()
        {
            if (mouseOverlap && !mousePressed && !isDragging)
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
