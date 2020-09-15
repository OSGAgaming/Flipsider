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
        WeaponIcon leftIcon;
        WeaponIcon rightIcon;

        public Hud()
        {
            leftIcon.SetDimensions(10, Main.ScreenSize.Y - 10, 24, 24);
            leftIcon.weapon = Main.player.leftWeapon;
            elements.Add(leftIcon);

            rightIcon.SetDimensions(38, Main.ScreenSize.Y - 10, 24, 24);
            rightIcon.weapon = Main.player.rightWeapon;
            elements.Add(rightIcon);
        }
    }

    class WeaponIcon : UIElement
    {
        public Weapon weapon { get; set; }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureCache.hudSlot, dimensions, Color.White);
            weapon.DrawInventory(spriteBatch, dimensions.Location.ToVector2());
        }
    }
}
