using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

using Flipsider.Engine.Input;
using Flipsider.Weapons;
using Flipsider.Weapons.Ranged.Pistol;

#nullable enable
// TODO fix this..
namespace Flipsider
{
    public class Player : Entity
    {

        public IStoreable? SelectedItem;

        private readonly float jumpheight = 3.7f;
        private bool crouching;

        public int life = 500;
        public int maxLife = 500;
        public float percentLife => life / (float)maxLife;

        public Weapon leftWeapon = new TestGun(); //Temporary
        public Weapon rightWeapon = new TestGun2(); //Temporary

        public Weapon leftWeaponStore = new ShortSword();
        public Weapon rightWeaponStore = new ShortSword();
        public bool usingWeapon => (leftWeapon != null ? leftWeapon.active : false) || (rightWeapon != null ? rightWeapon.active : false);
        public IStoreable[] inventory;
        public int inventorySize => 20;
        public Player(Vector2 position)
        {

            inventory = new IStoreable[20];
            this.position = position;
            width = 30;
            maxHeight = 60;
            height = 60;
            framewidth = 48;
            Collides = false;
        }

        public void AddToInventory(IStoreable item, int slot)
        {
            if (item != null)
            {
                if (slot >= inventorySize)
                {
                    inventory[inventorySize - 1] = item;
                }
                else
                {
                    inventory[slot] = item;
                }
            }
        }

        public int swapTimer;
        public bool Swapping => swapTimer > 0;

        private void SwapWeapons()
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

        private void SwapWeapon(ref Weapon first, ref Weapon second)
        {
            var temp = first;
            first = second;
            second = temp;
        }

        private new void ResetVars()
        {
            onGround = false;
            isColliding = false;
            gravity = 0.08f;
            airResistance.Y = 0.99f;
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
            TileCollisions(Main.CurrentWorld);
            PostUpdates();
            FindFrame();
            leftWeapon?.UpdatePassive();
            rightWeapon?.UpdatePassive();
        }

        private void CoreUpdates()
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

        private void PostUpdates()
        {
            KeyboardState state = Keyboard.GetState();
            if(Wet)
            {
                airResistance.X = 0.94f;
                gravity = 0.03f;
            }
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

        private void PlayerInputs()
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
                leftWeapon?.Activate(this);

            if (mouseState.RightButton == ButtonState.Pressed)
                rightWeapon?.Activate(this);
        }

        private void Jump()
        {
            if (onGround)
            {
                velocity.Y -= jumpheight;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            texture = TextureCache.player;
            spriteBatch.Draw(texture, Center - new Vector2(0, 18), frame, Color.White, 0f, frame.Size.ToVector2() / 2, 2f, spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            if (Math.Abs(velocity.LengthSquared()) > 1)
            {
                for (int i = 0; i < oldPositions.Length; i++)
                {
                    int length = oldPositions.Length;
                    float alpha = (length - i) / length;
                    spriteBatch.Draw(texture, oldPositions[i] - new Vector2(-width / 2, 18 - height / 2), frame, Color.White * alpha * 0.3f, 0f, frame.Size.ToVector2() / 2, 2f, spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
                }
            }
        }
        bool FreeFall;
        bool isRecovering;
        float VelYCache;
        private void FindFrame()
        {
            if (onGround)
            {
                if(FreeFall)
                {
                    frameY = 0;
                    isRecovering = true;
                    VelYCache = oldVelocity.Y;
                }
                if (isRecovering)
                {
                    if (VelYCache > 4)
                    {
                        velocity.X *= 0.9f;
                        isRecovering = !Animate(4, 7, 48, 8, false);
                    }
                    else if(VelYCache > 0)
                    {
                        velocity.X *= 0.97f;
                        isRecovering = !Animate(7, 2, 48, 7, false);
                    }
                }
                else
                {
                    if (friction != 0.99f)
                    {
                        Animate(6, 11, 48);
                    }
                    else
                    {
                        float vel = MathHelper.Clamp(Math.Abs(velocity.X), 1, 20);
                        int velFunc = (int)(Math.Round(10 / Math.Abs(vel * 0.6f)) * Time.DeltaTimeRoundedVar(600, 10) / 1600);
                        Animate(velFunc, 8, 48, 1);
                    }
                }
                FreeFall = false;
            }
            else
            {
                if (velocity.Y < 0)
                {
                    Animate(1, 1, 48, 2, false);
                }
                else
                {
                    if(!FreeFall)
                        FreeFall = Animate(2, 7, 48, 11, false, 1);
                    if(FreeFall)
                    {
                        Animate(4, 4, 48, 12, true);
                    }
                }
            }
        }
    }
}
