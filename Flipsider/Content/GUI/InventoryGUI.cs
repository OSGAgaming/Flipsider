using FlipEngine;
using Flipsider.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using static FlipEngine.Item;

namespace Flipsider.GUI
{
    internal class InventoryGUI : UIScreen
    {
        private InventoryPanel[]? tilePanel;
        private ItemPanel[]? itemPanel;
        private readonly int rows = 5;
        private readonly int widthOfPanel = 64;
        private readonly int heightOfPanel = 64;
        private readonly int paddingX = 5;
        private readonly int paddingY = 20;
        public int chosen = -1;
        private Vector2 topRight = new Vector2(Main.ActualScreenSize.X, 0);
        public int lastInvSlot;
        protected override void OnLoad()
        {
            itemPanel = new ItemPanel[ItemTypes.Length];
            if (itemPanel.Length != 0)
            {
                for (int i = 0; i < itemPanel.Length; i++)
                {
                    Vector2 panelPoint = new Vector2(Main.ActualScreenSize.X / 3.5f + widthOfPanel + (i % rows) * (widthOfPanel + paddingX) - paddingX, Main.ActualScreenSize.Y + topRight.Y + paddingY + (i / rows) * heightOfPanel);
                    itemPanel[i] = new ItemPanel();
                    itemPanel[i].SetDimensions((int)panelPoint.X, (int)panelPoint.Y, widthOfPanel, heightOfPanel);
                    itemPanel[i].startingDimensions = new Rectangle((int)panelPoint.X, (int)panelPoint.Y, widthOfPanel, heightOfPanel);
                    itemPanel[i].item = Activator.CreateInstance(ItemTypes[i]) as IStoreable;
                    elements.Add(itemPanel[i]);
                }
            }


            tilePanel = new InventoryPanel[Main.player.inventorySize];
            if (tilePanel.Length != 0)
            {
                for (int i = 0; i < tilePanel.Length; i++)
                {
                    Vector2 panelPoint = new Vector2(topRight.X - widthOfPanel - (i % rows) * (widthOfPanel + paddingX) - paddingX, topRight.Y + paddingY + (i / rows) * heightOfPanel);
                    tilePanel[i] = new InventoryPanel();
                    tilePanel[i].SetDimensions((int)panelPoint.X, (int)panelPoint.Y, widthOfPanel, heightOfPanel);
                    tilePanel[i].startingDimensions = new Rectangle((int)panelPoint.X, (int)panelPoint.Y, widthOfPanel, heightOfPanel);
                    tilePanel[i].inventorySlot = i;
                    elements.Add(tilePanel[i]);
                }
            }
        }
        protected override void OnUpdate()
        {
            for (int i = 0; i < tilePanel?.Length; i++)
            {
                tilePanel[i].item = Main.player.inventory[i];
            }
        }
        protected override void OnDraw()
        {

        }
    }

    internal class ItemPanel : UIElement
    {
        public IStoreable? item;
        private float lerpage = 0;
        public Rectangle startingDimensions;
        private readonly bool chosen;
        public int goToPoint = (int)Main.ActualScreenSize.X - 140;
        public float alpha = 0;
        private float progression = 0;
        public bool active = true;
        private Texture2D? tex;
        public override void Draw(SpriteBatch spriteBatch)
        {

            tex ??= item?.inventoryIcon;

            int fluff = 1;
            Rectangle panelDims = new Rectangle(dimensions.X - fluff, dimensions.Y - fluff, dimensions.Width + fluff * 2, dimensions.Height + fluff * 2);
            spriteBatch.Draw(TextureCache.NPCPanel, panelDims, Color.Lerp(Color.White, Color.Black, lerpage) * alpha);
            spriteBatch.Draw(tex, dimensions, Color.Lerp(Color.White, Color.Black, lerpage) * alpha);
        }
        protected override void OnUpdate()
        {
            if (delay > 0)
                delay--;
            if (chosen)
            {
                progression += (1 - progression) / 16f;
            }
            else
            {
                progression -= progression / 16f;

            }
            if (Main.Editor.CurrentState == EditorUIState.Inventory)
            {
                dimensions.Y += (int)(Main.ActualScreenSize.Y - 90 - dimensions.Y) / 32;
                alpha += (1 - alpha) / 16f;
            }
            else
            {
                dimensions.Y += (startingDimensions.Y - dimensions.Y) / 32;
                alpha -= alpha / 16f;
            }
        }

        private int delay;
        protected override void OnLeftClick()
        {
            if (Main.Editor.CurrentState == EditorUIState.Inventory)
            {
                if (delay == 0)
                {
                    Main.player.SelectedItem = item;
                    delay = 20;
                }
            }
        }
        protected override void OnHover()
        {
            if (active)
            {
                lerpage += (0.5f - lerpage) / 16f;
            }
        }

        protected override void NotOnHover()
        {
            lerpage -= lerpage / 16f;
        }
    }

    internal class InventoryPanel : UIElement
    {
        public IStoreable? item;
        private float lerpage = 0;
        public Rectangle startingDimensions;
        private readonly bool chosen;
        private int delay;
        public int goToPoint = (int)Main.ActualScreenSize.X - 140;
        public float alpha = 0;
        private float progression = 0;
        public bool active = true;
        public int inventorySlot;

        private bool hasItem => Main.player.inventory[inventorySlot] != null;

        private bool isWeapon => item?.GetType().IsSubclassOf(typeof(Weapon)) ?? false;

        private string GetStats()
        {
            if (item != null && isWeapon)
            {
                return "Damage: " + ((Weapon)item).damage.ToString();
            }
            return "";
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            int fluff = 1;
            Rectangle panelDims = new Rectangle(dimensions.X - fluff, dimensions.Y - fluff, dimensions.Width + fluff * 2, dimensions.Height + fluff * 2);
            spriteBatch.Draw(TextureCache.NPCPanel, panelDims, Color.Lerp(Color.White, Color.Black, lerpage) * alpha);
            if (item != null)
            {
                spriteBatch.Draw(item.inventoryIcon, dimensions, Color.Lerp(Color.White, Color.Black, lerpage) * alpha);
                Utils.DrawTextToLeft(GetStats(), Color.White * lerpage * 2, Mouse.GetState().Position.ToVector2() + new Vector2(20, 0));
            }
        }
        protected override void OnUpdate()
        {
            if (delay > 0)
                delay--;
            if (chosen)
            {
                progression += (1 - progression) / 16f;
            }
            else
            {
                progression -= progression / 16f;

            }
            if (Main.Editor.CurrentState == EditorUIState.Inventory)
            {
                alpha += (1 - alpha) / 16f;
            }
            else
            {
                alpha -= alpha / 16f;
            }
        }
        protected override void OnLeftClick()
        {
            if (Main.Editor.CurrentState == EditorUIState.Inventory)
            {
                if (hasItem)
                {
                    if (delay == 0)
                    {
                        if (Main.player.SelectedItem != null)
                            Main.player.AddToInventory(Main.player.SelectedItem, inventorySlot);
                        Main.player.SelectedItem = item;
                        delay = 20;
                    }
                }
                else
                {
                    if (delay == 0)
                    {
                        if (Main.player.SelectedItem != null)
                            Main.player.AddToInventory(Main.player.SelectedItem, inventorySlot);
                        Main.player.SelectedItem = null;
                        delay = 20;
                    }
                }
            }
        }
        protected override void OnHover()
        {
            if (active)
            {
                lerpage += (0.5f - lerpage) / 32f;
            }
        }

        protected override void NotOnHover()
        {
            lerpage -= lerpage / 5f;
        }
    }

}
