using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using static Flipsider.NPC;

namespace Flipsider.GUI.HealthManaGUI
{
    internal class HealthAndMana : UIScreen
    {
        public Vector2 LocalPosition => new Vector2(40, 10);
        protected override void OnLoad()
        {
            Health health = new Health()
            {
                dimensions = new Rectangle(new Point(100,10), TextureCache.HealthUI.Bounds.Size)
            };
            AddElement(health);

            Mana mana = new Mana()
            {
                dimensions = new Rectangle(new Point(100, 35), TextureCache.ManaUI.Bounds.Size)
            };
            AddElement(mana);

            WeaponPanel weaponPanel = new WeaponPanel()
            {
                dimensions = new Rectangle(new Point(20, 35), TextureCache.WeaponPanel.Bounds.Size)
            };
            AddElement(weaponPanel);
        }

        protected override void OnUpdate()
        {
        }
        protected override void OnDraw()
        {

        }
    }

    internal class Health : UIElement
    {
        float HealthPerc => Main.player.percentLife;

        float UpperHealthPerc => Math.Max(0, (Main.player.life - ExtraLife) / 50f);

        float LowerHealthPerc => Math.Min(1,Main.player.life / (float)ExtraLife);

        int ExtraLife => Main.player.maxLife - 50;

        float alpha;
        HealthAndMana? parent => Parent as HealthAndMana;
        public override void Draw(SpriteBatch spriteBatch)
        {
            int ExtraChains = (int)((ExtraLife / 2)* LowerHealthPerc);

            Point left = new Point(dimensions.X - 14, dimensions.Y - 5);
            Point right = new Point(dimensions.X + dimensions.Size.X + 5 + ExtraChains*2, dimensions.Y - 5);

            Point size = new Point((int)(TextureCache.HealthUI.Width * UpperHealthPerc), TextureCache.HealthUI.Height);
            Point pos = new Point(dimensions.X + ExtraChains * 2, dimensions.Y);
            for(int i = 0; i<ExtraChains; i++)
            {
                spriteBatch.Draw(TextureCache.HealthUIExtra, new Rectangle(new Point(dimensions.X + i*2, dimensions.Y), new Point(2,6)), Color.White);
            }

            spriteBatch.Draw(TextureCache.HealthUI, new Rectangle(pos, size), new Rectangle(Point.Zero, size), Color.White * alpha);
            spriteBatch.Draw(TextureCache.HealthUILeftPointer, new Rectangle(left, TextureCache.HealthUILeftPointer.Bounds.Size), Color.White * alpha);
            spriteBatch.Draw(TextureCache.HealthUIRightPointer, new Rectangle(right, TextureCache.HealthUIRightPointer.Bounds.Size), Color.White * alpha);

            Utils.DrawRectangle(new Rectangle(dimensions.Location, new Point(TextureCache.HealthUI.Width + ExtraChains*2, size.Y)).Inf(2, 2), Color.Maroon, 2f);
        }
        protected override void OnUpdate()
        {
            if (Main.player.TimeOutsideOfCombat == 0)
            {
                alpha -= alpha / 20f;
            }
            else
            {
                alpha = alpha.ReciprocateTo(1f, 20f);
            }

            dimensions.Y = (int)MathHelper.SmoothStep(-100, (parent?.LocalPosition.Y ?? 0), alpha);
        }
        protected override void OnLeftClick()
        {

        }
        protected override void OnHover()
        {

        }

        protected override void NotOnHover()
        {

        }
    }

    internal class Mana : UIElement
    {
        float ManaPerc;

        float alpha;

        HealthAndMana? parent => Parent as HealthAndMana;

        public override void Draw(SpriteBatch spriteBatch)
        {
            Point left = new Point(dimensions.X - 14, dimensions.Y - 5);
            Point right = new Point(dimensions.X + dimensions.Size.X + 5, dimensions.Y - 5);

            spriteBatch.Draw(TextureCache.ManaUI, dimensions, Color.White);
            spriteBatch.Draw(TextureCache.HealthUILeftPointer, new Rectangle(left, TextureCache.HealthUILeftPointer.Bounds.Size), Color.White * alpha);
            spriteBatch.Draw(TextureCache.HealthUIRightPointer, new Rectangle(right, TextureCache.HealthUIRightPointer.Bounds.Size), Color.White * alpha);

            Utils.DrawRectangle(dimensions.Inf(2,2),Color.Maroon,2f);
        }
        protected override void OnUpdate()
        {
            if (Main.player.TimeOutsideOfCombat == 0)
            {
                alpha -= alpha / 20f;
            }
            else
            {
                alpha = alpha.ReciprocateTo(1f, 20f);
            }
            dimensions.Y = (int)MathHelper.SmoothStep(-100, (parent?.LocalPosition.Y ?? 0) + 25, alpha);

        }
        protected override void OnLeftClick()
        {

        }
        protected override void OnHover()
        {

        }

        protected override void NotOnHover()
        {

        }
    }

    internal class WeaponPanel : UIElement
    {
        float alpha;
        HealthAndMana? parent => Parent as HealthAndMana;

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle secondaryPanel = dimensions.Inf(-5,-5).AddPos(new Point(30, 30));
            spriteBatch.Draw(TextureCache.WeaponPanel, dimensions.Inf(10,10), Color.White);
            spriteBatch.Draw(TextureCache.WeaponPanel, secondaryPanel, Color.White);
            Main.player.leftWeapon.DrawInventory(spriteBatch, dimensions);
            Main.player.rightWeapon.DrawInventorySecondary(spriteBatch, secondaryPanel.Inf(-4,-4));
        }
        protected override void OnUpdate()
        {
            if (Main.player.TimeOutsideOfCombat == 0)
            {
                alpha -= alpha / 16f;
            }
            else
            {
                alpha = alpha.ReciprocateTo(1f, 16f);
            }
            dimensions.Y = (int)MathHelper.SmoothStep(-100, (parent?.LocalPosition.Y ?? 0), alpha);
        }
        protected override void OnLeftClick()
        {

        }
        protected override void OnHover()
        {

        }

        protected override void NotOnHover()
        {

        }
    }

}
