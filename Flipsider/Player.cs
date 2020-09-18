using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using Flipsider.Weapons;

namespace Flipsider
{
    public class Player : Entity
    {
        public float acceleration = 0.08f;
        public float gravity = 0.15f;

        public Vector2 airResistance = new Vector2(0.985f, 0.999f);

        float jumpheight = 5;
        public bool onGround;
        public float friction = 0.982f;
        public int spriteDirection;
        bool isColliding;
        bool crouching;

        public int life = 90;
        public int maxLife = 100;
        public float percentLife => life / (float)maxLife;

        public float feetPos {
            get => position.Y + maxHeight;
            set => position.Y = value - maxHeight;
        }

        public Weapon leftWeapon = new Weapons.Ranged.Pistol.TestGun(); //Temporary
        public Weapon rightWeapon = new Weapons.Ranged.Pistol.TestGun2(); //Temporary

        public Weapon leftWeaponStore;
        public Weapon rightWeaponStore;
        public bool usingWeapon => leftWeapon.active || rightWeapon.active;

        public Player(Vector2 position)
        {
            this.position = position;
            width = 30;
            maxHeight = 60;
            height = 60;
        }

        public int swapTimer;
        public bool Swapping => swapTimer > 0;

        void SwapWeapons()
        {
            swapTimer++;

            if (swapTimer == 15)
            {
                SwapWeapon(ref leftWeapon, ref leftWeaponStore);
                SwapWeapon(ref rightWeapon, ref rightWeaponStore);
            }

            if (swapTimer >= 30)
                swapTimer = 0;
        }

        void SwapWeapon(ref Weapon first, ref Weapon second)
        {
            var temp = first;
            first = second;
            second = temp;
        }
        void ResetVars()
        {
            onGround = false;
            isColliding = false;

            leftWeapon?.UpdatePassive();
            rightWeapon?.UpdatePassive();

            if (Swapping) SwapWeapons(); //TODO: move this
        }
        protected internal override void Initialize()
        {
            Main.entities.Add(this);
        }
        protected override void OnUpdate()
        {
            PlayerInputs();
            ResetVars();
            CoreUpdates();
            Constraints();
            TileCollisions();
            PostUpdates();

            leftWeapon?.UpdatePassive();
            rightWeapon?.UpdatePassive();
        }

        void CoreUpdates()
        {
            KeyboardState state = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            velocity.Y += gravity;
            velocity *= airResistance;
            position += velocity * Time.QuickDelta;

            if (state.IsKeyDown(Keys.X) && !Swapping)
            {
                swapTimer = 1;
            }
            if (mouseState.LeftButton == ButtonState.Pressed)
            leftWeapon?.Activate(this);

            if (mouseState.RightButton == ButtonState.Pressed)
            rightWeapon?.Activate(this);

        }

        void PostUpdates()
        {
            KeyboardState state = Keyboard.GetState();
            if (onGround)
            {
                velocity.X *= friction;
            }
            if ((state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Space)) && !crouching)
            {
                Jump();
            }
            if (velocity.X >= acceleration)
            {
                spriteDirection = 1;
            }
            else if (velocity.X <= -acceleration)
            {
                spriteDirection = -1;
            }
        }

        void TileCollisions()
        {
            float up = -1;
            float down = Main.MaxTilesX;
            float left = -1;
            float right = Main.MaxTilesY;
            int res = TileManager.tileRes;
            int tileHeight = height / res + 2;
            int tileWidth = width / res + 2;
            for (int i = (int)position.X/res - tileWidth; i < (int)position.X / res + tileWidth; i++)
            {
                for (int j = (int)position.Y / res - tileHeight; j < (int)position.Y / res + tileHeight; j++)
                {
                    if (i > 0 && j > 0)
                    {
                        if (Main.tiles[i, j].active)
                        {
                            bool case1 = (feetPos - height - velocity.Y >= j * res - 1 && feetPos - velocity.Y + 1 >= j * res) &&
                                         (feetPos - height - velocity.Y >= j * res + res - 1 && feetPos - velocity.Y + 1 >= j * res + res);
                            bool case2 = (feetPos - height - velocity.Y <= j * res + 1 && feetPos - velocity.Y - 1 <= j * res) &&
                                         (feetPos - height - velocity.Y <= j * res + res + 1 && feetPos - velocity.Y - 1 <= j * res + res);
                            bool hasSideCollision = !(case1 || case2);
                            bool case3 = (position.X - velocity.X >= i * res - 1 && position.X + width - velocity.X + 1 >= i * res) &&
                                         (position.X - velocity.X >= i * res + res - 1 && position.X + width - velocity.X + 1 >= i * res + res);
                            bool case4 = (position.X - velocity.X <= i * res + 1 && position.X + width - velocity.X - 1 <= i * res) &&
                                         (position.X - velocity.X <= i * res + res + 1 && position.X + width - velocity.X - 1 <= i * res + res);
                            bool hasVertCollision = !(case3 || case4);
                            if (hasVertCollision)
                            {
                                if (j > up - 1 && j * res < position.Y && velocity.Y < 0)
                                {
                                    up = j + 1;
                                }
                                if (j < down && j * res + res / 2 > feetPos && velocity.Y > 0)
                                {
                                    down = j;
                                }
                            }
                            if (hasSideCollision)
                            {
                                if (i > left - 1 && i * res < position.X && velocity.X < 0)
                                {
                                    left = i + 1;
                                }
                                if (i < right && i * res + res / 2 > position.X + width && velocity.X > 0)
                                {
                                    right = i;
                                }
                            }
                        }
                    }
                }
            }

            if (position.Y < up * res && up != -1)
            {
                position.Y = up * res;
                velocity.Y = 0;
                isColliding = true;
            }
            if (position.Y > down * res - height && down != Main.tiles.GetLength(1))
            {
                position.Y = down * res - height;
                velocity.Y = 0;
                onGround = true;
                isColliding = true;
            }
            if (position.X < left * res + 1 && left != -1)
            {
                position.X = left * res;
                velocity.X = 0;
                isColliding = true;
                spriteDirection = -1;
            }
            if (position.X > right * res - width - 1 && right != Main.tiles.GetLength(0))
            {
                position.X = right * res - width;
                velocity.X = 0;
                isColliding = true;
                spriteDirection = 1;
            }
        }

        void PlayerInputs()
        {
            KeyboardState state = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            friction = 0.89f;

            if (crouching) airResistance.X = 0.99f;
            else if (!onGround) airResistance.X = 0.97f;
            else airResistance.X = 0.985f;

            crouching = state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Down);

            if (!(crouching && onGround))
            {
                if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
                {
                    velocity.X += acceleration;
                    friction = 0.99f;
                }

                if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A))
                {
                    velocity.X -= acceleration;
                    friction = 0.99f;
                }
            }

            if(crouching)
            {
              //  height = 48;
                friction = Math.Abs(velocity.X) > 0.2f ? 1 : 0.96f;
            }
            else
            {
                if (height != 72)
                {
                  //  height = 72;
                   // position.Y -= (72 - 48);
                }
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
                leftWeapon.Activate(this);

            if (mouseState.RightButton == ButtonState.Pressed)
                rightWeapon.Activate(this);
        }

        void Constraints()
        {
            position.Y = MathHelper.Clamp(position.Y, -200, Main.ScreenSize.Y - maxHeight);
            position.X = MathHelper.Clamp(position.X, 0, 100000);
            if(feetPos >= Main.ScreenSize.Y)
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
        public override void Draw(SpriteBatch spriteBatch)
        {
            texture = TextureCache.player;
            FindFrame();
            spriteBatch.Draw(texture, Center- new Vector2(0,18), frame, Color.White, 0f, frame.Size.ToVector2()/2, 2f, spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            DrawMethods.DrawRectangle(position + new Vector2(0,maxHeight-height), width, height, Color.Green);
        }

        private void FindFrame()
        {
            if (friction != 0.99f)
            {
                int frameY = (int)(Main.gameTime.TotalGameTime.TotalMilliseconds / 100) % 11 * 48;
                frame = new Rectangle(0, frameY, 48, 48);
            }
            else
            {
                int frameY = (int)(Main.gameTime.TotalGameTime.TotalMilliseconds / (Math.Abs(velocity.X) > 2 ? 60 : 80)) % 8 * 48;
                frame = new Rectangle(48, frameY, 48, 48);
            }

            if (!onGround)
            {
                int frameY = velocity.Y < 0 ? 0 : 48;
                frame = new Rectangle(48 * 2, frameY, 48, 48);
            }
        }
    }
}
