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

        public int life;
        public int maxLife;
        protected void Jump(float jumpheight, bool hasToBeOnGround = true)
        {
            if (hasToBeOnGround ? onGround : true)
            {
                velocity.Y -= jumpheight;
            }
        }
        public struct NPCInfo
        {
            public Texture2D icon;
            public Type type;
            public NPCInfo(Texture2D icon, Type type)
            {
                this.icon = icon;
                this.type = type;
            }
        }
        public static NPCInfo[] NPCTypes;
        public float percentLife => life / (float)maxLife;

        public bool hostile;

        public static void SpawnNPC(Vector2 position, Type type)
        {
            NPC NPC = Activator.CreateInstance(type) as NPC;
            NPC.SetDefaults();
            NPC.position = position;
        }

        public static void SpawnNPC<T>(Vector2 position) where T : NPC, new()
        {
            T NPC = new T();
            NPC.SetDefaults();
            NPC.position = position;
        }
        protected override void OnUpdate()
        {
            Constraints();
            if (Collides)
                TileCollisions();
        }

        protected virtual void SetDefaults()
        {

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

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Center, frame, Color.White, 0f, frame.Size.ToVector2() / 2, 1f, spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            DrawMethods.DrawRectangle(position + new Vector2(0, maxHeight - height), width, height, Color.Green);
        }

    }
}
