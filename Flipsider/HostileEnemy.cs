using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

using Flipsider.Engine.Input;
using Flipsider.Weapons;

namespace Flipsider
{
    public class NPC : Entity
    {

        public int life = 90;
        public int maxLife = 100;
        public float percentLife => life / (float)maxLife;


        public NPC(Vector2 position)
        {
            this.position = position;
        }
        
        protected internal override void Initialize()
        {
            Main.entities.Add(this);
        }
        protected override void OnUpdate()
        {
            ResetVars();
            Constraints();
        }

        void Constraints()
        {
            position.Y = MathHelper.Clamp(position.Y, -200, Main.ScreenSize.Y - maxHeight);
            position.X = MathHelper.Clamp(position.X, 0, 100000);
            if(Bottom >= Main.ScreenSize.Y)
            {
                onGround = true;
                velocity.Y = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Center, frame, Color.White, 0f, frame.Size.ToVector2()/2, 2f, spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            DrawMethods.DrawRectangle(position + new Vector2(0,maxHeight-height), width, height, Color.Green);
        }

    }
}
