using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Flipsider
{
    public class Player
    {
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 Center => position + new Vector2(width / 2f, height / 2f);
        public float acceleration = 0.1f;
        public float gravity = 0.2f;
        public float airResistance = 0.97f;
        private int width = 50;
        private int height = 72;
        float jumpheight = 5;
        public bool onGround;
        public float friction = 0.9f;
        public int spriteDirection;

        public Player(Vector2 position)
        {
            this.position = position;
        }
        void ResetVars()
        {
            onGround = false;
        }
        public void Update()
        {
            if(velocity.X > 0)
            {
                spriteDirection = 1;
            }
            else
            {
                spriteDirection = -1;
            }
            ResetVars();
            CoreUpdates();
            TileCollisions();
            Constraints();
            PlayerInputs();
        }
        void CoreUpdates()
        {
            velocity.Y += gravity;
            velocity *= airResistance;
            position += velocity;
        }
        void TileCollisions()
        {
            float up = -1;
            float down = Main.tiles.GetLength(1);
            float left = -1;
            float right = Main.tiles.GetLength(0);
            int res = TileManager.tileRes;
            for (int i = 0; i < Main.tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Main.tiles.GetLength(1); j++)
                {
                    if (Main.tiles[i,j] == 1)
                    {
                        bool case1 = (position.Y - velocity.Y >= j * res && position.Y + height - velocity.Y >= j * res) &&
                                     (position.Y - velocity.Y >= j * res + res && position.Y + height - velocity.Y >= j * res + res);
                        bool case2 = (position.Y - velocity.Y <= j * res && position.Y + height - velocity.Y <= j * res) &&
                                     (position.Y - velocity.Y <= j * res + res && position.Y + height - velocity.Y <= j * res + res);
                        bool hasSideCollision = !(case1 || case2);
                        bool case3 = (position.X - velocity.X >= i * res && position.X + width - velocity.X >= i * res) &&
                                     (position.X - velocity.X >= i * res + res && position.X + width - velocity.X >= i * res + res);
                        bool case4 = (position.X - velocity.X <= i * res && position.X + width - velocity.X <= i * res) &&
                                     (position.X - velocity.X <= i * res + res && position.X + width - velocity.X <= i * res + res);
                        bool hasVertCollision = !(case3 || case4);
                        if (hasVertCollision)
                        {
                            if (j > up && j * res < position.Y)
                            {
                                up = j + 1;
                            }
                            if (j < down && j * res + res > position.Y + height)
                            {
                                down = j;
                            }
                        }
                        if (hasSideCollision)
                        {
                            if (i > left && i* res < position.X)
                            {
                                left = i + 1;
                            }
                            if (i < right && i * res + res > position.X + width)
                            {
                                right = i;
                            }
                        }
                        
                    }
                }
            }
            if (position.Y < up * res && up != -1)
            {
                position.Y = up * res;
                velocity.Y = 0;
            }
            if (position.Y > down * res - height && down != Main.tiles.GetLength(1))
            {
                position.Y = down * res - height;
                velocity.Y = 0;
                onGround = true;
            }
            if (position.X < left * res && left != -1)
            {
                position.X = left * res;
                velocity.X = 0;
            }
            if (position.X > right * res - width && right != Main.tiles.GetLength(0))
            {
                position.X = right * res - width;
                velocity.X = 0;
            }
        }
        void PlayerInputs()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
            {
                velocity.X += acceleration;
            }
            if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A))
            {
                velocity.X -= acceleration;
            }
            if (state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W))
            {
                Jump();
            }
            if (onGround)
            {
                velocity.X *= friction;
            }
        }

        void Constraints()
        {
            position.Y = MathHelper.Clamp(position.Y,0,Main.screenSize.Y - height);
            position.X = MathHelper.Clamp(position.X, 0, Main.screenSize.X - width);
            if(position.Y >= Main.screenSize.Y - height)
            {
                onGround = true;
                velocity.Y = 0;
            }
        }

        void Jump()
        {
            if (onGround)
            {
                velocity.Y -= jumpheight;
            }
        }
        public void RenderPlayer()
        {
            Main.spriteBatch.Draw(TextureCache.player,Center, new Rectangle(0,0,width,height),Color.White,0f,new Vector2(width / 2, height / 2),1f, spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,0f);
        }
    }
}
