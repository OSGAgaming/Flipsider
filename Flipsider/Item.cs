
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        void SetInventoryIcon(Texture2D icon);
    }

    public class Item : IStoreable
    {
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
        public void SetInventoryIcon(Texture2D icon) =>  inventoryIcon = icon;

        public int maxStack;
    }
}
