﻿using Microsoft.Xna.Framework;
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
        protected virtual void Jump(float jumpheight, bool hasToBeOnGround = true)
        {
            if (hasToBeOnGround ? onGround : true)
                velocity.Y -= jumpheight;
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
        public static NPCInfo[] NPCTypes = new NPCInfo[0];
        public float percentLife => life / (float)maxLife;

        public bool hostile;

        public static void SpawnNPC(Vector2 position, Type type)
        {
            NPC NPC = Activator.CreateInstance(type) as NPC ?? throw new InvalidOperationException("Type wasn't an NPC");
            NPC.SetDefaults();
            NPC.position = position;
        }

        public static void SpawnNPC<T>(Vector2 position) where T : NPC, new()
        {
            T NPC = new T();
            NPC.SetDefaults();
            NPC.position = position;
        }
        protected virtual void SetDefaults()
        {

        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Center, frame, Color.White, 0f, frame.Size.ToVector2() / 2, 1f, spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            DrawMethods.DrawRectangle(position + new Vector2(0, maxHeight - height), width, height, Color.Green);
        }

    }
}