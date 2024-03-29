﻿using FlipEngine;
using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Flipsider.Weapons
{
    internal class Crowbar : Sword
    {
        public override Texture2D swordSheet => TextureCache.GreenSlime;

        public Crowbar() : base(5, 108, 2)
        {
            SetInventoryIcon(Textures._Weapons_Crowbar_Icon);
        }
        Player player => Main.player;
        protected override void OnActivation()
        {
            if (player.onGround)
            {
                player.isAttacking = true;
                player.frameY = 0;
                ComboLag = 20;
            }
        }
        public override void UpdatePassive()
        {
            base.UpdatePassive();
            if (!player.isAttacking)
            {
                InvetoryRotation = InvetoryRotation.ReciprocateTo(0f);
                off = off.ReciprocateTo(Vector2.Zero);
            }
        }

        protected override void ConfigureHitbox()
        {
            Vector2 T = Vector2.Zero;

            int UpTime = delay - activeTimeLeft;
            int d = Main.player.spriteDirection;

            int Shake = 2;
            int Damage = damage;
            float KnockBack = 1.2f;

            /*switch (comboState)
            {
                case 0:
                T = Utils.TraverseBezier(
                player.position + new Vector2(15 * d, 30) + new Vector2(15, 0) * d,
                player.position + new Vector2(30 * d, 20) + new Vector2(15, 0) * d,
                player.position + new Vector2(0, 0) + new Vector2(15, 0) * d,
                UpTime / (float)(delay - 90));
                    break;
                case 1:
                    T = Utils.TraverseBezier(
                player.position + new Vector2(10 * d, -20) + new Vector2(25, 0) * d,
                player.position + new Vector2(20 * d, 10) + new Vector2(25, 0) * d,
                player.position + new Vector2(10 * d, 24) + new Vector2(25, 0) * d,
                UpTime / (float)(delay - 90));
                    break;
                case 2:
                    if(activeTimeLeft >= delay - 30)
                    T = Utils.TraverseBezier(
               player.position + new Vector2(-80 * d, -40) + new Vector2(45, 0) * d,
               player.position + new Vector2(30 * d, 40) + new Vector2(45, 0) * d,
               player.position + new Vector2(-80 * d, 17) + new Vector2(45, 0) * d,
               UpTime / (float)(delay - 80));
                    Damage *= 2;
                    Shake *= 3;
                    KnockBack *= 2;
                    break;
            }*/
            Damage = 0;

            switch (comboState)
            {
                case 0:
                    T = Utils.TraverseBezier(
                    player.Center + new Vector2(15 * d, 30) + new Vector2(15 * d, -20),
                    player.Center + new Vector2(30 * d, 20) + new Vector2(15 * d, -20),
                    player.Center + new Vector2(0, 0) + new Vector2(15 * d, -20),
                    UpTime / (float)(delay - 90));
                    if (activeTimeLeft == delay - 10)
                    {
                        Damage = 5;
                    }
                    break;
                case 1:
                    T = Utils.TraverseBezier(
                player.Center + new Vector2(10 * d, -20) + new Vector2(25 * d, -30),
                player.Center + new Vector2(20 * d, 10) + new Vector2(25 * d, -30),
                player.Center + new Vector2(10 * d, 24) + new Vector2(25 * d, -30),
                UpTime / (float)(delay - 90));
                    if (activeTimeLeft == delay - 10)
                    {
                        Damage = 7;
                    }
                    break;
                case 2:
                    if (activeTimeLeft >= delay - 30)
                        T = Utils.TraverseBezier(
                   player.Center + new Vector2(-80 * d, -40) + new Vector2(55 * d, -20) ,
                   player.Center + new Vector2(30 * d, 40) + new Vector2(55 * d, -20),
                   player.Center + new Vector2(-80 * d, 17) + new Vector2(55 * d, -20),
                   UpTime / (float)(delay - 80));
                    Shake *= 3;
                    KnockBack *= 3;
                    if (activeTimeLeft == delay - 25)
                    {
                        Damage = 10;
                    }
                    break;
            }
            
           Main.player.GetEntityModifier<HitBox>().GenerateHitbox(
                new Rectangle(T.ToPoint().Add(new Point(-40,-40)), new Point(80, 80)),
                false,
                (HitBox) => {
                    var npc = (HitBox.LE as NPC);
                    npc?.TakeDamage(Damage);
                    if (npc != null)
                    {
                        if (npc.IFrames == NPC.GlobalIFrames)
                        {
                            npc.velocity = -Vector2.Normalize(player.Center - npc.Center) * KnockBack;
                        }
                    }
                });
        }
        public override void UpdateActive()
        {
            if (player.isAttacking)
            {
                int MouseDisp = (Mouse.GetState().Position.X) < Utils.ActualScreenSize.X / 2 ? -1 : 1;
                switch (comboState)
                {
                    case 0:
                        player.isAttacking = !player.Animate(5, 6, 48, 4, false);
                        player.velocity.X += 0.004f * (delay - activeTimeLeft) * MouseDisp;
                        if (activeTimeLeft == delay - 10)
                        {
                            Main.Gamecamera.ScreenShake += 2;
                        }
                        break;
                    case 1:
                        player.isAttacking = !player.Animate(5, 6, 48, 5, false);
                        player.velocity.X += 0.006f * (delay - activeTimeLeft) * MouseDisp;
                        if (activeTimeLeft == delay - 10)
                        {
                             Main.Gamecamera.ScreenShake += 2;
                        }
                        break;
                    case 2:
                        player.isAttacking = !player.Animate(5, 11, 48, 6, false);
                        if (activeTimeLeft == delay - 30)
                        {
                            Main.Gamecamera.ScreenShake += 4;
                        }
                        if (activeTimeLeft >= delay - 30)
                        {
                            player.velocity.X += 0.008f * (delay - activeTimeLeft) * MouseDisp;
                        }
                        else
                        {
                            player.velocity.X *= 0.5f;
                        }
                        break;
                }

                
            }
            if (!player.isAttacking)
            {
                activeTimeLeft = 0;
            }
        }
        public int ComboLag;

        public float InvetoryRotation;
        public override void Update()
        {
            if (ComboLag > 0 && !player.isAttacking) ComboLag--;

            if (ComboLag == 0) comboState = 2;

        }

        Vector2 off;
        public override void DrawInventory(SpriteBatch spriteBatch, Rectangle source)
        {
            if (Main.player.leftWeapon.inventoryIcon != null)
            {
                Texture2D tex = Main.player.leftWeapon.inventoryIcon;
                Rectangle rect = tex.Bounds;
                spriteBatch.Draw(tex, source.AddPos(off), rect, Color.White, InvetoryRotation, Vector2.Zero, SpriteEffects.None, 0f);
            }
        }

        public override void DrawInventorySecondary(SpriteBatch spriteBatch, Rectangle source)
        {
            if (Main.player.leftWeapon.inventoryIcon != null)
            {
                Texture2D tex = Main.player.leftWeapon.inventoryIcon;
                Rectangle rect = tex.Bounds;
                spriteBatch.Draw(tex, source.AddPos(off), rect, Color.White, InvetoryRotation, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            }
        }
    }

    public class ExampleProj : Projectile
    {
        public static Texture2D icon = TextureCache.Blob;
        protected override void SetDefaults()
        {
            damage = 50;
            Width = 20;
            hostile = false;
            framewidth = Width;
            Height = 20;
            Texture = TextureCache.magicPixel;
            noGravity = true;
            noAirResistance = true;
            TileCollide = true;
        }

        protected override void OnAI()
        {
            Constraints();
            Animate(5, 1, 20, 0);
        }
    }
}
