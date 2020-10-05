using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

using Flipsider.Engine.Input;
using Flipsider.Weapons;
using Flipsider.Engine.Assets;

#nullable disable
// TODO fix this..
namespace Flipsider
{
    public class Player : Entity
    {


        float jumpheight = 3.9f;

        bool crouching;

        public int life = 90;
        public int maxLife = 100;
        public float percentLife => life / (float)maxLife;

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
            framewidth = 48;
            Collides = true;
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

        new void ResetVars()
        {
            onGround = false;
            isColliding = false;

            leftWeapon?.UpdatePassive();
            rightWeapon?.UpdatePassive();

            if (Swapping) SwapWeapons(); //TODO: move this
        }
        protected override void OnUpdate()
        {
            PlayerInputs();
            ResetVars();
            CoreUpdates();
            Constraints();
            if (Collides)
                TileCollisions();
            PostUpdates();

            leftWeapon?.UpdatePassive();
            rightWeapon?.UpdatePassive();
        }

        void CoreUpdates()
        {
            KeyboardState state = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

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


        void PlayerInputs()
        {
            KeyboardState state = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            friction = 0.92f;

            if (crouching) airResistance.X = 0.99f;
            else if (!onGround) airResistance.X = 0.97f;
            else airResistance.X = 0.985f;

            crouching = state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Down);

            if (!(crouching && onGround))
            {
                if (GameInput.Instance["MoveRight"].IsDown())
                {
                    float amount = GameInput.Instance["MoveRight"].GetPressValue();
                    velocity.X += acceleration * amount;
                    friction = 0.99f;
                }

                if (GameInput.Instance["MoveLeft"].IsDown())
                {
                    float amount = GameInput.Instance["MoveLeft"].GetPressValue();
                    velocity.X -= acceleration * amount;
                    friction = 0.99f;
                }
            }

            if (crouching)
            {
                //  height = 48;
                // friction = Math.Abs(velocity.X) > 0.2f ? 1 : 0.96f;
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
            if (Bottom >= Main.ScreenSize.Y)
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
            texture = AssetManager.player;
            FindFrame();
            spriteBatch.Draw(texture, Center - new Vector2(0, 18), frame, Color.White, 0f, frame.Size.ToVector2() / 2, 2f, spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            DrawMethods.DrawRectangle(position + new Vector2(0, maxHeight - height), width, height, Color.Green);
        }

        private void FindFrame()
        {
            if (friction != 0.99f)
            {
                Animate(6, 11, 48);
            }
            else
            {
                float vel = MathHelper.Clamp(Math.Abs(velocity.X), 1, 20);
                Animate((int)(4 / Math.Abs(vel*0.6f)), 8, 48, 1);
            }

            if (!onGround)
            {
                frame = new Rectangle(48 * 2, velocity.Y > 0 ? 48 : 0, framewidth, 48);
            }
        }
    }
}
