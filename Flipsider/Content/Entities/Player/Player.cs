using Flipsider.Engine.Input;
using Flipsider.Engine.Maths;
using Flipsider.Weapons;
using Flipsider.Weapons.Ranged.Pistol;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.IO;

#nullable enable
// TODO fix this..
namespace Flipsider
{
    public partial class Player : LivingEntity
    {

        public IStoreable? SelectedItem;

        public bool crouching;

        public int life = 100;
        public int maxLife = 100;
        public float percentLife => life / (float)maxLife;

        public Weapon leftWeapon = new TestGun(); //Temporary
        public Weapon rightWeapon = new TestGun2(); //Temporary

        public Weapon leftWeaponStore = new Crowbar();
        public Weapon rightWeaponStore = new Crowbar();
        public bool usingWeapon => (leftWeapon != null ? leftWeapon.active : false) || (rightWeapon != null ? rightWeapon.active : false);
        public IStoreable[] inventory;
        public int inventorySize => 20;
        public Player(Vector2 position, bool Active = true) : base()
        {
            AddModule("Movement", new PlayerMovement(this));
            inventory = new IStoreable[20];
            this.position = position;
            width = 30;
            height = 60;
            framewidth = 48;
            noGravity = false;
            Collides = true;
            this.Active = Active;
        }
        public Player() : base()
        {
            AddModule("Movement", new PlayerMovement(this));
            inventory = new IStoreable[20];
            width = 30;
            height = 60;
            framewidth = 48;
            noGravity = false;
            Collides = true;
        }
        public override void Dispose()
        {
            Active = false;
            Main.layerHandler.Layers[Layer].Drawables.Remove(this);
            Chunk.Colliedables.RemoveThroughEntity(this);
            UpdateModules.Clear();
            Chunk.Entities.Remove(this);
            Main.lighting.RemoveBloom(this);
            OnKill();
        }
        public override Entity Deserialize(Stream stream)
        {
            BinaryReader binaryWriter = new BinaryReader(stream);
            float X = binaryWriter.ReadSingle();
            float Y = binaryWriter.ReadSingle();
            Player player = new Player(new Vector2(X, Y));
            new EntityBloom(player, player.texture, 2.6f);
            return Main.CurrentWorld.ReplacePlayer(player);
        }
        public override void Serialize(Stream stream)
        {
            BinaryWriter binaryWriter = new BinaryWriter(stream);
            binaryWriter.Write(position.X);
            binaryWriter.Write(position.Y);
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
            Weapon? temp = first;
            first = second;
            second = temp;
        }

        private new void ResetVars()
        {
            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                leftWeapon?.Activate(this);
            }

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                rightWeapon?.Activate(this);
            }

            if (Swapping) SwapWeapons(); //TODO: move this
        }
        protected override void PreAI()
        {
            ResetVars();
            if (IFrames > 0)
                IFrames--;
        }
        protected override void AI()
        {
            CoreUpdates();
        }
        protected override void PostAI()
        {
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
            GetEntityModifier<HitBox>().GenerateHitbox(CollisionFrame, true, (HitBox hitBox) => { });


            if (TimeOutsideOfCombat > 0)
                TimeOutsideOfCombat--;

            if (isAttacking)
            {
                TimeOutsideOfCombat = 200;
            }
        }

        public int IFrames;
        public static readonly int GlobalIFrames = 50;
        public void TakeDamage(int amount)
        {
            if (IFrames == 0)
            {
                life -= amount;
                IFrames = GlobalIFrames;
                TimeOutsideOfCombat = 200;
            }
        }

        private void PlayerInputs()
        {
            KeyboardState state = Keyboard.GetState();
            InFrame = true;

            crouching = state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Down);

            UpdateEntityModifier("Movement");

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            float IFrameSine = (float)Math.Abs(Math.Sin(IFrames / 2));
            texture = TextureCache.player;
            Rectangle weaponFrame = new Rectangle(((frame.X / 48 - 4) * 69), frameY * 50, 69, 50);
            if(isAttacking)
                spriteBatch.Draw(weapon, Center - new Vector2(0,18), weaponFrame, Color.White, 0f, weaponFrame.Size.ToVector2() / 2, 2f, spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            
            spriteBatch.Draw(texture, Center - new Vector2(0, 18), frame, Color.Lerp(Color.White, Color.Red, IFrameSine) * (1 - IFrameSine), 0f, frame.Size.ToVector2() / 2, 2f, spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);

            //Main.lighting.Maps.DrawToMap("Bloom", (SpriteBatch sb) => { sb.Draw(texture, Center - new Vector2(0, 18), frame, Color.White, 0f, frame.Size.ToVector2() / 2, 2f, spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f); });
        }
        public override void ApplyForces()
        {
            PlayerInputs();
        }
        private bool FreeFall;
        private bool isRecovering;
        private float VelYCache;
        public bool isAttacking;
        //temp
        Texture2D weapon => TextureCache.CrowBar;
        private void FindFrame()
        {
            if (onGround)
            {
                if(!isAttacking)
                {
                    if (FreeFall)
                    {
                        frameY = 0;
                        isRecovering = true;
                        VelYCache = oldVelocity.Y;
                    }
                    if (isRecovering)
                    {
                        if (VelYCache > GetEntityModifier<PlayerMovement>().velocityBeforeLandStun)
                        {
                            velocity.X *= 0.8f;
                            isRecovering = !Animate(3, 7, 48, 8, false);
                        }
                        else if (VelYCache > 0)
                        {
                            velocity.X *= 0.97f;
                            isRecovering = !Animate(5, 2, 48, 7, false);
                        }
                    }
                    else
                    {
                        if (GetEntityModifier<PlayerMovement>().varfriction != 0.99f)
                        {
                            Animate(6, 11, 48);
                        }
                        else
                        {
                            float vel = MathHelper.Clamp(Math.Abs(velocity.X), 1, 20);
                            int velFunc = Math.Max((int)(Math.Round(4 / Math.Abs(vel))), 3);
                            Animate(velFunc, 8, 48, 1);
                        }
                    }
                    FreeFall = false;
                }
            }
            else
            {
                if (!isAttacking)
                {
                    if (DeltaPos.Y <= GetEntityModifier<PlayerMovement>().velocityBeforeFallingAnimation)
                    {
                        Animate(1, 1, 48, 2, false);
                    }
                    else
                    {
                        if (!FreeFall)
                            FreeFall = Animate(2, 7, 48, 11, false, 1);
                        if (FreeFall)
                        {
                            Animate(4, 4, 48, 12);
                        }
                    }
                }
            }
        }
    }
}
