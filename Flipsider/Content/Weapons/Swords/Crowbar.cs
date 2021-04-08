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
                if(comboState == 1 || comboState == 0)
                {
                  //  player.velocity.X += 6f;
                }
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
        public override void UpdateActive()
        {
            if (player.isAttacking)
            {
                int MouseDisp = Mouse.GetState().Position.X < Utils.ScreenSize.X/2 ? -1 : 1;
                switch (comboState)
                {
                    case 0:
                        player.isAttacking = !player.Animate(5, 6, 48, 4, false);
                        player.velocity.X += 0.001f * (delay - activeTimeLeft) * MouseDisp;
                        if (activeTimeLeft == delay - 10)
                        {
                            Camera.screenShake += 2;
                        }
                        break;
                    case 1:
                        player.isAttacking = !player.Animate(5, 6, 48, 5, false);
                        player.velocity.X += 0.002f * (delay - activeTimeLeft) * MouseDisp;
                        if (activeTimeLeft == delay - 10)
                        {
                            Camera.screenShake += 2;
                        }
                        break;
                    case 2:
                        player.isAttacking = !player.Animate(5, 11, 48, 6, false);
                        if (activeTimeLeft == delay - 30)
                        {
                            Camera.screenShake += 10;
                        }
                        if(activeTimeLeft >= delay - 30)
                        {
                            player.velocity.X += 0.015f * (delay - activeTimeLeft) * MouseDisp;
                        }
                        else
                        {
                            player.velocity.X *= 0.5f;
                        }
                        break;
                }
            }
            if(!player.isAttacking)
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
                spriteBatch.Draw(tex, source.AddPos(off), rect, Color.White, InvetoryRotation,Vector2.Zero, SpriteEffects.None,0f);
            }
        }
    }

    public class ExampleProj : Projectile
    {
        public static Texture2D icon = TextureCache.Blob;
        protected override void SetDefaults()
        {
            damage = 50;
            width = 20;
            hostile = false;
            framewidth = width;
            height = 20;
            texture = TextureCache.magicPixel;
            Collides = true;
            noGravity = true;
            gravity = 0.1f;
            friction = 0.98f;
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
