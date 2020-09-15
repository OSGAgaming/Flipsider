using System;
using System.Collections.Generic;
using System.Text;
using Flipsider.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flipsider.GUI.HUD
{
    class Hud : UIScreen
    {
        WeaponIcon leftIcon = new WeaponIcon();
        WeaponIcon rightIcon = new WeaponIcon();

        public Hud()
        {
            leftIcon.SetDimensions(10, (int)Main.ScreenSize.Y - 10, 48, 48);
            //leftIcon.weapon = Main.player.leftWeapon;
            elements.Add(leftIcon);

            rightIcon.SetDimensions(64, (int)Main.ScreenSize.Y - 10, 48, 48);
            //rightIcon.weapon = Main.player.rightWeapon;
            elements.Add(rightIcon);
        }
    }

    class WeaponIcon : UIElement
    {
        public Weapon weapon;

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureCache.hudSlot, dimensions, Color.White);
            weapon?.DrawInventory(spriteBatch, dimensions.Location.ToVector2());
        }
    }
}
