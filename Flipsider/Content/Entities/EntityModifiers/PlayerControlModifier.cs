
using Flipsider.Engine.Input;
using Flipsider.Engine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Flipsider.Engine.Maths
{
    public class PlayerMovement : IEntityModifier
    {
        Player player;
        public int TigerTimer;
        public float varfriction;
        public Vector2 varAirResistance;

        //VARIABLES YOU WANT TO CHANGE ------------------------------ //    //Triv and OS smell
        private readonly int TigerTime = 5;
        private readonly float jumpheight = 3.5f;
        public readonly float friction = 0.85f;
        public readonly float acceleration = 0.085f;
        public readonly Vector2 airResistance = new Vector2(0.985f, 0.999f);
        public readonly float maxSpeedX = 20f;
        public readonly float maxSpeedY = 20f;
        public readonly float velocityBeforeFallingAnimation = 2f;
        public readonly float velocityBeforeLandStun = 4f;
        public readonly float jumpBoost = 1f;
        //VARIABLES YOU WANT TO CHANGE ------------------------------ //

        public void JumpMechanic()
        {
            KeyboardState state = Keyboard.GetState();

            if ((state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Space)) && !player.crouching)
            {
                if (TigerTimer < TigerTime && !player.isAttacking)
                {
                    player.velocity.Y -= jumpheight;
                    float AbsVel = jumpBoost;
                    float Dir = 0;
                    if (state.IsKeyDown(Keys.D)) Dir = 1;
                    if (state.IsKeyDown(Keys.A)) Dir = -1;
                    player.velocity.X += AbsVel * Dir * jumpBoost;
                    TigerTimer = TigerTime + 1;
                }
            }
        }

        public void MoveLeftOrRight()
        {
            if (!(player.crouching && player.onGround) && !player.isAttacking)
            {
                if (GameInput.Instance["MoveRight"].IsDown())
                {
                    float amount = GameInput.Instance["MoveRight"].GetPressValue();
                    player.velocity.X += acceleration * amount;
                    varfriction = 0.99f;
                }

                if (GameInput.Instance["MoveLeft"].IsDown())
                {
                    float amount = GameInput.Instance["MoveLeft"].GetPressValue();
                    player.velocity.X -= acceleration * amount;
                    varfriction = 0.99f;
                }
            }
        }
        public void Update(in Entity entity)
        {
            if (!player.onGround)
            {
                TigerTimer++;
            }
            else
            {
                TigerTimer = 0;
            }

            if (player.IFrames == 0)
            {
                if (player.velocity.X >= acceleration)
                {
                    player.spriteDirection = 1;
                }
                else if (player.velocity.X <= -acceleration)
                {
                    player.spriteDirection = -1;
                }
            }

            varfriction = friction;

            if (player.crouching) varAirResistance.X = 0.99f;
            else if (!player.onGround) varAirResistance.X = 0.97f;
            else varAirResistance = airResistance;

            JumpMechanic();
        
            MoveLeftOrRight();

            if (player.onGround)
            {
                player.velocity.X *= varfriction;
            }

            if (!player.noAirResistance)
                player.velocity *= varAirResistance;

            player.velocity.X = Math.Clamp(player.velocity.X, -maxSpeedX, maxSpeedX);
            player.velocity.Y = Math.Clamp(player.velocity.Y, -maxSpeedY, maxSpeedY);
        }

        public PlayerMovement(Player player)
        {
            this.player = player;
        }
    }
}

