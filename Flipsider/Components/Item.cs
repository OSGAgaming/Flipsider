
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Flipsider.IStoreable;
using static Flipsider.PropInteraction;
using static Flipsider.PropManager;

namespace Flipsider
{
    public interface IStoreable
    {
        public int MaxStack
        {
            get;
            set;
        }

        public Texture2D? inventoryIcon
        {
            get;
            set;
        }

        public struct ItemInfo
        {
            public string ToolTip;
            public Texture2D icon;
            public ItemInfo(Texture2D icon)
            {
                ToolTip = "insertTooltipHere";
                this.icon = icon;
            }
        }

        public ItemInfo itemInfo
        {
            get;
            set;
        }
        void SetInventoryIcon(Texture2D icon);
    }

    public abstract class Item : Entity, IStoreable
    {
        public static Type[] ItemTypes = new Type[0];
        public ItemInfo itemInfo
        {
            get;
            set;
        }
        public static string ToolTip = "";
        public int MaxStack
        {
            get => maxStack;
            set => maxStack = value;
        }
        public Texture2D? inventoryIcon
        {
            get;
            set;
        }
        public void SetInventoryIcon(Texture2D icon) => inventoryIcon = icon;

        public int maxStack;

        protected virtual void SetDefaults() {; }

    }
}
