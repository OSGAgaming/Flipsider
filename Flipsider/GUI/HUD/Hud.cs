using System;
using System.Collections.Generic;
using System.Text;
using Flipsider.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flipsider.GUI.HUD
{
    internal class Hud : UIScreen
    {
        private readonly WeaponIcon leftIcon = new WeaponIcon();
        private readonly WeaponIcon rightIcon = new WeaponIcon();

        public Hud()
        {
            Main.UIScreens.Add(this);
            leftIcon.SetDimensions(10, (int)Main.ScreenSize.Y - 64, 48, 48);
            //leftIcon.weapon = Main.player.leftWeapon;
            elements.Add(leftIcon);

            rightIcon.SetDimensions(64, (int)Main.ScreenSize.Y - 64, 48, 48);
            //rightIcon.weapon = Main.player.rightWeapon;
            elements.Add(rightIcon);
        }

        protected override void OnUpdate()
        {
            leftIcon.SetDimensions(10, (int)Main.ScreenSize.Y - 64, 48, 48);
            leftIcon.weapon = Main.player.leftWeapon;

            rightIcon.SetDimensions(64, (int)Main.ScreenSize.Y - 64, 48, 48);
            rightIcon.weapon = Main.player.rightWeapon;
        }
    }

    internal class WeaponIcon : UIElement
    {
        public Weapon? weapon;

        public override void Draw(SpriteBatch spriteBatch)
        {
            float progress = Main.player.swapTimer < 15 ? Main.player.swapTimer / 30f : 1 - Main.player.swapTimer / 30f;
            Vector2 off = Vector2.SmoothStep(new Vector2(0, 0), new Vector2(8, -8), progress);
            Vector2 off2 = Vector2.SmoothStep(new Vector2(8, -8), new Vector2(0, 0), progress);

            Rectangle target = new Rectangle(dimensions.X + (int)off.X, dimensions.Y + (int)off.Y, 48, 48);
            Rectangle target2 = new Rectangle(dimensions.X + (int)off2.X, dimensions.Y + (int)off2.Y, 48, 48);

            Color color = Color.Lerp(Color.White, Color.Gray, progress);
            Color color2 = Color.Lerp(Color.Gray, Color.White, progress);

            spriteBatch.Draw(TextureCache.hudSlot, target2, color2);
            spriteBatch.Draw(TextureCache.hudSlot, target, color);

            weapon?.DrawInventory(spriteBatch, dimensions.Location.ToVector2() + off);
        }
    }

    internal class ResourceBar : UIElement
    {
        public float amount;

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle target = new Rectangle(dimensions.X + 2, dimensions.Y + 2, (int)((dimensions.Width - 4) * amount), dimensions.Height - 4);

            Color color = Color.Lerp(Color.Red, Color.LimeGreen, amount);

            spriteBatch.Draw(TextureCache.magicPixel, dimensions, Color.Black);
            spriteBatch.Draw(TextureCache.magicPixel, target, color);

            var msg = (int)(amount * 100) + "%";
            spriteBatch.DrawString(Main.font, msg, dimensions.Center.ToVector2(), Color.White, 0, Main.font.MeasureString(msg) * 0.5f, 0.5f, 0, 0);
        }
    }
}
